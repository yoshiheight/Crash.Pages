using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Crash.Core.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public static class AssemblyUtil
    {
        public static string GetEntryDirectoryPath()
        {
            return new FileInfo(Assembly.GetEntryAssembly().Location).Directory.FullName;
        }
    }
}
