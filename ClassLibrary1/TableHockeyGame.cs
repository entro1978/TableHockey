//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TableHockeyData
{
    using System;
    using System.Collections.Generic;
    
    public partial class TableHockeyGame
    {
        public int GameId { get; set; }
        public int HomePlayerId { get; set; }
        public int AwayPlayerId { get; set; }
        public Nullable<int> HomePlayerScore { get; set; }
        public Nullable<int> AwayPlayerScore { get; set; }
        public System.DateTime GameStartDate { get; set; }
        public Nullable<System.DateTime> GameClosedDate { get; set; }
        public bool isFinalGame { get; set; }
        public bool hasSuddenDeath { get; set; }
        public int ContestId { get; set; }
        public int TableHockeyContestRoundId { get; set; }
        public int TableNumber { get; set; }
        public int IdlePlayerId { get; set; }
    
        public virtual TableHockeyContest TableHockeyContest { get; set; }
        public virtual TableHockeyContestRound TableHockeyContestRound { get; set; }
        public virtual TableHockeyPlayer TableHockeyPlayer { get; set; }
        public virtual TableHockeyPlayer TableHockeyPlayer1 { get; set; }
        public virtual TableHockeyPlayer TableHockeyPlayer2 { get; set; }
    }
}
