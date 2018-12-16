using Crash.Pages.Model.WebPage.Parts.Content.Template;

namespace Crash.Pages.Model.WebPage.Parts.Content
{
    /// <summary>
    /// タグページ
    /// </summary>
    public sealed class TagSearchContent : ContentBase
    {
        /// <summary>
        /// パーマリンク
        /// </summary>
        public static readonly string PermalinkConst = "/tags/";

        /// <summary>
        /// 検索インデックスのリンク書式
        /// </summary>
        public static readonly string AjaxDataPermalinkFormat = "/ajaxdata/tag-search/plain-article{0}.json";

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
        public override string Title { get; } = "タグ";

        private readonly Sidebar _sidebar;

        public TagSearchContent(Sidebar sidebar)
        {
            _sidebar = sidebar;
        }

        /// <summary>
        /// 変換処理
        /// </summary>
        public override void Build()
        {
            HtmlText = new TagSearchContentTemplateTansformer().Transform(Page.SiteName, _sidebar);
        }
    }
}
