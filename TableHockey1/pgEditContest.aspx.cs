using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TableHockeyData;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using AutoMapper;
using System.Web.Security;

namespace TableHockey
{
    public partial class pgEditContest : CheckAuth
    {
        static pgEditContest()
        {
            Mapper.CreateMap<TableHockeyContest, TableHockeyContest>().ForMember("ContestId", r => r.Ignore());
        }

        protected TableHockeyData.TableHockeyContest m_ContestContext;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                m_ContestContext = new TableHockeyContest();
                //Get contest details for existing contest or create new.  Init contest uc
                int m_nContestId = -1;
                if (!String.IsNullOrEmpty(Request.QueryString["ContestId"]))  //TODO: Integer validation.
                {
                    //Get existing contest and init uc.
                    m_nContestId = Convert.ToInt32(Request.QueryString["ContestId"]);

                    MembershipUser user = Membership.GetUser(m_sUser.Trim());
                    if (user != null)
                    {
                        using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
                        {
                            try
                            {
                                var querySingleContest = (from c in context.TableHockeyContests
                                                          where c.OwnerUserId == (Guid)user.ProviderUserKey &&
                                                                c.ContestId == m_nContestId
                                                          select c).FirstOrDefault();

                                if (querySingleContest.EndGameForContestId != null)
                                { 
                                   //It's an end game for another contest, redirect to end game page edit.
                                    Response.Redirect("~/pgEditContestTableEndGame.aspx?ContestId=" + Convert.ToString(m_nContestId), false);
                                }

                                if (querySingleContest != null)
                                   this.ucEditTableHockeyContest1.InitControl((TableHockeyContest)querySingleContest);
                                else
                                    this.ucEditTableHockeyContest1.InitControl(null);
                            }
                            catch (Exception ex)
                            {
                                Response.Redirect("~/pgEditContest.aspx");
                            }
                        }
                        divAddPlayers.Visible = true;
                    }            
                }
                else
                {
                    //Init uc for new contest
                    divAddPlayers.Visible = false;
                    this.ucEditTableHockeyContest1.InitControl(null);
                }
            } else 
            {

            }
            
        }

        protected void ButtonSaveContest_Click(object sender, EventArgs e)
        {
            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
            {
                if (this.ucEditTableHockeyContest1.m_currentContest.ContestId == -1)
                {
                    //New contest
                    context.TableHockeyContests.Add(this.ucEditTableHockeyContest1.m_currentContest);
                }
                else
                {
                    //Edit existing contest
                    TableHockeyContest m_currentContest = context.TableHockeyContests.FirstOrDefault(i => i.ContestId == this.ucEditTableHockeyContest1.m_currentContest.ContestId);
                    //Mapper.Map(this.ucEditTableHockeyContest1.m_currentContest, m_currentContest);  
                    m_currentContest.ContestBinary = this.ucEditTableHockeyContest1.m_currentContest.ContestBinary;
                    m_currentContest.ContestDateClosed = this.ucEditTableHockeyContest1.m_currentContest.ContestDateClosed;
                    m_currentContest.ContestDateOpened = this.ucEditTableHockeyContest1.m_currentContest.ContestDateOpened;
                    m_currentContest.ContestDescription = this.ucEditTableHockeyContest1.m_currentContest.ContestDescription;
                    m_currentContest.ContestLocation = this.ucEditTableHockeyContest1.m_currentContest.ContestLocation;
                    m_currentContest.ContestName = this.ucEditTableHockeyContest1.m_currentContest.ContestName;
                    m_currentContest.GameLengthMinutes = this.ucEditTableHockeyContest1.m_currentContest.GameLengthMinutes;
                    m_currentContest.isFinalGameContest = this.ucEditTableHockeyContest1.m_currentContest.isFinalGameContest;
                    m_currentContest.isGoalDifferenceRanked = this.ucEditTableHockeyContest1.m_currentContest.isGoalDifferenceRanked;
                    m_currentContest.NumberOfPlayersToNextRound = this.ucEditTableHockeyContest1.m_currentContest.NumberOfPlayersToNextRound;
                    m_currentContest.numberOfRounds = this.ucEditTableHockeyContest1.m_currentContest.numberOfRounds;
                    m_currentContest.PointsLostGame = this.ucEditTableHockeyContest1.m_currentContest.PointsLostGame;
                    m_currentContest.PointsTiedGame = this.ucEditTableHockeyContest1.m_currentContest.PointsTiedGame;
                    m_currentContest.PointsWinningGame = this.ucEditTableHockeyContest1.m_currentContest.PointsWinningGame;
                    //Also update closing date for any linked end game contest!
                    TableHockeyContest m_currentEndGameContest = context.TableHockeyContests.FirstOrDefault(i => i.EndGameForContestId == this.ucEditTableHockeyContest1.m_currentContest.ContestId);
                    if (m_currentEndGameContest != null)
                    {
                        m_currentEndGameContest.ContestDateClosed = this.ucEditTableHockeyContest1.m_currentContest.ContestDateClosed;
                    }
                }
                context.SaveChanges();
                Response.Redirect("~/pgMain.aspx");
            }

            //List<string> list = new List<string>();
            //list.Add("test");
            //list.Where(p => p.StartsWith("t")).ToList();

            //var test = from f in list
            //           where f.StartsWith("t")
            //           select f;


        }

        protected void ButtonAddPlayers_Click(object sender, EventArgs e)
        {
            //Navigate to new page that lets user add/remove players to selected contest.
            Response.Redirect("~/pgEditContestPlayers.aspx?ContestId=" + Convert.ToString(this.ucEditTableHockeyContest1.m_currentContest.ContestId));
        }
    }
}
