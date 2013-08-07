using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TableHockey;
using TableHockeyData;

namespace TableHockey
{

    public class PlayerViewModel
    { 
        public int PlayerId {get; set;}
        public string FirstName {get; set;}
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string ClubName { get; set; }
        public byte[] PlayerBinary { get; set; }
        public int PreviousStanding { get; set; }
    }

    public class EndGamePlayerViewModelList
    {
        public Dictionary<int,int> m_lstContestRankedPlayerId;
        public EndGamePlayerViewModelList(List<TableHockeyContestPlayer> i_lstContestPlayer)
        {
            m_lstContestRankedPlayerId = new Dictionary<int, int>();
            foreach (TableHockeyContestPlayer m_player in i_lstContestPlayer)
                m_lstContestRankedPlayerId.Add(m_player.PlayerId, (int)m_player.FinalPreviousStanding);
        }
    }

    public class PlayerViewModelList
    {
        public List<int> m_lstContestPlayerId;
        public PlayerViewModelList(List<TableHockeyContestPlayer> i_lstContestPlayer)
        {
            m_lstContestPlayerId = new List<int>();
            foreach (TableHockeyContestPlayer m_player in i_lstContestPlayer)
                m_lstContestPlayerId.Add(m_player.PlayerId);
        }
    }
}