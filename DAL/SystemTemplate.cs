
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
    
public partial class SystemTemplate
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public SystemTemplate()
    {

        this.TemplateCategories = new HashSet<TemplateCategory>();

    }


    public int Id { get; set; }

    public string TemplateName { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<TemplateCategory> TemplateCategories { get; set; }

}

}
