using System.Collections.Generic;
using System.Linq;
using Crash.Pages.Model.WebPage.Parts.Content;

namespace Crash.Pages.Model.WebPage.Parts
{
    public sealed class Sidebar
    {
        /// <summary>
        /// タグ情報一覧
        /// </summary>
        public List<TagInfo> TagInfos { get; }

        public Sidebar(IReadOnlyList<ArticleContent> articlePageList)
        {
            TagInfos = articlePageList
                .SelectMany(page => page.Tags)
                .GroupBy(tagName => tagName)
                .Select(group => new TagInfo(group.Key, group.Count()))
                .OrderByDescending(tagInfo => tagInfo.Count)
                .ThenBy(tagInfo => tagInfo.Name)
                .ToList();
        }

        /// <summary>
        /// タグ情報
        /// </summary>
        public sealed class TagInfo
        {
            /// <summary>
            /// タグ名
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// 記事数
            /// </summary>
            public int Count { get; }

            public TagInfo(string name, int count)
            {
                Name = name;
                Count = count;
            }
        }
    }
}
