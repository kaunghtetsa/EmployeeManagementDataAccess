
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace ASM.EmployeeManagement.DataAccess.Model
{

using System;
    using System.Collections.Generic;
    
public partial class User
{

    public long ID { get; set; }

    public string UserID { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public long DepartmentID { get; set; }

    public Nullable<long> DeleteUserID { get; set; }

    public System.DateTime RegistDateTime { get; set; }

    public Nullable<System.DateTime> UpdateDateTime { get; set; }



    public virtual Department Department { get; set; }

    public virtual UserDetail UserDetail { get; set; }

}

}