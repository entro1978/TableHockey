<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="pgEditPlayer.aspx.cs" Inherits="TableHockey.pgEditPlayer" %>
<%@ Register TagPrefix="uc1" TagName="ucEditTableHockeyPlayer" Src="uc/ucEditPlayer.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" runat="server">
    <uc1:ucEditTableHockeyPlayer ID="ucEditTableHockeyPlayer1" runat="server" />
    <div class="line">
       <div style="float:left; margin-right:10px;">
            <asp:Button ID="ButtonSavePlayer" runat="server" Text="Save player" 
        onclick="ButtonSavePlayer_Click" />
       </div>
       <div style="float:left; margin-right:10px;">
           <asp:Button ID="ButtonDeletePlayer" runat="server" Text="Delete player" OnClick="ButtonDeletePlayer_Click" OnClientClick="return confirm('Do you really want to delete player?');" />
       </div>
    </div>
</asp:Content>

