using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TableHockey;
using TableHockeyData;
using TableHockey.ViewModels;
using AutoMapper;

namespace TableHockey
{
    public partial class pgEditContestTableEndGame : PageUtility
    {
        protected int m_nContestId;
        protected int m_nNumberOfPlayersLowerRotationLimit;
        protected int m_nNumberOfRounds;
        protected int m_nCurrentRoundNumber;

        protected string m_sContestRoundPlaceholder;
        TableHockeyContest m_currentContest;
        TableHockeyEndGameQueueHandler m_queueHandler;

        static pgEditContestTableEndGame()
        {
            Mapper.CreateMap<TableHockeyContestRound, TableHockeyContestRound>();
            Mapper.CreateMap<IEnumerable<TableHockeyGame>, IEnumerable<TableHockeyGame>>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Get contest id.
            if (!Page.IsPostBack)
            {
                m_nContestId = -1;
                m_sContestRoundPlaceholder = "Contest round [ROUND]"; //TODO:  Config setting!
                if (!String.IsNullOrEmpty(Request.QueryString["ContestId"]))
                {
                    m_nContestId = Convert.ToInt32(Request.QueryString["ContestId"]);
                    Session["pgEditContestTableEndGame.m_nContestId"] = m_nContestId;
                    //Get current contest
                    using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
                    {
                        m_currentContest = context.TableHockeyContest.FirstOrDefault(i => i.ContestId == m_nContestId);
                    }

                    if (m_currentContest.EndGameForContestId == null)
                        Response.Redirect("~/pgMain.aspx");

                    Session["pgEditContestTableEndGame.m_currentContest"] = m_currentContest;
                    //Find or regenerate contest end game rounds. Just generate 1 + ([number of players] - 1) / 2 rounds.
                    Session["pgEditContestTable.m_currentContest"] = m_currentContest;
                    //Find or regenerate contest rounds.
                    handleContestRounds(m_nContestId);
                    //Find or regenerate contest end games.
                    handleContestGames(m_currentContest);
                    //Init end game ucs
                    if (Session["pgEditContestTableEndGame.m_nCurrentRoundNumber"] != null)
                    {
                        m_nCurrentRoundNumber = (int)Session["pgEditContestTableEndGame.m_nCurrentRoundNumber"];
                    }
                    else
                    {
                        m_nCurrentRoundNumber = 1;
                    }
                    this.LabelContestHeader.Text = m_sContestRoundPlaceholder.Replace("[ROUND]", Convert.ToString(m_nCurrentRoundNumber));
                    //Init table user control
                    bool isFinalRound = displayCurrentRound(m_nContestId, m_nCurrentRoundNumber);
                }
                else
                {
                    Response.Redirect("~/pgMain.aspx");
                }
            }
            else
            {

                if (Session["pgEditContestTableEndGame.m_nCurrentRoundNumber"] != null)
                    m_nCurrentRoundNumber = (int)Session["pgEditContestTableEndGame.m_nCurrentRoundNumber"];
                else
                    m_nCurrentRoundNumber = 1;

                if (Session["pgEditContestTableEndGame.m_nContestId"] != null)
                    m_nContestId = (int)Session["pgEditContestTableEndGame.m_nContestId"];

                bool isFinalRound = displayCurrentRound(m_nContestId, m_nCurrentRoundNumber);

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
                for (int m_nCurrentTableNumber = 1; m_nCurrentTableNumber <= m_nMaxNumberOfConcurrentGames; m_nCurrentTableNumber++)
                {
                    List<TableHockeyGame> m_gamesOnCurrentTable = m_round.TableHockeyGames.Where(g => g.TableNumber == m_nCurrentTableNumber).ToList();
                    if (m_gamesOnCurrentTable.Count > 0)
                    {
                        TableHockeyContestRound m_roundGame = new TableHockeyContestRound();
                        Mapper.Map(m_round, m_roundGame);
                        m_roundGame.TableHockeyGames = m_gamesOnCurrentTable;
                        uc.ucEndGameSeries uc = (uc.ucEndGameSeries)LoadControl("~/uc/ucEndGameSeries.ascx");
                        uc.InitControl(m_roundGame);
                        uc.m_nTableNumber = m_nCurrentTableNumber;
                        PlaceHolder1.Controls.Add(uc);
                    }
                }
            }
        }

        private bool displayCurrentRound(int i_nContestId, int i_nCurrentRoundNumber)
        {
            //Populate contest round uc.  Get data for current round.
            bool isFinalRound = false;

            List<TableHockeyContestRound> m_rounds;
            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
            {

                if (Session["pgEditContestTableEndGame.m_rounds"] == null)
                {
                    m_rounds = (from r in context.TableHockeyContestRounds
                                where ((r.ContestId == i_nContestId) && (r.RoundNumber == i_nCurrentRoundNumber))
                                select r).ToList();
                    Session["pgEditContestTableEndGame.m_rounds"] = m_rounds;
                }
                else
                    m_rounds = (List<TableHockeyContestRound>)Session["pgEditContestTableEndGame.m_rounds"];

                //Get entered results from uc and update games for table number.

                if (Session["ucEndGameSeries.m_round"] != null)
                {
                    TableHockeyContestRound m_enteredRound = (TableHockeyContestRound)Session["ucEndGameSeries.m_round"];
                    int m_nEnteredTableNumber = m_enteredRound.TableHockeyGames.FirstOrDefault().TableNumber;
                    var m_gamesOnCurrentTable = m_enteredRound.TableHockeyGames.Where(g => g.TableNumber == m_nEnteredTableNumber);
                    Mapper.Map(m_gamesOnCurrentTable, m_rounds[0].TableHockeyGames.Where(g => g.TableNumber == m_nEnteredTableNumber));
                    Session["ucEndGameSeries.m_round"] = m_rounds[0];
                }

                this.ResultsRepeater.DataSource = m_rounds;
                this.ResultsRepeater.DataBind();

                this.divPreviousRound.Visible = (!m_rounds[0].isFirstRound);
                isFinalRound = m_rounds[0].isFinalRound;
                this.divNextRound.Visible = (!isFinalRound);
                this.divSave.Visible = isFinalRound;
                m_sContestRoundPlaceholder = "Contest round [ROUND]"; //TODO:  Config setting!
                this.LabelContestHeader.Text = m_sContestRoundPlaceholder.Replace("[ROUND]", Convert.ToString(m_nCurrentRoundNumber));
                return isFinalRound;
            }
        }

        //Generate contest rounds.
        private int calcNumberOfContestRounds(int i_nContestId)
        {
            //First, count the number of rounds to generate for end game. 
            //This depends on the number of participating players. 
            int m_nPlayerCount = -1;
            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
            {
                //Get players.
                var contestPlayersQuery = from p in context.TableHockeyPlayer
                                          join cp in context.TableHockeyContestPlayers on p.PlayerId equals cp.PlayerId
                                          where cp.ContestId == i_nContestId
                                          select new { p.PlayerId };

                m_nPlayerCount = contestPlayersQuery.ToList().Count();
            }

            //Determine and return number of rounds. If uneven number of players with respect to even end game pairs, add elimination rund.

            int m_nHighestEvenPairs = PageUtility.highestExponentBelowSum(2, m_nPlayerCount);
            int m_nNumberOfEvenRounds = Convert.ToInt32(0.5 * m_nHighestEvenPairs);
            return ((m_nPlayerCount - m_nHighestEvenPairs) == 0) ? m_nNumberOfEvenRounds : 1 + m_nNumberOfEvenRounds;
        }

        private void handleContestRounds(int i_nContestId)
        {
            m_nNumberOfRounds = calcNumberOfContestRounds(i_nContestId);
            TableHockeyContestHandler m_handler = new TableHockeyContestHandler();
            m_handler.handleContestRounds(i_nContestId, m_nNumberOfRounds);
        }

        private int calcCorrectNumberOfEndGames(List<TableHockeyContestPlayer> i_players)
        {
            //Count the number of games to generate for end game. 
            //This depends on the number of participating players and the number of games per round.

            int m_nPlayerCount = i_players.Count;

            if (m_currentContest == null)
                m_currentContest = (TableHockeyContest)Session["pgEditContestTableEndGame.m_currentContest"];

            int m_nGamesPerRound = m_currentContest.numberOfRounds;
            return (m_nPlayerCount - 1) * m_nGamesPerRound;
        }

        private void initEndGameQueueHandler(List<TableHockeyContestPlayer> i_contestPlayers, int i_nGamesPerRound)
        {
            EndGamePlayerViewModelList m_lstEndGamePlayerViewModel = new EndGamePlayerViewModelList(i_contestPlayers);

            if (Session["pgEditContestTableEndGame.queueHandler"] == null)
            {
                m_queueHandler = new TableHockeyEndGameQueueHandler(m_lstEndGamePlayerViewModel.m_lstContestRankedPlayerId, i_nGamesPerRound, m_nNumberOfRounds);
                Session["pgEditContestTableEndGame.queueHandler"] = m_queueHandler;
            }
            else
                m_queueHandler = (TableHockeyEndGameQueueHandler)Session["pgEditContestTableEndGame.queueHandler"];
        }

        private List<TableHockeyGame> getTableHockeyEndGameListForCurrentRound(TableHockeyContest i_contest, List<TableHockeyContestPlayer> i_contestPlayers)
        {
            //Get game list for end game. Always add full number of possible games per round. If a match series ends prematurely, games are returned as "scoreless".
            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
            {
                var roundsQuery = from r in context.TableHockeyContestRounds
                                  where r.ContestId == i_contest.ContestId
                                  select r;

                List<TableHockeyContestRound> m_lstTableHockeyRounds = (List<TableHockeyContestRound>)roundsQuery.ToList();
                List<TableHockeyGame> m_lstTableHockeyGames = new List<TableHockeyGame>();

                int m_nGamesPerRound = i_contest.numberOfRounds;
                DateTime m_dStartDate = DateTime.Now;

                //Create game list for current round.

                TableHockeyContestRound m_round = m_lstTableHockeyRounds[m_queueHandler.m_nCurrentRoundNumber - 1];
                for (int k = 0; k < m_queueHandler.m_dictGamePlayersPerRound[m_queueHandler.m_nCurrentRoundNumber - 1].Count; k += 2)
                {
                    for (int i = 0; i < m_nGamesPerRound; i++)
                    {
                        TableHockeyGame m_currentGame = new TableHockeyGame();
                        m_currentGame.ContestId = i_contest.ContestId;
                        m_currentGame.GameStartDate = m_dStartDate;
                        m_currentGame.isFinalGame = i_contest.isFinalGameContest;
                        m_currentGame.TableNumber = (int)(k / 2.0) + 1;
                        m_currentGame.TableHockeyContestRoundId = m_round.TableHockeyContestRoundId;
                        m_currentGame.HomePlayerId = m_queueHandler.m_dictGamePlayersPerRound[m_queueHandler.m_nCurrentRoundNumber - 1].ElementAt(k).Key;
                        m_currentGame.AwayPlayerId = m_queueHandler.m_dictGamePlayersPerRound[m_queueHandler.m_nCurrentRoundNumber - 1].ElementAt(k + 1).Key;
                        m_currentGame.IdlePlayerId = -1;
                        m_lstTableHockeyGames.Add(m_currentGame);
                    }
                }
                return m_lstTableHockeyGames;
            }
        }

        private void handleContestGames(TableHockeyContest i_currentContest)
        {
            int i_nContestId = i_currentContest.ContestId;
            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
            {
                //Get contest games, if any.
                var gamesQuery = from g in context.TableHockeyGame
                                 where g.ContestId == i_nContestId
                                 select g;

                //Get contest players.
                var playersQuery = from p in context.TableHockeyContestPlayers
                                   where p.ContestId == i_nContestId
                                   select p;

                //Get contest games/players.

                List<TableHockeyGame> m_games = (List<TableHockeyGame>)gamesQuery.ToList();
                List<TableHockeyContestPlayer> m_players = (List<TableHockeyContestPlayer>)playersQuery.ToList();
                int m_nCorrectNumberOfGames = calcCorrectNumberOfEndGames(m_players);
                initEndGameQueueHandler(m_players, i_currentContest.numberOfRounds);
                //if (gamesQuery.ToList().Count < m_nCorrectNumberOfGames)
                //{
                    List<TableHockeyGame> m_gamesForCurrentRound = getTableHockeyEndGameListForCurrentRound(i_currentContest, m_players);
                    //TODO: Game list should be partially re-generated here, ONLY for games where at least one player has changed!
                    //An alternative is to drop ALL games for current round and ALWAYS re-generate. 
                    var m_storedGamesForCurrentRound = m_games.Where(g => g.TableHockeyContestRoundId == m_gamesForCurrentRound[0].TableHockeyContestRoundId);
                    if (m_storedGamesForCurrentRound.FirstOrDefault() == null)
                    {
                        //No stored games for current round, just add generated game list.
                        m_games.AddRange(m_gamesForCurrentRound);
                        foreach (TableHockeyGame m_game in m_games.Where(g => g.GameId == 0))
                        {
                            context.TableHockeyGame.Add(m_game);
                        }
                    }
                    else
                    { 
                       //There are previously stored games for current round.  If something in a game series has changed, replace table hockey games,
                       //otherwise do nothing.
                        List<TableHockeyGameComparable> m_lstGamesForCurrentRound = new TableHockeyGameComparableList(m_gamesForCurrentRound.ToList()).tableHockeyGameList();
                        List<TableHockeyGameComparable> m_lstStoredGamesForCurrentRound =new TableHockeyGameComparableList(m_storedGamesForCurrentRound.ToList()).tableHockeyGameList();
                        var areEquivalent = (m_lstGamesForCurrentRound.Count == m_lstStoredGamesForCurrentRound.Count) && Enumerable.SequenceEqual(m_lstGamesForCurrentRound,m_lstStoredGamesForCurrentRound);
                        if ((areEquivalent != null) && !areEquivalent)
                        {
                            foreach (TableHockeyGame m_game in (m_storedGamesForCurrentRound.ToList()))
                            {
                                m_games.Remove(m_game);

                                var gameToDelete = from p in context.TableHockeyGame
                                                   where p.GameId == m_game.GameId
                                                   select p;

                                context.TableHockeyGame.Remove(gameToDelete.Single());
                                
                            }
                            m_games.AddRange(m_gamesForCurrentRound);
                            foreach (TableHockeyGame m_game in m_games.Where(g => g.GameId == 0))
                            {
                                context.TableHockeyGame.Add(m_game);
                            }
                        }           
                    }
                //}
                context.SaveChanges();
            }
        }

        protected void ButtonPreviousRound_Click(object sender, EventArgs e)
        {
            //Generate games for next round.
            if (Session["pgEditContestTableEndGame.queueHandler"] != null)
            {
                //Decrement round number.
                m_nCurrentRoundNumber--;

                TableHockeyEndGameQueueHandler m_queueHandler = (TableHockeyEndGameQueueHandler)Session["pgEditContestTableEndGame.queueHandler"];
                m_queueHandler.previousRound();

                //Clear round session.
                Session["pgEditContestTableEndGame.m_rounds"] = null;
                Session["ucEndGameSeries.m_round"] = null;
                Session["pgEditContestTableEndGame.m_nCurrentRoundNumber"] = m_nCurrentRoundNumber;
                Session["pgEditContestTableEndGame.queueHandler"] = m_queueHandler;
                //Navigate to previous round.
                Response.Redirect("~/pgEditContestTableEndGame.aspx?ContestId=" + m_nContestId);
            }
        }

        private Dictionary<int, int> getPlayersToNextRound(TableHockeyContestRound i_enteredRound, int i_nGamesPerSeries)
        {

            int m_nNumberOfGamesToWinSeries = Convert.ToInt32((1 + i_nGamesPerSeries) / 2.0);
            Dictionary<int, int> m_dictPlayersToNextRound = new Dictionary<int, int>();

            if (i_enteredRound.TableHockeyGames.Count > 0)
            {
                for (int m_nTableNumber = 1; m_nTableNumber <= m_nMaxNumberOfConcurrentGames; m_nTableNumber++)
                {
                    int m_nHomeWon = 0;
                    int m_nAwayWon = 0;
                    var m_gamesForTable = i_enteredRound.TableHockeyGames.Where(g => g.TableNumber == m_nTableNumber);
                    foreach (TableHockeyGame m_game in m_gamesForTable)
                    {
                        if ((m_game.AwayPlayerScore >= 0) && (m_game.HomePlayerScore >= 0))
                        {
                            if (m_game.AwayPlayerScore > m_game.HomePlayerScore)
                                m_nAwayWon++;
                            if (m_game.HomePlayerScore > m_game.AwayPlayerScore)
                                m_nHomeWon++;
                        }
                    }
                    TableHockeyGame m_firstGame = m_gamesForTable.FirstOrDefault();
                    if (m_nAwayWon == m_nNumberOfGamesToWinSeries)
                        m_dictPlayersToNextRound.Add(m_firstGame.TableNumber, m_firstGame.AwayPlayerId);
                    if (m_nHomeWon == m_nNumberOfGamesToWinSeries)
                        m_dictPlayersToNextRound.Add(m_firstGame.TableNumber, m_firstGame.HomePlayerId);
                }
            }
            return m_dictPlayersToNextRound;
        }

        private void SaveEnteredGameResults()
        {
            //Get results for each end game uc and update TableHockeyGame table.
            PlaceHolder PlaceHolder1 = (PlaceHolder)Page.Controls[0].FindControl("form1").FindControl("mainContent").FindControl("resultsRepeater").Controls[1].FindControl("PlaceHolder1");
            for (int jCount = 0; jCount < PlaceHolder1.Controls.Count; jCount++)
            {
                uc.ucEndGameSeries ucCurrentEndGameSeries = (uc.ucEndGameSeries)PlaceHolder1.Controls[jCount];
                ucCurrentEndGameSeries.getUCGUI();
                for (int iCount = 0; iCount < ucCurrentEndGameSeries.m_games.Count; iCount++)
                {
                    int currentHomeScoreEntered = ucCurrentEndGameSeries.m_games[iCount].HomePlayerScore;
                    int currentAwayScoreEntered = ucCurrentEndGameSeries.m_games[iCount].AwayPlayerScore;
                    int m_nCurrentGameId = ucCurrentEndGameSeries.m_games[iCount].GameId;
                    using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
                    {
                        var querySingleGame = context.TableHockeyGame.First(g => g.GameId == m_nCurrentGameId);
                        TableHockeyGame m_currentGame = (TableHockeyGame)querySingleGame;
                        m_currentGame.HomePlayerScore = currentHomeScoreEntered;
                        m_currentGame.AwayPlayerScore = currentAwayScoreEntered;
                        context.SaveChanges();
                    }
                }
            }
        }

        protected void ButtonNextRound_Click(object sender, EventArgs e)
        {
            //Get scores from each end game uc and validate
            List<TableHockeyContestRound> m_enteredRounds = (List<TableHockeyContestRound>)Session["pgEditContestTableEndGame.m_rounds"];

            if (m_enteredRounds != null)
            {
                if (m_currentContest == null)
                    m_currentContest = (TableHockeyContest)Session["pgEditContestTableEndGame.m_currentContest"];
                int m_nGamesPerRound = m_currentContest.numberOfRounds;
                int m_nContestId = m_currentContest.ContestId;
                //Get round for current round number.
                TableHockeyContestRound m_currentRound = m_enteredRounds.Where(g => g.RoundNumber == m_nCurrentRoundNumber).FirstOrDefault();
                //If everything validates, save results to database and display next round.
                Dictionary<int, int> m_dictPlayersToNextRound = getPlayersToNextRound(m_currentRound, m_nGamesPerRound);
                int m_nPlayersToNextRound = m_enteredRounds[0].TableHockeyGames.Count / m_nGamesPerRound;
                if (m_dictPlayersToNextRound.Count == m_nPlayersToNextRound)
                {
                    SaveEnteredGameResults();
                }
                //Increment round number.
                m_nCurrentRoundNumber++;
                //Generate games for next round.
                if (Session["pgEditContestTableEndGame.queueHandler"] != null)
                {
                    TableHockeyEndGameQueueHandler m_queueHandler = (TableHockeyEndGameQueueHandler)Session["pgEditContestTableEndGame.queueHandler"];
                    m_queueHandler.nextRound(m_dictPlayersToNextRound);

                    //Clear round session.
                    Session["pgEditContestTableEndGame.m_rounds"] = null;
                    Session["ucEndGameSeries.m_round"] = null;
                    Session["pgEditContestTableEndGame.m_nCurrentRoundNumber"] = m_nCurrentRoundNumber;
                    Session["pgEditContestTableEndGame.queueHandler"] = m_queueHandler;
                    //Navigate to next round.
                    Response.Redirect("~/pgEditContestTableEndGame.aspx?ContestId=" + m_nContestId);
                }
            }
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
        }

    }
}
