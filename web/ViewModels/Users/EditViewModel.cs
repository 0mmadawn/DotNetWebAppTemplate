using System.Collections.Generic;
using web.Models.Entities;

namespace web.ViewModels.Users
{
    public class EditViewModel : UserViewModelBase
    {
        public User User { get; set; }

        public Dictionary<string, List<string>> Errors { get; set; }
            = new Dictionary<string, List<string>>();

        public EditViewModel()
        {
            H1 = "Edit";
        }
    }
}
