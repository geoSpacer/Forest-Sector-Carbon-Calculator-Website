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

public partial class run_both_5substitution : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Page.Session["subDisplacement"] != null)
                rb_subDisplacement.SelectedValue = Page.Session["subDisplacement"].ToString();
            if (Page.Session["subBuilding"] != null)
                rb_subBuilding.SelectedValue = Page.Session["subBuilding"].ToString();
            if (Page.Session["subBioenergyFactor"] != null)
                txt_bioenergyDisp.Text = Page.Session["subBioenergyFactor"].ToString();
            if (Page.Session["subLeakage"] != null)
                rb_leakage.SelectedValue = Page.Session["subLeakage"].ToString();
        }
    }
     protected void setSessionVariables()
    {
        Session["subDisplacement"] = rb_subDisplacement.SelectedValue;
        Session["subBuilding"] = rb_subBuilding.SelectedValue;
        Session["subBioenergyFactor"] = txt_bioenergyDisp.Text;
        Session["subLeakage"] = rb_leakage.SelectedValue;
    }

    protected void btn6next_Click(object sender, EventArgs e)
    {
        setSessionVariables();
        Response.Redirect("run-both-landcarb.aspx");
    }
    protected void btn6previous_Click(object sender, EventArgs e)
    {
        setSessionVariables();
        Response.Redirect("run-both-5products.aspx");
    }
}