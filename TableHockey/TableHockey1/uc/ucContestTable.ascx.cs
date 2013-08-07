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
    public partial class ucContestTable : System.Web.UI.UserControl
    {
        private bool _isFinalRound;
        private bool _hideTrendIndicator;
        public int _numberOfPlayersToEndGame { get; set; }
        public Dictionary<int,int> CheckedPlayersToEndGame { get { return PageUtility.GetCheckedPlayerIndexes(this.GridViewContestTable, "CheckBoxSlutspel"); } }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void GridViewContestTable_RowCreated(object sender, GridViewRowEventArgs e)
        {
            ((DataControlField)GridViewContestTable.Columns
                    .Cast<DataControlField>()
                    .Where(fld => fld.HeaderText == "To End Game")
                    .SingleOrDefault()).Visible = _isFinalRound;

           ((DataControlField)GridViewContestTable.Columns
                    .Cast<DataControlField>()
                    .Where(fld => fld.AccessibleHeaderText.Equals("Trend"))
                    .SingleOrDefault()).Visible = !_hideTrendIndicator;
        }

        private void setTrendIndicators(List<TableRowPlayerViewModel> i_previousTable, List<TableRowPlayerViewModel> i_currentTable)
        {
            foreach (TableRowPlayerViewModel m_tableRow in i_currentTable)
            {
                TableRowPlayerViewModel m_previousTableRow = i_previousTable.First(p => p.PlayerId == m_tableRow.PlayerId);
                if (m_tableRow.NumberOfGamesPlayed >= m_previousTableRow.NumberOfGamesPlayed)
                {
                    //Only show trend if traversing forwards in rounds.
                    if (m_tableRow.PlayerStanding == m_previousTableRow.PlayerStanding)
                    {
                        //Neutral trend indicator
                        m_tableRow.TrendIndicatorType = 0;
                    }
                    if (m_tableRow.PlayerStanding < m_previousTableRow.PlayerStanding)
                    {
                        //Advancing trend indicator
                        m_tableRow.TrendIndicatorType = 1;
                    }
                    if (m_tableRow.PlayerStanding > m_previousTableRow.PlayerStanding)
                    {
                        //Regressing trend indicator
                        m_tableRow.TrendIndicatorType = 2;
                    }
                }
                else
                {
                    m_tableRow.TrendIndicatorType = 0;
                }
            }
        }

        public void InitControl(List<TableRowPlayerViewModel> m_tableRows, bool isFinalRound = false, int numberOfPlayersToEndGame = 100, bool showTrend = true)
        {
            _isFinalRound = isFinalRound;
            _hideTrendIndicator = !showTrend;
            _numberOfPlayersToEndGame = numberOfPlayersToEndGame;
            if (Session["ucContestTable.previousTable"] != null)
            {
                var m_previousTable = (List<TableRowPlayerViewModel>)Session["ucContestTable.previousTable"];
                setTrendIndicators(m_previousTable, m_tableRows);
            }
            Session["ucContestTable.previousTable"] = m_tableRows;
            m_tableRows.Sort();
            int iCount = 1;
            foreach (TableRowPlayerViewModel m_tablevm in m_tableRows)
            {
                if (m_tablevm.PlayerId > 0)
                {
                    m_tablevm.PlayerStanding = iCount;
                    iCount++;
                }
            }
            var m_currentTable = m_tableRows.Where(p => p.PlayerId > 0);
            this.GridViewContestTable.DataSource = m_currentTable;
            this.GridViewContestTable.DataBind();
            
        }
    }
}