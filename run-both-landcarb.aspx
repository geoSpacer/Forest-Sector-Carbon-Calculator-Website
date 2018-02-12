<%@ Page Language="C#" AutoEventWireup="true" CodeFile="run-both-landcarb.aspx.cs" Inherits="run_landcarb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
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
        <script type= "text/javascript">loadNavMenu();</script>
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
        
        <div id="mainPageTitle" style="margin-left:20px">Start Simulation - <script type="text/javascript">formatScaleName('<%=HttpContext.Current.Session["scale"]%>')</script>
        </div>
        
        <div id="main">
            <h3>
                Review input selections</h3>
            Once &#39;Run Model&#39; is pressed, the simulation may take a few minutes to display 
            the output page.
            <form id="outputForm2" runat="server" onsubmit="return setHourglass()">
            <div id="inputForm">
                <table><tr><td width="400px">
                    Run Name: <asp:Label ID="lbl_2runName" runat="server" Text=""></asp:Label><br />
                    Region: <asp:Label ID="lbl_1region" runat="server" Text=""></asp:Label><br />
                    Ownership: <asp:Label ID="lbl_1own" runat="server" Text=""></asp:Label><br />
                    Elevation Class: <asp:Label ID="lbl_1elevClass" runat="server" Text=""></asp:Label><br />
                    Grid Cell Size: <asp:Label ID="lbl_2cellSize" runat="server" Text=""></asp:Label> ha<br />
                    </td><td valign="top">
                    Current Year: <asp:Label ID="lbl_2currentYear" runat="server" Text=""></asp:Label><br />
                    Simulation Run Time (Years): <asp:Label ID="lbl_2numSimYears" runat="server" Text=""></asp:Label><br />
                    Uncertainty Analysis: <asp:Label ID="lbl_2randomSeed" runat="server" Text=""></asp:Label><br />
                        Default
                    Product Substitution: <asp:Label ID="lbl_5prodSubstitution" runat="server" Text=""></asp:Label><br />
                        Default
                    Bioenergy Substitution: <asp:Label ID="lbl_5energySubstitution" runat="server" Text=""></asp:Label><br />
                    </td></tr></table>
                    <br />
                    <script type="text/javascript">checkParamsTitle('<%=HttpContext.Current.Session["scale"]%>')</script><br />
                    <asp:TextBox ID="TextBox_disturbance" runat="server" ReadOnly="True" 
                    TextMode="MultiLine" CssClass="formLargeTextBox"></asp:TextBox>
                   
                    <br /><br />
                    <asp:Button ID="btn_previous" class="formButton" runat="server" Text="Previous" 
                        onclick="btn_previous_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                
                    <!--<asp:Button ID="btnRunModel" class="formButton" runat="server" Text="Run Model" OnClientClick="openWaitWindow()" />-->
                    <input id="btn_runModel" type="button" value="Run Model" class="formButton" onclick="openWaitWindow();" />
                    
                    </div>
 
            </form>
</div>
    <script type= "text/javascript">loadFooter();</script>
    </div>
</body>
</html>
