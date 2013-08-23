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
    public partial class pgEditContestTable : CheckAuth
    {
        protected int m_nContestId;
        protected int m_nNumberOfPlayersLowerRotationLimit;
        protected int m_nNumberOfRounds;
        protected int m_nCurrentRoundNumber;
        protected string m_sContestRoundPlaceholder;
        TableHockeyContest m_currentContest;

        static pgEditContestTable()
        {
            Mapper.CreateMap<TableHockeyContestRound, TableHockeyContestRound>();
        }

        //Generate contest rounds.
        private int calcNumberOfContestRounds(int i_nContestId)
        {
            //First, count the number of rounds to generate for particular contest.
            //If even amount of players n, and r repetitions, the number of rounds
            //will be r * (n - 1). TODO:  KOLLA UPP!!
            //If odd amount of players n, and r repetitions, the number of rounds 
            //will be r * n.

            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
            {
                //Get contest details.
                TableHockeyContest m_contest = context.TableHockeyContest.First(c => c.ContestId == i_nContestId);
                int m_nNumberOfRepetitions = m_contest.numberOfRounds;
                //Get players.
                var contestPlayersQuery = from p in context.TableHockeyPlayer
                                          join cp in context.TableHockeyContestPlayers on p.PlayerId equals cp.PlayerId
                                          where cp.ContestId == i_nContestId                                
                                          select new { p.PlayerId };

                int m_nPlayerCount = contestPlayersQuery.ToList().Count();

                //Determine number of rounds.
                int m_nNumberOfRounds = 0;
                if (!isOdd(m_nPlayerCount))
                {
                    m_nNumberOfRounds = m_nNumberOfRepetitions * (m_nPlayerCount - 1);
                }
                else
                {
                    m_nNumberOfRounds = m_nNumberOfRepetitions * m_nPlayerCount;
                }

                return m_nNumberOfRounds;
            }
        }

        private int getNumberOfGamesPerRound(int i_nNumberOfPlayers)
        {

            //Even number of players N, if above rotation limit the number of games per round N'= 0.5*(N - 2).

            if (isOdd(i_nNumberOfPlayers))
            {
                return Convert.ToInt32(0.5 * (i_nNumberOfPlayers - 1));
            }
            else
            {             
                return Convert.ToInt32(0.5 * i_nNumberOfPlayers);
            }
        }

        //private int getLowerPlayerCountRotationLimit()
        //{
        //    using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
        //    {
        //        var querySetting = context.Settings.First(s => s.SettingDescription == "INT_NUMBER_OF_PLAYERS_ROTATION_LOWER_LIMIT");
        //        return (int)querySetting.ValueInt;
        //    }
        //}

        private List<TableHockeyGame> getTableHockeyGameList(TableHockeyContest i_contest, List<TableHockeyContestPlayer> i_contestPlayers)
        {
            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
            {
                var roundsQuery = from r in context.TableHockeyContestRounds
                                  where r.ContestId == i_contest.ContestId
                                  select r;

                List<TableHockeyContestRound> m_lstTableHockeyRounds = (List<TableHockeyContestRound>)roundsQuery.ToList();
                List<TableHockeyGame> m_lstTableHockeyGames = new List<TableHockeyGame>();
                int m_nGamesPerRound = getNumberOfGamesPerRound(i_contestPlayers.Count);
                DateTime m_dStartDate = DateTime.Now;
                PlayerViewModelList m_lstPlayerViewModel = new PlayerViewModelList(i_contestPlayers);
                TableHockeyQueueHandler2 m_queueHandler = new TableHockeyQueueHandler2(m_lstPlayerViewModel.m_lstContestPlayerId, m_nGamesPerRound);

                foreach (TableHockeyContestRound m_round in m_lstTableHockeyRounds)
                {
                    for (int i = 0; i < m_nGamesPerRound; i++)
                    {
                        TableHockeyGame m_currentGame = new TableHockeyGame();
                        m_currentGame.ContestId = i_contest.ContestId;
                        m_currentGame.GameStartDate = m_dStartDate;
                        m_currentGame.isFinalGame = i_contest.isFinalGameContest;
                        m_currentGame.TableNumber = i + 1;
                        m_currentGame.TableHockeyContestRoundId = m_round.TableHockeyContestRoundId;
                        m_currentGame.HomePlayerId = m_queueHandler.m_lstCurrentHomePlayerId[i];
                        m_currentGame.AwayPlayerId = m_queueHandler.m_lstCurrentAwayPlayerId[i];
                        m_currentGame.IdlePlayerId = m_queueHandler.getIdlePlayer();
                        m_lstTableHockeyGames.Add(m_currentGame);
                    }
                    m_queueHandler.nextRound();
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

                int m_nCorrectNumberOfUniqueGames = Convert.ToInt32(0.5 * i_currentContest.numberOfRounds * m_players.Count * (m_players.Count - 1));
                if (gamesQuery.ToList().Count != m_nCorrectNumberOfUniqueGames)
                {
                    if (gamesQuery.ToList().Count > 0)
                    {
                        //Game count mismatch. Remove games and re-generate game list.
                        foreach (TableHockeyGame m_game in m_games)
                        {
                            context.TableHockeyGame.Remove(m_game);
                        }
                        context.SaveChanges();
                    }
                    //Generate game list for entire contest.
                    m_games = getTableHockeyGameList(i_currentContest, m_players);
                    foreach (TableHockeyGame m_game in m_games)
                    {
                        context.TableHockeyGame.Add(m_game);
                    }
                }
                context.SaveChanges();
            }
        }

        private void handleContestRounds(int i_nContestId)
        {
            m_nNumberOfRounds = calcNumberOfContestRounds(i_nContestId);
            TableHockeyContestHandler m_handler = new TableHockeyContestHandler();
            m_handler.handleContestRounds(i_nContestId, m_nNumberOfRounds);
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
                    Session["pgEditContestTable.m_nContestId"] = m_nContestId;
                    //Get current contest
                    using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
                    {
                        m_currentContest = context.TableHockeyContest.FirstOrDefault(i => i.ContestId == m_nContestId);
                    }
                    Session["pgEditContestTable.m_currentContest"] = m_currentContest;
                    //Find or regenerate contest rounds.
                    handleContestRounds(m_nContestId);
                    //Find or regenerate contest games.
                    handleContestGames(m_currentContest);
                    if (Session["pgEditContestTable.m_nCurrentRoundNumber"] != null)
                    {
                        m_nCurrentRoundNumber = (int)Session["pgEditContestTable.m_nCurrentRoundNumber"];
                    }
                    else
                    {
                        m_nCurrentRoundNumber = 1;
                    }
                    this.LabelContestHeader.Text = m_sContestRoundPlaceholder.Replace("[ROUND]", Convert.ToString(m_nCurrentRoundNumber));
                    //Init table user control
                    bool isFinalRound = displayCurrentRound(m_nContestId, m_nCurrentRoundNumber);
                    int m_nNumberOfEndGamePlayers = (int)((m_currentContest.NumberOfPlayersToNextRound != null) ? m_currentContest.NumberOfPlayersToNextRound : 100);
                    TableViewModel m_tableViewModel = new TableViewModel(m_nContestId, m_nCurrentRoundNumber);
                    this.ucContestTable1.InitControl(m_tableViewModel.m_tableRows, false, m_nNumberOfEndGamePlayers);
                    Session["pgEditContestTable.m_nCurrentRoundNumber"] = m_nCurrentRoundNumber;
                }
                else
                {
                    Response.Redirect("~/pgMain.aspx");
                }
            }
            else
            {
                if (Session["pgEditContestTable.m_nCurrentRoundNumber"] != null)
                    m_nCurrentRoundNumber = (int)Session["pgEditContestTable.m_nCurrentRoundNumber"];
                else
                    m_nCurrentRoundNumber = 1;

                if (Session["pgEditContestTable.m_nContestId"] != null)
                    m_nContestId = (int)Session["pgEditContestTable.m_nContestId"];
            }
        }

        private bool displayCurrentRound(int i_nContestId, int i_nCurrentRoundNumber)
        {
            //Populate contest round uc.  Get data for current round.
            bool isFinalRound = false;
            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
            {
                TableHockeyContestRound m_round = context.TableHockeyContestRounds.FirstOrDefault(r => (r.ContestId == i_nContestId) && (r.RoundNumber == i_nCurrentRoundNumber));
                this.ucContestRound1.InitControl(m_round);
                this.divPreviousRound.Visible = (!m_round.isFirstRound);
                isFinalRound = m_round.isFinalRound;
                this.divNextRound.Visible = (!isFinalRound);
                this.divSave.Visible = isFinalRound;
                this.divEndGame.Visible = isFinalRound;
                m_sContestRoundPlaceholder = "Contest round [ROUND]"; //TODO:  Config setting!
                this.LabelContestHeader.Text = m_sContestRoundPlaceholder.Replace("[ROUND]", Convert.ToString(m_nCurrentRoundNumber));
            }
            return isFinalRound;
        }

        protected void ButtonPreviousRound_Click(object sender, EventArgs e)
        {
            if (m_currentContest == null)
                m_currentContest = (TableHockeyContest)Session["pgEditContestTable.m_currentContest"];

            //Display previous round.
            m_nCurrentRoundNumber--;
            bool isFinalRound = displayCurrentRound(m_nContestId, m_nCurrentRoundNumber);
            int m_nNumberOfEndGamePlayers = (int)((m_currentContest.NumberOfPlayersToNextRound != null) ? m_currentContest.NumberOfPlayersToNextRound : 100);
            TableViewModel m_tableViewModel = new TableViewModel(m_nContestId, m_nCurrentRoundNumber);
            this.ucContestTable1.InitControl(m_tableViewModel.m_tableRows, false, m_nNumberOfEndGamePlayers);
            Session["pgEditContestTable.m_nCurrentRoundNumber"] = m_nCurrentRoundNumber;
        }

        protected void ButtonNextRound_Click(object sender, EventArgs e)
        {
            //Check if all score textboxes have valid numbers entered.  If not, display error message.
            if (this.ucContestRound1.isRoundDataOk())
            {
                SaveEnteredGameResults();

                if (m_currentContest == null)
                    m_currentContest = (TableHockeyContest)Session["pgEditContestTable.m_currentContest"];

                //Display next round.
                m_nCurrentRoundNumber++;
                bool isFinalRound = displayCurrentRound(m_nContestId, m_nCurrentRoundNumber);
                int m_nNumberOfEndGamePlayers = (int)((m_currentContest.NumberOfPlayersToNextRound != null) ? m_currentContest.NumberOfPlayersToNextRound : 100);
                TableViewModel m_tableViewModel = new TableViewModel(m_nContestId, m_nCurrentRoundNumber);
                this.ucContestTable1.InitControl(m_tableViewModel.m_tableRows, false, m_nNumberOfEndGamePlayers);
                Session["pgEditContestTable.m_nCurrentRoundNumber"] = m_nCurrentRoundNumber;
            }
            else
            {
                //Display error message.
                DisplayError();
            }
        }

        private void DisplayError()
        {
            string msg = "<script language=\"javascript\">";
            msg += "alert('All entered scores must be numeric!');";
            msg += "</script>";
            Response.Write(msg);
        }

        private void SaveEnteredGameResults()
        {
            //Get results for each row and update TableHockeyGame table.
            this.ucContestRound1.getUCGUI();
            for (int iCount = 0; iCount < this.ucContestRound1.m_games.Count; iCount++)
            {
                int currentHomeScoreEntered = ucContestRound1.m_games[iCount].HomePlayerScore;
                int currentAwayScoreEntered = ucContestRound1.m_games[iCount].AwayPlayerScore;
                int m_nCurrentGameId = ucContestRound1.m_games[iCount].GameId;
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

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            //Check if all score textboxes have valid numbers entered.  If not, display error message.
            if (this.ucContestRound1.isRoundDataOk())
            {
                SaveEnteredGameResults();

                //Display final standings.

                if (m_currentContest == null)
                    m_currentContest = (TableHockeyContest)Session["pgEditContestTable.m_currentContest"];

                bool isFinalRound = displayCurrentRound(m_nContestId, m_nCurrentRoundNumber);
                int m_nNumberOfEndGamePlayers = (int)((m_currentContest.NumberOfPlayersToNextRound != null) && (m_currentContest.NumberOfPlayersToNextRound > 0) ? m_currentContest.NumberOfPlayersToNextRound : 100);
                TableViewModel m_tableViewModel = new TableViewModel(m_nContestId, m_nCurrentRoundNumber);
                this.ucContestTable1.InitControl(m_tableViewModel.m_tableRows, isFinalRound, m_nNumberOfEndGamePlayers);
                Session["pgEditContestTable.m_nCurrentRoundNumber"] = m_nCurrentRoundNumber;
            }
            else
            {
                //Display error message.
                DisplayError();
            }
        }

        protected void ButtonEndGame_Click(object sender, EventArgs e)
        {
            int m_nNumberOfRounds = -1;
            if (!int.TryParse(TextBoxEndGameRounds.Text.Trim(), out m_nNumberOfRounds))
            {
                this.cvEndGameRounds.IsValid = false;
                return;
            }

            this.cvEndGameRounds.IsValid = true;
            //Get selected players for end game.
            Dictionary<int, int> m_selectedPlayerIDs = this.ucContestTable1.CheckedPlayersToEndGame;
            if ((m_selectedPlayerIDs != null) && (m_selectedPlayerIDs.Count > 1))
            {
                this.cvEndGamePlayers.IsValid = true;
                if (m_currentContest == null)
                    m_currentContest = (TableHockeyContest)Session["pgEditContestTable.m_currentContest"];

                //Create new end game contest for current contest.
                TableHockeyContestHandler m_handler = new TableHockeyContestHandler();
                TableHockeyContest m_newEndGameContest = m_handler.createDefaultEndGameTableHockeyContest(m_currentContest);
                m_newEndGameContest.numberOfRounds = Convert.ToInt32(TextBoxEndGameRounds.Text);
                //Add selected players to end game contest.  Make sure to export final table standing for each player.
                int m_nEndGameContestId = -1;
                using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
                {
                    //Create new end game only if no previous exists.
                    var queryExistingEndGame = context.TableHockeyContest.FirstOrDefault(c => c.EndGameForContestId == m_nContestId);
                    if (queryExistingEndGame == null)
                    {
                        context.TableHockeyContest.Add(m_newEndGameContest);
                        context.SaveChanges();
                    }          
                }

                using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
                {
                    //Add new players to end game if none exist.

                     var queryExistingEndGame = context.TableHockeyContest.FirstOrDefault(c => c.EndGameForContestId == m_nContestId);
                     if (queryExistingEndGame != null)
                     {
                         m_nEndGameContestId = queryExistingEndGame.ContestId;
                         var queryExistingPlayers = context.TableHockeyContestPlayers.FirstOrDefault(p => p.ContestId == m_nEndGameContestId);
                         //Only add players if none exist.
                         if (queryExistingPlayers == null)
                         {
                             foreach (KeyValuePair<int, int> kvp in m_selectedPlayerIDs)
                             {
                                 TableHockeyContestPlayer m_contestPlayer = new TableHockeyContestPlayer();
                                 m_contestPlayer.ContestId = m_nEndGameContestId;
                                 m_contestPlayer.PlayerId = kvp.Value;
                                 m_contestPlayer.FinalPreviousStanding = 1 + kvp.Key;
                                 m_contestPlayer.isDummyContestPlayer = false;
                                 context.TableHockeyContestPlayers.Add(m_contestPlayer);
                             }
                             context.SaveChanges();
                         }             
                     }
                }
                using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
                {
                    //Create new end game only if no previous exists.
                    var queryExistingEndGame = context.TableHockeyContest.FirstOrDefault(c => c.EndGameForContestId == m_nContestId);
                    if (queryExistingEndGame != null)
                    {
                        TableHockeyContest m_endGame = (TableHockeyContest)queryExistingEndGame;
                        m_nEndGameContestId = m_endGame.ContestId;           
                    }
                }
                Response.Redirect("~/pgEditContestTableEndGame.aspx?ContestId=" + m_nEndGameContestId);
            }
            else
            {
                this.cvEndGamePlayers.IsValid = false;
            }
        }

    }
}