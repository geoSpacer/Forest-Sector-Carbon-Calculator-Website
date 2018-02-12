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

using System.Diagnostics; // for Request
using System.Collections.Generic;
using System.Drawing; // for colors

public partial class run_landcarb : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            renameValue_class renameValue = new renameValue_class();
            displayDict_class displayDict = new displayDict_class();
            SortedList<string, ArrayList> managementDict = new SortedList<string, ArrayList>();
            SortedList<string, ArrayList> natDisturbDict = new SortedList<string, ArrayList>();
            managementDict = (SortedList<string, ArrayList>)Page.Session["managementDict"];
            natDisturbDict = (SortedList<string, ArrayList>)Page.Session["natDisturbDict"];

            // check for session timeout and send user back to the beginning if timeout has occured
            if (Page.Session["scale"] == null)
                Response.Redirect("default.aspx");

            // get the physical folder; currently on Charcoal
            string landCarbDir = "LandCarbData31";
            string appPath = Request.PhysicalApplicationPath; // requires System.Diagnostics

            // ***** Setup directories, copy standard files and write driver files
            DateTime centuryBegin = new DateTime(2001, 1, 1);
            DateTime currentDate = DateTime.Now;
            long elapsedTicks = (currentDate.Ticks - centuryBegin.Ticks) / 100000;

            // Identify unique directory for this user.
            Session["userDir"] = appPath + landCarbDir + "\\user" + elapsedTicks.ToString();

            if (Page.Session["runName"] != null)
                lbl_2runName.Text = Page.Session["runName"].ToString();
            if (Page.Session["region"] != null)
                lbl_1region.Text = renameValue.regionName(Page.Session["region"].ToString());
            if (Page.Session["ownership"] != null)
                lbl_1own.Text = renameValue.ownershipName(Page.Session["ownership"].ToString());
            if (Page.Session["elevClass"] != null)
                lbl_1elevClass.Text = Page.Session["elevClass"].ToString();

            if (Page.Session["currentYear"] != null)
                lbl_2currentYear.Text = Page.Session["currentYear"].ToString();
            else
            {
                lbl_2currentYear.ForeColor = Color.Red;
                lbl_2currentYear.Text = "Value required";
            }
            if (Page.Session["numSimYears"] != null)
                lbl_2numSimYears.Text = Page.Session["numSimYears"].ToString();
            else
            {
                lbl_2numSimYears.ForeColor = Color.Red;
                lbl_2numSimYears.Text = "Value required";
            }

            if (Page.Session["randomSeed"] != null)
                lbl_2randomSeed.Text = Page.Session["randomSeed"].ToString();
            if (Page.Session["cellAreaHa"] != null)
                lbl_2cellSize.Text = Page.Session["cellAreaHa"].ToString();

            if (Page.Session["substitutionProd"] != null)
                lbl_5prodSubstitution.Text = Page.Session["substitutionProd"].ToString();
            if (Page.Session["substitutionEnergy"] != null)
                lbl_5energySubstitution.Text = Page.Session["substitutionEnergy"].ToString();

            TextBox_disturbance.Text = "Management Regime:\n";
            if (Page.Session["scale"].ToString() == "stand")
            {
                foreach (KeyValuePair<string, ArrayList> dictEntry in managementDict)
                    TextBox_disturbance.Text += displayDict.displayManagementStand(dictEntry) + "\n";
            }
            else
            {
                foreach (KeyValuePair<string, ArrayList> dictEntry in managementDict)
                    TextBox_disturbance.Text += displayDict.displayManagement(dictEntry) + "\n";
            }

            TextBox_disturbance.Text += "\nDisturbance Regime:\n";
            foreach (KeyValuePair<string, ArrayList> dictEntry in natDisturbDict)
                TextBox_disturbance.Text += displayDict.displayNatDisturb(dictEntry) + "\n";
        }
    }

    protected void btn_previous_Click(object sender, EventArgs e)
    {
        Response.Redirect("run-both-5products.aspx");
    }
    //protected void btn_runLC_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("run-writeXML.aspx");
    //}
}

