<%@ Page Language="C#" AutoEventWireup="true" CodeFile="run-waitWindow.aspx.cs" Inherits="run_waitWindow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forest Sector Carbon Calculator</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h1>Please wait. The simulation is running.</h1>
    <h2>When the simulation is finished, the output page will appear</h2>
    <p>&nbsp;</p><div style='text-align: center;'><img alt='Busy. Please wait.' src='images/busy.gif' align='middle' /></div>
    <asp:ScriptManager ID="ScriptManager1" runat="server" 
            AsyncPostBackErrorMessage="Ajax error">
        </asp:ScriptManager>
         <asp:Timer ID="Timer1" runat="server" Interval="2000" ontick="Timer1_Tick">
        </asp:Timer>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick">
                </asp:AsyncPostBackTrigger>
            </Triggers>
             <ContentTemplate>
               <p><asp:TextBox ID="tb_fileViewer" runat="server" Height="200px" Width="600px" TextMode="MultiLine"></asp:TextBox>
                 </p>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
    </form>
</body>
</html>
