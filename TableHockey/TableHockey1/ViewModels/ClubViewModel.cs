using TableHockeyData;
using System.Collections.Generic;

namespace TableHockey
{
    public class ClubViewModelList
    {
        public class ClubViewModel
        {
            public int ClubId { get; set; }
            public string ClubDescription { get; set; }

            public ClubViewModel(TableHockeyClub i_Club)
            {
                ClubId = i_Club.ClubId;
                ClubDescription = i_Club.ClubName;
                if (!string.IsNullOrEmpty(i_Club.ClubLocation))
                {
                    ClubDescription += " (" + i_Club.ClubLocation + ")";
                }
            }
        }

        public List<ClubViewModel> m_ClubVmList { get; set; }

        public ClubViewModelList(List<TableHockeyClub> i_ClubList)
        {
            m_ClubVmList = new List<ClubViewModel>();
            foreach (TableHockeyClub i_Club in i_ClubList)
            {
                m_ClubVmList.Add(new ClubViewModel(i_Club));
            }
        }
    }
}

