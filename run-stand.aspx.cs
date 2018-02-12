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

using System.IO;
using System.Diagnostics;  // for appPath
using System.Xml;
using System.Collections.Generic;  // for sortedList

public partial class run_stand : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["scale"] = "stand";

            // set grid size for stand
            Session["simCols"] = 5;
            Session["simRows"] = 5;

            // if management dictionary has records from landscape mode, delete records
            SortedList<string, ArrayList> managementDict = new SortedList<string, ArrayList>();
            managementDict = (SortedList<string, ArrayList>)Session["managementDict"];
            if (managementDict.Count > 0)
                if (managementDict.Keys[0].Substring(7, 4) != "0000")
                    managementDict.Clear();

            // if natDisturb dictionary has records from landscape mode, delete records
            SortedList<string, ArrayList> natDisturbDict = new SortedList<string, ArrayList>();
            natDisturbDict = (SortedList<string, ArrayList>)Session["natDisturbDict"];
            if (natDisturbDict.Count > 0)
                natDisturbDict.Clear();


            //    ***** Delete all user directories more than 2 days old
            try
            {
                // Only get subdirectories that begin with "user"
                string appPath = Request.PhysicalApplicationPath; // requires System.Diagnostics
                string landCarbDir = "LandCarbData31";
                string[] dirs = Directory.GetDirectories(appPath + landCarbDir, "user*");

                foreach (string dir in dirs)
                {

                    // Get the creation time of a well-known directory.
                    DateTime dt = Directory.GetCreationTime(dir);
                    //TextBox1.Text += "\n" + dir + " is " + DateTime.Now.Subtract(dt).TotalDays.ToString() + " days old";
                    if (DateTime.Now.Subtract(dt).TotalDays >= 2)
                    {
                        System.IO.Directory.Delete(dir, true);
                        // TextBox1.Text += "\nDirectory " + dir + " deleted";
                    }
                }
            }
            catch // (Exception ex)
            {
                // Label_upload.Text = ex.Message.ToString();
                //  TextBox1.Text += "Delete failed: " + ex.ToString();
                //  Response.Write(@"<script language='javascript'>
                //  alert('Error deleting user directories > 2 days old on server.');
                //  </script>");
                //                MessageBox.Show("Error deleting user directories > 2 days old on server", "Deletion Error",
                //                   MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
    protected void btn0next_Click(object sender, EventArgs e)
    {
        Response.Redirect("run-both-1site.aspx");
    }

    protected void btn0load_Click(object sender, EventArgs e)
    {
        Response.Redirect("run-both-loadXML.aspx");
    }
}
