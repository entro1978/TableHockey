using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TableHockeyData;

namespace TableHockey.ViewModels
{
    public class ContestViewModel
    {

        public int ContestId { get; set; }
        public string ContestHeader { get; set; }
        public string ContestDescription { get; set; }

        public ContestViewModel(TableHockeyContest i_Contest)
        {
            ContestId = i_Contest.ContestId;
            if (!string.IsNullOrEmpty(i_Contest.ContestName))
            {
                ContestDescription = i_Contest.ContestName;
            }
            if (!string.IsNullOrEmpty(i_Contest.ContestLocation))
            {
                ContestDescription += " (" + i_Contest.ContestLocation + ") ";
            }
            if (i_Contest.ContestDateOpened != null)
            {
                ContestDescription += i_Contest.ContestDateOpened.ToString("yyyy-MM-dd");
            }
            ContestHeader = "Contest (ID: " + Convert.ToString(ContestId) + ")";
        }
    }

    //Class just to preserve the own compareto implementation when TableHockey is re-generated.

    public class TableHockeyGameComparableList
    {
        List<TableHockeyGameComparable> m_list { get; set; }

        public TableHockeyGameComparableList(List<TableHockeyGame> i_games)
        {
            List<TableHockeyGameComparable> m_newLst = new List<TableHockeyGameComparable>();
            foreach (TableHockeyGame m_game in i_games)
                m_newLst.Add(new TableHockeyGameComparable(m_game));
            m_list = m_newLst;
        }

        public List<TableHockeyGameComparable> tableHockeyGameList()
        {
            return m_list;
        }
    }

    public class TableHockeyGameComparable : IEquatable<TableHockeyGameComparable>

    {
        TableHockeyGame m_game { get; set; }

        public TableHockeyGameComparable(TableHockeyGame i_game)
        {
            m_game = i_game;
        }

        public bool Equals(TableHockeyGameComparable otherGame)
        {
            //Check whether the compared object is null. 
            if (Object.ReferenceEquals(otherGame, null)) return false;

            //Check whether the compared object references the same data. 
            if (Object.ReferenceEquals(this, otherGame)) return true;

            if (otherGame.m_game.AwayPlayerId != m_game.AwayPlayerId)
                return false;
            if (otherGame.m_game.HomePlayerId != m_game.HomePlayerId)
                return false;
            if (otherGame.m_game.ContestId != m_game.ContestId)
                return false;
            if (otherGame.m_game.TableHockeyContestRoundId != m_game.TableHockeyContestRoundId)
                return false;
            if (otherGame.m_game.TableNumber != m_game.TableNumber)
                return false;
            return true;
        }

        // If Equals() returns true for a pair of objects  
        // then GetHashCode() must return the same value for these objects. 

        public override int GetHashCode()
        {
            int hashGame = m_game == null ? 0 : m_game.GetHashCode();
            return hashGame;
        }
    }
}