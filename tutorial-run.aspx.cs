using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;

public partial class tutorial_run : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        firstButton_Land.Attributes.Add("OnClick", "return setImageFirst('slideShowEx2');");
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public static Slide[] GetSlides()
    {
        Slide[] slideArray = new Slide[38];

        for (int i = 1; i <= 38; i++)
             slideArray[i - 1] = new Slide("images/slides/stand/stand-slide-" + i.ToString() + ".jpg", "Stand Slide " + i.ToString(), string.Empty);

        return slideArray;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    public static Slide[] GetSlides_Landscape()
    {
        Slide[] slideArray = new Slide[33];

        for (int i = 1; i <= 33; i++)
            slideArray[i - 1] = new Slide("images/slides/landscape/landscape-slide-" + i.ToString() + ".jpg", "Landscape Slide " + i.ToString(), string.Empty);

        return slideArray;
    }

}