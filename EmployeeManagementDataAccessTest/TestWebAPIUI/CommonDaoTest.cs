using System;
using ASM.EmployeeManagement.DataAccess.Dao.WebAPIUI.Common;
using ASM.EmployeeManagement.DataAccess.Exception;
using ASM.EmployeeManagement.DataAccess.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmployeeManagementDataAccessTest.TestWebAPIUI
{
    /// <summary>
    /// WebAPIUICommonDao Test
    /// </summary>
    [TestClass]
    public class CommonDaoTest : BaseTest
    {
        /// <summary>
        /// Login_NormalCase
        /// </summary>
        [TestMethod]
        public void Login_NormalCase()
        {
            Department department = null;
            User user = null;
            try
            {
                // Create Department
                department = CreateDummyDepartment("ASM", "ACTY SYSTEM MYANMAR");

				// Create user
				string userID = "acty.testuser";
				string password = "12345";

				user = CreateDummyUserData(userID, "Test User", password, department.ID, 1, 1, Convert.ToDateTime("1994-03-01"), null, "testuser@gmail.com", "+95123485474");

                WebAPIUICommonDao dao = new WebAPIUICommonDao();

                // Login
                User loginUser = dao.Login(userID, password);
				Assert.IsNotNull(loginUser);
            }
            finally
            {
                if(user != null)
                {
                    DeleteDummyUser(user.ID);
                }

                if(department != null)
                {
                    DeleteDummyDepartment(department.ID);
                }
            }
        }

        /// <summary>
        /// Loing (User Does not Exist)
        /// </summary>
        [TestMethod]
        public void Login_IrregularCase_UserNotExist()
        {
            try
            {
                WebAPIUICommonDao dao = new WebAPIUICommonDao();

                // Login
                dao.Login("acty.testuser", "1234");
            }
            catch(AuthenticationException ex)
            {
                Assert.AreEqual(ex.ErrorId, AuthenticationException.MessageIDType.E001.ToString());
            }
        }

        /// <summary>
        /// Login (Deleted User)
        /// </summary>
        /// 
        [TestMethod]
        public void Login_IrregularCase_DeletedUser()
        {
            Department department = null;
            User user = null;
            try
            {
                // Create Department
                department = CreateDummyDepartment("ASM", "ACTY SYSTEM MYANMAR");

				// Create user
				string userID = "acty.testuser";
				string password = "12345";

				// Create user
				user = CreateDummyUserData(userID, "Test User", password, department.ID, 1, 1, Convert.ToDateTime("1994-03-01"), null, "testuser@gmail.com", "+95123485474");

                // Delete user
                DeleteUser(user.ID);

                WebAPIUICommonDao dao = new WebAPIUICommonDao();

                // Login
                dao.Login(userID, password);
            }
            catch (AuthenticationException ex)
            {
                Assert.AreEqual(ex.ErrorId, AuthenticationException.MessageIDType.E001.ToString());
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

        /// <summary>
        /// Login (LoginID and Password not match)
        /// </summary>
        [TestMethod]
        public void Login_IrregularCase_LoginIDPasswordNotMatch()
        {
            Department department = null;
            User user = null;
            try
            {
                // Create Department
                department = CreateDummyDepartment("ASM", "ACTY SYSTEM MYANMAR");

				// Create user
				string userID = "acty.testuser";
				string password = "12345";

				// Create user
				user = CreateDummyUserData(userID, "Test User", password, department.ID, 1, 1, Convert.ToDateTime("1994-03-01"), null, "testuser@gmail.com", "+95123485474");

                WebAPIUICommonDao dao = new WebAPIUICommonDao();

                // Login
                dao.Login(user.UserID, "45678");
            }
            catch (AuthenticationException ex)
            {
                Assert.AreEqual(ex.ErrorId, AuthenticationException.MessageIDType.E002.ToString());
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
    }
}
