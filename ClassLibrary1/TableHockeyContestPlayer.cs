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
    
    public partial class TableHockeyContestPlayer
    {
        public int TableHockeyContestPlayerId { get; set; }
        public int ContestId { get; set; }
        public int PlayerId { get; set; }
        public bool isDummyContestPlayer { get; set; }
        public Nullable<int> FinalPreviousStanding { get; set; }
    
        public virtual TableHockeyPlayer TableHockeyPlayer { get; set; }
        public virtual TableHockeyContest TableHockeyContest { get; set; }
    }
}
