<%@ Page Language="C#" AutoEventWireup="true" CodeFile="run-stand-3history.aspx.cs"
    Inherits="run_stand_3history" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
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
        <script type= "text/javascript">loadNavMenu();</script>
        <noscript><br /><h1 style="text-align:center;">The Forest Sector Carbon Calculator</h1>
            <h1 style="text-align:center; color:Red;">Please Enable Javascript Before Proceeding</h1></noscript>

        <div class="container">
            <ul class="minitabs">
                <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 1)">1. Site Selection</a></li>
                <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 2)">2. Simulation Characteristics</a></li>
                <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 3)" class="active">3. Disturbance History</a></li>
                <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 4)">4. Future Land-use Management</a></li>
                <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 5)">5. Carbon Use</a></li>
            </ul>
        </div>
        <div id="mainPageTitle" style="margin-left: 20px">
            <span><a href="javascript:insertContent('stepNotes', 'notesView', 'notesHide')" id="notesView">
                View</a><a href="javascript:removeContent('stepNotes', 'notesView', 'notesHide')" id="notesHide" style="display: none;">
                Hide</a> notes for this step </span>Management and Disturbance History - Stand
        </div>
        <div id="main">
            <div id="stepNotes" style="display: none">
                This page determines how the stand you are examining was disturbed and managed in the past. For each event select a year, treatment regime type, 
                percent disturbed and utilization / severity before clicking the 'Add ' button. You may check rows that you want to remove: to delete checked rows 
                from the history window, click the 'Remove Elements' button. The 'Clear All’  button will delete all events from the history widow in case you wish 
                to start over. Treatment types include harvests  (stem only, aboveground, whole tree, and salvage), prescribed fires, and wildfires. If there is site 
                preparation fire following a harvest, then we recommend adding that event in the year following the harvest. Percent disturbed allows one to indicate a 
                part of the stand was not affected by the disturbance event. For example, if 50% of the stand live biomass is cut, then specify that. The remaining part 
                of the stand will not be affected by that disturbance event. As utilization / severity increases from very low to very high more of the live carbon is 
                harvested or killed and more is removed by harvest or combustion depending on the disturbance type. In the case of utilization, one can specify that 
                none of the cut trees are removed by selecting ‘none’.
            </div>
            <div id="inputForm">
                <form id="step3input" runat="server" method="post" defaultbutton="btn3next">
                <table width="100%">
                <tr><td>
                    Treatment<asp:RequiredFieldValidator ID="valReq_Primary" 
                        runat="server" ControlToValidate="ddl_primary" 
                        ErrorMessage="Select Primary Regime Type" InitialValue="SelectRegime" 
                        ValidationGroup="priValidators" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                    <br />
                        <asp:DropDownList ID="ddl_primary" runat="server"
                        OnSelectedIndexChanged="ddl_primary_SelectedIndexChanged" 
                        AutoPostBack="True">
                    </asp:DropDownList>
                </td><td>
                    <asp:RangeValidator 
                            ID="valRange_priStartYear" runat="server" 
                                    ControlToValidate="txt_priStartYear" ErrorMessage="Select Start Year of Regime (After spinup)" 
                                    MaximumValue="1" MinimumValue="0" ValidationGroup="priValidators" 
                            Type="Integer" SetFocusOnError="True" Enabled="False">*</asp:RangeValidator>
                                <asp:CompareValidator ID="valComp_priStartYear" runat="server" 
                            ControlToCompare="txt_priEndYear" ControlToValidate="txt_priStartYear" 
                            ErrorMessage="Enter a start year less than the end year" Operator="LessThan" 
                            Type="Integer" ValidationGroup="priValidators" SetFocusOnError="True" Enabled="False">*</asp:CompareValidator>
                                <br />
                    <asp:TextBox ID="txt_priStartYear" runat="server" Visible="False" 
                            CssClass="formText50"></asp:TextBox>
                </td><td>
                    <asp:RangeValidator ID="valRange_priEndYear" runat="server" 
                                    ControlToValidate="txt_priEndYear" ErrorMessage="Select End Year of Regime (before current year)" 
                                    ValidationGroup="priValidators" Type="Integer" 
                            SetFocusOnError="True" Enabled="False" MaximumValue="1" MinimumValue="0">*</asp:RangeValidator>
                                <br />
                    <asp:TextBox ID="txt_priEndYear" runat="server" Visible="False" 
                            CssClass="formText50"></asp:TextBox>
                </td><td>
                    <asp:Label ID="lbl_priYear" runat="server" Text="Year"></asp:Label><asp:RangeValidator ID="valRange_priYear" runat="server" 
                            ErrorMessage="RangeValidator" ControlToValidate="txt_priYear" Text="*" 
                            SetFocusOnError="True" ValidationGroup="priValidators" Type="Integer"></asp:RangeValidator><asp:RequiredFieldValidator ID="valReq_priYear" runat="server" 
                                    ControlToValidate="txt_priYear" 
                                    ErrorMessage="Select Event Year" 
                                    ValidationGroup="priValidators" SetFocusOnError="True" Text="*"></asp:RequiredFieldValidator><a href="#" 
                     onclick="helpStartYear()"><asp:Image ID="img_help_startYear" runat="server" ImageUrl="~/images/help-about-icon.png" AlternateText="help" /></a>
                    &nbsp;&nbsp;<br />
                    <asp:TextBox ID="txt_priYear" runat="server" CssClass="formText50"></asp:TextBox>
                </td><td>
                    <asp:Label ID="lbl_priDisturb" runat="server" Text="Disturbance"></asp:Label>
                    <asp:RangeValidator ID="valRange_priDisturb" runat="server" 
                                    ControlToValidate="txt_priDisturb" 
                                    ErrorMessage="Enter Percent Disturbed (0-100)" MaximumValue="100" 
                                    MinimumValue="0" ValidationGroup="priValidators" Type="Integer" 
                                    SetFocusOnError="True">*</asp:RangeValidator>
                                <br />
                    <asp:TextBox ID="txt_priDisturb" runat="server" CssClass="formText50">100</asp:TextBox>&nbsp;&nbsp;%
                </td><td>
                    <asp:Label ID="lbl_priUtilization" runat="server" Text="Utilization"></asp:Label>
                    <asp:RequiredFieldValidator ID="valReq_priUtilization" 
                            runat="server" ControlToValidate="ddl_priUtilization" 
                            ErrorMessage="Select Utilization for Primary Regime" 
                            ValidationGroup="priValidators" InitialValue="Select Level" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                    &nbsp;<a href="docs/Carbon_Calculator_utilization.pdf"><asp:Image 
                        ID="img_help_utilization" runat="server" ImageUrl="~/images/help-about-icon.png" 
                        AlternateText="help" /></a>
                    <br />
                    <asp:DropDownList ID="ddl_priUtilization" runat="server">
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddl_priFireSize" runat="server" Visible="False">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="valReq_priFireSize" 
                            runat="server" ControlToValidate="ddl_priFireSize" 
                            ErrorMessage="Select Fire Size for Primary Regime" 
                            ValidationGroup="priValidators" InitialValue="Select Level" 
                            SetFocusOnError="True" Enabled="False">*</asp:RequiredFieldValidator>
                </td>
                <td width="275px">
                    <asp:Label ID="lbl_priSeverity" runat="server" Text="Severity"></asp:Label>
                    <asp:RequiredFieldValidator ID="valReq_priSeverity" 
                            runat="server" ControlToValidate="ddl_priSeverity" 
                            ErrorMessage="Select Severity for Primary Regime" 
                            ValidationGroup="priValidators" InitialValue="Select Level" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                    <a href="docs/Carbon_Calculator_Fire_History_Parameterization.pdf">&nbsp;<asp:Image 
                        ID="img_help_severity" runat="server" ImageUrl="~/images/help-about-icon.png" 
                        AlternateText="help" /></a>
                        <br />
                    <asp:DropDownList ID="ddl_priSeverity" runat="server">
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddl_priSuppression" runat="server" Visible="False">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="valReq_priSuppression" 
                            runat="server" ControlToValidate="ddl_priSuppression" 
                            ErrorMessage="Select suppression level for primary regime" 
                            ValidationGroup="priValidators" InitialValue="Select Level" 
                        SetFocusOnError="True" Enabled="False">*</asp:RequiredFieldValidator>
                </td><td align="right">
                        <asp:Button ID="btn_addPrimary" runat="server" Font-Bold="True" 
                            Font-Size="Large" Text="Add" Height="40px" 
                            onclick="btn_addPrimary_Click" ValidationGroup="priValidators" />
                    </td>
                </tr>
                </table>

                <table cellspacing="10px">
                    <tr>
                        <td >Management and Natural Disturbance Event List:
                            <div style="overflow:auto; width:700px; height:200px; background: white; border: 1px solid grey">
                            <asp:CheckBoxList ID="cbList_management" runat="server" CellPadding="0" 
                                CellSpacing="0">
                            </asp:CheckBoxList></div>
                        </td>
                        <td valign="top">
                            <br />
                            <asp:Button ID="btn4_removeElements" CssClass="formButtonSecondary" runat="server" 
                                Text="Remove Elements" onclick="btn4_removeElements_Click"
                                 /><br /><br />                           
                            <asp:Button ID="btn4_clearAll" CssClass="formButtonSecondary" runat="server" 
                                Text="Clear All" onclick="btn4_clearAll_Click"
                                 />
                            
                        </td>
                    </tr>
                </table>
                
                 <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                    ShowMessageBox="True" ValidationGroup="priValidators" 
                    ShowSummary="False" />
                <br />
                <asp:Button ID="btn3previous" CssClass="formButton" runat="server" Text="Previous" OnClick="btn3previous_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btn3next" CssClass="formButton" runat="server" Text="Next" OnClick="btn3next_Click" />
                </form>
            </div>
        </div>
    <script type= "text/javascript">loadFooter();</script>
    </div>
</body>
</html>
