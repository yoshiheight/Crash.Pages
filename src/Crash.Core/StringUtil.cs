using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Crash.Core
{
    /// <summary>
    /// 文字列ユーティリティー
    /// </summary>
    public static class StringUtil
    {
        /// <summary>
        /// 空文字列
        /// </summary>
        public const string Empty = "";

        /// <summary>
        /// ダブルクォーテーション
        /// </summary>
        public const string DoubleQuote = "\"";

        /// <summary>
        /// キャリッジリターン（\r）
        /// </summary>
        public const string Cr = "\r";

        /// <summary>
        /// ラインフィード（\n）
        /// </summary>
        public const string Lf = "\n";

        /// <summary>
        /// キャリッジリターンとラインフィード（\r\n）
        /// </summary>
        public const string CrLf = "\r\n";

        /// <summary>
        /// 文字列を括る
        /// </summary>
        /// <param name="str"></param>
        /// <param name="enclose"></param>
        /// <returns></returns>
        public static string Enclose(string str, string enclose)
        {
            // 元々括られているかの判断は先頭部分のみとする
            if (!str.StartsWith(enclose))
            {
                return enclose + str + enclose;
            }
            return str;
        }

        /// <summary>
        /// 文字列をダブルクォーテーションで括る
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncloseDoubleQuote(string str)
        {
            return Enclose(str, DoubleQuote);
        }

        /// <summary>
        /// 文字列が同一かどうかをバイナリレベルで判定する
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static bool Equals(string str1, string str2)
        {
            return string.Equals(str1, str2, StringComparison.Ordinal);
        }

        /// <summary>
        /// 文字列が同一かどうかをバイナリレベルで判定する（大文字小文字は無視する）
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(string str1, string str2)
        {
            return string.Equals(str1, str2, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 文字列を比較する
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static int Compare(string str1, string str2)
        {
            return string.Compare(str1, str2);
        }

        /// <summary>
        /// 文字列を比較する（大文字小文字は無視する）
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static int CompareIgnoreCase(string str1, string str2)
        {
            return string.Compare(str1, str2, true);
        }

        /// <summary>
        /// 改行毎の文字列を列挙する
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static IEnumerable<string> ReadLines(string text)
        {
            using (var reader = new StringReader(text))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        /// <summary>
        /// 先頭から指定長分の文字列を取得する
        /// </summary>
        /// <param name="text"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string LeftString(string text, int length)
        {
            return text.Substring(0, Math.Min(text.Length, length));
        }

        public static string Repeat(string text, int count)
        {
            return new StringBuilder().Insert(0, text, count).ToString();
        }
    }
}
