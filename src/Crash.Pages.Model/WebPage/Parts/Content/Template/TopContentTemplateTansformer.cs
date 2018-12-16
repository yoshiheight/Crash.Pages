using System.Collections.Generic;
using System.Linq;

namespace Crash.Pages.Model.WebPage.Parts.Content.Template
{
    class TopContentTemplateTansformer
    {
        public string Transform(IEnumerable<ArticleContent> pages)
        {
            var session = new Dictionary<string, object> { { "pages", pages.ToArray() } };
            return new TopContentTemplate { Session = session }.TransformText();
        }
    }
}
