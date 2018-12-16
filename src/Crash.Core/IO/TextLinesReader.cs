using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Crash.Core.IO
{
    /// <summary>
    /// テキストの解析読み込みを行うクラス。
    /// StreamReaderだと「改行コードの種類が識別できない」「ReadLineで末尾の改行を認識できない」といった機能不足があるので、その改善用。
    /// </summary>
    public sealed class TextLinesReader : IDisposable
    {
        /// <summary>
        /// 読み込み元
        /// </summary>
        private TextReader _reader;

        private HashSet<string> _newLineSet = new HashSet<string>();

        public IReadOnlyCollection<string> NewLines => _newLineSet;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TextLinesReader(TextReader reader)
        {
            _reader = reader;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> ReadLines()
        {
            var buffer = new StringBuilder();
            while (IOUtil.ReadChar(_reader, out var ch, out var newLine))
            {
                if (newLine != null)
                {
                    _newLineSet.Add(newLine);

                    yield return buffer.ToString();
                    buffer.Clear();
                }
                else
                {
                    buffer.Append(ch);
                }
            }

            yield return buffer.ToString();
        }

        public void ReadAllToWriter(TextWriter writer, string writeNewLine)
        {
            while (IOUtil.ReadChar(_reader, out var ch, out var newLine))
            {
                if (newLine != null)
                {
                    writer.Write(writeNewLine);
                }
                else
                {
                    writer.Write(ch);
                }
            }
        }

        /// <summary>
        /// 後処理
        /// </summary>
        public void Dispose()
        {
            if (_reader != null)
            {
                _reader.Dispose();
                _reader = null;

                _newLineSet.Clear();
                _newLineSet = null;
            }
        }
    }
}
