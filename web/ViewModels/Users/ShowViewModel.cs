using web.Models.Entities;

namespace web.ViewModels.Users
{
    public class ShowViewModel : UserViewModelBase
    {
        public User User { get; set; }

        public ShowViewModel()
        {
            H1 = "Show";
        }
    }
}
