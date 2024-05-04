using System;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

using ASM.EmployeeManagement.Common.Logger;

namespace ASM.EmployeeManagement.DataAccess.Common.ExecutionStrategy
{
    public class EmployeeExecutionStrategy : DbExecutionStrategy
    {
        #region Private Memebrs
        /// <summary>
        /// 0		: 
        /// 5		: 
        /// 53		: 
        /// 1205	: deadlock
        /// -1      : Unable to connect to the DB. (Physical connection is not usable, etc...)
        /// 19      : The task or query can be retried. (May be the failure of software or hardware)
        /// 258     : The wait operation timed out
        /// </summary>
        private static List<int> _lstErrorCodes = new List<int>() { 5, 0, 53, 1205, -1, 19, 258 };
        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxRetryCount"></param>
        /// <param name="maxDelay"></param>
        public EmployeeExecutionStrategy(int maxRetryCount, TimeSpan maxDelay)
            : base(maxRetryCount, maxDelay)
        {
        }

        #endregion

        #region Override Method ShouldRetryOn

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        protected override bool ShouldRetryOn(System.Exception exception)
        {
            bool bRetry = false;

            SqlException objSqlException = exception as SqlException;

            if (objSqlException != null)
            {
                List<int> lstErrorCodes = objSqlException.Errors.Cast<SqlError>().Select(S => S.Number).ToList();
                if (lstErrorCodes.Any(A => _lstErrorCodes.Contains(A)))
                {
                    bRetry = true;
                }
                LogHelper.Warn(null, string.Format("ErrorNumbers:{0} \tErrorCode:{1} \tRetry:{2} \tMsg:{3} ", 
					string.Join(", ", lstErrorCodes), objSqlException.ErrorCode, bRetry, objSqlException.Message));
            }
			else if(exception != null)
			{
				LogHelper.Warn(null, string.Format("{0}({1})", exception.GetType().Name, exception.Message));
			}

            return bRetry;
        }

        #endregion
    }
}
