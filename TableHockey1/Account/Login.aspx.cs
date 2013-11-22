using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using TableHockeyData;
using TableHockey;
using TableHockey.ViewModels;

namespace TableHockey
{
    public partial class Login : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            Datashow();
        }

        private void Datashow()
        {
            List<TableHockeyContest> m_lstTableHockeyContest;
            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
            {
                DateTime m_dDateLimit = DateTime.Today.AddDays(-7);  //TODO:  Setting?!
                m_lstTableHockeyContest = (from c in context.TableHockeyContest
                                           where ((c.ContestDateClosed.Value.CompareTo(m_dDateLimit) > 0) && c.isPubliclyVisible)
                                           select c).ToList();
            }

            List<TableHockeyContestViewModel> m_lstContestVm = new List<TableHockeyContestViewModel>();
            foreach (TableHockeyContest m_contest in m_lstTableHockeyContest)
                m_lstContestVm.Add(new TableHockeyContestViewModel(m_contest));

            if ((m_lstContestVm != null) && (m_lstContestVm.Count > 0))
                this.ucViewContests_public1.InitControl(m_lstContestVm);
            else
                this.divInfo.Visible = false;
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

