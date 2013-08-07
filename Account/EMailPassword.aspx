<%@ Page Title="" Language="C#" MasterPageFile="../Site.Master" AutoEventWireup="true" CodeBehind="EMailPassword.aspx.cs" Inherits="TableHockey.Account.EMailPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" runat="server">
<asp:PasswordRecovery ID="PasswordRecovery1" runat="server" 
        onsendingmail="PasswordRecovery1_SendingMail">
    </asp:PasswordRecovery>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sideContent" runat="server">
</asp:Content>
