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

public partial class run_both_5products : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            double Num;

            if (Page.Session["mfgExBiofuel"] == null)
                setDefaultValues();
            else if (!(double.TryParse(Page.Session["mfgExBiofuel"].ToString(), out Num)))
                setDefaultValues();
            else
            {
                textExtBio.Text = Page.Session["mfgExBiofuel"].ToString();
                textStrWood.Text = Page.Session["mfgStrWood"].ToString();
                textPulp.Text = Page.Session["mfgPulp"].ToString();

                textLTRecycle.Text = Page.Session["recycleLT"].ToString();
                textSTRecycle.Text = Page.Session["recycleST"].ToString();
                textPaperRecycle.Text = Page.Session["recyclePaper"].ToString();

                textLTStrFraction.Text = Page.Session["productLTStr"].ToString();

                textDump.Text = Page.Session["disposalDump"].ToString();
                textLandfill.Text = Page.Session["disposalLandfill"].ToString();
                textIncinVol.Text = Page.Session["disposalIncinVol"].ToString();
                textIncinBioenergy.Text = Page.Session["disposalIncinBioenergy"].ToString();

                rb_substitution.SelectedValue = Page.Session["substitutionProd"].ToString();
                rb_bioenergySub.SelectedValue = Page.Session["substitutionEnergy"].ToString();
            }
             
            calcTotalManufacturing();
            calcTotalProductUse();
            calcTotalDisposal();
        }
    }
    protected void setDefaultValues()
    {
        textExtBio.Text = "2";
        textStrWood.Text = "93";
        textPulp.Text = "5";

        textLTRecycle.Text = "10";
        textSTRecycle.Text = "10";
        textPaperRecycle.Text = "30";

        textLTStrFraction.Text = "75";

        textDump.Text = "1";
        textLandfill.Text = "84";
        textIncinVol.Text = "10";
        textIncinBioenergy.Text = "5";

        rb_substitution.SelectedValue = "Yes";
        rb_bioenergySub.SelectedValue = "Yes";
    }
    protected void setSessionVariables()
    {
        Session["mfgExBiofuel"] = textExtBio.Text;
        Session["mfgStrWood"] = textStrWood.Text;
        Session["mfgPulp"] = textPulp.Text;

        Session["recycleLT"] = textLTRecycle.Text;
        Session["recycleST"] = textSTRecycle.Text;
        Session["recyclePaper"] = textPaperRecycle.Text;

        Session["productLTStr"] = textLTStrFraction.Text;

        Session["disposalDump"] = textDump.Text;
        Session["disposalLandfill"] = textLandfill.Text;
        Session["disposalIncinVol"] = textIncinVol.Text;
        Session["disposalIncinBioenergy"] = textIncinBioenergy.Text;

        Session["substitutionProd"] = rb_substitution.SelectedValue;
        Session["substitutionEnergy"] = rb_bioenergySub.SelectedValue;

        if (rb_substitution.SelectedValue == "Yes" & rb_bioenergySub.SelectedValue == "Yes")
        {
            Session["subDisplacement"] = "No";
            Session["subBuilding"] = "Yes";
            Session["subBioenergyFactor"] = "60";
            Session["subLeakage"] = "Yes";
        }
    }

    protected void btn5next_Click(object sender, EventArgs e)
    {
        setSessionVariables();
        if (rb_substitution.SelectedValue == "No" | rb_bioenergySub.SelectedValue == "No")
            Response.Redirect("run-both-5substitution.aspx");
        else
            Response.Redirect("run-both-landcarb.aspx");
    }
    protected void btn5previous_Click(object sender, EventArgs e)
    {
        setSessionVariables();
        if (Page.Session["scale"] != null)
            Response.Redirect("run-" + Page.Session["scale"].ToString() + "-4landuse.aspx");
        else
            Response.Redirect("default.aspx");
    }
    protected void textLTStrFraction_TextChanged(object sender, EventArgs e)
    {
        calcTotalProductUse();
        textLandfill.Focus();
    }

    // Disposal
    protected void textLandfill_TextChanged(object sender, EventArgs e)
    {
        calcTotalDisposal();
        textIncinVol.Focus();
    }
    protected void textIncinVol_TextChanged(object sender, EventArgs e)
    {
        calcTotalDisposal();
        textIncinBioenergy.Focus();
    }
    protected void textIncinBioenergy_TextChanged(object sender, EventArgs e)
    {
        calcTotalDisposal();
        btn5next.Focus();
    }

    // Calculate totals
    protected void calcTotalManufacturing()
    {
        double totalValue = Convert.ToDouble(textStrWood.Text) + Convert.ToDouble(textExtBio.Text) + Convert.ToDouble(textPulp.Text);
        textTotalMfg.Text = totalValue.ToString();
    }
    protected void calcTotalProductUse()
    {
        double stValue = 100.0 - Convert.ToDouble(textLTStrFraction.Text);
        textSTStrFraction.Text = stValue.ToString();

        double totalValue = Convert.ToDouble(textLTStrFraction.Text) + Convert.ToDouble(textSTStrFraction.Text);
        textTotalUse.Text = totalValue.ToString();
    }
    protected void calcTotalDisposal()
    {
        double totalValue = Convert.ToDouble(textDump.Text) + Convert.ToDouble(textLandfill.Text) + Convert.ToDouble(textIncinVol.Text) + Convert.ToDouble(textIncinBioenergy.Text);
        textTotalDisposal.Text = totalValue.ToString();
    }

    protected void btn5reset_Click(object sender, EventArgs e)
    {
        setDefaultValues();
     
        calcTotalManufacturing();
        calcTotalProductUse();
        calcTotalDisposal();

        textExtBio.Focus();
    }
}
