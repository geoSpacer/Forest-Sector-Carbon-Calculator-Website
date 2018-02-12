<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tutorial-examples.aspx.cs" Inherits="tutorial_examples" %>

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
    <form id="form1" runat="server">
  <div id="mainPageDiv">
    <script type="text/javascript">loadNavMenu();</script>
    <noscript><br /><h1 style="text-align:center;">The Forest Sector Carbon Calculator</h1>
        <h1 style="text-align:center; color:Red;">Please Enable Javascript Before Proceeding</h1></noscript>

    <div id="main">
        <div id="mainPageTitle">Examples of Carbon Calculator Runs</div>
        <br />
        Note: To run an example yourself follow these steps. First click 
        &#39;download output files&#39; for the run your are intested in and save the output files on your computer. Next, <a href="http://landcarb.forestry.oregonstate.edu/run-landscape.aspx">navigate</a>
        to the 'Run Landscape' page and select 'Load Existing Run'. On the Load Existing Run page, click browse and select the 'FSCC_landscape_parameters.xml' file in the output files you downloaded to your computer. You can then progress 
        through the parameterization pages with all the parameters set. Select 'Run Model' to run the example and view the output graphs.
        <h3>Landscape Example 1</h3>
        <ul>
            <li>Eastern Cascades Region, Federal Lands, Mid Elevation Class</li>
            <li>Run time 400 years from a start year of 2012</li>
            <li>No historical harvests or fires</li>
            <li>No Future harvests or fires</li>
            <li>
                <asp:LinkButton ID="btn_example1output" runat="server" onclick="btn_example1output_Click">Download Output Files</asp:LinkButton>
            </li>
        </ul>
        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/exampleRuns/Run1totalEcosystemStores.jpg" />
        <p>&nbsp;</p>
        <h3>Landscape Example 2</h3>
        <ul>
            <li>Eastern Cascades Region, Federal Lands, Mid Elevation Class</li>
            <li>Run time 400 years from a start year of 2012</li>
            <li>Default historical harvests and wildfires</li>
            <li>Continuing with default harvest and wildfires into the future</li>
            <li>
                <asp:LinkButton ID="btn_example2output" runat="server" onclick="btn_example2output_Click">Download Output Files</asp:LinkButton>
            </li>
        </ul>
        <asp:Image ID="Image2" runat="server" ImageUrl="~/images/exampleRuns/Run2totalEcosystemStores.jpg" />
        <p>&nbsp;</p>
    </div>
    <script type= "text/javascript">loadFooter();</script>
</div>
    </form>
</body>
</html>
