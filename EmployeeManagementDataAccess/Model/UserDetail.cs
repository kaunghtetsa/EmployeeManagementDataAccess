
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
    
public partial class UserDetail
{

    public long UserID { get; set; }

    public byte Gender { get; set; }

    public System.DateTime DateOfBirth { get; set; }

    public string Address { get; set; }

    public string Email { get; set; }

    public string PhoneNo { get; set; }

    public Nullable<System.DateTime> JobStartDate { get; set; }

    public Nullable<System.DateTime> JobResignDate { get; set; }

    public byte MaritalStatus { get; set; }



    public virtual User User { get; set; }

}

}
