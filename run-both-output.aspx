<%@ Page Language="C#" AutoEventWireup="true" CodeFile="run-both-output.aspx.cs" Inherits="run_output" %>

<%--<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>The Forest Sector Carbon Calculator</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="Keywords" content="forest, forest management, carbon calculator, forest products, forest sector, wildfire, disturbance regime" />
    <link href="css/aspMenu.css" type="text/css" rel="stylesheet" />
    <link href="css/carbon.css" type="text/css" rel="stylesheet" />
    <link href="css/sb_minitabs.css" type="text/css" rel="stylesheet" />
    <link href="css/csshorizontalmenu.css" type="text/css" rel="stylesheet" />
    <script src="css/csshorizontalmenu.js" type="text/javascript"></script>
    <script src="css/script_carbon.js" type="text/javascript" language="javascript"></script>
</head>
<body onload="formatScaleNameBody('scaleName', 'modelScale');">
  <div id="mainPageDiv">
    <script type= "text/javascript">loadNavMenu();</script>
    <noscript><br /><h1 style="text-align:center;">The Forest Sector Carbon Calculator</h1>
        <h1 style="text-align:center; color:Red;">Please Enable Javascript Before Proceeding</h1></noscript>
    
    <div id="main">
        <div id="mainPageTitle">
            <span><a href="javascript:insertContent('stepNotes', 'notesView', 'notesHide')" id="notesView">
                View</a><a href="javascript:removeContent('stepNotes', 'notesView', 'notesHide')" id="notesHide" style="display: none;">
                Hide</a> notes for this step </span>View Model Output - <b id="scaleName">File</b>
        </div>

        <div id="stepNotes" style="display:none"><br />When the calculator has finished processing, please select 
        a graph or output file to view from the pulldown list. The scale of the graph can be changed by entering a start year or clearing the 'Auto
        Scale' check box. Once cleared, the Y axis min and max values may be entered directly. If you are viewing an individual graph, you can turn the 
        minimum and maximum lines off with the 'Show Min/Max Lines' checkbox.  To copy the graph into another application, 
        right click and choose 'copy image' (Mozilla Firefox) or right click and choose 'Save Picture As' (IE). In (IE) the picture will need to be saved
        to your computer then imported. In (Mozilla Firefox) the image can be pasted into another application.</div>
        
        <form id="outputForm" runat="server">
        <div id="inputForm">
            <input id="userDirectory" runat="server" type="hidden" />
            <input id="menuSelection" runat="server" type="hidden" />
            <input id="runDateTime" runat="server" type="hidden" />
            <input id="runName" runat="server" type="hidden" />
            <input id="numCells" runat="server" type="hidden" />
            <input id="modelScale" runat="server" type="hidden" />
        <asp:Menu ID="menuOutput" runat="server" 
            DataSourceID="XmlDataSource1" DynamicHorizontalOffset="5" 
            Orientation="Horizontal" onmenuitemclick="menuOutput_MenuItemClick">
            <StaticMenuItemStyle CssClass="staticMenuItem" />
            <DynamicHoverStyle CssClass="dynamicMenuHover" />
            <DynamicMenuItemStyle CssClass="dynamicMenuItem" />
            <DataBindings>
                <asp:MenuItemBinding DataMember="Menu" Selectable="False" TextField="text" />
                <asp:MenuItemBinding DataMember="subMenu" TextField="text" ValueField="value" 
                    SelectableField="selectable" />
            </DataBindings>
            <StaticHoverStyle CssClass="staticMenuHover" />
        </asp:Menu>
        <asp:XmlDataSource ID="XmlDataSource1" runat="server" 
            DataFile="~/menuListOutput.xml" XPath="/Home/Menu"></asp:XmlDataSource>
            <br />
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="view_Graphs" runat="server">
                   Start Year: <asp:TextBox ID="TextBox_startYear" runat="server" 
                        Width="50px" ontextchanged="TextBox_startYear_TextChanged" 
                        AutoPostBack="True" CausesValidation="True" ValidationGroup="AllValidators"></asp:TextBox>
                    <asp:RangeValidator ID="RangeValidator_startYear" runat="server" 
                        ErrorMessage="Please enter a valid year" ControlToValidate="TextBox_startYear" 
                        MaximumValue="1" MinimumValue="0" SetFocusOnError="True" Type="Integer" 
                        ValidationGroup="AllValidators">*</asp:RangeValidator>&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:CheckBox ID="Check_autoScale" runat="server" AutoPostBack="True" 
                        Text="Auto Scale" Checked="True" 
                        oncheckedchanged="Check_autoScale_CheckedChanged" />&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_minY" runat="server" 
                        ErrorMessage="Please enter a number for Min Y" ControlToValidate="TextBox_minY" 
                        SetFocusOnError="True" ValidationGroup="AllValidators">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator_minY" 
                        runat="server" ErrorMessage="Please enter a number for Min Y" 
                        ControlToValidate="TextBox_minY" SetFocusOnError="True" 
                        ValidationExpression="^[+-]?\d+(\.\d+)?$" ValidationGroup="AllValidators">*</asp:RegularExpressionValidator>
                    <asp:CompareValidator ID="CompareValidator_minY" runat="server" 
                        ControlToCompare="TextBox_maxY" ControlToValidate="TextBox_minY" 
                        ErrorMessage="Min Y must be lower than Max Y" Operator="LessThan" 
                        SetFocusOnError="True" Type="Double" ValidationGroup="AllValidators">*</asp:CompareValidator>
                    Min Y: 
                    <asp:TextBox ID="TextBox_minY" runat="server" 
                        Width="50px" AutoPostBack="True" ontextchanged="TextBox_minY_TextChanged" 
                        Enabled="False" CausesValidation="True" ValidationGroup="AllValidators"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp; 
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_maxY" runat="server" 
                        ErrorMessage="Please enter a number for Max Y" ControlToValidate="TextBox_maxY" 
                        SetFocusOnError="True" ValidationGroup="AllValidators">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator_maxY" 
                        runat="server" ErrorMessage="Please enter a number for Max Y" 
                        ControlToValidate="TextBox_maxY" SetFocusOnError="True" 
                        ValidationExpression="^[+-]?\d+(\.\d+)?$" ValidationGroup="AllValidators">*</asp:RegularExpressionValidator>
                    Max Y: 
                    <asp:TextBox ID="TextBox_maxY" runat="server" 
                        Width="50px" AutoPostBack="True" ontextchanged="TextBox_maxY_TextChanged" 
                        Enabled="False" CausesValidation="True" ValidationGroup="AllValidators"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:CheckBox ID="Check_minMax" runat="server" AutoPostBack="True" 
                        Text="Show Min/Max Lines" Checked="True" Visible="False"
                        oncheckedchanged="Check_minMax_CheckedChanged"  /><br /><br />
                    <div id="outChart">
                        <asp:Chart ID="Chart1" runat="server" Width="860px" Height="510px">
                            <ChartAreas><asp:ChartArea Name="ChartArea1"><AxisX Title="Year of Simulation"></AxisX></asp:ChartArea></ChartAreas></asp:Chart>
                    </div>
                    <div style="text-align:center">To export this image right click and choose 'copy image' or 'save image as'. See notes for this step.</div>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                        ShowMessageBox="True" ValidationGroup="AllValidators" />
                </asp:View>
                
                <asp:View ID="view_Files" runat="server">
                    <asp:Label ID="Label1" runat="server"></asp:Label>
                    <br /><br />
                    <asp:TextBox ID="TextBox1" runat="server" Font-Names="Courier New" 
                        Height="500px" ReadOnly="True" TextMode="MultiLine" Width="860px" Wrap="False"></asp:TextBox>
                    <asp:TextBox ID="TextBox_save" runat="server" Visible="False" ReadOnly="True" TextMode="MultiLine" Wrap="False"></asp:TextBox><br />
                </asp:View>
            </asp:MultiView>
        </div>
        </form>
    </div>
    <script type= "text/javascript">loadFooter();</script>
  </div>
</body>
</html>
