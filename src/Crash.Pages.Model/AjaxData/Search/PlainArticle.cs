using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Crash.Core.Text;
using Crash.Pages.Model.WebPage.Parts.Content;

namespace Crash.Pages.Model.AjaxData.Search
{
    /// <summary>
    /// 検索インデックス情報
    /// </summary>
    [DataContract]
    public sealed class PlainArticle
    {
        /// <summary>
        /// ターミネート用
        /// </summary>
        public static readonly PlainArticle Terminate = new PlainArticle();

        private PlainArticle() { }

        public PlainArticle(ArticleContent article)
        {
            Url = article.Permalink;
            Title = article.Title;
            Tags = article.Tags;
            PlainText = article.PlainText;
            Date = article.Date;
        }

        /// <summary>
        /// ページURL
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; } = string.Empty;

        /// <summary>
        /// ページタイトル
        /// </summary>
        [DataMember(Name = "title")]
        public string Title { get; } = string.Empty;

        /// <summary>
        /// タグ
        /// </summary>
        [DataMember(Name = "tags")]
        public string[] Tags { get; } = Array.Empty<string>();

        /// <summary>
        /// ページ内容
        /// </summary>
        [DataMember(Name = "plainText")]
        public string PlainText { get; private set; } = string.Empty;

        /// <summary>
        /// ページ更新日
        /// </summary>
        [DataMember(Name = "date")]
        public string Date { get; } = string.Empty;

        public void ClearContent()
        {
            PlainText = string.Empty;
        }

        /// <summary>
        /// ページ内容のバイト数を取得する
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public int GetContentByteCount(Encoding encoding)
        {
            return encoding.GetByteCount(Title) + encoding.GetByteCount(nameof(Title)) + 5
                + encoding.GetByteCount(Url) + encoding.GetByteCount(nameof(Url)) + 5
                + encoding.GetByteCount(Date) + encoding.GetByteCount(nameof(Date)) + 5
                + encoding.GetByteCount(PlainText) + encoding.GetByteCount(nameof(PlainText)) + 5
                + Tags.Select(tagName => encoding.GetByteCount(tagName)).Sum() + encoding.GetByteCount(nameof(Tags)) + 5;
        }
    }
}
