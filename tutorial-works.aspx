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
    <div id="mainPageTitle">How It Works</div>
    <p>&nbsp;</p>
    <h3>Introduction</h3>
        This user's guide describes the third version of a model called LANDCARB that is designed to simulate the dynamics of 
        living and dead pools of carbon in a forest landscape.  LANDCARB 3.0 can be used to examine the effects that climate, tree species, 
        succession, natural disturbances such as wildfire, and management activities such as timber harvest and salvage, site preparation, 
        and artificial regeneration have on carbon dynamics at the landscape level.  These results are presented in ASCII output files 
        that can be used to examine the spatial and temporal pattern of carbon stores and fluxes.  While this model also estimates the 
        mass and volume of boles removed by harvest, it tracks the fate of this harvested material using a highly aggregated submodel.  
        Another model should be used if one intends to examine changes in forest products mixes and uses (e.g., Harmon et al. 1996).
        
        <p>Because LANDCARB 3.0 is designed to operate at the landscape level, it has incorporated finer levels of resolution within its 
        computations using a meta-model approach.  The meta-model approach is used to capture the overall response of more detailed simulation 
        models of a phenomenon without all the computational burdens entailed in including the full model.  LANDCARB 3.0 has been set 
        up to mimic the stand level behaviors found in STANDCARB 2.0.  This includes heterogeneity of species and ages, age-specific changes 
        in the tree mortality rate as well as temporal lags associated with heart-rot and decomposition. LANDCARB 3.0 includes all the carbon 
        pools that STANDCARB 2.0 does and all the same stand-level processes.  To the degree possible the parameter files used by two models 
        are the same, although they may be used in slightly different ways. Because the models are closely interrelated one can examine the 
        effects of similar treatments at different spatial extents and expect comparable answers.  For example, one might use STANDCARB 2.0 
        to test the effects of the interval between harvests, and LANDCARB 3.0 to test how landscape level carbon stores will change if the 
        harvest intervals are implemented on a given landscape forest history.</p>
        
        <p>Temporally, LANDCARB 3.0 is a difference model that operates on an annual time step for all variables, except those used to estimate 
        the effects of climate on tree establishment, growth, and decomposition.  These climate related variables are calculated on a 
        monthly time step.  In addition, while disturbances are simulated annually, there are arbitrary semiannual timesteps that occur 
        once the normal growth and decomposition related processes are addressed.</p>
        
        <p>Spatially, LANDCARB 3.0  is designed to simulate the dynamics of a number of cells within a landscape.  Each cell represents 
        the area occupied by a stand of trees, which can range from 0.25 to 100 ha.  The stand of trees need not be homogeneous, because 
        LANDCARB 3.0 uses a tree cohort structure to allow disturbances that remove part of each stand.  The spatial location of these cohorts 
        is not modeled directly, but the spatial interactions of cohorts is included. This approach allows the model to efficiently mimic the 
        substand level disturbances that can be modeled in STANDCARB 2.0 without greatly imcreasing the computational demands.</p>
        
        <p>This users guide is designed to explain how to use the LANDCARB 3.0 model to investigate the effects of various types of forest 
        management at the landscape level on live and dead carbon stores.  We first provide an overview of the objectives and structure 
        of the model.  This is followed by a description of the modules used to run simulations.  A brief summary of each of the major 
        sections of the model is then described with particular attention to the equations used for critical calculations.  Finally, the 
        types and structures of the input and output files are defined.</p>
        
        <p>Before using the model a final word of caution is in order.  LANDCARB 3.0 is a simulation model.  As such, it represents our best 
        representation of reality, but the results must be used with caution.  There are many factors that may cause the projected results to 
        deviate from what actually occurs.  This is no different than the distinction between volume yield projections and the actual 
        harvested volume.  Bear in mind each simulation has a number of tacit assumptions, and when these are not met, the projected 
        results may be entirely misleading.  It may also be the case that the simulations are correct in a relative sense but not in 
        an absolute sense.  When interpreting results bear in mind that relative differences will always be more robust than absolute 
        differences.  Finally, it must be kept in mind that simulation models are only tools to be used primarily for planning or 
        understanding how system works.  They are not a substitute for actual measurements of the actual forest carbon stores of a particular landscape.
        </p>

    <h3>Objectives</h3>
    The object of LANDCARB 3.0 is to simulate the accumulation of carbon over succession in a landscape with mixed species-mixed aged forest stands 
    and spatially variable climate, soil, topography, and history.  This version of the model is currently parameterized for the Pacific Northwest.  
    There is no reason, however, that it could not be used for other types of forests as well.  The model can be used to investigate the landscape 
    level effects of various regeneration strategies, harvesting, herbiciding, salvage, patch cutting, tree species replacement by design or by 
    natural succession, site preparation, and wildfires. 
    
    <p>The model provides output on seven live state variables, nine detritus (partially decomposed) state variables, three stable (highly decomposed) 
    state variables, two charcoal relate variables and the volume harvested (see Output files section for more details).  
    </p>
    <p>LANDCARB incorporates the notion of multiple biological levels that each contribute to how carbon changes in forests (Table 2-1).  
    Many carbon models incorporate at least several biological levels, typically physiological and ecosystem ones. LANDCARB is somewhat unsual 
    in that it includes five: physiological, population, community, ecosystem, and landscape.  Through various parameterization of the model, it 
    is possible to remove several of these biological levels, allowing to one to test the kinds of behaviors that disappear when they are removed. 

    </p>
    <h3>Basic Approach</h3>
        <a href="images/figure2-1.jpg"><img alt="Figure 2-1" title="Figure 2-1" src="images/figure2-1.jpg" width="450px" align="right" style="padding-left: 10px; border: 0 none;" /></a>
    The approach used in LANDCARB 3.0 is to divide the landscape into a grid of cells, each grid representing a stand. Each stand is part of a specific 
    climate, soil, radiation, disturbance and seed zone (Figure 2-1). The interactions of climate, soil, and radiation are modeled at the zone level and 
    then applied to individual stands. Disturbance zones are used to characterize the sub-landscape disturbance regime, although the particular effect of 
    each disturbance is calculated for each stand depending on its state and that of its neighbors. Seed zones specify the seed sources likely to be be 
    present and their abundance, but as with disturbance the actual seed sources are dependent on the state of neighboring cells. Within each stand the 
    abundance of different plant life forms is simulated by tracking colonization, growth, and mortality.  Disturbances can create within-stand 
    heterogeneity representing disturbances that do not kill the entire stand of trees. These cohorts of tree regeneration are allowed to interact 
    to mimic the effect of older cohorts on younger cohorts.   

        <p><a href="images/figure2-2.jpg"><img alt="Figure 2-2" title="Figure 2-2" src="images/figure2-2.jpg" width="450px" align="right" style="padding-left: 10px; border: 0 none;" /></a>
    LANDCARB 3.0 has a number of levels of organization it uses to estimate changes in carbon stores within a landscape (Figure 2-2).  
    At the highest level there is a landscape comprized of  stand grid cells, each which represents part of a stand of trees (given this is a 
    raster or gridbased landscape depiction, a stand is any set of adjacent cells which have a similar species composition, environment, and 
    disturbance history).  Each stand grid cell belongs to a series of zones describing key landscape attributes (disturbance regime, climate, 
    soil, radiation, seed sources, etc).  Each stand grid cell contains a number of cohorts (in the program this is called a cohort set) that 
    represent different episodes of disturbance and colonization. Each cohort contains up to four layers of vegetation each having up to seven 
    live parts, eight dead pools, three stable pools representing highly decomposed material, and two pools representing charcoal.  The four 
    layers of vegetation that can occur in each cohort are upper trees, lower trees, shrubs, and herbs.  The two tree layers can have 
    different species, whereas the shrub and herb layers are viewed as each representing a mix of species.  Each cell can have any combination 
    of layers except that lower trees can only occur when upper trees are present.  Each layer of plants has an age-class structure reflecting 
    gradual colonization of each cohort. 
    </p>

        <p><a href="images/figure2-3.jpg"><img alt="Figure 2-3" title="Figure 2-3" src="images/figure2-3.jpg" width="450px" align="right" style="padding-left: 10px; border: 0 none;" /></a>
    Each of the layers in a cohort can potentially have seven live parts: 1) foliage, 2) fine roots, 3) branches, 4) sapwood, 5) heartwood, 
    6) coarse roots, and 7) heart-rot (Figure 2-3).  To make the layers correspond to the actual structure of certain life forms, herbs are restricted from 
    having woody parts and shrubs cannot have heartwood or heart rot (as they do not form a bole).  All the live parts correspond to parts typically reported 
    in the ecological literature with the exception of the bole.  The later would be composed of sapwood, heartwood, and heart rot.  In our model heartwood 
    includes the heartwood and the outer bark as these are non respiring, decay-resistant layers.  The sapwood includes the sapwood and the inner bark 
    layers as these are respiring and decompose relatively quickly compared to outer bark or heartwood.  Heart rot represents the portion of the 
    heartwood that is being degraded by parasitic and saprophytic fungi inside the living trees.  
    </p>

    <p>Each of the live parts of each layer contributes material to a corresponding dead pool.  Thus foliage adds material to the dead foliage, 
    fine roots to dead fine roots, branches to dead branches, sapwood to dead sapwood, heartwood and heart rot to dead heartwood, and coarse roots to 
    dead coarse roots.  Rather than have every plant layer in each cell have its own dead pool, we have combined all the inputs from the layers of the 
    cell to form a single detrital pool for each plant part.  For example, the foliage from the four plant layers feeds into a single dead foliage pool.  
    We have also separated dead sapwood and dead heartwood into snags versus logs so that the effects of position on microclimate can be modeled.  
    Snags and logs are further divided into salvageable versus unsalvagable fractions so that realistic amounts of dead trees can be removed during 
    simulated salvage operations.   All the detritus pools in a cell can potentially add material to one of three stable pools (stable foliage, 
    stable wood, stable soil).  The objective is to simulate a pool of highly decomposed material that changes slowly and is quite resistant to 
    decomposition.  Finally, fires can create surface charcoal from live parts or dead pools. Surface charcoal can be incorporated into the mineral 
    soil and become protected from future fires as buried charcoal, whereas surface charcoal can be lost during subsequent fires. 
    </p>
    <h3>Computing Environment</h3>
    LandCarb is developed under Microsoft Visual C++ for .NET, using Visual Studio 2005. Most of the program code is written in ANSI C++. A small portion of code, 
    which manages the user interface, is written in C++/CLI. LandCarb is distributed as an executable program with supporting libraries. It runs under Microsoft 
    Windows XP and requires version 2.0 of the .NET Framework (this is available as a free download at www.microsoft.com).

    </div>
    <script type= "text/javascript">loadFooter();</script>
  </div>

</body>
</html>