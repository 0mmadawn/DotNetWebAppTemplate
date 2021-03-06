using System.Collections.Generic;
using System.Data;
using Dapper;
using web.Models.Entities;

namespace web.DAO
{
    public interface IUserDAO
    {
        public IEnumerable<User> GetAllRecords(IDbConnection dbConnection);
        public User GetRecord(IDbConnection dbConnection, int id);
        public void UpdateRecord(IDbConnection dbConnection, User user);
    }

    public class UserDAO : IUserDAO
    {
        public IEnumerable<User> GetAllRecords(IDbConnection dbConnection)
        {
            var sql = "SELECT U.id, U.name, U.group, U.description FROM Users U ORDER BY U.group, U.name";
            var result = dbConnection.Query<User>(sql);
            return result;
        }

        public User GetRecord(IDbConnection dbConnection, int id)
        {
            var sql = "SELECT U.id, U.name, U.group, U.description FROM Users U WHERE U.id = @Id";
            var result = dbConnection.QueryFirstOrDefault<User>(sql, new { Id = id });
            return result;
        }

        public void UpdateRecord(IDbConnection dbConnection, User user)
        {
            var sql = @"
UPDATE Users U
SET
    U.name = @Name
    ,U.group = @Group
    ,U.description = @Description
WHERE
    U.id = @Id
";
            var param = new {
                Id = user.Id,
                Name = user.Name,
                Group = user.Group,
                Description = user.Description
            };
            dbConnection.Execute(sql, param);
        }
    }
}
