using System.Linq;

using ASM.EmployeeManagement.Common.Logger;
using ASM.EmployeeManagement.Common.Encryption;
using ASM.EmployeeManagement.DataAccess.Exception;
using ASM.EmployeeManagement.DataAccess.Common.Connection;
using ASM.EmployeeManagement.DataAccess.Model;
using ASM.EmployeeManagement.DataAccess.Common.Enums;
using ASM.EmployeeManagement.DataAccess.Common.ExecutionStrategy;

namespace ASM.EmployeeManagement.DataAccess.Dao.WebAPIUI.Common
{
    /// <summary>
    /// WebAPIUICommonDao
    /// </summary>
    public class WebAPIUICommonDao : BaseDao, IWebAPIUICommonDao
    {
        #region private

        /// <summary>
        /// Initialize flag
        /// </summary>
        private static bool _initialized = false;

        #endregion

        #region public methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public void Initialize(string path)
        {
            if (_initialized == false)
            {
                ReInitialize(path);
                _initialized = true;
            }
        }

        /// <summary>
        /// ReInitialize
        /// </summary>
        /// <param name="path"></param>
        public static void ReInitialize(string path)
        {
            try
            {
                string connectionString = DBConnection.Configure(path);
                BaseDao.SetConnectionString(connectionString);
                BaseDao.InitializeRetrySetting();

                if (BaseDao.CanAccessDB())
                {
                    LogHelper.Info(null, path);
                    _initialized = true;
                }
                else
                {
                    LogHelper.Error(null, string.Format("Can not access database.({0})", path));
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.Error(null, ex);
                throw new DaoException(ex);
            }
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User Login(string loginID, string password)
        {
            User user = null;
            try
            {
                EmployeeExecutionStrategy executionStrategy = GetEmployeeExecutionStrategy();
                executionStrategy.Execute(() =>
                {
                    using(EmployeeEntities context = GetEmployeeEntities())
                    {
                        user = context.Users
                            .Where(W => W.DeleteUserID == null)
                            .Where(W => W.UserID == loginID)
                            .SingleOrDefault();                      
                    }  
                });

                // Deleted user or user does not exist 
                if (user == null)
                {
                    throw new AuthenticationException(AuthenticationException.MessageIDType.E001);
                }

				string encryptPassword = CypherAndHashManager.Hash(password);
				// LoginID and Password does not match
				if (!user.Password.Equals(encryptPassword))
                {
                    throw new AuthenticationException(AuthenticationException.MessageIDType.E002);
                }

                return user;
            }
            catch (AuthenticationException)
            {
                throw;
            }
            catch(System.Exception ex)
            {
                LogHelper.Error(null, string.Format("Unknown error occured. Table Name: {0}", TableName.Users), ex);
                throw new DaoException(DaoException.ErrorID.E999, ex);
            }
        }

        #endregion
    }
}
