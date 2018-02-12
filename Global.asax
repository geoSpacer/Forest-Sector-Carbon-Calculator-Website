<%@ Application Language="C#" %>
<%@ Import Namespace="System.Collections.Generic" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started
        Session.Clear();
        Session["managementDict"] = new SortedList<string, ArrayList>();
        Session["natDisturbDict"] = new SortedList<string, ArrayList>();
        Session["currentYear"] = "2012";
        Session["numSimYears"] = "100";
        Session["cellAreaHa"] = "1";
        Session["runName"] = "Run1";
    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
