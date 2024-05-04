using ASM.EmployeeManagement.DataAccess.Common.FilterInfo;
using ASM.EmployeeManagement.DataAccess.Common.Paging;
using ASM.EmployeeManagement.DataAccess.Dao.WebAPIUI.GetUserInfoList;
using ASM.EmployeeManagement.DataAccess.Exception;
using ASM.EmployeeManagement.DataAccess.Model;
using ASM.EmployeeManagement.DataAccess.Model.WebAPI.GetUserInfoList;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace EmployeeManagementDataAccessTest.TestWebAPIUI
{
	/// <summary>
	/// Test GetUserInfoDao
	/// </summary>
	[TestClass]
	public class GetUserInfoListDaoTest : BaseTest
	{
		#region Test Methods
		/// <summary>
		/// TestGetUserInfoList_NormalCase
		/// </summary>
		[TestMethod]
		public void TestGetUserInfoList_NormalCase()
		{
			try
			{
				IGetUserInfoListDao dao = new GetUserInfoListDao();
				Paging newPagingObject = new Paging();
				newPagingObject.Num = 10;
				newPagingObject.SortKey = 1;
				newPagingObject.SortOrder = 1;
				newPagingObject.StartIndex = 0;
				UserFilterInfo newUserFilterInfo = new UserFilterInfo();
				newUserFilterInfo.IsExactMatchSearch = true;
				newUserFilterInfo.UserID = "acty.hr";
				newUserFilterInfo.Email = "acty.hr@gmail.com";
				List<UserInfoList> userDetails = dao.GetUserInfoList(newPagingObject, newUserFilterInfo);
				Assert.AreNotEqual(userDetails, null);

				Assert.IsTrue(!string.IsNullOrEmpty(userDetails[0].UserID));
				Assert.IsTrue(!string.IsNullOrEmpty(userDetails[0].UserName));
				Assert.IsTrue(!string.IsNullOrEmpty(userDetails[0].DepartmentName));
				Assert.IsTrue(userDetails[0].Gender != 0);
				Assert.IsTrue(userDetails[0].MaritalStatus != 0);
				Assert.IsTrue(userDetails[0].DateOfBirth != null);
				Assert.IsTrue(!string.IsNullOrEmpty(userDetails[0].Email));
				Assert.IsTrue(!string.IsNullOrEmpty(userDetails[0].PhoneNo));
				Assert.IsTrue(userDetails[0].JobStartDate != null);
			}
			catch (Exception ex)
			{
				Assert.Fail(ex.Message);
			}
		}
		/// <summary>
		/// TestGetUserInfoList_IrregularCase_NotExistUserID
		/// </summary>
		[TestMethod]
		public void TestGetUserInfoList_IrregularCase_NotExistUserID()
		{
			try
			{
				IGetUserInfoListDao dao = new GetUserInfoListDao();

				// Not exist user ID
				Paging newPagingObject = new Paging();
				newPagingObject.Num = 10;
				newPagingObject.SortKey = 1;
				newPagingObject.SortOrder = 1;
				newPagingObject.StartIndex = 0;
				UserFilterInfo newUserFilterInfo = new UserFilterInfo();
				newUserFilterInfo.IsExactMatchSearch = true;
				newUserFilterInfo.UserID = "acty.testuser";
				newUserFilterInfo.Email = "testuser@gmail.com";
				List<UserInfoList> userDetails = dao.GetUserInfoList(newPagingObject, newUserFilterInfo);

			}
			catch (DaoException ex)
			{
				Assert.AreEqual(ex.ErrorId, DaoException.ErrorID.E003.ToString());
			}
		}
		///// <summary>
		///// TestGetUserInfoList_IrregularCase_NullUserID
		///// </summary>
		//[TestMethod]
		//public void TestGetUserInfoList_IrregularCase_NullUserID()
		//{
		//	try
		//	{
		//		IGetUserInfoListDao dao = new GetUserInfoListDao();

		//		// Not exist user ID
		//		Paging newPagingObject = new Paging();
		//		newPagingObject.Num = 10;
		//		newPagingObject.SortKey = 1;
		//		newPagingObject.SortOrder = 1;
		//		newPagingObject.StartIndex = 0;
		//		UserFilterInfo newUserFilterInfo = new UserFilterInfo();
		//		newUserFilterInfo.IsExactMatchSearch = true;
		//		newUserFilterInfo.UserID = null;
		//		newUserFilterInfo.Email = "testuser@gmail.com";
		//		List<UserInfoList> userDetails = dao.GetUserInfoList(newPagingObject, newUserFilterInfo);

		//	}
		//	catch (DaoException ex)
		//	{
		//		Assert.AreEqual(ex.ErrorId, DaoException.ErrorID.E003.ToString());
		//	}
		//}
		///// <summary>
		///// TestGetUserInfoList_IrregularCase_SpaceUserID
		///// </summary>
		//[TestMethod]
		//public void TestGetUserInfoList_IrregularCase_SpaceUserID()
		//{
		//	try
		//	{
		//		IGetUserInfoListDao dao = new GetUserInfoListDao();

		//		// Not exist user ID
		//		Paging newPagingObject = new Paging();
		//		newPagingObject.Num = 10;
		//		newPagingObject.SortKey = 1;
		//		newPagingObject.SortOrder = 1;
		//		newPagingObject.StartIndex = 0;
		//		UserFilterInfo newUserFilterInfo = new UserFilterInfo();
		//		newUserFilterInfo.IsExactMatchSearch = true;
		//		newUserFilterInfo.UserID = " ";
		//		newUserFilterInfo.Email = "testuser@gmail.com";
		//		List<UserInfoList> userDetails = dao.GetUserInfoList(newPagingObject, newUserFilterInfo);

		//	}
		//	catch (DaoException ex)
		//	{
		//		Assert.AreEqual(ex.ErrorId, DaoException.ErrorID.E003.ToString());
		//	}
		//}
		///// <summary>
		///// TestGetUserInfoList_IrregularCase_InvalidLength_UserID
		///// </summary>
		//[TestMethod]
		//public void TestGetUserInfoList_IrregularCase_InvalidLength_UserID()
		//{
		//	try
		//	{
		//		IGetUserInfoListDao dao = new GetUserInfoListDao();

		//		// Not exist user ID
		//		Paging newPagingObject = new Paging();
		//		newPagingObject.Num = 10;
		//		newPagingObject.SortKey = 1;
		//		newPagingObject.SortOrder = 1;
		//		newPagingObject.StartIndex = 0;
		//		UserFilterInfo newUserFilterInfo = new UserFilterInfo();
		//		newUserFilterInfo.IsExactMatchSearch = true;
		//		newUserFilterInfo.UserID = "acty.hr23945678236452784657834657862357235482345955";
		//		newUserFilterInfo.Email = "testuser@gmail.com";
		//		List<UserInfoList> userDetails = dao.GetUserInfoList(newPagingObject, newUserFilterInfo);

		//	}
		//	catch (DaoException ex)
		//	{
		//		Assert.AreEqual(ex.ErrorId, DaoException.ErrorID.E003.ToString());
		//	}
		//}
		/// <summary>
		/// TestGetUserInfoList_IrregularCase_NotExistEmail
		/// </summary>
		[TestMethod]
		public void TestGetUserInfoList_IrregularCase_NotExistEmail()
		{
			try
			{
				IGetUserInfoListDao dao = new GetUserInfoListDao();

				// Not exist user ID
				Paging newPagingObject = new Paging();
				newPagingObject.Num = 10;
				newPagingObject.SortKey = 1;
				newPagingObject.SortOrder = 1;
				newPagingObject.StartIndex = 0;
				UserFilterInfo newUserFilterInfo = new UserFilterInfo();
				newUserFilterInfo.IsExactMatchSearch = true;
				newUserFilterInfo.UserID = "acty.hr";
				newUserFilterInfo.Email = "appwiz.cpl@gmail.com";
				List<UserInfoList> userDetails = dao.GetUserInfoList(newPagingObject, newUserFilterInfo);

			}
			catch (DaoException ex)
			{
				Assert.AreEqual(ex.ErrorId, DaoException.ErrorID.E004.ToString());
			}
		}
		///// <summary>
		///// TestGetUserInfoList_IrregularCase_NullEmail
		///// </summary>
		//[TestMethod]
		//public void TestGetUserInfoList_IrregularCase_NullEmail()
		//{
		//	try
		//	{
		//		IGetUserInfoListDao dao = new GetUserInfoListDao();

		//		// Not exist user ID
		//		Paging newPagingObject = new Paging();
		//		newPagingObject.Num = 10;
		//		newPagingObject.SortKey = 1;
		//		newPagingObject.SortOrder = 1;
		//		newPagingObject.StartIndex = 0;
		//		UserFilterInfo newUserFilterInfo = new UserFilterInfo();
		//		newUserFilterInfo.IsExactMatchSearch = true;
		//		newUserFilterInfo.UserID = "acty.hr";
		//		newUserFilterInfo.Email = null;
		//		List<UserInfoList> userDetails = dao.GetUserInfoList(newPagingObject, newUserFilterInfo);

		//	}
		//	catch (DaoException ex)
		//	{
		//		Assert.AreEqual(ex.ErrorId, DaoException.ErrorID.E004.ToString());
		//	}
		//}

		///// <summary>
		///// TestGetUserInfoList_IrregularCase_SpaceEmail
		///// </summary>
		//[TestMethod]
		//public void TestGetUserInfoList_IrregularCase_SpaceEmail()
		//{
		//	try
		//	{
		//		IGetUserInfoListDao dao = new GetUserInfoListDao();

		//		// Not exist user ID
		//		Paging newPagingObject = new Paging();
		//		newPagingObject.Num = 10;
		//		newPagingObject.SortKey = 1;
		//		newPagingObject.SortOrder = 1;
		//		newPagingObject.StartIndex = 0;
		//		UserFilterInfo newUserFilterInfo = new UserFilterInfo();
		//		newUserFilterInfo.IsExactMatchSearch = true;
		//		newUserFilterInfo.UserID = "acty.hr";
		//		newUserFilterInfo.Email = " ";
		//		List<UserInfoList> userDetails = dao.GetUserInfoList(newPagingObject, newUserFilterInfo);

		//	}
		//	catch (DaoException ex)
		//	{
		//		Assert.AreEqual(ex.ErrorId, DaoException.ErrorID.E004.ToString());
		//	}
		//}

		///// <summary>
		///// TestGetUserInfoList_IrregularCase_Invalid_Length_Email
		///// </summary>
		//[TestMethod]
		//public void TestGetUserInfoList_IrregularCase_Invalid_Length_Email()
		//{
		//	try
		//	{
		//		IGetUserInfoListDao dao = new GetUserInfoListDao();

		//		// Not exist user ID
		//		Paging newPagingObject = new Paging();
		//		newPagingObject.Num = 10;
		//		newPagingObject.SortKey = 1;
		//		newPagingObject.SortOrder = 1;
		//		newPagingObject.StartIndex = 0;
		//		UserFilterInfo newUserFilterInfo = new UserFilterInfo();
		//		newUserFilterInfo.IsExactMatchSearch = true;
		//		newUserFilterInfo.UserID = "acty.hr";
		//		newUserFilterInfo.Email = "acty2156731824673612478361473637246362736247837821437826@gmail.com";
		//		List<UserInfoList> userDetails = dao.GetUserInfoList(newPagingObject, newUserFilterInfo);

		//	}
		//	catch (DaoException ex)
		//	{
		//		Assert.AreEqual(ex.ErrorId, DaoException.ErrorID.E004.ToString());
		//	}
		//}

		/// <summary>
		/// TestGetUserInfoList_IrregularCase_DeletedUserID
		/// </summary>
		[TestMethod]
		public void TestGetUserInfoList_IrregularCase_DeletedUserID()
		{
			Department department = null;
			User user = null;
			try
			{
				// Create Department
				department = CreateDummyDepartment("ASM", "ACTY SYSTEM MYANMAR");

				// Create user
				user = CreateDummyUserData("acty.testuser", "Test User", "12345", department.ID, 1, 1, Convert.ToDateTime("1994-03-01"), null, "testuser@gmail.com", "+95123485474");

				// Delete user
				DeleteUser(user.ID);

				IGetUserInfoListDao dao = new GetUserInfoListDao();

				// Not exist user ID
				Paging newPagingObject = new Paging();
				newPagingObject.Num = 10;
				newPagingObject.SortKey = 1;
				newPagingObject.SortOrder = 1;
				newPagingObject.StartIndex = 0;
				UserFilterInfo newUserFilterInfo = new UserFilterInfo();
				newUserFilterInfo.IsExactMatchSearch = true;
				newUserFilterInfo.UserID = "acty.testuser";
				newUserFilterInfo.Email = "testuser@gmail.com";
				List<UserInfoList> userDetails = dao.GetUserInfoList(newPagingObject, newUserFilterInfo);


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
