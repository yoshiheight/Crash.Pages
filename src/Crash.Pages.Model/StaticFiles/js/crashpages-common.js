// 共通用JavaScript

class CrashUtil {
    // クエリ文字列をマップとして取得する
    static getQueryStringMap() {
        let map = new Map();

        let queries = window.location.search.substr(1).split("&");
        let len = queries.length;
        for (let i = 0; i < len; i++) {
            let keyvalue = queries[i].split("=");
            map.set(keyvalue[0], keyvalue[1]);
        }

        return map;
    }

    // 検索ワード用クエリ文字列をURIデコード及び正規化する
    static decodeSearchQueryString(searchQueryString) {
        if (typeof searchQueryString === "undefined" || searchQueryString === null) {
            return "";
        }

        searchQueryString = decodeURIComponent(searchQueryString.replace(/\+/g, "%20"));
        searchQueryString = searchQueryString.replace(/\s+/g, " ");
        searchQueryString = searchQueryString.trim();

        return searchQueryString;
    }

    // debounce
    static debounce(interval, callbackFunc) {
        let timerId = null;
        return () => {
            if (timerId !== null) {
                window.clearTimeout(timerId);
            }
            timerId = window.setTimeout(() => {
                timerId = null;
                callbackFunc();
            }, interval);
        };
    }

    // throttle
    static throttle(interval, callbackFunc) {
        let timerId = null;
        return () => {
            if (timerId === null) {
                timerId = window.setTimeout(() => {
                    timerId = null;
                    callbackFunc();
                }, interval);
            }
        };
    }

    // 対象エレメント内のテキストをクリップボードにコピーする
    static copyElement(targetElement) {
        let range = document.createRange();
        range.selectNode(targetElement);

        let selection = window.getSelection();
        selection.removeAllRanges();
        selection.addRange(range);

        document.execCommand("copy");

        selection.removeAllRanges();
    }

    // 目次作成
    static createToc(selectorForContent, findSelectorForHeading, selectorForToc) {
        let tocRoot = $("<ul></ul>", {});
        $(selectorForToc).append(tocRoot);

        $(selectorForContent).find(findSelectorForHeading).each((index, element) => {
            let headingId = "toc-heading-" + index;
            let itemId = headingId + "-item";
            let itemClass = "toc-item-" + $(element).get(0).tagName.toLowerCase();
            let linkText = $(element).text();

            $(element).attr("id", headingId);

            let tocItem = $("<li></li>", { "id": itemId, "class": itemClass });
            tocRoot.append(tocItem);

            let tocLink = $("<a></a>", { "href": "javascript:void(0);" }).text(linkText);
            tocLink.on("click", () => {
                $(window).scrollTop($(element).offset().top);
            });
            tocItem.append(tocLink);
        });

        $(window).on("scroll", this.throttle(100, () => {
            const classNameForSelectedTocItem = "selected-toc-item";
            tocRoot.find("li").removeClass(classNameForSelectedTocItem);

            if ($(selectorForToc).is(":hidden")) {
                return;
            }

            let scrollTop = $(window).scrollTop();

            $($(selectorForContent).find(findSelectorForHeading).get().reverse()).each((index, element) => {
                let headingTop = $(element).offset().top;
                let headingHeight = $(element).height();

                if (scrollTop >= headingTop - headingHeight / 2) {
                    let headingId = $(element).attr("id");

                    let targetItem = $("#" + headingId + "-item");
                    targetItem.addClass(classNameForSelectedTocItem);
                    return false;
                }
            });
        }));
    }
}
