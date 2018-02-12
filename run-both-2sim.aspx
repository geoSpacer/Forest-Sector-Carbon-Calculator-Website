<%@ Page Language="C#" AutoEventWireup="true" CodeFile="run-both-2sim.aspx.cs" Inherits="run_both_2sim_code" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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

        <div class="container">
            <ul class="minitabs">
                <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 1)">1. Site Selection</a></li>
                <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 2)" class="active">2. Simulation Characteristics</a></li>
                <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 3)">3. Disturbance History</a></li>
                <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 4)">4. Future Land-use Management</a></li>
                <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 5)">5. Carbon Use</a></li>
            </ul>
        </div>
        <div id="mainPageTitle" style="margin-left: 20px">
            <span><a href="javascript:insertContent('stepNotes', 'notesView', 'notesHide')" id="notesView">
                View</a><a href="javascript:removeContent('stepNotes', 'notesView', 'notesHide')" id="notesHide" style="display: none;">
                Hide</a> notes for this step </span>Simulation Characteristics - 
                <script type="text/javascript">formatScaleName('<%=HttpContext.Current.Session["scale"]%>')</script>
        </div>
        <div id="main">
            <div id="stepNotes" style="display: none">
                Please enter the parameters below to determine several key aspects of your simulation.  The reference calendar year determines when the simulation shifts 
                from the past to the future.  When in doubt enter the current year.  Next enter how many years into the future your simulation will continue.  To see long-term 
                trends it is best to set this to at least several management rotation intervals.  If you want the random features of the simulation, such as future wildfire 
                occurrence to occur in the same years for each simulation run, then indicate No on the third question.  If you want these to occur in different years for 
                each simulation run, then indicate Yes.
            </div>
            <div id="inputForm">
                <form id="step2input" runat="server" method="post" defaultbutton="btn2next">
                <table width="100%" cellspacing="10">
                    <tr>
                        <td width="480px">
                            What is the current or <a href="glossary.aspx#reference_calendar_year">reference calendar year</a> of the simulation?<asp:RequiredFieldValidator
                                ID="RequiredFieldValidator_currentYear" runat="server" ControlToValidate="textCalendarYear"
                                ErrorMessage="Please enter a current year" ValidationGroup="AllValidators" 
                                SetFocusOnError="True">*</asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="RangeValidator_currentYear" runat="server" 
                                ControlToValidate="textCalendarYear" 
                                ErrorMessage="Enter current year between 0 and 3000" MaximumValue="3000" 
                                MinimumValue="0" SetFocusOnError="True" Type="Integer" 
                                ValidationGroup="AllValidators">*</asp:RangeValidator>
                            
                        </td>
                        <td width="120px">
                            <asp:TextBox ID="textCalendarYear" runat="server" MaxLength="4" Width="120px"
                                TabIndex="1" ontextchanged="textCalendarYear_TextChanged"></asp:TextBox>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            How many years in the future should the simulation continue?
                            <asp:RequiredFieldValidator
                                ID="RequiredFieldValidator_numYears" runat="server" ControlToValidate="textSimYears"
                                ErrorMessage="Please enter number of years for simulation" 
                                ValidationGroup="AllValidators" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="RangeValidator_numYears" runat="server" 
                                ControlToValidate="textSimYears" 
                                ErrorMessage="Enter number of simulation years between 1 and 10000" 
                                MaximumValue="10000" MinimumValue="1" SetFocusOnError="True" Type="Integer" 
                                ValidationGroup="AllValidators">*</asp:RangeValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="textSimYears" runat="server" Width="120"
                                TabIndex="2"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Are you using the output for risk and 
                            <a href="glossary.aspx#uncertainty_analysis">uncertainty analysis</a>?
                            &nbsp;&nbsp;<a href="#" onclick="helpRand()"><asp:Image ID="img_help_rand" 
                                runat="server" ImageUrl="~/images/help-about-icon.png" AlternateText="help" /></a>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="radioRepeatRand" runat="server" RepeatDirection="Horizontal"
                                TabIndex="3" Width="120px">
                                <asp:ListItem>Yes</asp:ListItem>
                                <asp:ListItem Selected="True">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td></td>
                    </tr>
                     <tr>
                        <td>
                            Enter unique name to identify this run on output graphs.
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_runName" runat="server" 
                                ControlToValidate="txtRunName" 
                                ErrorMessage="Please enter a unique name to identify run." 
                                SetFocusOnError="True" ValidationGroup="AllValidators">*</asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRunName" runat="server" Width="120"
                                TabIndex="4"></asp:TextBox>
                        </td>
                    </tr>
                   <tr>
                        <td>
                            <asp:Label ID="lbl_cellSize1" runat="server" Text="Enter grid cell size in hectares."></asp:Label>
                            <asp:RequiredFieldValidator
                                ID="RequiredFieldValidator_cellArea" runat="server" ControlToValidate="txtCellAreaHa"
                                ErrorMessage="Please enter number of hectares for each grid cell" 
                                ValidationGroup="AllValidators" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="RangeValidator_cellArea" runat="server" 
                                ControlToValidate="txtCellAreaHa" 
                                ErrorMessage="Enter cell area in hectares between 1 and 100" 
                                MaximumValue="100" MinimumValue="1" SetFocusOnError="True" Type="Double" 
                                ValidationGroup="AllValidators">*</asp:RangeValidator>
                            &nbsp;&nbsp;<a href="#" onclick="helpCellSize()">
                                <asp:Image ID="img_help_cellSize" runat="server" ImageUrl="~/images/help-about-icon.png" AlternateText="help" /></a>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCellAreaHa" runat="server" Width="120"
                                TabIndex="5" AutoPostBack="True" ontextchanged="txtCellAreaHa_TextChanged"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Label ID="lbl_cellSize2" runat="server" Text="Total:&nbsp;"></asp:Label> 
                            <asp:Label ID="lblTotalAreaHa" runat="server" Text="0"></asp:Label>
                            <asp:Label ID="lbl_cellSize3" runat="server" Text="&nbsp;ha ("></asp:Label>
                            <asp:Label ID="lblTotalAreaAc" runat="server" Text="0"></asp:Label>
                            <asp:Label ID="lbl_cellSize4" runat="server" Text="&nbsp;ac)"></asp:Label></td>
                    </tr>
                </table>
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
