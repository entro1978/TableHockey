using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TableHockeyData;
using TableHockey;

namespace TableHockey
{
    public partial class pgEditContestPlayers : CheckAuth
    {
        protected int m_nContestId;

        private void populateAvailablePlayers(int i_nContestId)
        {
            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
            {

                var availablePlayersQuery = from p in context.TableHockeyPlayer
                                            join c in context.TableHockeyClubs on p.ClubId equals c.ClubId
                                            where !(from cp in context.TableHockeyContestPlayers
                                                    where cp.ContestId == i_nContestId
                                                    select cp.PlayerId).Contains(p.PlayerId)
                                            select new { p.PlayerId, p.FirstName, p.LastName, p.BirthDate, c.ClubName, p.PlayerBinary, p.isHistoric };

                this.GridViewAvailablePlayers.DataSource = availablePlayersQuery.ToList().Where(p => p.PlayerId >= 0).Where(p => p.isHistoric != 1);
                this.GridViewAvailablePlayers.DataBind();
            }
        }

        private void populateSelectedPlayers(int i_nContestId)
        {
            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
            {
                var contestPlayersQuery = from p in context.TableHockeyPlayer
                                          join c in context.TableHockeyClubs on p.ClubId equals c.ClubId
                                          join cp in context.TableHockeyContestPlayers on p.PlayerId equals cp.PlayerId
                                          where (cp.ContestId == i_nContestId)
                                          select new { p.PlayerId, p.FirstName, p.LastName, p.BirthDate, c.ClubName, p.PlayerBinary };

                this.GridViewSelectedPlayers.DataSource = contestPlayersQuery.ToList().Where(p => p.PlayerId >= 0);
                this.GridViewSelectedPlayers.DataBind();
                divStartContest.Visible = (GridViewSelectedPlayers.Rows.Count >= 2);
            }
        }

        private void setContestHeader(int i_nContestId)
        {
            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
            {
                TableHockeyContest m_contest = context.TableHockeyContests.First(c => c.ContestId == i_nContestId);
                ViewModels.ContestViewModel m_contestViewModel = new ViewModels.ContestViewModel(m_contest);
                LabelContestHeader.Text = m_contestViewModel.ContestHeader;
                LabelContestDescription.Text = m_contestViewModel.ContestDescription;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                m_nContestId = -1;
                if (!String.IsNullOrEmpty(Request.QueryString["ContestId"]))
                {
                    m_nContestId = Convert.ToInt32(Request.QueryString["ContestId"]);
                    Session["pgEditContestPlayers.m_nContestId"] = m_nContestId;
                    setContestHeader(m_nContestId);
                    populateAvailablePlayers(m_nContestId);
                    populateSelectedPlayers(m_nContestId);
                }
                else
                {
                    Response.Redirect("~/pgMain.aspx");
                }
            }
        }

        protected void ButtonAddSelectedPlayersToContest_Click(object sender, EventArgs e)
        {
            Dictionary<int,int> m_selectedPlayerIDs = GetCheckedPlayerIndexes(GridViewAvailablePlayers, "checkSelectPlayer");
            if (Session["pgEditContestPlayers.m_nContestId"] != null)
            {
                m_nContestId = Convert.ToInt32(Session["pgEditContestPlayers.m_nContestId"]);
            }
            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
            {
                foreach (int m_nPlayerId in m_selectedPlayerIDs.Values)
                {
                    TableHockeyContestPlayer m_contestPlayer = new TableHockeyContestPlayer();
                    m_contestPlayer.ContestId = m_nContestId;
                    m_contestPlayer.PlayerId = m_nPlayerId;
                    m_contestPlayer.isDummyContestPlayer = false;
                    context.TableHockeyContestPlayers.Add(m_contestPlayer);
                }
                context.SaveChanges();
            }

            if (m_selectedPlayerIDs.Count > 0)
            {
                populateAvailablePlayers(m_nContestId);
                populateSelectedPlayers(m_nContestId);
            }
        }

        private bool isPlayerOKToRemoveFromContest(int i_nPlayerId, int i_nContestId)
        {
            //Check for contest games for player.  Also check that contest is still open.

            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
            {
                TableHockeyGame m_tableHockeyGame = context.TableHockeyGame.FirstOrDefault(c => ((c.HomePlayerId == i_nPlayerId) || (c.AwayPlayerId == i_nPlayerId)) && (c.ContestId == m_nContestId));
                TableHockeyContest m_tableHockeyContest = context.TableHockeyContests.First(c => c.ContestId == i_nContestId);
                return ((m_tableHockeyGame == null) && (m_tableHockeyContest.ContestDateClosed.Equals(DateTime.Parse("1900-01-01"))));
            }
        }

        protected void ButtonRemoveSelectedPlayersFromContest_Click(object sender, EventArgs e)
        {
            Dictionary<int,int> m_selectedPlayerIDs = GetCheckedPlayerIndexes(GridViewSelectedPlayers, "checkSelectPlayer");
            if (Session["pgEditContestPlayers.m_nContestId"] != null)
            {
                m_nContestId = Convert.ToInt32(Session["pgEditContestPlayers.m_nContestId"]);
            }
            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
            {
                foreach (int m_nPlayerId in m_selectedPlayerIDs.Values)
                {
                    if (isPlayerOKToRemoveFromContest(m_nPlayerId, m_nContestId))
                    {
                        TableHockeyContestPlayer m_contestPlayer = context.TableHockeyContestPlayers.First(c => (c.PlayerId == m_nPlayerId) && (c.ContestId == m_nContestId));
                        context.TableHockeyContestPlayers.Remove(m_contestPlayer);
                    }
                }
                context.SaveChanges();
            }
            if (m_selectedPlayerIDs.Count > 0)
            {
                populateAvailablePlayers(m_nContestId);
                populateSelectedPlayers(m_nContestId);
            }
        }

        protected void ButtonStartContest_Click(object sender, EventArgs e)
        {

            //Redirect to contest start page.
            if (Session["pgEditContestPlayers.m_nContestId"] != null)
            {
                m_nContestId = Convert.ToInt32(Session["pgEditContestPlayers.m_nContestId"]);
            }

            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
            {
                int m_nNumberOfPlayers = context.TableHockeyContestPlayers.Where(c => (c.PlayerId != -1) && (c.ContestId == m_nContestId)).Count();
                //If odd number of players, add extra dummy player (if none exists, shouldn't happen). This is a temporary fix that may be altered in the future. 120507.
                if (PageUtility.isOdd(m_nNumberOfPlayers))
                {
                    TableHockeyContestPlayer m_existingDummyPlayer = context.TableHockeyContestPlayers.FirstOrDefault(c => (c.PlayerId == -1) && (c.ContestId == m_nContestId));
                    if (m_existingDummyPlayer == null)
                    {
                        TableHockeyContestPlayer m_dummyPlayer = new TableHockeyContestPlayer();
                        m_dummyPlayer.ContestId = m_nContestId;
                        m_dummyPlayer.PlayerId = -1;
                        m_dummyPlayer.isDummyContestPlayer = true;
                        context.TableHockeyContestPlayers.Add(m_dummyPlayer);
                    }                  
                    context.SaveChanges();
                }
            }

            Response.Redirect("~/pgEditContestTable.aspx?ContestId=" + m_nContestId);
        }
    }
}