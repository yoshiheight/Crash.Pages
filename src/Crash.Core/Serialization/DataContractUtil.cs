using System;
using System.Diagnostics;
using System.Threading;
using MessagePack;
using Newtonsoft.Json;

namespace Crash.Core.Serialization
{
    /// <summary>
    /// ユーティリティー
    /// </summary>
    public static class DataContractUtil
    {
        /// <summary>
        /// ディープコピー。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(object obj)
        {
            return MessagePackSerializer.Deserialize<T>(MessagePackSerializer.Serialize(obj));
        }
    }
}
