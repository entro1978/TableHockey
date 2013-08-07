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
    public partial class ucViewEndGameSeries : System.Web.UI.UserControl
    {
        private TableHockeyContestRound m_round;
        public List<GameViewModel> m_games { get; set; }
        private RoundViewModel m_vmRound { get; set; }
        public int m_nTableNumber { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
             
        }

        private void setUCGUI(RoundViewModel i_vmRound, int i_nNumberOfIndents)
        {
            string m_gamesDescription = "";
            int iCount = 0;
            foreach (GameViewModel m_model in i_vmRound.m_vmGames.OrderBy(g => g.GameId))
            {
                if ((m_model.HomePlayerScore >= 0) && (m_model.HomePlayerScore >= 0))
                {
                    m_gamesDescription += m_model.HomePlayerScore + "-" + m_model.AwayPlayerScore + ((m_model.hasSuddenDeath) ? "s" : "");
                    if ((iCount < i_vmRound.m_vmGames.Count() - 1) && (i_vmRound.m_vmGames[iCount + 1].HomePlayerScore >= 0) && (i_vmRound.m_vmGames[iCount + 1].AwayPlayerScore >= 0))
                    {
                        m_gamesDescription += ",";
                    }
                }                
                iCount++;
            }
            lblGameResults.Text = m_gamesDescription; 
            //Set overall game standings.
            if ((i_vmRound != null) && (i_vmRound.m_vmFirstGame.Count > 0))
            {
                List<int> m_lstGamesWon = TableHockeyContestHandler.getNumberOfGamesWon(i_vmRound);
                i_vmRound.m_vmFirstGame[0].AwayPlayerScore = m_lstGamesWon[0];
                i_vmRound.m_vmFirstGame[0].HomePlayerScore = m_lstGamesWon[1];
            }
            //Indent control
            string m_sIndent = "";
            for (int jCount = 0; jCount < i_nNumberOfIndents; jCount++)
                m_sIndent += "&nbsp;";
            this.lblFiller.Text = m_sIndent;
        }

        public void getUCGUI()
        {
        }

        public void InitControl(TableHockeyContestRound i_TableHockeyRound, int i_nNumberOfIndents = 0)
        {
            if (i_TableHockeyRound != null)
            {
                m_round = i_TableHockeyRound;
                Session["ucEndGameSeries.m_round"] = m_round;
            }

            m_vmRound = new RoundViewModel(m_round);
            DataShow(m_vmRound, i_nNumberOfIndents);
        }

        public void DataShow(RoundViewModel i_round, int i_nNumberOfIndents)
        {
            setUCGUI(m_vmRound, i_nNumberOfIndents);
            Session["ucEndGameSeries.m_vmround"] = m_vmRound;
            m_games = m_vmRound.m_vmGames;
            this.GridViewEndGameOverview.DataSource = m_vmRound.m_vmFirstGame;
            this.GridViewEndGameOverview.DataBind();
        }
    }
}