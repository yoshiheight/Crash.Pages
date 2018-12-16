using System;
using System.Text;
using Crash.Core.Text;

namespace Crash.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class GuidUtil
    {
        /// <summary>
        /// GUID文字列を生成する
        /// </summary>
        /// <returns></returns>
        public static string NewNormalGuidString()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
