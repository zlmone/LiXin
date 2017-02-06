<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="LiXinLMS.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align:center;">
        <div style="margin-top:100px; margin:auto; width:300px; height:100px;">
            <label>登录名：</label>
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            <div style="text-align:center;">
                <asp:Button ID="Button1" runat="server" Text="登录" onclick="Button1_Click"/>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
