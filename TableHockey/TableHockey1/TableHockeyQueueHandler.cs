using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TableHockey
{
    public class TableHockeyEndGameQueueHandler
    {
        protected int m_nNumberOfPlayers;
        protected int m_nNumberOfRounds;
        private int m_nNumberOfGamesPerRound;

        public List<Dictionary<int, int>> m_dictGamePlayersPerRound;

        public int m_nCurrentRoundNumber;

        public Dictionary<int, int> m_nextRoundPlayers;

        public TableHockeyEndGameQueueHandler(Dictionary<int,int> i_dictRankedPlayerId, int i_nNumberOfGamesPerRound, int i_nNumberOfRounds)
        {
            //{Player ID, Player Rank}
            //Sort by rank descending.
            var m_sortedPlayersByRank = (from entry in i_dictRankedPlayerId orderby entry.Value ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
            m_nNumberOfPlayers = i_dictRankedPlayerId.Count;
            m_nNumberOfGamesPerRound = i_nNumberOfGamesPerRound;
            m_nNumberOfRounds = i_nNumberOfRounds;
            int m_nNumberOfEvenRounds = PageUtility.isOdd(m_nNumberOfRounds) ? (m_nNumberOfRounds - 1) : m_nNumberOfRounds;
            //Build end game tree, starting with lowest ranked players..

            //First, find out if there are players that do not fit into an end game tree.
            int m_nNumberOfPlayersToEliminate = m_nNumberOfPlayers - Convert.ToInt32(Math.Pow(2.0, m_nNumberOfEvenRounds));
            int m_nNumberOfPlayersToNextRound = m_nNumberOfPlayers - 2 * m_nNumberOfPlayersToEliminate;
            //Export any odd top players from initial dictionary, into a List (per round) of player dictionaries.  
            //First list entry pertains to first round, etc.

            m_dictGamePlayersPerRound = new List<Dictionary<int, int>>();
            Dictionary<int, int> m_nextRoundPlayerGames = new Dictionary<int, int>();
            Dictionary<int, int> m_eliminatePlayerGames = new Dictionary<int, int>(); 
            if (m_nNumberOfPlayersToEliminate > 0)
            {
                m_nextRoundPlayers = (from entry in m_sortedPlayersByRank orderby entry.Value ascending where entry.Value <= m_nNumberOfPlayersToNextRound select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                
                foreach(KeyValuePair<int,int> kvp in m_nextRoundPlayers)
                {
                    m_nextRoundPlayerGames.Add(kvp.Key,kvp.Value);
                    m_nextRoundPlayerGames.Add(-kvp.Key, -1);
                }         
            }

            Dictionary<int, int> m_eliminatePlayers = (from entry in m_sortedPlayersByRank orderby entry.Value ascending where entry.Value > m_nNumberOfPlayersToNextRound select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
            int m_nHalfNumberOfEvenPlayers = Convert.ToInt32(m_eliminatePlayers.Count/2);
            for (int i = 0; i < m_nHalfNumberOfEvenPlayers; i++)
            {
                Dictionary<int, int> m_currentHomePlayer = (from entry in m_eliminatePlayers where entry.Value == (m_nNumberOfPlayersToNextRound + i + 1) select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                Dictionary<int, int> m_currentAwayPlayer = (from entry in m_eliminatePlayers where entry.Value == (m_nNumberOfPlayers - i) select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                m_eliminatePlayerGames.Add(m_currentHomePlayer.First().Key, m_currentHomePlayer.First().Value);
                m_eliminatePlayerGames.Add(m_currentAwayPlayer.First().Key, m_currentAwayPlayer.First().Value);
            }
            m_dictGamePlayersPerRound.Add(m_eliminatePlayerGames);
            if (m_nextRoundPlayerGames.Count > 0)
                m_dictGamePlayersPerRound.Add(m_nextRoundPlayerGames);
            m_nCurrentRoundNumber = 1;
        }

        public void nextRound(Dictionary<int, int> i_dictPlayersToNextRound)
        {
            this.m_nCurrentRoundNumber++;
            //Remove any empty/undecided players (-1).
            if (m_dictGamePlayersPerRound.Count >= m_nCurrentRoundNumber)
            {
                for (int i = m_dictGamePlayersPerRound[m_nCurrentRoundNumber - 1].Count - 1; i >= 0; i--)
                {
                    KeyValuePair<int, int> kvp = m_dictGamePlayersPerRound[m_nCurrentRoundNumber - 1].ElementAt(i);
                    if (kvp.Value == -1)
                    {
                        m_dictGamePlayersPerRound[m_nCurrentRoundNumber - 1].Remove(kvp.Key);
                    }
                }
            }
            else
            { 
               //Add new round in game players per round dictionary.
                Dictionary<int,int> m_dictPlayers = new Dictionary<int,int>();
                m_dictGamePlayersPerRound.Add(m_dictPlayers);
            }
            
            //Find next round players in dictionary for previous round based on m_dictPlayersToNextRound.
            //m_queueHandler.m_dictGamePlayersPerRound[m_nCurrentRoundNumber - 1].
            var m_dictGamePlayersPerRoundToNextRound = (from entry in m_dictGamePlayersPerRound[m_nCurrentRoundNumber - 2]
                                                        where i_dictPlayersToNextRound.Values.Contains(entry.Key)
                                                        orderby entry.Key ascending
                                                        select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
            foreach (KeyValuePair<int, int> kvp in m_dictGamePlayersPerRoundToNextRound)
            {
                if(!m_dictGamePlayersPerRound[m_nCurrentRoundNumber - 1].Keys.Contains(kvp.Key))
                    m_dictGamePlayersPerRound[m_nCurrentRoundNumber - 1].Add(kvp.Key, kvp.Value);
            }
        }

        public void previousRound(){

            //Find next round players in dictionary for previous round based on m_dictPlayersToNextRound.
            //m_queueHandler.m_dictGamePlayersPerRound[m_nCurrentRoundNumber - 1].
            var m_dictGamePlayersPerRoundToNextRound = (from entry in m_dictGamePlayersPerRound[m_nCurrentRoundNumber - 1]
                                                        where !m_nextRoundPlayers.Values.Contains(entry.Key)
                                                        orderby entry.Key ascending
                                                        select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
            foreach (KeyValuePair<int, int> kvp in m_dictGamePlayersPerRoundToNextRound)
            {
                m_dictGamePlayersPerRound[m_nCurrentRoundNumber - 1].Remove(kvp.Key);
            }

            this.m_nCurrentRoundNumber--;           
        }
    }


    public class TableHockeyQueueHandler
    {
        protected int m_nNumberOfPlayers;
        protected int m_nNumberOfRounds;
        protected int m_nCurrentIdlePlayerId;
        public Dictionary<int,int> m_lstIdlePlayersIndexAndId;
        private int m_nHalfNumberOfPlayers;
        private int m_nNumberOfPlayersLowerRotationLimit;
        private int m_nNumberOfGamesPerRound;

        public List<int> m_lstCurrentHomePlayerId;
        public List<int> m_lstCurrentAwayPlayerId;
    
        public int m_nCurrentRoundNumber;
        public List<int> m_lstPlayerQueue;
        private List<int> m_lstPlayerId;

        public TableHockeyQueueHandler(List<int> i_lstPlayerId, int i_nNumberOfPlayersLowerRotationLimit, int i_nNumberOfGamesPerRound)
        { 
            //TODO: Option to randomize initial player order.
            i_lstPlayerId.Sort();
            m_lstPlayerId = i_lstPlayerId;
            m_nNumberOfPlayers = m_lstPlayerId.Count;
            m_nNumberOfPlayersLowerRotationLimit = i_nNumberOfPlayersLowerRotationLimit;
            m_nNumberOfGamesPerRound = i_nNumberOfGamesPerRound;
            this.m_lstIdlePlayersIndexAndId = new Dictionary<int, int>();
            if (PageUtility.isOdd(m_nNumberOfPlayers))
            {
                //Odd number of players - set "middle" player in player id list as first idle player.
                int m_nHalfWayIndex = Convert.ToInt32((m_lstPlayerId.Count + 1) / 2 - 1);
                m_lstIdlePlayersIndexAndId.Add(m_nHalfWayIndex, m_lstPlayerId[m_nHalfWayIndex]);
                //Set home players to first partition.
                this.m_lstCurrentHomePlayerId = m_lstPlayerId.GetRange(0, m_nNumberOfGamesPerRound);
                //Set away players to second partition.
                this.m_lstCurrentAwayPlayerId = m_lstPlayerId.GetRange(m_nNumberOfGamesPerRound + 1, m_nNumberOfGamesPerRound);
            }
            else
            { 

                //In case of an even number of players, the cases n = 2, n = 4 and n = 6 are handled separately.
                //n = 4;
                //n = 6;

                if (m_nNumberOfPlayers >= m_nNumberOfPlayersLowerRotationLimit)
                {
                    //n >=  Rotation limit (Currently=8 per 2012-02-10);
                    //Even number of players - set "middle" player in player id list 
                    //as first idle player, and last as second idle player.

                    //Even number of players - set "middle" player in player id list as first idle player,
                    //and last player in player id list as last idle player.

                    m_nHalfNumberOfPlayers = Convert.ToInt32(m_nNumberOfPlayers / 2);
                    //Store idle player on "lower" side of table.
                    m_lstIdlePlayersIndexAndId.Add(m_nNumberOfGamesPerRound, m_lstPlayerId[m_nNumberOfGamesPerRound]);
                    //Store idle player on "upper" side of table.
                    m_lstIdlePlayersIndexAndId.Add(m_nNumberOfPlayers - 1, m_lstPlayerId[m_nNumberOfPlayers - 1]); 

                    //Set home players to first partition.
                    this.m_lstCurrentHomePlayerId = m_lstPlayerId.GetRange(0, m_nNumberOfGamesPerRound);

                    //Set away players to second partition.
                    this.m_lstCurrentAwayPlayerId = m_lstPlayerId.GetRange(m_nNumberOfGamesPerRound + 1, m_nNumberOfGamesPerRound);
                }
            }

            //Initialize List to handle as a Queue.
            m_lstPlayerQueue = new List<int>();
            m_lstPlayerQueue.AddRange(m_lstCurrentHomePlayerId);
            m_lstPlayerQueue.AddRange(m_lstCurrentAwayPlayerId);
            m_nCurrentRoundNumber = 1;
        }

        private List<int> shiftPlayerQueueRight(List<int> i_lstQueue)
        {
            int i_nQueueLength = i_lstQueue.Count;
            List<int> m_lstShiftedQueue = new List<int>(i_nQueueLength);
            for(int i=0; i < i_nQueueLength; i++)
                m_lstShiftedQueue.Add(0);
            for(int i=0; i < i_lstQueue.Count; i++)
            {
                if (i < i_lstQueue.Count - 1)
                    m_lstShiftedQueue[i + 1] = i_lstQueue[i];
                else
                    m_lstShiftedQueue[0] = i_lstQueue[i];
            }
            return m_lstShiftedQueue;
        }

        public void nextRound()
        {
            if (PageUtility.isOdd(m_nNumberOfPlayers))
            { 
                //Before shifting:
                //1. Store player in position to be idle 
                //2. In queue, replace player in position to be idle with currently idle player.
                int m_nIdlePlayerId_1 = m_lstIdlePlayersIndexAndId.ElementAt(0).Value;
                m_lstIdlePlayersIndexAndId[m_lstIdlePlayersIndexAndId.ElementAt(0).Key] = m_lstPlayerQueue[m_nNumberOfGamesPerRound - 1];
                m_lstPlayerQueue[m_nNumberOfGamesPerRound - 1] = m_nIdlePlayerId_1;

                m_lstPlayerQueue = shiftPlayerQueueRight(m_lstPlayerQueue);
                m_lstCurrentHomePlayerId = m_lstPlayerQueue.GetRange(0, m_nNumberOfGamesPerRound);
                m_lstCurrentAwayPlayerId = m_lstPlayerQueue.GetRange(m_nNumberOfGamesPerRound, m_nNumberOfGamesPerRound);
                m_nCurrentRoundNumber++;
            } else
            {
                if (m_nNumberOfPlayers >= m_nNumberOfPlayersLowerRotationLimit)
                {
                    //If number of players >= Player rotation lower limit.
                    //Before shifting:
                    //1. Store both players in idle position.
                    //2. In queue, replace both players in position to be idle with currently respectively idle player.
                    int m_nIdlePlayerId_1 = m_lstIdlePlayersIndexAndId.ElementAt(0).Value;
                    m_lstIdlePlayersIndexAndId[m_lstIdlePlayersIndexAndId.ElementAt(0).Key] = m_lstPlayerQueue[m_nNumberOfGamesPerRound - 1];
                    m_lstPlayerQueue[m_nNumberOfGamesPerRound - 1] = m_nIdlePlayerId_1;

                    int m_nIdlePlayerId_2 = m_lstIdlePlayersIndexAndId.ElementAt(1).Value;
                    m_lstIdlePlayersIndexAndId[m_lstIdlePlayersIndexAndId.ElementAt(1).Key] = m_lstPlayerQueue[2 * m_nNumberOfGamesPerRound - 1];
                    m_lstPlayerQueue[2 * m_nNumberOfGamesPerRound - 1] = m_nIdlePlayerId_2;

                    m_lstPlayerQueue = shiftPlayerQueueRight(m_lstPlayerQueue);
                    m_lstCurrentHomePlayerId = m_lstPlayerQueue.GetRange(0, m_nNumberOfGamesPerRound);
                    m_lstCurrentAwayPlayerId = m_lstPlayerQueue.GetRange(m_nNumberOfGamesPerRound, m_nNumberOfGamesPerRound);
                    m_nCurrentRoundNumber++;
                }
            }
        }
    }
}