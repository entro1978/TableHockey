using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;


namespace TableHockey
{
    public partial class Login : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
        }

        protected void LoginUser_Authenticate(object sender, AuthenticateEventArgs e)
        {
            // Get the membership details for the user
            MembershipUser user = Membership.GetUser(LoginUser.UserName.Trim());

            // Did we find the user?
            if (user != null)
            {
                //  Check if the user password has expired after 90 days 
                if (DateTime.Now.Subtract(user.LastPasswordChangedDate).TotalDays >= 90)
                {
                    Response.Redirect("PasswordExpired.aspx");
                }
                else
                {
                    // Authenticate user
                    if (Membership.ValidateUser(LoginUser.UserName.Trim(), LoginUser.Password.Trim()))
                    {
                        e.Authenticated = true;                      
                    }                    
                }
            }
        }
    }
}

