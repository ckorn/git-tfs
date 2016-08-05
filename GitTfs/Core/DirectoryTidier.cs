﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sep.Git.Tfs.Core.TfsInterop;

namespace Sep.Git.Tfs.Core
{
    public class DirectoryTidier : ITfsWorkspaceModifier, IDisposable
    {
        enum FileOperation
        {
            Add,
            Remove,
            RenameFrom,
            RenameTo,
            Edit,
            EditAndRenameFrom,
        }

        ITfsWorkspaceModifier _workspace;
        Func<IEnumerable<TfsTreeEntry>> _getInitialTfsTree;
        List<string> _filesInTfs;
        Dictionary<string, FileOperation> _fileOperations;
        bool _disposed;

        public DirectoryTidier(ITfsWorkspaceModifier workspace, Func<IEnumerable<TfsTreeEntry>> getInitialTfsTree)
        {
            _workspace = workspace;
            _getInitialTfsTree = getInitialTfsTree;
            _fileOperations = new Dictionary<string, FileOperation>(StringComparer.InvariantCultureIgnoreCase);
        }

        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;

            var candidateDirectories = CalculateCandidateDirectories();
            if (!candidateDirectories.Any())
                return;

            _filesInTfs = _getInitialTfsTree().Where(entry => entry.Item.ItemType == TfsItemType.File).Select(entry => entry.FullName.ToLowerInvariant()).ToList();

            foreach (var fileAndOperation in _fileOperations)
            {
                if (fileAndOperation.Value == FileOperation.Remove)
                    _filesInTfs.Remove(fileAndOperation.Key.ToLowerInvariant());
            }

            var deletedDirs = new List<string>();
            foreach (var dir in candidateDirectories.OrderBy(d => d, StringComparer.InvariantCultureIgnoreCase))
            {
                DeleteEmptyDir(dir, deletedDirs);
            }
        }

        void DeleteEmptyDir(string dirName, List<string> deletedDirs)
        {
            if (dirName == null)
                return;
            var downcasedDirName = dirName.ToLowerInvariant();
            if (!HasEntryInDir(downcasedDirName))
            {
                DeleteEmptyDir(GetDirectoryName(dirName), deletedDirs);
                if (!IsDirDeletedAlready(downcasedDirName, deletedDirs))
                {
                    _workspace.Delete(dirName);
                    deletedDirs.Add(downcasedDirName);
                }
            }
        }

        bool IsDirDeletedAlready(string downcasedDirName, IEnumerable<string> deletedDirs)
        {
            return deletedDirs.Any(t => downcasedDirName.StartsWith(t + "/") || t == downcasedDirName);
        }

        string GetDirectoryName(string path)
        {
            var separatorIndex = path.LastIndexOf('/');
            if (separatorIndex == -1)
                return null;
            return path.Substring(0, separatorIndex);
        }

        bool HasEntryInDir(string dirName)
        {
            dirName = dirName + "/";
            return _filesInTfs.Any(file => file.StartsWith(dirName));
        }

        string ITfsWorkspaceModifier.GetLocalPath(string path)
        {
            return _workspace.GetLocalPath(path);
        }

        void ITfsWorkspaceModifier.Add(string path)
        {
            _workspace.Add(path);
            _fileOperations.Add(path, FileOperation.Add);
        }

        void ITfsWorkspaceModifier.Edit(string path)
        {
            _workspace.Edit(path);
            _fileOperations.Add(path, FileOperation.Edit);
        }

        void ITfsWorkspaceModifier.Delete(string path)
        {
            _workspace.Delete(path);
            _fileOperations.Add(path, FileOperation.Remove);
        }

        void ITfsWorkspaceModifier.Rename(string pathFrom, string pathTo, string score)
        {
            _workspace.Rename(pathFrom, pathTo, score);

            FileOperation pathFromOperation;
            if (_fileOperations.TryGetValue(pathFrom, out pathFromOperation) &&
                pathFromOperation == FileOperation.Edit)
            {
                _fileOperations[pathFrom] = FileOperation.EditAndRenameFrom;
            }
            else
            {
                _fileOperations.Add(pathFrom, FileOperation.RenameFrom);
            }
            _fileOperations.Add(pathTo, FileOperation.RenameTo);
        }

        IEnumerable<string> CalculateCandidateDirectories()
        {
            var directoriesWithRemovedFiles = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
            var directoriesBlockedForRemoval = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var fileAndOperation in _fileOperations)
            {
                var directory = GetDirectoryName(fileAndOperation.Key);
                switch (fileAndOperation.Value)
                {
                    case FileOperation.Remove:
                        directoriesWithRemovedFiles.Add(directory);
                        break;
                    default:
                        directoriesBlockedForRemoval.Add(directory);
                        break;
                }
            }

            directoriesBlockedForRemoval.Add(null);
            directoriesWithRemovedFiles.ExceptWith(directoriesBlockedForRemoval);
            return directoriesWithRemovedFiles;
        }
    }
}
