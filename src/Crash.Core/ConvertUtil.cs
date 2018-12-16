using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crash.Core.Collections;

namespace Crash.Core
{
    public static class ConvertUtil
    {
        public static string BytesToHexString(byte[] buffer)
        {
            return string.Concat(buffer.Select(b => b.ToString("x2")));
            // 以下の方法でも可
            //return BitConverter.ToString(buffer).ToLower().Replace("-", string.Empty);
        }

        public static byte[] HexStringToBytes(string hexString)
        {
            return hexString
                .SelectBuffer((highChar, lowChar) => Convert.ToByte($"{highChar}{lowChar}", 16))
                .ToArray();
        }
    }
}
