<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEndGameSeries.ascx.cs"
    Inherits="TableHockey.uc.ucEndGameSeries" %>
<asp:UpdatePanel ID="UpdatePanel1" ChildrenAsTriggers="true" runat="server">
    <ContentTemplate>
        <div style="border-style: solid; border-width: medium; border-color: White">
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
                <asp:GridView ID="GridViewEndGames" runat="server" AutoGenerateColumns="false" DataKeyNames="GameId">
                    <Columns>
                        <asp:BoundField DataField="GameId" HeaderText="Game ID" />
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
                        <asp:TemplateField HeaderText="SD">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBoxSuddenDeath" runat="server" Checked='<%# (bool)Eval("hasSuddenDeath") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div style="float: right;">
                <asp:Button ID="ButtonRefresh" runat="server" Text="Refresh" OnClick="ButtonRefresh_Click" />
            </div>
            <div class="lineshort">
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
