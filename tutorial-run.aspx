<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tutorial-run.aspx.cs" Inherits="tutorial_run" %>
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
        <div id="mainPageTitle">How do I run the calculator?</div>
        <div style="color:Red">WARNING: Movies and slideshows describe the previous software version. Updates coming soon... </div>
        <p>&nbsp;</p>
        <ul>
        <li><h3>Stand Level Overview Movie - <a href="javascript:openMovieWindow('images/movies/stand-walk-through3.wmv', 'Stand Level Overview')">(view)</a></h3></li>
        <li><h3>Landscape Level Overview Movie - <a href="javascript:openMovieWindow('images/movies/landscape-walk-through4.wmv', 'Landscape Level Overview')">(view)</a></h3></li>
        <li><h3>Navigation Overview Movie - <a href="javascript:openMovieWindow('images/movies/navigation2.wmv', 'Navigation Overview')">(view)</a></h3></li>
        </ul>

        <p>&nbsp;</p>
        <h3>Stand Level Overview Slideshow</h3>
         <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" 
         	runat="server" />
         <div style="text-align: center">
            <asp:Label runat="Server" ID="imageTitle" /> <br />
             <asp:Image ID="ImageSlide" runat="server" 
             	Style="border: 1px solid black; width: auto" 
                ImageUrl="images/slides/stand/stand-slide-1.jpg" 
                AlternateText="Stand Level Parameterization" />
             <ajaxToolkit:SlideShowExtender ID="SlideShowExtender1" 
             	runat="server" 
                TargetControlID="ImageSlide"
                SlideShowServiceMethod="GetSlides" 
                AutoPlay="false" 
                ImageTitleLabelID="imageTitle"
                NextButtonID="nextButton" 
                PlayButtonText="Play"
                StopButtonText="Stop" 
                PreviousButtonID="prevButton" 
                PlayButtonID="playButton"
                Loop="true" 
                BehaviorID="slideShowEx1" 
                PlayInterval="5000" />
                
             <div class="SSControlsFirst">
                    <input id="firstButton" type="button" value="First Slide" onclick="setImageFirst('slideShowEx1')" style="font-size: x-large; height: 45px" />
             </div>
             <table class="SSControls" cellpadding="0" cellspacing="10">
                <tr><td>            
             <asp:ImageButton runat="Server" ID="prevButton"
             	Text="Prev" Font-Size="Larger" ImageUrl="~/images/Back_h.gif" />
                </td><td>
             <asp:Button runat="Server" ID="playButton" 
             	Text="Play" Font-Size="X-Large" Height="45px" Width="70px" />
                </td><td>
             <asp:ImageButton runat="Server" ID="nextButton" 
             	Text="Next" Font-Size="Larger" ImageUrl="~/images/Forward_h.gif" />
                </td></tr>
             </table>
         </div>

         <p>&nbsp;</p>
        <h3>Landscape Level Overview Slideshow</h3>

            <div style="text-align: center">
            <asp:Label runat="Server" ID="imageTitle_Land" /> <br />
             <asp:Image ID="ImageSlide_Land" runat="server" 
             	Style="border: 1px solid black; width: auto" 
                ImageUrl="images/slides/landscape/landscape-slide-1.jpg" 
                AlternateText="Landscape Level Parameterization" />
             <ajaxToolkit:SlideShowExtender ID="SlideShowExtender2" 
             	runat="server" 
                TargetControlID="ImageSlide_Land"
                SlideShowServiceMethod="GetSlides_Landscape" 
                AutoPlay="false" 
                ImageTitleLabelID="imageTitle_Land"
                NextButtonID="nextButton_Land" 
                PlayButtonText="Play"
                StopButtonText="Stop" 
                PreviousButtonID="prevButton_Land" 
                PlayButtonID="playButton_Land"
                Loop="true" 
                BehaviorID="slideShowEx2" 
                PlayInterval="5000" />
                
             <div class="SSControlsFirst">
                 <asp:Button ID="firstButton_Land" runat="server" Text="First Slide" Height="45" Font-Size="X-Large" />
             </div>
             <table class="SSControls" cellpadding="0" cellspacing="10">
                <tr><td>
             <asp:ImageButton runat="Server" ID="prevButton_Land"
             	Text="Prev" Font-Size="Larger" ImageUrl="~/images/Back_h.gif" />
                </td><td>
             <asp:Button runat="Server" ID="playButton_Land" 
             	Text="Play" Font-Size="X-Large" Height="45px" Width="70px" />
                </td><td>
             <asp:ImageButton runat="Server" ID="nextButton_Land" 
             	Text="Next" Font-Size="Larger" ImageUrl="~/images/Forward_h.gif" />
                </td></tr>
             </table>
         </div>
         
    </div>
    <script type= "text/javascript">loadFooter();</script>
    </div>

    </form>

</body>
</html>
