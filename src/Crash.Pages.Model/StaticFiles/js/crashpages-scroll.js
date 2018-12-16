// 検索用JavaScript

// 検索クラス
class PagesRestoreScroll {
    // コンストラクタ
    constructor(sessionKey) {
        this.sessionKey = sessionKey;

        $(window).on("scroll", CrashUtil.debounce(100, () => {
            let scrollTop = $(window).scrollTop();
            sessionStorage.setItem(this.sessionKey, scrollTop);
        }));
    }

    // 前回のスクロール位置を復元する
    restore() {
        let performNavi = window.performance.navigation;
        if (performNavi.type === performNavi.TYPE_RELOAD || performNavi.type === performNavi.TYPE_BACK_FORWARD) {
            let scrollTop = sessionStorage.getItem(this.sessionKey);
            $(window).scrollTop(scrollTop);
        }
    }
}
