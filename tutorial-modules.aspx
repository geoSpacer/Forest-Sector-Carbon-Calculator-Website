<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
<head>
  <title>The Forest Sector Carbon Calculator</title>
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <link href="css/carbon.css" type="text/css" rel="stylesheet" />
  <link href="css/sb_minitabs.css" type="text/css" rel="stylesheet" />
  <link href="css/csshorizontalmenu.css" type="text/css" rel="stylesheet" />
  <script src="css/csshorizontalmenu.js" type="text/javascript"></script>
  <script src="css/script_carbon.js" type="text/javascript"></script>
</head>

<body>
  <div id="mainPageDiv">
    <script type= "text/javascript">loadNavMenu();</script>
    <noscript><br /><h1 style="text-align:center;">The Forest Sector Carbon Calculator</h1>
        <h1 style="text-align:center; color:Red;">Please Enable Javascript Before Proceeding</h1></noscript>

	<div id="main">
        <div id="mainPageTitle">Model Module Overview</div>
        <p>&nbsp;</p>
        <a href="images/figure2-4.jpg"><img alt="Figure 2-4" title="Figure 2-4" src="images/figure2-4.jpg" width="450px" align="right" style="padding-left: 10px; border: 0 none;" /></a>
        LANDCARB 3.0 consists of a number of modules which perform specific functions. See figure 2-4 for module relationships. The following describes the general purpose of each module.  
        A fuller description of each can be found by clicking [view] next to each heading. 

        <h3>Climate - <a href="docs/3-CLIMATE2.pdf">[view]</a></h3>
        The purpose of this module is to determine the effect of climate on tree species establishment, growth, and decomposition.  
        Climatic effects are calculated for each combination of climate-, soil-, solar radiation-zone assuming an old-growth stand structure. 
        The results are then used for each  stand grid cell that belongs to this combination of zones.
        
        <h3>Soiltexture - <a href="docs/4-SOILTEXT.pdf">[view]</a></h3>
        This module is used to calculate the effects of soil texture, depth and rockiness on the water holding capacity of soils in a soil zone.  
        These results are used by the CLIMATE module.
        
        <h3>Neighbor - <a href="docs/5-NEIGHBOR.pdf">[view]</a></h3>
        The purpose of this module is to determine the overall light environment of a cohort and the interaction with neighboring cohorts and stands.  The degree of 
        blocking of light is determined by the relative height of cohorts and the average distance between cohorts. A similar process is used to determine the influence 
        of one stand gird cell upon another. Height of the tree layer is a function of the age of that layer in a cohort. The tree height of a stand grid cell is the 
        average height of all the cohorts in a stand grid cell. The average distance between cohorts is determined from the number of within stand grid cell patches a 
        disturbance forms.   The greater the number of patches, the shorter the average distance between cohorts. The distance between stand grid cells is determined 
        by the stand grid cell width used to portray the landscape.  The heights and distances are used to compute the average angle between cohorts or stand grid cells. 
        The steeper the angle, the lower the light amount passed onto the lower cohort or stand grid cell. 
        
        <h3>Plant - <a href="docs/6-PLANT.pdf">[view]</a></h3>
        This module determines the age-class structure of plant layers in a cohort.  The age-class structure of all layers is based on a fixed probability. 
        Each cohort is given a limited time to have its layers established and if all the area is not used in the allotted time, then a new cohort is established. 
        This assures that each cohort has a narrow age range and age classes will be compariable in terms of size, species, etc. The upper tree layer is planted 
        at the same time as the lower tree layer.  This is unlike STANDCARB, in which the lower tree is always planted after the upper tree.  The probability of 
        a tree species colonizing a site is a function of shade tolerance, temperature, moisture limits, and the local abundance of species in surrounding stand grid cells.
        
        <h3>Dieout - <a href="docs/7-DIEOUT.pdf">[view]</a></h3>  
        The purpose of this module is simulate the upper tree layer dying out in a cohort.  This reflects the fact that above a certain age, trees are unable 
        to spread horizontally.  When trees in this state die either through normal mortality processes or disturbance they can not replace themselves, hence 
        the area they cover decreases. This allows more light to reach the lower trees and consequently the underlying tree layer grows faster and eventually 
        assumes dominance relative to the upper tree layer.  This allows LANDCARB 2.0 to simulate species replacement within a cohort. 
        
        <h3>Growth - <a href="docs/8-GROWTH.pdf">[view]</a></h3> 
        This module determines the rate that living plant parts grow in a cohort.  The living parts tracked by the GROWTH module include foliage, branches, 
        sapwood, heartwood, heart-rot, coarse roots, and fine roots.  The rate of growth is dependent upon the amount of foliage within a cohort and the maximum 
        rate of net production as determined by the CLIMATE module.  The growth of foliage for each layer is dependent on the amount of light it receives, 
        and the layers light extinction rate and light compensation point. Foliage mass is adjusted to reflect the lags in growth caused by the age-class structure.
        
        <h3>Mortality - <a href="docs/9-MORTALITY.pdf">[view]</a></h3>
        This module determines the rate of detrital production when a cohort has not been harvested or burned.  For foliage and fine roots, a fixed proportion is assumed 
        to die each year.  These proportions are functions of the species (e.g., deciduous trees and herbs lose all their leaves each year).  The proportion of branches, 
        and coarse roots lost to pruning is a function of the light environment, as calculated in the GROWTH module, so that as the light passing through the foliage of 
        a layer approaches the light compensation point, the pruning rate reaches a maximum.  The proportion of bole-related parts, branches, and coarse roots lost to 
        mortality varies with tree age.  Initially mortality is a function of the light environment, so that as the light passing through the foliage of a layer 
        approaches the light compensation point, the mortality rate reaches a maximum. Once trees reach their maximum horizontal extent, mortality is determined 
        from the maximum age of trees. The transition from one form of mortality (i.e., density dependent to density independent) is influenced by the age-class 
        structure of the tree layers. The mortality function also determines the proportion of trees dying from natural causes that form snags versus logs.  
        This is a function of the zone a stand grid cell occurs in and the age of the cohort.   
        
        <h3>Decompose - <a href="docs/10-DECOMP.pdf">[view]</a></h3>
        This module determines the balance of inputs from normal mortality, harvesting and fires, and the losses from decomposition and fire for each cohort.  
        These balances are calculated for each of the eight detritus pools (dead foliage, dead fine roots, dead coarse roots, dead branches, dead sapwood, 
        and dead heartwood; the latter two subdivided into snags and logs) and three stable pools (stable foliage, stable wood, and stable soil organic matter).  
        In addition to these 11 pools, this module calculates total detritus (excludes stable pools), total stable, and total dead stores.  The MORTALITY, 
        HARVEST, PRESCRIBED FIRE, and WILDFIRE module are used to calculate detritus inputs.  The rates of decomposition of each pool are determined by the 
        species contributing detritus to a plot, and climatic effects as calculated in the CLIMATE module.  Losses from fires are determined by the SITEPREP 
        module.  Lags associated with the formation of stable material, degradation of salvageable wood, and snag fall are approximated by altering the relevant 
        transfer rate-constants when inputs exceed the long-erm average value. 
        
        <h3>Harvest - <a href="docs/11-HARVEST.pdf">[view]</a></h3> 
        This module determines how a stand grid cell is to be harvested or salvaged and the amount of live plant parts removed from each cohort versus the 
        amount added to detrital pools.  Harvest can remove part or all of the upper or lower tree layers and dead wood that is salvagable. For each simulation, 
        the user can set the levels of utilization standards (the amount cut and removed).  Only sapwood and heartwood (i.e., boles) either alive or dead can be 
        removed from the simulated forest.  All other living pools (leaves, branches, fine roots, and coarse roots) are transferred to the appropriate dead pools after a harvest.

        <h3>Prescribed Fire - <a href="docs/12-PRESCRIBED FIRE.pdf">[view]</a></h3>
        This module determines the effect of prescribed fires in a cohort. Prescribed fires can occur on their own or after timber harvest. 
        This module kills plant layers, burns live parts as well as dead and stable pools, and forms charcoal. 
        
        <h3>Wildfire - <a href="docs/13-WILDFIRE.pdf">[view]</a></h3> 
        This module determines the effect of wildfires fires in a cohort.  This module kills plant layers, burns live parts as well as dead and stable pools, and forms charcoal.  

        <h3>Forest Products - <a href="docs/14-forest products 3.pdf">[view]</a></h3>
        This module processes the harvested carbon from grid cells into forest wood products that are used and disposed. In addition, some of the harvest can be
        processed for either internal or external bioenergy. This model is patterned after the FORPROD model (Harmon et al. 1996), although some pools have been combined and
        bioenergy has been added.

        <h3>Seed Dispersal</h3>
        This module determines the mixture of species and the abundance of tree seed sources. Seeds are assumed to originate from neighboring cells, the maximum dispersal distance 
        determines the number of neighboring stand grid cells considered. For carbon calculator runs, this module is turned off.

        <h3>Harvest Scheduler - <a href="docs/16-HARVEST SCHEDULER.pdf">[view]</a></h3> 
        This module determines when and where a timber harvest occurs.  In addition it determines the harvest level employed.
        
        <h3>Prescribed Fire Scheduler - <a href="docs/17-PRESCRIBED FIRE SCHEDULER.pdf">[view]</a></h3> 
        This module determines when and where a prescribed fire occurs.  Prescribed fire can occur after harvest as a site preparation step or be independent of harvest.  
        
        <h3>Wildfire Scheduler - <a href="docs/18-WILDFIRE SCHEDULER.pdf">[view]</a></h3>
        This module determines when and where a wildfire occurs. The occurrence of wildfire is random, but determined by an average interval between fires. In addition it 
        determines the most likely fire severity that will occur. See the <a href="docs/Carbon_Calculator_Fire_History_Parameterization.pdf">Carbon Calculator Fire History Parameterization</a> writeup.
        
        <h3>Ecosystem Flows - <a href="docs/19-ecosystem flux calculations.pdf">[view]</a></h3>
        This uses data produced by the model to calculate important ecosystem flows related to the carbon cycle of forests such as net primary production (NPP), 
        heterotrophic respiration (Rh), and net ecosystem production (NEP).  This allows one to compare results to other forest ecosystem models and field data.

    </div>
    <script type= "text/javascript">loadFooter();</script>
  </div>

</body>
</html>