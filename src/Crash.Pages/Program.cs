using System;
using CommandLine;
using Crash.Pages.Generator;

namespace Crash.Pages
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = (Parser.Default.ParseArguments<Options>(args) as Parsed<Options>)?.Value;
            if (options == null)
            {
                return;
            }

            Console.WriteLine("================ 開始 ================");

            var ganerator = new SiteGenerator(options.SourcePath, options.DestinationPath, options.IsPreviewMode);
            ganerator.PageGenerated += page => Console.WriteLine($"ページ変換: {page.Content.Title}");
            ganerator.Generate();

            Console.WriteLine("================ 完了 ================");
        }
    }
}
