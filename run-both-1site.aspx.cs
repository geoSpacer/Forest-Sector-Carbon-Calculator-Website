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

public partial class run_both_1site_code : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            setOptions_class setOptions = new setOptions_class();
            setOptions.setRegionItems(ddl_region.Items);
            setOptions.setOwnershipItems(ddl_ownership.Items);
            setOptions.setElevationItems(ddl_elevation.Items);

            if (Page.Session["region"] != null)
                ddl_region.SelectedValue = Page.Session["region"].ToString();
            if (Page.Session["ownership"] != null)
                ddl_ownership.SelectedValue = Page.Session["ownership"].ToString();
            if (Page.Session["elevClass"] != null)
                ddl_elevation.SelectedValue = Page.Session["elevClass"].ToString();

        }
    }

    protected void btn1Next_Click(object sender, EventArgs e)
    {
        // Session["region"] = Request.Form["regionName"];
        // Session["own"] = Request.Form["ownership"];
        // Session["elevClass"] = Request.Form["elevationClass"];
        // Session["siteIndx"] = Request.Form["siteIndex"];

        Session["region"] = ddl_region.SelectedValue;
        Session["ownership"] = ddl_ownership.SelectedValue;
        Session["elevClass"] = ddl_elevation.SelectedValue;
        // Session["siteIndx"] = "Site Class 1";

        Response.Redirect("run-both-2sim.aspx");
    }
    protected void btn1Previous_Click(object sender, EventArgs e)
    {
        if (Page.Session["scale"] != null)
            Response.Redirect("run-" + Page.Session["scale"].ToString() + ".aspx");
        else
            Response.Redirect("default.aspx");
    }
    protected void ImageMap_ecoregions_Click(object sender, ImageMapEventArgs e)
    {
        if (e.PostBackValue == "westCasc")
            ddl_region.SelectedValue = "WestCascades";
        else if (e.PostBackValue == "eastCasc")
            ddl_region.SelectedValue = "EastCascades";
    }
}
