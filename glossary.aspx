<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
<head>
  <title>The Forest Sector Carbon Calculator</title>
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <meta name="Keywords" content="forest, forest management, carbon calculator, forest products, forest sector, wildfire, disturbance regime" />
  <link href="css/carbon.css" type="text/css" rel="stylesheet" />
  <link href="css/sb_minitabs.css" type="text/css" rel="stylesheet" />
  <link href="css/csshorizontalmenu.css" type="text/css" rel="stylesheet" />
  <script src="css/csshorizontalmenu.js" type="text/javascript"></script>
  <script src="css/script_carbon.js" type="text/javascript"></script>
</head>

<body>
<div id="mainPageDiv">
    <script type="text/javascript">loadNavMenu();</script>
    <noscript><br /><h1 style="text-align:center;">The Forest Sector Carbon Calculator</h1>
        <h1 style="text-align:center; color:Red;">Please Enable Javascript Before Proceeding</h1></noscript>

	<div id="main">
    	<div id="mainPageTitle">Glossary of Terms</div>
        <p>&nbsp;</p>
        <ul>
            <li><strong>Autotrophic respiration (Ra)</strong> - the respiration associated with primary producers or plants.</li>
            <li><a name="balance"></a><strong>Balance</strong> - the net difference between input and output flows or 
                the net change in stores of a pool. They have units of MgC/ha/year.</li>
            <li><strong>Bioenergy recovery</strong> - the process of recovering energy contained 
                within wood products when they are burned in disposal. </li>
            <li><strong>Biofuel offset</strong> - the amount of fossil carbon that is replaced 
                by a biofuel derived from forest harvest.</li>
            <li><a name="dead"></a><strong>Dead</strong> -&nbsp; this includes all the dead vegetation parts that 
                have not decomposed sufficiently to form stable material.</li>
            <li><a name="displacementFactor"></a><strong>Displacement factor</strong> -  the tons of fossil carbon that is not used because a ton of long-term 
                wood products or bioenegry is used. For wood products this value is usually over 1.   For bioenergy, the value is less than 0.9 or 
                90% expressed as a percentage.  </li>
            <li><a name="disposal"></a><strong>Disposal</strong> - the fate of wood products once their use has ended. 
                In some cases products are stored in disposal sites (e.g., landfills); in other 
                cases disposed products are combusted.</li>
            <li><strong>Disturbance history</strong> - the series of disturbance events (natural 
                or human-caused) that a stand has undergone.</li>
            <li><strong>Drivers</strong> - variables that “drive” the system from the outside 
                such as climate, radiation, soil properties, etc.</li>
            <li><strong>Events</strong> - the timing disturbances in a stand, or the fraction of 
                the landscape experiencing particular disturbances.&nbsp;</li>
            <li><strong>Flow</strong> - the movement of carbon from one pool to another. They 
                have units of MgC/ha/year. &nbsp; </li>
            <li><strong>Forest ecosystem</strong> - this includes the live, dead, and stable 
                pools.</li>
            <li><a name="forest_products"></a><strong>Forest products</strong> - this includes the stores of harvested carbon 
                in use or in disposal. It does not include carbon offsets as these are treated 
                separately.&nbsp;</li>
            <li><a name="forest_sector"></a><strong>Forest sector</strong> - this includes the forest ecosystem, the wood 
                products harvested from the forest, and any carbon offsets. </li>
            <li><strong>Gross primary production (GPP)</strong> - is the total amount of carbon 
                fixed by primary producers in a given area or ecosystem.</li>
            <li><strong>Harvest</strong> - the amount of carbon removed from a forest.</li>
            <li><strong>Heterotroph</strong> - an organism that can not make it’s own food and 
                depends on other organisms as a food source.</li>
            <li><strong>Heterotrophic respiration (Rh)</strong> - the respiration associated 
                with organisms that can not photosynthesize (e.g., fungi).</li>
            <li><strong>Input parameters</strong> - variables that control the rate processes 
                occur or the relationship between pools.</li>
            <li><strong>Landfill</strong> - a disposal form in which decomposition is generally 
                slow because the disposed material is sealed and not exposed to the atmosphere.</li>
            <li><a name="landscape"></a><strong>Landscape</strong> - an area of forested land in which the disturbance 
                history is asynchronous from area to area. The climate, soils, and species 
                mixture has the potential to be inhomogeneous.</li>
            <li><a name="leakage"></a><strong>Leakage</strong> - this process occurs when market forces counter an action in one place or time with another action at another 
                time or place.  For example, less fossil carbon use would lead to more supply and more supply might lead to lower prices, and that ultimately 
                could lead to the displaced carbon being used. </li>
            <li><a name="live"></a><strong>Live</strong> - this includes all live vegetation parts (foliage, 
                branches, stems, and roots) for the following life-forms: trees, shrubs, and 
                herbs.</li>
            <li><strong>Long-term structures</strong> - products that have life-spans less.</li>
            <li><strong>Manufacturing</strong> - the process by which raw materials such as wood 
                and bark are converted to wood products.</li>
            <li><strong>Net primary production (NPP)</strong> - the rate at which new biomass or 
                carbon is added to an ecosystem.</li>
            <li><strong>Offset</strong> - an offset is not a store per se, but an accounting 
                unit that assumes carbon is stored elsewhere (in the context of this model in 
                the ground as unused fossil carbon).</li>
            <li><strong>Open dump</strong> - a form of disposal in which the disposed material 
                is exposed to the atmosphere and subject to aerobic decomposition processes and 
                combustion.</li>
            <li><strong>Parameters</strong> - variables that control the rate processes occur or 
                the relationship between pools and other state variables.</li>
            <li><a name="post_harvest_carbon_use"></a><strong>Post-harvest carbon use</strong> - the fate of carbon that has be 
                removed from the forest ecosystem.</li>
            <li><strong>Prescribed fire</strong> - a fire that is planned and set by humans. It 
                may occur after a harvest or independent of a harvest.</li>
            <li><strong>Primary producers</strong> - organisms that can make their own food; an 
                autotroph.</li>
            <li><strong>Primary treatment</strong> - the most important management treatment a 
                stand receives. An example might be a final harvest of a management cycle.</li>
            <li><a name="product_use"></a><strong>Product use</strong> - the way wood products manufacturing outputs are 
                used.</li>
            <li><strong>Random number seed</strong> - a starting value for a pseudo-random 
                number generator. When the same seed value is used, the same random number is 
                generated.</li>
            <li><strong>Recycling</strong> - the process by which disposed products are re-used. 
                We assume that products are recycled into their original use.</li>
            <li><a name="reference_calendar_year"></a><strong>Reference calendar year</strong> - the year the simulation transitions from
                the historical or 'spinup' part of the simulation into the future part of the simulation.</li>
            <li><strong>Regime</strong> - the group of disturbance events that characterize a 
                landscape. Regimes are described by the type of disturbance, the interval 
                between disturbance events, the severity of the disturbances, and the fraction 
                of a given stand that is impacted by disturbance events.</li>
            <li><strong>Risk analysis</strong> - a method that examines multiple possible 
                outcomes based on the random occurrence of events that are not the main focus of 
                study. </li>
            <li><strong>Secondary treatment</strong> - the management treatment a stand receives 
                that supports the primary treatment. An example might be either series of 
                thinnings during a management cycle, or a prescribed fire following a clear-cut 
                harvest.</li>
            <li><strong>Severity level</strong> - this indicates the amount of carbon killed and 
                removed by a disturbance.</li>
            <li><strong>Short-term structures</strong> - products that have life-spans less than 
                30 years. </li>
            <li><strong>Spin-up</strong> - a period in a simulation in which the model is 
                calculating the starting conditions for a simulation experiment. Typically the 
                results of spin-up period are ignored in the analysis. </li>
            <li><strong>Stable</strong> - this includes carbon in the mineral soil, 
                well-decomposed foliage and wood, and charcoal. </li>
            <li><a name="stand"></a><strong>Stand</strong> - an area of forested land in which the disturbance 
                history is synchronous from area to area. The climate, soils, and species 
                mixture is relatively homogeneous. </li>
            <li><strong>State variable</strong> - a variable that describes the condition of the 
                system.</li>
            <li><strong>Store</strong> - the amount of carbon within a forest area or a product 
                that was harvested from that forest area. It can be specifically located as 
                opposed to an offset which can not be specifically located. Stores have units of 
                MgC/ha.</li>
            <li><strong>Substitution</strong> - in a general sense substitution is the replacement 
                of one thing with another.  In the context of forest sector carbon there are two kinds of substitutions: 
                products- and energy-related. Energy-related ones are the most straightforward: energy from the forest 
                sector is used to replace energy from fossil fuels.  Since some fossil carbon is not used to create energy, 
                this can be counted as a store of carbon.  For product substitutions the use of wood can lead to lower use of 
                materials that take more energy to manufacture.  This could lead to less fossil energy being used 
                (it depends on the how and what of manufacturing) and this in turn could lead to a lower use of fossil carbon; 
                hence a kind of carbon store.  Because substitution stores of fossil carbon cannot be measured directly 
                they are best thought of as virtual stores that may or may not exist depending on the accounting framework adopted.</li>
            <li><strong>Total suppression</strong> - management actions that completely 
                eliminate wildfires. While not realistic it allows one to examine forests 
                without any wildfires.</li>
            <li><strong>Treatment</strong> - the kinds of disturbance or management events that 
                occur.</li>
            <li><strong>Typical suppression</strong> - management actions that reduces the 
                incidence of wildfires, but does not eliminate them. We assume that typical 
                suppression will reduce the frequency of wildfire 50%.</li>
            <li><a name="uncertainty_analysis"></a><strong>Uncertainty analysis</strong> - a method that examines multiple possible 
                outcomes based on the certainty of parameters or driving variables. </li>
            <li><strong>Utilization level</strong> - this combines the proportion of live carbon 
                felled or cut and the proportion of carbon removed from a forest. </li>
            <li><strong>Virtual store</strong> - unlike and actual store, a virtual store is one that is a result of accounting. Unlike an actual store, 
                the specific location and form of a virtual store cannot be identified.  Substitutions can be treated as a store, but since the specific 
                location and form of the fossil carbon cannot be identified, one can only estimate the value by establishing accounting rules that are strongly 
                influenced by the assumptions being made.  </li>
            <li><strong>Wildfire</strong> - a fire that is not planned; it can be started by nature or humans.</li>
            <li><strong>Year of simulation</strong> - the calendar year of the simulation. This 
                is set by user when they select the reference calendar year.</li>
       </ul>
    </div>
    <script type= "text/javascript">loadFooter();</script>
  </div>
  <div style="margin-top: 1000px;">&nbsp;</div>
</body>
</html>