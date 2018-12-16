using System;
using System.Collections.Generic;
using System.Linq;
using Crash.Core.Collections;
using Crash.Core.IO;
using Crash.Core.Reflection;
using Crash.Pages.Model.AjaxData.Search;
using Crash.Pages.Model.WebPage;
using Crash.Pages.Model.WebPage.Parts;
using Crash.Pages.Model.WebPage.Parts.Content;
using Crash.Pages.Storage;

namespace Crash.Pages.Generator
{
    /// <summary>
    /// メインの処理フローを管理する。
    /// </summary>
    public sealed class SiteGenerator
    {
        /// <summary>
        /// HTMLを1ページ出力する毎のイベント
        /// </summary>
        public event Action<Page> PageGenerated;

        private readonly SourceStorage _srcRepository;
        private readonly DestinationStorage _outputRepository;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="projectDir"></param>
        public SiteGenerator(string src, string dest, bool isPreviewMode)
        {
            _srcRepository = new SourceStorage(src, isPreviewMode);
            _outputRepository = new DestinationStorage(dest, isPreviewMode);
        }

        /// <summary>
        /// 全ページのHTML出力
        /// </summary>
        public void Generate()
        {
            var articlePageList = _srcRepository.ReadArticles();
            if (!articlePageList.Any())
            {
                return;
            }

            var duplicateIdList = articlePageList.DuplicateKey(page => page.Id).ToList();
            if (duplicateIdList.Any())
            {
                throw new Exception($"記事IDが重複しています：{string.Join(", ", duplicateIdList)}");
            }

            var sidebar = new Sidebar(articlePageList);

            var pageList = new List<Page>();
            pageList.AddRange(articlePageList.Select(article => new Page(article, sidebar)));

            pageList.AsParallel().ForAll(page =>
            {
                page.Build();

                PageGenerated?.Invoke(page);
            });

            pageList.Add(new Page(new TopContent(articlePageList), sidebar).Build());
            pageList.Add(new Page(new TagSearchContent(sidebar), sidebar).Build());
            pageList.Add(new Page(new SearchContent(), sidebar).Build());

            foreach (var page in pageList)
            {
                _outputRepository.WritePage(page);
            }

            _outputRepository.WriteAjaxDataForSearch(articlePageList);
            _outputRepository.WriteAjaxDataForTagSearch(articlePageList);

            var staticFilesDir = PathUtil.CombineToFullPath(AssemblyUtil.GetEntryDirectoryPath(), "StaticFiles");
            _outputRepository.CopyDirectory(staticFilesDir, string.Empty);

            foreach (var item in _srcRepository.SearchOtherFiles())
            {
                _outputRepository.CopyFile(item.fi.FullName, item.relativePath);
            }
        }
    }
}
