using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;
using web.Common.Enums;

namespace web.Common.DataAccess
{
    /// <summary>DBConnectionFactoryIF</summary>
    public interface IDbConnectionFactory
    {
        /// <summary>接続文字列を元にDBConnectionを生成</summary>
        /// <param name="connectionName">接続文字列</param>
        /// <returns>IDbConnection</returns>
        IDbConnection CreateDbConnection(ConnectionName connectionName);
    }

    /// <inheritdoc/>
    public class DbConnectionFactory : IDbConnectionFactory
    {
        /// <summary>DB接続文字列の辞書</summary>
        private readonly IReadOnlyDictionary<ConnectionName, string> _connectionDict;

        /// <summary>コンストラクタ</summary>
        /// <param name="connectionDict">DB接続文字列の辞書</param>
        public DbConnectionFactory(IReadOnlyDictionary<ConnectionName, string> connectionDict)
            => _connectionDict = connectionDict;

        /// <inheritdoc/>
        public IDbConnection CreateDbConnection(ConnectionName connectionName)
        {
            if (_connectionDict.TryGetValue(connectionName, out string connectionString))
            {
                return new MySqlConnection(connectionString);
            }
            throw new ArgumentNullException($"指定されたConnectionNameは未定義です: {connectionName}");
        }
    }
}
