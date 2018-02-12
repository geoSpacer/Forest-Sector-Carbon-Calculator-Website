<%@ Page Language="C#" AutoEventWireup="true" CodeFile="run-stand.aspx.cs" Inherits="run_stand" %>

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
  <script src="css/script_carbon.js" type="text/javascript"></script>
</head>

<body onunload="setHourglass();">
    <form id="form1" runat="server" defaultbutton="btn0next">
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

  <div id="mainPageTitle" style="margin-left:20px">Run Stand Level Simulation</div>
  <div id="main">
      This allows one to analyze how disturbance and management influence the carbon 
      stores and flows of a forest stand. By stand level we mean an area of ground 
      that has a relatively similar disturbance and land-use history. This is the most 
      common level to see results presented and in this mode the calculator will run 
      the fastest. While one can see the impact of individual disturbance and 
      management events, in this mode it can be difficult to assess the net impact of 
      multiple simultaneous actions.<br />
      <br />
      If you have a previous run saved on your computer, you can upload the model 
      parameters by selecting &#39;Load Existing Run&#39;. Select &#39;Create New run&#39; to start 
      building a model run from scratch.
    <div><br /></div>
    <table width="100%" border="0">
        <tr>
            <td><asp:Button ID="btn0next" class="formButtonLarge" runat="server" Text="Create New Run" 
                    onclick="btn0next_Click" />&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btn0load" class="formButtonLarge" runat="server" 
                    Text="Load Existing Run" onclick="btn0load_Click"  />

</td>
            <td>
            </td>
        </tr>
    </table>
    <div><br /></div>
  </div>
    <script type= "text/javascript">loadFooter();</script>
</div>
    </form>
</body>
</html>
