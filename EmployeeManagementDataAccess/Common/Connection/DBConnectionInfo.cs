using System;

namespace ASM.EmployeeManagement.DataAccess.Common.Connection
{
    public class DBConnectionInfo
    {
        // <summary>
        /// データベースサーバー名
        /// </summary>
        public String DBServer { get; set; }

        /// <summary>
        /// データベース名
        /// </summary>
        public String DBName { get; set; }

        /// <summary>
        /// ユーザ名
        /// </summary>
        public String UserId { get; set; }

        /// <summary>
        /// パスワード
        /// </summary>
        public String Password { get; set; }
    }
}
