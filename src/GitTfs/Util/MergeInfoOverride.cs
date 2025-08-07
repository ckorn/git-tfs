using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GitTfs.Util
{
    [StructureMapSingleton]
    public class MergeInfoOverride
    {
        private const string GitTfsOverriddenMergeInfoFileName = "tfsMergeInfoOverride.xml";
        private string GetCacheFilePath(string gitDir) => Path.Combine(gitDir, GitTfsOverriddenMergeInfoFileName);
        private List<MergeInfoOverrideEntry> mergeInfoOverrideList = null;
        public bool TryGetValue(string branchToCreate, out string overriddenParentBranch)
        {
            overriddenParentBranch = string.Empty;
            if (this.mergeInfoOverrideList == null)
            {
                return false;
            }
            MergeInfoOverrideEntry entry = this.mergeInfoOverrideList.SingleOrDefault(x => x.TfsPathBranchToCreate == branchToCreate);
            if (entry == null)
            {
                return false;
            }
            overriddenParentBranch = entry.TfsPathParentBranch;
            return true;
        }

        public void Parse(string gitDir)
        {
            string cachePath = GetCacheFilePath(gitDir);
            if (!File.Exists(cachePath))
            {
                return;
            }
            Trace.WriteLine("Reading " + GitTfsOverriddenMergeInfoFileName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<MergeInfoOverrideEntry>));
            using (var stream = System.IO.File.OpenRead(cachePath))
            {
                this.mergeInfoOverrideList = xmlSerializer.Deserialize(stream) as List<MergeInfoOverrideEntry>;
            }
        }

        public class MergeInfoOverrideEntry
        {
            public string TfsPathBranchToCreate { get; set; }
            public string TfsPathParentBranch { get; set; }
        }
    }
}
