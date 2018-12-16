using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace Crash.Pages
{
    /// <summary>
    /// コマンドライン引数を表す
    /// </summary>
    public sealed class Options
    {
        [Option("src", Required = true, HelpText = "=入力ディレクトリ")]
        public string SourcePath { get; set; }

        [Option("dest", Required = true, HelpText = "=出力ディレクトリ")]
        public string DestinationPath { get; set; }

        [Option("preview", HelpText = "=プレビューモード")]
        public bool IsPreviewMode { get; set; }
    }
}
