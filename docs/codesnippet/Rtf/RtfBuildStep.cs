// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace RtfDocumentProcessors
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Composition;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Schedulers;

    using MarkupConverter;
    using Microsoft.DocAsCode.Plugins;

    [Export(nameof(RtfDocumentProcessor), typeof(IDocumentBuildStep))]
    public class RtfBuildStep : IDocumentBuildStep
    {
        #region Build
        private readonly TaskFactory _taskFactory = new TaskFactory(new StaTaskScheduler(1));

        public void Build(FileModel model, IHostService host)
        {
            string content = (string)((Dictionary<string, object>)model.Content)["conceptual"];
            content = _taskFactory.StartNew(() => RtfToHtmlConverter.ConvertRtfToHtml(content)).Result;
            ((Dictionary<string, object>)model.Content)["conceptual"] = content;
        }
        #endregion

        #region Others
        public int BuildOrder => 0;

        public string Name => nameof(RtfBuildStep);

        public void Postbuild(ImmutableList<FileModel> models, IHostService host)
        {
        }

        public IEnumerable<FileModel> Prebuild(ImmutableList<FileModel> models, IHostService host)
        {
            return models;
        }
        #endregion
    }
}
