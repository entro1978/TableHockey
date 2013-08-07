using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace TableHockey.Account
{
    public partial class EMailPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

       


        protected void PasswordRecovery1_SendingMail(object sender, MailMessageEventArgs e)
        {
            e.Cancel = true;
             // Get the membership details for the user
            
            MembershipUser user = Membership.GetUser(PasswordRecovery1.UserName.Trim());
            
            // Did we find the user?
            if (user != null)
            {
                PageUtility.sendGMailMsgFromSite("New password for user " + user.UserName, user.ResetPassword(), user.Email, user.UserName); 
            }
        }
    }
}