using System.Collections.Generic;
using System.Linq;

namespace Crash.Pages.Model.WebPage.Parts.Content.Template
{
    class ArticleContentTemplateTansformer
    {
        public string Transform(ArticleContent article, string htmlFromMarkdown)
        {
            var session = new Dictionary<string, object>
            {
                ["article"] = article,
                ["htmlFromMarkdown"] = htmlFromMarkdown,
            };
            return new ArticleContentTemplate { Session = session }.TransformText();
        }
    }
}
