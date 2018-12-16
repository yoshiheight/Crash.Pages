using System.Text;

namespace Crash.Core.Text
{
    /// <summary>
    /// エンコードユーティリティー
    /// </summary>
    public static class EncodeUtil
    {
        /// <summary>
        /// UTF-8 BOM無しエンコーディング
        /// </summary>
        public static readonly Encoding UTF8NoBOM = new UTF8Encoding(false, true);
    }
}
