<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEditTableHockeyContest.ascx.cs"
    Inherits="TableHockey.uc.ucEditTableHockeyContest" %>
<%@ Register TagPrefix="uc1" TagName="ucCalendar" Src="ucCalendar.ascx" %>
<div style="border-style:solid;border-width:1px;border-color:White;">
    <div class="line">
        <div class="labelheader">
            <asp:Label ID="LabelContestHeader" runat="server" Text="Contest details"></asp:Label>
        </div>
    </div>
    <div class="line">
        <div class="labelbold">
            <asp:Label ID="LabelContestIdDescription" runat="server" Text="Contest Id"></asp:Label>
        </div>  
        <div style="float: left; margin-top: 5px;">
             <asp:Label ID="LabelContestId" runat="server" Text="[New contest]"></asp:Label>
        </div>
    </div>
    <div class="line">
        <div class="labelbold">
            <asp:Label ID="LblContestName" runat="server" Text="Name of contest"></asp:Label>
        </div>
        <div style="float: left; margin-top: 5px;">
            <asp:TextBox ID="TextBoxContestName" runat="server"></asp:TextBox>
        </div>
        <asp:RequiredFieldValidator ID="rfvContestName" runat="server" ControlToValidate="TextBoxContestName" ErrorMessage="Enter name of contest.">*</asp:RequiredFieldValidator>
    </div>
    <div class="line">
        <div class="labelbold">
          <asp:Label ID="LabelContestStartedDate" runat="server" Text="Contest started on"></asp:Label>
        </div>
        <div style="float: left; margin-top: 5px;">
            <asp:TextBox ID="TextBoxContestStartedDate" runat="server" ReadOnly="true"></asp:TextBox>
            <uc1:ucCalendar ID="ucCalendarStarted" runat="server"></uc1:ucCalendar>
        </div>
        <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="TextBoxContestClosedDate" ErrorMessage="Please enter a valid contest opening date">*</asp:CustomValidator>
     </div>
     <div class="line">
        <div class="labelbold">
          <asp:Label ID="LabelContestClosedDate" runat="server" Text="Contest ended on"></asp:Label>
        </div>
        <div style="float: left; margin-top: 5px;">
            <asp:TextBox ID="TextBoxContestClosedDate" runat="server" ReadOnly="true"></asp:TextBox>
            <uc1:ucCalendar ID="ucCalendarClosed" runat="server"></uc1:ucCalendar>
        </div>
        <asp:CustomValidator ID="cvClosedDate" runat="server" ControlToValidate="TextBoxContestClosedDate" ErrorMessage="Please enter a valid contest closing date">*</asp:CustomValidator>
     </div>
     <div class="line">
        <div class="labelbold">
            <asp:Label ID="LabelContestDescription" runat="server" Text="Contest description"></asp:Label>
        </div>
        <div style="float: left; margin-top: 5px;">
            <asp:TextBox ID="TextBoxContestDescription" runat="server"></asp:TextBox>
        </div>
     </div>
      <div class="line">
        <div class="labelbold">
           <asp:Label ID="LabelContestLocation" runat="server" Text="Contest location"></asp:Label>
        </div>
        <div style="float: left; margin-top: 5px;">
           <asp:TextBox ID="TextBoxContestLocation" runat="server"></asp:TextBox>
        </div>
      </div>    
      <div class="line">
        <div class="labelbold">
            <asp:Label ID="LabelGoalDifference" runat="server" Text="Table ranking mode"></asp:Label>
        </div>
        <div style="float: left; margin-top: 5px;">
            <asp:RadioButtonList ID="RadioButtonListGoalDifference" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Selected="True" Value="0">Internal game</asp:ListItem>
                <asp:ListItem Value="1">Goal difference</asp:ListItem>
            </asp:RadioButtonList>
         </div>
      </div>
      <div class="line">
        <div class="labelbold">
            <asp:Label ID="LabelNumberOfRounds" runat="server" Text="Number of rounds"></asp:Label>
        </div>
        <div style="float: left; margin-top: 5px;">
            <asp:TextBox ID="TextBoxNumberOfRounds" runat="server" Text="1"></asp:TextBox>
        </div>
        <asp:CustomValidator ID="cvNumberOfRounds" runat="server" ControlToValidate="TextBoxNumberOfRounds" ErrorMessage="Please enter a valid number of rounds">*</asp:CustomValidator>
      </div>
      <div class="line">
        <div class="labelbold">
            <asp:Label ID="LabelNumberOfPlayersToNextRound" runat="server" Text="Number of players to next round"></asp:Label>
        </div>
        <div style="float: left; margin-top: 5px;">
            <asp:TextBox ID="TextBoxNumberOfPlayersToNextRound" runat="server"></asp:TextBox>
        </div>
        <asp:CustomValidator ID="cvNumberOfPlayersToNextRound" runat="server" ControlToValidate="TextBoxNumberOfPlayersToNextRound" ErrorMessage="Please enter a valid number of players on to next round">*</asp:CustomValidator>
      </div>
      <div class="line">
        <div class="labelbold">
            <asp:Label ID="LabelPointsSpecification" runat="server" Text="Points for win/tie/loss"></asp:Label>
        </div>
        <div style="float: left; margin-top: 5px;">
            <asp:TextBox ID="TextBoxPointsWinTieLoss" runat="server" Text="2/1/0"></asp:TextBox>
        </div>
        <asp:CustomValidator ID="cvPointsWinTieLoss" runat="server" ControlToValidate="TextBoxPointsWinTieLoss" ErrorMessage="Please enter a valid point specification on the form W/T/L i.e. 2/1/0">*</asp:CustomValidator>
      </div>
      <div class="line">
        <div class="labelbold">
            <asp:Label ID="LabelNumberOfGameMinutes" runat="server" Text="Minutes per game"></asp:Label>
        </div>
        <div style="float: left; margin-top: 5px;">
            <asp:TextBox ID="TextBoxNumberOfGameMinutes" runat="server" Text="2/1/0"></asp:TextBox>
        </div>
        <asp:CustomValidator ID="cvNumberOfGameMinutes" runat="server" ControlToValidate="TextBoxNumberOfGameMinutes" ErrorMessage="Please enter a valid number of game minutes">*</asp:CustomValidator>
      </div>
      <div class="line">
        <div class="labelbold">
            <asp:Label ID="LabelTypeOfContest" runat="server" Text="Type of contest"></asp:Label>
        </div>
        <div style="float: left; margin-top: 5px;">
            <asp:RadioButtonList ID="RadioButtonListTypeOfContest" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Selected="True" Value="0">Series</asp:ListItem>
                <asp:ListItem Value="1">End game</asp:ListItem>
            </asp:RadioButtonList>
         </div>
      </div>

      <br /><br /><br />      
</div>



