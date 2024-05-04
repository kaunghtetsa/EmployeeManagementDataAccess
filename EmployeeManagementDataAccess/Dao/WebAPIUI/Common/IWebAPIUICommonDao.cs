using ASM.EmployeeManagement.DataAccess.Model;

namespace ASM.EmployeeManagement.DataAccess.Dao.WebAPIUI.Common
{
    /// <summary>
    /// IWebAPIUICommonDao
    /// </summary>
    public interface IWebAPIUICommonDao
    {
        /// <summary>
        /// Initialize DataAccess
        /// </summary>
        /// <param name="path"></param>
        void Initialize(string path);

        /// <summary>
        /// Performs login operation
        /// </summary>
        /// <param name="loginID"></param>
        /// <param name="password"></param>
        User Login(string loginID, string password);
    }
}
