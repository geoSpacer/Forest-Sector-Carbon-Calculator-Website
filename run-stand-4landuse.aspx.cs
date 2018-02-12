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

using System.Diagnostics; // for request
using System.Collections.Generic; // for sortedList 
using System.Drawing; // for colors
using System.Text.RegularExpressions;

public partial class run_stand_4landuse : System.Web.UI.Page
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

            valRange_priInterval.MinimumValue = "1";
            valRange_priInterval.MaximumValue = Page.Session["numSimYears"].ToString();
            valRange_priInterval.ErrorMessage = "Select primary treatment interval between " + valRange_priInterval.MinimumValue + " and " + valRange_priInterval.MaximumValue;
            valRange_secOffset.MinimumValue = "0";
            valRange_secOffset.MaximumValue = Page.Session["numSimYears"].ToString();
            valRange_secOffset.ErrorMessage = "Select secondary treatment offset between " + valRange_secOffset.MinimumValue + " and " + valRange_secOffset.MaximumValue;

            displayDictionary((SortedList<string, ArrayList>)Session["managementDict"], (SortedList<string, ArrayList>)Session["natDisturbDict"]);
        }
    }

    protected void btn4previous_Click(object sender, EventArgs e)
    {
        Response.Redirect("run-stand-3history.aspx");
    }

    protected void btn4next_Click(object sender, EventArgs e)
    {
        Response.Redirect("run-both-5products.aspx");
    }

    protected void btn_addPrimary_Click(object sender, EventArgs e)
    {
        manageDict_class manageDict = new manageDict_class();
        SortedList<string, ArrayList> managementDict = new SortedList<string, ArrayList>();
        SortedList<string, ArrayList> natDisturbDict = new SortedList<string, ArrayList>();
        managementDict = (SortedList<string, ArrayList>)Session["managementDict"];
        natDisturbDict = (SortedList<string, ArrayList>)Session["natDisturbDict"];
        int currentYear = Convert.ToInt16(Page.Session["currentYear"].ToString());
        int endYear = Convert.ToInt16(Page.Session["currentYear"].ToString()) + Convert.ToInt16(Page.Session["numSimYears"].ToString());
        string dictKey;
        //object junkObject = new object();
        //EventArgs junkEvent = new EventArgs();
        //btn4_clearAll_Click(junkObject, junkEvent);

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

        if (ddl_primary.SelectedItem.Text.StartsWith("Wildfire"))
        {
            manageDict.clearAllFuture(cbList_natDisturb.Items, natDisturbDict, currentYear);
            dictKey = "p" + currentYear.ToString() + "to" + endYear.ToString();
            natDisturbDict.Add(dictKey, dictData);
        }
        else
        {
            manageDict.clearAllFuture(cbList_management.Items, managementDict, currentYear);

            for (int year = currentYear; year < endYear; year += Convert.ToInt16(txt_priInterval.Text))
            {
                dictKey = "p" + year.ToString() + "xx0000";
                managementDict.Add(dictKey, dictData);
            }
        }

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
        int currentYear = Convert.ToInt16(Page.Session["currentYear"].ToString());
        int endYear = Convert.ToInt16(Page.Session["currentYear"].ToString()) + Convert.ToInt16(Page.Session["numSimYears"].ToString());
        string dictKey;
        int yearOffset;

        ArrayList dictData = new ArrayList();
        dictData.Add(ddl_secondary.SelectedValue);

        // load secondary regime settings into data array
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

        if (ddl_primary.SelectedItem.Text.StartsWith("Wildfire"))
        {
            dictKey = "p" + currentYear.ToString() + "to" + endYear.ToString() + "s";
            if (natDisturbDict.ContainsKey(dictKey))
                natDisturbDict.Remove(dictKey);

            natDisturbDict.Add(dictKey, dictData);
        }
        else
        {
            for (int year = currentYear; year < endYear; year += Convert.ToInt16(txt_priInterval.Text))
            {
                yearOffset = Convert.ToInt16(txt_secOffset.Text);
                dictKey = "p" + year.ToString() + "xx0000s" + yearOffset.ToString();

                while (managementDict.ContainsKey(dictKey))
                {
                    // if there exists a secondary harvest regime with the same offset, then delete it
                    if (managementDict[dictKey][0].ToString().Substring(0, 4) != ddl_secondary.SelectedValue.Substring(0, 4))
                        yearOffset += 1;
                    else
                        managementDict.Remove(dictKey);

                    dictKey = "p" + year.ToString() + "xx0000s" + yearOffset.ToString();
                }

                if ((year + yearOffset) < endYear)
                    managementDict.Add(dictKey, dictData);
            }
        }

        displayDictionary(managementDict, natDisturbDict);
    }
    //protected void btn4addText_Click(object sender, EventArgs e)
    //{
    //    SortedList<string, ArrayList> futureDict = new SortedList<string, ArrayList>();
    //    futureDict = (SortedList<string, ArrayList>)Session["futDictionary"];

    //    int currentYear = Convert.ToInt16(Page.Session["currentYear"].ToString());
    //    int endYear = Convert.ToInt16(Page.Session["currentYear"].ToString()) + Convert.ToInt16(Page.Session["numSimYears"].ToString());
    //    ArrayList dictData = new ArrayList();
    //    string treatmentKey;
    //    Regex reNum = new Regex(@"^\d+$");

    //    if (Panel_primary.Enabled == true)
    //    {
    //        dictData.Add(futA_Treatment.SelectedValue);
    //        dictData.Add(futA_PercentStand.Text);
    //        dictData.Add(futA_Utilization.SelectedValue);

    //        for (int year = currentYear; year < endYear; year += Convert.ToInt16(futA_Years.Text))
    //        {
    //            treatmentKey = year.ToString();
    //            while (futureDict.ContainsKey(treatmentKey))
    //            {
    //                treatmentKey = (Convert.ToInt16(treatmentKey) + 1).ToString();
    //            }

    //            futureDict.Add(treatmentKey, dictData);
    //        }
    //    }
    //    else
    //    {
    //        dictData.Add(futB_Treatment.SelectedValue);
    //        dictData.Add(futB_PercentStand.Text);
    //        dictData.Add(futB_Utilization.SelectedValue);

    //        for (int year = currentYear; year < endYear; year += Convert.ToInt16(futA_Years.Text))
    //        {
    //            treatmentKey = (year + Convert.ToInt16(futB_Years.Text)).ToString();
    //            while (futureDict.ContainsKey(treatmentKey))
    //            {
    //                treatmentKey = (Convert.ToInt16(treatmentKey) + 1).ToString();
    //            }

    //            if (Convert.ToInt16(treatmentKey) < endYear)
    //                futureDict.Add(treatmentKey, dictData);
    //        }  
    //    }

    //    displayDictionary(futureDict);
    //    setTreatmentPanel();
    //}

    protected void displayDictionary(SortedList<string, ArrayList> managementDict, SortedList<string, ArrayList> natDisturbDict)
    {
        displayDict_class displayDict = new displayDict_class();

        int simCurrentYear = Convert.ToInt16(Page.Session["currentYear"].ToString());
        string defaultFireKey = "p" + Page.Session["currentYear"].ToString() + "to" + (simCurrentYear + Convert.ToInt16(Page.Session["numSimYears"].ToString())).ToString();
        cbList_management.Items.Clear();
        cbList_natDisturb.Items.Clear();

        foreach (KeyValuePair<string, ArrayList> dictEntry in managementDict)
            if (Convert.ToInt16(dictEntry.Key.Substring(1, 4)) >= simCurrentYear)
                cbList_management.Items.Add(new ListItem(displayDict.displayManagementStand(dictEntry), dictEntry.Key));

        foreach (KeyValuePair<string, ArrayList> dictEntry in natDisturbDict)
            if (Convert.ToInt16(dictEntry.Key.Substring(1, 4)) >= simCurrentYear)
                cbList_natDisturb.Items.Add(new ListItem(displayDict.displayNatDisturb(dictEntry), dictEntry.Key));

        if (cbList_natDisturb.Items.Count == 0)
            displayDict.setNoFireDefault(cbList_natDisturb.Items, (SortedList<string, ArrayList>)Session["natDisturbDict"], defaultFireKey);

    }

    //protected void futA_Treatment_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    ListItem noneItem = futA_Utilization.Items.FindByText("None");
    //    if (futA_Treatment.SelectedValue == "CCut")
    //    {
    //        if (noneItem == null)
    //            futA_Utilization.Items.Insert(1, new ListItem("None", "None"));
    //    }
    //    else
    //    {
    //        if (noneItem != null)
    //            futA_Utilization.Items.Remove(noneItem);
    //    }
    //}
    //protected void futB_Treatment_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    ListItem noneItem = futB_Utilization.Items.FindByText("None");
    //    if (futB_Treatment.SelectedValue == "CCut")
    //    {
    //        if (noneItem == null)
    //            futB_Utilization.Items.Insert(1, new ListItem("None", "None"));
    //    }
    //    else
    //    {
    //        if (noneItem != null)
    //            futB_Utilization.Items.Remove(noneItem);
    //    }
    //}
    protected void btn4_clearAll_Click(object sender, EventArgs e)
    {
        manageDict_class manageDict = new manageDict_class();
        displayDict_class displayDict = new displayDict_class();

        // remove all future check box elements with removeItems class. Pass in collection of check box items, dictionary, and current year
        manageDict.clearAllFuture(cbList_management.Items, (SortedList<string, ArrayList>)Session["managementDict"], Convert.ToInt16(Page.Session["currentYear"].ToString()));
        manageDict.clearAllFuture(cbList_natDisturb.Items, (SortedList<string, ArrayList>)Session["natDisturbDict"], Convert.ToInt16(Page.Session["currentYear"].ToString()));

        // disable secondary regime controls
        ddl_secondary.SelectedIndex = 0;
        ddl_secondary.Enabled = false;
        lbl_secondary.ForeColor = Color.Gray;

        displayDictionary((SortedList<string, ArrayList>)Session["managementDict"], (SortedList<string, ArrayList>)Session["natDisturbDict"]);
    }
    protected void btn4_removeElements_Click(object sender, EventArgs e)
    {
        manageDict_class manageDict = new manageDict_class();

        // remove selected check box elements with removeItems class. Pass in collection of check box items and dictionary
        manageDict.removeItems(cbList_management.Items, (SortedList<string, ArrayList>)Session["managementDict"]);
        manageDict.removeItems(cbList_natDisturb.Items, (SortedList<string, ArrayList>)Session["natDisturbDict"]);

        displayDictionary((SortedList<string, ArrayList>)Session["managementDict"], (SortedList<string, ArrayList>)Session["natDisturbDict"]);
    }
    protected void ddl_primary_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddl_priSeverity.Items.FindByText("Default").Enabled = false;
        img_help_pri_severity.Visible = false;
        img_help_pri_suppression.Visible = false;
        img_help_fireSize.Visible = false;
        img_help_priUtilization.Visible = false;

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
            img_help_priUtilization.Visible = true;
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
            img_help_priUtilization.Visible = true;
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

            ddl_priSeverity.Items.FindByText("Default").Enabled = true;
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
}
