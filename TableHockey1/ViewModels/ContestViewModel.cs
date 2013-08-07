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
}