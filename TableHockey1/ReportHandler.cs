using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TableHockey
{
    using System;
    using System.Data;
    using System.Configuration;
    using System.Linq;
    using System.Web;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using System.Xml.Linq;
    using Microsoft.Reporting.WebForms;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;


    namespace W3D3EServices.utils
    {
        public class ReportHandler
        {
            private ReportViewer m_viewer = new ReportViewer();
            public ReportViewer getset_m_viewer
            {
                get { return m_viewer; }
                set { m_viewer = value; }
            }

            public void renderReport(ReportViewer reportViewer, HttpResponse response)
            {
                Warning[] warnings = null;
                string[] streamids = null;
                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = string.Empty;
                byte[] bytes = null;


                try
                {
                    bytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);

                    response.Clear();
                    response.ClearHeaders();
                    response.ContentType = "application/pdf";
                    response.AddHeader("Content-Disposition", "Attachment; filename=Blankett." + extension);
                    response.BinaryWrite(bytes);

                }
                catch
                {
                    throw new Exception("Fel i RenderReport()"); ;
                }
                finally
                {
                    response.End();
                }

            }

        }
    }

}