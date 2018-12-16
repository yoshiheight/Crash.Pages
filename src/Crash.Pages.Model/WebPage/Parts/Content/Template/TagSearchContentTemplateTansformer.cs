using System.Collections.Generic;

namespace Crash.Pages.Model.WebPage.Parts.Content.Template
{
    class TagSearchContentTemplateTansformer
    {
        public string Transform(string sitename, Sidebar sidebar)
        {
            var session = new Dictionary<string, object>
            {
                ["sitename"] = sitename,
                ["sidebar"] = sidebar,
            };

            return new TagSearchContentTemplate() { Session = session }.TransformText();
        }
    }
}
