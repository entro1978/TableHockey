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

        public TableHockeyEndGameQueueHandler(Dictionary<int, int> i_dictRankedPlayerId, int i_nNumberOfGamesPerRound, int i_nNumberOfRounds)
        {
            //{Player ID, Player Rank}
            //Sort by rank descending.
            var m_sortedPlayersByRank = (from entry in i_dictRankedPlayerId orderby entry.Value ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
            m_nNumberOfPlayers = i_dictRankedPlayerId.Count;
            m_nNumberOfGamesPerRound = i_nNumberOfGamesPerRound;
            m_nNumberOfRounds = i_nNumberOfRounds;
            bool m_bEliminationRound = (PageUtility.highestExponentLessThanOrEqualToSum(2, m_nNumberOfPlayers) != m_nNumberOfPlayers); 
            int m_nNumberOfEvenRounds = m_bEliminationRound ? (m_nNumberOfRounds - 1) : m_nNumberOfRounds;
            //Build end game tree, starting with lowest ranked players..

            //First, find out if there are players that do not fit into an end game tree.
            int m_nNumberOfPlayersToEliminate = m_nNumberOfPlayers - Convert.ToInt32(Math.Pow(2.0, m_nNumberOfEvenRounds));
            int m_nNumberOfPlayersToNextRound = m_nNumberOfPlayers - 2 * m_nNumberOfPlayersToEliminate;
            //Export any odd top players from initial dictionary, into a List (per round) of player dictionaries.  
            //First list entry pertains to first round, etc.

            m_dictGamePlayersPerRound = new List<Dictionary<int, int>>();
            Dictionary<int, int> m_nextRoundPlayerGames = new Dictionary<int, int>();
            Dictionary<int, int> m_eliminatePlayerGames = new Dictionary<int, int>();
            if ((m_nNumberOfPlayersToNextRound > 0) && (m_nNumberOfPlayersToEliminate > 0))
            {
                m_nextRoundPlayers = (from entry in m_sortedPlayersByRank orderby entry.Value ascending where entry.Value <= m_nNumberOfPlayersToNextRound select entry).ToDictionary(pair => pair.Key, pair => pair.Value);

                foreach (KeyValuePair<int, int> kvp in m_nextRoundPlayers)
                {
                    m_nextRoundPlayerGames.Add(kvp.Key, kvp.Value);
                    m_nextRoundPlayerGames.Add(-kvp.Key, -1);
                }
            }

            Dictionary<int, int> m_eliminatePlayers;
            if (m_nNumberOfPlayersToEliminate > 0)
                m_eliminatePlayers = (from entry in m_sortedPlayersByRank orderby entry.Value ascending where entry.Value > m_nNumberOfPlayersToNextRound select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
            else
            {
                m_eliminatePlayers = (from entry in m_sortedPlayersByRank orderby entry.Value ascending where entry.Value <= m_nNumberOfPlayersToNextRound select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                m_nNumberOfPlayersToNextRound = 0;
            }

            int m_nHalfNumberOfEvenPlayers = Convert.ToInt32(m_eliminatePlayers.Count / 2);
            for (int i = 0; i < m_nHalfNumberOfEvenPlayers; i++)
            {
                Dictionary<int, int> m_currentHomePlayer = (from entry in m_eliminatePlayers where entry.Value == (m_nNumberOfPlayersToNextRound + i + 1) select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                Dictionary<int, int> m_currentAwayPlayer = (from entry in m_eliminatePlayers where entry.Value == (m_nNumberOfPlayers - i) select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                m_eliminatePlayerGames.Add(m_currentHomePlayer.First().Key, m_currentHomePlayer.First().Value);
                m_eliminatePlayerGames.Add(m_currentAwayPlayer.First().Key, m_currentAwayPlayer.First().Value);
            }
            m_dictGamePlayersPerRound.Add(m_eliminatePlayerGames);
            m_dictGamePlayersPerRound.Add(m_nextRoundPlayerGames);

            m_nCurrentRoundNumber = 1;
        }

        public void nextRound(Dictionary<int, int> i_dictPlayersToNextRound)
        {
            this.m_nCurrentRoundNumber++;

            if (m_dictGamePlayersPerRound.Count <= m_nCurrentRoundNumber - 1)
                m_dictGamePlayersPerRound.Add(new Dictionary<int, int>());
            else
            {
                //Remove any empty/undecided players (-1).
                for (int i = m_dictGamePlayersPerRound[m_nCurrentRoundNumber - 1].Count - 1; i >= 0; i--)
                {
                    KeyValuePair<int, int> kvp = m_dictGamePlayersPerRound[m_nCurrentRoundNumber - 1].ElementAt(i);
                    if (kvp.Value == -1)
                    {
                        m_dictGamePlayersPerRound[m_nCurrentRoundNumber - 1].Remove(kvp.Key);
                    }
                }
            }

            //Find next round players in dictionary for previous round based on m_dictPlayersToNextRound.
            //m_queueHandler.m_dictGamePlayersPerRound[m_nCurrentRoundNumber - 1].
            var m_dictGamePlayersPerRoundToNextRound = (from entry in m_dictGamePlayersPerRound[m_nCurrentRoundNumber - 2]
                                                        where i_dictPlayersToNextRound.Values.Contains(entry.Key)
                                                        orderby entry.Key ascending
                                                        select entry).ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (KeyValuePair<int, int> kvp in m_dictGamePlayersPerRoundToNextRound)
            {
                if (!m_dictGamePlayersPerRound[m_nCurrentRoundNumber - 1].ContainsKey(kvp.Key))
                    m_dictGamePlayersPerRound[m_nCurrentRoundNumber - 1].Add(kvp.Key, kvp.Value);
            }
        }

        public void previousRound()
        {

            //Find next round players in dictionary for previous round based on m_dictPlayersToNextRound.
            //m_queueHandler.m_dictGamePlayersPerRound[m_nCurrentRoundNumber - 1].
            if (m_nextRoundPlayers != null)
            {
                var m_dictGamePlayersPerRoundToNextRound = (from entry in m_dictGamePlayersPerRound[m_nCurrentRoundNumber - 1]
                                                            where !m_nextRoundPlayers.Values.Contains(entry.Key)
                                                            orderby entry.Key ascending
                                                            select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                foreach (KeyValuePair<int, int> kvp in m_dictGamePlayersPerRoundToNextRound)
                {
                    m_dictGamePlayersPerRound[m_nCurrentRoundNumber - 1].Remove(kvp.Key);
                }
            }

            this.m_nCurrentRoundNumber--;
        }
    }

    public class RRMatrix
    {
        public int[,] val;
        public int Cols;
        public int Rows;
        public List<int> m_playersToRotate;

        public RRMatrix(int X, int Y)
        {
            Cols = X;
            Rows = Y;
            val = new int[Rows, Cols];
            Homogenous(0);
        }

        public RRMatrix(int[,] array)
        {
            val = array;
            Cols = val.GetLength(1);
            Rows = val.GetLength(0);
            m_playersToRotate = new List<int>();
            int i = 0;
            int j = 0;
            for (i = 0; i < Rows; i++)
            {
                if (i == 0)
                {
                    for (j = 1; j < Cols; j++)
                        m_playersToRotate.Add(val[i, j]);
                }
                if (i == 1)
                    for (j = Cols - 1; j >= 0; j--)
                        m_playersToRotate.Add(val[i, j]);
            }
        }

        public void Homogenous(int i_nValue)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    val[i, j] = i_nValue;
                }
            }
        }

        public void SetIdentity()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    if (i == j)
                    {
                        val[i, j] = 1;
                    }
                    else
                    {
                        val[i, j] = 0;
                    }
                }
            }
        }

        public RRMatrix Transpose()
        {
            int[,] values = new int[Cols, Rows];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    values[j, i] = val[i, j];
                }
            }
            return new RRMatrix(values);
        }

        public RRMatrix Scale(int factor)
        {
            int[,] values = new int[Rows, Cols];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    values[i, j] = factor * val[i, j];
                }
            }
            return new RRMatrix(values);
        }

        public int Trace()
        {
            if (Rows != Cols)
            {
                return 0;
            }
            int trace = 0;
            for (int i = 0; i < Rows; i++)
            {
                trace += val[i, i];
            }
            return trace;
        }

        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    str += val[i, j].ToString() + ",";
                }
                str += Environment.NewLine;
            }
            return str;
        }

        public List<int> getRowAsList(int i_nRowNumber)
        {
            List<int> m_outRow = new List<int>();
            for (int i = 0; i < Cols; i++)
                m_outRow.Add(val[i_nRowNumber, i]);
            return m_outRow;
        }

        public static RRMatrix operator +(RRMatrix left, RRMatrix right)
        {
            if ((left.Cols != right.Cols) || (left.Rows != right.Rows))
            {
                return left;
            }

            int[,] values = new int[left.Rows, left.Cols];

            for (int i = 0; i < left.Rows; i++)
            {
                for (int j = 0; j < left.Cols; j++)
                {
                    values[i, j] = left.val[i, j] + right.val[i, j];
                }
            }
            return (new RRMatrix(values));
        }

        public static RRMatrix operator +(int left, RRMatrix right)
        {
            int[,] values = new int[right.Rows, right.Cols];
            for (int i = 0; i < right.Rows; i++)
            {
                for (int j = 0; j < right.Cols; j++)
                {
                    values[i, j] = left + right.val[i, j];
                }
            }
            return (new RRMatrix(values));
        }

        public static RRMatrix operator -(RRMatrix left, RRMatrix right)
        {
            if ((left.Cols != right.Cols) || (left.Rows != right.Rows))
            {
                return left;
            }
            int[,] values = new int[left.Rows, left.Cols];
            for (int i = 0; i < left.Rows; i++)
            {
                for (int j = 0; j < left.Cols; j++)
                {
                    values[i, j] = left.val[i, j] - right.val[i, j];

                }
            }
            return (new RRMatrix(values));
        }

        public static RRMatrix operator *(RRMatrix left, RRMatrix right)
        {
            int[,] values = new int[left.Rows, right.Cols];
            for (int h = 0; h < left.Rows; h++)
            {
                for (int i = 0; i < right.Cols; i++)
                {
                    for (int j = 0; j < left.Cols; j++)
                    {
                        values[h, i] += left.val[h, j] * right.val[j, i];
                    }
                }
            }
            return (new RRMatrix(values));
        }

        private List<int> shiftPlayerQueueRight(List<int> i_lstQueue)
        {
            int i_nQueueLength = i_lstQueue.Count;
            List<int> m_lstShiftedQueue = new List<int>(i_nQueueLength);
            for (int i = 0; i < i_nQueueLength; i++)
                m_lstShiftedQueue.Add(0);
            for (int i = 0; i < i_lstQueue.Count; i++)
            {
                if (i < i_lstQueue.Count - 1)
                    m_lstShiftedQueue[i + 1] = i_lstQueue[i];
                else
                    m_lstShiftedQueue[0] = i_lstQueue[i];
            }
            return m_lstShiftedQueue;
        }

        public void shiftRR()
        {
            this.m_playersToRotate = shiftPlayerQueueRight(m_playersToRotate);
            int i = Cols - 1;
            int k = 0;
            int m_nBreakQueueIndex = Convert.ToInt32((1 + m_playersToRotate.Count())/2) - 1;
            for (k = 0; k < m_nBreakQueueIndex; k++)
            {          
               val[0, k + 1] = m_playersToRotate.ElementAt(k);                           
            }
            for (k = m_nBreakQueueIndex; k < m_playersToRotate.Count(); k++)
            {
                val[1, i] = m_playersToRotate.ElementAt(k);
                i--;
            }          
        }

        public bool AllElementsPositive()
        {
            bool m_bAllPositive = true;

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    if (val[i, j] < 0)

                        return false;
                }
            }
            return m_bAllPositive;
        }
    }

    public class TableHockeyQueueHandler2
    {
        protected int m_nNumberOfPlayers;
        protected int m_nNumberOfRounds;
        private int m_nHalfNumberOfPlayers;
        private int m_nNumberOfPlayersLowerRotationLimit;
        private int m_nNumberOfGamesPerRound;

        public List<int> m_lstCurrentHomePlayerId;
        public List<int> m_lstCurrentAwayPlayerId;

        public int m_nCurrentRoundNumber;
        public List<int> m_lstPlayerQueue;
        private List<int> m_lstPlayerId;
        public RRMatrix m_matPlayers;

        private int[,] ConvertDictionaryToIntArray(Dictionary<int, List<int>> Dictionary)
        {
            int[,] intArray2d = new int[2, Dictionary[1].Count()];
            int i = 0;

            foreach (KeyValuePair<int, List<int>> item in Dictionary)
            {
                int j = 0;
                foreach (int m_val in item.Value)
                {
                    intArray2d[i, j] = m_val;
                    j++;
                }                           
                i++;
            }
            return intArray2d;
        }


        public TableHockeyQueueHandler2(List<int> i_lstPlayerId, int i_nNumberOfGamesPerRound)
        {
            //TODO: Re-write entire handler according to much simpler method described in TableHockey.txt!
            //TODO: Option to randomize initial player order.
            i_lstPlayerId.Sort();
            m_lstPlayerId = i_lstPlayerId;
            m_nNumberOfPlayers = m_lstPlayerId.Count;
            m_nNumberOfGamesPerRound = i_nNumberOfGamesPerRound;     
            int m_nHalfNumberOfPlayers = Convert.ToInt32(m_lstPlayerId.Count / 2);
            List<int> m_lstFirstRowPlayers = m_lstPlayerId.GetRange(0, m_nHalfNumberOfPlayers);
            List<int> m_lstSecondRowPlayers = m_lstPlayerId.GetRange(m_nHalfNumberOfPlayers, m_nHalfNumberOfPlayers);
            m_lstSecondRowPlayers.Reverse();
            Dictionary<int,List<int>> m_players = new Dictionary<int,List<int>>();
            m_players.Add(1,m_lstFirstRowPlayers);
            m_players.Add(2,m_lstSecondRowPlayers);
            m_matPlayers = new RRMatrix(ConvertDictionaryToIntArray(m_players));
            m_lstCurrentHomePlayerId = m_matPlayers.getRowAsList(0);
            m_lstCurrentAwayPlayerId = m_matPlayers.getRowAsList(1);
            m_nCurrentRoundNumber = 1;
        }

        public void nextRound()
        {
            m_matPlayers.shiftRR();
            m_lstCurrentHomePlayerId = m_matPlayers.getRowAsList(0);
            m_lstCurrentAwayPlayerId = m_matPlayers.getRowAsList(1);
            m_nCurrentRoundNumber++;
        }

        public int getIdlePlayer()
        {
            int m_nHomeDummyIndex = m_lstCurrentHomePlayerId.IndexOf(-1);
            int m_nAwayDummyIndex = m_lstCurrentAwayPlayerId.IndexOf(-1);
            if (m_nHomeDummyIndex >= 0)
                return m_lstCurrentAwayPlayerId[m_nHomeDummyIndex];
            if (m_nAwayDummyIndex >= 0)
                return m_lstCurrentHomePlayerId[m_nAwayDummyIndex];
            return -1;
        }
    }
}