<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucContestTable.ascx.cs" Inherits="TableHockey.uc.ucContestTable" %>
 <div style="height:350px;">
        <asp:GridView ID="GridViewContestTable" runat="server" AutoGenerateColumns="false" DataKeyNames="PlayerId" BackColor="Black" ForeColor="White" Font-Bold="true" Font-Size="X-Large" OnRowCreated="GridViewContestTable_RowCreated">
            <Columns>
                <asp:BoundField DataField="PlayerId" HeaderText="" Visible="false" />
                <asp:ImageField AccessibleHeaderText="Trend" ItemStyle-Width="20px" DataImageUrlField="TrendIndicatorType" DataImageUrlFormatString="~\ImageHandler.ashx?id={0}&imagetype=trendindicator" />
                <asp:BoundField DataField="PlayerStanding" HeaderText="" />
                <asp:BoundField DataField="PlayerDescription" HeaderText="Name" />
                <asp:BoundField DataField="NumberOfGamesPlayed" HeaderText="G" />
                <asp:BoundField DataField="NumberOfGamesWon" HeaderText="W" />
                <asp:BoundField DataField="NumberOfGamesTied" HeaderText="T" />
                <asp:BoundField DataField="NumberOfGamesLost" HeaderText="L" />
                <asp:BoundField DataField="NumberOfGoalsScored" HeaderText="GS" />
                <asp:BoundField DataField="Divider" HeaderText="" />
                <asp:BoundField DataField="NumberOfGoalsTendered" HeaderText="GT" />
                <asp:BoundField DataField="GoalDifference" HeaderText="GD" />
                <asp:BoundField DataField="NumberOfPoints" HeaderText="PTS" />
                <asp:TemplateField HeaderText="To End Game"> 
                    <ItemTemplate>    
                       <asp:CheckBox ID="CheckBoxSlutspel" runat="server" Visible='<%# (int)Eval("PlayerStanding") <= _numberOfPlayersToEndGame %>'/>            
                    </ItemTemplate> 
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>