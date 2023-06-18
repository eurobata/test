﻿

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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

using System.Data.Entity.Core.Objects;
using System.Linq;


public partial class TemplateManagementSystemEntities : DbContext
{
    public TemplateManagementSystemEntities()
        : base("name=TemplateManagementSystemEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<CallOutReport> CallOutReports { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<DailyTimeSheet> DailyTimeSheets { get; set; }

    public virtual DbSet<Favourite> Favourites { get; set; }

    public virtual DbSet<Template5> Template5 { get; set; }

    public virtual DbSet<TimeSheetDetail> TimeSheetDetails { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<WashingTempletItem> WashingTempletItems { get; set; }

    public virtual DbSet<PressureWashingViewModel> PressureWashingViewModels { get; set; }

    public virtual DbSet<bussinessQuotesItem> bussinessQuotesItems { get; set; }

    public virtual DbSet<BussinessQuotesViewModel> BussinessQuotesViewModels { get; set; }

    public virtual DbSet<customerItem> customerItems { get; set; }

    public virtual DbSet<CustomerTempleteModel> CustomerTempleteModels { get; set; }

    public virtual DbSet<SubscriptionModel> SubscriptionModels { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<TemplateCategory> TemplateCategories { get; set; }

    public virtual DbSet<SystemTemplate> SystemTemplates { get; set; }


    public virtual ObjectResult<Nullable<int>> FetchTemplateCountOfMonth(string userId, string month, string year)
    {

        var userIdParameter = userId != null ?
            new ObjectParameter("userId", userId) :
            new ObjectParameter("userId", typeof(string));


        var monthParameter = month != null ?
            new ObjectParameter("month", month) :
            new ObjectParameter("month", typeof(string));


        var yearParameter = year != null ?
            new ObjectParameter("year", year) :
            new ObjectParameter("year", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("FetchTemplateCountOfMonth", userIdParameter, monthParameter, yearParameter);
    }


    public virtual ObjectResult<FetchTemplateHistory_Result> FetchTemplateHistory(Nullable<System.DateTime> startDate, Nullable<System.DateTime> endDate)
    {

        var startDateParameter = startDate.HasValue ?
            new ObjectParameter("startDate", startDate) :
            new ObjectParameter("startDate", typeof(System.DateTime));


        var endDateParameter = endDate.HasValue ?
            new ObjectParameter("endDate", endDate) :
            new ObjectParameter("endDate", typeof(System.DateTime));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<FetchTemplateHistory_Result>("FetchTemplateHistory", startDateParameter, endDateParameter);
    }


    public virtual ObjectResult<Nullable<int>> FetchTemplatesCount(string userId)
    {

        var userIdParameter = userId != null ?
            new ObjectParameter("userId", userId) :
            new ObjectParameter("userId", typeof(string));


        return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("FetchTemplatesCount", userIdParameter);
    }

}

}

