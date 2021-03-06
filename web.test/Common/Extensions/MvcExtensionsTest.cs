using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using web.Common.Extensions;
using Xunit;

namespace web.test.Common.Extensions
{
    public class MvcExtensionsTest
    {
        //もう少しましな書き方があるはず...

        public static IEnumerable<object[]> CreateValidModelState()
        {
            //エラーなし
            var modelStateValid = new ModelStateDictionary();
            return new List<object[]>() { new object[]{ modelStateValid } };
        }

        public static IEnumerable<object[]> CreateInvalidModelState()
        {
            var keyId = "Id";
            var keyTitle = "Title";

            // Error: Title
            var modelStateInValid_Title = new ModelStateDictionary();
            modelStateInValid_Title.AddModelError(keyTitle, "SomeError");
            // Error: Id
            var modelStateInValid_Id = new ModelStateDictionary();
            modelStateInValid_Id.AddModelError(keyId, "SomeError");
            // Error: Title *2
            var modelStateInValid_Title_two = new ModelStateDictionary();
            modelStateInValid_Title_two.AddModelError(keyTitle, "SomeError");
            modelStateInValid_Title_two.AddModelError(keyTitle, "SomeError2");
            // Erro: Id, Title
            var modelStateInValid_Title_Id = new ModelStateDictionary();
            modelStateInValid_Title_Id.AddModelError(keyId, "SomeError");
            modelStateInValid_Title_Id.AddModelError(keyTitle, "SomeError");
            return new List<object[]>()
            {
                new object[]{ modelStateInValid_Title, new List<string> { keyTitle } },
                new object[]{ modelStateInValid_Id, new List<string> { keyId } },
                new object[]{ modelStateInValid_Title_two, new List<string> { keyTitle } },
                new object[]{ modelStateInValid_Title_Id, new List<string> { keyId, keyTitle } },
            };
        }

        [Theory]
        [MemberData(nameof(CreateValidModelState))]
        public void ToErrorDictionaryTest_正常系_エラーなし(ModelStateDictionary modelState)
        {
            var dict = modelState.ToErrorDictionary();
            Assert.IsType<Dictionary<string, List<string>>>(dict);
            Assert.Equal(modelState.ErrorCount, dict.Count);
        }

        [Theory]
        [MemberData(nameof(CreateInvalidModelState))]
        public void ToErrorDictionaryTest_正常系_エラーあり(ModelStateDictionary modelState, IEnumerable<string> keys)
        {
            var dict = modelState.ToErrorDictionary();
            Assert.IsType<Dictionary<string, List<string>>>(dict);
            Assert.Equal(modelState.ErrorCount, dict.SelectMany(x=>x.Value).Count());
        }

        [Theory]
        [MemberData(nameof(CreateInvalidModelState))]
        public void HasError_正常系_エラーあり(ModelStateDictionary modelState, List<string> keys)
            => keys.ForEach(key => Assert.True(modelState.HasError(key)));
    }
}
