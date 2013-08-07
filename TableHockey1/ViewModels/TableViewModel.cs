using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TableHockey;
using TableHockeyData;

namespace TableHockey
{
    public class TableRowPlayerViewModel : IComparable
    {
        public int PlayerId { get; set; }
        public int TrendIndicatorType { get; set; }
        public int PlayerStanding { get; set; }
        public string PlayerDescription { get; set; }
        public int NumberOfGamesPlayed { get; set; }
        public int NumberOfGamesWon { get; set; }
        public int NumberOfGamesTied { get; set; }
        public int NumberOfGamesLost { get; set; }
        public int NumberOfGoalsScored { get; set; }
        public int NumberOfGoalsTendered { get; set; }
        public string Divider { get; set; }
        public int GoalDifference { get; set; }
        public int NumberOfPoints { get; set; }

        public TableRowPlayerViewModel(List<TableHockeyGame> i_gamesToSummarize, PlayerViewModel i_player, TableHockeyContest i_contest)
        {
            this.PlayerId = i_player.PlayerId;
            this.PlayerDescription = i_player.FirstName + " " + i_player.LastName;
            this.Divider = "-";
            this.TrendIndicatorType = 0;
            this.NumberOfGamesPlayed = 0;
            this.NumberOfGamesWon = 0;
            this.NumberOfGamesTied = 0;
            this.NumberOfGamesLost = 0;
            this.NumberOfGoalsScored = 0;
            this.NumberOfGoalsTendered = 0;
            this.GoalDifference = 0;
            this.NumberOfPoints = 0;

            //Get point settings.
            int m_nNumberOfPointsWinning;
            int m_nNumberOfPointsTie;
            int m_nNumberOfPointsLosing;
         
             m_nNumberOfPointsWinning = i_contest.PointsWinningGame;
             m_nNumberOfPointsTie = i_contest.PointsTiedGame;
             m_nNumberOfPointsLosing = i_contest.PointsLostGame;

            //Order by roundid to get games in chronological order.
            List<TableHockeyGame> m_orderedGamesList = i_gamesToSummarize.OrderBy(g => g.TableHockeyContestRoundId).ToList();

            for (int i = 0; i < m_orderedGamesList.Count; i++)
            {
                int m_nHomePlayerScore = -1;
                if (m_orderedGamesList.ElementAt(i).HomePlayerScore != null)
                    m_nHomePlayerScore = (int)m_orderedGamesList.ElementAt(i).HomePlayerScore;

                int m_nAwayPlayerScore = -1;
                if(m_orderedGamesList.ElementAt(i).AwayPlayerScore != null)
                   m_nAwayPlayerScore = (int)m_orderedGamesList.ElementAt(i).AwayPlayerScore;

                if ((m_nHomePlayerScore >= 0) && (m_nAwayPlayerScore >= 0))
                {
                    //Actual score entered
                    int m_nMyScore = 0;
                    int m_nTheirScore = 0;

                    if (m_orderedGamesList.ElementAt(i).HomePlayerId == this.PlayerId)
                    {
                        m_nMyScore = m_nHomePlayerScore;
                        m_nTheirScore = m_nAwayPlayerScore;
                    }
                    if (m_orderedGamesList.ElementAt(i).AwayPlayerId == this.PlayerId)
                    {
                        m_nMyScore = m_nAwayPlayerScore;
                        m_nTheirScore = m_nHomePlayerScore;
                    }
                    if (m_nMyScore > m_nTheirScore)
                    {
                        //Player won
                        this.NumberOfPoints += m_nNumberOfPointsWinning;
                        this.NumberOfGamesWon++;
                    }
                    if (m_nMyScore < m_nTheirScore)
                    {
                        //Player lost
                        this.NumberOfPoints += m_nNumberOfPointsLosing;
                        this.NumberOfGamesLost++;
                    }
                    if (m_nMyScore == m_nTheirScore)
                    {
                        //Players tied
                        this.NumberOfPoints += m_nNumberOfPointsTie;
                        this.NumberOfGamesTied++;
                    }
                    this.NumberOfGamesPlayed++;
                    this.NumberOfGoalsScored += m_nMyScore;
                    this.NumberOfGoalsTendered += m_nTheirScore;
                    this.GoalDifference = (this.NumberOfGoalsScored - this.NumberOfGoalsTendered);
                }
            }
        }

        public int CompareTo(object obj)
        {
            TableRowPlayerViewModel otherPlayer = (TableRowPlayerViewModel)obj;
            if (this.PlayerId > 0)
            {
                if (this.NumberOfPoints > otherPlayer.NumberOfPoints)
                {
                    return -1;
                }
                else if (this.NumberOfPoints == otherPlayer.NumberOfPoints)
                {
                    if (this.GoalDifference > otherPlayer.GoalDifference)
                        return -1;
                    else if (this.GoalDifference == otherPlayer.GoalDifference)
                    {
                        if (this.NumberOfGoalsScored >= otherPlayer.NumberOfGoalsScored)
                            return -1;
                    }
                }
            }
            return 1;
        }

        override public string ToString()
        {
            return "";
        }

    }


    public class TableViewModel
    {

        public List<TableRowPlayerViewModel> m_tableRows;

        public TableViewModel(int i_nContestId, int i_nRoundNumber)
        {
            m_tableRows = new List<TableRowPlayerViewModel>();
            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
            {
                var contestPlayersQuery = from p in context.TableHockeyPlayer
                                          join c in context.TableHockeyClub on p.ClubId equals c.ClubId
                                          join cp in context.TableHockeyContestPlayers on p.PlayerId equals cp.PlayerId
                                          where cp.ContestId == i_nContestId
                                          select new { p.PlayerId, p.FirstName, p.LastName, p.BirthDate, c.ClubName, p.PlayerBinary };
                List<PlayerViewModel> m_lstPlayerViewModel = new List<PlayerViewModel>();
                for (int i = 0; i < contestPlayersQuery.Count(); i++)
                {
                    PlayerViewModel m_model = new PlayerViewModel();
                    m_model.BirthDate = (DateTime)contestPlayersQuery.ToList().ElementAt(i).BirthDate;
                    m_model.ClubName = contestPlayersQuery.ToList().ElementAt(i).ClubName;
                    m_model.FirstName = contestPlayersQuery.ToList().ElementAt(i).FirstName;
                    m_model.LastName = contestPlayersQuery.ToList().ElementAt(i).LastName;
                    m_model.PlayerBinary = contestPlayersQuery.ToList().ElementAt(i).PlayerBinary;
                    m_model.PlayerId = contestPlayersQuery.ToList().ElementAt(i).PlayerId;
                    m_lstPlayerViewModel.Add(m_model);
                }

                var querySingleContest = context.TableHockeyContest.First(c => c.ContestId == i_nContestId);

                foreach (PlayerViewModel m_currentPlayer in m_lstPlayerViewModel)
                {
                    List<TableHockeyGame> m_lstCurrentPlayerGames;
                    var contestPlayerRoundStandingsQuery = from thg in context.TableHockeyGame
                                                           join thcr in context.TableHockeyContestRounds on thg.TableHockeyContestRoundId equals thcr.TableHockeyContestRoundId
                                                           where ((thg.ContestId == i_nContestId) && ((thg.HomePlayerId == m_currentPlayer.PlayerId) || (thg.AwayPlayerId == m_currentPlayer.PlayerId)) && (thcr.RoundNumber <= i_nRoundNumber))
                                                           select thg;
                    m_lstCurrentPlayerGames = contestPlayerRoundStandingsQuery.ToList();
                    m_tableRows.Add(new TableRowPlayerViewModel(m_lstCurrentPlayerGames, m_currentPlayer, (TableHockeyContest)querySingleContest));
                }

                //Sort table and update standings.

                int iCount = 1;
                m_tableRows.Sort();
                foreach (TableRowPlayerViewModel m_tablevm in m_tableRows)
                {
                    if (m_tablevm.PlayerId > 0)            
                    {
                        m_tablevm.PlayerStanding = iCount;
                        iCount++;
                    }
                }
            }
        }
    }
}