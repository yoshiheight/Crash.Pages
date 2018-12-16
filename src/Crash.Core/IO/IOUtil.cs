using System;
using System.IO;
using System.Text;

namespace Crash.Core.IO
{
    /// <summary>
    /// IOユーティリティー
    /// </summary>
    public static class IOUtil
    {
        /// <summary>
        /// テキストファイルを読み込む。
        /// 改行コードはCRLF、CR、LFのどれでも可。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encode"></param>
        /// <returns>読み込んだ文字列</returns>
        public static string ReadAllText(string path, Encoding encode, string afterNewLine)
        {
            var sb = new StringBuilder();
            using (var reader = new TextLinesReader(new StreamReader(path, encode)))
            using (var writer = new StringWriter(sb))
            {
                reader.ReadAllToWriter(writer, afterNewLine);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="text"></param>
        /// <param name="encode"></param>
        public static void WriteAllText(string path, string text, Encoding encode, string newLine)
        {
            var fileInfo = new FileInfo(path);
            fileInfo.Directory.Create();

            using (var reader = new TextLinesReader(new StringReader(text)))
            using (var writer = new StreamWriter(fileInfo.FullName, false, encode))
            {
                reader.ReadAllToWriter(writer, newLine);
            }
        }

        /// <summary>
        /// テキストファイルをコピーする。エンコード変換、改行コード変換。タイムスタンプは同じにしない。
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="srcEncode"></param>
        /// <param name="destPath"></param>
        /// <param name="destEncode"></param>
        /// <param name="newLine"></param>
        public static void CopyTextFileWithEncodeEol(string srcPath, Encoding srcEncode, string destPath, Encoding destEncode, string newLine)
        {
            if (PathUtil.EqualsPath(srcPath, destPath))
            {
                throw new IOException();
            }

            using (var reader = new TextLinesReader(new StreamReader(srcPath, srcEncode)))
            using (var writer = new StreamWriter(destPath, false, destEncode))
            {
                reader.ReadAllToWriter(writer, newLine);
            }
        }

        public static void ThrowIfDirectoryNotFound(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException(directory + " ディレクトリが見つかりません。");
            }
        }

        /// <summary>
        /// ファイルをコピーする
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="destPath"></param>
        /// <param name="options"></param>
        public static void CopyFile(string srcPath, string destPath, FileCopyOptions options)
        {
            if (PathUtil.EqualsPath(srcPath, destPath))
            {
                throw new IOException();
            }

            var srcFile = new FileInfo(srcPath);
            var destFile = new FileInfo(destPath);

            if (options.HasFlag(FileCopyOptions.Update)
                && destFile.Exists
                && srcFile.Length == destFile.Length
                && srcFile.LastWriteTimeUtc == destFile.LastWriteTimeUtc)
            {
                return;
            }

            destFile.Directory.Create();
            srcFile.CopyTo(destFile.FullName, true);
        }

        /// <summary>
        /// ディレクトリコピー。空のディレクトリはコピーしない。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        public static void CopyDirectory(string source, string dest, FileCopyOptions options)
        {
            if (PathUtil.EqualsOrSubPath(source, dest))
            {
                throw new IOException();
            }

            foreach (var item in FileSearcher.EnumerateFileInfos(source))
            {
                CopyFile(item.fi.FullName, PathUtil.CombineToFullPath(dest, item.relativePath), options);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="timeoutMilliseconds"></param>
        public static void DeleteDirectory(string target, int timeoutMilliseconds = 0)
        {
            Directory.Delete(target, true);

            CoreUtil.Retry(timeoutMilliseconds, 100,
                () =>
                {
                    if (Directory.Exists(target))
                    {
                        throw new IOException();
                    }
                },
                () => throw new IOException(target + " ディレクトリは削除できませんでした。"));
        }

        public static void ClearDirectory(string target, int timeoutMilliseconds = 0)
        {
            DeleteDirectory(target, timeoutMilliseconds);
            Directory.CreateDirectory(target);
        }

        public static bool ReadChar(TextReader reader, out char ch)
        {
            var code = reader.Read();
            if (code != -1)
            {
                ch = (char)code;
                return true;
            }
            ch = CharUtil.Null;
            return false;
        }

        public static bool ReadChar(TextReader reader, out char ch, out string newLine)
        {
            newLine = null;

            if (ReadChar(reader, out ch))
            {
                if (ch == CharUtil.Lf)
                {
                    newLine = StringUtil.Lf;
                }
                else if (ch == CharUtil.Cr)
                {
                    var code = reader.Peek();
                    if (code != -1 && (char)code == CharUtil.Lf)
                    {
                        ch = (char)reader.Read();
                        newLine = StringUtil.CrLf;
                    }
                    else
                    {
                        newLine = StringUtil.Cr;
                    }
                }

                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// ファイルコピーオプション
    /// </summary>
    [Flags]
    public enum FileCopyOptions
    {
        None = 0x00,

        /// <summary>
        /// ファイルサイズもしくは最終更新日時に相違がある場合のみコピーする
        /// </summary>
        Update = 0x01,
    }
}
