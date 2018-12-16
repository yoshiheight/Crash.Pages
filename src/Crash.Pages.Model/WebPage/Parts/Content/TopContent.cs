using System.Collections.Generic;
using System.Linq;
using Crash.Pages.Model.WebPage.Parts.Content.Template;

namespace Crash.Pages.Model.WebPage.Parts.Content
{
    /// <summary>
    /// 記事一覧ページ
    /// </summary>
    public sealed class TopContent : ContentBase
    {
        /// <summary>
        /// パーマリンク
        /// </summary>
        public static readonly string PermalinkConst = "/";

        /// <summary>
        /// 
        /// </summary>
        public override bool IsRobotAllow => true;

        /// <summary>
        /// パーマリンク
        /// </summary>
        public override string Permalink { get; } = PermalinkConst;

        /// <summary>
        /// タイトル
        /// </summary>
        public override string Title { get; } = null;

        /// <summary>
        /// 
        /// </summary>
        private readonly IReadOnlyList<ArticleContent> _pageList;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pageList"></param>
        public TopContent(IReadOnlyList<ArticleContent> pageList)
        {
            _pageList = pageList;
        }

        /// <summary>
        /// 変換処理
        /// </summary>
        public override void Build()
        {
            var pages = _pageList
                .OrderByDescending(page => page.Date)
                .ThenByDescending(page => page.Title);

            HtmlText = new TopContentTemplateTansformer().Transform(pages);
        }
    }
}
