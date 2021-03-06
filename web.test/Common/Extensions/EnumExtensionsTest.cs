using System;
using System.ComponentModel.DataAnnotations;
using web.Common.Extensions;
using Xunit;

namespace web.test.Common.Extensions
{
    public class EnumExtensionsTest
    {
        public EnumExtensionsTest()
        {
        }

        public enum TestEnum: byte
        {
            //valid
            [Display(Name = "テスト1")]
            Test1 = 0,
            [Display(Name = "")]
            Test2 = 1,
            //invalid
            Test3 = 2,
        }

        [Theory]
        [InlineData(TestEnum.Test1, "テスト1")]
        [InlineData(TestEnum.Test2, "")]
        public void DisplayNameTest_正常系(TestEnum value, string expextRes)
            => Assert.Equal(expextRes, value.DisplayName());

        [Theory]
        [InlineData(TestEnum.Test3)]
        [InlineData((TestEnum)4)]
        public void DisplayNameTest_準正常系(TestEnum value)
            => Assert.ThrowsAny<ArgumentException>(() => value.DisplayName());
    }
}
