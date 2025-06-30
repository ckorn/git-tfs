using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GitTfs.Util
{
    public class GitTfsChangesetXmlSerializer
    {
        private readonly Dictionary<(int changeset, string branch), GitTfsChangeset> cache = new Dictionary<(int changeset, string branch), GitTfsChangeset>();
        private readonly Dictionary<(int changeset, string branch), TfsChangesetChanges> changesCache = new Dictionary<(int changeset, string branch), TfsChangesetChanges>();
        private const string GitTfsChangesetsFileName = "tfsChangesets.xml";
        private const string GitTfsChangesetChangesFileName = "tfsChangesetChanges.xml";
        private string gitDir = string.Empty;

        private string GetXmlFilePath(string gitDir) => Path.Combine(gitDir, GitTfsChangesetsFileName);
        private string GetXmlChangesFilePath(string gitDir) => Path.Combine(gitDir, GitTfsChangesetChangesFileName);

        public bool ParseChangesets(string gitDir)
        {
            this.gitDir = gitDir;
            string xmlPath = GetXmlFilePath(gitDir);
            if (!File.Exists(xmlPath))
            {
                return false;
            }
            Trace.WriteLine("Reading " + GitTfsChangesetsFileName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<GitTfsChangeset>));
            using (var stream = System.IO.File.OpenRead(xmlPath))
            {
                List<GitTfsChangeset> gitTfsChangesetList = xmlSerializer.Deserialize(stream) as List<GitTfsChangeset>;
                foreach (GitTfsChangeset entry in gitTfsChangesetList)
                {
                    this.cache[(entry.Changeset, entry.Branch)] = entry;
                }
            }
            return true;
        }

        public void ParseChangesetChanges(string gitDir)
        {
            this.gitDir = gitDir;
            string xmlPath = GetXmlChangesFilePath(gitDir);
            if (!File.Exists(xmlPath))
            {
                return;
            }
            Trace.WriteLine("Reading " + GitTfsChangesetChangesFileName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<TfsChangesetChanges>));
            using (var stream = System.IO.File.OpenRead(xmlPath))
            {
                List<TfsChangesetChanges> tfsChangesetChangesList = xmlSerializer.Deserialize(stream) as List<TfsChangesetChanges>;
                foreach (TfsChangesetChanges entry in tfsChangesetChangesList)
                {
                    this.changesCache[(entry.Changeset, entry.Branch)] = entry;
                }
            }
        }

        public bool TryGetValue(int changeset, string branch, out string commitId)
        {
            if (this.cache.TryGetValue((changeset, branch), out GitTfsChangeset entry))
            {
                commitId = entry.CommitId;
                return true;
            }
            commitId = null;
            return false;
        }

        public void AddAndSave(int changeset, string branch, string commitId, IEnumerable<string> parentCommitIdList)
        {
            GitTfsChangeset entry = new GitTfsChangeset()
            {
                Changeset = changeset,
                Branch = branch,
                CommitId = commitId,
                ParentCommitIdList = parentCommitIdList.ToList()
            };
            this.cache[(entry.Changeset, entry.Branch)] = entry;
            if (string.IsNullOrEmpty(this.gitDir))
            {
                throw new InvalidOperationException("gitDir not set");
            }
            string xmlPath = GetXmlFilePath(gitDir);
            using (var writer = new System.IO.StreamWriter(xmlPath))
            {
                var serializer = new XmlSerializer(typeof(List<GitTfsChangeset>));
                serializer.Serialize(writer, this.cache.Values.ToList());
                writer.Flush();
            }
        }

        public void AddAndSaveChanges(int changeset, string branch, int updates, int deletes, int ignores)
        {
            TfsChangesetChanges entry = new TfsChangesetChanges()
            {
                Changeset = changeset,
                Branch = branch,
                Updates = updates,
                Deletes = deletes,
                Ignores = ignores
            };
            this.changesCache[(entry.Changeset, entry.Branch)] = entry;
            if (string.IsNullOrEmpty(this.gitDir))
            {
                throw new InvalidOperationException("gitDir not set");
            }
            string xmlPath = GetXmlChangesFilePath(gitDir);
            using (var writer = new System.IO.StreamWriter(xmlPath))
            {
                var serializer = new XmlSerializer(typeof(List<TfsChangesetChanges>));
                serializer.Serialize(writer, this.changesCache.Values.ToList());
                writer.Flush();
            }
        }

        public class GitTfsChangeset
        {
            public int Changeset { get; set; }
            public string Branch { get; set; }
            public string CommitId { get; set; }
            public List<string> ParentCommitIdList { get; set; }
        }

        public class TfsChangesetChanges
        {
            public int Changeset { get; set; }
            public string Branch { get; set; }
            public int Deletes { get; set; }
            public int Updates { get; set; }
            public int Ignores { get; set; }
        }
    }
}
