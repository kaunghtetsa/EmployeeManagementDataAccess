using System;
using System.Linq;

using ASM.EmployeeManagement.Common.Logger;
using ASM.EmployeeManagement.DataAccess.Model;
using ASM.EmployeeManagement.DataAccess.Exception;
using ASM.EmployeeManagement.DataAccess.Dao.WebAPIUI.Common;
using ASM.EmployeeManagement.DataAccess.Model.WebAPI.GetUserInfo;
using ASM.EmployeeManagement.DataAccess.Common.ExecutionStrategy;

namespace ASM.EmployeeManagement.DataAccess.Dao.WebAPIUI.GetUserInfo
{
    /// <summary>
    /// GetUserInfoDao
    /// </summary>
    public class GetUserInfoDao : WebAPIUICommonDao, IGetUserInfoDao
    {
        #region GetUserInfo

        /// <summary>
        /// GetUserInfo
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public UserDetails GetUserInfo(string userID)
        {
            UserDetails userDetails = null;
            DaoException daoEx = null;
            try
            {
                EmployeeExecutionStrategy executionStrategy = GetEmployeeExecutionStrategy();
                executionStrategy.Execute(() =>
                {
                    using (EmployeeEntities context = GetEmployeeEntities())
                    {
                        CheckInputParameter(context, userID, out daoEx);
                        if(daoEx != null)
                        {
                            return;
                        }

                        userDetails = GetUserInfo(context, userID);
                    }
                });

                if(daoEx != null)
                {
                    throw daoEx;
                }
            }
            catch (DaoException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                LogHelper.Error(this, ex);
                throw new DaoException(DaoException.ErrorID.E999, ex);
            }

            return userDetails;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// CheckInputParameter
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userID"></param>
        /// <param name="daoEx"></param>
        private void CheckInputParameter(EmployeeEntities context, string userID, out DaoException daoEx)
        {
            try
            {
                // Check User exist
                CheckUserExist(context, userID, out daoEx);
            }
            catch(System.Exception ex)
            {
                LogHelper.Warn(this, ex);
                throw;
            }
        }

        /// <summary>
        /// CheckUserExist
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userID"></param>
        /// <param name="daoEx"></param>
        private void CheckUserExist(EmployeeEntities context, string userID, out DaoException daoEx)
        {
            try
            {
                daoEx = null;

                bool bExist = context.Users
                    .Where(W => W.UserID == userID)
                    .Where(W => W.DeleteUserID == null)
                    .ToList()
                    .Count > 0;

                if (!bExist)
                {
                    daoEx = new DaoException(DaoException.ErrorID.E003, new string[] { userID });
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.Warn(this, ex);
                throw;
            }
        }

        /// <summary>
        /// Get user detail info
        /// </summary>
        /// <param name="context"></param>
        /// <param name="daoEx"></param>
        /// <returns></returns>
        private UserDetails GetUserInfo(EmployeeEntities context, string userID)
        {
            try
            {
                UserDetails userDetails = context.Users
                    .Where(W => W.DeleteUserID == null)
                    .Where(W => W.UserID == userID)
                    .Join(context.UserDetails,
                        objUser => objUser.ID,
                        objUserDetail => objUserDetail.UserID,
                        (objUser, objUserDetail) => new { objUser, objUserDetail })
                    .Select(S => new UserDetails()
                    {
                        UserID = S.objUser.UserID,
                        UserName = S.objUser.UserName,
                        DepartmentName = S.objUser.Department.Name,
                        Gender = S.objUserDetail.Gender,
                        DateOfBirth = S.objUserDetail.DateOfBirth,
                        Address = S.objUserDetail.Address,
                        Email = S.objUserDetail.Email,
                        PhoneNo = S.objUserDetail.PhoneNo,
                        JobStartDate = (S.objUserDetail.JobStartDate != null) ? (DateTime)S.objUserDetail.JobStartDate : (DateTime?)null,
                    })
                    .SingleOrDefault();

                return userDetails;
            }
            catch (System.Exception ex)
            {
                LogHelper.Warn(null, ex);
                throw;
            }
        }

        #endregion
    }
}
