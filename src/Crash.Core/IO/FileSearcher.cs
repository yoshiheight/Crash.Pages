using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Crash.Core.IO
{
    /// <summary>
    /// ファイル、ディレクトリの検索クラス。
    /// ディレクトリ探索毎について、ソートとフィルター機能を提供する。
    /// </summary>
    public static class FileSearcher
    {
        /// <summary>
        /// ファイル検索
        /// </summary>
        /// <param name="directory"></param>
        public static IEnumerable<(FileInfo fi, string relativePath)> EnumerateFileInfos(string directory, Predicate<DirectoryInfo> recursiveFilter = null)
        {
            var rootDir = new DirectoryInfo(directory);

            return RecursiveSearch(rootDir, recursiveFilter)
                .OfType<FileInfo>()
                .Select(fi => (fi, PathUtil.GetRelativePath(rootDir.FullName, fi.FullName)));
        }

        public static IEnumerable<(DirectoryInfo di, string relativePath)> EnumerateDirectoryInfos(string directory, Predicate<DirectoryInfo> recursiveFilter = null)
        {
            var rootDir = new DirectoryInfo(directory);

            return RecursiveSearch(rootDir, recursiveFilter)
                .OfType<DirectoryInfo>()
                .Select(di => (di, PathUtil.GetRelativePath(rootDir.FullName, di.FullName)));
        }

        public static IEnumerable<(FileSystemInfo fsi, string relativePath)> EnumerateFileSystemInfos(string directory, Predicate<DirectoryInfo> recursiveFilter = null)
        {
            var rootDir = new DirectoryInfo(directory);

            return RecursiveSearch(rootDir, recursiveFilter)
                .Select(fsi => (fsi, PathUtil.GetRelativePath(rootDir.FullName, fsi.FullName)));
        }

        /// <summary>
        /// 指定ディレクトリ内を再帰的に検索する
        /// </summary>
        /// <param name="targetDir">検索対象ディレクトリ</param>
        private static IEnumerable<FileSystemInfo> RecursiveSearch(DirectoryInfo targetDir, Predicate<DirectoryInfo> recursiveFilter = null)
        {
            foreach (var di in targetDir.EnumerateDirectories().OrderBy(di => di.Name))
            {
                yield return di;

                if (recursiveFilter == null || recursiveFilter(di))
                {
                    foreach (var sub in RecursiveSearch(di, recursiveFilter))
                    {
                        yield return sub;
                    }
                }
            }

            foreach (var fi in targetDir.EnumerateFiles().OrderBy(fi => fi.Name))
            {
                yield return fi;
            }
        }
    }
}
