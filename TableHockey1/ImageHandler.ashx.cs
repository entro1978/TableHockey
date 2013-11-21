using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TableHockey
{
    /// <summary>
    /// Summary description for ImageHandler
    /// </summary>
    public class ImageHandler : IHttpHandler
    {
        public bool IsReusable { get { return true; } }

        public void ProcessRequest(HttpContext ctx)
        {
            int id = Convert.ToInt32(ctx.Request.QueryString["id"]);
            string m_sImageType = ctx.Request.QueryString["imagetype"];
            if ((m_sImageType != null) && (m_sImageType.ToLower().Trim().Equals("player")))
            {
                using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
                {
                    var querySinglePlayer = context.TableHockeyPlayer.First(c => c.PlayerId == id);

                    byte[] m_picture = (byte[])querySinglePlayer.PlayerBinary;
                    if ((m_picture != null) && (m_picture.Length > 0))
                    {
                        ctx.Response.Clear();
                        ctx.Response.ContentType = "image/pjpeg";
                        ctx.Response.BinaryWrite(m_picture);
                        ctx.Response.End();
                    }
                    else
                    { 
                        //Make sure dummy picture is displayed instead.
                        var querySetting = context.Settings.First(s => s.SettingDescription == "BINARY_DUMMY_PLAYER_IMAGE");
                        m_picture = (byte[])querySetting.ValueBinary;
                        ctx.Response.Clear();
                        ctx.Response.ContentType = "image/pjpeg";
                        ctx.Response.BinaryWrite(m_picture);
                        ctx.Response.End();
                    }
                }
            }
            if ((m_sImageType != null) && (m_sImageType.ToLower().Trim().Equals("trendindicator")))
            {
                using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
                {
                    byte[] m_picture;
                    string m_sImageName = "BINARY_TREND_INDICATOR_NEUTRAL";
                    if (id == 1)
                        m_sImageName = "BINARY_TREND_INDICATOR_UP";
                    else if (id == 2)
                        m_sImageName = "BINARY_TREND_INDICATOR_DOWN";
                    var querySetting = context.Settings.First(s => s.SettingDescription == m_sImageName);
                    m_picture = (byte[])querySetting.ValueBinary;
                    if (m_picture != null)
                    {
                        ctx.Response.Clear();
                        ctx.Response.ContentType = "image/pjpeg";
                        ctx.Response.BinaryWrite(m_picture);
                        ctx.Response.End();
                    }
                }
            }
            if ((m_sImageType != null) && (m_sImageType.ToLower().Trim().Equals("club")))
            {
                using (var context = new TableHockeyData.UHSSWEB_DEVEntities())
                {
                    var querySingleClub = context.TableHockeyClubs.First(c => c.ClubId == id);

                    byte[] m_picture = (byte[])querySingleClub.ClubBinary;
                    if ((m_picture != null) && (m_picture.Length > 0))
                    {
                        ctx.Response.Clear();
                        ctx.Response.ContentType = "image/pjpeg";
                        ctx.Response.BinaryWrite(m_picture);
                        ctx.Response.End();
                    }
                    else
                    {
                        //Make sure dummy picture is displayed instead. TODO: Replace with dummy club image!
                        var querySetting = context.Settings.First(s => s.SettingDescription == "BINARY_DUMMY_PLAYER_IMAGE");
                        m_picture = (byte[])querySetting.ValueBinary;
                        ctx.Response.Clear();
                        ctx.Response.ContentType = "image/pjpeg";
                        ctx.Response.BinaryWrite(m_picture);
                        ctx.Response.End();
                    }
                }
            }
        }
    }
}