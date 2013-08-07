<%@ Page Language="C#" MasterPageFile="~/Site.master" Title="Content Page" CodeBehind="~/pgMain.aspx.cs" Inherits="TableHockey.pgMain"%>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" Runat="Server">
      <div class="line">
        <div class="labelheader">
            <asp:Label ID="LabelContestHeader" runat="server">My contests</asp:Label>
        </div>
    </div>
     <div class="line">
         <asp:GridView ID="GridViewContests" runat="server" AutoGenerateColumns="false">
             <Columns>
                 <asp:BoundField DataField="ContestId" HeaderText="ContestId" />
                 <asp:BoundField DataField="ContestName" HeaderText="Contest name" />
                 <asp:BoundField DataField="ContestDescription" HeaderText="Contest description" />
                 <asp:BoundField DataField="ContestLocation" HeaderText="Location" />
                 <asp:BoundField DataField="ContestDateOpened" HeaderText="Started on"  DataFormatString="{0:d}" />
                 <asp:BoundField DataField="ContestDateClosed" HeaderText="Ended on"  DataFormatString="{0:d}" />
                 <asp:HyperLinkField datanavigateurlfields="ContestID" datanavigateurlformatstring="~\pgEditContest.aspx?ContestID={0}"  Text="Edit" />
                 <asp:HyperLinkField datanavigateurlfields="ContestID" datanavigateurlformatstring="~\pgContestSummary.aspx?ContestID={0}"  Text="View/Export" />
             </Columns>
             
         </asp:GridView>
     </div>
     <br />
     <br />
     <div class="line">
        <div class="labelheader">
            <asp:Label ID="LabelPlayersHeader" runat="server" Text="Players"></asp:Label>
        </div>
    </div>
    <div class="line">
        <div class="player-table">
           <asp:GridView ID="GridViewPlayers" runat="server" AutoGenerateColumns="false">
             <Columns>
                <asp:BoundField DataField="PlayerId" HeaderText="Player ID" />
                <asp:BoundField DataField="FirstName" HeaderText="First name" />
                <asp:BoundField DataField="LastName" HeaderText="Last name" />
                 <asp:TemplateField HeaderText="Club"> 
                    <ItemTemplate> 
                         <asp:Label ID="LabelClub" Visible='<%# Eval("RegisteredByUserId").Equals(Membership.GetUser(User.Identity.Name).ProviderUserKey) ? false : true %>' Text='<%# Eval("ClubName") %>' runat="server"></asp:Label>
                         <asp:HyperLink ID="HyperLink1" NavigateUrl='<%# Eval("ClubID", "~/pgEditClub.aspx?ClubID={0}") %>' Visible='<%# Eval("RegisteredByUserId").Equals(Membership.GetUser(User.Identity.Name).ProviderUserKey) ? true : false %>' Text='<%# Eval("ClubName") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="BirthDate" HeaderText="Birth date" />
                <asp:BoundField DataField="RegisteredByUserId" HeaderText="RegisteredByUserId" Visible="false" />
                <asp:BoundField DataField="ClubId" HeaderText="ClubId" Visible="false" />
                <asp:ImageField ItemStyle-Width="70px" DataImageUrlField="PlayerId" DataImageUrlFormatString="~/ImageHandler.ashx?id={0}&imagetype=player" />      
                <asp:TemplateField HeaderText="Edit"> 
                    <ItemTemplate>                
                       <asp:HyperLink ID="HyperLink1" NavigateUrl='<%# Eval("PlayerID", "~/pgEditPlayer.aspx?PlayerID={0}") %>' Visible='<%# Eval("RegisteredByUserId").Equals(Membership.GetUser(User.Identity.Name).ProviderUserKey) ? true : false %>' Text="Edit" runat="server" />                    
                    </ItemTemplate> 
                </asp:TemplateField>
             </Columns>
         </asp:GridView>
        </div>       
    </div>
</asp:Content>


