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

//using System.IO; // for directory listing and creation
using System.Diagnostics; // for Request
// using System.Windows.Forms; // for messagebox
using System.Text.RegularExpressions;
using System.Collections.Generic;  // for sortedList
using System.Drawing; // for colors

public partial class run_stand_3history : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // check for session timeout and send user back to the beginning if timeout has occured
            if (Page.Session["scale"] == null)
                Response.Redirect("default.aspx");

            setOptions_class setOptions = new setOptions_class();
            setOptions.setPriRegimeItems(ddl_primary.Items);
            ddl_primary.Items.FindByValue("WildfireDefault").Enabled = false;
            ddl_primary.Items.FindByValue("WildfireCustom").Enabled = false;
            ddl_primary.Items.Add(new ListItem("Wildfire", "WildfireEvent"));

            setOptions.setUtilizationItems(ddl_priUtilization.Items);
            setOptions.setSeverityItems(ddl_priSeverity.Items);
            setOptions.setSuppressionItems(ddl_priSuppression.Items);
            setOptions.setFireSizeItems(ddl_priFireSize.Items);

            int endYear = Convert.ToInt16(Page.Session["currentYear"].ToString()) - 1;
            valRange_priYear.MinimumValue = Page.Session["simStartYear"].ToString();
            valRange_priYear.MaximumValue = endYear.ToString();
            valRange_priYear.ErrorMessage = "Select a year between " + valRange_priYear.MinimumValue + " and " + valRange_priYear.MaximumValue;

            displayDictionary((SortedList<string, ArrayList>)Session["managementDict"]);
        }

    }
    protected void btn3previous_Click(object sender, EventArgs e)
    {
        Response.Redirect("run-both-2sim.aspx");
    }
    protected void btn3next_Click(object sender, EventArgs e)
    {
        Response.Redirect("run-stand-4landuse.aspx");
    }
    protected void btn_addPrimary_Click(object sender, EventArgs e)
    {
        // manageDict_class manageDict = new manageDict_class();
        SortedList<string, ArrayList> managementDict = new SortedList<string, ArrayList>();
        managementDict = (SortedList<string, ArrayList>)Session["managementDict"];
        int eventYear = Convert.ToInt16(txt_priYear.Text);
        string dictKey;

        ArrayList dictData = new ArrayList();
        dictData.Add(ddl_primary.SelectedValue);

        if (ddl_primary.SelectedItem.Text.StartsWith("Harvest") | ddl_primary.SelectedItem.Text.StartsWith("Salvage"))
        {
            dictData.Add("null");
            dictData.Add(txt_priDisturb.Text);
            dictData.Add(ddl_priUtilization.SelectedValue);
        }
        else if (ddl_primary.SelectedItem.Text.StartsWith("Prescribed") | ddl_primary.SelectedItem.Text == "Wildfire")
        {
            dictData.Add("null");
            dictData.Add(txt_priDisturb.Text);
            dictData.Add(ddl_priSeverity.SelectedValue);
        }

        dictKey = "p" + eventYear.ToString() + "xx0000";
        while (managementDict.ContainsKey(dictKey))
        {
            eventYear += 1;
            dictKey = "p" + eventYear.ToString() + "xx0000";
        }

        managementDict.Add(dictKey, dictData);
        displayDictionary(managementDict);
    }
    protected void displayDictionary(SortedList<string, ArrayList> managementDict)
    {
        displayDict_class displayDict = new displayDict_class();

        int simCurrentYear = Convert.ToInt16(Page.Session["currentYear"].ToString());
        cbList_management.Items.Clear();

        foreach (KeyValuePair<string, ArrayList> dictEntry in managementDict)
            if (Convert.ToInt16(dictEntry.Key.Substring(1, 4)) < simCurrentYear)
                cbList_management.Items.Add(new ListItem(displayDict.displayManagementStand(dictEntry), dictEntry.Key));
    }

    protected void btn4_clearAll_Click(object sender, EventArgs e)
    {
        manageDict_class manageDict = new manageDict_class();

        // remove all future check box elements with removeItems class. Pass in collection of check box items, dictionary, and current year
        manageDict.clearAllHistory(cbList_management.Items, (SortedList<string, ArrayList>)Session["managementDict"], Convert.ToInt16(Page.Session["currentYear"].ToString()));
    }
    protected void btn4_removeElements_Click(object sender, EventArgs e)
    {
        manageDict_class manageDict = new manageDict_class();

        // remove selected check box elements with removeItems class. Pass in collection of check box items and dictionary
        manageDict.removeItems(cbList_management.Items, (SortedList<string, ArrayList>)Session["managementDict"]);

        displayDictionary((SortedList<string, ArrayList>)Session["managementDict"]);
    }
    protected void ddl_primary_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_primary.SelectedItem.Text.StartsWith("Harvest") | ddl_primary.SelectedItem.Text.StartsWith("Salvage"))
        {
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
        }
        else if (ddl_primary.SelectedItem.Text.StartsWith("Prescribed") | ddl_primary.SelectedItem.Text == "Wildfire")
        {
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
        }
        else
        {
            lbl_priUtilization.ForeColor = Color.Gray;
            lbl_priSeverity.ForeColor = Color.Gray;
        }
    }
}
