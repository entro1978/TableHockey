using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace TableHockey
{
    public partial class pgMain : CheckAuth
    {
        protected void Page_Load(object sender, EventArgs e)
        {                     
            DataShow();                
        }

        private void DataShow()
        {
            MembershipUser CurrentUser = Membership.GetUser(User.Identity.Name);
            Guid m_providerUserKey = (Guid)CurrentUser.ProviderUserKey;

            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
            {
                var query = from c in context.TableHockeyContest 
                            where c.OwnerUserId == m_providerUserKey
                            select new { c.ContestId, c.ContestName, c.ContestDescription, c.ContestLocation, c.ContestDateOpened, c.ContestDateClosed };
                this.GridViewContests.DataSource = query.ToList();
                this.GridViewContests.DataBind();
                divContests.Visible = (query.Count() > 0);
                divContestsHeader.Visible = (query.Count() > 0);
                var playerQuery = from p in context.TableHockeyPlayer
                                  join c in context.TableHockeyClub on p.ClubId equals c.ClubId
                                  select new { p.PlayerId, p.FirstName, p.LastName, p.BirthDate, c.ClubName, p.PlayerBinary, p.RegisteredByUserId, c.ClubId, p.isHistoric };

                this.GridViewPlayers.DataSource = playerQuery.ToList().Where(p => p.PlayerId >= 0).Where(p => p.isHistoric != 1);
                this.GridViewPlayers.DataBind();
            }

            //Clear any session for previous contest.
            Session["ucContestTable.previousTable"] = null;
            Session["pgEditContestTable.m_nCurrentRoundNumber"] = null;
            Session["pgEditContestTableEndGame.m_nCurrentRoundNumber"] = null;
            Session["pgEditContestTableEndGame.m_rounds"] = null;
            Session["ucEndGameSeries.m_round"] = null;
            Session["pgEditContestTableEndGame.queueHandler"] = null; 
        }
    }

}
