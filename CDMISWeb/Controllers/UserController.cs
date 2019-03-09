using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using CDMISWeb.Models;
//using System.Net;

namespace CDMISWeb.Controllers
{
    public class UserController : Controller
    {
        //Registration action
        [HttpGet]

        public ActionResult Registration()
        {
            return View();
        }

        //Registration post action

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified,ActivationCode")] User user)
        {
            bool Status = false;
            string message = "";

            //Model validation

            if (ModelState.IsValid)

            {
                #region Email exists or not
                var isExist = IsEmailExist(user.EmailID);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExists", "Email already exist");
                    return View(user);
                }
                #endregion

                #region Generate activation code

                user.ActivationCode = Guid.NewGuid();
                #endregion

                #region password hashing

                user.Password = Crypto.Hash(user.Password);
                user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);

                #endregion

                user.IsEmailVerified = false;

                #region Save to database

                using (CDMISWebDbEntities de = new CDMISWebDbEntities())
                {
                    de.Users.Add(user);
                    de.SaveChanges();

                    SendVerificationLinkEmail(user.EmailID, user.ActivationCode.ToString());
                    message = "Registration successfully done. Account activation link is sent to your email id:" +
                        user.EmailID;
                    Status = true;

                }
                
                #endregion





                #region MyRegion

                #endregion


            }
            else
            {
                message = "Invalid request";
            }
            ViewBag.Message = message;
            ViewBag.Status = Status;
            
            
            return View(user);
        }

        //Verify Email

        // Login

        // Login POST

        //Logout

        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (CDMISWebDbEntities de = new CDMISWebDbEntities())
            {
                var v = de.Users.Where(a => a.EmailID == emailID).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode)
        {
            var verifyURL = "/User/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyURL);

            var fromEmail = new MailAddress("vishnu.cec@gmail.com", "VKT");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "1EF&ri5$";
            string subject = "Your account is successfully created!";

            string body = "<br></br>We are excited to tell you that your account is created.";

            //smtp client
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false, 
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);

           // var scheme = Request.Url.Scheme;
           // var host = Request.Url.Host;
           // var port = Request.Url.Port;

           // string url = scheme + "://" + host + 
                


        }
    }

    
}