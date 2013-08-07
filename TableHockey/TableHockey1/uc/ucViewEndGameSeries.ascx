<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucViewEndGameSeries.ascx.cs"
    Inherits="TableHockey.uc.ucViewEndGameSeries" %>
<div class="line">
    <div id="divFiller" style="float: left;">
        <asp:Label id="lblFiller" runat="server" Text=""></asp:Label>
    </div>
    <div style="float: left; border-style: solid; border-width: medium; border-color: White">
        <div style="font-weight: bold; font-size: larger;">
            <asp:GridView ID="GridViewEndGameOverview" runat="server" AutoGenerateColumns="false"
                DataKeyNames="GameId">
                <Columns>
                    <asp:BoundField DataField="HomePlayerDescription" HeaderText="" />
                    <asp:ImageField ItemStyle-Width="70px" DataImageUrlField="HomePlayerId" DataImageUrlFormatString="~\ImageHandler.ashx?id={0}&imagetype=player" />
                    <asp:BoundField DataField="PlayerDivider" HeaderText="" />
                    <asp:BoundField DataField="AwayPlayerDescription" HeaderText="" />
                    <asp:ImageField ItemStyle-Width="70px" DataImageUrlField="AwayPlayerId" DataImageUrlFormatString="~\ImageHandler.ashx?id={0}&imagetype=player" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="HomePlayerGamesWon" runat="server" Width="40px"><%# Eval("HomePlayerScore") %></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ScoreDivider" HeaderText="" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="AwayPlayerGamesWon" runat="server" Width="40px"><%# Eval("AwayPlayerScore") %></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div style="float: left;">
            <asp:Label ID="lblGameResults" runat="server"></asp:Label>
        </div>
        <div class="lineshort">
        </div>
    </div>
</div>
