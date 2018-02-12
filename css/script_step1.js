// script_step1.js
// Keith Olsen - 25 August 2010
//
// For use with the carbon calculator website


function updateElevZone(selectedRegion){
  var elevationlist = document.getElementById("step1input").elevationClass;

  var elevationGroup = new Array();
  elevationGroup[0]="";
  elevationGroup[1]=["CRZone1|CRzone1value", "CRZone2|CRzone2value", "CRZone3|CRzone3value"];
  elevationGroup[2]=["WCZone1|WCzone1value", "WCZone2|WCzone2value", "WCZone3|WCzone3value"];
  elevationGroup[3]=["KLZone1|KLzone1value", "KLZone2|KLzone2value", "KLZone3|KLzone3value"];
  elevationGroup[4]=["ECZone1|ECzone1value", "ECZone2|ECzone2value", "ECZone3|ECzone3value"];
  elevationGroup[5]=["BMZone1|BMzone1value", "BMZone2|BMzone2value", "BMZone3|BMzone3value"];

  elevationlist.options.length = 1;
  if (selectedRegion > 0){
    for (i=0; i < elevationGroup[selectedRegion].length; i++)
      elevationlist.options[elevationlist.options.length]=new Option(elevationGroup[selectedRegion][i].split("|")[0], elevationGroup[selectedRegion][i].split("|")[1]);
  }
}

function updateSiteIndex(formObject){
  var regionSelect = formObject.regionName.selectedIndex;
  var selectedZone = formObject.elevationClass.selectedIndex;
  var siteIndexlist = formObject.siteIndex

  var siteIndexGroup = new Array(new Array(), new Array(), new Array(), new Array(), new Array(), new Array());
  siteIndexGroup[0][0]="";
  siteIndexGroup[1][1]=["CRZone1SI1|CRzone1SI1value", "CRZone1SI2|CRzone1SI2value"];
  siteIndexGroup[1][2]=["CRZone2SI1|CRzone2SI1value", "CRZone2SI2|CRzone2SI2value"];
  siteIndexGroup[1][3]=["CRZone3SI1|CRzone3SI1value", "CRZone3SI2|CRzone3SI2value"];
  siteIndexGroup[2][1]=["WCZone1SI1|WCzone1SI1value", "WCZone1SI2|WCzone1SI2value"];
  siteIndexGroup[2][2]=["WCZone2SI1|WCzone2SI1value", "WCZone2SI2|WCzone2SI2value"];
  siteIndexGroup[2][3]=["WCZone3SI1|WCzone3SI1value", "WCZone3SI2|WCzone3SI2value"];
  siteIndexGroup[3][1]=["KLZone1SI1|KLzone1SI1value", "KLZone1SI2|KLzone1SI2value"];
  siteIndexGroup[3][2]=["KLZone2SI1|KLzone2SI1value", "KLZone2SI2|KLzone2SI2value"];
  siteIndexGroup[3][3]=["KLZone3SI1|KLzone3SI1value", "KLZone3SI2|KLzone3SI2value"];
  siteIndexGroup[4][1]=["ECZone1SI1|ECzone1SI1value", "ECZone1SI2|ECzone1SI2value"];
  siteIndexGroup[4][2]=["ECZone2SI1|ECzone2SI1value", "ECZone2SI2|ECzone2SI2value"];
  siteIndexGroup[4][3]=["ECZone3SI1|ECzone3SI1value", "ECZone3SI2|ECzone3SI2value"];
  siteIndexGroup[5][1]=["BMZone1SI1|BMzone1SI1value", "BMZone1SI2|BMzone1SI2value"];
  siteIndexGroup[5][2]=["BMZone2SI1|BMzone2SI1value", "BMZone2SI2|BMzone2SI2value"];
  siteIndexGroup[5][3]=["BMZone3SI1|BMzone3SI1value", "BMZone3SI2|BMzone3SI2value"];

  siteIndexlist.options.length = 1;
  if (selectedZone > 0){
    for (i=0; i < siteIndexGroup[regionSelect][selectedZone].length; i++)
      siteIndexlist.options[siteIndexlist.options.length]=new Option(siteIndexGroup[regionSelect][selectedZone][i].split("|")[0], siteIndexGroup[regionSelect][selectedZone][i].split("|")[1]);
  }
}

function setFormFromSession(formObject)
{
    var sessionRegion = '<%=HttpContext.Current.Session["region"]%>';
    var sessionOwn = '<%=HttpContext.Current.Session["own"]%>';
    var sessionElevClass = '<%=HttpContext.Current.Session["elevClass"]%>';
    var sessionSiteIndx = '<%=HttpContext.Current.Session["siteIndx"]%>';
    
    if (sessionRegion != "") {
        for (i=0; i<formObject.regionName.options.length; i++) {
            if (formObject.regionName.options[i].value == sessionRegion) {
                formObject.regionName.selectedIndex = i;
                if (!formObject.regionName.options[i].selected) {
                    formObject.regonName.options[i].selected = true;
                }
            }
        }
   //     alert("Session data " + sessionOwn + "\n" + formObject.regionName.selectedIndex);
        updateElevZone(formObject.regionName.selectedIndex);
        
    }
    if (sessionOwn != "") {
        for (i=0; i<formObject.ownership.options.length; i++) {
            if (formObject.ownership.options[i].value == sessionOwn) {
                formObject.ownership.selectedIndex = i;
                if (!formObject.ownership.options[i].selected) {
                    formObject.ownership.options[i].selected = true;
                }
            }
        }
        
    }
    if (sessionElevClass != "") {
        for (i=0; i<formObject.elevationClass.options.length; i++) {
            if (formObject.elevationClass.options[i].value == sessionElevClass) {
                formObject.elevationClass.selectedIndex = i;
                if (!formObject.elevationClass.options[i].selected) {
                    formObject.elevationClass.options[i].selected = true;
                }
            }
        }
        updateSiteIndex(formObject);
        
    }
    if (sessionSiteIndx != "") {
        for (i=0; i<formObject.siteIndex.options.length; i++) {
            if (formObject.siteIndex.options[i].value == sessionSiteIndx) {
                formObject.siteIndex.selectedIndex = i;
                if (!formObject.siteIndex.options[i].selected) {
                    formObject.siteIndex.options[i].selected = true;
                }
            }
        }
        
    }
}
