<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userclassreport.aspx.cs" Inherits="ZGZY.WebUI.admin.html.userclassreport" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSource1" DataTextField="ClassName" DataValueField="Id" Visible="False"></asp:DropDownList>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" Visible="False" />
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ZGConnectionString %>" ProviderName="<%$ ConnectionStrings:ZGConnectionString.ProviderName %>" SelectCommand="SELECT * FROM [tbClass]"></asp:SqlDataSource>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True" GroupTreeImagesFolderUrl="" Height="50px" ToolbarImagesFolderUrl="" ToolPanelWidth="200px" Width="350px" ToolPanelView="None" />
    
    </div>
    </form>
</body>
</html>
