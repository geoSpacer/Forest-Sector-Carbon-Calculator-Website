<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="run-both-1site.aspx.cs" Inherits="run_both_1site_code" %>

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
  <!-- <script src="css/script_step1.js" type="text/javascript" language="javascript"></script> -->
</head>
<body onunload="setHourglass()">
<div id="mainPageDiv">
    <script type="text/javascript">loadNavMenu();</script>
    <noscript><br /><h1 style="text-align:center;">The Forest Sector Carbon Calculator</h1>
        <h1 style="text-align:center; color:Red;">Please Enable Javascript Before Proceeding</h1></noscript>

  <div class="container">
    <ul class="minitabs">
        <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 1)" class="active">1. Site Selection</a></li>
        <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 2)">2. Simulation Characteristics</a></li>
        <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 3)">3. Disturbance History</a></li>
        <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 4)">4. Future Land-use Management</a></li>
        <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 5)">5. Carbon Use</a></li>
    </ul>
  </div>
  
  <div id="mainPageTitle" style="margin-left:20px">
       <span><a href="javascript:insertContent('stepNotes', 'notesView', 'notesHide')" id="notesView">
            View</a><a href="javascript:removeContent('stepNotes', 'notesView', 'notesHide')" id="notesHide" style="display: none;">
            Hide</a> notes for this step </span>Site Selection - 
            <script type="text/javascript">formatScaleName('<%=HttpContext.Current.Session["scale"]%>')</script>
  </div>

  <div id="main">
      <div id="stepNotes" style="display:none">Please select the region, ownership, and elevation interval for the site 
      that you wish to examine. Elevation intervals are equal. A drop down list will help you make a selection. This 
      helps to determine which species, climate, soils, and land-use history the model 
      should use for your simulations. See the map for supported regions.</div>
    <div id="inputForm">
        <form id="step1input" runat="server" method="post" defaultbutton="btn1next">
        <!-- <img src="images/ecoregion_map.jpg" height="400px" align="right" border="1" style="margin-bottom: 20px" alt="Available Ecoregions" /> -->
        <asp:ImageMap ID="ImageMap_ecoregions" runat="server" 
            ImageUrl="images/ecoregion_map.jpg" BorderStyle="Solid" BorderWidth="1" 
            Height="400px" ImageAlign="Right" HotSpotMode="PostBack" 
            onclick="ImageMap_ecoregions_Click" CssClass="imageBottomMargin">
            <asp:PolygonHotSpot AlternateText="Western Cascades" 
                Coordinates="82,302,72,264,76,213,118,147,142,122,167,121,129,214,101,286" 
                HotSpotMode="PostBack" PostBackValue="westCasc" />
            <asp:PolygonHotSpot AlternateText="Eastern Cascades" 
                Coordinates="170,325,84,303,102,286,133,211,168,120,181,129,149,197,169,237,149,244,175,302" 
                HotSpotMode="PostBack" PostBackValue="eastCasc" />
        </asp:ImageMap>
        <table width="50%" cellspacing="10">
            <tr>
                <td width="300px">
                    Select region to run
                    <asp:RequiredFieldValidator
                        ID="valReq_region" runat="server" ControlToValidate="ddl_region"
                        ErrorMessage="Please select a region from the list" ValidationGroup="AllValidators" 
                        SetFocusOnError="True" InitialValue="selectRegion">*</asp:RequiredFieldValidator>
                            
                </td>
                <td>
                    <asp:DropDownList ID="ddl_region" runat="server"
                        AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td width="300px">
                    Select principle ownership
                    <asp:RequiredFieldValidator
                        ID="valReq_ownership" runat="server" ControlToValidate="ddl_ownership"
                        ErrorMessage="Please select an ownership from the list" ValidationGroup="AllValidators" 
                        SetFocusOnError="True" InitialValue="selectOwnership">*</asp:RequiredFieldValidator>
                            
                &nbsp;&nbsp;<a href="#" onclick="helpOwnership()"><asp:Image 
                        ID="img_help_ownership" runat="server" ImageUrl="~/images/help-about-icon.png" 
                        AlternateText="help" /></a>
                </td>
                <td>
                    <asp:DropDownList ID="ddl_ownership" runat="server"
                        AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td width="300px">
                    Select elevation class
                    <asp:RequiredFieldValidator
                        ID="valReq_elevation" runat="server" ControlToValidate="ddl_elevation"
                        ErrorMessage="Please select an elevation class from the list" ValidationGroup="AllValidators" 
                        SetFocusOnError="True" InitialValue="selectElevation">*</asp:RequiredFieldValidator>
                            
                &nbsp;&nbsp;<a href="#" onclick="helpElevationClass()"><asp:Image ID="img_help_elevationClass" runat="server" ImageUrl="~/images/help-about-icon.png" AlternateText="help" /></a>
                </td>
                <td>
                    <asp:DropDownList ID="ddl_elevation" runat="server"
                        AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                    ValidationGroup="AllValidators" ShowSummary="False" />

        <br />
        <asp:Button ID="btn1Previous" class="formButton" runat="server" onclick="btn1Previous_Click" 
                    Text="Previous" TabIndex="1" />&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btn1Next" class="formButton" runat="server" Text="Next" 
                     OnClick="btn1Next_Click" TabIndex="0" 
            ValidationGroup="AllValidators" />

    </form>

    </div>
  </div>
    <script type= "text/javascript">loadFooter();</script>
</div>
</body>
</html>