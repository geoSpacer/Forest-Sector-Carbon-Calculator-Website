using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class tutorial_examples : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btn_example1output_Click(object sender, EventArgs e)
    {
        Response.ContentType = "application/x-zip-compressed";
        Response.AppendHeader("Content-Disposition", "attachment; filename=run1_FSCC_runOutput.zip");
        Response.TransmitFile("docs\\exampleRuns\\run1_FSCC_runOutput.zip");
        Response.End();
    }
    protected void btn_example2output_Click(object sender, EventArgs e)
    {
        Response.ContentType = "application/x-zip-compressed";
        Response.AppendHeader("Content-Disposition", "attachment; filename=run2_FSCC_runOutput.zip");
        Response.TransmitFile("docs\\exampleRuns\\run2_FSCC_runOutput.zip");
        Response.End();
    }
    protected void btn_example3output_Click(object sender, EventArgs e)
    {
        Response.ContentType = "application/x-zip-compressed";
        Response.AppendHeader("Content-Disposition", "attachment; filename=run3_FSCC_runOutput.zip");
        Response.TransmitFile("docs\\exampleRuns\\run3_FSCC_runOutput.zip");
        Response.End();
    }
    protected void btn_example4output_Click(object sender, EventArgs e)
    {
        Response.ContentType = "application/x-zip-compressed";
        Response.AppendHeader("Content-Disposition", "attachment; filename=run4_FSCC_runOutput.zip");
        Response.TransmitFile("docs\\exampleRuns\\run4_FSCC_runOutput.zip");
        Response.End();
    }
    protected void btn_example5output_Click(object sender, EventArgs e)
    {
        Response.ContentType = "application/x-zip-compressed";
        Response.AppendHeader("Content-Disposition", "attachment; filename=run5_FSCC_runOutput.zip");
        Response.TransmitFile("docs\\exampleRuns\\run5_FSCC_runOutput.zip");
        Response.End();
    }
}