using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TableHockeyData;
using AutoMapper;
using System.Web.Security;

namespace TableHockey
{
    public partial class pgEditPlayer : PageUtility
    {
        static pgEditPlayer()
        {
            Mapper.CreateMap<TableHockeyPlayer, TableHockeyPlayer>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Get Club details
                using (var clubcontext = new TableHockeyData.UHSSWEB_DEVEntities())
                {
                    var queryAllClubs = clubcontext.TableHockeyClub;
                    //Get Player details for existing Player or create new.  Init Player uc
                    int m_nPlayerId = -1;
                    if (!String.IsNullOrEmpty(Request.QueryString["PlayerId"]))
                    {
                        //Get existing Player and init uc.
                        m_nPlayerId = Convert.ToInt32(Request.QueryString["PlayerId"]);
                        MembershipUser user = Membership.GetUser(m_sUser.Trim());
                        if (user != null)
                        {
                            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
                            {
                                try
                                {
                                    var querySinglePlayer = (from c in context.TableHockeyPlayer
                                                             where c.RegisteredByUserId == (Guid)user.ProviderUserKey &&
                                                                   c.PlayerId == m_nPlayerId
                                                             select c).FirstOrDefault();
                                    if (querySinglePlayer != null)
                                    {
                                        this.ucEditTableHockeyPlayer1.InitControl((TableHockeyPlayer)querySinglePlayer, queryAllClubs.ToList());
                                        this.ButtonDeletePlayer.Visible = true;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Response.Redirect("~/pgEditPlayer.aspx");
                                }
                            }
                        }
                    }
                    else
                    {
                        //Init uc for new Player
                        this.ucEditTableHockeyPlayer1.InitControl(null, queryAllClubs.ToList());
                        this.ButtonDeletePlayer.Visible = false;
                    }
                }
            }
            else
            {

            }
        }

        protected void ButtonSavePlayer_Click(object sender, EventArgs e)
        {
            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
            {
                if (this.ucEditTableHockeyPlayer1.m_currentPlayer.PlayerId == -1)
                {
                    //New Player
                    TableHockeyPlayer m_newPlayer = this.ucEditTableHockeyPlayer1.m_currentPlayer;
                    m_newPlayer.RegisteredByUserId = (Guid)Membership.GetUser(m_sUser.Trim()).ProviderUserKey;
                    context.TableHockeyPlayer.Add(m_newPlayer);
                }
                else
                {
                    //Edit existing Player
                    TableHockeyPlayer m_currentPlayer = context.TableHockeyPlayer.FirstOrDefault(i => i.PlayerId == this.ucEditTableHockeyPlayer1.m_currentPlayer.PlayerId);
                    Mapper.Map(this.ucEditTableHockeyPlayer1.m_currentPlayer, m_currentPlayer);
                }
                context.SaveChanges();
                Response.Redirect("~/pgMain.aspx");
            }

        }

        protected void ButtonDeletePlayer_Click(object sender, EventArgs e)
        {
            if (this.ucEditTableHockeyPlayer1.m_currentPlayer.PlayerId != -1)
            {
                using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
                {

                    //Set existing Player to historic.
                    TableHockeyPlayer m_currentPlayerSource = this.ucEditTableHockeyPlayer1.m_currentPlayer;
                    m_currentPlayerSource.isHistoric = 1;
                    TableHockeyPlayer m_currentPlayerDest = context.TableHockeyPlayer.FirstOrDefault(i => i.PlayerId == this.ucEditTableHockeyPlayer1.m_currentPlayer.PlayerId);
                    Mapper.Map(m_currentPlayerSource, m_currentPlayerDest);
                    context.SaveChanges();
                }
                Response.Redirect("~/pgMain.aspx");
            }
        }
    }
}