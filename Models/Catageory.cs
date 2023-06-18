using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TemplateManagementSystem.Models
{
    public class Catageory
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Plans
    {
        public int amount { get; set; }
        public  string name { get; set; }
        public static Plans SelectedPlan(string plan)
        {
            Plans plans = new Plans();
            /*if (plan == "Enterprise Plan")
            {
                plans.amount = 12999;
                plans.name = plan;
            }
            else if (plan == "Elite Plan")
            {
                plans.amount = 4999;
                plans.name = plan;

            }
            else if (plan == "Premium Plan")
            {
                plans.amount = 1999;
                plans.name = plan;

            }
            else if (plan == "Growing Plan")
            {
                plans.amount = 999;
                plans.name = plan;

            }*/
            if (plan == "Essential Plan")
            {
                plans.amount = 399;
                plans.name = plan;
            }
            /*else if(plan == "Gold Plan")
            {
                plans.amount = 9900;
                plans.name = "Gold Plan";

            }*/
            else
            {

                plans.amount = 10000;
                plans.name = "Arsalan";
                   
            }


            return plans;
        }
    }

}