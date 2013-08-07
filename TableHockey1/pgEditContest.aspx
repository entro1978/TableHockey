<%@ Page Language="C#" MasterPageFile="~/Site.master" Title="Content Page"
    CodeBehind="~/pgEditContest.aspx.cs" Inherits="TableHockey.pgEditContest" %>
<%@ Register TagPrefix="uc1" TagName="ucEditTableHockeyContest" Src="uc/ucEditTableHockeyContest.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" runat="Server">
    <uc1:ucEditTableHockeyContest ID="ucEditTableHockeyContest1" runat="server" />
    <div class="line">
        <div class="labelbold">
            <asp:Button ID="ButtonSaveContest" runat="server" Text="Create/Update Contest" OnClick="ButtonSaveContest_Click" />
        </div>
        <div style="float: left; margin-left: 20px;margin-top: 5px;" id="divAddPlayers" runat="server">
            <asp:Button ID="ButtonAddPlayers" runat="server" Text="Add/Remove players" onclick="ButtonAddPlayers_Click" />
        </div>
    </div>
</asp:Content>
