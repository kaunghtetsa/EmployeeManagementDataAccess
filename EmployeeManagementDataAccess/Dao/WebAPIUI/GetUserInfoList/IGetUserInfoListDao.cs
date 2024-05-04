using ASM.EmployeeManagement.DataAccess.Dao.WebAPIUI.Common;
using ASM.EmployeeManagement.DataAccess.Model.WebAPI.GetUserInfoList;
using System.Collections.Generic;

namespace ASM.EmployeeManagement.DataAccess.Dao.WebAPIUI.GetUserInfoList
{
	/// <summary>
	/// IGetUserInfoListDao
	/// </summary>
	public interface IGetUserInfoListDao : IWebAPIUICommonDao
	{
		List<UserInfoList> GetUserInfoList(DataAccess.Common.Paging.Paging inputDataPaging, DataAccess.Common.FilterInfo.UserFilterInfo iFilterInfo);
	}
}
