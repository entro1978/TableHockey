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
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net.Mail;

namespace TableHockey
{
    public class PageUtility : System.Web.UI.Page
    {
        public const int m_nMaxNumberOfConcurrentGames = 8;
        protected string _m_sUser;
        public string m_sUser 
        {
            get {
                if (Session["CurrentUser"] != null)
                {
                    _m_sUser = (string)Session["CurrentUser"];
                }
                 return _m_sUser; 
            }
            set { _m_sUser = value; }
        }

        public PageUtility()
        {
            this.Load += new EventHandler(this.Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["CurrentUser"] = Page.User.Identity.Name;
            if (String.IsNullOrEmpty(m_sUser) || !Page.User.Identity.IsAuthenticated)
                Response.Redirect("~/Account/Login.aspx");
        }

        # region Email

        //protected string m_sUserSessionName = "A0A1AF_&%";

        //protected string getLoggedInUserKey()
        //{
        //    return (Session[m_sUserSessionName] != null) ? m_sUserSessionName.Replace(m_sUserSessionName, "") : "";
        //}

        public static void sendGMailMsgFromSite(string i_sSubject, string i_sBody, string i_sEmailTo, string i_sNameTo)
        {
            var fromAddress = new MailAddress("uhsstablehockey@gmail.com", "UHSS Table Hockey");
            var toAddress = new MailAddress(i_sEmailTo, i_sNameTo);
            const string fromPassword = "Vict0r2007";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = i_sSubject,
                Body = i_sBody
            })
            {
                smtp.Send(message);
            }
        }
            

        # endregion

        #region CalcUtility

        public static bool isOdd(int number)
        {
            return ((number & 1) != 0) ? true : false;
        }

        public static int highestExponentLessThanOrEqualToSum(int i_nBase, int i_nSum)
        {
            double m_nOut = 1;
            double m_nSum = 0;

            while (m_nSum <= i_nSum)
            {
                m_nSum = Math.Pow(i_nBase,m_nOut);
                m_nOut += 1.0;
            }
            return (int)m_nOut;
        }

        #endregion

        #region fileUtility

        public string getFileHashValueForSelectedFile(byte[] i_fileByteArray)
        {
            byte[] m_fileByteArray;
            System.Security.Cryptography.SHA256Managed MySHA = new System.Security.Cryptography.SHA256Managed();
            m_fileByteArray = MySHA.ComputeHash(i_fileByteArray);
            string hash = "";
            int iCount = 0;
            for (iCount = 0; (iCount <= (m_fileByteArray.Length - 1)); iCount++)
            {
                hash = (hash + m_fileByteArray[iCount].ToString());
            }
            return hash;
        }

        public static Byte[] getBinaryFromURL(string i_sURL)
        {
            WebClient wc = new WebClient();
            byte[] m_curData = wc.DownloadData(i_sURL);
            return m_curData;
        }

        #endregion

        #region dataUtility

        public static string getStringFromXMLFile(string i_sFilename)
        {
            XmlDocument oXmlDoc = new XmlDocument();
            oXmlDoc.Load(i_sFilename);
            if (oXmlDoc != null)
            {
                StringWriter sw = new System.IO.StringWriter();
                XmlTextWriter xw = new XmlTextWriter(sw);
                oXmlDoc.WriteTo(xw);
                return sw.ToString();
            }
            else
                return "";
        }

        public DataSet getDataSetFromXMLString(string i_str)
        {
            StringReader XMLreader = new System.IO.StringReader(i_str);
            DataSet newDS = new DataSet();
            newDS.ReadXml(XMLreader);
            return newDS;
        }

        public DataSet getDataSetFromXMLFile(string i_sFilename)
        {
            return getDataSetFromXMLString(getStringFromXMLFile(i_sFilename));
        }

        public static DataTable getDataTableFromRowArray(DataTable i_dtSourceDataTable, DataRow[] i_dr)
        {
            DataTable dtOut = i_dtSourceDataTable.Clone();
            foreach (DataRow dr in i_dr)
            {
                dtOut.ImportRow(dr);
            }
            return dtOut;
        }

        public static string getXMLValue(string i_sKey, string i_sErrandType, System.Web.HttpServerUtility i_server)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(i_server.MapPath("~\\xml\\Config_" + i_sErrandType + ".xml"));
            XmlNodeList m_nodeList = doc.SelectNodes("/config/" + i_sKey + "_" + i_sErrandType);

            if ((m_nodeList != null) && (m_nodeList.Count > 0))
                return Convert.ToString(m_nodeList[0].Attributes["value"].Value);
            else
                return "";
        }

        public static string getXMLRegionalValue(string i_sKey, string i_sErrandType, string i_sRegionalShortName, System.Web.HttpServerUtility i_server)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(i_server.MapPath("~\\xml\\Config_" + i_sErrandType + ".xml"));
            XmlNodeList m_nodeList = doc.SelectNodes("/config/" + i_sKey + "_" + i_sErrandType + "_" + i_sRegionalShortName);

            if ((m_nodeList != null) && (m_nodeList.Count > 0))
                return Convert.ToString(m_nodeList[0].Attributes["value"].Value);
            else
                return "";
        }


        #endregion

        #region UIUtility

        public static void populateDDLFromGenericList(ref DropDownList i_ddl, List<Object> i_lst, string i_sNotSelected)
        {
            if ((i_sNotSelected != null) && (i_sNotSelected.Trim() != ""))
            {
                i_ddl.Items.Add(new ListItem(i_sNotSelected, "0"));
            }
            foreach (Object obj in i_lst)
            {
                i_ddl.Items.Add(new ListItem(obj.ToString(), Convert.ToString(obj.GetHashCode())));
            }
        }

        public static string WrappableText(string source)
        {
            string nwln = Environment.NewLine;
            return "<p>" +
            source.Replace(nwln + nwln, "</p><p>").Replace(nwln, "<br />") + "</p>";
        }

        public void restrictTextAreaLength(ref HtmlTextArea i_txtArea, int i_nMaxLength)
        {

            string m_sLengthFunction = "function isMaxLength(txtBox) {";
            m_sLengthFunction += " if(txtBox) { ";
            m_sLengthFunction += "     return ( txtBox.value.length <=" + i_nMaxLength + ");";
            m_sLengthFunction += " }";
            m_sLengthFunction += "}";

            i_txtArea.Attributes.Add("onkeypress", "return isMaxLength(this);");
            ClientScript.RegisterClientScriptBlock(
                this.GetType(),
                "txtLength",
                m_sLengthFunction, true);
        }

        public static void clearCache(Page p)
        {
            p.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            p.Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
        }

        public static void killSession(Page p)
        {
            p.Session["ucPosition.EntPostitionList"] = null;
            p.Session["ucContactDetails_selectedEMailAddress"] = null;
            p.Session["ucContactDetails_selectedEMailAddressConfirm"] = null;
            p.Session["ucContactDetails_selectedPhoneNo"] = null;
            p.Session["ucContactDetails_selectedAddress"] = null;
            p.Session["ucPosition.selectedEntLaen"] = null;
            p.Session["plats"] = null;
        }

        public static Control IterateThroughChildren(Control parent, string ControlToFind)
        {
            foreach (Control c in parent.Controls)
            {
                if (c.ID != null)
                {
                    if (c.ID == ControlToFind)
                        return c;
                }
                if (c.Controls.Count > 0)
                {
                    Control ct = IterateThroughChildren(c, ControlToFind);
                    if (ct != null)
                        return ct;
                }
            }

            return null;
        }

        public static DateTime RetrieveLinkerTimestamp()
        {
            string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            byte[] b = new byte[2048];
            System.IO.Stream s = null;

            try
            {
                s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            finally
            {
                if (s != null)
                {
                    s.Close();
                }
            }

            int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
            int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
            return dt;
        }

        public static void addControlHyperLink(Control i_c, string i_sLinkText, string i_sLinkURL, bool i_bNewWindow, string i_sHyperLinkID)
        {
            HyperLink l = new HyperLink();
            l.Text = i_sLinkText;
            l.ID = i_sHyperLinkID;
            l.NavigateUrl = i_sLinkURL;
            if (i_bNewWindow)
                l.Target = "_blank";
            i_c.Controls.Add(l);
        }

        public static void updateControlHyperLink(Control i_c, string i_sLinkText, string i_sLinkURL, bool i_bNewWindow, string i_sHyperLinkIDToUpdate)
        {
            HyperLink l = (HyperLink)i_c.FindControl(i_sHyperLinkIDToUpdate);
            if (l != null)
            {
                l.Text = i_sLinkText;
                l.NavigateUrl = i_sLinkURL;
                if (i_bNewWindow)
                    l.Target = "_blank";
            }
        }

        public static string trimToMaxLength(string i_sStr, int i_nMaxLength)
        {
            if ((i_sStr.Length > i_nMaxLength))
            {
                return i_sStr.Substring(0, i_nMaxLength);
            }
            else
            {
                return i_sStr;
            }
        }

        public static List<DateTime> DatesToDisable()
        {
            List<DateTime> lst = new List<DateTime>();
            lst.Add(new DateTime(2009, 12, 24));
            lst.Add(new DateTime(2010, 12, 24));
            lst.Add(new DateTime(2011, 12, 24));
            lst.Add(new DateTime(2012, 12, 24));
            lst.Add(new DateTime(2013, 12, 24));
            lst.Add(new DateTime(2014, 12, 24));
            lst.Add(new DateTime(2015, 12, 24));
            lst.Add(new DateTime(2016, 12, 24));
            lst.Add(new DateTime(2017, 12, 24));
            lst.Add(new DateTime(2018, 12, 24));
            lst.Add(new DateTime(2019, 12, 24));
            lst.Add(new DateTime(2020, 12, 24));
            lst.Add(new DateTime(2021, 12, 24));
            lst.Add(new DateTime(2022, 12, 24));
            lst.Add(new DateTime(2023, 12, 24));
            lst.Add(new DateTime(2024, 12, 24));
            lst.Add(new DateTime(2025, 12, 24));

            return lst;
        }

        /// <summary>
        /// Hämtar alla controller med innehåll inom en annan control. Tar inte med Buttons eller LinkButtons.
        /// </summary>
        /// <param name="c">parent control</param>
        /// <returns>sträng med html</returns>
        /// 
        public static string GetSummary(Control c)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter textWriter = new StringWriter(sb);
            HtmlTextWriter writer = new HtmlTextWriter(textWriter);

            if (c.HasControls())
            {
                foreach (Control ctrl in c.Controls)
                {
                    ctrl.RenderControl(writer);
                }
            }

            return sb.ToString();
        }

        //För att kunna rendera sidan som HTML utan problem.
        public override void VerifyRenderingInServerForm(Control control)
        { /* Do nothing */ }

        public string buildHtmlStyles()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<style>");
            sb.Append(@"body{text-align:left;font-size:0.75em;font-family:Arial, Helvetica, sans-serif;line-height: 1.5em;color:#000;margin: 0px;padding: 0px;}
                        hr{height: 1px;background-color:#CFCFCF;color:#CFCFCF;border:0;}
                        textarea{padding: 1px 1px 1px 1px;width:99%;}
                        div.clear{clear:both;}
                        div.line{clear: both;display: block;margin-bottom: 5px;MARGIN-LEFT: 3px;WIDTH: 380px;PADDING-TOP: 5px}
                        div.input{FLOAT: left;	WIDTH: 225px;TEXT-ALIGN: right;}
                        div.label{ padding: 0px 0px 0px 0px;margin-top: 2px;float: left;width: 150px}
                        div.labelbold{float: left;width: 13em;text-align: left;padding: 4px 4px 4px 0px;margin: 2px;font-weight:bold;}
                        div.category{font-weight:bold;float:left;width:100%;background-color:#E9EDF9;}
                        div.categoryboldonly{font-weight:bold;float:left;width:100%;}");

            sb.Append("</style>");

            return sb.ToString();


        }

        public static Dictionary<int,int> GetCheckedPlayerIndexes(GridView i_gv, string i_sChbColumnName)
        {
            //First column: Row number.
            //Second column: PlayerID.

            Dictionary<int, int> checkedIDs = new Dictionary<int, int>();
            foreach (GridViewRow playerRow in i_gv.Rows)
            {
                CheckBox chk = (CheckBox)playerRow.FindControl(i_sChbColumnName);
                if ((chk!= null) && (chk.Checked))
                {
                    //we want the GridViewRow's DataKey value    
                    checkedIDs[playerRow.RowIndex] = int.Parse(i_gv.DataKeys[playerRow.RowIndex].Value.ToString());
                }
            }
            return checkedIDs;
        }

        #endregion

        #region images

        public static Bitmap BytesToBitmap(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {

                Bitmap img = (Bitmap)System.Drawing.Image.FromStream(ms);
                return img;
            }
        }

        public static byte[] DownscaleImageToWidth(byte[] src, int w)
        {
            System.Drawing.Image imgTmp = BytesToBitmap(src);
            double sf;
            Bitmap imgFoto;
            if (imgTmp.Width > w)
            {
                sf = (imgTmp.Width / w);
                imgFoto = new System.Drawing.Bitmap(w, Convert.ToInt32(imgTmp.Height / sf));
                Rectangle recDest = new Rectangle(0, 0, w, imgFoto.Height);
                Graphics gphCrop = Graphics.FromImage(imgFoto);
                gphCrop.SmoothingMode = SmoothingMode.HighQuality;
                gphCrop.CompositingQuality = CompositingQuality.HighQuality;
                gphCrop.InterpolationMode = InterpolationMode.High;
                gphCrop.DrawImage(imgTmp, recDest, 0, 0, imgTmp.Width, imgTmp.Height, GraphicsUnit.Pixel);
            }
            else
            {
                imgFoto = (Bitmap)imgTmp;
            }
            // Dim myImageCodecInfo As System.Drawing.Imaging.ImageCodecInfo
            System.Drawing.Imaging.Encoder myEncoder;
            System.Drawing.Imaging.EncoderParameter myEncoderParameter;
            System.Drawing.Imaging.EncoderParameters myEncoderParameters;
            System.Drawing.Imaging.ImageCodecInfo[] arrayICI = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            System.Drawing.Imaging.ImageCodecInfo jpegICI = null;
            int x = 0;
            for (x = 0; (x <= (arrayICI.Length - 1)); x++)
            {
                if (arrayICI[x].FormatDescription.Equals("BMP"))
                {
                    jpegICI = arrayICI[x];
                    break;
                }
            }
            myEncoder = System.Drawing.Imaging.Encoder.Quality;
            myEncoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
            myEncoderParameter = new System.Drawing.Imaging.EncoderParameter(myEncoder, w);
            myEncoderParameters.Param[0] = myEncoderParameter;
            using (MemoryStream ms = new MemoryStream())
            {
                imgFoto.Save(ms, jpegICI, myEncoderParameters);
                imgTmp.Dispose();
                imgFoto.Dispose();
                return ms.ToArray();
            }
        }

        #endregion

    }


}
