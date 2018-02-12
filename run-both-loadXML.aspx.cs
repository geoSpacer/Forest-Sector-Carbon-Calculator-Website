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

using System.IO; // for directory listing and creation
using System.Xml;
using System.Collections.Generic;  // for sortedList
using System.Diagnostics; // for Request
// using System.Windows.Forms; // for messagebox

public partial class run_both_loadXML_code : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        }
    }

    protected void btn2return_Click(object sender, EventArgs e)
    {
        Response.Redirect("run-" + Page.Session["scale"].ToString() + ".aspx");
    }

    protected void btn2next_Click(object sender, EventArgs e)
    {
        if (FileUpload_xml.HasFile)
        {
            string landCarbDir = "LandCarbData31";
            string appPath = Request.PhysicalApplicationPath; // requires System.Diagnostics

            if (FileUpload_xml.FileName.Contains(Page.Session["scale"].ToString()))
            {
                // ***** Setup directories, copy standard files and write driver files
                DateTime centuryBegin = new DateTime(2001, 1, 1);
                DateTime currentDate = DateTime.Now;
                long elapsedTicks = (currentDate.Ticks - centuryBegin.Ticks) / 100000;
                string userDir = appPath + landCarbDir + "\\user" + elapsedTicks.ToString();
                Directory.CreateDirectory(userDir); // requires System.IO

                try
                {
                    FileUpload_xml.SaveAs(userDir + "\\" + FileUpload_xml.FileName);
                }
                catch (Exception ex)
                {
                    label_upload.Text = "ERROR: " + ex.Message.ToString();
                }

                setSessionFromFile(userDir + "\\" + FileUpload_xml.FileName);
                Response.Redirect("run-both-1site.aspx");
            }
            else
                label_upload.Text = "ERROR loading parameters - not " + Page.Session["scale"].ToString() + " level";
        }
    }

    protected void setSessionFromFile(string xmlParamFile)
    {
        manageDict_class manageDict = new manageDict_class();
        SortedList<string, ArrayList> managementDict = new SortedList<string, ArrayList>();
        SortedList<string, ArrayList> natDisturbDict = new SortedList<string, ArrayList>();
        managementDict = (SortedList<string, ArrayList>)Session["managementDict"];
        natDisturbDict = (SortedList<string, ArrayList>)Session["natDisturbDict"];

        XmlDocument oXmlDoc = new XmlDocument();
        XmlTextReader xmlParamReader = new XmlTextReader(xmlParamFile);
        xmlParamReader.Read();
        oXmlDoc.Load(xmlParamReader);
        XmlNode oNode = oXmlDoc.DocumentElement;

        Page.Session["region"] = oNode.SelectSingleNode("/RunParameters/SiteRegion").InnerText;
        Page.Session["ownership"] = oNode.SelectSingleNode("/RunParameters/SiteOwner").InnerText;
        Page.Session["elevClass"] = oNode.SelectSingleNode("/RunParameters/SiteElevationClass").InnerText;
 //       Page.Session["siteIndx"] = oNode.SelectSingleNode("/RunParameters/SiteSiteIndex").InnerText;

        Page.Session["currentYear"] = oNode.SelectSingleNode("/RunParameters/RunCurrentYear").InnerText;
        Page.Session["numSimYears"] = oNode.SelectSingleNode("/RunParameters/RunSimYears").InnerText;
        Page.Session["randomSeed"] = oNode.SelectSingleNode("/RunParameters/RunRandomSeed").InnerText;
        Page.Session["runName"] = oNode.SelectSingleNode("/RunParameters/RunName").InnerText;
        Page.Session["cellAreaHa"] = oNode.SelectSingleNode("/RunParameters/RunCellAreaHa").InnerText;

        Page.Session["mfgExBiofuel"] = oNode.SelectSingleNode("/RunParameters/UseMfgExBioFuel").InnerText;
        Page.Session["mfgStrWood"] = oNode.SelectSingleNode("/RunParameters/UseMfgStrWood").InnerText;
        Page.Session["mfgPulp"] = oNode.SelectSingleNode("/RunParameters/UseMfgPulp").InnerText;

        Page.Session["recycleLT"] = oNode.SelectSingleNode("/RunParameters/UseRecycleLT").InnerText;
        Page.Session["recycleST"] = oNode.SelectSingleNode("/RunParameters/UseRecycleST").InnerText;
        Page.Session["recyclePaper"] = oNode.SelectSingleNode("/RunParameters/UseRecyclePaper").InnerText;

        Page.Session["productLTStr"] = oNode.SelectSingleNode("/RunParameters/UseProductLTStr").InnerText;

        Page.Session["disposalDump"] = oNode.SelectSingleNode("/RunParameters/UseDisposalDump").InnerText;
        Page.Session["disposalLandfill"] = oNode.SelectSingleNode("/RunParameters/UseDisposalLandfill").InnerText;
        Page.Session["disposalIncinVol"] = oNode.SelectSingleNode("/RunParameters/UseDisposalIncinVol").InnerText;
        Page.Session["disposalIncinBioenergy"] = oNode.SelectSingleNode("/RunParameters/UseDisposalIncinBioenergy").InnerText;

        Page.Session["substitutionProd"] = oNode.SelectSingleNode("/RunParameters/SubstitutionProductDefault").InnerText;
        Page.Session["substitutionEnergy"] = oNode.SelectSingleNode("/RunParameters/SubstitutionBioenergyDefault").InnerText;
        Page.Session["subDisplacement"] = oNode.SelectSingleNode("/RunParameters/SubstitutionDisplacement").InnerText;
        Page.Session["subBuilding"] = oNode.SelectSingleNode("/RunParameters/SubstitutionBuildings").InnerText;
        Page.Session["subBioenergyFactor"] = oNode.SelectSingleNode("/RunParameters/SubstitutionBioenergyFactor").InnerText;
        Page.Session["subLeakage"] = oNode.SelectSingleNode("/RunParameters/SubstitutionLeakage").InnerText;

        manageDict.loadDictFromXML(managementDict, oNode.SelectNodes("/RunParameters/ManagementRegime"));
        manageDict.loadDictFromXML(natDisturbDict, oNode.SelectNodes("/RunParameters/NaturalDisturbanceRegime"));

    }
}
