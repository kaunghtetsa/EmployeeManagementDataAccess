using ASM.EmployeeManagement.DataAccess.Dao.WebAPIUI.Common;
using ASM.EmployeeManagement.DataAccess.Model.WebAPI.GetUserInfo;

namespace ASM.EmployeeManagement.DataAccess.Dao.WebAPIUI.GetUserInfo
{
    /// <summary>
    /// IGetUserInfoDao
    /// </summary>
    public interface IGetUserInfoDao : IWebAPIUICommonDao
    {
        UserDetails GetUserInfo(string userID);
    }
}
