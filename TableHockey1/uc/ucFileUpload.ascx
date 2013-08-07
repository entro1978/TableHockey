<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucFileUpload.ascx.cs"
    Inherits="TableHockey.uc.ucFileUpload" %>
<div class="line">
    <div class="categoryboldonly">
       <asp:Label ID="lblAttachmentInfo" runat="server">Bifoga fil(er) (Max [ANT] st.)</asp:Label>
    </div>
</div>
<div class="line">
    <div style="font-size: x-small;">
        Tillåtna format är: .jpg, .gif, .png, .bmp, .doc, .docx, .pdf, .xls, .xlsx, .ppt, .pptx. <br /> <span id="Wizard1_lblFileSize">Max
            filstorlek 5MB.</span></div>
</div>
<div class="line">
    Välj fil:
</div>
<div class="line">
    <asp:FileUpload ID="filePlace" runat="server" Size="55px" CssClass="uploadfile" ToolTip="Välj fil att bifoga"/>
</div>
<div class="line">
    <asp:Button ID="btnAddPlaceFile" runat="server" ToolTip="Klicka här om du vill lägga till en fil som du valt genom att bläddra"
        Text="L&#228;gg till" OnClick="btnAddPlaceFile_Click" />
    <asp:Button ID="btnRemovePlaceFile" runat="server" ToolTip="Klicka här om du vill ta bort en markerad fil i listan"
        Text="Ta bort" OnClick="btnRemovePlaceFile_Click" />
</div>
<div class="line">
                    (Välj fil genom att trycka på bläddra-knappen. Tryck sedan på lägg till-knappen för att lägga till filen.
                     Vill du ta bort en fil så kan du markera filen och trycka på ta bort-knappen)
                    </div>
<div class="line">
    Bifogade filer:<asp:Label ID="lblPlaceErr" runat="server" ForeColor="Red" Text="Filen kunde inte l&#228;ggas till"
        Visible="False"></asp:Label>
</div>
<div class="line">
    <div style="float: left; clear: right;">
        <asp:ListBox ID="lstPlaceFiles" runat="server" Width="95%" ToolTip="Fil(er) som kommer att bifogas"
            AutoPostBack="true"></asp:ListBox>
    </div>
    <div class="error" style="float: right;">
        <asp:CustomValidator ID="cvFileUpload" runat="server" ErrorMessage="Ladda upp en fil."
            ValidationGroup="Step" ControlToValidate="lstPlaceFiles">*</asp:CustomValidator>
    </div>
</div>
