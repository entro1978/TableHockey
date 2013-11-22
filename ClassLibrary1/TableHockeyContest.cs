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
    
    public partial class TableHockeyContest
    {
        public TableHockeyContest()
        {
            this.TableHockeyContestPlayer = new HashSet<TableHockeyContestPlayer>();
            this.TableHockeyContestRound = new HashSet<TableHockeyContestRound>();
            this.TableHockeyGame = new HashSet<TableHockeyGame>();
        }
    
        public int ContestId { get; set; }
        public string ContestName { get; set; }
        public System.DateTime ContestDateOpened { get; set; }
        public string ContestLocation { get; set; }
        public Nullable<System.DateTime> ContestDateClosed { get; set; }
        public string ContestDescription { get; set; }
        public byte[] ContestBinary { get; set; }
        public bool isFinalGameContest { get; set; }
        public bool isGoalDifferenceRanked { get; set; }
        public int numberOfRounds { get; set; }
        public int PointsWinningGame { get; set; }
        public int PointsTiedGame { get; set; }
        public int PointsLostGame { get; set; }
        public int GameLengthMinutes { get; set; }
        public Nullable<int> NumberOfPlayersToNextRound { get; set; }
        public Nullable<System.Guid> OwnerUserId { get; set; }
        public Nullable<int> EndGameForContestId { get; set; }
        public bool isPubliclyVisible { get; set; }
    
        public virtual ICollection<TableHockeyContestPlayer> TableHockeyContestPlayer { get; set; }
        public virtual ICollection<TableHockeyContestRound> TableHockeyContestRound { get; set; }
        public virtual ICollection<TableHockeyGame> TableHockeyGame { get; set; }
    }
}
