using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitTfs.Util
{
    [StructureMapSingleton]
    public class GitTfsChangesetRepository
    {
        private readonly GitTfsChangesetXmlSerializer xmlSerializer = new GitTfsChangesetXmlSerializer();

        public bool TryGetValue(int changeset, string branch, out string commitId)
            => xmlSerializer.TryGetValue(changeset, branch, out commitId);

        public void Parse(string gitDir)
        {
            xmlSerializer.ParseChangesets(gitDir);
            xmlSerializer.ParseChangesetChanges(gitDir);
        }

        public void AddAndSave(int changeset, string branch, string commitId, IEnumerable<string> parentCommitIdList)
            => xmlSerializer.AddAndSave(changeset, branch, commitId, parentCommitIdList);

        public void AddAndSaveChanges(int changeset, string branch, int updates, int deletes, int ignores)
            => xmlSerializer.AddAndSaveChanges(changeset, branch, updates, deletes, ignores);
    }
}
