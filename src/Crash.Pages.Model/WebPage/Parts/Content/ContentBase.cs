namespace Crash.Pages.Model.WebPage.Parts.Content
{
    /// <summary>
    /// ページの基底クラス
    /// </summary>
    public abstract class ContentBase
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract bool IsRobotAllow { get; }

        /// <summary>
        /// パーマリンク
        /// </summary>
        public abstract string Permalink { get; }

        public string PermalinkFull => Permalink + "index.html";

        /// <summary>
        /// タイトル
        /// </summary>
        public abstract string Title { get; }

        public string HtmlText { get; protected set; }

        /// <summary>
        /// 変換処理
        /// </summary>
        public abstract void Build();
    }
}
