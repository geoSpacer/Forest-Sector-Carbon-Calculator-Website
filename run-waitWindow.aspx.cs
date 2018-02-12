using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class run_waitWindow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //int accum ;
        //accum = Convert.ToInt32(TimeSpan.Text);
        
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        string fileName = Page.Session["userDir"].ToString() + "\\OutputFiles\\YearTracker.dgn";
        string line = "";
        int runDuration = Convert.ToInt16(Page.Session["currentYear"].ToString()) - Convert.ToInt16(Page.Session["simStartYear"].ToString()) + Convert.ToInt16(Page.Session["numSimYears"].ToString()) + 1;
        int numDigits = 4;

        // read a line of text
        //int accum = Convert.ToInt32(tr.ReadLine());
        //TimeSpan.Text = accum.ToString();
        //fileLines = File.ReadAllLines(fileName);
        //tb_fileViewer.Text = fileLines[fileLines.Length - 1];
        if (File.Exists(fileName))
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                //tb_fileViewer.Text = sr.ReadToEnd();
                while (sr.Peek() != -1)
                {
                    line = sr.ReadLine();
                }
                //while ((line = sr.ReadLine()) != null) ;
            }

            if (line == "")
                line = "Processing Output";

            tb_fileViewer.Text = line;

            if (runDuration < 1000)
                numDigits = 3;

            if (line.Substring(10 - numDigits, numDigits) == (runDuration).ToString())
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "closePage", "window.close();", true);
                //tb_fileViewer.Text += "\n\nDone.";
            }
        }
    }
    

}