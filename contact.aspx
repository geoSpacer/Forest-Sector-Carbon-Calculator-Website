<%@ Page Language="C#" AutoEventWireup="true" CodeFile="contact.aspx.cs" Inherits="contact" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
<head>
  <title>The Forest Sector Carbon Calculator</title>
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <meta name="Keywords" content="forest, forest management, carbon calculator, forest products, forest sector, wildfire, disturbance regime" />
  <link href="css/carbon.css" type="text/css" rel="stylesheet" />
  <link href="css/sb_minitabs.css" type="text/css" rel="stylesheet" />
  <link href="css/csshorizontalmenu.css" type="text/css" rel="stylesheet" />
  <script src="css/csshorizontalmenu.js" type="text/javascript"></script>
  <script src="css/script_carbon.js" type="text/javascript" language="javascript"></script>
</head>

<body>
  <div id="mainPageDiv">
    <script type="text/javascript">loadNavMenu();</script>
    <noscript><br /><h1 style="text-align:center;">The Forest Sector Carbon Calculator</h1>
        <h1 style="text-align:center; color:Red;">Please Enable Javascript Before Proceeding</h1></noscript>

    <div id="main">
  <form id="form1" runat="server">
        <div id="mainPageTitle">Provide Feedback</div>
        <br />
        <table cellpadding="5" width="700px">
        <tr><td>Your Name</td>
        <td>
            <asp:TextBox ID="txtName" runat="server" Width="500px"></asp:TextBox></td>
        </tr>
        <tr><td>Your Organization</td>
        <td>
            <asp:TextBox ID="txtOrganization" runat="server" Width="500px"></asp:TextBox></td>
        </tr>
        <tr><td>Your Email Address<asp:RequiredFieldValidator 
                ID="RequiredFieldValidator_email" runat="server" 
                ControlToValidate="txtFromEmail" ErrorMessage="Please Enter Your Email Address" 
                SetFocusOnError="True" ValidationGroup="AllValidators">*</asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator_email" 
                runat="server" ControlToValidate="txtFromEmail" 
                ErrorMessage="Please enter a valid email address" SetFocusOnError="True" 
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                ValidationGroup="AllValidators">*</asp:RegularExpressionValidator>
            </td>
        <td>
            <asp:TextBox ID="txtFromEmail" runat="server" Width="500px"></asp:TextBox></td>
        </tr>
        <tr><td>Subject of Feedback</td>
        <td>
            <asp:TextBox ID="txtSubject" runat="server" Width="500px"></asp:TextBox></td>
        </tr>
        <tr><td>Please tell us your concerns and ideas for improvement.<asp:RequiredFieldValidator 
                ID="RequiredFieldValidator_message" runat="server" 
                ControlToValidate="txtMessage" ErrorMessage="Please enter feedback" 
                SetFocusOnError="True" ValidationGroup="AllValidators">*</asp:RequiredFieldValidator>
            </td>
        <td>
            <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Height="150px" 
                Width="500px"></asp:TextBox></td>
        </tr>
        
        </table>
        <br />
        <asp:Button ID="btnSend" runat="server" Text="Send Feedback" 
            CssClass="formButton" onclick="btnSend_Click" 
            ValidationGroup="AllValidators" />
        <br />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
            ShowMessageBox="True" ShowSummary="False" ValidationGroup="AllValidators" />
        <p>&nbsp;</p>

</form>
    </div>
    <script type= "text/javascript">loadFooter();</script>
</div>
</body>
</html>
