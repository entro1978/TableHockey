<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="pgEditContestTable.aspx.cs" Inherits="TableHockey.pgEditContestTable" %>

<%@ Register TagPrefix="uc1" TagName="ucContestRound" Src="uc/ucContestRound.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ucContestTable" Src="uc/ucContestTable.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ucHandleSong" Src="uc/ucHandleSong.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" runat="server">
 <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="False"
            ShowSummary="True" ValidationGroup="ContestTable" />
    <div class="lineshort">
        <div class="labelheader">
            <asp:Label ID="LabelContestHeader" runat="server" Text="Contest round [ROUND]"></asp:Label>
        </div>
    </div>
    <div class="lineshort">
        <div class="label_long">
            <asp:Label ID="LabelContestRoundDescription" runat="server"></asp:Label>
        </div>
    </div>
    <div>
        <div style="float: left;width:40%;">
            <uc1:ucContestRound ID="ucContestRound1" runat="server" />
        </div>
        <div style="float: left; margin-left: 50px; margin-right: 50px;margin-top: 5px;" id="div1" runat="server">
            <uc1:ucContestTable ID="ucContestTable1" runat="server" />
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
        <div style="float: left; margin-left: 20px; margin-top: 5px;" id="divEndGame" runat="server">
            <asp:Button ID="ButtonEndGame" runat="server" Text="To end game" OnClick="ButtonEndGame_Click"
                OnClientClick="return confirm('Move on to end game?');" />
            <asp:CustomValidator ID="cvEndGamePlayers" runat="server" ControlToValidate="" ErrorMessage="Please select at least 2 players for end game." ValidationGroup="ContestTable">*</asp:CustomValidator>
            <div class="line">
                <div class="labelbold">
                    <asp:Label ID="LabelEndGameRounds" runat="server" Text="Games per round"></asp:Label>
                </div>
                <div style="float: left; margin-top: 5px;">
                    <asp:TextBox ID="TextBoxEndGameRounds" runat="server"></asp:TextBox>
                </div>
                <asp:CustomValidator ID="cvEndGameRounds" runat="server" ControlToValidate="TextBoxEndGameRounds" ErrorMessage="Enter number of end game rounds." ValidationGroup="ContestTable">*</asp:CustomValidator>
            </div>
        </div>
    </div>
</asp:Content>
