using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TableHockeyData;
using System.Web.Security;

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

    public class TableHockeyContestViewModel
    {
        public int m_nContestId { get; set; }
        public string m_sContestName { get; set; }
        public string m_sContestLocation { get; set; }
        public string m_sContestDescription { get; set; }
        public string m_sContestStarted { get; set; }
        public string m_sUser {get; set;}
        public byte[] m_contestBinary { get; set; }

        public TableHockeyContestViewModel(TableHockeyContest i_contest)
        {
            this.m_nContestId = i_contest.ContestId;
            this.m_sContestName = i_contest.ContestName;
            this.m_sContestLocation = i_contest.ContestLocation;
            this.m_sContestDescription = i_contest.ContestDescription;
            this.m_sContestStarted = i_contest.ContestDateOpened.ToString("yyyy-MM-dd");
            MembershipUser mu = Membership.GetUser(i_contest.OwnerUserId.GetValueOrDefault());
            string m_sUserName = "";
            if (mu!= null)
               m_sUserName = mu.UserName;
            this.m_sUser = m_sUserName;
        }

    }
}