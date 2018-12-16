// 検索用JavaScript

// 検索クラス
class PagesSearch {
    // コンストラクタ
    constructor(indexJsonUrlFormat, searchHitCallback, searchCompleteCallback) {
        this.indexJsonUrlFormat = indexJsonUrlFormat; // インデックス用JSONファイルのURL書式
        this.searchHitCallback = searchHitCallback; // 検索がヒットする毎に呼び出すコールバック関数
        this.searchCompleteCallback = searchCompleteCallback; // 検索が完了した際に呼び出すコールバック関数

        this.keywords = []; // 検索ワード配列
        this.tagName = ""; // 対象タグ
        this.indexFileNo = 0; // インデックス用JSONファイルの番号
        this.hitCount = 0; // 検索結果件数
    }

    // 検索実行
    search(queryString, tagName) {
        this.keywords = queryString.length > 0 ? queryString.toLowerCase().split(" ") : [];
        this.tagName = tagName;

        this.searchJson();
    }

    // インデックスJSONを取得して検索する
    searchJson() {
        let jsonUrl = this.indexJsonUrlFormat.replace("{0}", this.indexFileNo);
        $.getJSON(jsonUrl, indexDatas => {
            let len = indexDatas.length;
            for (let i = 0; i < len; i++) {
                // インデックスの終端の場合
                if (indexDatas[i].title.length === 0) {
                    this.searchCompleteCallback(this.hitCount);
                    return;
                }

                // 検索ワードを文字列検索
                let searchHitItem = this.searchKeyword(indexDatas[i]);

                // 見つかった場合
                if (searchHitItem !== null) {
                    this.hitCount++;
                    this.searchHitCallback(indexDatas[i], searchHitItem);
                }
            }

            // 次のJSONの検索へ
            this.indexFileNo++;
            this.searchJson();
        });
    }

    // 検索ワードを文字列検索する（検索ワードが複数の場合はAND検索）
    searchKeyword(indexData) {
        if (this.tagName.length > 0 && !this.isMatchTag(indexData, this.tagName)) {
            return null;
        }

        let searchHitItem = new SearchHitItem(this.hitCount);

        let keywordsLen = this.keywords.length;
        if (keywordsLen > 0) {
            const plainText = indexData.plainText.toLowerCase();
            for (let i = 0; i < keywordsLen; i++) {
                let keyword = this.keywords[i];

                let contentHitIndex = -1;

                if (indexData.title.toLowerCase().indexOf(keyword) !== -1 || this.containsTagSearchWord(indexData, keyword)) {
                    contentHitIndex = 0;
                }
                else {
                    contentHitIndex = plainText.indexOf(keyword);
                }

                if (contentHitIndex === -1) {
                    return null;
                }

                // コンテンツで最初にヒットした文字列位置を保持
                if (searchHitItem.contentHitIndex === -1) {
                    searchHitItem.contentHitIndex = contentHitIndex;
                }
            }
        }

        return searchHitItem;
    }

    isMatchTag(indexData, tagName) {
        let tagsLen = indexData.tags.length;
        for (let i = 0; i < tagsLen; i++) {
            if (indexData.tags[i] === tagName) {
                return true;
            }
        }
        return false;
    }

    containsTagSearchWord(indexData, keyword) {
        let tagsLen = indexData.tags.length;
        for (let i = 0; i < tagsLen; i++) {
            if (indexData.tags[i].toLowerCase().indexOf(keyword) !== -1) {
                return true;
            }
        }
        return false;
    }
}

// 検索結果1件分を表すクラス
class SearchHitItem {
    // コンストラクタ
    constructor(no) {
        this.no = no;
        this.contentHitIndex = -1; // コンテンツで最初にヒットした文字列位置
    }
}
