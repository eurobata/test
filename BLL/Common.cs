using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Services.Description;
using TemplateManagementSystem.DAL;
using TemplateManagementSystem.Models;

namespace TemplateManagementSystem.BLL
{
    public class Common
    {
        TemplateManagementSystemEntities db = new TemplateManagementSystemEntities();
        public List<DropDownListModel> FetchCountries()
        {
            var countries = db.Countries.ToList();
            List<DropDownListModel> lstCountries = new List<DropDownListModel>();
            foreach (var item in countries)
            {
                lstCountries.Add(new DropDownListModel()
                {
                    Id = string.Format("{0}-{1}", item.NiceName, item.PhoneCode),
                    Name = item.NiceName
                });
            }
            return lstCountries;
        }

        public bool SendEmail(string to, string subject, string body, string attachment, string cc)
        {

            var email = new MailMessage(ConfigurationManager.AppSettings["Username"], to)
            {
                Subject = subject,
                Body = body,

                IsBodyHtml = true
            };

            if (!string.IsNullOrEmpty(attachment))
            {
                email.Attachments.Add(new Attachment(attachment, "application/pdf"));
            }

            if (!string.IsNullOrEmpty(cc))
            {
                string[] ccs = cc.Split(',');
                foreach (var item in ccs)
                {
                    if (!string.IsNullOrEmpty(item))
                        email.CC.Add(item);
                }
            }

            bool isSend = false;
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = ConfigurationManager.AppSettings["Host"];
                smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                System.Net.NetworkCredential networkCredential = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["Username"], ConfigurationManager.AppSettings["Password"]);
                smtpClient.Credentials = networkCredential;
                smtpClient.EnableSsl = true;
                smtpClient.Send(email);
                isSend = true;
            }
            catch (Exception ex)
            {

            }
            return isSend;
        }

        public AspNetUser GetById(string userId)
        {
            return db.AspNetUsers.Where(s => s.Id == userId).FirstOrDefault();
        }

        public int FetchTemplateCount(string userId)
        {
            return db.FetchTemplatesCount(userId).FirstOrDefault().Value;
        }

        public int FetchTemplateCount(string userId,string month,string year)
        {
            return db.FetchTemplateCountOfMonth(userId,month,year).FirstOrDefault().Value;
        }

        public string FetchProfilePicture(string userId)
        {
            var user = db.AspNetUsers.Where(s => s.Id == userId).FirstOrDefault();
            string profilePicture = "/assets/images/Avatar.png";
            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.ProfilePicture))
                {
                    profilePicture = "/pics/" + user.ProfilePicture;
                }
            }
            return profilePicture;
        }
    }
}