// script_carbon.js
// Keith Olsen - 25 August 2010
//
// For use with the carbon calculator website

// Google analytics code
var _gaq = _gaq || [];
_gaq.push(['_setAccount', 'UA-23764532-1']);
_gaq.push(['_trackPageview']);

(function () {
    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
})();

function setHourglass() {
    //document.body.style.cursor = "wait";
    document.getElementsByTagName("html")[0].style.cursor = 'wait';
}

// *********** Navigation
function loadNavMenu()
{
    document.writeln('<div><a href="default.aspx"><img src="images/banner_bw.jpg" alt="The Forest Sector Carbon Calculator" border="0" /></a></div>');
    document.writeln('<div class="horizontalcssmenu">');
    document.writeln('<div align="center">');
    document.writeln('<ul id="cssmenu1">');
    document.writeln('<li style="border-left: 1px solid #BBB;"><a href="default.aspx">Home</a></li>');
    document.writeln('<li><a href="#">Overview</a>');
    document.writeln('<ul>');
    document.writeln('<li><a href="summary.aspx">Summary</a></li>');
    document.writeln('<li><a href="people.aspx">About Us</a></li>');
    document.writeln('<li><a href="glossary.aspx">Glossary</a></li>');
    document.writeln('<li><a href="docs.aspx">Documentation</a></li>');
    document.writeln('<li><a href="whatsNew.aspx">New Features</a></li>');
    // document.writeln('<li><a href="contact.aspx">Provide Feedback</a></li>');
    document.writeln('</ul></li>');
    document.writeln('<li><a href="tutorial.aspx">Tutorial</a>');
    document.writeln('<ul>');
    document.writeln('<li><a href="tutorial-works.aspx">How it works</a></li>');
    document.writeln('<li><a href="tutorial-modules.aspx">Module overview</a></li>');
    document.writeln('<li><a href="tutorial-run.aspx">How do I run it</a></li>');
    document.writeln('<li><a href="tutorial-behave.aspx">Carbon in the forest</a></li>');
    document.writeln('<li><a href="tutorial-examples.aspx">Examples</a></li>');
    document.writeln('</ul></li>');
    document.writeln('<li><a href="run-stand.aspx">Run Stand</a></li>');
    document.writeln('<li><a href="run-landscape.aspx">Run Landscape</a></li>');
    document.writeln('<li><a href="contact.aspx">Provide Feedback</a></li>');
    document.writeln('</ul>');
    document.writeln('<br style="clear: left;" />');
    document.writeln('</div>');
    document.writeln('</div>');
}

function loadFooter() {
    document.writeln('<div id="footer"><a href="mailto:Keith.Olsen@oregonstate.edu">Contact the webmaster</a> |');
    document.writeln('<a href="http://validator.w3.org/check/referer">HTML</a> |');
    document.writeln('<a href="http://jigsaw.w3.org/css-validator/check/referer">CSS</a> |');
    document.writeln('Updated: 31 December 2014 - Version 2.3</div>');
    document.writeln('<div id="footer_2"></div>');    
}

function writeNavURL(scale, step) {
    if (step == 1)
        window.location = "run-both-1site.aspx";
    else if (step == 2)
        window.location = "run-both-2sim.aspx";
    else if (step == 3)
        window.location = "run-" + scale + "-3history.aspx";
    else if (step == 4)
        window.location = "run-" + scale + "-4landuse.aspx";
    else if (step == 5)
        window.location = "run-both-5products.aspx";
}

function openWaitWindow() {
//    window.open("run-waitWindow.aspx", "WaitWindow", "height=505,width=657");
    window.open("run-waitWindow.aspx", "_blank", "toolbar=yes, location=yes, directories=no, status=no, menubar=yes, scrollbars=yes, resizable=no, copyhistory=yes, width=657, height=505");

//    var props = {
//        url: "http://www.stackoverflow.com",
//        height: "100",
//        width: "100",
//        type: "popup"
//    }

//    chrome.windows.create(props, function (windowObj) {
//        console.log("Here's the window object.");
//        console.dir(windowObj);
//    });

////    OpenWindow = window.open("", "FSCC_busy", "height=505, width=657,toolbar=no,scrollbars=no,menubar=no,location=no,resizable=no", false);
//    OpenWindow.document.write("<title>Forest Sector Carbon Calculator</title>");
//    OpenWindow.document.write("<body>");
//    OpenWindow.document.write("<script type='text/javascript'>");
//    OpenWindow.document.write("setTimeout('window.top.opener = null; window.close();', 30000)");
//    OpenWindow.document.write("</script>");
//    OpenWindow.document.write("<div>");
//    OpenWindow.document.write("<h1>Please wait. The simulation is running.</h1>");
//    OpenWindow.document.write("<h2>When the simulation is finished, the output page will appear</h2>");
//    OpenWindow.document.write("</div>")
//    OpenWindow.document.write("<p>&nbsp;</p><div style='text-align: center;'><img alt='Busy. Please wait.' src='images/busy.gif' align='middle' /></div>")
//    OpenWindow.document.write("</body>")
//    OpenWindow.document.write("</html>")

//    OpenWindow.document.close()
//    self.name = "main"

    window.location = "run-both-output.aspx";
}

function formatScaleName(scale) {
    if (scale == "stand")
        document.write("Stand");
    else if (scale == "landscape")
        document.write("Landscape");
}
function formatScaleNameBody(elID, scaleID) {
    if (document.getElementById(scaleID).value == 'stand')
        document.getElementById(elID).innerHTML = 'Stand';
    else if (document.getElementById(scaleID).value == 'landscape')
        document.getElementById(elID).innerHTML = 'Landscape';
}

function checkParamsTitle(scale) {
    if (scale == "stand")
        document.write("Stand Disturbance and Management Events:");
    else
        document.write("Landscape Disturbance and Management Regimes:");
}

// ********************* Movie and slide show functions
function setImageFirst(SSBehaviorID) {
    SSE = $find(SSBehaviorID);
    SSE._currentIndex = 0;
    SSE.setCurrentImage();
    return false;
}

function openMovieWindow(moviePath, movieTitle) {
    OpenWindow = window.open("", null, "height=505, width=657,toolbar=no,scrollbars=no,menubar=no,location=no,resizable=no", false);
    OpenWindow.document.write("<title>" + movieTitle + "</title>");
    OpenWindow.document.write("<body BGCOLOR=black>");
    OpenWindow.document.write("<div>");
    OpenWindow.document.write("<object width='640px' height='480px' type='video/x-ms-wmv' data=" + moviePath + ">");
    OpenWindow.document.write("<param name='src' value=" + moviePath + " />");
    //OpenWindow.document.write("<param name='autostart' value='0' />")
    OpenWindow.document.write("<param name='controller' value='0' />")
    OpenWindow.document.write("<strong>Error:</strong> Embedding <a href=" + moviePath + ">" + moviePath + "</a> with the <a href='http://port25.technet.com/pages/windows-media-player-firefox-plugin-download.aspx'>Windows Media Plugin</a> failed.");
    OpenWindow.document.write("</object>")
    OpenWindow.document.write("</div>")
    OpenWindow.document.write("</body>")
    OpenWindow.document.write("</html>")

    OpenWindow.document.close()
    self.name = "main"
}

// ****************** Help funcitons
function removeContent(elID, viewID, hideID) {
    document.getElementById(elID).style.display = "none";
    document.getElementById(viewID).style.display = "";
    document.getElementById(hideID).style.display = "none";
}

function insertContent(elID, viewID, hideID) {
    document.getElementById(elID).style.display = "";
    document.getElementById(viewID).style.display = "none";
    document.getElementById(hideID).style.display = "";
}

function helpOwnership() {
    alert("By selecting the ownership of the forest, you are determining the default disturbance and management history that was most likely in the region you are examining.");
}
function helpElevationClass() {
    alert("By selecting the elevation class you are determining many factors that influence the forest you are examining.  This includes the climate, soil, species of trees present, and the disturbance history (e.g., fires).  Of course these are approximations and your particular forest may differ from that selected based on the elevation class.");
}
function helpCalendarYear()
{
    alert("Think of this as the year you wish to begin your experiment.");
}
function helpRand()
{
    alert("If this option does not apply to you, please accept the default (No). The same random number seed will be used for each model run.");
}
function helpCellSize() {
    alert("The landscape in the calculator consists of a fixed number of stand grid cells (260). By setting the size of an individual stand to a value between (1 and 100 ha) you can control the absolute size of the landscape (260 ha to 26,000 ha). This choice will affect the temporal variability of outputs related to wildfires:  small stand grid cells and landscapes will exhibit more variation for a given wildfire regime than larger stand grid cells and landscapes.");
}
function helpStartYear() {
    alert("Select an historical event year between the reference calendar year and 600 years prior. This is sometimes referred to as the 'spinup' time span.");
}
function helpWildfireLevel() {
    alert("For wildfires, the intervals, percent disturbed and severity are based on an analysis of historical trends in the selected region. The wildfire suppression level can be set to either 'total suppression' (no wildfires are generated), 'no suppression' (wildfires are generated at estimated pre-european settlement levels), or 'typical suppression' (wildfires are generated at 2.5x the 'no suppression' fire return interval to simulate the suppressed wildfire of recent history).");
}
function helpWildfireSize() {
    alert("The size of fires may be varied from the most likely condition based on the region and elevation class selected.  Fire size can either be halved or doubled relative to the default value for a region and elevation class.  Bear in mind that fire size is described by a distribution, so doubling fire size will increase the chances of a very large fire, whereas halving it will reduce the chances of a very large fire.  Since individual fire size is random, one could have very large fires under either scenario.");
}
function helpDefaultSubstitution() {
    alert("If you answer yes to this question, then you are accepting the default assumptions regarding product substitution being used: 1)  the displacement factor will decrease by 50% in the next 50 years; 2) there is a relationship between product substitution and long-term structures with the former is lost at the same rate as the latter; and 3)  that market forces will eventually lead to the use of the displaced fossil carbon over a period of 100 years. If you answered no to this question, then you will be directed to a new page where you can alter the assumptions.");
}
function helpDefaultBioenergy() {
    alert("If you answered yes to this question, then you are accepting the default assumptions regarding biofuel substitution being used: 1) the bioenergy displacement factor is 60%, that is for every ton of biofuel carbon used, 0.6 tons of fossil carbon are not used; and 2)  market forces will eventually lead to the use of the displaced fossil carbon over a period of 100 years. If you answered no to this question, then you will be directed to a new page where you can alter the assumptions.");
}
function helpSubDisplacement() {
    alert("The displacement factor is the tons of fossil carbon that is not used because a ton of long-term wood products is used. As energy use to make products changes, the displacement factor changes.  If you answer yes to this question, then we assume that the current displacement value of 1.1 MgC fossil carbon per 1 MgC of long-term structures is used into the future.  If you answer no (default), then we assume that the displacement factor will decrease by 50% in the next 50 years to reflect the greater improvements in energy use and use of lower carbon fuels in the manufacturing of non-wood building materials such as concrete and steel.");
}
function helpSubStructures() {
    alert("If you answer yes (default) then there is a relationship between product substitution and long-term structures with the former is lost at the same rate as the latter. Product substitution is therefore assumed to be impermanent and must be maintained by replacing long-term structures and will only increase when the amount of long-term structures increases. If you answer no, then there is no relationship, implying that regardless of how long long-term structures last production substitution displacement is permanent.  This means that product substitution can only increase over time.");
}
function helpSubFactor() {
    alert("The bioenergy displacement factor is the fraction of fossil carbon not used because one ton of bioenergy carbon is used.  It is expressed as a percent and has values between 0 and 90%. It is lower than 100% because bioenergy contains less energy per unit carbon than fossil carbon. This means that some fossil energy will be required to supply the full amount of energy; hence some fossil carbon is needed and released. It varies so much because the net energy released per unit bioenergy burned depends on the fossil carbon being displaced (e.g., natural gas is half the value of coal), the moisture of the fuel stock (i..e, the wetter the fuel the less net energy), energy lost in converting the raw fuel stock to the final fuel, and energy lost in transportation.  The default bioenergy displacement factor is 60%, which is based on coal being the fossil carbon being replaced and a moderately high efficiency of energy capture.");
}
function helpSubLeakage() {
    alert("This question addresses your assumption about the economic process of leakage. If you answered no, then you are assuming that fossil carbon that is not used because of product and bioenergy substitution will never be used in the future.  Due to the economic behavior of the fossil carbon sector, this assumption is unlikely to be met unless something discourages fossil carbon use (e.g., either reguations or taxes). Less fossil carbon use would lead to more supply and more supply might lead to lower prices, and that ultimately could lead to the displaced carbon being used.  If you answered yes (default), then you are assuming that market forces will eventually lead to the use of the displaced fossil carbon over a period of 100 years.");
}

// ******************* Validation functions
function handleEnterKey(e) {
    var key;
    if (window.event)
        key = window.event.keyCode; //IE
    else
        key = e.which; //firefox     

    return (key != 13);
}

function IsNumeric(sText)
{
    var ValidChars = "-.0123456789";
    var IsNumber = true;
    var Char;

    if (sText.length == 0)
        IsNumber = false;
  
    for (i = 0; i < sText.length && IsNumber == true; i++) 
    { 
        Char = sText.charAt(i); 
        if (ValidChars.indexOf(Char) == -1) 
            IsNumber = false;
    }
    return IsNumber;
}

function InRange(sText, minVal, maxVal)
{
    if (parseFloat(sText) < minVal || parseFloat(sText) > maxVal)
        return(0);
    else
        return(1);
}

// enter  (onblur="checkValue(this.value)") in asp:textbox
function checkValue(inValue)
{
    if (!IsNumeric(inValue))
        alert ("Please enter a number");

    if (!InRange(inValue, 0, 10000))
        alert ("Please enter a value in the range 0 - 10000");
}

// from step 5 stand level
function checkValueAndTotal(inValue, groupName)
{
    var errorCode = 0;
    if (!IsNumeric(inValue))
    {
        alert ("Please enter a number");
        errorCode = 1;
    }
    
    if (!InRange(inValue, 0.0, 100.0))
    {
        alert ("Please enter a value in the range 0 - 100");
        errorCode = 1;
    }
        
    if (groupName == "mfg")
        calculateMfgTotal(errorCode);
    else if (groupName == "product")
        calculateProductTotal(errorCode);
    else if (groupName == "disposal")
        calculateDisposalTotal(errorCode);
}

function clearFileUpload(elID) {
    var who = document.getElementById(elID);
    who.value = "";

    var who2 = who.cloneNode(false);
    who2.onchange = who.onchange;
    who.parentNode.replaceChild(who2, who);
}

// for step 3 and 4
function disableTxtBox(offsetVal, disturbID, disturbLabelID, disturbPriID) {
    if (offsetVal == "0") {
        document.getElementById(disturbID).disabled = true;
        document.getElementById(disturbLabelID).style.color = "gray";
        document.getElementById(disturbID).value = document.getElementById(disturbPriID).value;
    }
    else {
        document.getElementById(disturbID).disabled = false;
        document.getElementById(disturbLabelID).style.color = "black";
    }
}

function script2() {
    return confirm('Hello!');
}
