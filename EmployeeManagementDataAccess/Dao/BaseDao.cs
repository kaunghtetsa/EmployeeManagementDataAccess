using System;
using System.Transactions;
using ASM.EmployeeManagement.Common.Logger;
using ASM.EmployeeManagement.DataAccess.Common.ExecutionStrategy;
using ASM.EmployeeManagement.DataAccess.Model;

namespace ASM.EmployeeManagement.DataAccess.Dao
{
    public abstract class BaseDao
    {
        #region private static variables

        /// <summary>
        /// Connection string
        /// </summary>
        private static string _dbConnString;

        private static int _dbConnectionRetryCount = 0;

        private static int _dbConnectionRetryIntervalSec = 0;

        #endregion

        #region constant

        /// <summary>
        /// Database command execution timeout (in second)
        /// </summary>
        public const int DBCommandExecutionTimeout = 60;

        #endregion

        #region public methods

        /// <summary>
        /// SetConnectionString
        /// </summary>
        /// <param name="dbConnString"></param>
        public static void SetConnectionString(string dbConnString)
        {
            _dbConnString = dbConnString;
        }

        /// <summary>
        /// IsInitialized
        /// </summary>
        /// <returns></returns>
        protected static bool IsInitialized()
        {
            return !string.IsNullOrEmpty(_dbConnString);
        }

        /// <summary>
        /// InitializeRetrySetting
        /// </summary>
        public static void InitializeRetrySetting()
        {
            _dbConnectionRetryCount = 5;
            _dbConnectionRetryIntervalSec = 1;
        }

        /// <summary>
        /// DBをアクセスできるかどうか
        /// </summary>
        /// <returns></returns>
        public static bool CanAccessDB()
        {
            try
            {
                using (EmployeeEntities entities = GetEmployeeEntities())
                {
                    return entities.Database.Exists();
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.Error(null, ex);
            }
            return false;
        }

        /// <summary>
        /// Get EmployeeEntities
        /// </summary>
        /// <returns></returns>
        public static EmployeeEntities GetEmployeeEntities()
        {
            EmployeeEntities context = new EmployeeEntities(_dbConnString);
            context.Database.CommandTimeout = DBCommandExecutionTimeout;
            return context;
        }

        /// <summary>
        /// Gets TransactionScope with ReadUncommitted isolation level
        /// </summary>
        /// <returns>object of TransactionScope</returns>
        public static TransactionScope GetReadUncommittedTransactionScope()
        {
            TransactionScope objTrans = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                });
            return objTrans;
        }

        /// <summary>
        /// GetEmployeeExecutionStrategy
        /// </summary>
        /// <returns></returns>
        public static EmployeeExecutionStrategy GetEmployeeExecutionStrategy()
        {
            int nRetryCount = GetDBConnectionRetryCount();
            int nRetryIntervalSec = GetDBConnectionRetryIntervalSec();

            return new EmployeeExecutionStrategy(nRetryCount, TimeSpan.FromSeconds(nRetryIntervalSec));
        }

        #endregion

        #region private methods

        /// <summary>
        /// GetDBConnectionRetryCount
        /// </summary>
        /// <returns></returns>
        private static int GetDBConnectionRetryCount()
        {
            return _dbConnectionRetryCount;
        }

        /// <summary>
        /// GetDBConnectionRetryIntervalSec
        /// </summary>
        /// <returns></returns>
        private static int GetDBConnectionRetryIntervalSec()
        {
            return _dbConnectionRetryIntervalSec;
        }

        #endregion
    }
}
