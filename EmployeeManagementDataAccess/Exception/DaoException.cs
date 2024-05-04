using System;
using System.Diagnostics;
using System.Security.Permissions;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

using ASM.EmployeeManagement.Common.Exception;

namespace ASM.EmployeeManagement.DataAccess.Exception
{
    [Serializable]
    public class DaoException : EmployeeException
    {
        #region Message ID enum

        public enum ErrorID
        {
            E999, // Unknown error
            E003, // User does not exist.
			E004, // User email does not exist.
		}

        #endregion

        #region member variables

        /// <summary>
        /// Error ID
        /// </summary>
        public string ErrorId { get; protected set; } = Enum.GetName(typeof(ErrorID), ErrorID.E999);

        /// <summary>
        /// Error
        /// </summary>
        public string[] ErrorParams { get; set; }

        #endregion

        #region constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public DaoException() : base()
        {

        }

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="ex"></param>
        public DaoException(System.Exception ex, [CallerLineNumber] int nLineNumber = 0) : base(ex)
        {
            ErrorId = Enum.GetName(typeof(ErrorID), ErrorID.E999);

            StackFrame sf = new StackFrame(1, true);
            SetClassName(sf.GetMethod().ReflectedType.FullName);
            SetLineNumber(nLineNumber);
        }

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="ex"></param>
        /// <param name="nLineNumber"></param>
        public DaoException(ErrorID errorType, System.Exception ex, [CallerLineNumber] int nLineNumber = 0) : base(ex)
        {
            ErrorId = Enum.GetName(typeof(ErrorID), errorType);

            StackFrame sf = new StackFrame(1, true);
            SetClassName(sf.GetMethod().ReflectedType.FullName);
            SetLineNumber(nLineNumber);
        }

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="errParam"></param>
        /// <param name="nLineNumber"></param>
        public DaoException(ErrorID errorType, string[] errParam, [CallerLineNumber] int nLineNumber = 0)
        {
            StackFrame sf = new StackFrame(1, true);
            SetClassName(sf.GetMethod().ReflectedType.FullName);
            SetLineNumber(nLineNumber);
            Initialize(errorType, errParam);
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

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="messageID"></param>
        /// <param name="errParam"></param>
        private void Initialize(ErrorID errorType, string[] errParam)
        {
            ErrorId = Enum.GetName(typeof(ErrorID), errorType);
            ErrorParams = errParam;
        }

        #endregion
    }
}
