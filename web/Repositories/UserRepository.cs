using System.Collections.Generic;
using web.Common.DataAccess;
using web.Common.Enums;
using web.DAO;
using web.Models.Entities;

namespace web.Repositories
{
    public interface IUserRepository
    {
        public IEnumerable<User> Users { get; }
        public User User { get; }
        public void LoadAllUsers();
        public void LoadUser(int id);
        public void UpdateUser(User user);
    }

    public class UserRepository : RepositoryBase, IUserRepository
    {
        private readonly IUserDAO dao;

        public UserRepository(IUserDAO userDao, IDbConnectionFactory dbConnectionFactory)
            : base(dbConnectionFactory) => dao = userDao;

        public IEnumerable<User> Users { get; private set; }

        public User User {get; private set;}

        public void LoadAllUsers()
        {
            using (var con = _dbConnectionFactory.CreateDbConnection(ConnectionName.ReadOnlyConnection))
            {
                Users = dao.GetAllRecords(con);
            }
        }

        public void LoadUser(int id)
        {
            using (var con = _dbConnectionFactory.CreateDbConnection(ConnectionName.ReadOnlyConnection))
            {
                User = dao.GetRecord(con, id);
            }
        }

        public void UpdateUser(User user)
        {
            using (var con = _dbConnectionFactory.CreateDbConnection(ConnectionName.WriteConnection))
            {
                dao.UpdateRecord(con, user);
            }
        }
    }
}
