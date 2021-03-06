using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace web.Common.Extensions
{
    /// <summary>Enum拡張メソッドクラス</summary>
    public static class EnumExtensions
    {
        /// <summary>Display属性を取得</summary>
        /// <param name="value">Enum</param>
        /// <returns>Display属性の設定値</returns>
        /// <remarks>Display属性が設定されていなければ空文字を返す</remarks>
        public static string DisplayName(this Enum value)
        {
            //Enumに存在しない値の場合エラー
            if (!Enum.IsDefined(value.GetType(), value))
            {
                throw new ArgumentException($"{value}はEnum値ではありません");
            }

            var filedInfo = value.GetType().GetField(value.ToString());
            var dispAttr = filedInfo.GetCustomAttribute<DisplayAttribute>();
            if (dispAttr is null) {
                throw new ArgumentException($"{value}にDisplayAttributeが設定されていません");
            }
            return dispAttr.GetName();
        }
    }
}
