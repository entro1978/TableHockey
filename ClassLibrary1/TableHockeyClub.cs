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
    
    public partial class TableHockeyClub
    {
        public int ClubId { get; set; }
        public string ClubName { get; set; }
        public string ClubPhoneNo { get; set; }
        public System.DateTime FoundedDate { get; set; }
        public Nullable<System.DateTime> DefunctDate { get; set; }
        public string ClubLocation { get; set; }
        public byte[] ClubBinary { get; set; }
        public Nullable<System.Guid> RegisteredByUserId { get; set; }
        public string ClubContactEmail { get; set; }
        public string ClubCountryCode { get; set; }
        public string ClubContactPerson { get; set; }
    }
}
