using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TableHockey
{
    public class CheckAuth : PageUtility
    {

        protected string _m_sUser;
        public string m_sUser 
        {
            get {
                if (Session["CurrentUser"] != null)
                {
                    _m_sUser = (string)Session["CurrentUser"];
                }
                 return _m_sUser; 
            }
            set { _m_sUser = value; }
        }

        public CheckAuth()
        {
            this.Load += new EventHandler(this.Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["CurrentUser"] = Page.User.Identity.Name;
            if (String.IsNullOrEmpty(m_sUser) || !Page.User.Identity.IsAuthenticated)
                Response.Redirect("~/Account/Login.aspx");
        }
    }
}