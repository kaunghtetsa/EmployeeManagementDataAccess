using System;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ASM.EmployeeManagement.DataAccess.Dao;

namespace EmployeeManagementDataAccessTest.Common
{
    /// <summary>
    /// Commmon
    /// </summary>
    [TestClass]
    public class CommonTest
    {
        private const string BatRecreateDB = @"Database\TestTools\EmployeeManagement_UnitTest_ReCreate.bat";
        private const string BatDropDB = @"Database\TestTools\EmployeeManagement_UnitTest_Drop.bat";

        [AssemblyInitialize]
        public static void SetUp(TestContext test)
        {
            string strProjectPath = GetProjectPath();
            ExecuteBatchFile(Path.Combine(strProjectPath, BatRecreateDB));

            string strConnectionString = ConfigurationManager.ConnectionStrings["EmployeeEntities"].ConnectionString;
            BaseDao.SetConnectionString(strConnectionString);

            if (!BaseDao.CanAccessDB())
            {
                throw new Exception();
            }
        }

        [AssemblyCleanup]
        public static void CleanUp()
        {
            DBCleanUp();
        }

        /// <summary>
        /// Drop Test Database
        /// </summary>
        public static void DBCleanUp()
        {
            string strProjectPath = GetProjectPath();

            ExecuteBatchFile(Path.Combine(strProjectPath, BatDropDB));
        }

        /// <summary>
        /// batファイルを実行する
        /// </summary>
        /// <param name="batFilePath"></param>
        private static void ExecuteBatchFile(string batFilePath)
        {
            try
            {
                Process proc = new Process();
                proc.StartInfo.FileName = batFilePath;
                proc.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                proc.StartInfo.ErrorDialog = false;
                proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(batFilePath);
                proc.Start();
                proc.WaitForExit();
                proc.Close();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Get setting file path
        /// </summary>
        /// <returns></returns>
        private static string GetProjectPath()
        {
            string strProjectPath = Path.GetDirectoryName(
                Path.GetDirectoryName(
                Path.GetDirectoryName(
                Path.GetDirectoryName(
                System.IO.Directory.GetCurrentDirectory()))));

            return strProjectPath;
        }
    }
}
