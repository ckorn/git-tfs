// Generated by git-tfs-test-builder
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using Sep.Git.Tfs.Core.TfsInterop;
using Sep.Git.Tfs.Test.Integration;

namespace Sep.Git.Tfs.Test.Fixtures
{
    static class vtccds
    {
        public static void Prepare(IntegrationHelper.FakeHistoryBuilder r)
        {
            IntegrationHelper.FakeHistoryBuilder.FakeCommiter = "vtccds_cp";
//            r.Changeset(22484, "Created team project folder $/vtccds via the Team Project Creation Wizard", DateTime.Parse("2013-05-24T15:44:32.373Z"))
//                .Change(TfsChangeType.Add | TfsChangeType.Encoding, TfsItemType.Folder, "$/vtccds", Read(null), 390990)
//;
            r.Changeset(22487, "1er commit dans le tronc!!!", DateTime.Parse("2013-05-24T16:27:37.15Z"))
                .Change(TfsChangeType.Add | TfsChangeType.Encoding, TfsItemType.Folder, "$/vtccds/trunk", Read(null), 390992)
                .Change(TfsChangeType.Add | TfsChangeType.Edit | TfsChangeType.Encoding, TfsItemType.File, "$/vtccds/trunk/file.txt", Read("36eae6b6a4717375a4ff16287744cd1d"), 390991)
;
            r.Changeset(22511, "trunk1", DateTime.Parse("2013-05-24T22:58:06.307Z"))
                .Change(TfsChangeType.Edit, TfsItemType.File, "$/vtccds/trunk/file.txt", Read("74687eb86f9a0c7a2ca65efbbae6a5ad"), 390991)
;
            r.Changeset(22512, "trunk2", DateTime.Parse("2013-05-24T22:59:05.403Z"))
                .Change(TfsChangeType.Edit, TfsItemType.File, "$/vtccds/trunk/file.txt", Read("70e6b3871859cf52ef78fba876bc37a4"), 390991)
;
            r.BranchChangeset(22513, "Creation branch $/vtccds/b1", DateTime.Parse("2013-05-24T23:01:06.747Z"), "$/vtccds/trunk", "$/vtccds/b1", 22511)
                .Change(TfsChangeType.Branch, TfsItemType.Folder, "$/vtccds/b1", Read(null), 391048)
                .Change(TfsChangeType.Branch, TfsItemType.File, "$/vtccds/b1/file.txt", Read("74687eb86f9a0c7a2ca65efbbae6a5ad"), 391049)
;
            r.Changeset(22514, "b1.1", DateTime.Parse("2013-05-24T23:03:43.493Z"))
                .Change(TfsChangeType.Edit, TfsItemType.File, "$/vtccds/b1/file.txt", Read("96b58369f5bd2b081005ba208ac649b8"), 391049)
;
            r.MergeChangeset(22515, "Merge branch 'b1' in trunk\n\nb1.1\n\nCreation branch $/vtccds/b1", DateTime.Parse("2013-05-24T23:48:45.6Z"), "$/vtccds/b1", "$/vtccds/trunk", 22514)
                .Change(TfsChangeType.Edit | TfsChangeType.Merge, TfsItemType.File, "$/vtccds/trunk/file.txt", Read("4cd460612edcbab617997836871d7291"), 390991)
;
            r.Changeset(25197, "trunk after merge", DateTime.Parse("2013-09-09T20:33:30.637Z"))
                .Change(TfsChangeType.Edit, TfsItemType.File, "$/vtccds/trunk/file.txt", Read("fba9507f41916830d82be84924dded7b"), 390991)
;
            r.BranchChangeset(25198, "create branch to test renaming branch in tfs", DateTime.Parse("2013-09-09T20:39:59.453Z"), "$/vtccds/trunk", "$/vtccds/testRename", 25197)
                .Change(TfsChangeType.Branch, TfsItemType.Folder, "$/vtccds/testRename", Read(null), 426363)
                .Change(TfsChangeType.Branch, TfsItemType.File, "$/vtccds/testRename/file.txt", Read("fba9507f41916830d82be84924dded7b"), 426364)
;
            r.Changeset(25199, "rename branch1", DateTime.Parse("2013-09-09T20:45:50.05Z"))
                .Change(TfsChangeType.Edit, TfsItemType.File, "$/vtccds/testRename/file.txt", Read("2ea0b9d555f39b93aa67d834aec81dec"), 426364)
;
            r.BranchChangeset(25200, "testRename renamed in afterRename", DateTime.Parse("2013-09-09T22:13:05.513Z"), "$/vtccds/testRename", "$/vtccds/afterRename", 25199)
                .Change(TfsChangeType.Rename, TfsItemType.Folder, "$/vtccds/afterRename", Read(null), 426365)
                .Change(TfsChangeType.Delete, TfsItemType.Folder, "$/vtccds/testRename", Read(null), 426363)
                .Change(TfsChangeType.Rename, TfsItemType.File, "$/vtccds/afterRename/file.txt", Read("2ea0b9d555f39b93aa67d834aec81dec"), 426364)
                .Change(TfsChangeType.Delete, TfsItemType.File, "$/vtccds/testRename/file.txt", Read("2ea0b9d555f39b93aa67d834aec81dec"), 426364)
;
            r.Changeset(25202, "1st commit after rename", DateTime.Parse("2013-09-10T12:19:16.28Z"))
                .Change(TfsChangeType.Edit, TfsItemType.File, "$/vtccds/afterRename/file.txt", Read("6553cc439299c73677e0c43dd0fd786e"), 426364)
;
            r.BranchChangeset(25957, "branch to test rename of a file", DateTime.Parse("2013-10-25T08:33:22.15Z"), "$/vtccds/trunk", "$/vtccds/renameFile", 25197)
                .Change(TfsChangeType.Branch, TfsItemType.Folder, "$/vtccds/renameFile", Read(null), 439692)
                .Change(TfsChangeType.Branch, TfsItemType.File, "$/vtccds/renameFile/file.txt", Read("fba9507f41916830d82be84924dded7b"), 439693)
;
            r.Changeset(25958, "file.txt renamed to renamed_file.txt", DateTime.Parse("2013-10-25T08:37:09.53Z"))
                .Change(TfsChangeType.Delete, TfsItemType.File, "$/vtccds/renameFile/file.txt", Read("fba9507f41916830d82be84924dded7b"), 439693)
                .Change(TfsChangeType.Rename, TfsItemType.File, "$/vtccds/renameFile/renamed_file.txt", Read("fba9507f41916830d82be84924dded7b"), 439693)
;
            r.Changeset(25959, "other useless commit", DateTime.Parse("2013-10-25T08:37:33.233Z"))
                .Change(TfsChangeType.Edit, TfsItemType.File, "$/vtccds/renameFile/renamed_file.txt", Read("9a975dd2be9a190424fc226a5a25d84f"), 439693)
;
            r.Changeset(26394, "fix #1669 and  #1670", DateTime.Parse("2013-11-18T10:15:38.977Z"))
                .Change(TfsChangeType.Edit, TfsItemType.File, "$/vtccds/trunk/file.txt", Read("4f4cd8afcb1708c003540aea8d64239d"), 390991)
;
            r.Changeset(26496, "wi #1669", DateTime.Parse("2013-11-22T21:59:09.56Z"))
                .Change(TfsChangeType.Edit, TfsItemType.File, "$/vtccds/trunk/file.txt", Read("fd30d6210bb40fd2c9b689f0bf8b2833"), 390991)
;
            r.Changeset(26497, "wi #1670", DateTime.Parse("2013-11-22T22:03:45.9Z"))
                .Change(TfsChangeType.Edit, TfsItemType.File, "$/vtccds/trunk/file.txt", Read("86882c0ae4717b4969d54f676c0261e3"), 390991)
;
            r.BranchChangeset(27503, "create a new branch from nowhere\n=> not a branch from another branch!!", DateTime.Parse("2014-01-05T16:08:07.48Z"), string.Empty, "$/vtccds/branch_from_nowhere", -1)
                .Change(TfsChangeType.Add | TfsChangeType.Encoding, TfsItemType.Folder, "$/vtccds/branch_from_nowhere", Read(null), 479197)
;
            r.Changeset(27505, "first changeset from nowhere...", DateTime.Parse("2014-01-05T20:41:20.78Z"))
                .Change(TfsChangeType.Add | TfsChangeType.Edit | TfsChangeType.Encoding, TfsItemType.File, "$/vtccds/branch_from_nowhere/file_from_nowhere.txt", Read("6c6060d9696edc7000396cc499ab8277"), 479199)
;
            r.Changeset(27506, "2nd changeset from nowhere...", DateTime.Parse("2014-01-05T20:43:16.14Z"))
                .Change(TfsChangeType.Edit, TfsItemType.File, "$/vtccds/branch_from_nowhere/file_from_nowhere.txt", Read("5a18c163f1804fe83dfeae04f1d1d34e"), 479199)
;
            r.MergeChangeset(27507, "baseless merge from branch not branched \"branch_from_nowhere\"", DateTime.Parse("2014-01-05T21:02:33.82Z"), "$/vtccds/branch_from_nowhere", "$/vtccds/renameFile", 27506)
                .Change(TfsChangeType.Merge, TfsItemType.Folder, "$/vtccds/renameFile", Read(null), 439692)
                .Change(TfsChangeType.Encoding | TfsChangeType.Branch | TfsChangeType.Merge, TfsItemType.File, "$/vtccds/renameFile/file_from_nowhere.txt", Read("5a18c163f1804fe83dfeae04f1d1d34e"), 479200)
;
            r.BranchChangeset(27508, "rename the branch a second time...", DateTime.Parse("2014-01-05T23:08:43.21Z"), "$/vtccds/afterRename", "$/vtccds/renamedTwice", 25202)
                .Change(TfsChangeType.Delete, TfsItemType.Folder, "$/vtccds/afterRename", Read(null), 426365)
                .Change(TfsChangeType.Rename, TfsItemType.Folder, "$/vtccds/renamedTwice", Read(null), 479201)
                .Change(TfsChangeType.Delete, TfsItemType.File, "$/vtccds/afterRename/file.txt", Read("6553cc439299c73677e0c43dd0fd786e"), 426364)
                .Change(TfsChangeType.Rename, TfsItemType.File, "$/vtccds/renamedTwice/file.txt", Read("6553cc439299c73677e0c43dd0fd786e"), 426364)
;
            r.Changeset(27509, "first commit after 2nd branch renaming...", DateTime.Parse("2014-01-05T23:12:55.78Z"))
                .Change(TfsChangeType.Edit, TfsItemType.File, "$/vtccds/renamedTwice/file.txt", Read("d67034311a9ccd1a5a62cc0ecdaa1a6f"), 426364)
;
            r.BranchChangeset(30333, "Creation branch $/vtccds/b1.1", DateTime.Parse("2014-06-05T11:06:52.633Z"), "$/vtccds/trunk", "$/vtccds/b1.1", 26394)
                .Change(TfsChangeType.Branch, TfsItemType.Folder, "$/vtccds/b1.1", Read(null), 513921)
                .Change(TfsChangeType.Branch, TfsItemType.File, "$/vtccds/b1.1/file.txt", Read("4f4cd8afcb1708c003540aea8d64239d"), 513922)
;
            r.Changeset(30397, "b1.1", DateTime.Parse("2014-06-09T22:24:49.643Z"))
                .Change(TfsChangeType.Edit, TfsItemType.File, "$/vtccds/b1.1/file.txt", Read("c02331d395d07dc7efaaa295ac474971"), 513922)
;
            r.Changeset(30398, "b1.1 again", DateTime.Parse("2014-06-09T22:25:12.27Z"))
                .Change(TfsChangeType.Edit, TfsItemType.File, "$/vtccds/b1.1/file.txt", Read("1b9bf73806f680f3f6a79ca80e635dcf"), 513922)
;
            r.Changeset(31588, "test changeset after merge", DateTime.Parse("2014-07-13T19:23:16.753Z"))
                .Change(TfsChangeType.Edit, TfsItemType.File, "$/vtccds/b1/file.txt", Read("52c0fc9d4cfbf1ae07f36b8e711a5d6b"), 391049)
;
            r.BranchChangeset(33407, "Rename branch and other changes...\n\n-Modify file.txt\n-Add file2.txt", DateTime.Parse("2014-10-11T15:19:56.077Z"), "$/vtccds/renamedTwice", "$/vtccds/renamed3", 27509)
                .Change(TfsChangeType.Rename, TfsItemType.Folder, "$/vtccds/renamed3", Read(null), 479201)
                .Change(TfsChangeType.Delete, TfsItemType.Folder, "$/vtccds/renamedTwice", Read(null), 479201)
                .Change(TfsChangeType.Edit | TfsChangeType.Rename, TfsItemType.File, "$/vtccds/renamed3/file.txt", Read("fb5dcd187b5ab3729973b310f3c55596"), 426364)
                .Change(TfsChangeType.Add | TfsChangeType.Edit | TfsChangeType.Encoding, TfsItemType.File, "$/vtccds/renamed3/file2.txt", Read("0445de4a590c27552a06ecda1b5ff2af"), 587696)
                .Change(TfsChangeType.Delete, TfsItemType.File, "$/vtccds/renamedTwice/file.txt", Read("d67034311a9ccd1a5a62cc0ecdaa1a6f"), 426364)
;
            IntegrationHelper.FakeHistoryBuilder.FakeCommiter = null;
        }

        private static byte[] Read(string itemContentHash)
        {
            if (itemContentHash == null)
                return new byte[0];

            using (var stream = typeof(vtccds).Assembly.GetManifestResourceStream("Sep.Git.Tfs.Test.Fixtures.vtccds." + itemContentHash))
            {
                if (stream == null)
                    throw new Exception("Fail: Can not find test file:" + itemContentHash);

                using (var gzipReader = new GZipStream(stream, CompressionMode.Decompress))
                {
                    var buffer = new MemoryStream();
                    gzipReader.CopyTo(buffer);
                    return buffer.ToArray();
                }
            }
        }
    }
}
