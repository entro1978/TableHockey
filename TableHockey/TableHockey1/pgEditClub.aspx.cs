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
    public partial class pgEditClub : PageUtility
    {
        static pgEditClub()
        {
            Mapper.CreateMap<TableHockeyClub, TableHockeyClub>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Get Club details
                using (var clubcontext = new TableHockeyData.UHSSWEB_DEVEntities())
                {
                    var queryAllClubs = clubcontext.TableHockeyClub;
                    //Get Club details for existing Club or create new.  Init Club uc
                    int m_nClubId = -1;
                    if (!String.IsNullOrEmpty(Request.QueryString["ClubId"]))
                    {
                        //Get existing Club and init uc.
                        m_nClubId = Convert.ToInt32(Request.QueryString["ClubId"]);
                        MembershipUser user = Membership.GetUser(m_sUser.Trim());
                        if (user != null)
                        {
                            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
                            {
                                try
                                {
                                    var querySingleClub = (from c in context.TableHockeyClub
                                                             where c.RegisteredByUserId == (Guid)user.ProviderUserKey &&
                                                                   c.ClubId == m_nClubId
                                                             select c).FirstOrDefault();
                                    if (querySingleClub != null)
                                    {
                                        this.ucEditClub1.InitControl((TableHockeyClub)querySingleClub);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Response.Redirect("~/pgEditClub.aspx");
                                }
                            }
                        }
                    }
                    else
                    {
                        //Init uc for new Club
                        this.ucEditClub1.InitControl(null);
                    }
                }
            }
            else
            {

            }
        }

        protected void ButtonSaveClub_Click(object sender, EventArgs e)
        {
            using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
            {
                if (this.ucEditClub1.m_currentClub.ClubId == -1)
                {
                    //New Club 
                    TableHockeyClub m_newClub = this.ucEditClub1.m_currentClub;
                    m_newClub.RegisteredByUserId = (Guid)Membership.GetUser(m_sUser.Trim()).ProviderUserKey;
                    context.TableHockeyClub.Add(m_newClub);
                }
                else
                {
                    //Edit existing Club
                    TableHockeyClub m_currentClub = context.TableHockeyClub.FirstOrDefault(i => i.ClubId == this.ucEditClub1.m_currentClub.ClubId);
                    Mapper.Map(this.ucEditClub1.m_currentClub, m_currentClub);
                }
                context.SaveChanges();
                Response.Redirect("~/pgMain.aspx");
            }

        }
    }
}