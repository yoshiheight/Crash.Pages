using System.IO;
using System.Linq;
using Crash.Core.Collections;

namespace Crash.Core.IO
{
    /// <summary>
    /// パスユーティリティー
    /// </summary>
    public static class PathExtension
    {
        /// <summary>
        /// ディレクトリパスにファイルパスを連結する
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static FileInfo CombineFilePath(this DirectoryInfo dir, params string[] paths)
        {
            return new FileInfo(PathUtil.CombineToFullPath(dir.FullName, paths));
        }

        /// <summary>
        /// ディレクトリパスにサブディレクトリパスを連結する
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static DirectoryInfo CombineDirectoryPath(this DirectoryInfo dir, params string[] paths)
        {
            return new DirectoryInfo(PathUtil.CombineToFullPath(dir.FullName, paths));
        }
    }
}
