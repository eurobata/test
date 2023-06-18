using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TemplateManagementSystem.DAL;

namespace TemplateManagementSystem.Models
{
    public class TempletDbContext:DbContext
    {




        public DbSet<PressureWashingViewModel> pressureWashings { get; set; }
        public DbSet<BussinessQuotesViewModel> BussinessQuotes { get; set; }
        public DbSet<CustomerTempleteModel> CustomerTemplete { get; set; }
        public DbSet<RiskManagement> RiskManagements { get; set; }
        public DbSet<ACQuote> ACQuotes { get; set; }
        public DbSet<DomesticSmokeAlarm> DomesticSmokeAlarms { get; set; }
        public DbSet<covid19> Covid19s { get; set; }
        public DbSet<SubscriptionModel> SubscriptionModels { get; set; }
        public DbSet<JobCompletionII> JobCompletionIIs { get; set; }
        public DbSet<JobCompletion> JobCompletions { get; set; }
        public DbSet<CleaningCheckList> CleaningCheckLists { get; set; }
        public DbSet<DomesticSmokeAlarmII> DomesticSmokeAlarmIIs { get; set; }
        public DbSet<ServiceBoiler> ServiceBoilers { get; set; }


        public TempletDbContext()
        
            : base("DefaultConnection") { }

        public static TempletDbContext Create()
        {
            return new TempletDbContext();
        }

    }
}