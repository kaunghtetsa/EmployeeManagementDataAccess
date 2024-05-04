using System;
using System.Linq;
using System.Transactions;
using ASM.EmployeeManagement.DataAccess.Dao;
using ASM.EmployeeManagement.DataAccess.Exception;
using ASM.EmployeeManagement.DataAccess.Model;
using ASM.EmployeeManagement.Common.Encryption;

namespace EmployeeManagementDataAccessTest.TestWebAPIUI
{
    /// <summary>
    /// Base test class
    /// </summary>
    public class BaseTest : BaseDao
    {
        #region Create

        /// <summary>
        /// CreateDummyDepartment
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        protected Department CreateDummyDepartment(string name, string description)
        {
            try
            {
                using (EmployeeEntities context = new EmployeeEntities())
                {
                    Department department = new Department()
                    {
                        Name = name,
                        Descriptions = description
                    };

                    context.Departments.Add(department);
                    context.SaveChanges();

                    return department;
                }
            }
            catch (System.Exception ex)
            {
                throw new DaoException(DaoException.ErrorID.E999, ex);
            }
        }

		/// <summary>
		/// Create dummy user
		/// </summary>
		/// <param name="userID"></param>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <param name="departmentID"></param>
		/// <param name="gender"></param>
		/// <param name="maritalStatus"></param>
		/// <param name="birthDate"></param>
		/// <param name="address"></param>
		/// <param name="email"></param>
		/// <param name="phoneNo"></param>
		/// <returns></returns>
		protected User CreateDummyUserData(string userID, string userName, string password, long departmentID, byte gender,byte maritalStatus, DateTime birthDate, string address, 
            string email, string phoneNo)
        {
            try
            {
                using (EmployeeEntities context = new EmployeeEntities())
                {
                    using(TransactionScope trans = GetReadUncommittedTransactionScope())
                    {
                        User user = CreateUser(context, userID, userName, password, departmentID);

                        CreateUserDetails(context, user.ID, gender, maritalStatus, birthDate, address, email, phoneNo);

                        trans.Complete();

                        return user;
                    }
                }
            }
            catch(System.Exception ex)
            {
                throw new DaoException(DaoException.ErrorID.E999, ex);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteUser
        /// </summary>
        /// <param name="userID"></param>
        protected void DeleteUser(long userID)
        {
            try
            {
                using (EmployeeEntities context = new EmployeeEntities())
                {
                    User user = context.Users
                        .Where(W => W.ID == userID)
                        .SingleOrDefault();

                    if(user != null)
                    {
                        user.DeleteUserID = user.ID;
                        user.UpdateDateTime = DateTime.UtcNow;
                    }

                    context.SaveChanges();
                }
            }
            catch (System.Exception ex)
            {
                throw new DaoException(DaoException.ErrorID.E999, ex);
            }
        }

        /// <summary>
        /// DeleteDummyUser
        /// </summary>
        /// <param name="userID"></param>
        protected void DeleteDummyUser(long userID)
        {

            try
            {
                using (EmployeeEntities context = new EmployeeEntities())
                {
                    using (TransactionScope trans = GetReadUncommittedTransactionScope())
                    {
                        // Delete from UserDetails
                        UserDetail userDetail = context.UserDetails
                            .Where(W => W.UserID == userID)
                            .SingleOrDefault();

                        if (userDetail != null)
                        {
                            context.UserDetails.Remove(userDetail);
                            context.SaveChanges();
                        }

                        // Delete from Users
                        User user = context.Users
                            .Where(W => W.ID == userID)
                            .SingleOrDefault();

                        if(user != null)
                        {
                            context.Users.Remove(user);
                            context.SaveChanges();
                        }

                        trans.Complete();
                    }   
                }
            }
            catch (System.Exception ex)
            {
                throw new DaoException(DaoException.ErrorID.E999, ex);
            }
        }

        /// <summary>
        /// DeleteDummyDepartment
        /// </summary>
        /// <param name="departmentID"></param>
        protected void DeleteDummyDepartment(long departmentID)
        {
            try
            {
                using (EmployeeEntities context = new EmployeeEntities())
                {
                    Department department = context.Departments
                        .Where(W => W.ID == departmentID)
                        .SingleOrDefault();

                    if(department != null)
                    {
                        context.Departments.Remove(department);
                        context.SaveChanges();
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new DaoException(DaoException.ErrorID.E999, ex);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// CreateUser
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="departmentID"></param>
        /// <returns></returns>
        private User CreateUser(EmployeeEntities context, string userID, string userName, string password, long departmentID)
        {
            try
            {
                // Add new user
                User user = new User()
                {
                    UserID = userID,
                    UserName = userName,
                    Password = CypherAndHashManager.Hash(password),
                    DepartmentID = departmentID,
                    RegistDateTime = DateTime.UtcNow
                };

                context.Users.Add(user);
                context.SaveChanges();

                return user;
            }
            catch (System.Exception ex)
            {
                throw new DaoException(DaoException.ErrorID.E999, ex);
            }
        }

        /// <summary>
        /// CreateUserDetails
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="gender"></param>
        /// <param name="birthDate"></param>
        /// <param name="address"></param>
        /// <param name="email"></param>
        /// <param name="phoneNo"></param>
        private void CreateUserDetails(EmployeeEntities context, long userID, byte gender,byte maritalStatus, DateTime birthDate, string address,
            string email, string phoneNo)
        {
            try
            {
                // Add new user detail
                UserDetail userDetail = new UserDetail()
                {
                    UserID = userID,
                    Gender = gender,
					MaritalStatus= maritalStatus,
					DateOfBirth = birthDate,
                    Address = address,
                    Email = email,
                    PhoneNo = phoneNo
                };

                context.UserDetails.Add(userDetail);
                context.SaveChanges();
            }
            catch (System.Exception ex)
            {
                throw new DaoException(DaoException.ErrorID.E999, ex);
            }
        }

        #endregion
    }
}
