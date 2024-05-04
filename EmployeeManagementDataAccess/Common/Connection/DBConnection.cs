using System.Xml.Linq;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;

using ASM.EmployeeManagement.Common.Logger;
using ASM.EmployeeManagement.DataAccess.Exception;

namespace ASM.EmployeeManagement.DataAccess.Common.Connection
{
    /// <summary>
    /// DBConnection
    /// </summary>
    public class DBConnection
    {
        #region Define
        private const string TagDBConnectionSetting = "DatabaseConnection";
        private const string TagDBServer = "Server";
        private const string TagDBUserName = "UserID";
        private const string TagDBPassword = "Password";
        private const string TagDBDatabaseName = "Database";
        #endregion

        #region private
        private static string _connectionString;
        #endregion 

        #region Public
        /// <summary>
        /// 設定ファイルからConnectionStringを取得する
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string Configure(string filePath)
        {

            try
            {
                DBConnectionInfo info = GetDBConnectionData(filePath);
                _connectionString = CreateConnectionString(info);
            }
            catch (System.Exception ex)
            {
                // XMLファイル読み込み失敗
                string msg = string.Format("Fail to configure Database. ({0})", filePath);
                LogHelper.Error(null, msg, ex);
                throw new DaoException(DaoException.ErrorID.E999, ex);
            }

            return _connectionString;
        }

        /// <summary>
        /// Get Connection String
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString()
        {
            return _connectionString;
        }

        /// <summary>
        /// 設定ファイルから情報を読んでオブジェクトを作成する。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DBConnectionInfo GetDBConnectionData(string filePath)
        {
            try
            {
                XElement rootElement = XElement.Load(filePath);
                XElement element = rootElement.Element(TagDBConnectionSetting);

                DBConnectionInfo dbConnData = new DBConnectionInfo();

                dbConnData.DBServer = element.Element(TagDBServer).Value;
                if (string.IsNullOrWhiteSpace(dbConnData.DBServer))
                {
                    throw new System.Exception();
                }

                dbConnData.DBName = element.Element(TagDBDatabaseName).Value;
                if (string.IsNullOrWhiteSpace(dbConnData.DBName))
                {
                    throw new System.Exception();
                }

                dbConnData.UserId = element.Element(TagDBUserName).Value;
                if (string.IsNullOrWhiteSpace(dbConnData.UserId))
                {
                    throw new System.Exception();
                }

                dbConnData.Password = element.Element(TagDBPassword).Value;
                if (string.IsNullOrWhiteSpace(dbConnData.Password))
                {
                    throw new System.Exception();
                }

                return dbConnData;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// DB接続文字列を作成する。
        /// </summary>
        /// <param name="objDBData"></param>
        /// <returns></returns>
        private static string CreateConnectionString(DBConnectionInfo objDBData)
        {
            try
            {
                // Connection string builder　初期化
                SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

                // 接続初期データを設定する。
                sqlBuilder.MultipleActiveResultSets = true;
                sqlBuilder.ApplicationName = "EntityFramework";

                sqlBuilder.DataSource = objDBData.DBServer;
                sqlBuilder.InitialCatalog = objDBData.DBName;
                sqlBuilder.UserID = objDBData.UserId;
                sqlBuilder.Password = objDBData.Password;
                sqlBuilder.ConnectTimeout = 3600;
                // SqlConnection　作成する。
                string providerString = sqlBuilder.ToString();

                // EntityConnectionStringBuilder初期化
                EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();

                entityBuilder.Provider = "System.Data.SqlClient";
                entityBuilder.ProviderConnectionString = providerString;
                entityBuilder.Metadata = "res://*/";

                return entityBuilder.ToString();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
