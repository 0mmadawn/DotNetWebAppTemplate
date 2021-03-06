using System.Collections.Generic;
using web.Models.Entities;

namespace web.ViewModels.Users
{
    public class IndexViewModel: UserViewModelBase
    {
        public IEnumerable<User> Users { get; set; }

        public IndexViewModel()
        {
            H1 = "Index";
        }
    }
}
