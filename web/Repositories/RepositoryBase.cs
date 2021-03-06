using web.Common.DataAccess;

namespace web.Repositories
{
    /// <summary>Repository層基底クラス</summary>
    public class RepositoryBase
    {
        /// <summary>IDBConnectionFactory</summary>
        protected IDbConnectionFactory _dbConnectionFactory;

        /// <summary>コンストラクタ</summary>
        /// <param name="dbConnectionFactory">IDbConnectionFactory</param>
        public RepositoryBase(IDbConnectionFactory dbConnectionFactory)
            => _dbConnectionFactory = dbConnectionFactory;
    }
}
