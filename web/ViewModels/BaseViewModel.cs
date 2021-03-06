namespace web.Models.ViewModels
{
    // _Layout.cshtml用のフィールドはここにまとめる気持ち
    // 必要に応じてクラス切り出して合成
    // Meta情報とか, ヘッダフッタとか

    /// <summary>ViewModel基底クラス</summary>
    public class BaseViewModel
    {
        public string Title { get; set; }
        public string H1 { get; set; }
    }
}
