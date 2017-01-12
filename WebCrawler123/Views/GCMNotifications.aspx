<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GCMNotifications.aspx.cs" Inherits="WebCrawler.Views.GCMNotifications" %>

<!DOCTYPE html>  
  
<html xmlns="http://www.w3.org/1999/xhtml">  
<head id="Head1" runat="server">  
    <title></title>  
</head>  
<body>  
    <form id="form1" runat="server">  
    <div>  
        <asp:TextBox runat="server" ID="txt_message1" TextMode="MultiLine" Height="124px" Width="336px"></asp:TextBox>  
      
    </div>  
        <div>  
            <asp:Button ID="btn_send" Text="Send" runat="Server" OnClick="btn_send_Click" />  
            <br />  
            <asp:Label ID="res" runat="server" Text="Label"></asp:Label>
        </div>  
    </form>  
</body>  
</html>  