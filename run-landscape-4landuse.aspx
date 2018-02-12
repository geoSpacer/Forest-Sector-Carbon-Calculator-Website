<%@ Page Language="C#" AutoEventWireup="true" CodeFile="run-landscape-4landuse.aspx.cs"
    Inherits="run_landscape_4landuse" %>

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
<body>
    <div id="mainPageDiv">
        <script type="text/javascript">loadNavMenu();</script>
        <noscript><br /><h1 style="text-align:center;">The Forest Sector Carbon Calculator</h1>
            <h1 style="text-align:center; color:Red;">Please Enable Javascript Before Proceeding</h1></noscript>

        <div class="container">
            <ul class="minitabs">
        <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 1)">1. Site Selection</a></li>
        <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 2)">2. Simulation Characteristics</a></li>
        <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 3)">3. Disturbance History</a></li>
        <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 4)" class="active">4. Future Land-use Management</a></li>
        <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 5)">5. Carbon Use</a></li>
            </ul>
        </div>
        <div id="mainPageTitle" style="margin-left: 20px">
            <span><a href="javascript:insertContent('stepNotes', 'notesView', 'notesHide')" id="notesView">
                View</a><a href="javascript:removeContent('stepNotes', 'notesView', 'notesHide')" id="notesHide" style="display: none;">
                Hide</a> notes for this step </span>Future Management and Disturbance Regimes - Landscape
        </div>
        <div id="main">
            <div id="stepNotes" style="display: none">
                This page determines how the landscape you are examining will be disturbed and managed in the future. To continue the last regime from the past click on 
                the 'Continue previous’ button. To add a new primary regime element, enter the primary regime characteristics and choose 'Add'. Once a primary regime is 
                in place you may add as many secondary regimes as you like. The 'Primary Regime' is the main reoccurring management action or disturbance and this will 
                occur starting from the Reference Calendar Year selected in simulation characteristics. As regime elements are added, the date ranges of existing regimes 
                will be altered to accommodate the new regime. To remove a regime or set of regimes, check the row(s) you want removed and click the 'Remove Elements' button. 
                To start over click on 'Clear All' button and the regime boxes will be cleared. 

                <p>To describe each regime element select the start and end years of the regime, an average treatment interval, percent disturbed, utilization, or severity. 
                Disturbance regimes include harvest (stem only, aboveground, whole tree, and salvage), prescribed fires, and wildfires. To setup a harvest with site preparation 
                fire select a harvest primary regime, prescribed fire for the secondary regime and an offset of 0. For harvests and prescribed fires, the intervals between treatments 
                will be of relatively equal length. Percent disturbed allows one to indicate a part of each stand in the landscape was not affected by the disturbance event. For example, 
                if 50% of a stand’s live biomass is cut, then specify Percent Disturbed to be 50%. The remaining part of the stand will not be affected by that disturbance event. Note 
                that this parameter does not mean that 50% of the stands are not harvested! As utilization / severity increases from low to high more of the live carbon is harvested or 
                killed and more is removed by harvest or combustion depending on the disturbance type.</p>

                <p>For wildfire default, the intervals, percent disturbed and severity are based on an analysis of historical trends in the selected region. The wildfire suppression 
                level can be set to either 'total suppression' (no wildfires are generated), 'no suppression' (wildfires are generated at estimated pre-European settlement levels), 
                or 'typical suppression' (wildfires are generated at 2.5x the 'no suppression' fire return interval to simulate the suppressed wildfire of recent history.  For wildfire 
                custom the interval, severity, and size of fires may all be varied from the most likely condition based on the region and elevation class selected.  At this point one may alter 
                the fire interval continuously and shift severity from one of the five classes to another.  Fire size can either be halved or doubled relative to the default value for a 
                region and elevation class. Bear in mind that fire size is described by a distribution, so doubling fire size will increase the chances of a very large fire, whereas halving 
                it will reduce the chances of a very large fire.  Since individual fire size is random, one could have very large fires under either scenario.</p>

             </div>
            <div id="inputForm">
                <form id="step4input" runat="server" method="post" defaultbutton="btn4next">
                <table width="100%">
                <tr><td>
                    Primary Regime<asp:RequiredFieldValidator ID="valReq_Primary" 
                        runat="server" ControlToValidate="ddl_primary" 
                        ErrorMessage="Select Primary Regime Type" InitialValue="SelectRegime" 
                        ValidationGroup="priValidators" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                    <br />
                        <asp:DropDownList ID="ddl_primary" runat="server"
                        OnSelectedIndexChanged="ddl_primary_SelectedIndexChanged" 
                        AutoPostBack="True">
                    </asp:DropDownList>
                </td><td>
                    Start Year<asp:RangeValidator 
                            ID="valRange_priStartYear" runat="server" 
                                    ControlToValidate="txt_priStartYear" ErrorMessage="Select Start Year of Regime (After spinup)" 
                                    MaximumValue="1" MinimumValue="0" ValidationGroup="priValidators" 
                            Type="Integer" SetFocusOnError="True">*</asp:RangeValidator>
                                <asp:CompareValidator ID="valComp_priStartYear" runat="server" 
                            ControlToCompare="txt_priEndYear" ControlToValidate="txt_priStartYear" 
                            ErrorMessage="Enter a start year less than the end year" Operator="LessThan" 
                            Type="Integer" ValidationGroup="priValidators" SetFocusOnError="True">*</asp:CompareValidator>
                                <br />
                    <asp:TextBox ID="txt_priStartYear" runat="server" CssClass="formText50"></asp:TextBox>
                </td><td>
                    End Year<asp:RangeValidator ID="valRange_priEndYear" runat="server" 
                                    ControlToValidate="txt_priEndYear" ErrorMessage="Select End Year of Regime (before current year)" 
                                    ValidationGroup="priValidators" Type="Integer" SetFocusOnError="True">*</asp:RangeValidator>
                                <br />
                    <asp:TextBox ID="txt_priEndYear" runat="server" CssClass="formText50"></asp:TextBox>
                </td><td>
                    <asp:Label ID="lbl_priInterval" runat="server" Text="Interval"></asp:Label>
                    <asp:RequiredFieldValidator ID="valReq_priInterval" runat="server" 
                                    ControlToValidate="txt_priInterval" 
                                    ErrorMessage="Select Regime Treatment Interval" 
                                    ValidationGroup="priValidators" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="valRegExp_priInterval" runat="server" 
                                    ControlToValidate="txt_priInterval" ErrorMessage="Number required" 
                                    ValidationExpression="^\d+$" ValidationGroup="priValidators" 
                                    SetFocusOnError="True">*</asp:RegularExpressionValidator>
                                <br />
                    <asp:TextBox ID="txt_priInterval" runat="server" CssClass="formText50"></asp:TextBox>&nbsp;&nbsp;yrs
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
                    &nbsp;<a href="docs/Carbon_Calculator_utilization.pdf"><asp:Image ID="img_help_priUtilization" 
                        runat="server" ImageUrl="~/images/help-about-icon.png" AlternateText="help" /></a>
                    <a href="#" onclick="helpWildfireSize()"><asp:Image ID="img_help_fireSize" runat="server" ImageUrl="~/images/help-about-icon.png" Visible="False" AlternateText="help" /></a>
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
                <td class="formLastDropdown">
                    <asp:Label ID="lbl_priSeverity" runat="server" Text="Severity"></asp:Label>
                    <asp:RequiredFieldValidator ID="valReq_priSeverity" 
                            runat="server" ControlToValidate="ddl_priSeverity" 
                            ErrorMessage="Select Severity for Primary Regime" 
                            ValidationGroup="priValidators" InitialValue="Select Level" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                    &nbsp;<a href="docs/Carbon_Calculator_Fire_History_Parameterization.pdf"><asp:Image ID="img_help_pri_severity" runat="server" ImageUrl="~/images/help-about-icon.png" AlternateText="help" /></a>
                    <a href="#" onclick="helpWildfireLevel()"><asp:Image ID="img_help_pri_suppression" runat="server" 
                        ImageUrl="~/images/help-about-icon.png" Visible="False" AlternateText="help" /></a>
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
                <tr>
                <td>
                    <asp:Label ID="lbl_secondary" runat="server" Text="Secondary Regime" 
                        ForeColor="Gray"></asp:Label>
                    <asp:RequiredFieldValidator ID="valReq_secondary" 
                        runat="server" ControlToValidate="ddl_secondary" 
                        ErrorMessage="Select Secondary Regime Type" InitialValue="SelectRegime" 
                        ValidationGroup="secValidators" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                    <br />
                        <asp:DropDownList ID="ddl_secondary" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="ddl_secondary_SelectedIndexChanged" 
                        Enabled="False">
                    </asp:DropDownList>
                </td><td colspan="2">
                    <asp:Label ID="lbl_secPlacement" runat="server" Text="Location" ></asp:Label>
                    <br />
                        <asp:DropDownList ID="ddl_secPlacement" runat="server" AutoPostBack="True" 
                        Enabled="True">
                    </asp:DropDownList>
                </td><td>
                    <asp:Label ID="lbl_secOffset" runat="server" Text="Offset"></asp:Label>
                    <asp:CustomValidator ID="valCust_secOffset" runat="server" ErrorMessage="Offset not allowed. Offset either already used for secondary regime type or 0. Zero offset only allowed with primary harvest and secondary prescribed fire regimes." 
                                    Text="*" ControlToValidate="txt_secOffset" ValidationGroup="secValidators"
                                    OnServerValidate="valOffsetFunc" EnableClientScript="False"></asp:CustomValidator>
                    <asp:RangeValidator ID="valRange_secOffset" runat="server" ErrorMessage="RangeValidator" 
                                    Text="*" SetFocusOnError="True" ControlToValidate="txt_secOffset" 
                                    ValidationGroup="secValidators" Type="Integer"></asp:RangeValidator>
                    <asp:RequiredFieldValidator ID="valReq_secOffset" runat="server" 
                                    ControlToValidate="txt_secOffset" 
                                    ErrorMessage="Select Regime Treatment Interval" 
                                    ValidationGroup="secValidators" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                    <br />
                    <asp:TextBox ID="txt_secOffset" runat="server" 
                            
                            onblur="disableTxtBox(this.value, 'txt_secDisturb', 'lbl_secDisturb', 'txt_priDisturb')" 
                            CssClass="formText50"></asp:TextBox>&nbsp;&nbsp;yrs
                </td><td>
                    <asp:Label ID="lbl_secDisturb" runat="server" Text="Disturbance"></asp:Label>
                    <asp:RangeValidator ID="valRange_secDisturb" runat="server"
                                    ControlToValidate="txt_secDisturb" 
                                    ErrorMessage="Enter Percent Disturbed (0-100)" MaximumValue="100" 
                                    MinimumValue="0" ValidationGroup="secValidators" Type="Integer" 
                                    SetFocusOnError="True">*</asp:RangeValidator>
                                <br />
                    <asp:TextBox ID="txt_secDisturb" runat="server" CssClass="formText50">100</asp:TextBox>&nbsp;&nbsp;%
                </td><td>
                    <asp:Label ID="lbl_secUtilization" runat="server" Text="Utilization"></asp:Label>
                    <asp:RequiredFieldValidator ID="valReq_secUtilization" 
                            runat="server" ControlToValidate="ddl_secUtilization" 
                            ErrorMessage="Select Utilization for Secondary Regime" 
                            ValidationGroup="secValidators" InitialValue="Select Level" 
                            SetFocusOnError="True">*</asp:RequiredFieldValidator>
                    &nbsp;<a href="docs/Carbon_Calculator_utilization.pdf"><asp:Image ID="img_help_secUtilization" 
                        runat="server" ImageUrl="~/images/help-about-icon.png" AlternateText="help" /></a>
                    <br />
                    <asp:DropDownList ID="ddl_secUtilization" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Label ID="lbl_secSeverity" runat="server" Text="Severity" ForeColor="Gray"></asp:Label>
                    <asp:RequiredFieldValidator ID="valReq_secSeverity" 
                            runat="server" ControlToValidate="ddl_secSeverity" 
                            ErrorMessage="Select Severity for Secondary Regime" 
                            ValidationGroup="secValidators" InitialValue="Select Level" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                    &nbsp;<a href="docs/Carbon_Calculator_Fire_History_Parameterization.pdf"><asp:Image ID="img_help_secSeverity" 
                        runat="server" ImageUrl="~/images/help-about-icon.png" AlternateText="help" /></a>
                        <br />
                    <asp:DropDownList ID="ddl_secSeverity" runat="server" Enabled="False">
                    </asp:DropDownList>
                </td><td align="right">
                        <asp:Button ID="btn_addSecondary" runat="server" Font-Bold="True" 
                            Font-Size="Large" Text="Add" Height="40px" 
                            ValidationGroup="secValidators" onclick="btn_addSecondary_Click" />
                    </td>
                </tr>
                </table>

                <table cellspacing="10px">
                    <tr>
                        <td >Management Regime:
                            <div style="overflow:auto; width:700px; height:200px; background: white; border: 1px solid grey">
                            <asp:CheckBoxList ID="cbList_management" runat="server" CellPadding="0" 
                                CellSpacing="0">
                            </asp:CheckBoxList></div>
                             Natural Disturbance Regime:
                            <div style="overflow:auto; width:700px; height:90px; background: white; border: 1px solid grey">
                            <asp:CheckBoxList ID="cbList_natDisturb" runat="server" CellPadding="0" 
                                CellSpacing="0">
                            </asp:CheckBoxList></div>
                        </td>
                        <td valign="top">
                            <br />
                            <asp:Button ID="btn4_setDefault" CssClass="formButtonSecondary" runat="server" 
                                Text="Continue Previous" onclick="btn4_setDefault_Click" 
                                 /><br /><br />                            
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
                 <asp:ValidationSummary ID="ValidationSummary2" runat="server" 
                    ShowMessageBox="True" ValidationGroup="secValidators" />
                <br />
                
                <asp:Button ID="btn4previous" class="formButton" runat="server" Text="Previous" OnClick="btn4previous_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btn4next" class="formButton" runat="server" Text="Next" OnClick="btn4next_Click" />
                </form>
            </div>
        </div>
    <script type= "text/javascript">loadFooter();</script>
    </div>
</body>
</html>
