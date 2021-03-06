using System;
using System.ComponentModel.DataAnnotations;

namespace web.Common.Enums
{
    /// <summary>組織</summary>
    public enum Group: byte
    {
        [Display(Name = "エゥーゴ")]
        Aeug   = 1,
        [Display(Name = "ティターンズ")]
        Titans = 1 << 1,
        [Display(Name = "地球連邦軍")]
        Efsf   = 1 << 2,
        [Display(Name = "アクシズ")]
        Axis   = 1 << 3,
    }
}
