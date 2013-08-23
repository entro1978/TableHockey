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

namespace TableHockey.uc
{
    public partial class ucViewContests_public : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void InitControl(List<TableHockeyContestViewModel> i_lstContestVm)
        {
            this.GridViewContestsExternal.DataSource = i_lstContestVm;
            this.GridViewContestsExternal.DataBind();
        }
    }
}