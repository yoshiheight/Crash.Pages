using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Crash.Core.Diagnostics
{
    public static class DebugUtil
    {
        public const string DebugSymbol = "DEBUG";

        [Conditional(DebugUtil.DebugSymbol)]
        public static void DoDebug(Action action)
        {
            action();
        }
    }
}
