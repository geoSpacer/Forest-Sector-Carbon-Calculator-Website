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
using System.Xml;
using System.IO;

using System.Diagnostics; // for Request
using System.Text.RegularExpressions;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing; // for Color
using System.Collections.Generic;
// using System.Windows.Forms; // for messagebox
using Ionic.Zip; // DotNetZip

// load FireRegimeBuiler and harvestRegimeBuilder from Frank Schnekenburger
using crb;

public partial class run_output : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SortedList<string, ArrayList> managementDict = new SortedList<string, ArrayList>();
            SortedList<string, ArrayList> natDisturbDict = new SortedList<string, ArrayList>();
            managementDict = (SortedList<string, ArrayList>)Session["managementDict"];
            natDisturbDict = (SortedList<string, ArrayList>)Session["natDisturbDict"];

            string landCarbDir = "LandCarbData31";
            // get the physical folder; currently on Charcoal
            string appPath = Request.PhysicalApplicationPath; // requires System.Diagnostics
            string outputLC = "";
            string elevationZoneDir;

            // check for session timeout and send user back to the beginning if timeout has occured
            if (Page.Session["scale"] == null)
                Response.Redirect("default.aspx");

            // ***** Setup directories, copy standard files and write driver files
            // DateTime centuryBegin = new DateTime(2001, 1, 1);
            DateTime currentDate = DateTime.Now;
            // long elapsedTicks = (currentDate.Ticks - centuryBegin.Ticks) / 100000;

            // Create unique directory for this user.  Copy parameter files from base to new directory
            // string userDir = appPath + landCarbDir + "\\user" + elapsedTicks.ToString();
            string userDir = Page.Session["userDir"].ToString();
            string[] standardFiles = Directory.GetFiles(appPath + landCarbDir + "\\base\\ParameterFiles", "*");

            TextBox1.Text = "Creating directories...";
            Directory.CreateDirectory(userDir); // requires System.IO
            Directory.CreateDirectory(userDir + "\\ParameterFiles");
            Directory.CreateDirectory(userDir + "\\OutputFiles");
            Directory.CreateDirectory(userDir + "\\OutputFiles\\EventTrackers");

            // Create regime files and fileSet object
            string harvestRegimeFile = userDir + "\\ParameterFiles\\Harvint.dvr";
            string fireRegimeFile = userDir + "\\ParameterFiles\\FireRegime.dvr";
            string yearAreaFile = userDir + "\\OutputFiles\\EventTrackers\\HarvestYearArea.txt"; // name of year-area file
            FileSet regimeFileSet = new FileSet(harvestRegimeFile, fireRegimeFile, yearAreaFile);

            // set session variables to hidden tags to perserve values when session times out
            userDirectory.Value = userDir;
            runDateTime.Value = currentDate.ToString();
            runName.Value = Page.Session["runName"].ToString();
            modelScale.Value = Page.Session["scale"].ToString();
            numCells.Value = (Convert.ToInt16(Page.Session["simRows"].ToString()) * Convert.ToInt16(Page.Session["simCols"].ToString())).ToString();

            // write out session variables to XML file in user directory
            writeXMLparameters(userDir);

            // copy standard files from base to the user input directory
            TextBox1.Text += "\nCopying standard files to user directory...";
            foreach (string file2copy in standardFiles)
            {
                try
                {
                    File.Copy(file2copy, userDir + "\\ParameterFiles\\" + Path.GetFileName(file2copy));
                }
                catch
                {
                    TextBox1.Text += "\n\nError copying " + Path.GetFileName(file2copy);
                }
            }

            // copy the climate productionAndDecayIndexFiles to the parameterFiles directory
            if (Page.Session["elevClass"].ToString() == "low")
                elevationZoneDir = "\\ProductionAndDecayIndexFiles\\" + Page.Session["region"].ToString() + "\\ElevationZone1\\";
            else if (Page.Session["elevClass"].ToString() == "mid")
                elevationZoneDir = "\\ProductionAndDecayIndexFiles\\" + Page.Session["region"].ToString() + "\\ElevationZone2\\";
            else if (Page.Session["elevClass"].ToString() == "high")
                elevationZoneDir = "\\ProductionAndDecayIndexFiles\\" + Page.Session["region"].ToString() + "\\ElevationZone3\\";
            else
                elevationZoneDir = "Error: Elevation Zone not found";

            try
            {
                File.Copy(appPath + landCarbDir + "\\base" + elevationZoneDir + "AnnualAbioticDecayIndex.prm", userDir + "\\ParameterFiles\\AnnualAbioticDecayIndex.prm");
                File.Copy(appPath + landCarbDir + "\\base" + elevationZoneDir + "AnnualProductionIndex.prm", userDir + "\\ParameterFiles\\AnnualProductionIndex.prm");
            }
            catch
            {
                TextBox1.Text += "\n\nError copying climate parameter files";
            }

            // write new driver files to new user directory
            writeSimul();

            writeWildFireRegime(managementDict, natDisturbDict, regimeFileSet);
            writeHarvestRegime(managementDict, regimeFileSet);
            writePrescribedFireRegime(managementDict, regimeFileSet);

            writeManufacturing();
            writeProductsUse();
            writeDisposal();

            // ***** Start the landCarb model
            Process process = null;

            // get the physical folder; currently on KOA, this returns E:\LandCarb
            // string appPath = Request.PhysicalApplicationPath; // requires System.Diagnostics

            try
            {
                string landCarbPath = appPath + @"LandCarb3394.exe";
                string parameterPath = userDir + @"\ParameterFiles";
                // TextBox1.Text += "\nParameter input path: " + parameterPath;

                process = new Process();
                process.StartInfo.FileName = landCarbPath;
                process.StartInfo.Arguments = "\"" + parameterPath + "\"";
                //process.StartInfo.Arguments = @"E:\LandCarb\LandCarbData3\Base\ParameterFiles";
                // You must set UseShellExecute to false if you want to set RedirectStandardOutput
                // to true. Otherwise, reading from the StandardOutput stream throws an exception.
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                outputLC = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                if (process.HasExited)
                {
                    TextBox1.Text += "\n\nDone: " + outputLC;
                }
                else
                {
                    TextBox1.Text += "\n\nProcessing problem: " + outputLC;
                }
            }
            catch (Exception ex)
            {
                TextBox1.Text += "\n\nFailed: " + ex.Message;
            }
            finally
            {
                // whatever happens, try killing the process in case it's still going
                try
                {
                    process.Kill();
                }
                catch
                {
                }
            }

            // run DotNetZip code to zip up output files
            zipOutputFiles();

            // ***** setup default graph
            TextBox_startYear.Text = (Convert.ToInt16(Page.Session["currentYear"].ToString()) - 100).ToString();

            if (outputLC.Contains("run completed"))
            {
                MultiView1.ActiveViewIndex = 0;
                drawChart_summary(1);
                menuSelection.Value = "comb 1";
            }
            else
                MultiView1.ActiveViewIndex = 1;

            TextBox_save.Text = TextBox1.Text;
        }

    }

    protected void zipOutputFiles()
    {
        try
        {
            using (ZipFile outputZipFile = new ZipFile())
            {
                string appPath = Request.PhysicalApplicationPath; // requires System.Diagnostics

                // note: this does not recurse directories! 
                //string[] filenames = System.IO.Directory.GetFiles(userDirectory.Value + "\\OutputFiles", "*");
                outputZipFile.AddDirectory(userDirectory.Value, runName.Value);
                outputZipFile.Comment = "Added by the Forest Sector Carbon Calculator";

                // This is just a sample, provided to illustrate the DotNetZip interface.  
                // This logic does not recurse through sub-directories.
                // If you are zipping up a directory, you may want to see the AddDirectory() method, 
                // which operates recursively. 
               // foreach (String filename in filenames)
               // {
                    // Console.WriteLine("Adding {0}...", filename);
                //    ZipEntry e = outputZipFile.AddFile(filename);
                //    e.Comment = "Added by the Forest Sector Carbon Calculator";
               // }

                // outputZipFile.Comment = String.Format("This zip archive was created by the CreateZip example application on machine '{0}'",
                //   System.Net.Dns.GetHostName());

                outputZipFile.Save(userDirectory.Value + "\\FSCC_runOutput.zip");
            }

        }
        catch (System.Exception ex1)
        {
            System.Console.Error.WriteLine("exception: " + ex1);
        }

    }

    protected void writeXMLparameters(string userDir)
    {
        SortedList<string, ArrayList> managementDict = new SortedList<string, ArrayList>();
        SortedList<string, ArrayList> natDisturbDict = new SortedList<string, ArrayList>();

        managementDict = (SortedList<string, ArrayList>)Page.Session["managementDict"];
        natDisturbDict = (SortedList<string, ArrayList>)Page.Session["natDisturbDict"];

        XmlTextWriter paramWriter = new XmlTextWriter(userDir + "\\FSCC_" + modelScale.Value + "_parameters.xml", null);
        paramWriter.Formatting = Formatting.Indented;
        paramWriter.WriteStartDocument();
        paramWriter.WriteComment("Forest Sector Carbon Calculator");
        paramWriter.WriteComment("http://landcarb.forestry.oregonstate.edu");

        // Write first element
        paramWriter.WriteStartElement("RunParameters");

        paramWriter.WriteStartElement("RunDateTime");
        paramWriter.WriteString(runDateTime.Value);
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("RunScale");
        paramWriter.WriteString(modelScale.Value);
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("SiteRegion");
        paramWriter.WriteString(Page.Session["region"].ToString());
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("SiteOwner");
        paramWriter.WriteString(Page.Session["ownership"].ToString());
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("SiteElevationClass");
        paramWriter.WriteString(Page.Session["elevClass"].ToString());
        paramWriter.WriteEndElement();

        //paramWriter.WriteStartElement("SiteSiteIndex");
        //paramWriter.WriteString(Page.Session["siteIndx"].ToString());
        //paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("RunCurrentYear");
        paramWriter.WriteString(Page.Session["currentYear"].ToString());
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("RunSimYears");
        paramWriter.WriteString(Page.Session["numSimYears"].ToString());
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("RunRandomSeed");
        paramWriter.WriteString(Page.Session["randomSeed"].ToString());
        paramWriter.WriteEndElement();
        
        paramWriter.WriteStartElement("RunName");
        paramWriter.WriteString(Page.Session["runName"].ToString());
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("RunCellAreaHa");
        paramWriter.WriteString(Page.Session["cellAreaHa"].ToString());
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("RunCellNum");
        paramWriter.WriteString(numCells.Value);
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("UseMfgExBioFuel");
        paramWriter.WriteString(Page.Session["mfgExBiofuel"].ToString());
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("UseMfgStrWood");
        paramWriter.WriteString(Page.Session["mfgStrWood"].ToString());
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("UseMfgPulp");
        paramWriter.WriteString(Page.Session["mfgPulp"].ToString());
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("UseRecycleLT");
        paramWriter.WriteString(Page.Session["recycleLT"].ToString());
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("UseRecycleST");
        paramWriter.WriteString(Page.Session["recycleST"].ToString());
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("UseRecyclePaper");
        paramWriter.WriteString(Page.Session["recyclePaper"].ToString());
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("UseProductLTStr");
        paramWriter.WriteString(Page.Session["productLTStr"].ToString());
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("UseDisposalDump");
        paramWriter.WriteString(Page.Session["disposalDump"].ToString());
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("UseDisposalLandfill");
        paramWriter.WriteString(Page.Session["disposalLandfill"].ToString());
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("UseDisposalIncinVol");
        paramWriter.WriteString(Page.Session["disposalIncinVol"].ToString());
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("UseDisposalIncinBioenergy");
        paramWriter.WriteString(Page.Session["disposalIncinBioenergy"].ToString());
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("SubstitutionProductDefault");
        paramWriter.WriteString(Page.Session["substitutionProd"].ToString());
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("SubstitutionBioenergyDefault");
        paramWriter.WriteString(Page.Session["substitutionEnergy"].ToString());
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("SubstitutionDisplacement");
        paramWriter.WriteString(Page.Session["subDisplacement"].ToString());
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("SubstitutionBuildings");
        paramWriter.WriteString(Page.Session["subBuilding"].ToString());
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("SubstitutionBioenergyFactor");
        paramWriter.WriteString(Page.Session["subBioenergyFactor"].ToString());
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("SubstitutionLeakage");
        paramWriter.WriteString(Page.Session["subLeakage"].ToString());
        paramWriter.WriteEndElement();

        foreach (KeyValuePair<string, ArrayList> dictEntry in managementDict)
        {
            paramWriter.WriteStartElement("ManagementRegime");
            writeXMLRegime(dictEntry, paramWriter);
            paramWriter.WriteEndElement();
        }

        foreach (KeyValuePair<string, ArrayList> dictEntry in natDisturbDict)
        {
            paramWriter.WriteStartElement("NaturalDisturbanceRegime");
            writeXMLRegime(dictEntry, paramWriter);
            paramWriter.WriteEndElement();
        }

        paramWriter.WriteEndDocument();
        paramWriter.Close();

    }

    protected void writeXMLRegime(KeyValuePair<string, ArrayList> dictEntry, XmlTextWriter paramWriter)
    {
        paramWriter.WriteStartElement("TreatmentClass");
        if (dictEntry.Key.Length == 11)
            paramWriter.WriteString("PrimaryRegime");
        else
            paramWriter.WriteString("SecondaryRegime");
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("Type");
        paramWriter.WriteString(dictEntry.Value[0].ToString());
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("StartYear");
        paramWriter.WriteString(dictEntry.Key.Substring(1, 4));
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("EndYear");
        paramWriter.WriteString(dictEntry.Key.Substring(7, 4));
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("Interval-Offset");
        paramWriter.WriteString(dictEntry.Value[1].ToString());
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("Disturbed-Size");
        paramWriter.WriteString(dictEntry.Value[2].ToString());
        paramWriter.WriteEndElement();

        paramWriter.WriteStartElement("Util-Severity-Supp");
        paramWriter.WriteString(dictEntry.Value[3].ToString());
        paramWriter.WriteEndElement();

        if (dictEntry.Value.Count == 5)
        {
            paramWriter.WriteStartElement("Placement");
            paramWriter.WriteString(dictEntry.Value[4].ToString());
            paramWriter.WriteEndElement();
        }
    }

    protected void listInputSelections()
    {
        manageDict_class manageDict = new manageDict_class();
        displayDict_class displayDict = new displayDict_class();
        SortedList<string, ArrayList> managementDict = new SortedList<string, ArrayList>();
        SortedList<string, ArrayList> natDisturbDict = new SortedList<string, ArrayList>();
        managementDict = (SortedList<string, ArrayList>)Session["managementDict"];
        natDisturbDict = (SortedList<string, ArrayList>)Session["natDisturbDict"];

        XmlDocument oXmlDoc = new XmlDocument();
        XmlTextReader xmlParamReader = new XmlTextReader(userDirectory.Value + "\\FSCC_" + modelScale.Value + "_parameters.xml");
        xmlParamReader.Read();
        oXmlDoc.Load(xmlParamReader);
        XmlNode oNode = oXmlDoc.DocumentElement;
        XmlNodeList oNodeSelList = oNode.SelectNodes("/RunParameters/RunScale");

        TextBox1.Text += "\nRun Time Stamp: " + oNode.SelectSingleNode("/RunParameters/RunDateTime").InnerText + " PST";
        TextBox1.Text += "\nRun Scale: " + oNode.SelectSingleNode("/RunParameters/RunScale").InnerText;
        TextBox1.Text += "\nRun Name: " + oNode.SelectSingleNode("/RunParameters/RunName").InnerText;

        TextBox1.Text += "\n\nRegion Name: " + oNode.SelectSingleNode("/RunParameters/SiteRegion").InnerText;
        TextBox1.Text += "\nOwnership: " + oNode.SelectSingleNode("/RunParameters/SiteOwner").InnerText;
        TextBox1.Text += "\nElevation Class: " + oNode.SelectSingleNode("/RunParameters/SiteElevationClass").InnerText;
//        TextBox1.Text += "\nSite Index: " + oNode.SelectSingleNode("/RunParameters/SiteSiteIndex").InnerText;

        TextBox1.Text += "\n\nCurrent Year: " + oNode.SelectSingleNode("/RunParameters/RunCurrentYear").InnerText;
        TextBox1.Text += "\nSimulation Run Time (years): " + oNode.SelectSingleNode("/RunParameters/RunSimYears").InnerText;
        TextBox1.Text += "\nUncertainty Analysis: " + oNode.SelectSingleNode("/RunParameters/RunRandomSeed").InnerText;
        TextBox1.Text += "\nNumber of Cells: " + oNode.SelectSingleNode("/RunParameters/RunCellNum").InnerText;
        TextBox1.Text += "\nCell Area (Ha): " + oNode.SelectSingleNode("/RunParameters/RunCellAreaHa").InnerText;

        TextBox1.Text += "\n\nPost-Harvest Manufacturing Carbon Use:";
        TextBox1.Text += "\nBioenergy Production External Use: " + oNode.SelectSingleNode("/RunParameters/UseMfgExBioFuel").InnerText + "%";
        TextBox1.Text += "\nStructural Wood Production: " + oNode.SelectSingleNode("/RunParameters/UseMfgStrWood").InnerText + "%";
        TextBox1.Text += "\nPulp Wood Production: " + oNode.SelectSingleNode("/RunParameters/UseMfgPulp").InnerText + "%";

        TextBox1.Text += "\n\nPost-Harvest Recycling Carbon Use:";
        TextBox1.Text += "\nLong Term Structure Recycling: " + oNode.SelectSingleNode("/RunParameters/UseRecycleLT").InnerText + "%";
        TextBox1.Text += "\nShort Term Structure Recycling: " + oNode.SelectSingleNode("/RunParameters/UseRecycleST").InnerText + "%";
        TextBox1.Text += "\nPaper Recycling: " + oNode.SelectSingleNode("/RunParameters/UseRecyclePaper").InnerText + "%";

        TextBox1.Text += "\n\nPost-Harvest Product Carbon Use:";
        TextBox1.Text += "\nLong Term Structure Fraction: " + oNode.SelectSingleNode("/RunParameters/UseProductLTStr").InnerText + "%";
        TextBox1.Text += "\nShort Term Structure Fraction: ";
        int stStrFraction = 100 - Convert.ToInt16(oNode.SelectSingleNode("/RunParameters/UseProductLTStr").InnerText);
        TextBox1.Text += stStrFraction.ToString() + "%";

        TextBox1.Text += "\n\nPost-Harvest Carbon Disposal:";
        TextBox1.Text += "\nOpen Dump Fraction: " + oNode.SelectSingleNode("/RunParameters/UseDisposalDump").InnerText + "%";
        TextBox1.Text += "\nLandfill Fraction: " + oNode.SelectSingleNode("/RunParameters/UseDisposalLandfill").InnerText + "%";
        TextBox1.Text += "\nIncineration for Volume Reduction: " + oNode.SelectSingleNode("/RunParameters/UseDisposalIncinVol").InnerText + "%";
        TextBox1.Text += "\nIncineration for Bioenergy Recovery: " + oNode.SelectSingleNode("/RunParameters/UseDisposalIncinBioenergy").InnerText + "%";

        TextBox1.Text += "\n\nPost-Harvest Carbon Substitution:";
        TextBox1.Text += "\nDefault Product Substitution: " + oNode.SelectSingleNode("/RunParameters/SubstitutionProductDefault").InnerText;
        TextBox1.Text += "\nDefault Bioenergy Substitution: " + oNode.SelectSingleNode("/RunParameters/SubstitutionBioenergyDefault").InnerText;
        TextBox1.Text += "\nConstant Displacement Factor: " + oNode.SelectSingleNode("/RunParameters/SubstitutionDisplacement").InnerText;
        TextBox1.Text += "\nLong-term Structure Relationship: " + oNode.SelectSingleNode("/RunParameters/SubstitutionBuildings").InnerText;
        TextBox1.Text += "\nBioenergy Displacement Factor: " + oNode.SelectSingleNode("/RunParameters/SubstitutionBioenergyFactor").InnerText;
        TextBox1.Text += "\nFossil Carbon Leakage: " + oNode.SelectSingleNode("/RunParameters/SubstitutionLeakage").InnerText;

        manageDict.loadDictFromXML(managementDict, oNode.SelectNodes("/RunParameters/ManagementRegime"));
        manageDict.loadDictFromXML(natDisturbDict, oNode.SelectNodes("/RunParameters/NaturalDisturbanceRegime"));
        TextBox1.Text += "\n\n***Management Regimes:\n";
        if (modelScale.Value == "stand")
        {
            foreach (KeyValuePair<string, ArrayList> dictEntry in managementDict)
                TextBox1.Text += displayDict.displayManagementStand(dictEntry) + "\n";
        }
        else
        {
            foreach (KeyValuePair<string, ArrayList> dictEntry in managementDict)
                TextBox1.Text += displayDict.displayManagement(dictEntry) + "\n";
        }

        TextBox1.Text += "\n***Disturbance Regimes:\n";
        foreach (KeyValuePair<string, ArrayList> dictEntry in natDisturbDict)
            TextBox1.Text += displayDict.displayNatDisturb(dictEntry) + "\n";
    }

    protected void showTextFile(string fileClass, string selectedFileName)
    {
        Label1.Text = "Now viewing:  " + formatFileName(selectedFileName);
        string line;
        string fileName;

        if (fileClass == "out")
            fileName = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\" + selectedFileName;
        else if (fileClass == "outEvent")
            fileName = userDirectory.Value + "\\OutputFiles\\EventTrackers\\" + selectedFileName;
        else if (fileClass == "log")
            fileName = userDirectory.Value + "\\OutputFiles\\" + selectedFileName;
        else
            fileName = userDirectory.Value + "\\ParameterFiles\\" + selectedFileName;

        // Read the file and display it line by line.
        TextBox1.Text = "";
        if (selectedFileName == "listInputParameters")
        {
            // ***** list input selections in output text box
            TextBox1.Text += "Input Selections for Current Simulation *****\n";
            listInputSelections();
        }
        else
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    TextBox1.Text += line + "\n";
                }
            }
        }
    }

    protected string formatFileName(string unformattedFileName)
    {
        // unformattedFileName = Path.GetFileName(unformattedFileName);
        unformattedFileName = unformattedFileName.Replace("_stats", "");
        unformattedFileName = unformattedFileName.Replace(".csv", "");
        try { return (Regex.Replace(unformattedFileName, @"([a-z])([A-Z])", @"$1 $2", RegexOptions.None)); }
        catch { return ("Error"); }
    }

    protected void setupChart(bool drawZeroLine)
    {
        System.Drawing.Color bgColor = System.Drawing.ColorTranslator.FromHtml("#D7D6AE");

        // Set Border Skin
        Chart1.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
        Chart1.BorderSkin.PageColor = bgColor;

        // Set Titles and title fonts
        // Chart1.ChartAreas["ChartArea1"].AxisX.TitleFont = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);
        Chart1.Titles.Add("title0");
        Chart1.Titles[0].Name = "outChartTitle";
        Chart1.Titles["outChartTitle"].Font = new Font("Microsoft Sans Serif", 12, FontStyle.Bold);

        Chart1.Titles.Add("title1");
        Chart1.Titles[1].Name = "chartFooter";
        Chart1.Titles["chartFooter"].Alignment = System.Drawing.ContentAlignment.BottomRight;
        Chart1.Titles["chartFooter"].Docking = System.Web.UI.DataVisualization.Charting.Docking.Bottom;
        Chart1.Titles["chartFooter"].IsDockedInsideChartArea = true;
        Chart1.Titles["chartFooter"].Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
        Chart1.Titles["chartFooter"].Text = runName.Value + " - " + modelScale.Value + " level run - " + runDateTime.Value + " PST";

        Chart1.ChartAreas["ChartArea1"].BackColor = Color.FloralWhite;
        Chart1.ChartAreas["ChartArea1"].AxisX.TitleFont = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
        Chart1.ChartAreas["ChartArea1"].AxisY.TitleFont = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
        Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.Silver;
        Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.Silver;
        Chart1.ChartAreas["ChartArea1"].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
        Chart1.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = true;

        // Create legend
        Chart1.Legends.Clear();
        Chart1.Legends.Add("outChartLegend");
     //   Chart1.Legends["outChartLegend"].DockedToChartArea = "ChartArea1";
        Chart1.Legends["outChartLegend"].LegendStyle = LegendStyle.Row;
        Chart1.Legends["outChartLegend"].Docking = Docking.Top;
        Chart1.Legends["outChartLegend"].Alignment = StringAlignment.Center;
        Chart1.Legends["outChartLegend"].Font = new Font("Microsoft Sans Serif", 10);
 
        //Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 100;
        //Chart1.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Number;
        //Chart1.ChartAreas["ChartArea1"].AxisX.IntervalOffset = 10;
        //Chart1.ChartAreas["ChartArea1"].AxisX.IntervalOffsetType = DateTimeIntervalType.Auto;

        Chart1.Series.Clear();
        if (drawZeroLine)
        {
            addChartSeries("seriesZeroLine", SeriesChartType.Line, "", Color.Brown);
            Chart1.Series["seriesZeroLine"].IsVisibleInLegend = false;
            Chart1.Series["seriesZeroLine"].BorderWidth = 2;
            Chart1.Series["seriesZeroLine"].Points.AddXY(Convert.ToDouble(TextBox_startYear.Text) + 1, 0.0);
            Chart1.Series["seriesZeroLine"].Points.AddXY(Convert.ToDouble(Page.Session["currentYear"]) + Convert.ToDouble(Page.Session["numSimYears"]), 0.0);
        }
    }

    protected void drawChart(string selectedFileName)
    {
        readOutput_class readOutput = new readOutput_class();
        graphTitle_class graphTitle = new graphTitle_class();
        string fileName;
        string[] fieldNames = {""};
        ArrayList dataArray;
        int startYear = Convert.ToInt16(TextBox_startYear.Text);

        // open CSV file and read into array
        fileName = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\" + selectedFileName;
        dataArray = readOutput.readCSV(fileName, ref fieldNames);

        if (dataArray.Count == 0)
            returnErrorText(fileName);
        else
        {
            if (selectedFileName.EndsWith("Mass.csv") & selectedFileName != "HarvestMass.csv")
            {
                setupChart(false);
                Chart1.ChartAreas["ChartArea1"].AxisY.Title = "Carbon Stores (Mg C / ha)";
            }
            else if (selectedFileName.EndsWith("Balance.csv") & selectedFileName != "BiofuelBalance.csv")
            {
                setupChart(true);
                Chart1.ChartAreas["ChartArea1"].AxisY.Title = "Carbon Balance (Mg C / ha / year)";
            }
            else
            {
                setupChart(true);
                Chart1.ChartAreas["ChartArea1"].AxisY.Title = "Carbon Flow (Mg C / ha / year)";
            }

            Chart1.Titles["outChartTitle"].Text = graphTitle.lookupGraphTitle(selectedFileName);

            // set chart series info
            addChartSeries("SeriesMean", SeriesChartType.Line, "Mean", Color.DarkSlateBlue);
            Chart1.Series["SeriesMean"].BorderWidth = 3;
            readOutput.loadChartSeries(fileName, Chart1.Series["SeriesMean"], dataArray, fieldNames, "mean", startYear);

            if (Check_minMax.Checked)
            {
                addChartSeries("SeriesMin", SeriesChartType.Line, "Min", Color.DarkSeaGreen);
                readOutput.loadChartSeries(fileName, Chart1.Series["SeriesMin"], dataArray, fieldNames, "min", startYear);

                addChartSeries("SeriesMax", SeriesChartType.Line, "Max", Color.Goldenrod);
                readOutput.loadChartSeries(fileName, Chart1.Series["SeriesMax"], dataArray, fieldNames, "max", startYear);
            }
        }

        setChartAxis();
        dataArray.Clear();
    }

    protected void drawChart_event(string selectedFileName)
    {
        readOutput_class readOutput = new readOutput_class();
        graphTitle_class graphTitle = new graphTitle_class();
        string fileName;
        string[] fieldNames = {""};
        ArrayList dataArray;
        int startYear = Convert.ToInt16(TextBox_startYear.Text);
        SeriesChartType eventChartType;

        // setup chart type and titles
        setupChart(false);

        if (modelScale.Value == "stand")
        {
            eventChartType = SeriesChartType.StackedColumn;
            Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = 100.0;
        }
        else
            eventChartType = SeriesChartType.StackedArea;

        Chart1.ChartAreas["ChartArea1"].AxisY.Title = "Percent Area Disturbed";
        Chart1.Titles["outChartTitle"].Text = graphTitle.lookupGraphTitle(selectedFileName);

        // open CSV file and read into array
        fileName = userDirectory.Value + "\\OutputFiles\\EventTrackers\\" + selectedFileName;
        dataArray = readOutput.readCSV(fileName, ref fieldNames);

        if (dataArray.Count == 0)
            returnErrorText(fileName);
        else if (selectedFileName == "HarvestYearArea.txt")
        {
            addChartSeries("TargetPct", eventChartType, "Percent of Target", Color.DarkSlateBlue);
            readOutput.loadChartSeries(fileName, Chart1.Series["TargetPct"], dataArray, fieldNames, "PercentOfTarget", startYear);

            setChartAxis();
        }
        else
        {
            // set chart series info
            addChartSeries("SeriesVeryLow", eventChartType, "Very Low", Color.DarkSlateBlue);
            readOutput.loadChartSeries(fileName, Chart1.Series["SeriesVeryLow"], dataArray, fieldNames, "VeryLow%", startYear);

            addChartSeries("SeriesLow", eventChartType, "Low", Color.DarkSeaGreen);
            readOutput.loadChartSeries(fileName, Chart1.Series["SeriesLow"], dataArray, fieldNames, "Low%", startYear);

            addChartSeries("SeriesMedium", eventChartType, "Medium", Color.Goldenrod);
            readOutput.loadChartSeries(fileName, Chart1.Series["SeriesMedium"], dataArray, fieldNames, "Medium%", startYear);

            addChartSeries("SeriesHigh", eventChartType, "High", Color.SaddleBrown);
            readOutput.loadChartSeries(fileName, Chart1.Series["SeriesHigh"], dataArray, fieldNames, "High%", startYear);

            addChartSeries("SeriesVeryHigh", eventChartType, "Very High", Color.Red);
            readOutput.loadChartSeries(fileName, Chart1.Series["SeriesVeryHigh"], dataArray, fieldNames, "VeryHigh%", startYear);

            setChartAxis();
        }

        dataArray.Clear();
    }

    protected void drawChart_summary(int selectedListItem)
    {
        readOutput_class readOutput = new readOutput_class();
        graphTitle_class graphTitle = new graphTitle_class();
        int startYear = Convert.ToInt16(TextBox_startYear.Text);

        string fileName1 = "", legendName1 = "";
        string fileName2 = "", legendName2 = "";
        string fileName3 = "", legendName3 = "";
        string fileName4 = "", legendName4 = "";
        string chartTitle = "";
        SeriesChartType chartTypeName;
        Color series1color = Color.DarkSeaGreen;
        Color series2color = Color.DarkSlateBlue;
        Color series3color = Color.Goldenrod;
        Color series4color = Color.DarkMagenta;

        if (selectedListItem <= 1)
        {
            fileName1 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\TotalForestEcosystemMass.csv";
            legendName1 = "Ecosystem";
            fileName2 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\TotalForestProductMass.csv";
            legendName2 = "Products";
            fileName3 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\TotalSubstitutionMass.csv";
            legendName3 = "Substitution";
            fileName4 = "skip";
            chartTitle = graphTitle.lookupGraphTitle("TotalForestSectorMass.csv");
        }
        else if (selectedListItem == 2)
        {
            fileName1 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\ProductsInUseMass.csv";
            legendName1 = "Wood Products in Use";
            fileName2 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\DisposalMass.csv";
            legendName2 = "Disposal";
            fileName3 = "skip";
            legendName3 = "";
            fileName4 = "skip";
            chartTitle = graphTitle.lookupGraphTitle("TotalForestProductMass.csv");
        }
        else if (selectedListItem == 3)
        {
            fileName1 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\LiveMass.csv";
            legendName1 = "Live Ecosystem";
            fileName2 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\DeadMass.csv";
            legendName2 = "Dead Ecosystem";
            fileName3 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\StableMass.csv";
            legendName3 = "Stable (Soil)";
            fileName4 = "skip";
            chartTitle = graphTitle.lookupGraphTitle("TotalForestEcosystemMass.csv");
        }
        else if (selectedListItem == 4)
        {
            fileName1 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\ProductSubstitutionMass.csv";
            legendName1 = "Product Substitution";
            fileName2 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\BiofuelMass.csv";
            legendName2 = "Biofuel Offset";
            fileName3 = "skip";
            legendName3 = "";
            fileName4 = "skip";
            chartTitle = graphTitle.lookupGraphTitle("TotalSubstitutionMass.csv");
        }
        else if (selectedListItem == 10)
        {
            fileName1 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\ForestNep.csv";
            legendName1 = "Net Ecosystem Production";
            fileName2 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\ForestNpp.csv";
            legendName2 = "Net Primary Production";
            fileName3 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\ForestRh.csv";
            legendName3 = "Heterotrophic Respiration";
            fileName4 = "skip";
            chartTitle = "Forest Ecosystem Carbon Flows";
            series2color = Color.DarkSeaGreen;
            series1color = Color.DarkSlateBlue;
        }
        else if (selectedListItem == 11)
        {
            fileName1 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\TotalForestSectorBalance.csv";
            legendName1 = "Sector";
            fileName2 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\TotalForestEcosystemBalance.csv";
            legendName2 = "Ecosystem";
            fileName3 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\TotalForestProductBalance.csv";
            legendName3 = "Products";
            fileName4 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\TotalSubstitutionBalance.csv";
            legendName4 = "Substitution";
            chartTitle = graphTitle.lookupGraphTitle("TotalForestSectorBalance.csv");
            series2color = Color.DarkSeaGreen;
            series1color = Color.DarkSlateBlue;
        }
        else if (selectedListItem == 12)
        {
            fileName1 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\TotalForestEcosystemBalance.csv";
            legendName1 = "Forest Ecosystem";
            fileName2 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\LiveBalance.csv";
            legendName2 = "Live";
            fileName3 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\DeadBalance.csv";
            legendName3 = "Dead";
            fileName4 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\StableBalance.csv";
            legendName4 = "Stable (soil)";
            chartTitle = graphTitle.lookupGraphTitle("TotalForestEcosystemBalance.csv");
            series2color = Color.DarkSeaGreen;
            series1color = Color.DarkSlateBlue;
        }
        else if (selectedListItem == 13)
        {
            fileName1 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\TotalForestProductBalance.csv";
            legendName1 = "Forest Product";
            fileName2 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\ProductsInUseBalance.csv";
            legendName2 = "Products In Use";
            fileName3 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\DisposalBalance.csv";
            legendName3 = "Disposal";
            fileName4 = "skip";
            legendName4 = "";
            chartTitle = graphTitle.lookupGraphTitle("TotalForestProductBalance.csv");
            series2color = Color.DarkSeaGreen;
            series1color = Color.DarkSlateBlue;
        }
        else if (selectedListItem == 14)
        {
            fileName1 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\TotalSubstitutionBalance.csv";
            legendName1 = "Forest Substitution";
            fileName2 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\ProductSubstitutionBalance.csv";
            legendName2 = "Product Substitution";
            fileName3 = userDirectory.Value + "\\OutputFiles\\RasterStatistics\\BiofuelBalance.csv";
            legendName3 = "Biofuel Production";
            fileName4 = "skip";
            chartTitle = graphTitle.lookupGraphTitle("TotalSubstitutionBalance.csv");
            series2color = Color.DarkSeaGreen;
            series1color = Color.DarkSlateBlue;
        }

        if (fileName1.EndsWith("Mass.csv"))
        {
            setupChart(false);
            Chart1.ChartAreas["ChartArea1"].AxisY.Title = "Carbon Stores (Mg C / ha)";
        }
        else if (fileName1.EndsWith("Balance.csv"))
        {
            setupChart(true);
            Chart1.ChartAreas["ChartArea1"].AxisY.Title = "Carbon Balance (Mg C / ha / year)";
        }
        else
        {
            setupChart(true);
            Chart1.ChartAreas["ChartArea1"].AxisY.Title = "Carbon Flow (Mg C / ha / year)";
        }

        Chart1.Titles["outChartTitle"].Text = chartTitle;
        
        // set chart series info
        if (selectedListItem < 10)
            chartTypeName = SeriesChartType.StackedArea;
        else
            chartTypeName = SeriesChartType.Line;

        if (fileName1 != "skip")
        {
            addChartSeries("seriesFirst", chartTypeName, legendName1, series1color);
            Chart1.Series["seriesFirst"].BorderWidth = 3;
            if (readOutput.readCSV2Series(fileName1, Chart1.Series["seriesFirst"], "mean", startYear))
                returnErrorText(fileName1);
        }
        if (fileName2 != "skip")
        {
            addChartSeries("seriesSecond", chartTypeName, legendName2, series2color);
            if (readOutput.readCSV2Series(fileName2, Chart1.Series["seriesSecond"], "mean", startYear))
                returnErrorText(fileName2);
        }
        if (fileName3 != "skip")
        {
            addChartSeries("seriesThird", chartTypeName, legendName3, series3color);
            if (readOutput.readCSV2Series(fileName3, Chart1.Series["seriesThird"], "mean", startYear))
                returnErrorText(fileName3);
        }
        if (fileName4 != "skip")
        {
            addChartSeries("seriesFourth", chartTypeName, legendName4, series4color);
            if (readOutput.readCSV2Series(fileName4, Chart1.Series["seriesFourth"], "mean", startYear))
                returnErrorText(fileName4);
        }

        setChartAxis();
    }

    protected void returnErrorText(string fileName)
    {
        MultiView1.ActiveViewIndex = 1;
        TextBox1.Text += "\n\n****** Error reading: " + fileName;
    }

    protected void setChartAxis()
    {
        if (TextBox_minY.Text == "")
            TextBox_minY.Text = "-50";
        if (TextBox_maxY.Text == "")
            TextBox_maxY.Text = "50";

        // reset Y scale if desired
        if (!Check_autoScale.Checked)
        {
            Chart1.ChartAreas["ChartArea1"].AxisY.Minimum = Convert.ToDouble(TextBox_minY.Text);
            Chart1.ChartAreas["ChartArea1"].AxisY.Maximum = Convert.ToDouble(TextBox_maxY.Text);
        }
    }

    protected void addChartSeries(string seriesName, SeriesChartType chartTypeName, string legendName, Color seriesColor)
    {
        Chart1.Series.Add(seriesName);

        if (chartTypeName == SeriesChartType.Line)
            Chart1.Series[seriesName].BorderWidth = 2;
        if (chartTypeName == SeriesChartType.StackedColumn)
        {
            Chart1.Series[seriesName]["PointWidth"] = "10";
            Chart1.Series[seriesName]["DrawingStyle"] = "Emboss";
        }

        Chart1.Series[seriesName].ChartType = chartTypeName;
        Chart1.Series[seriesName].Color = seriesColor;
        Chart1.Series[seriesName].LegendText = legendName;
    }

    protected void TextBox_startYear_TextChanged(object sender, EventArgs e)
    {
        selectGraph();
    }
    protected void TextBox_minY_TextChanged(object sender, EventArgs e)
    {
        selectGraph();
    }
    protected void TextBox_maxY_TextChanged(object sender, EventArgs e)
    {
        selectGraph();
    }
    protected void Check_autoScale_CheckedChanged(object sender, EventArgs e)
    {
        if (Check_autoScale.Checked == true)
        {
            TextBox_minY.Enabled = false;
            TextBox_maxY.Enabled = false;
        }
        else
        {
            TextBox_minY.Enabled = true;
            TextBox_maxY.Enabled = true;
        }

        selectGraph();
    }

    protected void menuOutput_MenuItemClick(object sender, MenuEventArgs e)
    {
        menuSelection.Value = e.Item.Value;
        string[] menuSelArray = menuSelection.Value.Split(' ');

        Check_minMax.Visible = false;
        MultiView1.ActiveViewIndex = 0;
        if (menuSelArray[0] == "comb")
            drawChart_summary(Convert.ToInt16(menuSelArray[1]));
        else if (menuSelArray[0] == "ind")
        {
            Check_minMax.Visible = true;
            drawChart(menuSelArray[1]);
        }
        else if (menuSelArray[0] == "event")
            drawChart_event(menuSelArray[1]);
        else
        {
            MultiView1.ActiveViewIndex = 1;
            if (menuSelArray[1] == "outWindow")
            {
                Label1.Text = "Run Output";
                TextBox1.Text = TextBox_save.Text;
            }
            else if (menuSelArray[1] == "download")
            {
                Response.ContentType = "application/x-zip-compressed";
                Response.AppendHeader("Content-Disposition", "attachment; filename=FSCC_runOutput.zip");
                Response.TransmitFile(userDirectory.Value + "\\FSCC_runOutput.zip");
                Response.End();
            }
            else
                showTextFile(menuSelArray[0], menuSelArray[1]);
        }

    }
    protected void Check_minMax_CheckedChanged(object sender, EventArgs e)
    {
        selectGraph();
    }

    protected void selectGraph()
    {
        string[] menuSelArray = menuSelection.Value.Split(' ');

        if (menuSelArray[0] == "comb")
            drawChart_summary(Convert.ToInt16(menuSelArray[1]));
        else if (menuSelArray[0] == "ind")
            drawChart(menuSelArray[1]);
        else if (menuSelArray[0] == "event")
            drawChart_event(menuSelArray[1]);
    }
}


// ************ Functions to write out parameter files ****************
public partial class run_output
{
    protected void writeSimul()
    {
        // create filename
        string landCarbDir = "LandCarbData31";
        TextBox1.Text += "\nWriting Simul.dvr file...";
        string filename = userDirectory.Value + "\\ParameterFiles\\Simul.dvr";
        int spinupYears = Convert.ToInt16(Page.Session["currentYear"].ToString()) - Convert.ToInt16(Page.Session["simStartYear"].ToString());

        int firstCalendarYear = Convert.ToInt16(Page.Session["simStartYear"].ToString());
        int lastCalendarYear = Convert.ToInt16(Page.Session["currentYear"].ToString()) + Convert.ToInt16(Page.Session["numSimYears"].ToString());
        
        // set range validator for start year text box above graph
        RangeValidator_startYear.MinimumValue = firstCalendarYear.ToString();
        RangeValidator_startYear.MaximumValue = lastCalendarYear.ToString();
        RangeValidator_startYear.ErrorMessage = "Please enter a start year between " + firstCalendarYear.ToString() + " and " + lastCalendarYear.ToString();

        // open new file handle for simul.dvr
        using (StreamWriter sw = new StreamWriter(filename))
        {
            sw.WriteLine("# LandCarb Parameter File\n# Study site: West Cascades\n");

            sw.WriteLine("#####################################################################\n#####################################################################");
            sw.WriteLine("# Beginning of the parameters I added to get the current Calculator\n# file working with the latest version of LandCarb");

            sw.WriteLine("\n  OutputFilePrefix \"\"");
            sw.WriteLine("  OutputFileSuffix \"\"");

            sw.WriteLine("\n   MasterSlaveMode none");
            sw.WriteLine("     MasterSlaveId none");

            sw.WriteLine("\n        LightToPoolMethod Composite");
            sw.WriteLine("       LightProfileMethod Parallel");
            sw.WriteLine("  DoIntermittentMortality yes");
            sw.WriteLine("         DoShadingEffects yes");
            sw.WriteLine("            ShadingFactor 0.25");
            sw.WriteLine("     HeightProdEffectRate 4.6");
            sw.WriteLine("   UseColdProductionIndex yes");
            sw.WriteLine("AdjustHeightForProduction yes");

            sw.WriteLine("\n DoNutrientCycling no");
            sw.WriteLine("AdjustRespirationForAcclimatization yes");

            sw.WriteLine("\n             DoWindDisturbance no");
            sw.WriteLine("            InsectRegimeMethod none");
            sw.WriteLine("  AllowDuplicateInsectInstance no");
            sw.WriteLine("          UseHarvestManagement no");
            sw.WriteLine("             UseFireManagement no");
            sw.WriteLine("              UseHarvestBlocks no");
            sw.WriteLine("        HarvestBlockRasterName ");
            sw.WriteLine("                 SalvageMethod SnagsAndLogs");

            sw.WriteLine("\n             ZipRasters no");
            sw.WriteLine("       CopyInputRasters no");

            sw.WriteLine("\nDoHarvestSimulationReport no");
            sw.WriteLine("	HarvestMaxStartAttempts 100");
            sw.WriteLine("	HarvestMaxBuildAttempts 10");
            sw.WriteLine("  MinHarvestAreaReduction 10");

            sw.WriteLine("\nDoFireSimulationDiagnostics no");
            sw.WriteLine("            UseDroughtIndex yes");
            sw.WriteLine("           BaseDroughtIndex 14");
            sw.WriteLine("            MaxDroughtIndex 90");
            sw.WriteLine("           MaxDroughtFactor 2.0");

            sw.WriteLine("\n            ArchiveMode none # one of none, write, read");
            sw.WriteLine("    ArchiveCalendarYear 2008 # 2008 calendar year at which to write/read archive");
            sw.WriteLine("      ArchiveFolderName Archive");
            sw.WriteLine("        ArchiveFileName Archive.bin");
            sw.WriteLine("     ArchiveStopOnWrite yes");

            sw.WriteLine("\n         CanopyCoverTimeLag 10");

            sw.WriteLine("\nDoDecayProductionIndexDiagnostics yes");
            sw.WriteLine("DoColdProductionIndexDiagnostics yes");
            sw.WriteLine("      DoGrowthDiagnostics yes");

            sw.WriteLine("\n      GridCellSize 100");
            sw.WriteLine("       GridOffsetX 0");
            sw.WriteLine("       GridOffsetY 0");

            sw.WriteLine("\n     InitialStableSoil1(OM) 180");
            sw.WriteLine("     InitialStableSoil2(OM) 0");

            sw.WriteLine("\n          DoSummaryZoneStatistics yes");
            sw.WriteLine("DoAbioticDecayIndexGridStatistics no");
            sw.WriteLine("  DoProductionIndexGridStatistics no");

            sw.WriteLine("\n         UseMoistureIndex yes");
            sw.WriteLine("        BaseMoistureIndex 0.6");
            sw.WriteLine("         MaxMoistureIndex 0.9");
            sw.WriteLine("       MaxFuelLevelFactor 2.0");

            sw.WriteLine("\n  YearOfFireToHarvestSwitch 1905");
            sw.WriteLine("   LastYearOfAgeDisturbance 1971");
            sw.WriteLine("        AgeGridValidMinimum 0");
            sw.WriteLine("        AgeGridValidMaximum 3000");

            sw.WriteLine("# end of parameters I added");
            sw.WriteLine("\n#####################################################################\n#####################################################################");

            sw.WriteLine("\nProgram    LandCarb\nVersion    3.3.9.4");

            sw.WriteLine("\nUsePatchGroups yes");
            sw.WriteLine("CopyParameterFiles no");

            sw.WriteLine("\nUseStandardParameterFolder no");
            sw.WriteLine("StandardParameterFolder");

            sw.WriteLine("\nDoLogFile               no");
            sw.WriteLine("DoSpeciesTally          yes");
            sw.WriteLine("DoAgeClassDistribution  yes");
            sw.WriteLine("DoMortalityDiagnostics  no");
            sw.WriteLine("DoLightDiagnostics      no");
            sw.WriteLine("DoNeighborDiagnostics   no");
            sw.WriteLine("DoClimateDiagnostics    no         # set to 'yes' to report climate module statistics, 'no' otherwise");
            sw.WriteLine("DoMoistureDiagnostics   no");

            // write name of output folder
            sw.WriteLine("\n# Name of folder to which output files should be written (surround path in double-quotes if it has spaces)");
            sw.WriteLine("OutputFolderName \"" + userDirectory.Value + "\\OutputFiles\"");

            // write name of grid folder
            string appPath = Request.PhysicalApplicationPath; // requires System.Diagnostics
            sw.WriteLine("\n# Name of folder containing input data grids (surround path in double-quotes if it has spaces)");
            sw.WriteLine("GridFolderName \"" + appPath + landCarbDir + "\\base\\InputDataGrids_" + modelScale.Value + "\"");

            sw.WriteLine("\n# the level of log messages to display; set to 0 to display no log messages,");
            sw.WriteLine("# 1 or more to display log messages (the greater the value, the more messages that will be reported)");
            sw.WriteLine("LogLevel 3");

            sw.WriteLine("\n# units by which to report the results: 'carbon' or 'organicMatter'");
            sw.WriteLine("Units carbon");

            //sw.WriteLine("\n# Number of years for simulation");
            //int totalSimYears = Convert.ToInt16(Page.Session["numSimYears"].ToString()) + spinupYears;
            //sw.WriteLine("SimulationYears  " + totalSimYears.ToString() + " #### this variable now deprecated");

            // enter first and last calendar year from form
            sw.WriteLine("\n# First and last calendar year of the simulation");
            sw.WriteLine("FirstCalendarYear " + firstCalendarYear.ToString());
            sw.WriteLine("LastCalendarYear  " + lastCalendarYear.ToString());

            sw.WriteLine("\n# simulation parameters - If first two set to 'no' then the other 6 are ignored");
            sw.WriteLine("DoHarvestSimulation           no");
            sw.WriteLine("DoFireSimulation              no");
            sw.WriteLine("DoFireSimulationReport        no");
            sw.WriteLine("SimulationFirstYear         2100");
            sw.WriteLine("SimulationLastYear          2200");
            sw.WriteLine("AnnualHarvestAreaPercent       1");
            sw.WriteLine("MinHarvestBlockAreaHa          1");
            sw.WriteLine("MaxHarvestBlockAreaHa        100");

            sw.WriteLine("\n# set to 'yes' to use the default species, 'no' otherwise; if set to 'no', species will be selected");
            sw.WriteLine("# based on relative abundance and/or seed dispersal");
            sw.WriteLine("UseDefaultSpecies yes");
            sw.WriteLine("UpperTreeSpecies Psme	' if UseDefaultSpecies set to 'yes', use these species");
            sw.WriteLine("LowerTreeSpecies Tshe");

            sw.WriteLine("\n# General grid attributes\n# Note: The dimensions must correspond to the size of the data grids");
            sw.WriteLine("# Note: Cell size is in metres");

            sw.WriteLine("RasterInputFormat   AAIGrid    # one of 'AAIGrid' (ArcAscii), 'Imagine', 'GeoTiff'");
            sw.WriteLine("RasterOutputFormat  AAIGrid     # one of 'AAIGrid' (ArcAscii), 'Imagine', 'GeoTiff'");
            sw.WriteLine("RasterType SingleBand   # one of 'SingleBand' or 'MultipleBands'");
            sw.WriteLine("GridNumRows     " + Page.Session["simRows"].ToString());
            sw.WriteLine("GridNumColumns  " + Page.Session["simCols"].ToString());

            sw.WriteLine("GridCellAreaHa       " + Page.Session["cellAreaHa"].ToString());
            sw.WriteLine("GridSampleInterval   1");
            sw.WriteLine("GridXllCorner       -2102674.9791588");
            sw.WriteLine("GridYllCorner        2656688.7234232");
            sw.WriteLine("GridBackgroundValue -9999");
            sw.WriteLine("GridNoDataValue     -9999");

            sw.WriteLine("\n# grid output");
            sw.WriteLine("DoGridOutput          no # set to 'yes' to have the grids output, 'no' otherwise");
            sw.WriteLine("DoGridStatistics     yes # set to 'yes' to generate grid summary statistics (works even if DoGridOutput set to 'no')");
            sw.WriteLine("GridOutputFirstYear " + firstCalendarYear.ToString() + " # the first CALENDAR year of the simulation that grid output should be produced");
            sw.WriteLine("GridOutputInterval     1 # the interval (frequency) with which output grids should be produced after the first year");
            sw.WriteLine("GridStatisticsFirstYear  " + firstCalendarYear.ToString() + " # the first CALENDAR year of the simulation that grid statistics should be produced");
            sw.WriteLine("GridStatisticsInterval   1 # the interval (frequency) with which grids statistics should be produced after the first year");

            sw.WriteLine("\n# set to 'yes' to calculate NPP, Rh (heterotrophic respiration) and NEP (net ecosystem production)");
            sw.WriteLine("DoNetEcosystemProduction yes");
            sw.WriteLine("DoNepDiagnostics         no");
            sw.WriteLine("DoNppDiagnostics         no");

            sw.WriteLine("\n# set to 'yes' to calculate forest product pool and disposal site attributes, 'no' otherwise");
            sw.WriteLine("DoForestProducts                             yes");

            if (Page.Session["subBuilding"].ToString() == "No")
                sw.WriteLine("LinkProductSubstitutionToLongTermStructures  no");
            else
                sw.WriteLine("LinkProductSubstitutionToLongTermStructures  yes");

            sw.WriteLine("\n# set to 'instance' to use the fire instance method (i.e., using FireRegime.prm); 'class' to use the");
            sw.WriteLine("# class method (i.e., using fire class grids and FireClasses.prm); 'none' for no fires");
            sw.WriteLine("FireRegimeMethod            instance");
            sw.WriteLine("AllowDuplicateFireInstance  yes");

            sw.WriteLine("\n# set to 'instance' to use the harvest instance method (i.e., using HarvestInt.dvr); 'class' to use the");
            sw.WriteLine("# class method (i.e., using fire class grids and HarvestClasses.prm); 'none' for no harvests");
            sw.WriteLine("HarvestRegimeMethod             instance");
            sw.WriteLine("AllowDuplicateHarvestInstance   yes");

            // set the default elevation for the simulation
            sw.WriteLine("\nUseElevationGrid no ' set to 'yes' to use DEM, 'no' otherwise");
            if (Page.Session["region"].ToString() == "WestCascades")
            {
                if (Page.Session["elevClass"].ToString() == "low")
                    sw.WriteLine("DefaultElevation   754 ' elevation for " + Page.Session["region"].ToString() + " at " + Page.Session["elevClass"].ToString() + " elevation");
                else if (Page.Session["elevClass"].ToString() == "mid")
                    sw.WriteLine("DefaultElevation   1516 ' elevation for " + Page.Session["region"].ToString() + " at " + Page.Session["elevClass"].ToString() + " elevation");
                else
                    sw.WriteLine("DefaultElevation   2455 ' elevation for " + Page.Session["region"].ToString() + " at " + Page.Session["elevClass"].ToString() + " elevation");
            }
            else if (Page.Session["region"].ToString() == "EastCascades")
            {
                if (Page.Session["elevClass"].ToString() == "low")
                    sw.WriteLine("DefaultElevation   630 ' elevation for " + Page.Session["region"].ToString() + " at " + Page.Session["elevClass"].ToString() + " elevation");
                else if (Page.Session["elevClass"].ToString() == "mid")
                    sw.WriteLine("DefaultElevation   1421 ' elevation for " + Page.Session["region"].ToString() + " at " + Page.Session["elevClass"].ToString() + " elevation");
                else
                    sw.WriteLine("DefaultElevation   1898 ' elevation for " + Page.Session["region"].ToString() + " at " + Page.Session["elevClass"].ToString() + " elevation");
            }
            else
                sw.WriteLine("DefaultElevation   625 ' elevation for " + Page.Session["region"].ToString() + " at " + Page.Session["elevClass"].ToString() + " elevation");

            sw.WriteLine("\n# set to 'yes' to build and use an initial (historical) disturbance grid that will");
            sw.WriteLine("# yield stand ages corresponding to the age grid\n# (the grids 'disturbance.asc' and 'age.asc' must exist)");
            sw.WriteLine("UseDisturbanceGrid no");

            sw.WriteLine("\n# set to 'yes' to clear any existing output files from the output folder (OutputFolderName)");
            sw.WriteLine("# before writing new files; this ensures no ambiguity if different sets of files are ");
            sw.WriteLine("# output during a run; otherwise, existing files are left intact, and only overwritten when ");
            sw.WriteLine("# same-named output files are generated by the simulation\n# NOTE: PRESENTLY THIS DOES NOT WORK");
            sw.WriteLine("ClearExistingOutputFiles yes");

            sw.WriteLine("\n# set to 'yes' to output text reports; otherwise set to 'no'");
            sw.WriteLine("DoTextOutput    no");
            sw.WriteLine("DoCohortReports no");

            sw.WriteLine("\n# set to 'yes' to output CSV files as part of reporting; otherwise set to 'no'");
            sw.WriteLine("DoCsvFiles no");

            sw.WriteLine("\n# set to 'yes' to use an existing age grid to build a disturbance grid that will yield cell");
            sw.WriteLine("\n# ages corresponding to the age grid; an age grid must be available");
            sw.WriteLine("UseAgeBasedDisturbance no");
            sw.WriteLine("AgeGridReferenceYear   2000 		# this is the calendar year for which the ages in the age grid were determined");

            sw.WriteLine("\n# Seed dispersal parameters; dispersal distance is in metres");
            sw.WriteLine("DoSeedDispersal               no  # set to 'yes' to do seed dispersal; otherwise set to 'no'");
            sw.WriteLine("DoSeedDispersalDiagnostics    no  # set to 'yes' to do seed dispersal diagnostics; otherwise 'no'");
            sw.WriteLine("SeedDispersalCellDistance      2  # number of cells to go out from the target cell");

            sw.WriteLine("\n# set to 'yes' to do fire severity feedback; otherwise set to 'no'");
            sw.WriteLine("DoFireSeverityFeedback    yes");
            sw.WriteLine("TargetFuelLevelAdjustment 0.50 # the final target fuel level adjustment (age-based reduction) (ranges from 0.0 to 1.0)");
            sw.WriteLine("RateOfFuelLevelAdjustment 0.08 # rate at which fuel level adjustment is reduced, from 1.0 to the targetFuelLevelAdjustment");

            sw.WriteLine("\n# if set to 'yes', light-in is adjusted for shading effects (among trees and cohort patches); otherwise set 'no'");
            sw.WriteLine("DoShadingEffects yes");

            sw.WriteLine("\n# regeneration type method determines how upper tree colonization rates are selected from Estab_LC.prm.");
            sw.WriteLine("# Options are: 'alwaysArtificial' (always use artifical rate), 'alwaysNatural' (always use natural rate) and ");
            sw.WriteLine("# 'variable' (use either artificial or natural, whichever has the greatest colonization rate)");
            sw.WriteLine("RegenerationTypeMethod alwaysNatural");

            sw.WriteLine("\n# Minimum area (proportion) in which a cohort can be established (range: 0.0 to 1.0)");
            sw.WriteLine("CohortMinimumArea 0.10");

            sw.WriteLine("\n# Maximum number of years that a cohort can span; this effectively determines the number\n# of age classes a cohort can have.");
            sw.WriteLine("CohortMaximumYears 10");

            sw.WriteLine("\n# Minimum allowable adjusted colonization rate");
            sw.WriteLine("MinimumColonizationRate .03");
            sw.WriteLine("RandomizeColonizationRates  no");

            sw.WriteLine("\n# Random number seed");
            if (Page.Session["randomSeed"].ToString() == "No")
                sw.WriteLine("RandomNumberSeed 97643343");
            else
            {
                sw.WriteLine("RandomNumberSeed 0");
 //               int currentSeed = unchecked((int)DateTime.Now.Ticks);
 //               currentSeed = Math.Abs(currentSeed);
 //               sw.WriteLine("RandomNumberSeed " + currentSeed.ToString());
            }

            sw.WriteLine("\n# Specify method to calculate live growth");
            sw.WriteLine("GrowthMethod        PreRunClimate");
            sw.WriteLine("SiteIndexSpecies    Psme");
            sw.WriteLine("SiteIndex           Site3Medium");

            sw.WriteLine("\n# Specify how annual abiotic decay indices are to be determined\n# Valid options are Climate and PreRunClimate");
            sw.WriteLine("# If PreRunClimate is selected, the parameter file AnnualAbioticDecayIndex.prm must be provided");
            sw.WriteLine("AbioticDecayMethod  PreRunClimate");

            sw.WriteLine("\nPET_Reduction       10");

            sw.WriteLine("\n# use to shorten spin-up time for stable soil pool");
            sw.WriteLine("InitialStableSoil(OM)  180");

            sw.WriteLine("\n# sum of initial surface Charcoal");
            sw.WriteLine("InitialSurfaceCharcoal(OM) 3");
            sw.WriteLine("InitialBuriedCharcoal(OM) 5");

            sw.WriteLine("\n# determine the degree GGP decreases with tree height");
            sw.WriteLine("GPP_DecreaseMax      0");
            sw.WriteLine("GPP_Shape            1");

            sw.WriteLine("\nCrownWidth          17");

        }
    }

    protected void writeHarvestRegime(SortedList<string, ArrayList> managementDict, FileSet regimeFileSet)
    {
        int randSeed = 97643343;
        crb.HarvestRegimeBuilder hrb = null;
        crb.SecondaryRegime.Placement secPlacement = SecondaryRegime.Placement.ROTATED;

        if (Page.Session["randomSeed"].ToString() == "No")
            hrb = new HarvestRegimeBuilder(regimeFileSet, randSeed);
        else
            hrb = new HarvestRegimeBuilder(regimeFileSet);

        TextBox1.Text += "\n\nWriting Harvest Regime... version: " + hrb.version;

        hrb.region = setRegion();
        hrb.ownership = setOwnership();
        hrb.elevation = setElevation();

        hrb.numRows = Convert.ToInt16(Page.Session["simRows"].ToString());
        hrb.numColumns = Convert.ToInt16(Page.Session["simCols"].ToString());

        hrb.cellAreaHa = Convert.ToDouble(Page.Session["cellAreaHa"].ToString());
        hrb.rotationIntervalFactor = 1.0;

        hrb.firstYearOfSimulation = Convert.ToInt16(Page.Session["simStartYear"].ToString());

        foreach (KeyValuePair<string, ArrayList> dictEntry in managementDict)
        {
            if (dictEntry.Value[0].ToString().StartsWith("CCut") | dictEntry.Value[0].ToString().StartsWith("Salv"))
            {
                if (dictEntry.Key.Substring(5, 6) == "xx0000" & dictEntry.Key.Length == 11)
                {
                    // write out stand level events
                    hrb.harvestType = dictEntry.Value[0].ToString();
                    hrb.eventYear = Convert.ToInt16(dictEntry.Key.Substring(1, 4));
                    hrb.percentDisturbed = Convert.ToInt16(dictEntry.Value[2].ToString());
                    hrb.utilization = dictEntry.Value[3].ToString();

                    //if (dictEntry.Key.Length > 12)
                    //{
                    //    hrb.eventYear += Convert.ToInt16(dictEntry.Key.Substring(12));
                    //}

                    // scan for and add any secondary regimes
                    hrb.clearSecondaryRegimes();
                    foreach (KeyValuePair<string, ArrayList> secDictEntry in managementDict)
                    {
                        if (secDictEntry.Key.Length > 11)
                        {
                            if (Convert.ToInt16(secDictEntry.Key.Substring(1, 4)) == hrb.eventYear)
                            {
                                if (secDictEntry.Value[4].ToString() == "FIXED")
                                    secPlacement = SecondaryRegime.Placement.FIXED;
                                else
                                    secPlacement = SecondaryRegime.Placement.ROTATED;

                                if (secDictEntry.Value[0].ToString() == "Prescribed" | secDictEntry.Value[0].ToString() == "BurnPiles")
                                    hrb.addSecondaryPrescribedFire(Convert.ToInt16(secDictEntry.Value[1]), Convert.ToInt16(secDictEntry.Value[2]), secDictEntry.Value[0].ToString(), secDictEntry.Value[3].ToString(), secPlacement);
                                else
                                    hrb.addSecondaryHarvest(secDictEntry.Value[0].ToString(), Convert.ToInt16(secDictEntry.Value[1]), Convert.ToInt16(secDictEntry.Value[2]), secDictEntry.Value[3].ToString(), secPlacement);
                            }
                        }
                    }
                    
                    try
                    {
                        hrb.writeStandEvent(); // do a single cut
                        TextBox1.Text += "\n" + string.Format("Realized {0} percent disturbed: {1:#.##}% ({2:0.00}%)", hrb.eventYear, hrb.realizedPercentDisturbed, hrb.percentDisturbed);
                    }
                    catch (Exception ex)
                    {
                        TextBox1.Text += "\n*** HarvestRegimeBuilder: Stand Exception - " + ex.Message;
                    }
                }
                else
                {
                    if (dictEntry.Key.Length == 11)
                    {
                        // write out primary landscape regime
                        hrb.firstYear = Convert.ToInt16(dictEntry.Key.Substring(1, 4));
                        hrb.lastYear = Convert.ToInt16(dictEntry.Key.Substring(7, 4));
                        hrb.rotationInterval = Convert.ToInt16(dictEntry.Value[1]);
                        hrb.minHarvestAge = Convert.ToInt16(Convert.ToDouble(dictEntry.Value[1]) * 0.9);
                        hrb.percentDisturbed = Convert.ToInt16(dictEntry.Value[2]);
                        hrb.utilization = dictEntry.Value[3].ToString();
                        hrb.harvestType = dictEntry.Value[0].ToString();

                        // scan for and add any secondary regimes
                        hrb.clearSecondaryRegimes();
                        foreach (KeyValuePair<string, ArrayList> secDictEntry in managementDict)
                        {
                            if (secDictEntry.Key.Length > 11)
                            {
                                if (Convert.ToInt16(secDictEntry.Key.Substring(1, 4)) == hrb.firstYear & Convert.ToInt16(secDictEntry.Key.Substring(7, 4)) == hrb.lastYear)
                                {
                                    if (secDictEntry.Value[4].ToString() == "FIXED")
                                        secPlacement = SecondaryRegime.Placement.FIXED;
                                    else
                                        secPlacement = SecondaryRegime.Placement.ROTATED;

                                    if (secDictEntry.Value[0].ToString() == "Prescribed" | secDictEntry.Value[0].ToString() == "BurnPiles")
                                        hrb.addSecondaryPrescribedFire(Convert.ToInt16(secDictEntry.Value[1]), Convert.ToInt16(secDictEntry.Value[2]), secDictEntry.Value[0].ToString(), secDictEntry.Value[3].ToString(), secPlacement);
                                    else
                                        hrb.addSecondaryHarvest(secDictEntry.Value[0].ToString(), Convert.ToInt16(secDictEntry.Value[1]), Convert.ToInt16(secDictEntry.Value[2]), secDictEntry.Value[3].ToString(), secPlacement);
                                }
                            }
                        }

                        try
                        {
                            hrb.writeLandscapeRegime();  // write harvest regime to file
                            TextBox1.Text += "\n\nRegime: " + hrb.firstYear + " to " + hrb.lastYear;
                            TextBox1.Text += "\n" + string.Format("Realized percent disturbed: {0 : #.##}% ({1:0.00}%)", hrb.realizedPercentDisturbed, hrb.percentDisturbed);
                            TextBox1.Text += "\n" + string.Format("Realized rotation interval: {0 : #.##} ({1:0.00})", hrb.realizedRotationInterval, hrb.adjustedRotationInterval);
                            TextBox1.Text += "\nMinimum harvest age: " + hrb.minHarvestAge.ToString();
                            TextBox1.Text += "\n" + string.Format("Event area: {0:#.#} ha", hrb.eventAreaHa);
                        }
                        catch (Exception ex)
                        {
                            TextBox1.Text += "\n*** HarvestRegimeBuilder: Landscape Exception - " + ex.Message;
                        }
                    }
                }
            }
        }
    } // end write harvest regime

    protected void writePrescribedFireRegime(SortedList<string, ArrayList> managementDict, FileSet regimeFileSet)
    {
        int randSeed = 97643343;
        crb.PrescribedFireRegimeBuilder pfrb = null;
        crb.SecondaryRegime.Placement secPlacement = SecondaryRegime.Placement.ROTATED;

        if (Page.Session["randomSeed"].ToString() == "No")
            pfrb = new PrescribedFireRegimeBuilder(regimeFileSet, randSeed);
        else
            pfrb = new PrescribedFireRegimeBuilder(regimeFileSet);

        TextBox1.Text += "\n\nWriting Prescribed Fire Regime... version: " + pfrb.version;

        pfrb.region = setRegion();
        pfrb.ownership = setOwnership();
        pfrb.elevation = setElevation();

        pfrb.numRows = Convert.ToInt16(Page.Session["simRows"].ToString());
        pfrb.numColumns = Convert.ToInt16(Page.Session["simCols"].ToString());

        pfrb.cellAreaHa = Convert.ToDouble(Page.Session["cellAreaHa"].ToString());
        pfrb.firstYearOfSimulation = Convert.ToInt16(Page.Session["simStartYear"].ToString());
        pfrb.firstYear = 0;
        pfrb.lastYear = 0;

        foreach (KeyValuePair<string, ArrayList> dictEntry in managementDict)
        {
            if (dictEntry.Value[0].ToString() == "Prescribed" | dictEntry.Value[0].ToString() == "BurnPiles")
            {
                if (dictEntry.Key.Substring(5, 6) == "xx0000" & dictEntry.Key.Length == 11)
                {
                    // write out stand level events
                    pfrb.eventYear = Convert.ToInt16(dictEntry.Key.Substring(1, 4));
                    pfrb.percentDisturbed = Convert.ToInt16(dictEntry.Value[2].ToString());
                    pfrb.fireSeverity = dictEntry.Value[3].ToString();
                    pfrb.fireType = dictEntry.Value[0].ToString();

                    //if (dictEntry.Key.Length > 12)
                    //{
                    //    pfrb.eventYear += Convert.ToInt16(dictEntry.Key.Substring(12));
                    //}

                    // scan for and add any secondary regimes
                    pfrb.clearSecondaryRegimes();
                    foreach (KeyValuePair<string, ArrayList> secDictEntry in managementDict)
                    {
                        if (secDictEntry.Key.Length > 11)
                        {
                            if (Convert.ToInt16(secDictEntry.Key.Substring(1, 4)) == pfrb.eventYear)
                            {
                                if (secDictEntry.Value[4].ToString() == "FIXED")
                                    secPlacement = SecondaryRegime.Placement.FIXED;
                                else
                                    secPlacement = SecondaryRegime.Placement.ROTATED;

                                if (secDictEntry.Value[0].ToString() == "Prescribed" | secDictEntry.Value[0].ToString() == "BurnPiles")
                                    TextBox1.Text += "\n*********** Error: Prescribed secondary regime with Prescribed primary regime";
                                else
                                    pfrb.addSecondaryHarvest(secDictEntry.Value[0].ToString(), Convert.ToInt16(secDictEntry.Value[1]), Convert.ToInt16(secDictEntry.Value[2]), secDictEntry.Value[3].ToString(), secPlacement);
                            }
                        }
                    }
                    
                    try
                    {
                        pfrb.writeStandEvent(); // do a single prescribed fire
                        TextBox1.Text += "\n" + string.Format("Realized {0} percent disturbed: {1:#.##}% ({2:0.00}%)", pfrb.eventYear, pfrb.realizedPercentDisturbed, pfrb.percentDisturbed);
                    }
                    catch (Exception ex)
                    {
                        TextBox1.Text += "\n*** PrescribedFireRegimeBuilder: Stand Exception - " + ex.Message;
                    }


                }
                else
                {
                    if (dictEntry.Key.Length == 11)
                    {
                        // Write out primary landscape regime
                        pfrb.firstYear = Convert.ToInt16(dictEntry.Key.Substring(1, 4));
                        pfrb.lastYear = Convert.ToInt16(dictEntry.Key.Substring(7, 4));
                        pfrb.rotationInterval = Convert.ToInt16(dictEntry.Value[1]);
                        pfrb.percentDisturbed = Convert.ToInt16(dictEntry.Value[2]);
                        pfrb.fireSeverity = dictEntry.Value[3].ToString();
                        pfrb.fireType = dictEntry.Value[0].ToString();

                        // scan for and add any secondary regimes
                        pfrb.clearSecondaryRegimes();
                        foreach (KeyValuePair<string, ArrayList> secDictEntry in managementDict)
                        {
                            if (secDictEntry.Key.Length > 11)
                            {
                                if (Convert.ToInt16(secDictEntry.Key.Substring(1, 4)) == pfrb.firstYear & Convert.ToInt16(secDictEntry.Key.Substring(7, 4)) == pfrb.lastYear)
                                {
                                    if (secDictEntry.Value[4].ToString() == "FIXED")
                                        secPlacement = SecondaryRegime.Placement.FIXED;
                                    else
                                        secPlacement = SecondaryRegime.Placement.ROTATED;

                                    if (secDictEntry.Value[0].ToString() == "Prescribed" | secDictEntry.Value[0].ToString() == "BurnPiles")
                                        TextBox1.Text += "\n*********** Error: Prescribed secondary regime with Prescribed primary regime";
                                    //pfrb.addSecondaryPrescribedFire(Convert.ToInt16(secDictEntry.Value[1]), Convert.ToInt16(secDictEntry.Value[2]), secDictEntry.Value[3].ToString());
                                    else
                                        pfrb.addSecondaryHarvest(secDictEntry.Value[0].ToString(), Convert.ToInt16(secDictEntry.Value[1]), Convert.ToInt16(secDictEntry.Value[2]), secDictEntry.Value[3].ToString(), secPlacement);
                                }
                            }
                        }

                        try
                        {
                            pfrb.writeLandscapeRegime();  // write prescribed fire regime to file
                            TextBox1.Text += "\n\nRegime: " + pfrb.firstYear + " to " + pfrb.lastYear;
                            TextBox1.Text += "\n" + string.Format("Realized percent disturbed: {0 : #.##}% ({1:0.00}%)", pfrb.realizedPercentDisturbed, pfrb.percentDisturbed);
                            TextBox1.Text += "\n" + string.Format("Realized rotation interval: {0 : #.##} ({1:0.00})", pfrb.realizedRotationInterval, pfrb.adjustedRotationInterval);
                            TextBox1.Text += "\n" + string.Format("Area event: {0:#.#} ha", pfrb.eventAreaHa);
                        }
                        catch (Exception ex)
                        {
                            TextBox1.Text += "\n*** PrescribedFireRegimeBuilder: Landscape Exception - " + ex.Message;
                        }

                    }
                }
            }
        }

    } // end write prescribed fire regime

    protected void writeWildFireRegime(SortedList<string, ArrayList> managementDict, SortedList<string, ArrayList> natDisturbDict, FileSet regimeFileSet)
    {
        int firstFutureYear = Convert.ToInt16(Page.Session["currentYear"].ToString());
        int lastFutureYear = Convert.ToInt16(Page.Session["currentYear"].ToString()) + Convert.ToInt16(Page.Session["numSimYears"].ToString());
        //List<SortedList<string, ArrayList>> dictList = new List<SortedList<string, ArrayList>>();
        // dictList.Add(historyDict);
        // dictList.Add(futureDict);
        int randSeed = 97643343;
        crb.WildfireRegimeBuilder wfrb = null;

        if (Page.Session["randomSeed"].ToString() == "No")
            wfrb = new WildfireRegimeBuilder(regimeFileSet, randSeed);
        else
            wfrb = new WildfireRegimeBuilder(regimeFileSet);

        TextBox1.Text += "\n\nWriting Wildfire Regime... version: " + wfrb.version;

        wfrb.region = setRegion();
        wfrb.ownership = setOwnership();
        wfrb.elevation = setElevation();

        wfrb.numRows = Convert.ToInt16(Page.Session["simRows"].ToString());
        wfrb.numColumns = Convert.ToInt16(Page.Session["simCols"].ToString());
        wfrb.friFrequencyFile = userDirectory.Value + "\\ParameterFiles\\fireReturnIntervalFrequency.csv";
        wfrb.severityFrequencyFile = userDirectory.Value + "\\ParameterFiles\\fireSeverityFrequency.csv";
        wfrb.cellAreaHa = Convert.ToDouble(Page.Session["cellAreaHa"].ToString());
        wfrb.maxFireAreaHa = wfrb.cellAreaHa * Convert.ToDouble(Page.Session["simCols"].ToString()) * Convert.ToDouble(Page.Session["simRows"].ToString());

        wfrb.firstYearOfSimulation = Convert.ToInt16(Page.Session["simStartYear"].ToString());

        if (modelScale.Value == "stand")
        {
            foreach (KeyValuePair<string, ArrayList> dictEntry in managementDict)
            {
                if (dictEntry.Value[0].ToString() == "WildfireEvent")
                {
                    wfrb.eventYear = Convert.ToInt16(dictEntry.Key.Substring(1, 4));
                    wfrb.percentDisturbed = Convert.ToInt16(dictEntry.Value[2].ToString());
                    wfrb.fireSeverity = dictEntry.Value[3].ToString();

                    try
                    {
                        wfrb.writeStandEvent(); // do a single cut
                        TextBox1.Text += "\n" + string.Format("Realized {0} percent disturbed: {1:#.##}% ({2:0.00}%)", wfrb.eventYear, wfrb.realizedPercentDisturbed, wfrb.percentDisturbed);
                        TextBox1.Text += "\n" + string.Format("  Area event: {0:#.#} ha", wfrb.eventAreaHa);
                    }
                    catch (Exception ex)
                    {
                        TextBox1.Text += "\n*** WildfireRegimeBuilder: Stand Exception - " + ex.Message;
                    }
                }
            }
        }

        // build natural disturbance regimes
        wfrb.percentDisturbed = 100;
        foreach (KeyValuePair<string, ArrayList> dictEntry in natDisturbDict)
        {
            if (dictEntry.Key.Length == 11)
            {
                wfrb.firstYear = Convert.ToInt16(dictEntry.Key.Substring(1, 4));
                wfrb.lastYear = Convert.ToInt16(dictEntry.Key.Substring(7, 4));
                wfrb.friFactor = 1.0;

                if (dictEntry.Value[0].ToString() == "WildfireDefault")
                {
                    wfrb.fireSeverity = wfrb.getFireSeverity();
                    wfrb.fireReturnInterval = wfrb.getFireReturnInterval();  // use the builder to get the return interval

                    if (dictEntry.Value[3].ToString() == "typical")
                        wfrb.friFactor = 2.5;
                }
                else
                {
                    // setup custom wildfire
                    wfrb.fireReturnInterval = Convert.ToInt16(dictEntry.Value[1]);

                    if (dictEntry.Value[3].ToString() == "Default")
                        wfrb.fireSeverity = wfrb.getFireSeverity();
                    else
                        wfrb.fireSeverity = dictEntry.Value[3].ToString();

                    if (dictEntry.Value[2].ToString() == "Half")
                        wfrb.fireSizeFactor = 0.5;
                    else if (dictEntry.Value[2].ToString() == "Double")
                        wfrb.fireSizeFactor = 2.0;
                    else
                        wfrb.fireSizeFactor = 1.0;
                }
                try
                {
                    if (dictEntry.Value[3].ToString() == "total")
                    {
                        TextBox1.Text += "\n\nRegime: " + wfrb.firstYear + " to " + wfrb.lastYear + " - No wildfires scheduled";
                    }
                    else
                    {
                        // scan for and add any secondary regimes
                        wfrb.clearSecondaryRegimes();
                        foreach (KeyValuePair<string, ArrayList> secDictEntry in natDisturbDict)
                        {
                            if (secDictEntry.Key.Length > 11)
                            {
                                if (Convert.ToInt16(secDictEntry.Key.Substring(1, 4)) == wfrb.firstYear & Convert.ToInt16(secDictEntry.Key.Substring(7, 4)) == wfrb.lastYear)
                                {
                                        wfrb.addSecondaryHarvest(Convert.ToInt16(secDictEntry.Value[1]), secDictEntry.Value[3].ToString());
                                }
                            }
                        }
                        
                        wfrb.writeLandscapeRegime(); // write out fire regime to file
                        TextBox1.Text += "\n\nRegime: " + wfrb.firstYear + " to " + wfrb.lastYear;
                        TextBox1.Text += "\n" + string.Format("Realized percent disturbed: {0 : #.##}% ({1:0.00}%)", wfrb.realizedPercentDisturbed, wfrb.percentDisturbed);
                        TextBox1.Text += "\n" + string.Format("Realized fire return interval: {0 : #.##} ({1:0.00})", wfrb.realizedFireReturnInterval, wfrb.adjustedFireReturnInterval);
                        TextBox1.Text += "\n" + string.Format("Max fire area: {0:#.#} ha", wfrb.maxFireAreaHa);
                        TextBox1.Text += "\n" + string.Format("Area event: {0:#.#} ha", wfrb.eventAreaHa);
                    }
                }
                catch (Exception ex)
                {
                    TextBox1.Text += "\n*** WildFireRegimeBuilder: Primary Regime Exception - " + ex.Message;
                }
            }
            else
            {
                // secondary regime
                wfrb.addSecondaryHarvest(Convert.ToInt16(dictEntry.Value[1]), dictEntry.Value[3].ToString());

                try
                {
                    wfrb.writeLandscapeRegime(); // write out fire regime to file
                    TextBox1.Text += "\n\nSecondary Regime: " + dictEntry.Value[0].ToString();
                }
                catch (Exception ex)
                {
                    TextBox1.Text += "\n*** WildFireRegimeBuilder: Secondary Regime Exception - " + ex.Message;
                }

            }
        }
    } // end write wildfire regime

    protected RegionEnum setRegion()
    {
        // set region so fire return interval files can select correct constants.
        if (Page.Session["region"].ToString() == "EastCascades")
            return (RegionEnum.EastCascades);
        else if (Page.Session["region"].ToString() == "WestCascades")
            return (RegionEnum.WestCascades);
        else
            TextBox1.Text += "\n\n ******  Error: region " + Page.Session["region"].ToString() + " not identified\n\n";

        return (RegionEnum.nill);
    }
    protected OwnershipEnum setOwnership()
    {
        // set elevation so fire return interval files can select correct constants.
        if (Page.Session["ownership"].ToString() == "usfs")
            return (OwnershipEnum.Federal);
        else if (Page.Session["ownership"].ToString() == "blm")
            return (OwnershipEnum.Federal);
        else if (Page.Session["ownership"].ToString() == "federal")
            return (OwnershipEnum.Federal);
        else if (Page.Session["ownership"].ToString() == "state")
            return (OwnershipEnum.State);
        else if (Page.Session["ownership"].ToString() == "pi")
            return (OwnershipEnum.PrivateIndustrial);
        else if (Page.Session["ownership"].ToString() == "pni")
            return (OwnershipEnum.PrivateNonIndustrial);
        else
            TextBox1.Text += "\n\n ******  Error: ownership " + Page.Session["ownership"].ToString() + " not identified\n\n";

        return (OwnershipEnum.nill);

    }
    protected ElevationEnum setElevation()
    {
        // set elevation so fire return interval files can select correct constants.
        if (Page.Session["elevClass"].ToString() == "low")
            return (ElevationEnum.Low);
        else if (Page.Session["elevClass"].ToString() == "mid")
            return (ElevationEnum.Medium);
        else if (Page.Session["elevClass"].ToString() == "high")
            return (ElevationEnum.High);
        else
            TextBox1.Text += "\n\n ******  Error: elevation class " + Page.Session["elevClass"].ToString() + " not identified\n\n";

        return (ElevationEnum.nill);
    }

    protected void writeManufacturing()
    {
        string filename = userDirectory.Value + "\\ParameterFiles\\Manufacturing.prm";
        int currentYearInt = Convert.ToInt16(Page.Session["currentYear"].ToString());
        string mfg1900 = "       10         0          99        1          40        60         0     65          25          60\n";
        string mfg1950 = "       10         0          95        5          55        15        20     60          25          60\n";
        string mfg1970 = "       10         0          95        5          60        20        20     57          25          60\n";
        string mfg2000 = "       10         2          93        5          65        15        20     55          25          60\n";

        TextBox1.Text += "\n\nWriting Manufacturing.prm file...";

        // open new file handle for Manufacturing.prm
        using (StreamWriter sw = new StreamWriter(filename, true))
        {
            if (currentYearInt > 1900)
                sw.Write("    ML02  3     1900" + mfg1900);
            if (currentYearInt > 1950)
                sw.Write("    ML02  3     1950" + mfg1950);
            if (currentYearInt > 1970)
                sw.Write("    ML02  3     1970" + mfg1970);
            if (currentYearInt > 2000)
                sw.Write("    ML02  3     2000" + mfg2000);

            sw.Write("    ML02  3     " + Page.Session["currentYear"].ToString() + "       10");
            sw.Write("         " + Page.Session["mfgExBiofuel"].ToString());
            sw.Write("          " + Page.Session["mfgStrWood"].ToString());
            sw.Write("        " + Page.Session["mfgPulp"].ToString());
            sw.Write("          65        15        20     55          25          ");
            sw.Write(Page.Session["subBioenergyFactor"].ToString() + "\n");
        }
    }

    protected void writeProductsUse()
    {
        string filename = userDirectory.Value + "\\ParameterFiles\\ProductsUse.prm";
        int currentYearInt = Convert.ToInt16(Page.Session["currentYear"].ToString());
        string sessionProductLTStr = Page.Session["productLTStr"].ToString();
        string leakageRate;
        double defaultDisplacementValue = 1.1;
        double pctLossAtFinalValue = 50.0;
        double yearsToFinalValue = 50.0;
        double displacementValue;
        double baseLevel = defaultDisplacementValue * pctLossAtFinalValue / 100.0;

        string prod1900 = "        75        1.5         10     30     10            1.5         20        40       1.0           1.1\n";
        string prod1950 = "        75        1.25        10     30     10            1.25        15        30       1.5           1.1\n";
        string prod1970 = "        75        1.15        10     30     10            1.15        12        30       2.0           1.1\n";
        string prod2000 = "        75        1           10     30     10            1           10        30       3.0           1.1\n";

        if (Page.Session["subLeakage"].ToString() == "Yes")
            leakageRate = "       3.0";
        else
            leakageRate = "       0.0";

        TextBox1.Text += "\nWriting ProductsUse.prm file...";

        // open new file handle for ProductsUse.prm
        using (StreamWriter sw = new StreamWriter(filename, true))
        {
            if (currentYearInt > 1900)
                sw.Write("    ML02  3     1900" + prod1900);
            if (currentYearInt > 1950)
                sw.Write("    ML02  3     1950" + prod1950);
            if (currentYearInt > 1970)
                sw.Write("    ML02  3     1970" + prod1970);
            if (currentYearInt > 2000)
                sw.Write("    ML02  3     2000" + prod2000);

            sw.Write("    ML02  3     " + currentYearInt.ToString());
            sw.Write("        " + sessionProductLTStr);
            sw.Write("        1           10     30     10            1           10        30");
            sw.Write(leakageRate + "           " + defaultDisplacementValue.ToString() + "\n");

            if (Page.Session["subDisplacement"].ToString() == "No")
            {
                for (int year = 1; year <= 100; year++)
                {
                    displacementValue = baseLevel + (defaultDisplacementValue * (1 - pctLossAtFinalValue / 100.0) * Math.Exp(-1.0 * year * 3 / yearsToFinalValue));
                    sw.Write("    ML02  3     " + (currentYearInt + year).ToString());
                    sw.Write("        " + sessionProductLTStr);
                    sw.Write("        1           10     30     10            1           10        30");
                    sw.Write(leakageRate + "           " + string.Format("{0:0.#########}", displacementValue) + "\n");
                }
            }
        }
    }

    protected void writeDisposal()
    {
        string filename = userDirectory.Value + "\\ParameterFiles\\Disposal.prm";
        int currentYearInt = Convert.ToInt16(Page.Session["currentYear"].ToString());
        string disp1900 = "            1             0          0       100       0         0           0        60        0.5\n";
        string disp1950 = "            1             0          0        80      10        10           0        60        0.5\n";
        string disp1970 = "            1             1          5        25      64        10           1        60        0.5\n";
        string disp2000 = "           10            10         30        1       84        10           5        60        0.5\n";

        TextBox1.Text += "\nWriting Disposal.prm file...";

        // open new file handle for Disposal.prm
        using (StreamWriter sw = new StreamWriter(filename, true))
        {
            if (currentYearInt > 1900)
                sw.Write("    ML02  3     1900" + disp1900);
            if (currentYearInt > 1950)
                sw.Write("    ML02  3     1950" + disp1950);
            if (currentYearInt > 1970)
                sw.Write("    ML02  3     1970" + disp1970);
            if (currentYearInt > 2000)
                sw.Write("    ML02  3     2000" + disp2000);

            sw.Write("    ML02  3     " + Page.Session["currentYear"].ToString());
            sw.Write("           " + Page.Session["recycleLT"].ToString());
            sw.Write("            " + Page.Session["recycleST"].ToString());
            sw.Write("         " + Page.Session["recyclePaper"].ToString());
            sw.Write("        " + Page.Session["disposalDump"].ToString());
            sw.Write("       " + Page.Session["disposalLandfill"].ToString());
            sw.Write("        " + Page.Session["disposalIncinVol"].ToString());
            sw.Write("           " + Page.Session["disposalIncinBioenergy"].ToString());
            sw.Write("        60        0.5\n");
        }
    }


}