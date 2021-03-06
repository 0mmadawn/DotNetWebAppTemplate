using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace web.test.TestUtils
{
    internal static class ValidationUtils
    {
        //https://stackoverflow.com/a/4331964
        internal static IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }

        internal static bool ContainsKey(
            this IList<ValidationResult> validationResults,
            string key
        ) => validationResults.Any(x => x.MemberNames.Contains(key));
    }
}
