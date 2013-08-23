<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCalendar.ascx.cs" Inherits="TableHockey.uc.ucCalendar" %>
<asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="false" ToolTip="Välj datum från kalender" ImageUrl="~/images/Calendar.png"
    OnClick="ImageButton1_Click" />
<asp:PlaceHolder ID="plCalendar" Visible="false" runat="server">
    <asp:Calendar ID="Calendar1" runat="server" FirstDayOfWeek="Monday"
        OnDayRender="Calendar1_DayRender" OnSelectionChanged="Calendar1_SelectionChanged">
        <TitleStyle CssClass="calendartitle" BackColor="#ffffff" />
        <TodayDayStyle CssClass="calendar_today" BackColor="#ffffff"/>
        <DayStyle BackColor="#ffffff" />
        <SelectedDayStyle CssClass="calendar_selected" />
        <WeekendDayStyle CssClass="calendar_weekend" />
    </asp:Calendar>
</asp:PlaceHolder>
