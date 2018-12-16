using System;
using System.Collections.Generic;

namespace Crash.Core
{
    /// <summary>
    /// Enumユーティリティー
    /// </summary>
    public static class EnumUtil
    {
        /// <summary>
        /// 指定したEnum型の値を列挙する
        /// </summary>
        /// <typeparam name="TEnum">Enum型</typeparam>
        /// <returns>指定したEnum型の全ての値</returns>
        public static IEnumerable<TEnum> GetValues<TEnum>() where TEnum : Enum
        {
            return (IEnumerable<TEnum>)Enum.GetValues(typeof(TEnum));
        }
    }
}
