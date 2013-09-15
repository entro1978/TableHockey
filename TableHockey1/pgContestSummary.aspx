<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="pgContestSummary.aspx.cs" Inherits="TableHockey.pgContestSummary" %>
<%@ Register TagPrefix="uc1" TagName="ucContestTable" Src="uc/ucContestTable.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" runat="server">
    
        <div>
            <div style="float: left; margin-right: 10px;" id="divExportTable" runat="server">
                <uc1:ucContestTable ID="ucContestTable1" runat="server" />
            </div>
            <div id="divUcContestRoundPlaceHolder" runat="server" style="margin-left: 75px; float: left; overflow-y: scroll; width: 320px; height: 300px;">
                <asp:Repeater ID="RoundRepeater" OnItemDataBound="RoundRepeater_ItemDataBound"
                    runat="server">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div>
                            <asp:PlaceHolder ID="PlaceHolder2" runat="server"></asp:PlaceHolder>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        <div id="divUcEndGamePlaceHolder" runat="server">
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
    </div>
    <div class="line">
        <div style="float:left;margin-right:30px;">
              <asp:Button ID="ButtonExportToPdf" runat="server" Text="Export to PDF" OnClick="ButtonExportToPdf_Click" />
        </div>
        <div style="float:left;">
            <asp:ImageButton ID="ImageButtonShare" runat="server" ImageUrl="~/images/share.jpg" OnClick="ImageButtonShare_Click"/>
        </div>
    </div>
     <div class="line">
         </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sideContent" runat="server">
</asp:Content>
