using System;
using System.Diagnostics;
using System.Security.Permissions;
using System.Runtime.Serialization;
using ASM.EmployeeManagement.DataAccess.Common.Enums;

namespace ASM.EmployeeManagement.DataAccess.Exception
{
    /// <summary>
    /// Authentication Exception
    /// </summary>
    [Serializable]
    public class AuthenticationException : DaoException
    {
        #region Message ID enum

        /// <summary>
        /// MessageID Type
        /// </summary>
        public enum MessageIDType
        {
            E001,       // Deleted user or user does not exist
            E002,       // LoginID or Password does not match
        }

        #endregion

        #region constructor

        /// <summary>
        /// AuthenticationException
        /// </summary>
        /// <param name="messageID"></param>
        /// <param name="nLineNumber"></param>
        public AuthenticationException(MessageIDType messageID, [System.Runtime.CompilerServices.CallerLineNumber] int nLineNumber = 0)
        {
            ErrorId = Enum.GetName(typeof(MessageIDType), messageID);

            StackFrame sf = new StackFrame(1, true);
            SetClassName(sf.GetMethod().ReflectedType.FullName);
            SetLineNumber(nLineNumber);
        }

        #endregion

        #region override method

        /// <summary>
        /// GetObjectData
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        #endregion
    }
}
