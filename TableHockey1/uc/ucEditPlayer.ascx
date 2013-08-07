<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEditPlayer.ascx.cs"
    Inherits="TableHockey.uc.ucEditPlayer" %>
<%@ Register TagPrefix="uc1" TagName="ucCalendar" Src="ucCalendar.ascx" %>
<div style="border-style: solid; border-width: 1px; border-color: White;">
    <div class="line">
        <div class="labelheader">
            <asp:Label ID="LabelPlayerHeader" runat="server" Text="Player details"></asp:Label>
        </div>
    </div>
    <div class="line">
        <div class="labelbold">
            <asp:Label ID="LabelPlayerIdDescription" runat="server" Text="Player Id"></asp:Label>
        </div>
        <div style="float: left; margin-top: 5px;">
            <asp:Label ID="LabelPlayerId" runat="server" Text="[New player]"></asp:Label>
        </div>
    </div>
    <div class="line">
        <div class="labelbold">
            <asp:Label ID="LblFirstName" runat="server" Text="First name"></asp:Label>
        </div>
        <div style="float: left; margin-top: 5px;">
            <asp:TextBox ID="TextBoxFirstName" runat="server"></asp:TextBox>
        </div>
        <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="TextBoxFirstName"
            ErrorMessage="Enter player first name.">*</asp:RequiredFieldValidator>
    </div>
    <div class="line">
        <div class="labelbold">
            <asp:Label ID="LabelLastName" runat="server" Text="Last name"></asp:Label>
        </div>
        <div style="float: left; margin-top: 5px;">
            <asp:TextBox ID="TextBoxLastName" runat="server"></asp:TextBox>
        </div>
        <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="TextBoxLastName"
            ErrorMessage="Enter player last name.">*</asp:RequiredFieldValidator>
    </div>
    <div class="line">
        <div class="labelbold">
            <asp:Label ID="LabelPlayerBirthDate" runat="server" Text="Player birth date"></asp:Label>
        </div>
        <div style="float: left; margin-top: 5px;">
            <asp:TextBox ID="TextBoxPlayerBirthDate" runat="server"></asp:TextBox>
            <uc1:ucCalendar ID="ucCalendarPlayerBirthDate" runat="server"></uc1:ucCalendar>
        </div>
        <asp:CustomValidator ID="cvPlayerBirthDate" runat="server" ControlToValidate="TextBoxPlayerBirthDate"
            ErrorMessage="Please enter a valid player birth date">*</asp:CustomValidator>
    </div>
    <div class="line">
        <div class="labelbold">
            <asp:Label ID="LabelPlayerClub" runat="server" Text="Representing club"></asp:Label>
        </div>
        <div style="float: left; margin-top: 5px;">
            <asp:DropDownList ID="DropDownListClubs" runat="server">
            </asp:DropDownList>
        </div>
    </div>
    <div class="line">
        <div class="labelbold">
            <asp:Label ID="LabelImage" runat="server" Text="Player image"></asp:Label>
        </div>
        <div style="float: left; margin-top: 5px;" id="divImage" runat="server" >
            <asp:Image runat="server" ID="ImagePlayer" />
        </div>
    </div>
    <div class="line">
        <div>
            <asp:FileUpload ID="FileUploadPlayerImage" runat="server" ToolTip="Välj bild" />
        </div>
    </div>
</div>
