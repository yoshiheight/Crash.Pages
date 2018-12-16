using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Crash.Core;
using Crash.Core.Collections;
using Crash.Core.IO;
using Crash.Core.Net;
using Crash.Core.Text;
using Crash.Pages.Model.AjaxData.Search;
using Crash.Pages.Model.WebPage;
using Crash.Pages.Model.WebPage.Parts.Content;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Crash.Pages.Storage
{
    public sealed class DestinationStorage
    {
        /// <summary>
        /// エンコーディング
        /// </summary>
        private static readonly Encoding Encoding = EncodeUtil.UTF8NoBOM;

        private readonly DirectoryInfo _outputDir;

        /// <summary>
        /// 検索インデックスの1ファイルのサイズ上限
        /// </summary>
#if DEBUG
        public static readonly int SearchIndexMaxFileSize = 20 * 1024;
#else
        public static readonly int SearchIndexMaxFileSize = 200 * 1024;
#endif

        public DestinationStorage(string dest, bool isPreviewMode)
        {
            var destDir = new DirectoryInfo(dest);

            IOUtil.ThrowIfDirectoryNotFound(destDir.FullName);

            var outputDirName = isPreviewMode ? "_preview" : "_site";

            _outputDir = destDir.CombineDirectoryPath(outputDirName);
            _outputDir.Create();
        }

        public void WriteAjaxDataForSearch(IEnumerable<ArticleContent> articles)
        {
            WriteSearchIndexes(articles, SearchContent.AjaxDataPermalinkFormat, false);
        }

        public void WriteAjaxDataForTagSearch(IEnumerable<ArticleContent> articles)
        {
            WriteSearchIndexes(articles, TagSearchContent.AjaxDataPermalinkFormat, true);
        }

        /// <summary>
        /// 検索インデックス情報のファイル出力
        /// </summary>
        /// <param name="searchIndexContainer"></param>
        private void WriteSearchIndexes(IEnumerable<ArticleContent> articles, string linkFormat, bool isClearContent)
        {
            var searchIndexList = articles.Select(article => new PlainArticle(article)).ToList();

            if (isClearContent)
            {
                foreach (var article in searchIndexList)
                {
                    article.ClearContent();
                }
            }

            // 検索インデックス情報一覧を一定バイト数毎に分割し、ファイル保存する
            foreach (var searchIndex in GetDividedIndexes(searchIndexList, Encoding, SearchIndexMaxFileSize).Indexed())
            {
                var jsonUrl = string.Format(linkFormat, searchIndex.index);
                WriteJsonFile(jsonUrl, searchIndex.value);
            }
        }

        /// <summary>
        /// 検索インデックス情報リストを指定バイト数毎に分割して取得する
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        private List<IReadOnlyList<PlainArticle>> GetDividedIndexes(IEnumerable<PlainArticle> searchIndexes, Encoding encoding, int maxSize)
        {
            return searchIndexes
                .OrderByDescending(indexInfo => indexInfo.Date)
                .ThenByDescending(indexInfo => indexInfo.Title)
                .Concat(new[] { PlainArticle.Terminate })
                .Divide(indexInfo => indexInfo.GetContentByteCount(encoding), maxSize)
                .ToList();
        }

        public void WritePage(Page page)
        {
            var relativePath = UriToPath(page.PermalinkFull);
            var outputFile = _outputDir.CombineFilePath(relativePath);

            VerifyOutputPath(outputFile);

            outputFile.Directory.Create();
            IOUtil.WriteAllText(outputFile.FullName, page.Html, Encoding, StringUtil.Lf);
            if (page.Content is ArticleContent article)
            {
                outputFile.LastWriteTime = article.MarkdownFileUpdateTime;
            }
        }

        private void WriteJsonFile(string permalink, object data)
        {
            var relativePath = UriToPath(permalink);
            var outputJsonFile = _outputDir.CombineFilePath(relativePath);

            VerifyOutputPath(outputJsonFile);

            var jsonSettings = new JsonSerializerSettings();
#if DEBUG
            jsonSettings.Formatting = Formatting.Indented;
#endif
            IOUtil.WriteAllText(outputJsonFile.FullName, JsonConvert.SerializeObject(data, jsonSettings), Encoding, StringUtil.Lf);
        }

        public void CopyFile(string sourcePath, string destRelativePath)
        {
            var outputFile = _outputDir.CombineFilePath(destRelativePath);

            VerifyOutputPath(outputFile);

            IOUtil.CopyFile(sourcePath, outputFile.FullName, FileCopyOptions.Update);
        }

        public void CopyDirectory(string sourcePath, string destRelativePath)
        {
            var outputDir = _outputDir.CombineDirectoryPath(destRelativePath);

            VerifyOutputPath(outputDir);

            IOUtil.CopyDirectory(sourcePath, outputDir.FullName, FileCopyOptions.Update);
        }

        /// <summary>
        /// ファイルの出力先が正しいかの判定
        /// </summary>
        /// <param name="outputFile"></param>
        private void VerifyOutputPath(FileSystemInfo target)
        {
            if (!PathUtil.EqualsOrSubPath(_outputDir.FullName, target.FullName))
            {
                throw new InvalidPathException();
            }
        }

        /// <summary>
        /// URIの区切り文字をファイルシステムパスの区切り文字に変換する
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private static string UriToPath(string uri)
        {
            return uri
                .Trim(UriUtil.UriSeparatorChar)
                .Replace(UriUtil.UriSeparatorChar, Path.DirectorySeparatorChar);
        }
    }
}
