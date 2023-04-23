// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace RtfDocumentProcessors
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Composition;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Xml.Linq;

    using Microsoft.DocAsCode.Plugins;
    using Microsoft.DocAsCode.Utility;

    [Export(typeof(IDocumentProcessor))]
    public class RtfDocumentProcessor : IDocumentProcessor
    {
        [ImportMany(nameof(RtfDocumentProcessor))]
        public IEnumerable<IDocumentBuildStep> BuildSteps { get; set; }

        public string Name => nameof(RtfDocumentProcessor);

        public ProcessingPriority GetProcessingPriority(FileAndType file)
        {
            if (file.Type == DocumentType.Article &&
                ".rtf".Equals(Path.GetExtension(file.File), StringComparison.OrdinalIgnoreCase))
            {
                return ProcessingPriority.Normal;
            }
            return ProcessingPriority.NotSupported;
        }

        public FileModel Load(FileAndType file, ImmutableDictionary<string, object> metadata)
        {
            var content = new Dictionary<string, object>
            {
                ["conceptual"] = File.ReadAllText(Path.Combine(file.BaseDir, file.File)),
                ["type"] = "Conceptual",
                ["path"] = file.File,
            };
            return new FileModel(file, content);
        }

        #region Save
        public SaveResult Save(FileModel model)
        {
            HashSet<string> linkToFiles = CollectLinksAndFixDocument(model);

            return new SaveResult
            {
                DocumentType = "Conceptual",
                ModelFile = model.File,
                LinkToFiles = linkToFiles.ToImmutableArray(),
            };
        }
        #endregion

        #region CollectLinksAndFixDocument
        private static HashSet<string> CollectLinksAndFixDocument(FileModel model)
        {
            string content = (string)((Dictionary<string, object>)model.Content)["conceptual"];
            var doc = XDocument.Parse(content);
            var links =
                from attr in doc.Descendants().Attributes()
                where "href".Equals(attr.Name.LocalName, StringComparison.OrdinalIgnoreCase) || "src".Equals(attr.Name.LocalName, StringComparison.OrdinalIgnoreCase)
                select attr;
            var path = (RelativePath)model.File;
            var linkToFiles = new HashSet<string>();
            foreach (var link in links)
            {
                FixLink(link, path, linkToFiles);
            }
            using (var sw = new StringWriter())
            {
                doc.Save(sw);
                ((Dictionary<string, object>)model.Content)["conceptual"] = sw.ToString();
            }
            return linkToFiles;
        }
        #endregion

        #region FixLink
        private static void FixLink(XAttribute link, RelativePath filePath, HashSet<string> linkToFiles)
        {
            string linkFile;
            string anchor = null;
            if (PathUtility.IsRelativePath(link.Value))
            {
                var index = link.Value.IndexOf('#');
                if (index == -1)
                {
                    linkFile = link.Value;
                }
                else if (index == 0)
                {
                    return;
                }
                else
                {
                    linkFile = link.Value.Remove(index);
                    anchor = link.Value.Substring(index);
                }
                var path = filePath + (RelativePath)linkFile;
                var file = (string)path.GetPathFromWorkingFolder();
                link.Value = file + anchor;
                linkToFiles.Add(HttpUtility.UrlDecode(file));
            }
        }
        #endregion

        public void UpdateHref(FileModel model, IDocumentBuildContext context)
        {
        }
    }
}
