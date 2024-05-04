using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ASM.EmployeeManagement.DataAccess.Model;
using ASM.EmployeeManagement.DataAccess.Exception;
using ASM.EmployeeManagement.DataAccess.Dao.WebAPIUI.GetUserInfo;
using ASM.EmployeeManagement.DataAccess.Model.WebAPI.GetUserInfo;

namespace EmployeeManagementDataAccessTest.TestWebAPIUI
{
    /// <summary>
    /// Test GetUserInfoDao
    /// </summary>
    [TestClass]
    public class GetUserInfoDaoTest : BaseTest
    {
        #region Test Methods

        /// <summary>
        /// TestGetUserInfo_NormalCase
        /// </summary>
        [TestMethod]
        public void TestGetUserInfo_NormalCase()
        {
            try
            {
                IGetUserInfoDao dao = new GetUserInfoDao();

                UserDetails userDetails = dao.GetUserInfo("acty.hr");
                Assert.AreNotEqual(userDetails, null);

                Assert.IsTrue(!string.IsNullOrEmpty(userDetails.UserID));
                Assert.IsTrue(!string.IsNullOrEmpty(userDetails.UserName));
                Assert.IsTrue(!string.IsNullOrEmpty(userDetails.DepartmentName));
                Assert.IsTrue(userDetails.Gender != 0);
                Assert.IsTrue(userDetails.DateOfBirth != null);
                Assert.IsTrue(!string.IsNullOrEmpty(userDetails.Email));
                Assert.IsTrue(!string.IsNullOrEmpty(userDetails.PhoneNo));
                Assert.IsTrue(userDetails.JobStartDate != null);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
		

		/// <summary>
		/// TestGetUserInfo_IrregularCase_NotExistUserID
		/// </summary>
		[TestMethod]
        public void TestGetUserInfo_IrregularCase_NotExistUserID()
        {
            try
            {
                IGetUserInfoDao dao = new GetUserInfoDao();

                // Not exist user ID
                UserDetails userDetails = dao.GetUserInfo("acty.testuser");

            }
            catch (DaoException ex)
            {
                Assert.AreEqual(ex.ErrorId, DaoException.ErrorID.E003.ToString());
            }
        }

        /// <summary>
        /// TestGetUserInfo_IrregularCase_DeletedUserID
        /// </summary>
        [TestMethod]
        public void TestGetUserInfo_IrregularCase_DeletedUserID()
        {
            Department department = null;
            User user = null;
            try
            {
                // Create Department
                department = CreateDummyDepartment("ASM", "ACTY SYSTEM MYANMAR");

                // Create user
                user = CreateDummyUserData("acty.testuser", "Test User", "12345", department.ID, 1,1, Convert.ToDateTime("1994-03-01"), null, "testuser@gmail.com", "+95123485474");

                // Delete user
                DeleteUser(user.ID);

                IGetUserInfoDao dao = new GetUserInfoDao();

                // Not exist user ID
                UserDetails userDetails = dao.GetUserInfo("acty.testuser");

            }
            catch (DaoException ex)
            {
                Assert.AreEqual(ex.ErrorId, DaoException.ErrorID.E003.ToString());
            }
            finally
            {
                if (user != null)
                {
                    DeleteDummyUser(user.ID);
                }

                if (department != null)
                {
                    DeleteDummyDepartment(department.ID);
                }
            }
        }

        #endregion
    }
}
