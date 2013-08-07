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
    public partial class ucEndGameSeries : System.Web.UI.UserControl
    {
        private TableHockeyContestRound m_round;
        public List<GameViewModel> m_games { get; set; }
        private RoundViewModel m_vmRound { get; set; }
        public int m_nTableNumber { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void setUCGUI(RoundViewModel i_vmRound)
        {
         
            //Set scores in Gridview tbs.
            //Get results for each row and update TableHockeyGame table.
            int m_nNumberOfGamesWonHome = 0;
            int m_nNumberOfGamesWonAway = 0;
            for (int iCount = 0; iCount < this.GridViewEndGames.Rows.Count; iCount++)
            {
                TextBox currentHomeScore = (TextBox)this.GridViewEndGames.Rows[iCount].FindControl("TextBoxHomePlayerScore");
                TextBox currentAwayScore = (TextBox)this.GridViewEndGames.Rows[iCount].FindControl("TextBoxAwayPlayerScore");
                int m_nCurrentGameId = Convert.ToInt32(this.GridViewEndGames.Rows[iCount].Cells[0].Text);
                int m_nHomeScore = i_vmRound.m_vmGames.Find(g => g.GameId == m_nCurrentGameId).HomePlayerScore;
                int m_nAwayScore = i_vmRound.m_vmGames.Find(g => g.GameId == m_nCurrentGameId).AwayPlayerScore;
                if (m_nHomeScore >= 0)
                    currentHomeScore.Text = Convert.ToString(m_nHomeScore);
                else
                    currentHomeScore.Text = "";
                if (m_nAwayScore >= 0)
                    currentAwayScore.Text = Convert.ToString(m_nAwayScore);
                else
                    currentAwayScore.Text = "";
                if ((m_nHomeScore >= 0) && (m_nAwayScore >= 0))
                {
                    if (m_nHomeScore > m_nAwayScore)
                        m_nNumberOfGamesWonHome++;
                    if (m_nAwayScore > m_nHomeScore)
                        m_nNumberOfGamesWonAway++;
                }
            }
            //Set overall game standings.
            if ((i_vmRound != null) && (i_vmRound.m_vmFirstGame.Count > 0))
            {
                i_vmRound.m_vmFirstGame[0].HomePlayerScore = m_nNumberOfGamesWonHome;
                i_vmRound.m_vmFirstGame[0].AwayPlayerScore = m_nNumberOfGamesWonAway;
            }
        }

        public void getUCGUI()
        {
            if (m_vmRound == null)
                m_vmRound = (RoundViewModel)Session["ucEndGameSeries.m_vmround"];

            if (m_round == null)
                m_round = (TableHockeyContestRound)Session["ucEndGameSeries.m_round"];

            int m_nNumberOfGamesWonHome = 0;
            int m_nNumberOfGamesWonAway = 0;
            for (int iCount = 0; iCount < this.GridViewEndGames.Rows.Count; iCount++)
            {
                TextBox currentHomeScore = (TextBox)this.GridViewEndGames.Rows[iCount].FindControl("TextBoxHomePlayerScore");
                TextBox currentAwayScore = (TextBox)this.GridViewEndGames.Rows[iCount].FindControl("TextBoxAwayPlayerScore");
                CheckBox currentHasSuddenDeath = (CheckBox)this.GridViewEndGames.Rows[iCount].FindControl("CheckBoxSuddenDeath");
                int m_nCurrentGameId = Convert.ToInt32(this.GridViewEndGames.Rows[iCount].Cells[0].Text);
                var m_currentVmGame = m_vmRound.m_vmGames.Find(g => g.GameId == m_nCurrentGameId);
                var m_currentGame = m_round.TableHockeyGames.FirstOrDefault(g => g.GameId == m_nCurrentGameId);
                int m_nHomeScore = -1;
                bool m_bParseHomeScoreOk = int.TryParse(currentHomeScore.Text, out m_nHomeScore);
                if (!m_bParseHomeScoreOk)
                    m_nHomeScore = -1;
                if (m_bParseHomeScoreOk || (currentHomeScore.Text.Trim().Equals("")))
                {
                    
                    if (m_currentVmGame != null) {
                        m_currentVmGame.HomePlayerScore = m_nHomeScore;
                        m_currentVmGame.hasSuddenDeath = currentHasSuddenDeath.Checked;
                    }
                    if (m_currentGame != null)
                    {
                        m_currentGame.HomePlayerScore = m_nHomeScore;
                        m_currentGame.hasSuddenDeath = currentHasSuddenDeath.Checked;
                        Session["ucEndGameSeries.m_round"] = m_round;
                    }
                } 
                    
                int m_nAwayScore = -1;
                bool m_bParseAwayScoreOk = int.TryParse(currentAwayScore.Text, out m_nAwayScore);
                if (!m_bParseAwayScoreOk)
                    m_nAwayScore = -1;
                if (m_bParseAwayScoreOk || (currentAwayScore.Text.Trim().Equals("")))
                {
                    if (m_currentVmGame != null)
                    {
                        m_currentVmGame.AwayPlayerScore = m_nAwayScore;
                        m_currentVmGame.hasSuddenDeath = currentHasSuddenDeath.Checked;
                    }
                    if (m_currentGame != null)
                    {
                        m_currentGame.AwayPlayerScore = m_nAwayScore;
                        m_currentGame.hasSuddenDeath = currentHasSuddenDeath.Checked;
                        Session["ucEndGameSeries.m_round"] = m_round;
                    }
                }
                    
                if ((m_bParseAwayScoreOk) && (m_nHomeScore >= 0) && (m_nAwayScore >= 0))
                {
                    if (m_nHomeScore > m_nAwayScore)
                        m_nNumberOfGamesWonHome++;
                    if (m_nAwayScore > m_nHomeScore)
                        m_nNumberOfGamesWonAway++;
                }
            }
            //Set overall game standings.
            m_vmRound.m_vmFirstGame[0].HomePlayerScore = m_nNumberOfGamesWonHome;
            m_vmRound.m_vmFirstGame[0].AwayPlayerScore = m_nNumberOfGamesWonAway;
        }

        public void InitControl(TableHockeyContestRound i_TableHockeyRound)
        {
            if (i_TableHockeyRound != null)
            {
                m_round = i_TableHockeyRound;
                Session["ucEndGameSeries.m_round"] = m_round;
            }

            m_vmRound = new RoundViewModel(m_round);
            DataShow(m_vmRound);
        }

        public void DataShow(RoundViewModel i_round)
        {
            this.GridViewEndGames.DataSource = m_vmRound.m_vmGames;
            this.GridViewEndGames.DataBind();
            setUCGUI(m_vmRound);
            Session["ucEndGameSeries.m_vmround"] = m_vmRound;
            m_games = m_vmRound.m_vmGames;
            this.GridViewEndGameOverview.DataSource = m_vmRound.m_vmFirstGame;
            this.GridViewEndGameOverview.DataBind();
        }

        protected void ButtonRefresh_Click(object sender, EventArgs e)
        {
            getUCGUI();
            DataShow(m_vmRound);
        }
    }
}