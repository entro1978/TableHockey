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
    public partial class ucFileUpload : System.Web.UI.UserControl
    {
        public const string m_sSessionFileName = "plats";
        private int m_nMaxNumberOfFiles;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.lblAttachmentInfo.Text = this.lblAttachmentInfo.Text.Replace("[ANT]", Convert.ToString(m_nMaxNumberOfFiles));
            this.lstPlaceFiles.Rows = m_nMaxNumberOfFiles;
            this.lstPlaceFiles.Enabled = (this.lstPlaceFiles.Rows > 1);
        }

        public int maxNumberOfFiles
        {
            get { return m_nMaxNumberOfFiles; }
            set { this.m_nMaxNumberOfFiles = value; }
        }

        public bool cvFileUploadIsValid
        {
            get { return this.cvFileUpload.IsValid; }
            set { this.cvFileUpload.IsValid = value; }
        }

        public string getSessionFileName
        {
            get { return m_sSessionFileName; }
           
        }

        public ListBox getLstPlaceFiles
        {
            get { return this.lstPlaceFiles; }
        }

        public string getAllAddedItemsAsString
        { 
            get {string m_sFiles = "";
                 
                 ListItem curLi;
                 for(int iCount = 0; iCount < lstPlaceFiles.Items.Count; iCount++) 
                 {
                    curLi = lstPlaceFiles.Items[iCount];
                    if (iCount < lstPlaceFiles.Items.Count - 1)
                        m_sFiles += curLi.Text + ", <br/>";
                     else
                         m_sFiles += curLi.Text;
                 }
                 return m_sFiles;
             }
        }

        protected void btnAddPlaceFile_Click(object sender, EventArgs e)
        {

            lblPlaceErr.Visible = false;
            string strMessage = string.Empty;
            if (filePlace.HasFile)
            {
                //Automatically replace file if only one allowed.
                if ((lstPlaceFiles.Items.Count == 1) && (m_nMaxNumberOfFiles == 1))
                    lstPlaceFiles.ClearSelection();

                if (lstPlaceFiles.Items.Count < m_nMaxNumberOfFiles)
                {
                    //--check filesize
                    if (filePlace.PostedFile.ContentLength > 1000000)
                    {
                        lblPlaceErr.Visible = true;
                        lblPlaceErr.Text = " Filen är för stor.";
                    }
                }
            }
        }

        protected void btnRemovePlaceFile_Click(object sender, EventArgs e)
        {
            lblPlaceErr.Visible = false;
            if (lstPlaceFiles.SelectedIndex != -1)
            {
                lstPlaceFiles.ClearSelection();
            }
        }
    }
}