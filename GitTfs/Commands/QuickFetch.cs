﻿using System;
using System.ComponentModel;
using System.IO;
using NDesk.Options;
using Sep.Git.Tfs.Core;
using Sep.Git.Tfs.Util;

namespace Sep.Git.Tfs.Commands
{
    // This isn't intended to ever be a command. The intent is that
    // you create a repository with quick-clone, and then use
    // fetch to stay up-to-date.
    //
    // This cannot be a command until the following are sorted out:
    //  1. How to choose a parent commit.
    //  2. Load the correct set of extant casing.
    public class QuickFetch : Fetch
    {
        private ConfigProperties _properties;
        public QuickFetch(Globals globals, ConfigProperties properties, TextWriter stdout, RemoteOptions remoteOptions, AuthorsFile authors)
            : base(globals, properties, stdout, remoteOptions, authors, null)
        {
            _properties = properties;
        }

        protected override void DoFetch(IGitTfsRemote remote, bool stopOnFailMergeCommit)
        {
            if (InitialChangeset.HasValue)
                remote.QuickFetch(InitialChangeset.Value);
            else
                remote.QuickFetch();
            _properties.InitialChangeset = remote.MaxChangesetId;
            _properties.PersistAllOverrides();
        }
    }
}
