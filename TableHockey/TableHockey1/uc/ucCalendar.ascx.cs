using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace TableHockey.uc
{
    public partial class ucCalendar : System.Web.UI.UserControl
    {
        public string TextBoxToUse { get; set; }
        public bool DisableDatesBeforeToday { get; set; }
        public bool DisableDatesAfterToday { get; set; }
        public bool DisableWeekends { get; set; }

        protected void Calendar1_DayRender(object sender, DayRenderEventArgs e)
        {
            //-- disable weekends
            if (DisableWeekends)
            {
                if (e.Day.IsWeekend)
                    e.Day.IsSelectable = false;
            }

            //--disable days before current date
            if (DisableDatesBeforeToday)
            {
                if (e.Day.Date <= System.DateTime.Now.Date)
                    e.Day.IsSelectable = false;
            }

            if (DisableDatesAfterToday)
            {
                if (e.Day.Date > System.DateTime.Now.Date)
                    e.Day.IsSelectable = false;
            }

            //--show only days in current month
            if (e.Day.IsOtherMonth)
                e.Cell.Controls.RemoveAt(0);

            //--Kontrollera mot lista med helgdagar...jul osv...
            if (PageUtility.DatesToDisable().Contains(e.Day.Date))
            {
                e.Day.IsSelectable = false;
                e.Cell.ForeColor = System.Drawing.Color.FromArgb(227, 128, 29);
            }
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            if (DisableDatesBeforeToday)
            {
                if (Calendar1.SelectedDate <= DateTime.Now)
                    return;
            }
            TextBox tb = FindTextBox(this.Page.Controls);
            tb.Text = Calendar1.SelectedDate.ToShortDateString();
            plCalendar.Visible = false;

            Page.Validate();
        }

        //TextBox tb = (TextBox)this.Page.Master.FindControl("ContentPlaceholder1").FindControl("mainWizard").FindControl(TextBoxToUse);

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            if (plCalendar.Visible)
                plCalendar.Visible = false;
            else
                plCalendar.Visible = true;
        }

        private TextBox FindTextBox(ControlCollection controls)
        {
            TextBox tb;
            bool found = false;
            foreach (Control control in controls)
            {
                if ((control.ID != null) && (control.ID == TextBoxToUse))
                {
                    tb = (TextBox)control;
                    return tb;
                }
                if (control.HasControls() && !found)
                {
                    tb = FindTextBox(control.Controls);
                    if (tb != null)
                        return tb;
                }
            }
            return null;
        }
    }
}