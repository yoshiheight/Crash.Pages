using System;
using System.IO;
using System.Linq;
using Crash.Core.Attributes;
using Crash.Core.Collections;

namespace Crash.Core.IO
{
    /// <summary>
    /// パスユーティリティー
    /// </summary>
    public static class PathUtil
    {
        /// <summary>
        /// 拡張子を比較する。
        /// </summary>
        /// <param name="extension1"></param>
        /// <param name="extension2"></param>
        /// <returns></returns>
        public static bool EqualsExtension(string extension1, string extension2)
        {
            return StringUtil.EqualsIgnoreCase(extension1, extension2);
        }

        /// <summary>
        /// パスを連結し、絶対パスとして取得する。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string CombineToFullPath(string path, params string[] paths)
        {
            return Path.GetFullPath(
                Path.Combine(Path.GetFullPath(path).AsSingleEnumerable().Concat(paths).ToArray()));
        }

        /// <summary>
        /// 相対パスを取得する。
        /// 大文字小文字は区別する。
        /// </summary>
        /// <param name="rootPath"></param>
        /// <param name="targetPath"></param>
        /// <returns></returns>
        public static string GetRelativePath(string rootPath, string targetPath)
        {
            if (TryGetRelativePath(rootPath, targetPath, out var relativePath))
            {
                return relativePath;
            }

            throw new InvalidPathException();
        }

        /// <summary>
        /// 相対パスを取得する。
        /// 大文字小文字は区別する。
        /// </summary>
        /// <param name="rootPath"></param>
        /// <param name="targetPath"></param>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public static bool TryGetRelativePath(string rootPath, string targetPath, out string relativePath)
        {
            relativePath = null;

            if (EqualsPath(rootPath, targetPath))
            {
                relativePath = string.Empty;
                return true;
            }

            if (IsSubPath(rootPath, targetPath))
            {
                rootPath = Path.GetFullPath(rootPath).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
                targetPath = Path.GetFullPath(targetPath);

                relativePath = targetPath.Substring(rootPath.Length);
                return true;
            }

            return false;
        }

        /// <summary>
        /// パスが同じかどうかを判定する。
        /// 大文字小文字は区別する。
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        public static bool EqualsPath(string path1, string path2)
        {
            path1 = Path.GetFullPath(path1).TrimEnd(Path.DirectorySeparatorChar);
            path2 = Path.GetFullPath(path2).TrimEnd(Path.DirectorySeparatorChar);
            return path1 == path2;
        }

        /// <summary>
        /// パスがサブパスかどうかを判定する。
        /// 大文字小文字は区別する。
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        public static bool IsSubPath(string path1, string path2)
        {
            path1 = Path.GetFullPath(path1).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
            path2 = Path.GetFullPath(path2).TrimEnd(Path.DirectorySeparatorChar);

            return path2.StartsWith(path1);
        }

        /// <summary>
        /// パスが同じか、もしくはサブパスかどうかを判定する。
        /// 大文字小文字は区別する。
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        public static bool EqualsOrSubPath(string path1, string path2)
        {
            return EqualsPath(path1, path2) || IsSubPath(path1, path2);
        }
    }
}
