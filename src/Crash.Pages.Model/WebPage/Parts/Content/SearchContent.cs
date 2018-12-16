using Crash.Pages.Model.WebPage.Parts.Content.Template;

namespace Crash.Pages.Model.WebPage.Parts.Content
{
    /// <summary>
    /// 検索ページ
    /// </summary>
    public sealed class SearchContent : ContentBase
    {
        /// <summary>
        /// パーマリンク
        /// </summary>
        public static readonly string PermalinkConst = "/search/";

        /// <summary>
        /// 検索インデックスのリンク書式
        /// </summary>
        public static readonly string AjaxDataPermalinkFormat = "/ajaxdata/search/plain-article{0}.json";

        /// <summary>
        /// 
        /// </summary>
        public override bool IsRobotAllow => false;

        /// <summary>
        /// パーマリンク
        /// </summary>
        public override string Permalink { get; } = PermalinkConst;

        /// <summary>
        /// タイトル
        /// </summary>
        public override string Title { get; } = "検索";

        public SearchContent()
        {
        }

        /// <summary>
        /// 変換処理
        /// </summary>
        public override void Build()
        {
            HtmlText = new SearchContentTemplateTansformer().Transform(Page.SiteName);
        }
    }
}
