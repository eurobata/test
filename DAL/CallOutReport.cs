
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace TemplateManagementSystem.DAL
{

using System;
    using System.Collections.Generic;
    
public partial class CallOutReport
{

    public int Id { get; set; }

    public string ClientName { get; set; }

    public string JobAddress { get; set; }

    public string ContactTelephone { get; set; }

    public Nullable<System.DateTime> Date { get; set; }

    public string EngineerName { get; set; }

    public string DetailOfWork { get; set; }

    public Nullable<System.TimeSpan> ArrivalTime { get; set; }

    public Nullable<System.TimeSpan> DepartTime { get; set; }

    public string CreatedBy { get; set; }

    public Nullable<System.DateTime> CreatedDate { get; set; }

    public string JobNo { get; set; }

      public Nullable<System.DateTime> ModifiedDate { get; set; }

     public string ModifiedBy { get; set; }

    public string FileName { get; set; }

}

}
