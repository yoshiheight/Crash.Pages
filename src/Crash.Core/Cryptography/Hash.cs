using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Crash.Core.Cryptography
{
    /// <summary>
    /// ハッシュを計算するクラス
    /// </summary>
    public class Hash : IDisposable
    {
        private HashAlgorithm _hashAlgorithm;

        private bool _finaled;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="hashAlgorithm">ハッシュアルゴリズム</param>
        public Hash(HashAlgorithm hashAlgorithm)
        {
            _hashAlgorithm = hashAlgorithm;
        }

        public byte[] HashCode
        {
            get
            {
                UpdateFinal();
                return _hashAlgorithm.Hash;
            }
        }

        public string HashString => ConvertUtil.BytesToHexString(HashCode);

        public void Update(byte[] buffer, int offset, int length)
        {
            if (_finaled)
            {
                throw new InvalidOperationException();
            }
            _hashAlgorithm.TransformBlock(buffer, offset, length, null, 0);
        }

        public void Update(byte[] buffer)
        {
            Update(buffer, 0, buffer.Length);
        }

        private void UpdateFinal()
        {
            if (!_finaled)
            {
                _finaled = true;
                _hashAlgorithm.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
            }
        }

        public void Dispose()
        {
            _hashAlgorithm.Dispose();
        }
    }
}

#if false
            // 例
            using (var dlg = new OpenFileDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                using (var hash = new Hash(MD5.Create()))
                {
                    var buffer = new byte[1024 * 1024 * 1024];
                    using (var reader =  new FileStream(dlg.FileName, FileMode.Open, FileAccess.Read))
                    {
                        int readLen;
                        while ((readLen = reader.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            hash.Update(buffer, 0, readLen);
                        }
                    }
                    //textBox1.AppendText($"{hash.HashString} : {dlg.FileName}");
                    textBox1.Text = $"{hash.HashString} : {dlg.FileName}";
                }
            }
#endif