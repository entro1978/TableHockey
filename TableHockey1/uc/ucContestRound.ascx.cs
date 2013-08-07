using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TableHockey;
using TableHockeyData;

namespace TableHockey.uc
{
    public partial class ucContestRound : System.Web.UI.UserControl
    {

        private TableHockeyContestRound m_round;
        private string m_sIdlePlayerDescription;
        private string m_sIdlePlayerDescriptionPlaceholder;  
        public List<GameViewModel> m_games { get; set;}
        private RoundViewModel m_vmRound {get; set;}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                m_sIdlePlayerDescriptionPlaceholder = "[P] stands idle."; //TODO:  Config setting!
                Session["ucContestRound.IdlePlayerPlaceHolder"] = m_sIdlePlayerDescriptionPlaceholder;
                if (m_sIdlePlayerDescription == null)
                    m_sIdlePlayerDescription = Convert.ToString(Session["ucContestRound.IdlePlayer"]);
                if (m_vmRound.m_vmGames[0].IdlePlayerId != -1)
                {
                    this.LabelIdlePlayerDescription.Text = m_sIdlePlayerDescriptionPlaceholder.Replace("[P]", m_sIdlePlayerDescription);
                }
                    else
                {
                    this.LabelIdlePlayerDescription.Text = "";
                }          
            }
            else
            { 
            
            }
            
        }

        public void  InitControl(TableHockeyContestRound i_TableHockeyRound)
        {
            if (i_TableHockeyRound != null)
            {
                m_round = i_TableHockeyRound;
                Session["ucContestRound.m_round"] = m_round;
            }

            m_vmRound = new RoundViewModel(m_round);
            this.GridViewContestRoundGames.DataSource = m_vmRound.m_vmGames;
            this.GridViewContestRoundGames.DataBind();
            Session["ucContestRound.m_vmround"] = m_vmRound;
            m_games = m_vmRound.m_vmGames;
            setUCGUI();
        }

        public void getUCGUI()
        {
            if (Session["ucContestRound.m_vmround"] != null)
            {
                m_vmRound = (RoundViewModel)Session["ucContestRound.m_vmround"];
            }

              //Get results for each row and update TableHockeyGame table.
            for (int iCount = 0; iCount < this.GridViewContestRoundGames.Rows.Count; iCount++)
            {
                TextBox currentHomeScoreEntered = (TextBox)this.GridViewContestRoundGames.Rows[iCount].FindControl("TextBoxHomePlayerScore");
                TextBox currentAwayScoreEntered = (TextBox)this.GridViewContestRoundGames.Rows[iCount].FindControl("TextBoxAwayPlayerScore");
                int m_nCurrentGameId = Convert.ToInt32(this.GridViewContestRoundGames.Rows[iCount].Cells[0].Text);
                int m_nHomeScore = -1;
                if ((int.TryParse(currentHomeScoreEntered.Text, out m_nHomeScore)) && m_nHomeScore >= 0)
                    m_vmRound.m_vmGames.Find(g => g.GameId == m_nCurrentGameId).HomePlayerScore = Convert.ToInt32(currentHomeScoreEntered.Text);
                else
                    m_vmRound.m_vmGames.Find(g => g.GameId == m_nCurrentGameId).HomePlayerScore = -1;
                int m_nAwayScore = -1;
                if ((int.TryParse(currentAwayScoreEntered.Text, out m_nAwayScore)) && m_nAwayScore >= 0)
                    m_vmRound.m_vmGames.Find(g => g.GameId == m_nCurrentGameId).AwayPlayerScore = Convert.ToInt32(currentAwayScoreEntered.Text);
                else
                    m_vmRound.m_vmGames.Find(g => g.GameId == m_nCurrentGameId).AwayPlayerScore = -1;
            }
            Session["ucContestRound.m_vmround"] = m_vmRound;
            m_games = m_vmRound.m_vmGames;
        }

        private void setUCGUI()
        {
            //Update idle player string.
           
            if (m_vmRound.m_vmGames[0].IdlePlayerId != -1)
            {
                m_sIdlePlayerDescriptionPlaceholder = Convert.ToString(Session["ucContestRound.IdlePlayerPlaceHolder"]);
                this.m_sIdlePlayerDescription = m_vmRound.m_vmGames[0].IdlePlayerDescription;
                Session["ucContestRound.IdlePlayer"] = this.m_sIdlePlayerDescription;
                this.LabelIdlePlayerDescription.Text = m_sIdlePlayerDescriptionPlaceholder.Replace("[P]", m_sIdlePlayerDescription);
            }
                else
            {
                this.LabelIdlePlayerDescription.Text = "";
            }
          
            //Set scores in Gridview tbs.
            //Get results for each row and update TableHockeyGame table.
            for (int iCount = 0; iCount < this.GridViewContestRoundGames.Rows.Count; iCount++)
            {
                TextBox currentHomeScore = (TextBox)this.GridViewContestRoundGames.Rows[iCount].FindControl("TextBoxHomePlayerScore");
                TextBox currentAwayScore = (TextBox)this.GridViewContestRoundGames.Rows[iCount].FindControl("TextBoxAwayPlayerScore");
                int m_nCurrentGameId = Convert.ToInt32(this.GridViewContestRoundGames.Rows[iCount].Cells[0].Text);
                int m_nHomeScore = m_vmRound.m_vmGames.Find(g => g.GameId == m_nCurrentGameId).HomePlayerScore;
                int m_nAwayScore = m_vmRound.m_vmGames.Find(g => g.GameId == m_nCurrentGameId).AwayPlayerScore;
                if (m_nHomeScore >= 0)
                    currentHomeScore.Text = Convert.ToString(m_nHomeScore);
                else
                    currentHomeScore.Text = "";
                if (m_nAwayScore >= 0)
                    currentAwayScore.Text = Convert.ToString(m_nAwayScore);
                else
                    currentAwayScore.Text = "";
            }
        }

        public bool isRoundDataOk()
        {
            getUCGUI();
            for (int iCount = 0; iCount < m_games.Count; iCount++)
            {
                string currentHomeScoreEntered = Convert.ToString(m_games[iCount].HomePlayerScore);
                string currentAwayScoreEntered = Convert.ToString(m_games[iCount].AwayPlayerScore);
                int m_nHomeScore;
                bool isHomeScoreNumeric = int.TryParse(currentHomeScoreEntered.Trim(), out m_nHomeScore);
                int m_nAwayScore;
                bool isAwayScoreNumeric = int.TryParse(currentAwayScoreEntered.Trim(), out m_nAwayScore);
                if (!isHomeScoreNumeric || !isAwayScoreNumeric || m_nHomeScore < 0 || m_nAwayScore < 0)
                    return false;
            }
            return true;
        }
    }
}