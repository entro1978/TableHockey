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
            //Update if any changed players.
            bool m_bHasChanges = (bool)Session["ucEndGameSeries.hasChanges"];
            if (m_bHasChanges)
            {
                m_vmRound = (RoundViewModel)Session["ucEndGameSeries.m_vmround"];
                m_round = (TableHockeyContestRound)Session["ucEndGameSeries.m_round"];
            }
                
            int m_nNumberOfGamesWonHome = 0;
            int m_nNumberOfGamesWonAway = 0;
            for (int iCount = 0; iCount < this.GridViewEndGames.Rows.Count; iCount++)
            {
                TextBox currentHomeScore = (TextBox)this.GridViewEndGames.Rows[iCount].FindControl("TextBoxHomePlayerScore");
                TextBox currentAwayScore = (TextBox)this.GridViewEndGames.Rows[iCount].FindControl("TextBoxAwayPlayerScore");
                CheckBox currentHasSuddenDeath = (CheckBox)this.GridViewEndGames.Rows[iCount].FindControl("CheckBoxSuddenDeath");
                int m_nCurrentGameId = Convert.ToInt32(this.GridViewEndGames.Rows[iCount].Cells[0].Text);
                var m_currentVmGame = m_vmRound.m_vmGames.Find(g => g.GameId == m_nCurrentGameId);
                var m_currentGame = m_round.TableHockeyGame.FirstOrDefault(g => g.GameId == m_nCurrentGameId);
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

        protected void GridViewEndGameOverview_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList DdlHomePlayer = (DropDownList)e.Row.Cells[0].FindControl("DropDownListHomePlayer");
                DropDownList DdlAwayPlayer = (DropDownList)e.Row.Cells[0].FindControl("DropDownListAwayPlayer");
                //Call Datashow for each player.. :)
                m_vmRound = (RoundViewModel)Session["ucEndGameSeries.m_vmround"];
                DataShowPlayer(m_vmRound.ContestId, m_vmRound.m_vmFirstGame[0].HomePlayerId, DdlHomePlayer);
                DataShowPlayer(m_vmRound.ContestId, m_vmRound.m_vmFirstGame[0].AwayPlayerId, DdlAwayPlayer);
            }
        }

        private void DataShowPlayer(int i_nContestId, int i_nPlayerId, DropDownList i_ddl)
        {
            //Get players for current end game contest.

            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
            {
                //Get players.
                var contestPlayersQuery = from p in context.TableHockeyPlayer
                                          join cp in context.TableHockeyContestPlayer on p.PlayerId equals cp.PlayerId
                                          where cp.ContestId == i_nContestId
                                          select new { Value = p.PlayerId, Text = p.FirstName + " " + p.LastName };

                i_ddl.DataSource = contestPlayersQuery.ToList();
                i_ddl.DataValueField = "Value";
                i_ddl.DataTextField = "Text";
                i_ddl.DataBind();
            }
            i_ddl.SelectedValue = Convert.ToString(i_nPlayerId);
        }

        protected void updateHomePlayer(int i_nPlayerId)
        {
            m_vmRound.m_vmFirstGame[0].HomePlayerId = i_nPlayerId;
            foreach (GameViewModel m_gvm in m_vmRound.m_vmGames)
                m_gvm.HomePlayerId = i_nPlayerId;
            foreach (TableHockeyGame m_game in m_round.TableHockeyGame)
            {
                m_game.HomePlayerId = i_nPlayerId;
            }
        }

        protected void updateAwayPlayer(int i_nPlayerId)
        {
            m_vmRound.m_vmFirstGame[0].AwayPlayerId = i_nPlayerId;
            foreach (GameViewModel m_gvm in m_vmRound.m_vmGames)
                m_gvm.AwayPlayerId = i_nPlayerId;
            foreach (TableHockeyGame m_game in m_round.TableHockeyGame)
            {
                m_game.AwayPlayerId = i_nPlayerId;
            }
        }

        protected void DropDownListHomePlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            getUCGUI();
            DropDownList m_ddlHomePlayer = (DropDownList)sender;
            int m_nSelectedHomePlayerId = Convert.ToInt32(m_ddlHomePlayer.SelectedValue);
            updateHomePlayer(m_nSelectedHomePlayerId);

            Session["ucEndGameSeries.m_vmround"] = m_vmRound;
            Session["ucEndGameSeries.m_round"] = m_round;
            Session["ucEndGameSeries.hasChanges"] = true;
            DataShow(m_vmRound);
        }

        protected void DropDownListAwayPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            getUCGUI();
            DropDownList m_ddlAwayPlayer = (DropDownList)sender;
            int m_nSelectedAwayPlayerId = Convert.ToInt32(m_ddlAwayPlayer.SelectedValue);
            updateAwayPlayer(m_nSelectedAwayPlayerId);

            Session["ucEndGameSeries.m_vmround"] = m_vmRound;
            Session["ucEndGameSeries.m_round"] = m_round;
            Session["ucEndGameSeries.hasChanges"] = true;
            DataShow(m_vmRound);
        }

        protected void ButtonRefresh_Click(object sender, EventArgs e)
        {
            getUCGUI();
            DataShow(m_vmRound);
        }

       
    }
}