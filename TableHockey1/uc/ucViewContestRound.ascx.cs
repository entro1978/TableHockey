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
    public partial class ucViewContestRound : System.Web.UI.UserControl
    {
        private TableHockeyContestRound m_round;
        private string m_sIdlePlayerDescription;
        private string m_sIdlePlayerDescriptionPlaceholder;
        private string m_sRoundDescription;
        private string m_sRoundDescriptionPlaceholder;
        public List<GameViewModel> m_games { get; set; }
        private RoundViewModel m_vmRound { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                m_sIdlePlayerDescriptionPlaceholder = "[P] stands idle."; //TODO:  Config setting!
                m_sRoundDescriptionPlaceholder = "Round [N]"; //TODO:  Config setting!
                Session["ucViewContestRound.IdlePlayerPlaceHolder"] = m_sIdlePlayerDescriptionPlaceholder;
                Session["ucViewContestRound.RoundPlaceHolder"] = m_sRoundDescriptionPlaceholder;
                if (m_sIdlePlayerDescription == null)
                    m_sIdlePlayerDescription = Convert.ToString(Session["ucViewContestRound.Round"]);
                if (m_sRoundDescription == null)
                    m_sRoundDescription = Convert.ToString(Session["ucViewContestRound.Round"]);

                if (m_vmRound.m_vmGames[0].IdlePlayerId != -1)
                {
                    this.LabelIdlePlayerDescription.Text = m_sIdlePlayerDescriptionPlaceholder.Replace("[P]", m_sIdlePlayerDescription);
                }
                else
                {
                    this.LabelIdlePlayerDescription.Text = "";
                } 

                this.LabelRoundNumber.Text = m_sRoundDescriptionPlaceholder.Replace("[N]", m_sRoundDescription);
            }
            else
            {

            }

        }

        public void InitControl(TableHockeyContestRound i_TableHockeyRound)
        {
            if (i_TableHockeyRound != null)
            {
                m_round = i_TableHockeyRound;
                Session["ucViewContestRound.m_round"] = m_round;
            }

            m_vmRound = new RoundViewModel(m_round);
            this.GridViewContestRoundGames.DataSource = m_vmRound.m_vmGames;
            this.GridViewContestRoundGames.DataBind();
            Session["ucViewContestRound.m_vmround"] = m_vmRound;
            m_games = m_vmRound.m_vmGames;
            setUCGUI();
        }

        private void setUCGUI()
        {
            //Update idle player string.
            if (m_vmRound.m_vmGames[0].IdlePlayerId != -1)
            {
                m_sIdlePlayerDescriptionPlaceholder = Convert.ToString(Session["ucViewContestRound.IdlePlayerPlaceHolder"]);
                this.m_sIdlePlayerDescription = m_vmRound.m_vmGames[0].IdlePlayerDescription;
                Session["ucViewContestRound.IdlePlayer"] = this.m_sIdlePlayerDescription;
                this.LabelIdlePlayerDescription.Text = m_sIdlePlayerDescriptionPlaceholder.Replace("[P]", m_sIdlePlayerDescription);
            }
            else
            {
                this.LabelIdlePlayerDescription.Text = "";
            }
            //Update round number string
            m_sRoundDescriptionPlaceholder = Convert.ToString(Session["ucViewContestRound.RoundPlaceHolder"]);
            this.m_sRoundDescription = Convert.ToString(m_vmRound.RoundNumber);
            Session["ucViewContestRound.Round"] = this.m_sRoundDescription;
            this.LabelRoundNumber.Text = m_sRoundDescriptionPlaceholder.Replace("[N]", m_sRoundDescription); 
        }
    }
}