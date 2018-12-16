using System;
using System.Diagnostics;
using System.Threading;
using MessagePack;
using Newtonsoft.Json;

namespace Crash.Core
{
    /// <summary>
    /// ユーティリティー
    /// </summary>
    public static class CoreUtil
    {
        public static void Retry(int timeoutMilliseconds, int tick, Action condition, Action timeoutAction = null)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                while (true)
                {
                    try
                    {
                        condition();
                        break;
                    }
                    catch (Exception)
                    {
                    }

                    if (timeoutMilliseconds > 0 && stopwatch.ElapsedMilliseconds >= timeoutMilliseconds)
                    {
                        timeoutAction?.Invoke();
                        break;
                    }

                    Thread.Sleep(tick);
                }
            }
            finally
            {
                stopwatch.Stop();
            }
        }
    }
}
