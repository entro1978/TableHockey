<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Info.aspx.cs" Inherits="TableHockey.Account.Info" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" runat="server">
    <div class="info">
        <p>
        <b>This site is ment to work as a tournament tool for managing small table hockey tournaments.</b>
        <br />
        <br />
        Registration is required, although free as per 2013-08-15 (may be subject to change).
        <br />
        <br />
        <b>Code of conduct:</b>  &nbsp;You may add your own table hockey players and clubs with name and picture, but use common sense. 
        Inappropriate names and/or pictures WILL lead to a warning by email and deletion of content and/or account (repeated abuse).
        <br />
        <b>Disclaimer: &nbsp; In no event will the owners of this site be liable for unlawful or inappropriate events or content uploaded by any of its users!</b>  
        <br />
        <br />
        <b>Instructions</b>  &nbsp; You will start with an empty list of contests. To get started:
        <br />
        1. Add a new contest using the menu option you get once you are logged in.
        <br />
        <br />
        2. Add contest players that you require for the contest. Players added by other users will be available to you (but not editable).
        <br />If the new player is representing a club that is not in the list, you may add a new club using the "New club" menu option.
        <br />Players are added from the grid "Available players".  Check all that are to participate, and they appear in the grid "Selected players".
        <br />
        <br />
        3. Press the "Start contest" button. The tournament can now be edited one round at a time, and a table with current standings is displayed.
        <br />
        <br />
        4. When all rounds are played, press the "Save" button. If an end game contest is desired, check all players that will participate.
        <br /> In the text box "games per round", enter the number of rounds per end game.  Finally press "To end game". 
        <br />
        <br />
        5. A new, separate end game contest will be automatically created and you will be able to edit it round by round. Make sure to press the update button
        <br />after each entered result, to display the current game standing.
        <br />
        <br />
        6. All of "your" contests will be available in the "My contests" grid on the main page. They may be edited only as long as they are open 
        <br />(meaning closed date is not set). They may be viewed in summary at all times. Results can currently only be exported as a .pdf file.
        <br />Other formats and Facebook integration will be available in a near future.
        <br />
        <br />
        7. If you forget your password, there is a password recovery option that will re-generate a password which will be sent to the email address you provided on registration.     
        <br />
        <br />
        8. All contests that are open will be publicly visible on the start page of this site!
        <br />
        <br />
        <b>Contact:
        <br />    
               <br />
             uhsstablehockey(a)gmail.com
            <br />
         
            entro(a)kth.se</b>
        <br />
        <br />
        Last, but not least - have fun!  /Admin
        <b>

        </b> 
        </p>
    </div>
     <div>
        <div style="float:left;margin-right:3px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/TableHockey2.jpg" />
        </div>
        <div style="float:left;margin-right:3px;">
            <asp:Image ID="Image2" runat="server" ImageUrl="~/images/TableHockey3.jpg" />
        </div>
    </div>
     <div class="line">
         </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sideContent" runat="server">
</asp:Content>
