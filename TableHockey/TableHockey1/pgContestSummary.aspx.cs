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
    public partial class pgContestSummary : PageUtility
    {

        static pgContestSummary()
        {
            Mapper.CreateMap<TableHockeyContest, TableHockeyContest>();
            //Mapper.CreateMap<TableHockeyContestRound, TableHockeyContestRound>();
            Mapper.CreateMap<IEnumerable<TableHockeyGame>, IEnumerable<TableHockeyGame>>();
        }

        protected TableHockeyData.TableHockeyContest m_ContestContext;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                m_ContestContext = new TableHockeyContest();
                //Get contest details for existing contest or create new.  Init contest uc
                int m_nContestId = -1;
                if (!String.IsNullOrEmpty(Request.QueryString["ContestId"])) 
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
                                var querySingleContest = (from c in context.TableHockeyContest
                                                          where c.OwnerUserId == (Guid)user.ProviderUserKey &&
                                                                c.ContestId == m_nContestId
                                                          select c).FirstOrDefault();
                                if (querySingleContest != null)
                                {
                                    TableViewModel m_tableViewModel = new TableViewModel(m_nContestId, 100);
                                    this.ucContestTable1.InitControl(m_tableViewModel.m_tableRows,false,100,false);

                                    var queryEndGameContest = (from c in context.TableHockeyContest
                                                               where c.OwnerUserId == (Guid)user.ProviderUserKey &&
                                                               c.EndGameForContestId == m_nContestId
                                                               select c).FirstOrDefault();
                                    if(queryEndGameContest != null)
                                    {
                                        //We have end game rounds, display them all.
                                        displayEndGameRounds(queryEndGameContest.ContestId);
                                    }
                                    
                                }
             
                            }
                            catch (Exception ex)
                            {
                                Response.Redirect("~/pgMain.aspx");
                            }
                        }
                    }
                }
               
            }
            else
            {

            }
        }

        private bool displayEndGameRounds(int i_nContestId)
        {
            //Populate contest round uc.  Get data for current round.
            bool isFinalRound = false;

            List<TableHockeyContestRound> m_rounds;
            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
            {
     
                //Get any end game rounds for current contest.

                m_rounds = (from r in context.TableHockeyContestRounds
                                where ((r.ContestId == i_nContestId))
                                select r).ToList();
                        
                //Get entered results from uc and update games for table number.
                Session["pgContestSummary.ucList"] = null;
                this.ResultsRepeater.DataSource = m_rounds;
                this.ResultsRepeater.DataBind();     
                return isFinalRound;
            }
        }

        protected void ResultsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                PlaceHolder PlaceHolder1 = (PlaceHolder)e.Item.FindControl("PlaceHolder1");

                // declare/obtain the value of i given the DataItem
                // e.g.,
                TableHockeyContestRound m_round = (TableHockeyContestRound)e.Item.DataItem;
                List<uc.ucViewEndGameSeries> m_ucList;
                if (Session["pgContestSummary.ucList"] != null)
                    m_ucList = (List<uc.ucViewEndGameSeries>)Session["pgContestSummary.ucList"];
                else
                    m_ucList = new List<uc.ucViewEndGameSeries>();

                for (int m_nCurrentTableNumber = 1; m_nCurrentTableNumber <= m_nMaxNumberOfConcurrentGames; m_nCurrentTableNumber++)
                {
                    List<TableHockeyGame> m_gamesOnCurrentTable = m_round.TableHockeyGames.Where(g => g.TableNumber == m_nCurrentTableNumber).ToList();
                    if (m_gamesOnCurrentTable.Count > 0)
                    {
                        TableHockeyContestRound m_roundGame = new TableHockeyContestRound();
                        Mapper.Map(m_round, m_roundGame);
                        m_roundGame.TableHockeyGames = m_gamesOnCurrentTable;
                        uc.ucViewEndGameSeries uc = (uc.ucViewEndGameSeries)LoadControl("~/uc/ucViewEndGameSeries.ascx");
                        uc.InitControl(m_roundGame,15*m_round.RoundNumber);
                        uc.m_nTableNumber = m_nCurrentTableNumber;
                        m_ucList.Add(uc);
                        Session["pgContestSummary.ucList"] = m_ucList;            
                        if (m_round.isFinalRound){
                            foreach(uc.ucViewEndGameSeries m_uc in m_ucList.OrderBy(t => t.m_nTableNumber))
                            {
                                PlaceHolder1.Controls.Add(m_uc);
                            }
                        }                   
                    }
                }
            }
        }
    }
}