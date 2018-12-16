using System.Text;
using Crash.Core.Text;
using Crash.Pages.Model.WebPage.Layout.Template;
using Crash.Pages.Model.WebPage.Parts;
using Crash.Pages.Model.WebPage.Parts.Content;

namespace Crash.Pages.Model.WebPage
{
    public sealed class Page
    {
        /// <summary>
        /// サイト名
        /// </summary>
        public static readonly string SiteName = "サイト名"; // ユーザー指定部分

        public string Html { get; private set; }

        private readonly ContentBase _content;
        private readonly Sidebar _sidebar;

        public string PermalinkFull => _content.PermalinkFull;

        public bool IsArticle => _content is ArticleContent;

        public ArticleContent Article => _content as ArticleContent;

        public ContentBase Content => _content;

        public Page(ContentBase contentBase, Sidebar sidebar)
        {
            _content = contentBase;
            _sidebar = sidebar;
        }

        public Page Build()
        {
            _content.Build();

            Html = new LayoutTemplatetTansformer().Transform(_content, _sidebar);

            return this;
        }
    }
}
