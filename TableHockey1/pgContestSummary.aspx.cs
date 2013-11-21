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

                    //Check if caller requests the page in anonymous mode.

                    bool m_bAnonymousMode = false;
                    if (!String.IsNullOrEmpty(Request.QueryString["AnonymousMode"]))
                        m_bAnonymousMode = true;

                    if ((user != null) || (m_bAnonymousMode))
                    {
                        using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
                        {
                            try
                            {
                                var querySingleContest = (from c in context.TableHockeyContests
                                                          where c.ContestId == m_nContestId
                                                          select c).FirstOrDefault();
                                //c.OwnerUserId == (Guid)user.ProviderUserKey &&
                                var queryEndGameContest = (from c in context.TableHockeyContests
                                                           where c.EndGameForContestId == m_nContestId
                                                           select c).FirstOrDefault();
                                // where c.OwnerUserId == (Guid)user.ProviderUserKey &&

                                if (querySingleContest != null)
                                {
                                    if (querySingleContest.EndGameForContestId == null)
                                    {
                                        TableViewModel m_tableViewModel = new TableViewModel(m_nContestId, 100);
                                        this.ucContestTable1.InitControl(m_tableViewModel.m_tableRows, false, 100, false);
                                        displayRounds(m_nContestId);
                                    }
                                    else
                                    {
                                        this.ucContestTable1.Visible = false;
                                        this.divExportTable.Visible = false;
                                        this.divUcContestRoundPlaceHolder.Visible = false;
                                        displayEndGameRounds(querySingleContest.ContestId);
                                    }

                                }
                                if (queryEndGameContest != null)
                                {
                                    //We have end game rounds, display them all.
                                    displayEndGameRounds(queryEndGameContest.ContestId);
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

        private void displayRounds(int i_nContestId)
        {
            List<TableHockeyContestRound> m_rounds;
            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
            {

                //Get any end game rounds for current contest.

                m_rounds = (from r in context.TableHockeyContestRounds
                            where ((r.ContestId == i_nContestId))
                            select r).OrderBy(p => p.RoundNumber).ToList();

                //Get entered results from uc and update games for table number.
                Session["pgContestSummary.ucRoundList"] = null;
                this.RoundRepeater.DataSource = m_rounds;
                this.RoundRepeater.DataBind();
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
                            select r).OrderByDescending(p => p.RoundNumber).ToList();

                //Get player count for current end game contest.

                var contestPlayersQuery = from p in context.TableHockeyPlayer
                                          join cp in context.TableHockeyContestPlayers on p.PlayerId equals cp.PlayerId
                                          where cp.ContestId == i_nContestId
                                          select new { p.PlayerId };

                int m_nPlayerCount = contestPlayersQuery.ToList().Count();

                //Get entered results from uc and update games for table number.
                Session["pgContestSummary.ucList"] = null;
                Session["pgContestSummary.previousRound"] = null;
                Session["pgContestSummary.m_nNumberOfRounds"] = m_rounds.Count;
                Session["pgContestSummary.m_nPlayerCount"] = m_nPlayerCount;
                this.ResultsRepeater.DataSource = m_rounds;
                this.ResultsRepeater.DataBind();
                return isFinalRound;
            }
        }

        protected void RoundRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                PlaceHolder PlaceHolder2 = (PlaceHolder)e.Item.FindControl("PlaceHolder2");

                // declare/obtain the value of i given the DataItem
                // e.g.,
                TableHockeyContestRound m_round = (TableHockeyContestRound)e.Item.DataItem;
                List<uc.ucViewContestRound> m_ucRoundList;
                if (Session["pgContestSummary.ucRoundList"] != null)
                    m_ucRoundList = (List<uc.ucViewContestRound>)Session["pgContestSummary.ucRoundList"];
                else
                    m_ucRoundList = new List<uc.ucViewContestRound>();

                uc.ucViewContestRound uc = (uc.ucViewContestRound)LoadControl("~/uc/ucViewContestRound.ascx");
                uc.InitControl(m_round);
                m_ucRoundList.Add(uc);
                Session["pgContestSummary.ucRoundList"] = m_ucRoundList;
                if (m_round.isFinalRound)
                {
                    foreach (uc.ucViewContestRound m_uc in m_ucRoundList)
                        PlaceHolder2.Controls.Add(m_uc);
                    string m_sRoundHtml = GetSummary(divUcContestRoundPlaceHolder);
                    Session["pgContestSummary.roundHTML"] = m_sRoundHtml;
                }
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
                Dictionary<int, uc.ucViewEndGameSeries> m_ucList;
                if (Session["pgContestSummary.ucList"] != null)
                    m_ucList = (Dictionary<int, uc.ucViewEndGameSeries>)Session["pgContestSummary.ucList"];
                else
                    m_ucList = new Dictionary<int, uc.ucViewEndGameSeries>();

                for (int m_nCurrentTableNumber = 1; m_nCurrentTableNumber <= m_nMaxNumberOfConcurrentGames; m_nCurrentTableNumber++)
                {
                    List<TableHockeyGame> m_gamesOnCurrentTable = m_round.TableHockeyGames.Where(g => g.TableNumber == m_nCurrentTableNumber).ToList();
                    if (m_gamesOnCurrentTable.Count > 0)
                    {
                        TableHockeyContestRound m_roundGame = new TableHockeyContestRound();
                        Mapper.Map(m_round, m_roundGame);
                        m_roundGame.TableHockeyGames = m_gamesOnCurrentTable;
                        uc.ucViewEndGameSeries uc = (uc.ucViewEndGameSeries)LoadControl("~/uc/ucViewEndGameSeries.ascx");
                        uc.InitControl(m_roundGame, 25 * (m_round.RoundNumber - 1));
                        uc.m_nTableNumber = m_nCurrentTableNumber;
                        int m_nNumberOfRounds = Convert.ToInt32(Session["pgContestSummary.m_nNumberOfRounds"]);
                        //Get qualifying players from current round.
                        Dictionary<int, int> m_playersToNextRound = TableHockeyContestHandler.getPlayersToNextRound(m_round, m_gamesOnCurrentTable.Count);
                        if (m_playersToNextRound.Count > 0)
                        {
                            //Get qualifying player for current table number.
                            int m_nQualifyingPlayerForCurrentTableNumber = m_playersToNextRound[m_nCurrentTableNumber];
                            if (m_round.isFinalRound)
                            {
                                m_ucList.Add(8, uc);
                                Session["pgContestSummary.ucList"] = m_ucList;
                            }
                            if (m_round.RoundNumber == (m_nNumberOfRounds - 1))
                            {
                                //Get previous round.
                                TableHockeyContestRound m_prevRound = (TableHockeyContestRound)Session["pgContestSummary.previousRound"];
                                //Match qualifying player id with either home or away player.
                                if ((m_nCurrentTableNumber == 1) && ((m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).HomePlayerId) || (m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).AwayPlayerId)))
                                    m_ucList.Add(4, uc);
                                if ((m_nCurrentTableNumber == 2) && ((m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).HomePlayerId) || (m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).AwayPlayerId)))
                                    m_ucList.Add(12, uc);
                                //TODO: Analogt för följande omgångar. Traversera över m_playersToNextRound
                                Session["pgContestSummary.ucList"] = m_ucList;
                            }
                            if (m_round.RoundNumber == (m_nNumberOfRounds - 2))
                            {
                                //Get previous round.
                                TableHockeyContestRound m_prevRound = (TableHockeyContestRound)Session["pgContestSummary.previousRound"];
                                List<TableHockeyGame> m_prevGamesOnCurrentTable = m_prevRound.TableHockeyGames.Where(g => g.TableNumber == m_nCurrentTableNumber).ToList();
                                if ((m_nCurrentTableNumber == 1) &&
                                    ((m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).HomePlayerId) || (m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).AwayPlayerId) || (m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(m_gamesOnCurrentTable.Count).HomePlayerId) || (m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(m_gamesOnCurrentTable.Count).AwayPlayerId)))
                                    m_ucList.Add(2, uc);
                                if ((m_nCurrentTableNumber == 2) &&
                                    ((m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).HomePlayerId) || (m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).AwayPlayerId) || (m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(m_gamesOnCurrentTable.Count).HomePlayerId) || (m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(m_gamesOnCurrentTable.Count).AwayPlayerId)))
                                    m_ucList.Add(6, uc);
                                if ((m_nCurrentTableNumber == 3) &&
                                    ((m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).HomePlayerId) || (m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).AwayPlayerId) || (m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(m_gamesOnCurrentTable.Count).HomePlayerId) || (m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(m_gamesOnCurrentTable.Count).AwayPlayerId)))
                                    m_ucList.Add(10, uc);
                                if ((m_nCurrentTableNumber == 4) &&
                                    ((m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).HomePlayerId) || (m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).AwayPlayerId) || (m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(m_gamesOnCurrentTable.Count).HomePlayerId) || (m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(m_gamesOnCurrentTable.Count).AwayPlayerId)))
                                    m_ucList.Add(14, uc);
                                Session["pgContestSummary.ucList"] = m_ucList;
                            }
                            if (m_round.RoundNumber == (m_nNumberOfRounds - 3))
                            {
                                //Get previous round.
                                //TODO:  Fixa för åttondel analogt med kvartsfinal ovan! Gnetigt men det går! 2013-07-23.
                                TableHockeyContestRound m_prevRound = (TableHockeyContestRound)Session["pgContestSummary.previousRound"];
                                List<TableHockeyGame> m_prevGamesOnCurrentTable = m_prevRound.TableHockeyGames.Where(g => g.TableNumber == m_nCurrentTableNumber).ToList();
                                if ((m_nCurrentTableNumber == 1) && ((m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).HomePlayerId) || (m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).AwayPlayerId)))
                                    m_ucList.Add(1, uc);
                                if ((m_nCurrentTableNumber == 2) && ((m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).HomePlayerId) || (m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).AwayPlayerId)))
                                    m_ucList.Add(3, uc);
                                if ((m_nCurrentTableNumber == 3) && ((m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).HomePlayerId) || (m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).AwayPlayerId)))
                                    m_ucList.Add(5, uc);
                                if ((m_nCurrentTableNumber == 4) && ((m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).HomePlayerId) || (m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).AwayPlayerId)))
                                    m_ucList.Add(7, uc);
                                if ((m_nCurrentTableNumber == 5) && ((m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).HomePlayerId) || (m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).AwayPlayerId)))
                                    m_ucList.Add(9, uc);
                                if ((m_nCurrentTableNumber == 6) && ((m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).HomePlayerId) || (m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).AwayPlayerId)))
                                    m_ucList.Add(11, uc);
                                if ((m_nCurrentTableNumber == 7) && ((m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).HomePlayerId) || (m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).AwayPlayerId)))
                                    m_ucList.Add(13, uc);
                                if ((m_nCurrentTableNumber == 8) && ((m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).HomePlayerId) || (m_nQualifyingPlayerForCurrentTableNumber == m_prevRound.TableHockeyGames.ElementAt(0).AwayPlayerId)))
                                    m_ucList.Add(15, uc);
                                Session["pgContestSummary.ucList"] = m_ucList;
                            }
                        }
                    }
                }
                Session["pgContestSummary.previousRound"] = m_round;
                if (m_round.isFirstRound)
                {
                    for (int iCount = 1; iCount < 15; iCount++)
                    {
                        uc.ucViewEndGameSeries m_uc;
                        if (m_ucList.ContainsKey(iCount))
                        {
                            m_uc = m_ucList[iCount];
                            PlaceHolder1.Controls.Add(m_uc);
                        }
                    }
                    string m_sEndGameHtml = GetSummary(divUcEndGamePlaceHolder);
                    Session["pgContestSummary.endGameHTML"] = m_sEndGameHtml;
                }
            }
        }

        protected void ButtonExportToPdf_Click(object sender, EventArgs e)
        {
            string m_sHtml = GetSummary(divExportTable);
            if (Session["pgContestSummary.roundHTML"] != null)
                m_sHtml += Convert.ToString(Session["pgContestSummary.roundHTML"]);
            if (Session["pgContestSummary.endGameHTML"] != null)
                m_sHtml += Convert.ToString(Session["pgContestSummary.endGameHTML"]);

            System.IO.MemoryStream m_stream = new System.IO.MemoryStream();
            EO.Pdf.HtmlToPdf.ConvertHtml(m_sHtml, m_stream);
            Response.Clear();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "Attachment; filename=TableHockeyContest.pdf");
            Response.BinaryWrite(m_stream.GetBuffer());
        }

        protected void ImageButtonShare_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}