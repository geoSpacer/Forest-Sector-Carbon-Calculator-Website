<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tutorial-behave.aspx.cs" Inherits="tutorial_behave" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
  <title>The Forest Sector Carbon Calculator</title>
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <link href="css/carbon.css" type="text/css" rel="stylesheet" />
  <link href="css/sb_minitabs.css" type="text/css" rel="stylesheet" />
  <link href="css/csshorizontalmenu.css" type="text/css" rel="stylesheet" />
  <script src="css/csshorizontalmenu.js" type="text/javascript"></script>
  <script src="css/script_carbon.js" type="text/javascript"></script>

</head>

<body>
  <form id="form1" runat="server">
  <div id="mainPageDiv">
    <script type= "text/javascript">loadNavMenu();</script>
    <noscript><br /><h1 style="text-align:center;">The Forest Sector Carbon Calculator</h1>
        <h1 style="text-align:center; color:Red;">Please Enable Javascript Before Proceeding</h1></noscript>

	<div id="main">
        <div id="mainPageTitle">How does carbon behave in the forest sector?</div>
        <div style="color:Red">WARNING: Movies and slideshows describe the previous software version. Updates coming soon... </div>

        <p>&nbsp;</p>
        <h3>Stand vs. Landscape Carbon Dynamics - <a href="javascript:openMovieWindow('images/movies/stand-landscape-c-dynamics.wmv', 'Stand vs. Landscape Carbon Dynamics')">(view)</a></h3>

        <p>&nbsp;</p>
            This presents results of a number of experiments that illustrate how the processes controlling carbon in the forest sector work. You can repeat the experiments as you learn how 
            to run the calculator.
            
        <h3>How are stand- versus landscape-level analyses related?</h3>
            This explores how one can transform stand-level analyses into the landscape-level.
        <h3>Which carbon pools need to be considered?</h3>
            In some analyses the dead and soil carbon pools are not considered or assumed to remain constant. What are the consequences of these assumptions?
        <h3>The importance of the starting point</h3>
            Additionality is an important concept when assessing the outcome of any management action.  Additionality means that to count management must deviate from business as usual; thus 
            knowing the starting point (i.e., the usual business) is essential.
        <h3>Storing carbon in the forest versus wood products</h3>
            One can store carbon in the forest itself or in wood products derived from harvest of the forest. What are the trade-offs between these two parts of the forest sector?
        <h3>Fire frequency versus severity</h3>
            There is usually a trade-off between fire frequency and fire severity. That is it is difficult to maintain frequent, high severity fires over the long-term. And it is also 
            difficult to have only low severity fires when the fire frequency is low (because more fuel builds up).  How do these two aspects of fire disturbance interact?
        <h3>Harvest interval and removal levels</h3>
            When one decides to harvest one can vary the interval of harvest and the amount removed independently. How do these factors interact to determine the amount of carbon stored 
            in the forest sector?
         
    </div>
    <script type= "text/javascript">loadFooter();</script>
  </div>

    </form>

</body>
</html>
