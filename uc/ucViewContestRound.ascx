<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucViewContestRound.ascx.cs" Inherits="TableHockey.uc.ucViewContestRound" %>
<div>
        <div style="font-size:larger;font-weight:bold;">
            <asp:Label ID="LabelRoundNumber" runat="server" Text="Round [N]"></asp:Label>  
        </div>
    </div>
    <div style="overflow-x: hidden;width:300px;">
        <asp:GridView ID="GridViewContestRoundGames" runat="server" AutoGenerateColumns="false" DataKeyNames="GameId">
            <Columns>
                <asp:BoundField DataField="GameId" HeaderText="Game ID" />
                <asp:BoundField DataField="HomePlayerDescription" HeaderText="Home player" />              
                <asp:BoundField DataField="PlayerDivider" HeaderText="" />
                <asp:BoundField DataField="AwayPlayerDescription" HeaderText="Away player" />
                <asp:BoundField DataField="HomePlayerScore" HeaderText="" />
                <asp:BoundField DataField="ScoreDivider" HeaderText="" />
                <asp:BoundField DataField="AwayPlayerScore" HeaderText="" />
            </Columns>
        </asp:GridView>
    </div>
    <div>
        <div>
            <asp:Label ID="LabelIdlePlayerDescription" runat="server" Text="[P] stands idle."></asp:Label>  
        </div>
    </div>
    &nbsp;
   

