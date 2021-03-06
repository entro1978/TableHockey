﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TableHockeyData;
using System.Web.Security;

namespace TableHockey
{
    public class TableHockeyContestHandler
    {

        public TableHockeyContest createDefaultEndGameTableHockeyContest(TableHockeyContest i_parentContest)
        {
            TableHockeyContest m_GUIContest = new TableHockeyContest();
            m_GUIContest.ContestDateOpened = DateTime.Now;
            m_GUIContest.ContestDateClosed = new DateTime(1900, 1, 1);
            m_GUIContest.ContestDescription = i_parentContest.ContestDescription + " End Game";
            m_GUIContest.ContestName = i_parentContest.ContestName + " End Game";
            m_GUIContest.ContestLocation = i_parentContest.ContestLocation;
            m_GUIContest.GameLengthMinutes = 5;  //TODO: Setting!
            m_GUIContest.NumberOfPlayersToNextRound = -1;
            m_GUIContest.numberOfRounds = 1;    //TODO:  Setting?!       
            m_GUIContest.PointsWinningGame = -1;
            m_GUIContest.PointsTiedGame = -1;
            m_GUIContest.PointsLostGame = -1;
            m_GUIContest.isGoalDifferenceRanked = false;
            m_GUIContest.isFinalGameContest = true;
            m_GUIContest.OwnerUserId = (Guid)i_parentContest.OwnerUserId;
            m_GUIContest.EndGameForContestId = i_parentContest.ContestId;
            return m_GUIContest;
        }

        public List<TableHockeyContestRound> getTableHockeyContestRoundList(int i_nContestId, int i_nNumberOfRounds, bool i_bIsEndGameRound = false)
        {
            List<TableHockeyContestRound> m_lstTableHockeyContestRound = new List<TableHockeyContestRound>();
            for (int i = 0; i < i_nNumberOfRounds; i++)
            {
                TableHockeyContestRound m_currentRound = new TableHockeyContestRound();
                m_currentRound.ContestId = i_nContestId;
                m_currentRound.RoundNumber = i + 1;
                m_currentRound.isFirstRound = (i == 0);
                m_currentRound.isFinalRound = ((i + 1) == i_nNumberOfRounds);
                m_currentRound.isEndGameRound = i_bIsEndGameRound;
                m_lstTableHockeyContestRound.Add(m_currentRound);
            }
            return m_lstTableHockeyContestRound;
        }

        public void handleContestRounds(int i_nContestId, int i_nNumberOfRounds)
        {
            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
            {
                var roundsQuery = from p in context.TableHockeyContestRound
                                  where p.ContestId == i_nContestId
                                  select p;

                if (roundsQuery.ToList().Count() != i_nNumberOfRounds)
                {
                    //Number of players or repetitions in current contest has changed,
                    //or a new game had been created.
                    //Drop any existing contest rounds and regenerate.
                    if (roundsQuery.ToList().Count > 0)
                    {
                        foreach (TableHockeyContestRound m_round in (List<TableHockeyContestRound>)roundsQuery.ToList())
                        {
                            foreach (TableHockeyGame m_game in m_round.TableHockeyGame.ToList())
                            {
                                context.TableHockeyGame.Remove(m_game);
                            }
                            context.TableHockeyContestRound.Remove(m_round);
                        }
                        context.SaveChanges();
                    }
                    TableHockeyContestHandler m_contestHandler = new TableHockeyContestHandler();
                    List<TableHockeyContestRound> m_lstTableHockeyContestRound = m_contestHandler.getTableHockeyContestRoundList(i_nContestId, i_nNumberOfRounds);
                    foreach (TableHockeyContestRound m_round in m_lstTableHockeyContestRound)
                    {
                        context.TableHockeyContestRound.Add(m_round);
                    }
                }
                context.SaveChanges();
            }
        }

        public static List<int> getNumberOfGamesWon(RoundViewModel i_vmRound)
        {
            List<int> m_lstOut = new List<int>();
            int m_nNumberOfGamesWonHome = 0;
            int m_nNumberOfGamesWonAway = 0;
            foreach (GameViewModel m_game in i_vmRound.m_vmGames)
            {

                int m_nHomeScore = m_game.HomePlayerScore;
                int m_nAwayScore = m_game.AwayPlayerScore;
                if ((m_nHomeScore >= 0) && (m_nAwayScore >= 0))
                {
                    if (m_nHomeScore > m_nAwayScore)
                        m_nNumberOfGamesWonHome++;
                    if (m_nAwayScore > m_nHomeScore)
                        m_nNumberOfGamesWonAway++;
                }
            }
            m_lstOut.Add(m_nNumberOfGamesWonAway);
            m_lstOut.Add(m_nNumberOfGamesWonHome);
            return m_lstOut;
        }

        public static Dictionary<int, int> getPlayersToNextRound(TableHockeyContestRound i_enteredRound, int i_nGamesPerSeries)
        {

            int m_nNumberOfGamesToWinSeries = Convert.ToInt32((1 + i_nGamesPerSeries) / 2.0);
            Dictionary<int, int> m_dictPlayersToNextRound = new Dictionary<int, int>();

            if (i_enteredRound.TableHockeyGame.Count > 0)
            {
                for (int m_nTableNumber = 1; m_nTableNumber <= PageUtility.m_nMaxNumberOfConcurrentGames; m_nTableNumber++)
                {
                    int m_nHomeWon = 0;
                    int m_nAwayWon = 0;
                    var m_gamesForTable = i_enteredRound.TableHockeyGame.Where(g => g.TableNumber == m_nTableNumber);
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
    }
}