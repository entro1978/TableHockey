<%@ Page Title="" Language="C#" MasterPageFile="../Site.Master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="TableHockey.Login" %>

<%@ Register TagPrefix="uc1" TagName="ucViewContests_public" Src="../uc/ucViewContests_public.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" runat="server">
    <div>
        <div style="float: left; margin-right: 10px;">
            <h2>Log In
            </h2>
            <p>
                Please enter your username and password.
        <asp:HyperLink ID="RegisterHyperLink" runat="server" EnableViewState="false">Register</asp:HyperLink>
                if you don't have an account.
            </p>
            <asp:Login ID="LoginUser" runat="server" EnableViewState="false" RenderOuterTable="false">
                <LayoutTemplate>
                    <span class="failureNotification">
                        <asp:Literal ID="FailureText" runat="server"></asp:Literal>
                    </span>
                    <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification"
                        ValidationGroup="LoginUserValidationGroup" />
                    <div class="accountInfo">
                        <fieldset class="login">
                            <legend>Account Information</legend>
                            <p>
                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Username:</asp:Label>
                                <asp:TextBox ID="UserName" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                    CssClass="failureNotification" ErrorMessage="User Name is required." ToolTip="User Name is required."
                                    ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                            </p>
                            <p>
                                <asp:HyperLink ID="EmailPasswordHyperLink" runat="server" NavigateUrl="~/Account/EmailPassword.aspx"
                                    CausesValidation="false">Forgot Password?</asp:HyperLink>
                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                    CssClass="failureNotification" ErrorMessage="Password is required." ToolTip="Password is required."
                                    ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                            </p>
                            <p>
                                <asp:CheckBox ID="RememberMe" runat="server" />
                                <asp:Label ID="RememberMeLabel" runat="server" AssociatedControlID="RememberMe" CssClass="inline">Keep me logged in</asp:Label>
                            </p>
                        </fieldset>
                        <p class="submitButton">
                            <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" ValidationGroup="LoginUserValidationGroup" />
                        </p>
                    </div>
                </LayoutTemplate>
            </asp:Login>
        </div>
        <div class="info" style="float: left; margin-left: 100px;">
            <uc1:ucViewContests_public ID="ucViewContests_public1" runat="server" />
        </div>
    </div>


    <div>
        <div style="float: left; margin-right: 3px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/TableHockey2.jpg" />
        </div>
        <div style="float: left; margin-right: 3px;">
            <asp:Image ID="Image2" runat="server" ImageUrl="~/images/TableHockey3.jpg" />
        </div>
    </div>
    <div class="line">
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sideContent" runat="server">
</asp:Content>
