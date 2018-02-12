<%@ Page Language="C#" AutoEventWireup="true" CodeFile="run-both-5substitution.aspx.cs"
    Inherits="run_both_5substitution" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
                <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 4)">4. Future Land-use Management</a></li>
                <li><a href="javascript:writeNavURL('<%=HttpContext.Current.Session["scale"]%>', 5)" class="active">5. Carbon Use</a></li>
            </ul>
        </div>
        <div id="mainPageTitle" style="margin-left: 20px">
            <span><a href="javascript:insertContent('stepNotes', 'notesView', 'notesHide')" id="notesView">
                View</a><a href="javascript:removeContent('stepNotes', 'notesView', 'notesHide')" id="notesHide" style="display: none;">
                Hide</a> notes for this step </span>Post-harvest Substitution - 
                <script type="text/javascript">formatScaleName('<%=HttpContext.Current.Session["scale"]%>')</script>
        </div>
        <div id="main">
            <div id="stepNotes" style="display: none">
                To estimate the effects of product substitution on the forest sector carbon balance you will need to decide on several key assumptions.
                <p><strong>Is the displacement factor constant in the future?</strong> - The displacement factor is the tons of fossil carbon that is not used because a ton of long-term 
                wood products is used. As energy use to make products changes, the displacement factor changes.  If you answer yes to this question, then we assume 
                that the current displacement value of 1.1 MgC fossil carbon per 1 MgC of long-term structures is used into the future.  If you answer no, then we 
                assume that the displacement factor will decrease by 50% in the next 50 years to reflect the greater improvements in energy use and use of lower 
                carbon fuels in the manufacturing of non-wood building materials such as concrete and steel. </p>
                <p><strong>Is there a relationship with long-term structures?</strong> - If you answered yes then there is a relationship between product substitution and long-term structures 
                with the former is lost at the same rate as the latter. Product substitution is therefore assumed to be impermanent  and must be maintained by replacing 
                long-term structures and will only increase when the amount of long-term structures increases. If you answered no, then there is no relationship, implying 
                that regardless of how long long-term structures last production substitution displacement is permanent.  This means that product substitution can only 
                increase over time.   </p>
                <p><strong>Default bioenergy displacement factor</strong> - The bioenergy displacement factor is the fraction of fossil carbon not used because one ton of bioenergy carbon is used.  
                It is expressed as a percent and has values between 0 and 90%. It is lower than 100% because bioenergy contains less energy per unit carbon than fossil carbon. 
                This means that some fossil energy will be required to supply the full amount of energy; hence some fossil carbon is needed Iand released. It varies so much 
                because the net energy released per unit bioenergy burned depends on the fossil carbon being displaced (e.g., natural gas is half the value of coal), the 
                moisture of the fuel stock (i..e, the wetter the fuel the less net energy), energy lost in converting the raw fuel stock to the final fuel, and energy lost 
                in transportation.  The default bioenergy displacement factor is 60%, which is based on coal being the fossil carbon being replaced and a moderately high 
                efficiency of energy capture. </p>
                <p><strong>Is there fossil carbon leakage?</strong> - This question addresses your assumption about the economic process of leakage. If you answered no, then you are assuming 
                that fossil carbon that is not used because of product and bioenergy substitution will never be used in the future.  Due to the economic behavior of the fossil 
                carbon sector, this assumption is unlikely to be met unless something discourages fossil carbon use (e.g., either reguations or taxes). Less fossil carbon use would 
                lead to more supply and more supply might lead to lower prices, and that ultimately could lead to the displaced carbon being used.  If you answered yes, then you 
                are assuming that market forces will eventually lead to the use of the displaced fossil carbon over a period of 100 years.    </p>

            </div>
            <div id="inputForm">
                <form id="step6input" runat="server" method="post" defaultbutton="btn6next" defaultfocus="textExtBio">
                <table width="90%" cellspacing="10">
                    <tr>
                        <td width="50%" colspan="2" align="center">
                            <h3>
                                Product Substitution</h3>
                    </tr>
                    <tr>
                        <td>
                            Is the <a href="glossary.aspx#displacementFactor">displacement factor</a> constant in the future?&nbsp; <a href="#" onclick="helpSubDisplacement()">
                            &nbsp;<asp:Image ID="img_help_displacement" 
                                runat="server" ImageUrl="~/images/help-about-icon.png" AlternateText="help" /></a>
                        </td>
                        <td align="right">
                            <asp:RadioButtonList ID="rb_subDisplacement" runat="server" 
                                RepeatDirection="Horizontal">
                                <asp:ListItem>Yes</asp:ListItem>
                                <asp:ListItem Selected="True">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Is there a relationship with long-term structures?&nbsp; <a href="#" onclick="helpSubStructures()">
                            &nbsp;<asp:Image ID="img_help_structures" 
                                runat="server" ImageUrl="~/images/help-about-icon.png" AlternateText="help" /></a>
                        </td>
                        <td align="right">
                            <asp:RadioButtonList ID="rb_subBuilding" runat="server" 
                                RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True">Yes</asp:ListItem>
                                <asp:ListItem>No</asp:ListItem>
                            </asp:RadioButtonList>
                         </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <h3>
                                Bioenergy Substitution</h3>
                        </td>
                    </tr>
                     <tr>
                        <td>
                            Displacement factor for bioenergy
                            <asp:RangeValidator ID="valRange_displacement" runat="server" 
                                ControlToValidate="txt_bioenergyDisp" 
                                ErrorMessage="Please enter a displacement factor between 0 and 90." 
                                MaximumValue="90" MinimumValue="0" Type="Integer" 
                                ValidationGroup="allValidators">*</asp:RangeValidator>
                            &nbsp; <a href="#" onclick="helpSubFactor()">
                            &nbsp;<asp:Image ID="img_help_bioenergy_factor" 
                                runat="server" ImageUrl="~/images/help-about-icon.png" AlternateText="help" /></a>
                        </td>
                        <td align="right">
                            <asp:TextBox ID="txt_bioenergyDisp" runat="server" CssClass="formText50">60</asp:TextBox>%
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <h3>
                                Fossil Carbon Leakage</h3>
                        </td>
                    </tr>
                     <tr>
                        <td>
                            Is there fossil carbon <a href="glossary.aspx#leakage">leakage</a>?&nbsp; <a href="#" onclick="helpSubLeakage()">
                            &nbsp;<asp:Image ID="img_help_leakage" 
                                runat="server" ImageUrl="~/images/help-about-icon.png" AlternateText="help" /></a>
                        </td>
                        <td align="right">
                            <asp:RadioButtonList ID="rb_leakage" runat="server" 
                                RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True">Yes</asp:ListItem>
                                <asp:ListItem>No</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="ValidationSummary" runat="server" ShowMessageBox="True"
                    ValidationGroup="allValidators" ShowSummary="False" />
                <br />
                <div style="float:right;">
                </div>                
                <asp:Button ID="btn6previous" class="formButton" runat="server" Text="Previous" 
                    OnClick="btn6previous_Click" TabIndex="11" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btn6next" class="formButton" runat="server" Text="Next" OnClick="btn6next_Click"
                    ValidationGroup="allValidators" />
               
                </form>
            </div>
        </div>
    <script type= "text/javascript">loadFooter();</script>
    </div>
</body>
</html>
