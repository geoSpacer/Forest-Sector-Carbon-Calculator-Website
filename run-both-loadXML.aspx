<%@ Page Language="C#" AutoEventWireup="true" CodeFile="run-both-loadXML.aspx.cs" Inherits="run_both_loadXML_code" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
<head>
    <title>The Forest Sector Carbon Calculator</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="css/carbon.css" type="text/css" rel="stylesheet" />
    <link href="css/sb_minitabs.css" type="text/css" rel="stylesheet" />
    <link href="css/csshorizontalmenu.css" type="text/css" rel="stylesheet" />
    <script src="css/csshorizontalmenu.js" type="text/javascript"></script>
    <script src="css/script_carbon.js" type="text/javascript" language="javascript"></script>
</head>
<body onunload="setHourglass();">
    <div id="mainPageDiv">
        <script type="text/javascript">loadNavMenu();</script>
        <noscript><br /><h1 style="text-align:center;">The Forest Sector Carbon Calculator</h1>
            <h1 style="text-align:center; color:Red;">Please Enable Javascript Before Proceeding</h1></noscript>

        <div class="container">
            <ul class="minitabs">
                <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 1)">1. Site Selection</a></li>
                <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 2)">2. Simulation Characteristics</a></li>
                <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 3)">3. Disturbance History</a></li>
                <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 4)">4. Future Land-use Management</a></li>
                <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 5)">5. Carbon Use</a></li>
            </ul>
        </div>
        <div id="mainPageTitle" style="margin-left: 20px">
            <span><a href="javascript:insertContent('stepNotes', 'notesView', 'notesHide')" id="notesView">
                View</a><a href="javascript:removeContent('stepNotes', 'notesView', 'notesHide')" id="notesHide" style="display: none;">
                Hide</a> notes for this step </span>Load Existing Run - 
                <script type="text/javascript">formatScaleName('<%=HttpContext.Current.Session["scale"]%>')</script>
        </div>
        <div id="main">
            <div id="stepNotes" style="display: none">
                If you have previously run this model and have saved the output, you may load the model parameters into this website from your computer. Select the browse
                button below and navigate to the FSCC_stand_paramters.xml file or the FSCC_landscape_parameters.xml file. Once the file is selected, choose next and the parameters
                should appear in the web forms.
            </div>
            <div id="inputForm">
                <form id="loadXMLfromFile" runat="server" method="post" defaultbutton="btn2next">
             <p>
                To upload an existing paramter set click browse
                &nbsp;<asp:RegularExpressionValidator 
                    ID="RegularExpressionValidator1" runat="server" 
                    ControlToValidate="FileUpload_xml" ErrorMessage="Only valid xml files may be uploaded" 
                    ValidationExpression="^.*\.(xml|XML)$" 
                    ValidationGroup="AllValidators">*</asp:RegularExpressionValidator>
                &nbsp;<br />
            <asp:FileUpload ID="FileUpload_xml" runat="server" />&nbsp;
                <input id="btn_clearFileUpload" type="button" 
                    onclick="clearFileUpload('FileUpload_stand')" value="Clear" />&nbsp;&nbsp;&nbsp;
                <asp:Label ID="label_upload" runat="server"></asp:Label>
            </p>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                    ValidationGroup="AllValidators" ShowSummary="False" />
                <br />
                <asp:Button ID="btn2return" class="formButton" runat="server" Text="Previous" OnClick="btn2return_Click"
                    TabIndex="4" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btn2next" class="formButton" runat="server" Text="Next" OnClick="btn2next_Click"
                    TabIndex="5" ValidationGroup="AllValidators" />
                </form>
            </div>
        </div>
    <script type= "text/javascript">loadFooter();</script>
    </div>
</body>
</html>
