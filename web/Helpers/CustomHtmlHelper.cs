using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace web.Helpers
{
    /// <summary>HTML Helper</summary>
    public static class CustomHtmlHelper
    {
        /// <summary>EnumからIEnumerable<SelectListItem>を生成</summary>
        /// <typeparam name="TEnum">Enum</typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="selectedValue">選択中のEnumの値(byte)</param>
        /// <returns>IEnumerable<SelectListItem></returns>
        // <remarks>HtmlHelperのGetEnumSeletListにselectedをもたせるためのメソッド</remarks>
        public static IEnumerable<SelectListItem>
            GetEnumSelectList<TEnum>(this IHtmlHelper htmlHelper, byte selectedValue)
            where TEnum : struct, Enum
        {
            var list = htmlHelper.GetEnumSelectList<TEnum>();
            list.ToList().ForEach(x =>
                    x.Selected = x.Value == selectedValue.ToString()
            );
            return list;
        }
    }
}
