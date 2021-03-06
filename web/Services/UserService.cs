using web.Models.Entities;
using web.Models.Parameters;
using web.Repositories;
using web.ViewModels.Users;

namespace web.Services
{
    public interface IUserService
    {
        public IndexViewModel GetIndexViewModel();
        public ShowViewModel  GetShowViewModel(int id);
        public EditViewModel  GetEditViewModel(int id);
        public EditViewModel  GetEditViewModel(UserParamModel param);
        void UpdateUser(UserParamModel param);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository repository;
        public UserService(IUserRepository repository)
        {
            this.repository = repository;
        }

        public IndexViewModel GetIndexViewModel()
        {
            repository.LoadAllUsers();
            var users = repository.Users;
            var model = new IndexViewModel();
            model.H1 = "Users";
            model.Users = users;
            return model;
        }

        public ShowViewModel GetShowViewModel(int id)
        {
            repository.LoadUser(id);
            var user = repository.User;
            if (user is null) { return null; }
            var model = new ShowViewModel();
            model.User = user;
            return model;
        }

        public EditViewModel GetEditViewModel(int id)
        {
            repository.LoadUser(id);
            var user = repository.User;
            if (user is null) { return null; }
            var model = new EditViewModel();
            model.User = user;
            return model;
        }

        // Updateから入力エラーでEditに回帰する時用のメソッド
        public EditViewModel GetEditViewModel(UserParamModel param)
        {
            var model = new EditViewModel();
            model.User = new User(param);
            return model;
        }

        public void UpdateUser(UserParamModel param)
            => repository.UpdateUser(new User(param));
    }
}
