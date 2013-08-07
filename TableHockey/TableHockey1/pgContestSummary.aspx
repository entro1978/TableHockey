<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="pgContestSummary.aspx.cs" Inherits="TableHockey.pgContestSummary" %>
<%@ Register TagPrefix="uc1" TagName="ucContestTable" Src="uc/ucContestTable.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" runat="server">
   <uc1:ucContestTable ID="ucContestTable1" runat="server" />
    <div id="divUcEndGamePlaceHolder">
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
    <div class="line"></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sideContent" runat="server">
</asp:Content>
