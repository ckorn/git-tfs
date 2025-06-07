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
    public class MergeInfoCache
    {
        private const string GitTfsCachedMergeInfoFileName = "tfsMergeInfoCache.xml";
        private string gitDir = string.Empty;

        private readonly Dictionary<(string tfsPathBranchToCreate, string tfsPathParentBranch, int firstChangesetInBranchToCreate, int lastChangesetIdToCheck), MergeInfoCacheEntry> cache
            = new Dictionary<(string tfsPathBranchToCreate, string tfsPathParentBranch, int firstChangesetInBranchToCreate, int lastChangesetIdToCheck), MergeInfoCacheEntry>();
        private readonly Dictionary<(string tfsPathBranchToCreate, string tfsPathParentBranch, int firstChangesetInBranchToCreate), MergeInfoCacheEntry> cacheMinimum
            = new Dictionary<(string tfsPathBranchToCreate, string tfsPathParentBranch, int firstChangesetInBranchToCreate), MergeInfoCacheEntry>();
        private string GetCacheFilePath(string gitDir) => Path.Combine(gitDir, GitTfsCachedMergeInfoFileName);

        public bool TryGetValue(string tfsPathBranchToCreate, string tfsPathParentBranch, int firstChangesetInBranchToCreate, int lastChangesetIdToCheck,
            out int mergeBaseChangeset, out List<MergeInfo> mergeInfoList)
        {
            if(this.cache.TryGetValue((tfsPathBranchToCreate, tfsPathParentBranch, firstChangesetInBranchToCreate, lastChangesetIdToCheck), out MergeInfoCacheEntry entry))
            {
                mergeBaseChangeset = entry.MergeBaseChangeset;
                mergeInfoList = entry.MergeInfoList;
                return true;
            }
            if (this.cacheMinimum.TryGetValue((tfsPathBranchToCreate, tfsPathParentBranch, firstChangesetInBranchToCreate), out entry))
            {
                mergeBaseChangeset = entry.MergeBaseChangeset;
                mergeInfoList = entry.MergeInfoList;
                return true;
            }
            mergeBaseChangeset = 0;
            mergeInfoList = null;
            return false;
        }

        public void Parse(string gitDir)
        {
            this.gitDir = gitDir;
            string cachePath = GetCacheFilePath(gitDir);
            if (!File.Exists(cachePath))
            {
                return;
            }
            Trace.WriteLine("Reading " + GitTfsCachedMergeInfoFileName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<MergeInfoCacheEntry>));
            using (var stream = System.IO.File.OpenRead(cachePath))
            {
                List<MergeInfoCacheEntry> mergeInfoCacheList = xmlSerializer.Deserialize(stream) as List<MergeInfoCacheEntry>;
                foreach (MergeInfoCacheEntry entry in mergeInfoCacheList)
                {
                    this.cache[(entry.TfsPathBranchToCreate, entry.TfsPathParentBranch, entry.FirstChangesetInBranchToCreate, entry.LastChangesetIdToCheck)] = entry;
                    this.cacheMinimum[(entry.TfsPathBranchToCreate, entry.TfsPathParentBranch, entry.FirstChangesetInBranchToCreate)] = entry;
                }
            }
        }

        public void AddAndSave(string tfsPathBranchToCreate, string tfsPathParentBranch, int firstChangesetInBranchToCreate, int lastChangesetIdToCheck,
            int mergeBaseChangeset, List<MergeInfo> mergeInfoList)
        {
            MergeInfoCacheEntry entry = new MergeInfoCacheEntry()
            {
                TfsPathBranchToCreate = tfsPathBranchToCreate,
                TfsPathParentBranch = tfsPathParentBranch,
                FirstChangesetInBranchToCreate = firstChangesetInBranchToCreate,
                LastChangesetIdToCheck = lastChangesetIdToCheck,
                MergeBaseChangeset = mergeBaseChangeset,
                MergeInfoList = mergeInfoList
            };
            this.cache[(entry.TfsPathBranchToCreate, entry.TfsPathParentBranch, entry.FirstChangesetInBranchToCreate, entry.LastChangesetIdToCheck)] = entry;
            this.cacheMinimum[(entry.TfsPathBranchToCreate, entry.TfsPathParentBranch, entry.FirstChangesetInBranchToCreate)] = entry;
            if (string.IsNullOrEmpty(this.gitDir))
            {
                throw new InvalidOperationException("gitDir not set");
            }
            string cachePath = GetCacheFilePath(gitDir);
            using (var writer = new System.IO.StreamWriter(cachePath))
            {
                var serializer = new XmlSerializer(typeof(List<MergeInfoCacheEntry>));
                serializer.Serialize(writer, this.cache.Values.ToList());
                writer.Flush();
            }
        }

        public class MergeInfoCacheEntry
        {
            public string TfsPathBranchToCreate { get; set; }
            public string TfsPathParentBranch { get; set; }
            public int FirstChangesetInBranchToCreate { get; set; }
            public int LastChangesetIdToCheck { get; set; }

            public int MergeBaseChangeset { get; set; }
            public List<MergeInfo> MergeInfoList { get; set; } = new List<MergeInfo>();
        }

        public class MergeInfo
        {
            public int SourceChangeType;
            public int SourceChangeset;
            public string SourceItem;
            public int TargetChangeType;
            public int TargetChangeset;
            public string TargetItem;

            public override string ToString() => $"`{TargetChangeType}` C{TargetChangeset} `{TargetItem}` Source `{SourceChangeType}` C{SourceChangeset} `{SourceItem}`";
        }
    }
}
