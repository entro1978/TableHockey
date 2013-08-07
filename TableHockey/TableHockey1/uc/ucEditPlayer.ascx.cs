using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using TableHockeyData;
using TableHockey;

namespace TableHockey.uc
{
    public partial class ucEditPlayer : System.Web.UI.UserControl
    {

        private TableHockeyPlayer m_player;

        public TableHockeyPlayer m_currentPlayer
        {
            get
            {
                m_player = getUCGUI();
                return m_player;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ucCalendarPlayerBirthDate.TextBoxToUse = TextBoxPlayerBirthDate.ID;
            this.ucCalendarPlayerBirthDate.DisableDatesBeforeToday = false;
            this.ucCalendarPlayerBirthDate.DisableWeekends = false;
    
            //byte[] m_Image = (byte[])dr["picture"]; 
            //MemoryStream ms = new MemoryStream(m_Image); 
            //ImagePlayer.Image = Image.FromStream(ms);
        }

        private void InitPlayerImage(TableHockeyPlayer i_player)
        {
            if ((i_player.PlayerId > 0) && (i_player.PlayerBinary != null))
            {
                divImage.Visible = true;
                ImagePlayer.ImageUrl = "~/ImageHandler.ashx?id=" + i_player.PlayerId + "&imagetype=player";
            }
            else
            {
                divImage.Visible = false;
            }
        }

        public void InitControl(TableHockeyPlayer i_TableHockeyPlayer, List<TableHockeyClub> i_allClubs)
        {
            if (i_TableHockeyPlayer != null)
            {
                //Edit existing player
                m_player = i_TableHockeyPlayer;
            }
            else
            {
                //Create new player
                m_player = new TableHockeyPlayer();
                m_player.PlayerId = -1;
            }

            ClubViewModelList m_clubList = new ClubViewModelList(i_allClubs);
            DropDownListClubs.DataSource = m_clubList.m_ClubVmList;
            DropDownListClubs.DataValueField = "ClubId";
            DropDownListClubs.DataTextField = "ClubDescription";
            DropDownListClubs.DataBind();
            InitPlayerImage(m_player);
            setUCGUI();
            Session["ucEditPlayer.m_player"] = m_player;
        }

        private TableHockeyPlayer getUCGUI()
        {
            if (Session["ucEditPlayer.m_player"] != null)
            {
                m_player = (TableHockeyPlayer)Session["ucEditPlayer.m_player"];      
            }
            TableHockeyPlayer m_GUIPlayer = new TableHockeyPlayer();
            if (m_player == null || m_player.PlayerId <= 0)
            {
                m_GUIPlayer.PlayerId = -1;
            }
            else
            {
                m_GUIPlayer.PlayerId = m_player.PlayerId;
                m_GUIPlayer.RegisteredByUserId = m_player.RegisteredByUserId;
            }
            m_GUIPlayer.FirstName = TextBoxFirstName.Text;
            m_GUIPlayer.LastName = TextBoxLastName.Text;
            m_GUIPlayer.ClubId = Convert.ToInt32(DropDownListClubs.SelectedValue);
            m_GUIPlayer.BirthDate = Convert.ToDateTime(TextBoxPlayerBirthDate.Text);
            if (FileUploadPlayerImage.FileBytes.Length > 0)
            {
                m_GUIPlayer.PlayerBinary = PageUtility.DownscaleImageToWidth(FileUploadPlayerImage.FileBytes, 43);  //TODO: Setting!
            } else
            {
                m_GUIPlayer.PlayerBinary = m_player.PlayerBinary;
            }
            
            Session["ucEditPlayer.m_player"] = m_GUIPlayer;
            return m_GUIPlayer;
        }

        private void setUCGUI()
        {
            DropDownListClubs.SelectedValue = "0";
            TextBoxFirstName.Text = "";
            TextBoxLastName.Text = "";
            if (m_player != null)
            {
                //Set DDL selected club if existing player.
                if (m_player.ClubId > 0)
                {
                    DropDownListClubs.SelectedValue = Convert.ToString(m_player.ClubId);
                }
                if (m_player.PlayerId > 0)
                {
                    LabelPlayerId.Text = Convert.ToString(m_player.PlayerId);
                }

                //Set player details if existing player.
                if (!String.IsNullOrEmpty(m_player.FirstName))
                {
                    TextBoxFirstName.Text = m_player.FirstName;
                }
                if (!String.IsNullOrEmpty(m_player.LastName))
                {
                    TextBoxLastName.Text = m_player.LastName;
                }
                if (m_player.BirthDate != null)
                {
                    TextBoxPlayerBirthDate.Text = Convert.ToDateTime(m_player.BirthDate).ToString("yyyy-MM-dd");  
                }
            }
            
            
        }
    }
}