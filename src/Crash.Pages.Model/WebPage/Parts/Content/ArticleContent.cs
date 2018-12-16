using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Crash.Core;
using Crash.Core.IO;
using Crash.Pages.Model.WebPage.Parts.Content.Template;
using Markdig;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Crash.Pages.Model.WebPage.Parts.Content
{
    /// <summary>
    /// 記事ページ
    /// </summary>
    public sealed class ArticleContent : ContentBase
    {
        /// <summary>
        /// 記事ページ
        /// </summary>
        private static readonly string PermalinkFormat = "/pages/{0}/";

        /// <summary>
        /// 検索インデックスの連続する空白の検索用正規表現
        /// </summary>
        private static readonly Regex TrimSpaceRegex = new Regex(@"\s+", RegexOptions.Multiline);

        /// <summary>
        /// 検索インデックスの連続する空白の置換後の文字列
        /// </summary>
        private static readonly string Space = " ";

        /// <summary>
        /// 記事ID
        /// </summary>
        public string Id => _metadata.Id;

        /// <summary>
        /// タグ
        /// </summary>
        public string[] Tags => _metadata.Tags;

        /// <summary>
        /// 
        /// </summary>
        public override bool IsRobotAllow => true;

        /// <summary>
        /// パーマリンク
        /// </summary>
        public override string Permalink { get; }

        /// <summary>
        /// タイトル
        /// </summary>
        public override string Title { get; }

        /// <summary>
        /// 作成日
        /// </summary>
        public string Date { get; }

        /// <summary>
        /// マークダウンファイルパス
        /// </summary>
        public string MarkdownFilePath { get; }

        public DateTime MarkdownFileUpdateTime { get; }

        public string PlainText { get; private set; }

        private readonly string _markdownText;

        private readonly ArticleMetadata _metadata;

        /// <summary>
        /// エンコーディング
        /// </summary>
        public static readonly Encoding MarkdownFileEncoding = Encoding.UTF8;

        // 記事内容のプレーンテキスト変換
        private readonly MarkdownPipeline _markdownPipeline = new MarkdownPipelineBuilder()
            .UseYamlFrontMatter()
            .UseDiagrams()
            .UsePipeTables()
            .Build();

        public bool IsShowMarkdownFilePath { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="markdownFileInfo"></param>
        public ArticleContent(FileInfo markdownFileInfo, bool isDraft, bool isShowMarkdownFilePath)
        {
            IsShowMarkdownFilePath = isShowMarkdownFilePath;

            _markdownText = IOUtil.ReadAllText(markdownFileInfo.FullName, MarkdownFileEncoding, StringUtil.Lf);
            _metadata = YamlConvert.DeserializeFirst<ArticleMetadata>(_markdownText);

#if DEBUG
            // 記事IDの指定がなかった場合に内部で一時的な値を割り当てる
            if (string.IsNullOrEmpty(_metadata.Id))
            {
                _metadata.Id = GuidUtil.NewNormalGuidString();
            }
#endif

            MarkdownFilePath = markdownFileInfo.FullName;
            MarkdownFileUpdateTime = markdownFileInfo.LastWriteTime;

            Permalink = string.Format(PermalinkFormat, _metadata.Id);

            var titlePre = isDraft ? "(draft) " : string.Empty;
            var fileName = Path.GetFileNameWithoutExtension(markdownFileInfo.Name);

            var date = fileName.Substring(0, 10);
            var title = fileName.Substring(11);

            Title = titlePre + title;
            Date = date;

            if (string.IsNullOrEmpty(Id))
            {
                throw new Exception($"記事IDが指定されていません：{markdownFileInfo.FullName}");
            }
        }

        /// <summary>
        /// 変換処理
        /// </summary>
        public override void Build()
        {
            BuildHtml();
            BuildPlainText();
        }

        private void BuildHtml()
        {
            var htmlFromMarkdown = Markdown.ToHtml(_markdownText, _markdownPipeline);

            HtmlText = new ArticleContentTemplateTansformer().Transform(this, htmlFromMarkdown);
        }

        /// <summary>
        /// プレーンテキストの生成
        /// </summary>
        private void BuildPlainText()
        {
            var sb = new StringBuilder();

            // 記事内容のプレーンテキスト変換
            sb.AppendLine(Markdown.ToPlainText(_markdownText, _markdownPipeline));

            // 連続する空白の除去
            PlainText = TrimSpaceRegex.Replace(sb.ToString(), Space);
        }

        /// <summary>
        /// 記事メタデータ
        /// </summary>
        private sealed class ArticleMetadata
        {
            /// <summary>
            /// 記事ID
            /// </summary>
            [YamlMember(Alias = "id")]
            public string Id { get; set; }

            /// <summary>
            /// タグ
            /// </summary>
            [YamlMember(Alias = "tags")]
            public string[] Tags { get; set; }
        }
    }
}
