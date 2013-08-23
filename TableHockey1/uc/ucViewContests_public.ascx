<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucViewContests_public.ascx.cs" Inherits="TableHockey.uc.ucViewContests_public" %>
<asp:GridView ID="GridViewContestsExternal" runat="server" AutoGenerateColumns="false">
    <Columns>
        <asp:BoundField DataField="m_nContestId" HeaderText="ContestId" />
        <asp:BoundField DataField="m_sContestName" HeaderText="Contest name" />
        <asp:BoundField DataField="m_sContestDescription" HeaderText="Description" />
        <asp:BoundField DataField="m_sContestLocation" HeaderText="Location" />
        <asp:BoundField DataField="m_sContestStarted" HeaderText="Started on" DataFormatString="{0:d}" />
        <asp:BoundField DataField="m_sUser" HeaderText="Responsible user" />
        <asp:HyperLinkField DataNavigateUrlFields="m_nContestId" DataNavigateUrlFormatString="../pgContestSummary.aspx?ContestID={0}&AnonymousMode=true" Text="View/Export" />
    </Columns>
</asp:GridView>
