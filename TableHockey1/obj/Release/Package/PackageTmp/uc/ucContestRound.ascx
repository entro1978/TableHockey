<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucContestRound.ascx.cs" Inherits="TableHockey.uc.ucContestRound" %>
    <div>
        <asp:GridView ID="GridViewContestRoundGames" runat="server" AutoGenerateColumns="false" DataKeyNames="GameId">
            <Columns>
                <asp:BoundField DataField="GameId" HeaderText="Game ID" />
                <asp:BoundField DataField="HomePlayerDescription" HeaderText="Home player" />
                <asp:ImageField ItemStyle-Width="70px" DataImageUrlField="HomePlayerId" DataImageUrlFormatString="~\ImageHandler.ashx?id={0}&imagetype=player" />
                <asp:BoundField DataField="PlayerDivider" HeaderText="" />
                <asp:BoundField DataField="AwayPlayerDescription" HeaderText="Away player" />
                <asp:ImageField ItemStyle-Width="70px" DataImageUrlField="AwayPlayerId" DataImageUrlFormatString="~\ImageHandler.ashx?id={0}&imagetype=player" />                
                <asp:TemplateField>
                     <ItemTemplate>
                         <asp:TextBox ID="TextBoxHomePlayerScore" runat="server" Width="20px"></asp:TextBox>
                     </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ScoreDivider" HeaderText="" />
                <asp:TemplateField>
                     <ItemTemplate>
                         <asp:TextBox ID="TextBoxAwayPlayerScore" runat="server" Width="20px"></asp:TextBox>
                     </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div class="line">
        <div class="label_long">
            <asp:Label ID="LabelIdlePlayerDescription" runat="server" Text="[P] stands idle."></asp:Label>
        </div>
    </div>
    
