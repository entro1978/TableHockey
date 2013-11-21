using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TableHockeyData;
using System.Web.Security;

namespace TableHockey.uc
{
    public partial class ucEditTableHockeyContest : System.Web.UI.UserControl
    {

        protected TableHockeyContest m_contest;

        public TableHockeyContest m_currentContest 
        {
            get
            {
                m_contest = getUCGUI(); 
                return m_contest; 
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ucCalendarClosed.TextBoxToUse = TextBoxContestClosedDate.ID;
            this.ucCalendarStarted.TextBoxToUse = TextBoxContestStartedDate.ID;
            this.ucCalendarClosed.DisableDatesBeforeToday = false;
            this.ucCalendarStarted.DisableDatesBeforeToday = false;
            this.ucCalendarClosed.DisableWeekends = false;
            this.ucCalendarStarted.DisableWeekends = false;
        }

        public void InitControl(TableHockeyData.TableHockeyContest i_TableHockeyContest)
        {
            if (i_TableHockeyContest != null)
            {
                //Edit existing contest
                m_contest = i_TableHockeyContest;
            }
            else
            { 
               //Create new contest
                m_contest = new TableHockeyContest();
                m_contest.ContestId = -1;
            }
            setUCGUI();
            Session["ucEditTableHockeyContest.m_contest"] = m_contest;
        }

        private TableHockeyContest getUCGUI()
        {
            if (Session["ucEditTableHockeyContest.m_contest"] != null)
            {
                m_contest = (TableHockeyContest)Session["ucEditTableHockeyContest.m_contest"];
            }
            TableHockeyContest m_GUIContest = new TableHockeyContest();
            if (m_contest == null || m_contest.ContestId <= 0)
            {
                m_GUIContest.ContestId = -1;
            }
            else
            {
                m_GUIContest.ContestId = m_contest.ContestId;
            }
            if (!String.IsNullOrEmpty(TextBoxContestStartedDate.Text))
                m_GUIContest.ContestDateOpened = Convert.ToDateTime(TextBoxContestStartedDate.Text);
            else
                m_GUIContest.ContestDateOpened = new DateTime(1900, 1, 1);
            if (!String.IsNullOrEmpty(TextBoxContestClosedDate.Text))
                m_GUIContest.ContestDateClosed = Convert.ToDateTime(TextBoxContestClosedDate.Text);
            else
                m_GUIContest.ContestDateClosed = new DateTime(1900, 1, 1);
            if (!String.IsNullOrEmpty(TextBoxContestDescription.Text))
                m_GUIContest.ContestDescription = TextBoxContestDescription.Text;
            else
                m_GUIContest.ContestDescription = "";
            if (!String.IsNullOrEmpty(TextBoxContestName.Text))
                m_GUIContest.ContestName = TextBoxContestName.Text;
            else
                m_GUIContest.ContestName = "";
            if (!String.IsNullOrEmpty(TextBoxContestLocation.Text))
                m_GUIContest.ContestLocation = TextBoxContestLocation.Text;
            else
                m_GUIContest.ContestLocation = "";
            if (!String.IsNullOrEmpty(TextBoxNumberOfGameMinutes.Text))
                m_GUIContest.GameLengthMinutes = Convert.ToInt32(TextBoxNumberOfGameMinutes.Text);
            else
                m_GUIContest.GameLengthMinutes = 5;  //TODO: Setting!
            if (!String.IsNullOrEmpty(TextBoxNumberOfPlayersToNextRound.Text))
                m_GUIContest.NumberOfPlayersToNextRound = Convert.ToInt32(TextBoxNumberOfPlayersToNextRound.Text);
            else
                m_GUIContest.NumberOfPlayersToNextRound = -1;
            if (!String.IsNullOrEmpty(TextBoxNumberOfRounds.Text))
                m_GUIContest.numberOfRounds = Convert.ToInt32(TextBoxNumberOfRounds.Text);
            else
                m_GUIContest.numberOfRounds = 1;    //TODO:  Setting?!  
            string[] m_sPointsWTL = {"2", "1", "0"};   //TODO:  Setting?!
            if (!String.IsNullOrEmpty(TextBoxPointsWinTieLoss.Text))
            {
                m_sPointsWTL = TextBoxPointsWinTieLoss.Text.Split('/');
                m_GUIContest.PointsWinningGame = Convert.ToInt32(m_sPointsWTL[0]);
                m_GUIContest.PointsTiedGame = Convert.ToInt32(m_sPointsWTL[1]);
                m_GUIContest.PointsLostGame = Convert.ToInt32(m_sPointsWTL[2]);
            }
            m_GUIContest.isGoalDifferenceRanked = (RadioButtonListGoalDifference.SelectedValue == "1");
            m_GUIContest.isFinalGameContest = (RadioButtonListTypeOfContest.SelectedValue == "1");
            MembershipUser CurrentUser = Membership.GetUser(this.Page.User.Identity.Name);
            m_GUIContest.OwnerUserId = (Guid)CurrentUser.ProviderUserKey;
            m_GUIContest.isPubliclyVisible = chbPubliclyVisible.Checked;
            Session["ucEditTableHockeyContest.m_contest"] = m_GUIContest;
            return m_GUIContest;
        }

        private void setUCGUI()
        {
            if (m_contest != null)
            {
                if (m_contest.ContestId <= 0)
                {
                    m_contest.ContestId = -1;
                    LabelContestId.Text = "[New contest]";
                } else 
                {
                    LabelContestId.Text = Convert.ToString(m_contest.ContestId);
                }
                if (m_contest.ContestDateOpened != null && !m_contest.ContestDateOpened.Equals(DateTime.MinValue))
                    TextBoxContestStartedDate.Text = m_contest.ContestDateOpened.ToString("yyyy-MM-dd");
                else
                    TextBoxContestStartedDate.Text = "";
                if (m_contest.ContestDateClosed != null && !m_contest.ContestDateClosed.Equals(DateTime.MinValue))
                    TextBoxContestClosedDate.Text = m_contest.ContestDateClosed.GetValueOrDefault().ToString("yyyy-MM-dd");
                else
                    TextBoxContestClosedDate.Text = "";
                if (!String.IsNullOrEmpty(m_contest.ContestDescription))
                    TextBoxContestDescription.Text = m_contest.ContestDescription;
                else
                    TextBoxContestDescription.Text = "";
                if (!String.IsNullOrEmpty(m_contest.ContestName))
                    TextBoxContestName.Text = m_contest.ContestName;
                else
                    TextBoxContestName.Text = "";
                if (!String.IsNullOrEmpty(m_contest.ContestLocation))
                    TextBoxContestLocation.Text = m_contest.ContestLocation;
                else
                    TextBoxContestLocation.Text = "";
                if (m_contest.GameLengthMinutes > 0)
                    TextBoxNumberOfGameMinutes.Text = Convert.ToString(m_contest.GameLengthMinutes);
                else
                    TextBoxNumberOfGameMinutes.Text = "5";  //TODO: Setting!
                if (m_contest.NumberOfPlayersToNextRound != null && m_contest.NumberOfPlayersToNextRound > 0)
                    TextBoxNumberOfPlayersToNextRound.Text = Convert.ToString(m_contest.NumberOfPlayersToNextRound);
                else
                    TextBoxNumberOfPlayersToNextRound.Text = "";
                if (m_contest.numberOfRounds > 0)
                    TextBoxNumberOfRounds.Text = Convert.ToString(m_contest.numberOfRounds);
                else
                    TextBoxNumberOfRounds.Text = "1";    //TODO:  Setting?! 
 
                string[] m_sPointsWTL = { "2", "1", "0" };   //TODO:  Setting?!
                if (m_contest.PointsWinningGame > 0)
                    m_sPointsWTL[0] = Convert.ToString(m_contest.PointsWinningGame);
                if (m_contest.PointsTiedGame > 0)
                    m_sPointsWTL[1] = Convert.ToString(m_contest.PointsTiedGame);
                if (m_contest.PointsLostGame > 0)
                    m_sPointsWTL[2] = Convert.ToString(m_contest.PointsLostGame);

                TextBoxPointsWinTieLoss.Text = m_sPointsWTL[0] + "/" + m_sPointsWTL[1] + "/" + m_sPointsWTL[2];
                RadioButtonListGoalDifference.SelectedValue = m_contest.isGoalDifferenceRanked ? "1" : "0";
                RadioButtonListTypeOfContest.SelectedValue = m_contest.isFinalGameContest ? "1" : "0";
            }
        }
    }
}