using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using web.Common.Enums;

namespace web.Models.Parameters
{
    public class UserParamModel
    {
        [BindRequired]
        [Range(1, 10)]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [EnumDataType(typeof(Group))]
        public Group Group { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }
    }
}
