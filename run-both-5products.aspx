<%@ Page Language="C#" AutoEventWireup="true" CodeFile="run-both-5products.aspx.cs"
    Inherits="run_both_5products" %>

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

    <script type="text/javascript" language="JavaScript">
    function calculateMfgTotal(errorCode)
    {
       // alert ("Hi " + document.getElementById('<%=textStrWood.ClientID%>').value);
       if (errorCode == 0)
       {
            var valExBiofuel = parseFloat(document.getElementById('<%=textExtBio.ClientID%>').value);
            var valStrWood = parseFloat(document.getElementById('<%=textStrWood.ClientID%>').value);
            var valPulp = parseFloat(document.getElementById('<%=textPulp.ClientID%>').value);
            document.getElementById('<%=textTotalMfg.ClientID%>').value = String(valExBiofuel + valStrWood + valPulp);
       }
       else
           document.getElementById('<%=textTotalMfg.ClientID%>').value = "999";
    }

    function calculateProductTotal(errorCode)
    {  
       if (errorCode == 0)
       {
            var valLTStr = parseFloat(document.getElementById('<%=textLTStrFraction.ClientID%>').value);             
            document.getElementById('<%=textSTStrFraction.ClientID%>').value = String(100.0 - valLTStr);
            document.getElementById('<%=textTotalUse.ClientID%>').value = "100";
       }
       else
            document.getElementById('<%=textTotalUse.ClientID%>').value = "999";
    }

    function calculateDisposalTotal(errorCode)
    {
       if (errorCode == 0)
       {
            var valDump = parseFloat(document.getElementById('<%=textDump.ClientID%>').value);
            var valLandfill = parseFloat(document.getElementById('<%=textLandfill.ClientID%>').value);
            var valIncinVol = parseFloat(document.getElementById('<%=textIncinVol.ClientID%>').value);
            var valIncinBio = parseFloat(document.getElementById('<%=textIncinBioenergy.ClientID%>').value);
            document.getElementById('<%=textTotalDisposal.ClientID%>').value = String(valDump + valLandfill + valIncinVol + valIncinBio);
       }
       else
            document.getElementById('<%=textTotalDisposal.ClientID%>').value = "999";
    }
    </script>

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
                Hide</a> notes for this step </span>Post-harvest Carbon Use - 
                <script type="text/javascript">formatScaleName('<%=HttpContext.Current.Session["scale"]%>')</script>
        </div>
        <div id="main">
            <div id="stepNotes" style="display: none">
                On this page you will need to specify how the harvested carbon is treated in the future. For years earlier than the calendar reference year, 
                historical values have been used. When totals are given, they need to sum to 100%. The first step is to decide how harvested carbon will be manufactured. 
                It can be used for biofuels to offset fossil fuel carbon, to produce structural wood that will be used in, for example, buildings, or used to produce paper 
                via pulp wood production. Bear in mind that efficiency of wood manufacturing is not perfect so allocating all harvest into structural wood will produce some paper, 
                bark mulch, etc. The second step is to decide how the products that been produced will be used as either long-term structures with maximum life-spans exceeding 30 
                years (e.g., buildings) or as short-term structures with maximum life-spans shorter than that (e.g., fences, pallets, railroad ties, etc). Paper is assumed to have 
                an average life-span in use of approximately 2 years. Mulch is assumed to have an average life-span of 10 years. When products come to the end of their life they can 
                either be disposed of or recycled and reused. For each of the major product pools (paper, short-term, and long-term structures) a different rate of recycling/reuse 
                can be specified. Note that even when recycling is set to 100% some of the material is not suitable for re-use and must be disposed. When products are finally disposed 
                they can be placed in open dumps where decomposition and combustion rates are high, placed into landfills where decomposition rates are very low; they can also be incinerated 
                to reduce volume, in which case no energy is recovered or they can be incinerated with recovery of energy. The latter is a form of bioenergy that can offset fossil fuels 
                and is added into an overall biofuel offset. Finally, you can change assumptions about product and bioenergy substitutions or use the default settings. 
            </div>
            <div id="inputForm">
                <form id="step5input" runat="server" method="post" defaultbutton="btn5next" defaultfocus="textExtBio">
                <table width="90%" cellspacing="10">
                    <tr>
                        <td width="50%" colspan="2" align="center">
                            <h3>
                                Manufacturing</h3>
                        </td><td rowspan="11">&nbsp;</td>
                        <td colspan="2" align="center">
                            <h3>
                                Recycling</h3>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Bioenergy Production External Use</td>
                        <td align="right">
                            <asp:TextBox ID="textExtBio" runat="server" onblur="checkValueAndTotal(this.value, 'mfg')" 
                                onKeyPress="return handleEnterKey(event)" TabIndex="1" 
                                CssClass="formText60" ></asp:TextBox>%
                        </td>
                        <td>
                            Long Term Structure Recycling
                        </td>
                        <td align="right">
                            <asp:TextBox ID="textLTRecycle" runat="server" onblur="checkValueAndTotal(this.value, 'recycle')"
                                onKeyPress="return handleEnterKey(event)" TabIndex="4" 
                                CssClass="formText60"></asp:TextBox>%
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Structural Wood Production
                        </td>
                        <td align="right">
                            <asp:TextBox ID="textStrWood" runat="server" onblur="checkValueAndTotal(this.value, 'mfg')"
                                onKeyPress="return handleEnterKey(event)" TabIndex="2" 
                                CssClass="formText60"></asp:TextBox>%
                        </td>
                        <td>
                            Short Term Structure Recycling
                        </td>
                        <td align="right">
                            <asp:TextBox ID="textSTRecycle" runat="server" onblur="checkValueAndTotal(this.value, 'recycle')"
                                onKeyPress="return handleEnterKey(event)" TabIndex="5" 
                                CssClass="formText60"></asp:TextBox>%
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Pulp Wood Production
                        </td>
                        <td align="right">
                            <asp:TextBox ID="textPulp" runat="server" onblur="checkValueAndTotal(this.value, 'mfg')"
                                onKeyPress="return handleEnterKey(event)" TabIndex="3" 
                                CssClass="formText60"></asp:TextBox>%
                        </td>
                        <td>
                            Paper Recycling
                        </td>
                        <td align="right">
                            <asp:TextBox ID="textPaperRecycle" runat="server" onblur="checkValueAndTotal(this.value, 'recycle')"
                                onKeyPress="return handleEnterKey(event)" TabIndex="6" 
                                CssClass="formText60"></asp:TextBox>%
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Total Manufacturing<asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="textTotalMfg"
                                ErrorMessage="Total manufacturing should sum to 100%" ValidationGroup="TotalValidators"
                                ValueToCompare="100">*</asp:CompareValidator>
                        </td>
                        <td align="right">
                            <asp:TextBox ID="textTotalMfg" runat="server" ReadOnly="True" BackColor="#CCCCCC"
                                ValidationGroup="TotalValidators" CssClass="formText60"></asp:TextBox>%
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="right">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <h3>
                                Product Use</h3>
                        </td>
                        <td colspan="2" align="center">
                            <h3>
                                Disposal</h3>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Long Term Structure Fraction
                        </td>
                        <td align="right">
                            <asp:TextBox ID="textLTStrFraction" runat="server" onblur="checkValueAndTotal(this.value, 'product')"
                                onKeyPress="return handleEnterKey(event)" TabIndex="7" 
                                CssClass="formText60"></asp:TextBox>%
                        </td>
                        <td>
                            Open Dump Fraction
                        </td>
                        <td align="right">
                            <asp:TextBox ID="textDump" runat="server" ReadOnly="True" BackColor="#CCCCCC" 
                                CssClass="formText60"></asp:TextBox>%
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Short Term Structure Fraction
                        </td>
                        <td align="right">
                            <asp:TextBox ID="textSTStrFraction" runat="server" ReadOnly="True" 
                                BackColor="#CCCCCC" CssClass="formText60"></asp:TextBox>%
                        </td>
                        <td>
                            Landfill Fraction
                        </td>
                        <td align="right">
                            <asp:TextBox ID="textLandfill" runat="server" onblur="checkValueAndTotal(this.value, 'disposal')"
                                onKeyPress="return handleEnterKey(event)" TabIndex="8" 
                                CssClass="formText60"></asp:TextBox>%
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Total Product Use<asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="textTotalUse"
                                ErrorMessage="Total product use should sum to 100%" ValidationGroup="TotalValidators"
                                ValueToCompare="100">*</asp:CompareValidator>
                        </td>
                        <td align="right">
                            <asp:TextBox ID="textTotalUse" runat="server" ReadOnly="True" 
                                BackColor="#CCCCCC" CssClass="formText60"></asp:TextBox>%
                        </td>
                        <td>
                            Incineration for Volume Reduction
                        </td>
                        <td align="right">
                            <asp:TextBox ID="textIncinVol" runat="server" onblur="checkValueAndTotal(this.value, 'disposal')"
                                onKeyPress="return handleEnterKey(event)" TabIndex="9" 
                                CssClass="formText60"></asp:TextBox>%
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Default Product Substitution&nbsp; <a href="#" onclick="helpDefaultSubstitution()">
                            &nbsp;<asp:Image ID="img_help_prod_substitution" 
                                runat="server" ImageUrl="~/images/help-about-icon.png" AlternateText="help" /></a></td>
                        <td align="right">
                            <asp:RadioButtonList ID="rb_substitution" runat="server" 
                                RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True">Yes</asp:ListItem>
                                <asp:ListItem>No</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            Incineration for Bioenergy Recovery
                        </td>
                        <td align="right">
                            <asp:TextBox ID="textIncinBioenergy" runat="server" onblur="checkValueAndTotal(this.value, 'disposal')"
                                onKeyPress="return handleEnterKey(event)" TabIndex="10" 
                                CssClass="formText60"></asp:TextBox>%
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Default Bioenergy Substitution&nbsp; <a href="#" onclick="helpDefaultBioenergy()">
                            &nbsp;<asp:Image 
                                ID="img_help_bioenergy_substitution" runat="server" 
                                ImageUrl="~/images/help-about-icon.png" AlternateText="help" /></a>
                        </td>
                        <td align="right">
                            <asp:RadioButtonList ID="rb_bioenergySub" runat="server" 
                                RepeatDirection="Horizontal">
                                <asp:ListItem Selected="True">Yes</asp:ListItem>
                                <asp:ListItem>No</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            Total Disposal<asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="textTotalDisposal"
                                ErrorMessage="Total disposal should sum to 100%" ValidationGroup="TotalValidators"
                                ValueToCompare="100">*</asp:CompareValidator>
                        </td>
                        <td align="right">
                            <asp:TextBox ID="textTotalDisposal" runat="server" BackColor="#CCCCCC"
                                ReadOnly="True" ValidationGroup="TotalValidators" CssClass="formText60"></asp:TextBox>%
                        </td>
                    </tr>
                </table>
                <asp:ValidationSummary ID="ValidationSummary" runat="server" ShowMessageBox="True"
                    ValidationGroup="TotalValidators" ShowSummary="False" />
                <br />
                <div style="float:right;">
                <asp:Button ID="btn5reset" runat="server" Text="Reset Defaults" CssClass="formButton"
                    OnClick="btn5reset_Click" TabIndex="10" /></div>                
                <asp:Button ID="btn5previous" class="formButton" runat="server" Text="Previous" 
                    OnClick="btn5previous_Click" TabIndex="11" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btn5next" class="formButton" runat="server" Text="Next" OnClick="btn5next_Click"
                    ValidationGroup="TotalValidators" />
               
                </form>
            </div>
        </div>
    <script type= "text/javascript">loadFooter();</script>
    </div>
</body>
</html>
