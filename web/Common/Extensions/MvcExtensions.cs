using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace web.Common.Extensions
{
    /// <summary>.NETCORE MVC関連の拡張メソッドクラス</summary>
    public static class MvcExtensions
    {
        /// <summary>ModelStateをフィールド名とエラーの辞書型に変換</summary>
        /// <param name="modelState">ModelState</param>
        /// <returns>辞書型(Key:フィールド名, Value:エラー)</returns>
        public static Dictionary<string, List<string>> ToErrorDictionary(this ModelStateDictionary modelState)
            => modelState
                .Where(x => x.Value.Errors.Any())
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => modelState[kvp.Key].Errors.Select(e => e.ErrorMessage).ToList()
                );

        /// <summary>ModelStateに指定したキーにおけるエラーがあるかどうか</summary>
        /// <param name="modelState">ModelState</param>
        /// <param name="key">キー</param>
        /// <returns>エラーの有無</returns>
        public static bool HasError(this ModelStateDictionary modelState, string key)
            => modelState
                    .Where(x => x.Value.Errors.Any())
                    .Select(x => x.Key)
                    .Contains(key);
    }
}
