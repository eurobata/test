
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
    
public partial class CustomerTempleteModel
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public CustomerTempleteModel()
    {

        this.customerItems = new HashSet<customerItem>();

    }


    public int Id { get; set; }

    public string Email { get; set; }

    public string Subject { get; set; }

    public string CC { get; set; }

    public string UserName { get; set; }

    public string userAdress { get; set; }

    public string userCountry { get; set; }

    public string useroffice { get; set; }

    public string userPostal { get; set; }

    public string userPhone { get; set; }

    public string useremail { get; set; }

    public string CustomerName { get; set; }

    public string CustomerAddress { get; set; }

    public string userProfile { get; set; }

    public decimal dscount { get; set; }

    public decimal totall { get; set; }

    public string EmailBody { get; set; }

    public string CreatedBy { get; set; }

    public string ModifiedBy { get; set; }

    public System.DateTime createdAt { get; set; }

    public string Filename { get; set; }

    public Nullable<System.DateTime> ModifiedDate { get; set; }

    public decimal subtotall { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<customerItem> customerItems { get; set; }

}

}