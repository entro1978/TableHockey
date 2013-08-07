<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="pgEditClub.aspx.cs" Inherits="TableHockey.pgEditClub" %>
<%@ Register TagPrefix="uc1" TagName="ucEditClub" Src="uc/ucEditClub.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" runat="Server">
    <uc1:ucEditClub ID="ucEditClub1" runat="server" />
     <asp:Button ID="ButtonSaveClub" runat="server" Text="Save club" 
        onclick="ButtonSaveClub_Click" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sideContent" runat="server">
</asp:Content>
