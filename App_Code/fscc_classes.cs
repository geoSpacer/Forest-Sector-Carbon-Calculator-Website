/* ------------------------------
 * Keith Olsen - 27 January 2012
 * Oregon State University
 * keith.olsen@oregonstate.edu
 * ------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using System.Web.UI.DataVisualization.Charting;
using System.Xml;
// using System.Collections.Generic;

/// <summary>
/// Summary description for fscc_classes
/// </summary>
public class renameValue_class
{
    public string regionName(string levelValue)
    {
        if (levelValue == "EastCascades")
            return ("Eastern Cascades");
        else if (levelValue == "WestCascades")
            return ("Western Cascades");
        else
            return (levelValue);
    }
    public string ownershipName(string levelValue)
    {
        if (levelValue == "usfs")
            return ("USFS");
        else if (levelValue == "blm")
            return ("BLM");
        else if (levelValue == "state")
            return ("State of Oregon");
        else if (levelValue == "pi")
            return ("Private Industrial");
        else if (levelValue == "pni")
            return ("Private Non-ind.");
        else
            return (levelValue);
    }
    public string regimeName(string regimeValue)
    {
        if (regimeValue == "CCut_SO")
            return ("Harvest Stem Only");
        else if (regimeValue == "CCut_AG")
            return ("Harvest Aboveground");
        else if (regimeValue == "CCut_WT")
            return ("Harvest Whole Tree");
        else if (regimeValue == "Prescribed")
            return ("Prescribed Broadcast Burn");
        else if (regimeValue == "BurnPiles")
            return ("Prescribed Burn Piles");
        else if (regimeValue == "Salv")
            return ("Salvage");
        else if (regimeValue == "WildfireEvent")
            return ("Wildfire");
        else
            return ("Error!!");
    }
    public string suppressionName(string suppressionValue)
    {
        if (suppressionValue == "typical")
            return ("Typical");
        else if (suppressionValue == "noSupp")
            return ("Set to zero");
        else if (suppressionValue == "total")
            return ("Total ** WARNING: No Wildfire Regime Selected **");
        else
            return ("Error!!");

    }
    public string levelName(string levelValue)
    {
        if (levelValue == "VeryLow")
            return ("Very low");
        else if (levelValue == "VeryHigh")
            return ("Very high");
        else if (levelValue == "VeryLight")
            return ("Very Light");
        else if (levelValue == "VeryHot")
            return ("Very Hot");
        else if (levelValue == "None")
            return ("Zero");
        else
            return (levelValue);
    }
}

public class displayDict_class
{
    public string displayManagementStand(KeyValuePair<string, ArrayList> dictEntry)
    {
        renameValue_class renameValue = new renameValue_class();
        string listItemString;

        if (dictEntry.Key.Length == 11)
        {
            listItemString = "Year: " + dictEntry.Key.Substring(1, 4) + " - ";
            listItemString += renameValue.regimeName(dictEntry.Value[0].ToString());
            if (dictEntry.Value[1].ToString() != "null")
                listItemString += " Every " + dictEntry.Value[1].ToString() + " yrs";

            listItemString += " - " + dictEntry.Value[2].ToString() + "% Disturbed - ";
        }
        else
        {
            listItemString = "-- " + renameValue.regimeName(dictEntry.Value[0].ToString()) + " - " + dictEntry.Value[4].ToString() + " - Offset ";
            listItemString += dictEntry.Value[1].ToString() + " yrs - ";
            listItemString += dictEntry.Value[2].ToString() + "% Disturbed - ";
        }

        listItemString += renameValue.levelName(dictEntry.Value[3].ToString());
        if (dictEntry.Value[0].ToString() == "Prescribed" | dictEntry.Value[0].ToString() == "BurnPiles" | dictEntry.Value[0].ToString() == "WildfireEvent")
            listItemString += " Severity";
        else
            listItemString += " Utilization";

        return (listItemString);
    }
    public string displayManagement(KeyValuePair<string, ArrayList> dictEntry)
    {
        renameValue_class renameValue = new renameValue_class();
        string listItemString;

        if (dictEntry.Key.Length == 11)
        {
            listItemString = dictEntry.Key.Substring(1, 4) + " to " + dictEntry.Key.Substring(7, 4) + " - ";
            listItemString += renameValue.regimeName(dictEntry.Value[0].ToString()) + " - Every ";
            listItemString += dictEntry.Value[1].ToString() + " yrs - ";
            listItemString += dictEntry.Value[2].ToString() + "% Disturbed - ";
        }
        else
        {
            listItemString = "-- " + renameValue.regimeName(dictEntry.Value[0].ToString()) + " - " + dictEntry.Value[4].ToString() + " - Offset ";
            listItemString += dictEntry.Value[1].ToString() + " yrs - ";
            listItemString += dictEntry.Value[2].ToString() + "% Disturbed - ";
        }

        listItemString += renameValue.levelName(dictEntry.Value[3].ToString());
        if (dictEntry.Value[0].ToString() == "Prescribed" | dictEntry.Value[0].ToString() == "BurnPiles")
            listItemString += " Severity";
        else
            listItemString += " Utilization";

        return (listItemString);
    }
    public string displayNatDisturb(KeyValuePair<string, ArrayList> dictEntry)
    {
        renameValue_class renameValue = new renameValue_class();
        string listItemString;

        if (dictEntry.Key.Length == 11)
        {
            listItemString = dictEntry.Key.Substring(1, 4) + " to " + dictEntry.Key.Substring(7, 4) + " - " + dictEntry.Value[0].ToString().Substring(0, 8);
            if (dictEntry.Value[0].ToString() == "WildfireDefault")
                listItemString += " - Suppression " + renameValue.suppressionName(dictEntry.Value[3].ToString());
            else
            {
                listItemString += " - Every ";
                listItemString += dictEntry.Value[1].ToString() + " yrs - ";
                listItemString += dictEntry.Value[2].ToString() + " Fire Size - ";
                listItemString += renameValue.levelName(dictEntry.Value[3].ToString()) + " Severity";
            }
        }
        else
        {
            listItemString = "-- " + renameValue.regimeName(dictEntry.Value[0].ToString()) + " - Offset ";
            listItemString += dictEntry.Value[1].ToString() + " years - ";
            listItemString += dictEntry.Value[2].ToString() + "% Disturbed - ";
            listItemString += renameValue.levelName(dictEntry.Value[3].ToString()) + " Utilization";
        }

        return (listItemString);
    }
    //public string noFireMsg()
    //{
    //    return ("****** Warning: No Wildfire Regime Defined for This Time Period ******");
    //}
    public void setNoFireDefault(ListItemCollection uiCheckBox, SortedList<string, ArrayList> regimeDict, string dictKey)
    {
        ArrayList dictData = new ArrayList();
        dictData.Add("WildfireDefault");
        dictData.Add("null");
        dictData.Add("null");
        dictData.Add("total");

        regimeDict.Add(dictKey, dictData);
        uiCheckBox.Add(new ListItem(displayNatDisturb(new KeyValuePair<string, ArrayList>(dictKey, dictData)), dictKey));
    }
}

public class setOptions_class
{
    public void setElevationItems(ListItemCollection uiListBox)
    {
        uiListBox.Add(new ListItem("Select elevation class", "selectElevation"));
        uiListBox.Add(new ListItem("Low", "low"));
        uiListBox.Add(new ListItem("Mid", "mid"));
        uiListBox.Add(new ListItem("High", "high"));
    }
    public void setOwnershipItems(ListItemCollection uiListBox)
    {
        uiListBox.Add(new ListItem("Select ownership", "selectOwnership"));
        uiListBox.Add(new ListItem("Federal", "federal"));
        uiListBox.Add(new ListItem("State of Oregon", "state"));
        uiListBox.Add(new ListItem("Private Industrial", "pi"));
        uiListBox.Add(new ListItem("Private Non-ind.", "pni"));
    }
    public void setRegionItems(ListItemCollection uiListBox)
    {
        uiListBox.Add(new ListItem("Select region", "selectRegion"));
        uiListBox.Add(new ListItem("Western Cascades", "WestCascades"));
        uiListBox.Add(new ListItem("Eastern Cascades", "EastCascades"));
    }
    public void setPriRegimeItems(ListItemCollection uiListBox)
    {
        uiListBox.Add(new ListItem("Select regime", "SelectRegime"));
        uiListBox.Add(new ListItem("Harvest stem only", "CCut_SO"));
        uiListBox.Add(new ListItem("Harvest aboveground", "CCut_AG"));
        uiListBox.Add(new ListItem("Harvest whole tree", "CCut_WT"));
        uiListBox.Add(new ListItem("Prescribed broadcast burn", "Prescribed"));
        uiListBox.Add(new ListItem("Prescribed burn pile", "BurnPiles"));
        uiListBox.Add(new ListItem("Salvage", "Salv"));
        uiListBox.Add(new ListItem("Wildfire default", "WildfireDefault"));
        uiListBox.Add(new ListItem("Wildfire custom", "WildfireCustom"));
    }
    public void setUtilizationItems(ListItemCollection uiListBox)
    {
        uiListBox.Add(new ListItem("Select Level"));
        uiListBox.Add(new ListItem("None"));
        uiListBox.Add(new ListItem("Very low", "VeryLow"));
        uiListBox.Add(new ListItem("Low"));
        uiListBox.Add(new ListItem("Medium"));
        uiListBox.Add(new ListItem("High"));
        uiListBox.Add(new ListItem("Very high", "VeryHigh"));
    }
    public void setSeverityItems(ListItemCollection uiListBox)
    {
        uiListBox.Add(new ListItem("Select Level"));
        uiListBox.Add(new ListItem("Default", "Default", false));
        uiListBox.Add(new ListItem("Very low", "VeryLow"));
        uiListBox.Add(new ListItem("Low"));
        uiListBox.Add(new ListItem("Medium"));
        uiListBox.Add(new ListItem("High"));
        uiListBox.Add(new ListItem("Very high", "VeryHigh"));
    }
    public void setFireSizeItems(ListItemCollection uiListBox)
    {
        uiListBox.Add(new ListItem("Select Level"));
        uiListBox.Add(new ListItem("Half"));
        uiListBox.Add(new ListItem("Default"));
        uiListBox.Add(new ListItem("Double"));
    }
    public void setSuppressionItems(ListItemCollection uiListBox)
    {
        uiListBox.Add(new ListItem("Select Level"));
        uiListBox.Add(new ListItem("Typical", "typical"));
        uiListBox.Add(new ListItem("No Suppression", "noSupp"));
        uiListBox.Add(new ListItem("Total (no wildfire)", "total"));
    }
    public void setSecRegimeItems(ListItemCollection uiListBox)
    {
        uiListBox.Add(new ListItem("Select regime", "SelectRegime"));
        uiListBox.Add(new ListItem("Harvest stem only", "CCut_SO"));
        uiListBox.Add(new ListItem("Harvest aboveground", "CCut_AG"));
        uiListBox.Add(new ListItem("Harvest whole tree", "CCut_WT"));
        uiListBox.Add(new ListItem("Prescribed broadcast burn", "Prescribed"));
        uiListBox.Add(new ListItem("Prescribed burn pile", "BurnPiles"));
        uiListBox.Add(new ListItem("Salvage", "Salv"));
    }
    public void setSecPlacementItems(ListItemCollection uiListBox)
    {
        uiListBox.Add(new ListItem("Rotated from Primary", "ROTATED"));
        uiListBox.Add(new ListItem("Fixed to Primary ", "FIXED"));
    }
}

public class manageDict_class
{
    public void removeItems(ListItemCollection uiCheckBox, SortedList<string, ArrayList> regimeDict)
    {
        ArrayList delKeyList = new ArrayList();

        foreach (ListItem selItem in uiCheckBox)
            if (selItem.Selected)
            {
                if (regimeDict.ContainsKey(selItem.Value))
                    regimeDict.Remove(selItem.Value);

                // if the selected regime is a primary regime, scan for its secondary regimes
                if (selItem.Value.Length == 11)
                    foreach (KeyValuePair<string, ArrayList> dictEntry in regimeDict)
                        if (dictEntry.Key.Substring(0, 5) == selItem.Value.Substring(0, 5))
                            delKeyList.Add(dictEntry.Key);
            }

        foreach (string delKey in delKeyList)
            regimeDict.Remove(delKey);
    }
    public void clearAllFuture(ListItemCollection uiCheckBox, SortedList<string, ArrayList> regimeDict, int currentYear)
    {
        ArrayList delKeyList = new ArrayList();

        foreach (KeyValuePair<string, ArrayList> dictEntry in regimeDict)
            if (Convert.ToInt16(dictEntry.Key.Substring(1, 4)) >= currentYear)
                delKeyList.Add(dictEntry.Key);

        foreach (string delKey in delKeyList)
            regimeDict.Remove(delKey);

        uiCheckBox.Clear();
    }
    public void clearAllHistory(ListItemCollection uiCheckBox, SortedList<string, ArrayList> regimeDict, int currentYear)
    {
        ArrayList delKeyList = new ArrayList();

        foreach (KeyValuePair<string, ArrayList> dictEntry in regimeDict)
            if (Convert.ToInt16(dictEntry.Key.Substring(7, 4)) < currentYear)
                delKeyList.Add(dictEntry.Key);

        foreach (string delKey in delKeyList)
            regimeDict.Remove(delKey);

        uiCheckBox.Clear();
    }
    public void insertRegime(SortedList<string, ArrayList> regimeDict, string dictKey, ArrayList dictData, int addItemStartYear, int addItemEndYear)
    {
        SortedList<string, ArrayList> oldDict = new SortedList<string, ArrayList>();
        int oldStartYear, oldEndYear;
        string splitRegimeAddKey, regimeAddKey;

        foreach (KeyValuePair<string, ArrayList> oldEntry in regimeDict)
            oldDict.Add(oldEntry.Key, oldEntry.Value);

        regimeDict.Clear();
        regimeDict.Add(dictKey, dictData);

        foreach (KeyValuePair<string, ArrayList> oldEntry in oldDict)
        {
            oldStartYear = Convert.ToInt16(oldEntry.Key.Substring(1, 4));
            oldEndYear = Convert.ToInt16(oldEntry.Key.Substring(7, 4));

            if (oldStartYear >= addItemStartYear & oldStartYear <= addItemEndYear)
                oldStartYear = addItemEndYear + 1;
            if (oldEndYear >= addItemStartYear & oldEndYear <= addItemEndYear)
                oldEndYear = addItemStartYear - 1;
            if (oldStartYear < addItemStartYear & oldEndYear > addItemEndYear)
            {
                splitRegimeAddKey = dictKey.Substring(0, 1) + (addItemEndYear + 1).ToString() + "to" + oldEntry.Key.Substring(7, 4);
                if (oldEntry.Key.Length > 11)
                    splitRegimeAddKey += oldEntry.Key.Substring(11, oldEntry.Key.Length - 11);

                regimeDict.Add(splitRegimeAddKey, oldEntry.Value);
                oldEndYear = addItemStartYear - 1;
            }

            if (oldEndYear > oldStartYear)
            {
                regimeAddKey = dictKey.Substring(0, 1) + oldStartYear.ToString() + "to" + oldEndYear.ToString();
                if (oldEntry.Key.Length > 11)
                    regimeAddKey += oldEntry.Key.Substring(11, oldEntry.Key.Length - 11);

                regimeDict.Add(regimeAddKey, oldEntry.Value);
            }
        }
    }
    public void loadDictFromXML(SortedList<string, ArrayList> regimeDict, XmlNodeList oNodeSelList)
    {
        string dictKey;
        string tempKey;
        int secRegimeNum;
        ArrayList dictData;

        regimeDict.Clear();
        foreach (XmlNode oNodeSel in oNodeSelList)
        {
            dictData = new ArrayList();
            if (oNodeSel.SelectSingleNode("EndYear").InnerText == "0000")
                dictKey = "p" + oNodeSel.SelectSingleNode("StartYear").InnerText + "xx" + oNodeSel.SelectSingleNode("EndYear").InnerText;
            else
                dictKey = "p" + oNodeSel.SelectSingleNode("StartYear").InnerText + "to" + oNodeSel.SelectSingleNode("EndYear").InnerText;

            dictData.Add(oNodeSel.SelectSingleNode("Type").InnerText);
            dictData.Add(oNodeSel.SelectSingleNode("Interval-Offset").InnerText);
            dictData.Add(oNodeSel.SelectSingleNode("Disturbed-Size").InnerText);
            dictData.Add(oNodeSel.SelectSingleNode("Util-Severity-Supp").InnerText);

            if (oNodeSel.SelectSingleNode("TreatmentClass").InnerText == "SecondaryRegime")
            {
                tempKey = dictKey + "s" + oNodeSel.SelectSingleNode("Interval-Offset").InnerText;
                secRegimeNum = Convert.ToInt16(oNodeSel.SelectSingleNode("Interval-Offset").InnerText);
                while (regimeDict.ContainsKey(tempKey))
                {
                    secRegimeNum += 1;
                    tempKey = dictKey + "s" + secRegimeNum.ToString();
                }
                dictKey = tempKey;

                if (oNodeSel.SelectSingleNode("Placement") != null)
                    dictData.Add(oNodeSel.SelectSingleNode("Placement").InnerText);
                else
                    dictData.Add("ROTATED");
            }

            regimeDict.Add(dictKey, dictData);
        }
    }
    public void modDictKey(SortedList<string, ArrayList> regimeDict, string newStartYear, string newEndYear)
    {
        ArrayList modKeyList = new ArrayList();
        int newEndYearInt = Convert.ToInt16(newEndYear);

        // regime start year
        foreach (KeyValuePair<string, ArrayList> dictEntry in regimeDict)
            if (dictEntry.Key.Substring(1, 4) == "1412")
                modKeyList.Add(dictEntry.Key);

        foreach (string modKey in modKeyList)
        {
            if (Convert.ToInt16(newStartYear) < Convert.ToInt16(modKey.Substring(7, 4)))
                regimeDict.Add(modKey.Replace("1412", newStartYear), regimeDict[modKey]);
            regimeDict.Remove(modKey);
        }

        // regime end year
        modKeyList.Clear();
        if (newEndYearInt > 2011)
        {
            // adjust end year into the future
            foreach (KeyValuePair<string, ArrayList> dictEntry in regimeDict)
                if (dictEntry.Key.Substring(7, 4) == "2011")
                    modKeyList.Add(dictEntry.Key);

            foreach (string modKey in modKeyList)
            {
                regimeDict.Add(modKey.Replace("2011", newEndYear), regimeDict[modKey]);
                regimeDict.Remove(modKey);
            }
        }
        else
        {
            // delete regime segments that are after new end year
            foreach (KeyValuePair<string, ArrayList> dictEntry in regimeDict)
                if (newEndYearInt <= Convert.ToInt16(dictEntry.Key.Substring(1, 4)))
                    modKeyList.Add(dictEntry.Key);

            foreach (string modKey in modKeyList)
                regimeDict.Remove(modKey);

            // adjust end year of latest regime segment
            modKeyList.Clear();
            foreach (KeyValuePair<string, ArrayList> dictEntry in regimeDict)
                if (newEndYearInt > Convert.ToInt16(dictEntry.Key.Substring(1, 4)) & newEndYearInt < Convert.ToInt16(dictEntry.Key.Substring(7, 4)))
                    modKeyList.Add(dictEntry.Key);

            foreach (string modKey in modKeyList)
            {
                regimeDict.Add(modKey.Replace(modKey.Substring(7, 4), newEndYear), regimeDict[modKey]);
                regimeDict.Remove(modKey);
            }
        }
    }
}

// ******** Classes for the output graphs

public class readOutput_class
{
    public ArrayList readCSV(string fileNameText, ref string[] fieldNames)
    {
        char[] delimiterChars = { ' ', ',', '\t' };
        Regex rex = new Regex(@"[ ]");
        string line;
        ArrayList dataArray = new ArrayList();

        try
        {
            // Read the file and display it line by line.
            using (StreamReader sr = new StreamReader(fileNameText))
            {
                line = sr.ReadLine();
                // replace all occurances of [ ] with ""
                line = rex.Replace(line, "");
                fieldNames = line.Split(delimiterChars);

                while ((line = sr.ReadLine()) != null)
                {
                    // replace all occurances of [ ] with ""
                    line = rex.Replace(line, "");
                    dataArray.Add(line.Split(delimiterChars));
                }
            }

            return dataArray;
        }
        catch
        {
            dataArray.Clear();
            return dataArray;
        }

    }
    public void loadChartSeries(string fileName, Series outputChartSeries, ArrayList dataArray, string[] fieldNames, string outputVariable, int startYear)
    {
        double yearAsNum;
        int fieldNum = 0;
        double dataFactor = 1.0;

        if (fileName.EndsWith("ForestRh.csv"))
            dataFactor = -1.0;

        for (int i = 0; i < fieldNames.Length; i++)
        {
            if (fieldNames[i] == outputVariable)
                fieldNum = i;
        }

        if (fieldNum == 0)
            outputChartSeries.LegendText = "Error reading " + outputVariable;
        else
            foreach (string[] dataLine in dataArray)
            {
                yearAsNum = Convert.ToInt16(dataLine[0]);
                if (yearAsNum > startYear)
                    outputChartSeries.Points.AddXY(yearAsNum, Convert.ToDouble(dataLine[fieldNum]) * dataFactor);
            }

    }
    public bool readCSV2Series(string fileName, Series outputChartSeries, string outputVariable, int startYear)
    {
        ArrayList dataArray;
        string[] fieldNames = { "" };

        // read from landcarb output file
        dataArray = readCSV(fileName, ref fieldNames);

        // return error state
        if (dataArray.Count > 0)
        {
            loadChartSeries(fileName, outputChartSeries, dataArray, fieldNames, outputVariable, startYear);
            dataArray.Clear();
            return (false);
        }
        else
            return (true);
    }
}

public class graphTitle_class
{
    public string lookupGraphTitle(string fileName)
    {
        if (fileName.EndsWith("Mass.csv"))
        {
            if (fileName.StartsWith("TotalForestSector"))
                return ("Total Stores for Forest Sector (Ecosystem + Products + Substitution)");
            else if (fileName.StartsWith("TotalForestEcosystem"))
                return ("Total Stores for Forest Ecosystem (Live + Dead + Stable Soils)");
            else if (fileName.StartsWith("TotalForestProduct"))
                return ("Total Stores Related to Forest Products (Use + Disposal)");
            else if (fileName.StartsWith("TotalSubstitution"))
                return ("Total Stores Related to Forest Substitution (Products + Biofuels)");
            else if (fileName.StartsWith("Biofuel"))
                return ("Cummulative Biofuel Offset by Manufacturing and Disposal");
            else if (fileName.StartsWith("Live"))
                return ("Total Stores for All Live Pools");
            else if (fileName.StartsWith("Dead"))
                return ("Total Stores for all Dead Pools");
            else if (fileName.StartsWith("Stable"))
                return ("Total Stores of Stable (Soil) Pools");
            else if (fileName.StartsWith("Disposal"))
                return ("Total Stores for Disposed Wood Products");
            else if (fileName.StartsWith("Harvest"))
                return ("Annual Amount of Carbon Harvested");
            else if (fileName.StartsWith("ProductsInUse"))
                return ("Total Stores for Wood Products in Use");
            else if (fileName.StartsWith("ProductSubstitution"))
                return ("Total Stores for Product Substitution");
        }
        else if (fileName.EndsWith("Balance.csv"))
        {
            if (fileName.StartsWith("TotalForestSector"))
                return ("Net Balance for Forest Sector (Ecosystem + Products + Substitution)");
            else if (fileName.StartsWith("TotalForestEcosystem"))
                return ("Net Balance for Forest Ecosystem (Live + Dead + Stable Soils)");
            else if (fileName.StartsWith("TotalForestProduct"))
                return ("Net Balance for Forest Products (Use + Disposal)");
            else if (fileName.StartsWith("TotalSubstitution"))
                return ("Net Balance Related to Forest Substitution (Products + Biofuels)");
            else if (fileName.StartsWith("Biofuel"))
                return ("Production of Biofuels in Manufacturing and Disposal");
            else if (fileName.StartsWith("Live"))
                return ("Net Balance for All Live Pools");
            else if (fileName.StartsWith("Dead"))
                return ("Net Balance for All Dead Pools");
            else if (fileName.StartsWith("Stable"))
                return ("Net Balance of Stable (Soil) Pools");
            else if (fileName.StartsWith("Disposal"))
                return ("Net Balance for Disposed Wood Products");
           else if (fileName.StartsWith("ProductsInUse"))
                return ("Net Balance for Wood Products in Use");
            else if (fileName.StartsWith("ProductSubstitution"))
                return ("Net Balance for Product Substitution");
        }
        else if (fileName.EndsWith("Areas.csv"))
        {
            if (fileName.StartsWith("HarvestEvent"))
                return ("Harvest Events");
            else if (fileName.StartsWith("PrescribedFireEvent"))
                return ("Prescribed Broadcast Burn Events");
            else if (fileName.StartsWith("BurnPileFireEvent"))
                return ("Prescribed Burn Pile Events");
            else if (fileName.StartsWith("WildFireEvent"))
                return ("Wildfire Events");
            else if (fileName.StartsWith("SalvageEvent"))
                return ("Salvage Events");
            else if (fileName.StartsWith("InsectEvent"))
                return ("Insect Events");
        }
        else
        {
            if (fileName.StartsWith("ForestNep"))
                return ("Net Ecosystem Production of Stand (NPP-Rh)");
            else if (fileName.StartsWith("ForestNpp"))
                return ("Net Primary Production of Stand (NPP)");
            else if (fileName.StartsWith("ForestRh"))
                return ("Heterotrophic Respiration of Stand (Rh)");
            else if (fileName.StartsWith("ManufacturingFlux"))
                return ("Output of Solid Forest Products via Manufacturing");
            else if (fileName.StartsWith("HarvestYearArea.txt"))
                return ("Percent of Harvest Target Achieved");
        }

        return ("No Title Found for " + fileName);

    }
}