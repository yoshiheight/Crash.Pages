using System.Collections.Generic;
using System.IO;
using System.Linq;
using Crash.Core;
using Crash.Core.IO;
using Crash.Pages.Model.WebPage.Parts.Content;

namespace Crash.Pages.Storage
{
    public sealed class SourceStorage
    {
        /// <summary>
        /// 入力ディレクトリ
        /// </summary>
        private readonly DirectoryInfo _srcDraftsDir;
        private readonly DirectoryInfo _srcPostsDir;

        private readonly bool _isPreviewMode;

        public SourceStorage(string src, bool isPreviewMode)
        {
            _srcPostsDir = new DirectoryInfo(src).CombineDirectoryPath("_posts");
            _srcDraftsDir = new DirectoryInfo(src).CombineDirectoryPath("_drafts");
            _isPreviewMode = isPreviewMode;
        }

        public List<ArticleContent> ReadArticles()
        {
            var test = ReadArticles(_srcPostsDir.FullName, false, _isPreviewMode).ToList();
            if (_isPreviewMode)
            {
                test.AddRange(ReadArticles(_srcDraftsDir.FullName, true, true));
            }
            return test;
        }

        public List<(FileInfo fi, string relativePath)> SearchOtherFiles()
        {
            var test = SearchOtherFiles(_srcPostsDir.FullName).ToList();
            if (_isPreviewMode)
            {
                test.AddRange(SearchOtherFiles(_srcDraftsDir.FullName));
            }
            return test;
        }

        /// <summary>
        /// マークダウンファイル検索
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<ArticleContent> ReadArticles(string path, bool isDraft, bool isShowMarkdownFilePath)
        {
            return FileSearcher
                .EnumerateFileInfos(path)
                .Where(item => PathUtil.EqualsExtension(item.fi.Extension, ".md"))
                .Select(item => new ArticleContent(item.fi, isDraft, isShowMarkdownFilePath));
        }

        private static IEnumerable<(FileInfo fi, string relativePath)> SearchOtherFiles(string path)
        {
            return FileSearcher
                .EnumerateFileInfos(path)
                .Where(item => !PathUtil.EqualsExtension(item.fi.Extension, ".md"));
        }
    }
}
