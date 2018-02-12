/* ------------------------------
 * Keith Olsen - 27 January 2012
 * Oregon State University
 * keith.olsen@oregonstate.edu
 * ------------------------------
 */
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Drawing;

//using System.IO; // for directory listing and creation
using System.Diagnostics; // for Request
// using System.Windows.Forms; // for messagebox
using System.Text.RegularExpressions;
using System.Collections.Generic;  // for sortedList
using System.Xml;

public partial class run_landscape_3history : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            setOptions_class setOptions = new setOptions_class();
            setOptions.setPriRegimeItems(ddl_primary.Items);
            setOptions.setSecRegimeItems(ddl_secondary.Items);
            setOptions.setUtilizationItems(ddl_priUtilization.Items);
            setOptions.setUtilizationItems(ddl_secUtilization.Items);
            setOptions.setSeverityItems(ddl_priSeverity.Items);
            setOptions.setSeverityItems(ddl_secSeverity.Items);
            setOptions.setSuppressionItems(ddl_priSuppression.Items);
            setOptions.setFireSizeItems(ddl_priFireSize.Items);
            setOptions.setSecPlacementItems(ddl_secPlacement.Items);

            // check for session timeout and send user back to the beginning if timeout has occured
            if (Page.Session["scale"] == null)
                Response.Redirect("default.aspx");

            int endYear = Convert.ToInt16(Page.Session["currentYear"].ToString()) - 1;
            txt_priStartYear.Text = Page.Session["simStartYear"].ToString();
            txt_priEndYear.Text = endYear.ToString();
            valRange_priStartYear.MinimumValue = Page.Session["simStartYear"].ToString();
            valRange_priStartYear.MaximumValue = endYear.ToString();
            valRange_priStartYear.ErrorMessage = "Select a start year between " + valRange_priStartYear.MinimumValue + " and " + valRange_priStartYear.MaximumValue;
            valRange_priEndYear.MinimumValue = Page.Session["simStartYear"].ToString();
            valRange_priEndYear.MaximumValue = endYear.ToString();
            valRange_priEndYear.ErrorMessage = "Select an end year between " + valRange_priEndYear.MinimumValue + " and " + valRange_priEndYear.MaximumValue;
            valRange_secOffset.MinimumValue = "0";
            valRange_secOffset.MaximumValue = Page.Session["numSimYears"].ToString();
            valRange_secOffset.ErrorMessage = "Select secondary treatment offset between " + valRange_secOffset.MinimumValue + " and " + valRange_secOffset.MaximumValue;

            displayDictionary((SortedList<string, ArrayList>)Session["managementDict"], (SortedList<string, ArrayList>)Session["natDisturbDict"]);            
        }
    }

    protected void btn3previous_Click(object sender, EventArgs e)
    {
        Response.Redirect("run-both-2sim.aspx");
    }
    protected void btn3next_Click(object sender, EventArgs e)
    {
        Response.Redirect("run-landscape-4landuse.aspx");
    }

    protected void displayDictionary(SortedList<string, ArrayList> managementDict, SortedList<string, ArrayList> natDisturbDict)
    {
        displayDict_class displayDict = new displayDict_class();

        int simCurrentYear = Convert.ToInt16(Page.Session["currentYear"].ToString());
        string defaultFireKey = "p" + Page.Session["simStartYear"].ToString() + "to" + (simCurrentYear - 1).ToString();
        cbList_management.Items.Clear();
        cbList_natDisturb.Items.Clear();

        foreach (KeyValuePair<string, ArrayList> dictEntry in managementDict)
            if (Convert.ToInt16(dictEntry.Key.Substring(7, 4)) < simCurrentYear)
                cbList_management.Items.Add(new ListItem(displayDict.displayManagement(dictEntry), dictEntry.Key));

        foreach (KeyValuePair<string, ArrayList> dictEntry in natDisturbDict)
            if (Convert.ToInt16(dictEntry.Key.Substring(7, 4)) < simCurrentYear)
                cbList_natDisturb.Items.Add(new ListItem(displayDict.displayNatDisturb(dictEntry), dictEntry.Key));

        if (cbList_natDisturb.Items.Count == 0)
            displayDict.setNoFireDefault(cbList_natDisturb.Items, (SortedList<string, ArrayList>)Session["natDisturbDict"], defaultFireKey);
    }
    protected void ddl_primary_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddl_priSeverity.Items.FindByValue("Default").Enabled = false;
        img_help_pri_severity.Visible = false;
        img_help_pri_suppression.Visible = false;
        img_help_fireSize.Visible = false;
        img_help_priUtilzation.Visible = false;

        if (ddl_primary.SelectedItem.Text.StartsWith("Harvest") | ddl_primary.SelectedItem.Text.StartsWith("Salvage"))
        {
            lbl_priInterval.ForeColor = Color.Black;
            txt_priInterval.Enabled = true;
            valReq_priInterval.Enabled = true;
            valRegExp_priInterval.Enabled = true;

            lbl_priDisturb.ForeColor = Color.Black;
            txt_priDisturb.Enabled = true;
            valRange_priDisturb.Enabled = true;

            lbl_priUtilization.ForeColor = Color.Black;
            lbl_priUtilization.Text = "Utilization";
            ddl_priUtilization.Visible = true;
            ddl_priUtilization.Enabled = true;
            valReq_priUtilization.Enabled = true;
            ddl_priFireSize.Visible = false;
            valReq_priFireSize.Enabled = false;

            lbl_priSeverity.ForeColor = Color.Gray;
            lbl_priSeverity.Text = "Severity";
            ddl_priSeverity.Visible = true;
            ddl_priSeverity.Enabled = false;
            valReq_priSeverity.Enabled = false;
            ddl_priSuppression.Visible = false;
            valReq_priSuppression.Enabled = false;

            img_help_pri_severity.Visible = true;
            img_help_priUtilzation.Visible = true;
        }
        else if (ddl_primary.SelectedItem.Text.StartsWith("Prescribed"))
        {
            lbl_priInterval.ForeColor = Color.Black;
            txt_priInterval.Enabled = true;
            valReq_priInterval.Enabled = true;
            valRegExp_priInterval.Enabled = true;

            lbl_priDisturb.ForeColor = Color.Black;
            txt_priDisturb.Enabled = true;
            valRange_priDisturb.Enabled = true;

            lbl_priUtilization.ForeColor = Color.Gray;
            lbl_priUtilization.Text = "Utilization";
            ddl_priUtilization.Visible = true;
            ddl_priUtilization.Enabled = false;
            valReq_priUtilization.Enabled = false;
            ddl_priFireSize.Visible = false;
            valReq_priFireSize.Enabled = false;

            lbl_priSeverity.ForeColor = Color.Black;
            lbl_priSeverity.Text = "Severity";
            ddl_priSeverity.Visible = true;
            ddl_priSeverity.Enabled = true;
            valReq_priSeverity.Enabled = true;
            ddl_priSuppression.Visible = false;
            valReq_priSuppression.Enabled = false;

            img_help_pri_severity.Visible = true;
            img_help_priUtilzation.Visible = true;
        }
        else if (ddl_primary.SelectedItem.Text.StartsWith("Wildfire default"))
        {
            lbl_priInterval.ForeColor = Color.Gray;
            txt_priInterval.Enabled = false;
            valReq_priInterval.Enabled = false;
            valRegExp_priInterval.Enabled = false;

            lbl_priDisturb.ForeColor = Color.Gray;
            txt_priDisturb.Enabled = false;
            valRange_priDisturb.Enabled = false;

            lbl_priUtilization.ForeColor = Color.Gray;
            lbl_priUtilization.Text = "Utilization";
            ddl_priUtilization.Visible = true;
            ddl_priUtilization.Enabled = false;
            valReq_priUtilization.Enabled = false;
            ddl_priFireSize.Visible = false;
            valReq_priFireSize.Enabled = false;

            lbl_priSeverity.ForeColor = Color.Black;
            lbl_priSeverity.Text = "Suppression";
            ddl_priSeverity.Visible = false;
            valReq_priSeverity.Enabled = false;
            ddl_priSuppression.Visible = true;
            valReq_priSuppression.Enabled = true;

            img_help_pri_suppression.Visible = true;
        }
        else if (ddl_primary.SelectedItem.Text.StartsWith("Wildfire custom"))
        {
            lbl_priInterval.ForeColor = Color.Black;
            txt_priInterval.Enabled = true;
            valReq_priInterval.Enabled = true;
            valRegExp_priInterval.Enabled = true;

            lbl_priDisturb.ForeColor = Color.Gray;
            txt_priDisturb.Enabled = false;
            valRange_priDisturb.Enabled = false;

            lbl_priUtilization.ForeColor = Color.Black;
            lbl_priUtilization.Text = "Fire Size";
            ddl_priUtilization.Visible = false;
            ddl_priUtilization.Enabled = false;
            valReq_priUtilization.Enabled = false;
            ddl_priFireSize.Visible = true;
            valReq_priFireSize.Enabled = true;

            lbl_priSeverity.ForeColor = Color.Black;
            lbl_priSeverity.Text = "Severity";
            ddl_priSeverity.Visible = true;
            ddl_priSeverity.Enabled = true;
            valReq_priSeverity.Enabled = true;
            ddl_priSuppression.Visible = false;
            valReq_priSuppression.Enabled = false;

            ddl_priSeverity.Items.FindByValue("Default").Enabled = true;
            img_help_pri_severity.Visible = true;
            img_help_fireSize.Visible = true;
        }
        else
        {
            lbl_priInterval.ForeColor = Color.Gray;
            lbl_priDisturb.ForeColor = Color.Gray;
            lbl_priUtilization.ForeColor = Color.Gray;
            lbl_priSeverity.ForeColor = Color.Gray;
            ddl_secondary.Enabled = false;
            lbl_secondary.ForeColor = Color.Gray;
        }

        if (ddl_primary.SelectedItem.Text.StartsWith("Wildfire"))
        {
            //foreach (ListItem ddlItems in ddl_secondary.Items)
            //    ddlItems.Enabled = false;

            ddl_secondary.Items.FindByValue("CCut_SO").Enabled = false;
            ddl_secondary.Items.FindByValue("CCut_AG").Enabled = false;
            ddl_secondary.Items.FindByValue("CCut_WT").Enabled = false;
            ddl_secondary.Items.FindByValue("Prescribed").Enabled = false;
            ddl_secondary.Items.FindByValue("BurnPiles").Enabled = false;
            txt_secDisturb.Enabled = false;
            lbl_secDisturb.ForeColor = Color.Gray;
        }
        else
        {
            ddl_secondary.Items.FindByValue("CCut_SO").Enabled = true;
            ddl_secondary.Items.FindByValue("CCut_AG").Enabled = true;
            ddl_secondary.Items.FindByValue("CCut_WT").Enabled = true;
            ddl_secondary.Items.FindByValue("Prescribed").Enabled = true;
            ddl_secondary.Items.FindByValue("BurnPiles").Enabled = true;
            txt_secDisturb.Enabled = true;
            lbl_secDisturb.ForeColor = Color.Black;
        }

    }
    protected void ddl_secondary_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_secondary.SelectedItem.Text.StartsWith("Harvest") | ddl_secondary.SelectedItem.Text.StartsWith("Salvage"))
        {
            lbl_secUtilization.ForeColor = Color.Black;
            ddl_secUtilization.Enabled = true;
            valReq_secUtilization.Enabled = true;

            lbl_secSeverity.ForeColor = Color.Gray;
            ddl_secSeverity.Enabled = false;
            valReq_secSeverity.Enabled = false;
        }
        else if (ddl_secondary.SelectedItem.Text.StartsWith("Prescribed"))
        {
            lbl_secUtilization.ForeColor = Color.Gray;
            ddl_secUtilization.Enabled = false;
            valReq_secUtilization.Enabled = false;

            lbl_secSeverity.ForeColor = Color.Black;
            ddl_secSeverity.Enabled = true;
            valReq_secSeverity.Enabled = true;
        }
    }
    protected void btn_addPrimary_Click(object sender, EventArgs e)
    {
        manageDict_class manageDict = new manageDict_class();

        SortedList<string, ArrayList> managementDict = new SortedList<string, ArrayList>();
        SortedList<string, ArrayList> natDisturbDict = new SortedList<string, ArrayList>();
        managementDict = (SortedList<string, ArrayList>)Session["managementDict"];
        natDisturbDict = (SortedList<string, ArrayList>)Session["natDisturbDict"];
        // int currentYearInt = Convert.ToInt16(Page.Session["currentYear"].ToString());
        string dictKey;

        //Regex reNum = new Regex(@"^\d+$");
        //if (!reNum.Match(TextBox_Year.Text).Success | TextBox_Year.Text == "" | Convert.ToInt16(TextBox_Year.Text) >= currentYearInt)
        //    TextBox1.Text = "*** Incomplete Event Input ***\nPlease enter a year prior to the current year for the disturbance.";

        ArrayList dictData = new ArrayList();
        dictData.Add(ddl_primary.SelectedValue);

        if (ddl_primary.SelectedItem.Text.StartsWith("Harvest") | ddl_primary.SelectedItem.Text.StartsWith("Salvage"))
        {
            dictData.Add(txt_priInterval.Text);
            dictData.Add(txt_priDisturb.Text);
            dictData.Add(ddl_priUtilization.SelectedValue);
        }
        else if (ddl_primary.SelectedItem.Text.StartsWith("Prescribed"))
        {
            dictData.Add(txt_priInterval.Text);
            dictData.Add(txt_priDisturb.Text);
            dictData.Add(ddl_priSeverity.SelectedValue);
        }
        else if (ddl_primary.SelectedItem.Text.StartsWith("Wildfire default"))
        {
            dictData.Add("null");
            dictData.Add("null");
            dictData.Add(ddl_priSuppression.SelectedValue);
        }
        else if (ddl_primary.SelectedItem.Text.StartsWith("Wildfire custom"))
        {
            dictData.Add(txt_priInterval.Text);
            dictData.Add(ddl_priFireSize.SelectedValue);
            dictData.Add(ddl_priSeverity.SelectedValue);
        }

        dictKey = "p" + txt_priStartYear.Text + "to" + txt_priEndYear.Text;

        if (ddl_primary.SelectedItem.Text.StartsWith("Wildfire"))
            manageDict.insertRegime(natDisturbDict, dictKey, dictData, Convert.ToInt16(txt_priStartYear.Text), Convert.ToInt16(txt_priEndYear.Text));
        else
            manageDict.insertRegime(managementDict, dictKey, dictData, Convert.ToInt16(txt_priStartYear.Text), Convert.ToInt16(txt_priEndYear.Text));

        displayDictionary(managementDict, natDisturbDict);

        // reset secondary range validation to keep secondary offset from being larger than primary interval in management disturbances
        if (ddl_primary.Text.StartsWith("Wildfire"))
            valRange_secOffset.MaximumValue = Page.Session["numSimYears"].ToString();
        else
            valRange_secOffset.MaximumValue = (Convert.ToInt16(txt_priInterval.Text) - 1).ToString();

        valRange_secOffset.ErrorMessage = "Select secondary treatment offset between " + valRange_secOffset.MinimumValue + " and " + valRange_secOffset.MaximumValue;

        //  enable secondary regime controls
        ddl_secondary.Enabled = true;
        lbl_secondary.ForeColor = Color.Black;
    }
    protected void valOffsetFunc(object source, ServerValidateEventArgs args)
    {
        SortedList<string, ArrayList> managementDict = new SortedList<string, ArrayList>();
        SortedList<string, ArrayList> natDisturbDict = new SortedList<string, ArrayList>();

        if (ddl_primary.SelectedItem.Text.StartsWith("Wildfire"))
        {
            natDisturbDict = (SortedList<string, ArrayList>)Session["natDisturbDict"];
            foreach (string regimeKey in natDisturbDict.Keys)
                if (regimeKey.StartsWith("p" + txt_priStartYear.Text + "to" + txt_priEndYear.Text + "s"))
                    if ((natDisturbDict[regimeKey][0].ToString().StartsWith("CCut") & ddl_secondary.SelectedItem.Text.StartsWith("Harv")) | natDisturbDict[regimeKey][0].ToString().StartsWith(ddl_secondary.SelectedItem.Text.Substring(0, 4)))
                        try
                        {
                            // test for offset equal to offset in existing secondary regime of same date range and secondary regime type.
                            if (Convert.ToInt16(args.Value) == Convert.ToInt16(natDisturbDict[regimeKey][1]))
                            {
                                args.IsValid = false;
                                break;
                            }
                            else
                                args.IsValid = true;
                        }
                        catch (Exception ex)
                        {
                            args.IsValid = false;
                            break;
                        }
        }
        else
        {
            managementDict = (SortedList<string, ArrayList>)Session["managementDict"];
            foreach (string regimeKey in managementDict.Keys)
                if (regimeKey.StartsWith("p" + txt_priStartYear.Text + "to" + txt_priEndYear.Text + "s"))
                    if ((managementDict[regimeKey][0].ToString().StartsWith("CCut") & ddl_secondary.SelectedItem.Text.StartsWith("Harv")) | managementDict[regimeKey][0].ToString().StartsWith(ddl_secondary.SelectedItem.Text.Substring(0, 4)))
                        try
                        {
                            // test for offset equal to offset in existing secondary regime of same date range and secondary regime type.
                            if (Convert.ToInt16(args.Value) == Convert.ToInt16(managementDict[regimeKey][1]))
                            {
                                args.IsValid = false;
                                break;
                            }
                            else
                                args.IsValid = true;
                        }
                        catch (Exception ex)
                        {
                            args.IsValid = false;
                            break;
                        }
        }

        // only allow 0 offset in the special case where the primary regime is Harvest and the secondary regime is prescribed fire.
        if (args.Value == "0")
            if (!ddl_primary.SelectedItem.Text.StartsWith("Harv") | !ddl_secondary.SelectedItem.Text.StartsWith("Prescribed"))
                args.IsValid = false;
    }
    protected void btn_addSecondary_Click(object sender, EventArgs e)
    {
        SortedList<string, ArrayList> managementDict = new SortedList<string, ArrayList>();
        SortedList<string, ArrayList> natDisturbDict = new SortedList<string, ArrayList>();
        managementDict = (SortedList<string, ArrayList>)Session["managementDict"];
        natDisturbDict = (SortedList<string, ArrayList>)Session["natDisturbDict"];
        int currentYearInt = Convert.ToInt16(Page.Session["currentYear"].ToString());
        string dictKey;
        int secRegimeNum;

        // Perform add if secondary regime passes validation
        if (Page.IsValid)
        {
            // Confirm that there is a primary regime to assign this secondary regime
            secRegimeNum = -1;
            if (ddl_primary.SelectedItem.Text.StartsWith("Wildfire"))
            {
                if (natDisturbDict.Keys.Contains("p" + txt_priStartYear.Text + "to" + txt_priEndYear.Text))
                    secRegimeNum = Convert.ToInt16(txt_secOffset.Text);
            }
            else
            {
                if (managementDict.Keys.Contains("p" + txt_priStartYear.Text + "to" + txt_priEndYear.Text))
                    secRegimeNum = Convert.ToInt16(txt_secOffset.Text);
            }

            if (secRegimeNum >= 0)
            {
                ArrayList dictData = new ArrayList();
                dictData.Add(ddl_secondary.SelectedValue);

                // read interface text boxes into dictionary element
                if (ddl_secondary.SelectedItem.Text.StartsWith("Harvest") | ddl_secondary.SelectedItem.Text.StartsWith("Salvage"))
                {
                    dictData.Add(txt_secOffset.Text);
                    dictData.Add(txt_secDisturb.Text);
                    dictData.Add(ddl_secUtilization.SelectedValue);
                }
                else if (ddl_secondary.SelectedItem.Text.StartsWith("Prescribed"))
                {
                    dictData.Add(txt_secOffset.Text);
                    dictData.Add(txt_secDisturb.Text);
                    dictData.Add(ddl_secSeverity.SelectedValue);
                }

                // add fixed or rotated data to dictionary element - kao 23 Dec 2014
                dictData.Add(ddl_secPlacement.SelectedValue);

                // build dictionary key for new secondary regime
                dictKey = "p" + txt_priStartYear.Text + "to" + txt_priEndYear.Text + "s" + secRegimeNum.ToString();

                // find unused secondary regime number to assign new regime and add to dictionary
                if (ddl_primary.SelectedItem.Text.StartsWith("Wildfire"))
                {
                    while (natDisturbDict.ContainsKey(dictKey))
                    {
                        secRegimeNum += 1;
                        dictKey = "p" + txt_priStartYear.Text + "to" + txt_priEndYear.Text + "s" + secRegimeNum.ToString();
                    }

                    natDisturbDict.Add(dictKey, dictData);
                }
                else
                {
                    while (managementDict.ContainsKey(dictKey))
                    {
                        secRegimeNum += 1;
                        dictKey = "p" + txt_priStartYear.Text + "to" + txt_priEndYear.Text + "s" + secRegimeNum.ToString();
                    }

                    managementDict.Add(dictKey, dictData);
                }

                displayDictionary(managementDict, natDisturbDict);
            }
        }
    }

    protected void btn3_clearAll_Click(object sender, EventArgs e)
    {
        manageDict_class manageDict = new manageDict_class();
        displayDict_class displayDict = new displayDict_class();

        // remove all future check box elements with removeItems class. Pass in collection of check box items, dictionary, and current year
        manageDict.clearAllHistory(cbList_management.Items, (SortedList<string, ArrayList>)Session["managementDict"], Convert.ToInt16(Page.Session["currentYear"].ToString()));
        manageDict.clearAllHistory(cbList_natDisturb.Items, (SortedList<string, ArrayList>)Session["natDisturbDict"], Convert.ToInt16(Page.Session["currentYear"].ToString()));

        ddl_secondary.Enabled = false;
        lbl_secondary.ForeColor = Color.Gray;

        displayDictionary((SortedList<string, ArrayList>)Session["managementDict"], (SortedList<string, ArrayList>)Session["natDisturbDict"]);
    }
    protected void btn3_removeElements_Click(object sender, EventArgs e)
    {
        manageDict_class manageDict = new manageDict_class();

        // remove selected check box elements with removeItems class. Pass in collection of check box items and dictionary
        manageDict.removeItems(cbList_management.Items, (SortedList<string, ArrayList>)Session["managementDict"]);
        manageDict.removeItems(cbList_natDisturb.Items, (SortedList<string, ArrayList>)Session["natDisturbDict"]);

        displayDictionary((SortedList<string, ArrayList>)Session["managementDict"], (SortedList<string, ArrayList>)Session["natDisturbDict"]);
    }

    protected void btn3_setDefault_Click(object sender, EventArgs e)
    {
        manageDict_class manageDict = new manageDict_class();
        SortedList<string, ArrayList> managementDict = new SortedList<string, ArrayList>();
        SortedList<string, ArrayList> natDisturbDict = new SortedList<string, ArrayList>();
        managementDict = (SortedList<string, ArrayList>)Session["managementDict"];
        natDisturbDict = (SortedList<string, ArrayList>)Session["natDisturbDict"];

        string appPath = Request.PhysicalApplicationPath; // requires System.Diagnostics
        string xmlParamFile = appPath + "LandCarbData31\\Base\\ManagementHistory\\";
        xmlParamFile += Page.Session["region"].ToString() + "_" + Page.Session["ownership"].ToString() + "_defaultHist.xml";

        XmlDocument oXmlDoc = new XmlDocument();
        XmlTextReader xmlParamReader = new XmlTextReader(xmlParamFile);
        xmlParamReader.Read();
        oXmlDoc.Load(xmlParamReader);
        XmlNode oNode = oXmlDoc.DocumentElement;

        manageDict.loadDictFromXML(managementDict, oNode.SelectNodes("/RunParameters/ManagementRegime"));
        manageDict.loadDictFromXML(natDisturbDict, oNode.SelectNodes("/RunParameters/NaturalDisturbanceRegime"));

        // adjust default management history for custom current run year
        if (Page.Session["currentYear"].ToString() != "2012")
        {
            ArrayList modKeyList = new ArrayList();
            string newStartYear = Page.Session["simStartYear"].ToString();
            string newEndYear = (Convert.ToInt16(Page.Session["currentYear"].ToString()) - 1).ToString();

            manageDict.modDictKey(managementDict, newStartYear, newEndYear);
            manageDict.modDictKey(natDisturbDict, newStartYear, newEndYear);
        }

        displayDictionary(managementDict, natDisturbDict);
    }

    //protected void btn3_setDefault_Click(object sender, EventArgs e)
    //{
    //    SortedList<string, ArrayList> managementDict = new SortedList<string, ArrayList>();
    //    managementDict = (SortedList<string, ArrayList>)Session["managementDict"];
    //    SortedList<string, ArrayList> natDisturbDict = new SortedList<string, ArrayList>();
    //    natDisturbDict = (SortedList<string, ArrayList>)Session["natDisturbDict"];
    //    ArrayList dictData = new ArrayList();
    //    string dictKey;
    //    object junkObject = new object();
    //    EventArgs junkEvent = new EventArgs();
    //    int endYear = Convert.ToInt16(Page.Session["currentYear"].ToString()) - 1;

    //    // clear all history regimes
    //    btn3_clearAll_Click(junkObject, junkEvent);
    //    managementDict.Clear();
    //    natDisturbDict.Clear();

    //    // load this from XML!!
    //    dictKey = "p1950to1955";
    //    dictData.Add("CCut_SO");
    //    dictData.Add("120");
    //    dictData.Add("100");
    //    dictData.Add("Medium");
    //    managementDict.Add(dictKey, dictData);

    //    dictData = new ArrayList();
    //    dictKey = "p1950to1955s1";
    //    dictData.Add("Prescribed");
    //    dictData.Add("0");
    //    dictData.Add("100");
    //    dictData.Add("High");
    //    managementDict.Add(dictKey, dictData);

    //    dictData = new ArrayList();
    //    dictKey = "p1956to1960";
    //    dictData.Add("CCut_SO");
    //    dictData.Add("100");
    //    dictData.Add("100");
    //    dictData.Add("Medium");
    //    managementDict.Add(dictKey, dictData);

    //    dictData = new ArrayList();
    //    dictKey = "p1956to1960s1";
    //    dictData.Add("Prescribed");
    //    dictData.Add("0");
    //    dictData.Add("100");
    //    dictData.Add("High");
    //    managementDict.Add(dictKey, dictData);

    //    dictData = new ArrayList();
    //    dictKey = "p1961to1965";
    //    dictData.Add("CCut_SO");
    //    dictData.Add("90");
    //    dictData.Add("100");
    //    dictData.Add("Medium");
    //    managementDict.Add(dictKey, dictData);

    //    dictData = new ArrayList();
    //    dictKey = "p1961to1965s1";
    //    dictData.Add("Prescribed");
    //    dictData.Add("0");
    //    dictData.Add("100");
    //    dictData.Add("High");
    //    managementDict.Add(dictKey, dictData);

    //    dictData = new ArrayList();
    //    dictKey = "p1966to1990";
    //    dictData.Add("CCut_SO");
    //    dictData.Add("80");
    //    dictData.Add("100");
    //    dictData.Add("Medium");
    //    managementDict.Add(dictKey, dictData);

    //    dictData = new ArrayList();
    //    dictKey = "p1966to1990s1";
    //    dictData.Add("Prescribed");
    //    dictData.Add("0");
    //    dictData.Add("100");
    //    dictData.Add("High");
    //    managementDict.Add(dictKey, dictData);

    //    dictData = new ArrayList();
    //    dictKey = "p1991to" + endYear.ToString();
    //    dictData.Add("CCut_SO");
    //    dictData.Add("200");
    //    dictData.Add("35");
    //    dictData.Add("Medium");
    //    managementDict.Add(dictKey, dictData);

    //    //dictKey = "P" + Page.Session["simStartYear"].ToString() + "to1960";
    //    //dictData.Add("120");
    //    //dictData.Add("85");
    //    //dictData.Add("Medium");
    //    //landscapeDict.Add(dictKey, dictData);
    //    //dictData = new ArrayList();

    //    //dictKey = "P1961to1984";
    //    //dictData.Add("60");
    //    //dictData.Add("90");
    //    //dictData.Add("Hot");
    //    //landscapeDict.Add(dictKey, dictData);
    //    //dictData = new ArrayList();

    //    //dictKey = "P1985to1991";
    //    //dictData.Add("60");
    //    //dictData.Add("90");
    //    //dictData.Add("Medium");
    //    //landscapeDict.Add(dictKey, dictData);
    //    //dictData = new ArrayList();

    //    //dictKey = "P1992to" + endYear.ToString();
    //    //dictData.Add("200");
    //    //dictData.Add("95");
    //    //dictData.Add("Light");
    //    //landscapeDict.Add(dictKey, dictData);
    //    //dictData = new ArrayList();

    //    dictData = new ArrayList();
    //    dictKey = "p" + Page.Session["simStartYear"].ToString() + "to1940";
    //    dictData.Add("WildfireDefault");
    //    dictData.Add("null");
    //    dictData.Add("null");
    //    dictData.Add("noSupp");
    //    natDisturbDict.Add(dictKey, dictData);

    //    dictData = new ArrayList();
    //    dictKey = "p1941to" + endYear.ToString();
    //    dictData.Add("WildfireDefault");
    //    dictData.Add("null");
    //    dictData.Add("null");
    //    dictData.Add("typical");
    //    natDisturbDict.Add(dictKey, dictData);

    //    displayDictionary(managementDict, natDisturbDict);
    //}
}
