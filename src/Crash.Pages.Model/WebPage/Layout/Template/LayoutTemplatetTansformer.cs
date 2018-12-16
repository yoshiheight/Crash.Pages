using System.Collections.Generic;
using Crash.Pages.Model.WebPage.Parts;
using Crash.Pages.Model.WebPage.Parts.Content;

namespace Crash.Pages.Model.WebPage.Layout.Template
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LayoutTemplatetTansformer
    {
        public string Transform(ContentBase page, Sidebar sidebar)
        {
            // HTML生成
            var session = new Dictionary<string, object>
            {
                ["page"] = page,
                ["sidebar"] = sidebar,
                ["sitename"] = Page.SiteName,
            };

            return new LayoutTemplate { Session = session }.TransformText().TrimStart();
        }
    }
}
