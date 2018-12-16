using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Crash.Core.Threading
{
    /// <summary>
    /// 簡易ミューテックス
    /// </summary>
    public sealed class MutexSlim : IDisposable
    {
        /// <summary>ミューテックス</summary>
        private Mutex _mutex;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mutex">ミューテックス</param>
        private MutexSlim(Mutex mutex)
        {
            _mutex = mutex;
        }

        /// <summary>
        /// 破棄処理
        /// </summary>
        public void Dispose()
        {
            if (_mutex != null)
            {
                _mutex.ReleaseMutex();
                _mutex.Dispose();
                _mutex = null;
            }
        }

        /// <summary>
        /// 簡易ミューテックスの生成
        /// </summary>
        /// <param name="name">ミューテックス名</param>
        /// <param name="createdMutexSlim">生成した簡易ミューテックスの格納先</param>
        /// <returns></returns>
        public static bool TryCreate(string name, out MutexSlim createdMutexSlim)
        {
            var mutex = new Mutex(true, name, out var isCreatedNew);
            if (!isCreatedNew)
            {
                mutex.Dispose();
                createdMutexSlim = null;
                return false;
            }

            createdMutexSlim = new MutexSlim(mutex);
            return true;
        }
    }
}
