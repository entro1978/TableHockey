<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="pgEditContestPlayers.aspx.cs" Inherits="TableHockey.pgEditContestPlayers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" runat="server">
    <div class="line">
        <div class="labelheader">
            <asp:Label ID="LabelContestHeader" runat="server" Text="Add players to contest"></asp:Label>
        </div>
    </div>
    <div class="line">
        <div class="label_long">
            <asp:Label ID="LabelContestDescription" runat="server"></asp:Label>
        </div>
    </div>
    <div class="line">
        <div class="labelheader">
            <asp:Label ID="LabelAvailablePlayersHeader" runat="server" Text="Available players"></asp:Label>
        </div>
    </div>
    <div class="line" style="overflow-y: scroll; overflow-x: hidden;height:250px;">
        <asp:GridView ID="GridViewAvailablePlayers" runat="server" AutoGenerateColumns="false" DataKeyNames="PlayerId">
            <Columns>
                <asp:BoundField DataField="PlayerId" HeaderText="Player ID" />
                <asp:BoundField DataField="FirstName" HeaderText="First name" />
                <asp:BoundField DataField="LastName" HeaderText="Last name" />
                <asp:BoundField DataField="ClubName" HeaderText="Club" />
                <asp:BoundField DataField="BirthDate" HeaderText="Birth date" />
                <asp:ImageField ItemStyle-Width="70px" DataImageUrlField="PlayerId" DataImageUrlFormatString="~\ImageHandler.ashx?id={0}&imagetype=player" />
                <asp:TemplateField>
                     <ItemTemplate>
                         <asp:CheckBox ID="checkSelectPlayer" runat="server"/>
                     </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div class="line">
        <div class="labelbold">
            <asp:Button ID="ButtonAddSelectedPlayersToContest" runat="server" 
                Text="Add selected" onclick="ButtonAddSelectedPlayersToContest_Click" />
        </div>
        <div style="float: left; margin-left: 20px; margin-top: 5px;" runat="server">
            <asp:Button ID="ButtonRemoveSelectedPlayersFromContest" runat="server" 
                Text="Remove selected" onclick="ButtonRemoveSelectedPlayersFromContest_Click" />
        </div>
    </div>
    <div class="line">
        <div class="labelheader">
            <asp:Label ID="LabelSelectedPlayers" runat="server" Text="Selected players"></asp:Label>
        </div>
    </div>
    <div class="line" style="overflow-y: scroll; overflow-x: hidden;height:250px;">
        <asp:GridView ID="GridViewSelectedPlayers" runat="server" AutoGenerateColumns="false" DataKeyNames="PlayerId">
            <Columns>
                <asp:BoundField DataField="PlayerId" HeaderText="Player ID" />
                <asp:BoundField DataField="FirstName" HeaderText="First name" />
                <asp:BoundField DataField="LastName" HeaderText="Last name" />
                <asp:BoundField DataField="ClubName" HeaderText="Club" />
                <asp:BoundField DataField="BirthDate" HeaderText="Birth date" />
                <asp:ImageField ItemStyle-Width="70px" DataImageUrlField="PlayerId" DataImageUrlFormatString="~\ImageHandler.ashx?id={0}&imagetype=player" />
                 <asp:TemplateField>
                     <ItemTemplate>
                         <asp:CheckBox ID="checkSelectPlayer" runat="server"/>
                     </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <br />
    <div class="line" id="divStartContest" runat="server">
        <asp:Button ID="ButtonStartContest" runat="server" Text="Start contest" 
            onclick="ButtonStartContest_Click" />
    </div>
</asp:Content>
