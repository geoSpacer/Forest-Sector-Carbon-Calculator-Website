using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;

public partial class contact : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        string server = "fsl.orst.edu";
        MailAddress from = new MailAddress(txtName.Text + " <" + txtFromEmail.Text + ">");
        MailAddress to = new MailAddress("Keith.Olsen@oregonstate.edu");
        MailMessage message = new MailMessage(from, to);
        message.Subject = "[Carbon Calc Feedback] " + txtSubject.Text;
        message.Body = @txtMessage.Text + "\n\nOrganization: " + @txtOrganization.Text;
        SmtpClient client = new SmtpClient(server);
        Console.WriteLine("Sending an e-mail message to {0} by using SMTP host {1} port {2}.",
             to.ToString(), client.Host, client.Port);

        try
        {
            client.Send(message);
            Response.Redirect("default.aspx");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception caught in CreateTestMessage4(): {0}",
                  ex.ToString());
        }
    }
}