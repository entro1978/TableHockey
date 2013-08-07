using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using TableHockeyData;
using TableHockey;


namespace TableHockey.uc
{
    public partial class ucEditClub : System.Web.UI.UserControl
    {
        private TableHockeyClub m_club;

        public TableHockeyClub m_currentClub
        {
            get
            {
                m_club = getUCGUI();
                return m_club;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ucCalendarStarted.TextBoxToUse = TextBoxClubFoundedDate.ID;
            this.ucCalendarStarted.DisableDatesBeforeToday = false;
            this.ucCalendarStarted.DisableWeekends = false;

            this.ucCalendarClosed.TextBoxToUse = TextBoxClubDefunctDate.ID;
            this.ucCalendarClosed.DisableDatesBeforeToday = true;
            this.ucCalendarClosed.DisableWeekends = false;
        }

        private void InitclubImage(TableHockeyClub i_club)
        {
            if ((i_club.ClubId > 0) && (i_club.ClubBinary != null))
            {
                divImage.Visible = true;
                ImageClub.ImageUrl = "~/ImageHandler.ashx?id=" + i_club.ClubId + "&imagetype=club";
            }
            else
            {
                divImage.Visible = false;
            }
        }

        public void InitControl(TableHockeyClub i_TableHockeyClub)
        {
            if (i_TableHockeyClub != null)
            {
                //Edit existing club
                m_club = i_TableHockeyClub;
            }
            else
            {
                //Create new club
                m_club = new TableHockeyClub();
                m_club.ClubId = -1;
                m_club.FoundedDate = DateTime.MinValue;
                m_club.DefunctDate = DateTime.MinValue;
            }

            InitclubImage(m_club);
            setUCGUI();
            Session["ucEditclub.m_club"] = m_club;
        }

        private TableHockeyClub getUCGUI()
        {
            if (Session["ucEditclub.m_club"] != null)
            {
                m_club = (TableHockeyClub)Session["ucEditclub.m_club"];
            }
            TableHockeyClub m_GUIclub = new TableHockeyClub();
            if (m_club == null || m_club.ClubId <= 0)
            {
                m_GUIclub.ClubId = -1;
            }
            else
            {
                m_GUIclub.ClubId = m_club.ClubId;
            }

            m_GUIclub.ClubContactEmail = TextBoxContactEmail.Text;
            m_GUIclub.ClubContactPerson = TextBoxClubContactPerson.Text;
            m_GUIclub.ClubLocation = TextBoxClubLocation.Text;
            m_GUIclub.ClubName = TextBoxClubName.Text;
            m_GUIclub.ClubPhoneNo = TextBoxClubPhoneNo.Text;
            m_GUIclub.ClubCountryCode = ddlClubCountry.SelectedValue;
            m_GUIclub.FoundedDate = Convert.ToDateTime(TextBoxClubFoundedDate.Text);
            if (!String.IsNullOrEmpty(TextBoxClubDefunctDate.Text))
               m_GUIclub.DefunctDate = Convert.ToDateTime(TextBoxClubDefunctDate.Text);

            if (FileUploadClubImage.FileBytes.Length > 0)
            {
                m_GUIclub.ClubBinary = PageUtility.DownscaleImageToWidth(FileUploadClubImage.FileBytes, 43);  //TODO: Setting!
            }
            else
            {
                m_GUIclub.ClubBinary = m_club.ClubBinary;
            }
            m_GUIclub.RegisteredByUserId = m_club.RegisteredByUserId;
            Session["ucEditclub.m_club"] = m_GUIclub;
            return m_GUIclub;
        }

        private void setUCGUI()
        {
            if (m_club != null)
            {
                //Set DDL selected club if existing club.
                if (m_club.ClubId > 0)
                {
                    LabelClubId.Text = Convert.ToString(m_club.ClubId);
                }
                if ((m_club.FoundedDate != null) && !(m_club.FoundedDate.Equals(DateTime.MinValue)))
                {
                    TextBoxClubFoundedDate.Text = Convert.ToDateTime(m_club.FoundedDate).ToString("yyyy-MM-dd");
                }
                if ((m_club.DefunctDate != null) && !(m_club.DefunctDate.Equals(DateTime.MinValue)))
                {
                    TextBoxClubDefunctDate.Text = Convert.ToDateTime(m_club.DefunctDate).ToString("yyyy-MM-dd");
                }
                //Set club details if existing club.
                if (!String.IsNullOrEmpty(m_club.ClubName))
                {
                    TextBoxClubName.Text = m_club.ClubName;
                }
                if (!String.IsNullOrEmpty(m_club.ClubPhoneNo))
                {
                    TextBoxClubPhoneNo.Text = m_club.ClubPhoneNo;
                }
                if (!String.IsNullOrEmpty(m_club.ClubContactEmail))
                {
                    TextBoxContactEmail.Text = m_club.ClubContactEmail;
                }
                if (!String.IsNullOrEmpty(m_club.ClubContactPerson))
                {
                    TextBoxClubContactPerson.Text = m_club.ClubPhoneNo;
                }
                if (!String.IsNullOrEmpty(m_club.ClubCountryCode))
                {
                    ddlClubCountry.SelectedValue = m_club.ClubCountryCode;
                }
                if (!String.IsNullOrEmpty(m_club.ClubLocation))
                {
                    TextBoxClubLocation.Text = m_club.ClubLocation;
                }
            } else
            {
            }
        }
    }
}