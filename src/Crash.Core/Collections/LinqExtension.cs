using System;
using System.Collections.Generic;
using System.Linq;

namespace Crash.Core.Collections
{
    /// <summary>
    /// LINQ拡張
    /// </summary>
    public static class LinqExtension
    {
        public static IEnumerable<T> AsSingleEnumerable<T>(this T source)
        {
            yield return source;
        }

        /// <summary>
        /// シーケンスの要素のバイト数合計が指定値以内となる様に、シーケンスを分割する
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="sizeSelector"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        public static IEnumerable<IReadOnlyList<TSource>> Divide<TSource>(this IEnumerable<TSource> source, Func<TSource, long> sizeSelector, long maxSize)
        {
            long totalSize = 0;
            var list = new List<TSource>();
            foreach (var elem in source)
            {
                var size = sizeSelector(elem);

                if (list.Any() && (totalSize + size) > maxSize)
                {
                    yield return list;

                    totalSize = 0;
                    list = new List<TSource>();
                }

                list.Add(elem);
                totalSize += size;
            }

            if (list.Any())
            {
                yield return list;
            }
        }

        /// <summary>
        /// シーケンスの各要素にインデックス値を付加する
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<(int index, TSource value)> Indexed<TSource>(this IEnumerable<TSource> source)
        {
            return source.Select((value, index) => (index, value));
        }

        /// <summary>
        /// シーケンス内でキーが重複する要素をグループ化して抽出する
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<IGrouping<TKey, TSource>> Duplicate<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source
                .GroupBy(keySelector)
                .Where(group => group.Skip(1).Any());
        }

        /// <summary>
        /// シーケンス内で重複するキーを抽出する
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TKey> DuplicateKey<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source
                .GroupBy(keySelector)
                .Where(group => group.Skip(1).Any())
                .Select(group => group.Key);
        }

        /// <summary>
        /// 指定されたセレクタを使用して要素を2個づつ処理する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> SelectBuffer<T, TResult>(this IEnumerable<T> source, Func<T, T, TResult> selector)
        {
            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return selector(enumerator.Current, MoveNextAndGet(enumerator));
                }
            }
        }

        /// <summary>
        /// 指定されたセレクタを使用して要素を3個づつ処理する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> SelectBuffer<T, TResult>(this IEnumerable<T> source, Func<T, T, T, TResult> selector)
        {
            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return selector(enumerator.Current, MoveNextAndGet(enumerator), MoveNextAndGet(enumerator));
                }
            }
        }

        /// <summary>
        /// 列挙子の現在位置を進め、現在値を取得する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerator"></param>
        /// <returns></returns>
        private static T MoveNextAndGet<T>(IEnumerator<T> enumerator)
        {
            if (!enumerator.MoveNext())
            {
                throw new InvalidOperationException();
            }
            return enumerator.Current;
        }
    }
}
