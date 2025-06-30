using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitTfs.Util;
using LibGit2Sharp;
using StructureMap;
using System.Diagnostics;

namespace GitTfs.Core
{
    [StructureMapSingleton]
    public class GitReferenceRepository : IDisposable
    {
        private readonly IContainer _container;
        private readonly Globals _globals;
        private Repository _repository;
        private readonly GitTfsChangesetXmlSerializer _xmlSerialier;
        private string GitDir;
        public bool Parsed { get; private set; }

        public GitReferenceRepository(IContainer container, Globals globals)
        {
            _container = container;
            _globals = globals;
            _xmlSerialier = _container.GetInstance<GitTfsChangesetXmlSerializer>();
        }

        ~GitReferenceRepository()
        {
            Dispose();
        }

        public bool Parse(string gitDir)
        {
            this.GitDir = gitDir;
            if (!string.IsNullOrEmpty(this.GitDir))
            {
                Parsed = _xmlSerialier.ParseChangesets(this.GitDir);
            }
            else
            {
                Parsed = false;
            }
            if (Parsed)
            {
                _repository?.Dispose();
                _repository = new Repository(GitDir);
            }
            return Parsed;
        }

        public void Dispose()
        {
            _repository?.Dispose();
        }

        public LibGit2Sharp.Tree GetTreeInfo(int changeset, string branchRefName)
        {
            if (this._xmlSerialier.TryGetValue(changeset, branchRefName, out string sha))
            {
                return GetTreeInfo(sha);
            }
            return null;
        }

        public LibGit2Sharp.Tree GetTreeInfo(string sha)
        {
            LibGit2Sharp.GitObject gitObject = _repository.Lookup(sha);
            if (gitObject is LibGit2Sharp.Commit commit)
            {
                return commit.Tree;
            }
            return null;
        }

        public bool TryWriteFile(LibGit2Sharp.Tree tree, string gitPath, string fsPath, HashSet<string> createdDirectoryCacheSet)
        {
            if (tree != null)
            {
                LibGit2Sharp.TreeEntry treeEntry = tree[gitPath];
                if (treeEntry?.Target is LibGit2Sharp.Blob blob)
                {
                    string directoryName = System.IO.Path.GetDirectoryName(fsPath);
                    if (createdDirectoryCacheSet.Add(directoryName))
                    {
                        System.IO.Directory.CreateDirectory(directoryName);
                    }
                    using (Stream stream = blob.GetContentStream())
                    using (FileStream fileStream = new FileStream(fsPath, FileMode.Create))
                    {
                        stream.CopyTo(fileStream);
                        fileStream.Flush();
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
