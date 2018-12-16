using System.Collections.Generic;

namespace Crash.Pages.Model.WebPage.Parts.Content.Template
{
    class SearchContentTemplateTansformer
    {
        public string Transform(string sitename)
        {
            var session = new Dictionary<string, object>
            {
                ["sitename"] = sitename,
            };

            return new SearchContentTemplate() { Session = session }.TransformText();
        }
    }
}
