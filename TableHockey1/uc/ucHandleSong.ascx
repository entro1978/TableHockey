<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucHandleSong.ascx.cs" Inherits="TableHockey.uc.ucHandleSong" %>
<div style="border-style: solid; border-width: 1px; border-color: White;">
    <div class="line">
        <div class="labelheader">
            <asp:Label ID="LabelPlaySongHeader" runat="server" Text="Now playing:"></asp:Label>
        </div>
    </div>
    <div class="line">
        <div class="labelbold">
            <asp:Label ID="LabelTicker" runat="server"></asp:Label>
        </div>
    </div>
     <div class="line">
        <div>
            <asp:FileUpload ID="FileUploadSong" runat="server" ToolTip="Välj slinga" />
        </div>
    </div>
    <div class="line">
        <div>
           <asp:Button ID="ButtonPlaySong" runat="server" Text="Spela" />
        </div>
        <div style="float:left;">
           <asp:Button ID="ButtonPauseSong" runat="server" Text="Paus" />
        </div>
    </div>
 </div>
