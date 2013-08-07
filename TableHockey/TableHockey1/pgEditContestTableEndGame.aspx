<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="pgEditContestTableEndGame.aspx.cs" Inherits="TableHockey.pgEditContestTableEndGame" %>

<%@ Register TagPrefix="uc1" TagName="ucEndGameSeries" Src="uc/ucEndGameSeries.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="line">
        <div class="labelheader">
            <asp:Label ID="LabelContestHeader" runat="server" Text="Contest round [ROUND]"></asp:Label>
        </div>
    </div>
    <div class="line">
        <div class="label_long">
            <asp:Label ID="LabelContestRoundDescription" runat="server"></asp:Label>
        </div>
    </div>
    <div id="divUcPlaceHolder">
        <div style="float: left;">
            <asp:Repeater ID="ResultsRepeater" OnItemDataBound="ResultsRepeater_ItemDataBound"
                runat="server">
                <HeaderTemplate>
                </HeaderTemplate>
                <ItemTemplate>
                    <div>
                        <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
    <div class="line">
        <div class="labelbold" id="divPreviousRound" runat="server">
            <asp:Button ID="ButtonPreviousRound" runat="server" Text="<< Previous round" OnClick="ButtonPreviousRound_Click" />
        </div>
        <div style="float: left; margin-left: 20px; margin-top: 5px;" id="divNextRound" runat="server">
            <asp:Button ID="ButtonNextRound" runat="server" Text="Next round >>" OnClick="ButtonNextRound_Click" />
        </div>
        <div style="float: left; margin-left: 20px; margin-top: 5px;" id="divSave" runat="server">
            <asp:Button ID="ButtonFinish" runat="server" Text="Save" OnClick="ButtonSave_Click" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sideContent" runat="server">
</asp:Content>
