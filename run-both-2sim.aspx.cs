using System;
using System.Collections;
using System.Collections.Generic;
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

public partial class run_both_2sim_code : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // check for session timeout and send user back to the beginning if timeout has occured
            if (Page.Session["scale"] == null)
                Response.Redirect("default.aspx");

            if (Page.Session["currentYear"] != null)
                textCalendarYear.Text = Page.Session["currentYear"].ToString();
            if (Page.Session["numSimYears"] != null)
                textSimYears.Text = Page.Session["numSimYears"].ToString();
            if (Page.Session["randomSeed"] != null)
                radioRepeatRand.Text = Page.Session["randomSeed"].ToString();
            if (Page.Session["cellAreaHa"] != null)
            {
                txtCellAreaHa.Text = Page.Session["cellAreaHa"].ToString();
                txtCellAreaHa_TextChanged(new object(), new EventArgs());
            }
            if (Page.Session["runName"] != null)
                txtRunName.Text = Page.Session["runName"].ToString();

            if (Page.Session["scale"].ToString() == "stand")
            {
                lbl_cellSize1.Visible = false;
                RequiredFieldValidator_cellArea.Enabled = false;
                RangeValidator_cellArea.Enabled = false;
                txtCellAreaHa.Text = "1";
                txtCellAreaHa.Visible = false;
                lbl_cellSize2.Visible = false;
                lbl_cellSize3.Visible = false;
                lbl_cellSize4.Visible = false;
                lblTotalAreaHa.Visible = false;
                lblTotalAreaAc.Visible = false;
                img_help_cellSize.Visible = false;
            }

            textCalendarYear.Focus();
        }
    }

    protected void btn2return_Click(object sender, EventArgs e)
    {
        setSessionVariables();
        Response.Redirect("run-both-1site.aspx");
    }

    protected void btn2next_Click(object sender, EventArgs e)
    {
        setSessionVariables();
        if (Page.Session["scale"] != null)
            Response.Redirect("run-" + Page.Session["scale"].ToString() + "-3history.aspx");
        else
            Response.Redirect("default.aspx");

         //   Response.Write(@"<script language='javascript'>
         //       alert('Please enter a value for the current year and number of years for simulation to run.');
         //       </script>"); 
            
            //MessageBox.Show("Please enter a value for the current year and number of years for simulation to run", "Missing Entry",
            //   MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }
    protected void setSessionVariables()
    {
        Session["currentYear"] = textCalendarYear.Text;
        Session["numSimYears"] = textSimYears.Text;
        Session["randomSeed"] = radioRepeatRand.Text;

        // Set spinup year to be the same for landscape and stand level runs - 20 Dec 2010
        Session["simStartYear"] = Convert.ToInt16(textCalendarYear.Text) - 600;

        Session["cellAreaHa"] = txtCellAreaHa.Text;
        Session["runName"] = txtRunName.Text;
    }

    protected void txtCellAreaHa_TextChanged(object sender, EventArgs e)
    {
        if (Page.Session["simCols"] != null & Page.Session["simRows"] != null)
        {
            double numHa = Convert.ToDouble(txtCellAreaHa.Text) * Convert.ToDouble(Page.Session["simCols"].ToString()) * Convert.ToDouble(Page.Session["simRows"].ToString());
            lblTotalAreaHa.Text = (Convert.ToInt32(numHa)).ToString();
            lblTotalAreaAc.Text = (Convert.ToInt32(numHa * 2.471)).ToString();
        }
        else
            Response.Redirect("default.aspx");
    }
    protected void textCalendarYear_TextChanged(object sender, EventArgs e)
    {
        Session["managementDict"] = new SortedList<string, ArrayList>();
        Session["natDisturbDict"] = new SortedList<string, ArrayList>();
    }
}
