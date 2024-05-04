using System;
using System.Linq;
using ASM.EmployeeManagement.Common.Logger;
using ASM.EmployeeManagement.DataAccess.Model;
using ASM.EmployeeManagement.DataAccess.Exception;
using ASM.EmployeeManagement.DataAccess.Dao.WebAPIUI.Common;
using ASM.EmployeeManagement.DataAccess.Model.WebAPI.GetUserInfo;
using ASM.EmployeeManagement.DataAccess.Common.ExecutionStrategy;
using System.Collections.Generic;
using ASM.EmployeeManagement.DataAccess.Common.FilterInfo;
using ASM.EmployeeManagement.DataAccess.Common.Paging;
using ASM.EmployeeManagement.DataAccess.Model.WebAPI.GetUserInfoList;

namespace ASM.EmployeeManagement.DataAccess.Dao.WebAPIUI.GetUserInfoList
{
	/// <summary>
	/// GetUserInfoDao
	/// </summary>
	public class GetUserInfoListDao : WebAPIUICommonDao, IGetUserInfoListDao
	{
		#region GetUserInfoList

		/// <summary>
		/// GetUserInfoList
		/// </summary>
		/// <param name="userID"></param>
		/// <returns></returns>


		public List<UserInfoList> GetUserInfoList(Paging iPagingPara, UserFilterInfo objFilterInfo)
		{
			List<UserInfoList> userDetails = null;
			DaoException daoEx = null;
			try
			{
				EmployeeExecutionStrategy executionStrategy = GetEmployeeExecutionStrategy();
				executionStrategy.Execute(() =>
				{
					using (EmployeeEntities context = GetEmployeeEntities())
					{
						if(objFilterInfo.IsExactMatchSearch)
						CheckInputParameter(context,  objFilterInfo, out daoEx);

						if (daoEx != null)
						{
							return;
						}

						userDetails = GetUserInfoList(context,iPagingPara, objFilterInfo);
					}
				});
				if (daoEx != null)
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
		private void CheckInputParameter(EmployeeEntities context, UserFilterInfo objFilterInfo, out DaoException daoEx)
		{
			try
			{
				// Check User&Email exist
				CheckUsernEmailExist(context,  objFilterInfo, out daoEx);
				
			}
			catch (System.Exception ex)
			{
				LogHelper.Warn(this, ex);
				throw;
			}
		}

		/// <summary>
		/// CheckUsernEmailExist
		/// </summary>
		/// <param name="context"></param>
		/// <param name="UserFilterInfo"></param>
		/// <param name="daoEx"></param>
		private void CheckUsernEmailExist(EmployeeEntities context,  UserFilterInfo objFilterInfo, out DaoException daoEx)
		{
			try
			{
				daoEx = null;

				bool bExist = context.Users
					.Where(W => W.UserID == objFilterInfo.UserID)
					.ToList()
					.Count > 0;

				if (!bExist)
				{
					daoEx = new DaoException(DaoException.ErrorID.E003, new string[] { objFilterInfo.UserID.ToString() });
				}
				else
				{
					 bExist = context.UserDetails
					.Where(W => W.Email == objFilterInfo.Email)
					.ToList()
					.Count > 0;


					if (!bExist)
					{
						daoEx = new DaoException(DaoException.ErrorID.E004, new string[] { objFilterInfo.Email.ToString() });
					}
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
		private List<UserInfoList> GetUserInfoList(EmployeeEntities context,Paging iPagingInfo, UserFilterInfo objFilterInfo)
		{
			try
			{
				List<UserInfoList> userDetails = null;
				if (iPagingInfo.Num > 0)
				{
					if (objFilterInfo.IsExactMatchSearch)
					{
						//userDetails = context.UserDetails
						//					.Where(W => W.UserID==objFilterInfo.UserID)
						//					.Where(W => W.Email==objFilterInfo.Email).ToList();

						//var udL = from ud in context.UserDetails
						//		  join u in context.Users on ud.UserID equals u.ID
						//		  join d in context.Departments on u.DepartmentID equals d.ID
						//		  where (ud.UserID == objFilterInfo.UserID && ud.Email == objFilterInfo.Email && u.DeleteUserID==null)
						//		  orderby iPagingInfo.SortKey
						//		  select new UserInfoList
						//		  {
						//			  UserID = ud.UserID.ToString(),
						//			  UserName = u.UserName,
						//			  DepartmentName = d.Name,
						//			  Gender = ud.Gender,
						//			  MaritalStatus = ud.MaritalStatus,
						//			  DateOfBirth = ud.DateOfBirth,
						//			  Address = ud.Address,
						//			  Email = ud.Email,
						//			  PhoneNo = ud.PhoneNo,
						//			  JobStartDate = ud.JobStartDate
						//		  };
						var udL = context.Users
					.Where(W => W.DeleteUserID == null && W.UserID == objFilterInfo.UserID)
					.Join(context.UserDetails.Where(W => W.Email == objFilterInfo.Email),
						objUser => objUser.ID,
						objUserDetail => objUserDetail.UserID,
						(objUser, objUserDetail) => new { objUser, objUserDetail }).Join(context.Departments, objUser => objUser.objUser.DepartmentID,
						objDep => objDep.ID,
						(objUser, objDep) => new { objUser, objDep })
					.Select(S => new UserInfoList()
					{
						UserID = S.objUser.objUser.UserID,
						UserName = S.objUser.objUser.UserName,
						DepartmentName = S.objUser.objUser.Department.Name,
						Gender = S.objUser.objUserDetail.Gender,
						MaritalStatus = S.objUser.objUserDetail.MaritalStatus,
						DateOfBirth = S.objUser.objUserDetail.DateOfBirth,
						Address = S.objUser.objUserDetail.Address,
						Email = S.objUser.objUserDetail.Email,
						PhoneNo = S.objUser.objUserDetail.PhoneNo,
						JobStartDate = (S.objUser.objUserDetail.JobStartDate != null) ? (DateTime)S.objUser.objUserDetail.JobStartDate : (DateTime?)null,
					});
						switch (iPagingInfo.SortKey)
						{
							case 0: userDetails = udL.OrderBy(uInfo => uInfo.UserID).ToList(); break;
							case 1: userDetails = udL.OrderBy(uInfo => uInfo.UserName).ToList(); break;
							case 2: userDetails = udL.OrderBy(uInfo => uInfo.DepartmentName).ToList(); break;
							case 3: userDetails = udL.OrderBy(uInfo => uInfo.Gender).ToList(); break;
							case 4: userDetails = udL.OrderBy(uInfo => uInfo.MaritalStatus).ToList(); break;
							case 5: userDetails = udL.OrderBy(uInfo => uInfo.DateOfBirth).ToList(); break;
							case 6: userDetails = udL.OrderBy(uInfo => uInfo.Address).ToList(); break;
							case 7: userDetails = udL.OrderBy(uInfo => uInfo.Email).ToList(); break;
							case 8: userDetails = udL.OrderBy(uInfo => uInfo.PhoneNo).ToList(); break;
							case 9: userDetails = udL.OrderBy(uInfo => uInfo.JobStartDate).ToList(); break;
							default: userDetails = udL.ToList(); break;
						}
						if (iPagingInfo.SortOrder == 2)
						{
							userDetails = Enumerable.Reverse(userDetails).ToList();
							userDetails = userDetails.Skip((iPagingInfo.StartIndex - 1) * iPagingInfo.Num).Take(iPagingInfo.Num).ToList();
						}
						else
						{
							userDetails = userDetails.Skip((iPagingInfo.StartIndex - 1) * iPagingInfo.Num).Take(iPagingInfo.Num).ToList();
						}
					}
					else
					{
						//var udL = from ud in context.UserDetails
						//		  join u in context.Users on ud.UserID equals u.ID
						//		  join d in context.Departments on u.DepartmentID equals d.ID
						//		  where u.DeleteUserID == null
						//		  orderby iPagingInfo.SortKey
						//		  select new UserInfoList
						//		  {
						//			  UserID = ud.UserID.ToString(),
						//			  UserName = u.UserName,
						//			  DepartmentName = d.Name,
						//			  Gender = ud.Gender,
						//			  MaritalStatus = ud.MaritalStatus,
						//			  DateOfBirth = ud.DateOfBirth,
						//			  Address = ud.Address,
						//			  Email = ud.Email,
						//			  PhoneNo = ud.PhoneNo,
						//			  JobStartDate = ud.JobStartDate
						//		  };
						var udL = context.Users
					.Where(W => W.DeleteUserID == null)
					.Join(context.UserDetails,
						objUser => objUser.ID,
						objUserDetail => objUserDetail.UserID,
						(objUser, objUserDetail) => new { objUser, objUserDetail }).Join(context.Departments, objUser => objUser.objUser.DepartmentID,
						objDep => objDep.ID,
						(objUser, objDep) => new { objUser, objDep })
					.Select(S => new UserInfoList()
					{
						UserID = S.objUser.objUser.UserID,
						UserName = S.objUser.objUser.UserName,
						DepartmentName = S.objUser.objUser.Department.Name,
						Gender = S.objUser.objUserDetail.Gender,
						MaritalStatus = S.objUser.objUserDetail.MaritalStatus,
						DateOfBirth = S.objUser.objUserDetail.DateOfBirth,
						Address = S.objUser.objUserDetail.Address,
						Email = S.objUser.objUserDetail.Email,
						PhoneNo = S.objUser.objUserDetail.PhoneNo,
						JobStartDate = (S.objUser.objUserDetail.JobStartDate != null) ? (DateTime)S.objUser.objUserDetail.JobStartDate : (DateTime?)null,
					});
						switch (iPagingInfo.SortKey)
						{
							case 0: userDetails = udL.OrderBy(uInfo => uInfo.UserID).ToList(); break;
							case 1: userDetails = udL.OrderBy(uInfo => uInfo.UserName).ToList(); break;
							case 2: userDetails = udL.OrderBy(uInfo => uInfo.DepartmentName).ToList(); break;
							case 3: userDetails = udL.OrderBy(uInfo => uInfo.Gender).ToList(); break;
							case 4: userDetails = udL.OrderBy(uInfo => uInfo.MaritalStatus).ToList(); break;
							case 5: userDetails = udL.OrderBy(uInfo => uInfo.DateOfBirth).ToList(); break;
							case 6: userDetails = udL.OrderBy(uInfo => uInfo.Address).ToList(); break;
							case 7: userDetails = udL.OrderBy(uInfo => uInfo.Email).ToList(); break;
							case 8: userDetails = udL.OrderBy(uInfo => uInfo.PhoneNo).ToList(); break;
							case 9: userDetails = udL.OrderBy(uInfo => uInfo.JobStartDate).ToList(); break;
							default: userDetails = udL.ToList(); break;
						}
						if (iPagingInfo.SortOrder == 2)
						{
							userDetails = Enumerable.Reverse(userDetails).ToList();
							userDetails = userDetails.Skip((iPagingInfo.StartIndex - 1) * iPagingInfo.Num).Take(iPagingInfo.Num).ToList();
						}
						else
						{
							userDetails = userDetails.Skip((iPagingInfo.StartIndex - 1) * iPagingInfo.Num).Take(iPagingInfo.Num).ToList();
						}

					}



					return userDetails;

				}
				else return null;
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
