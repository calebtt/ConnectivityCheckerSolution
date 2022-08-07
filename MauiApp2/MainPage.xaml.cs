namespace MauiApp2;
using System.Net.Sockets;
using System.Net;

public partial class MainPage : ContentPage
{
    System.Net.NetworkInformation.Ping m_pingObj = new();

    public MainPage()
	{
		InitializeComponent();
        m_pingObj.PingCompleted += Pp_PingCompleted;
    }

	private void btnGetIpOfRemote_Clicked(object sender, EventArgs e)
	{
		try
		{
			// Perform DNS lookup to gather IP for remote host.
			IPHostEntry hostEntry = Dns.GetHostEntry(entRemoteUrl.Text);
			// Update label with result.
			lblResolvedHost.Text = hostEntry.AddressList.FirstOrDefault().ToString();
			// Get ping to remote host.
			m_pingObj.SendAsync(hostEntry.AddressList.FirstOrDefault().ToString(), null);
		}
		catch(Exception ex)
		{
            m_pingObj.SendAsyncCancel();
            lblResolvedHost.Text = ex.Message;
			lblPingToRemote.Text = ex.Message;
		}
    }

	private void Pp_PingCompleted(object sender, System.Net.NetworkInformation.PingCompletedEventArgs e)
	{
		string GetPingMessage(string info, bool isGood = true)
		{
			string builtMessage = "Ping: " + info;
			if (isGood)
				builtMessage += " milliseconds.";
			return builtMessage;
		}
		void SetPingMessage(string info, bool isGood = true)
		{
			lblPingToRemote.Text = GetPingMessage(info, isGood);
		}
		if (e.Cancelled)
		{
			return;
		}
		if(e.Error != null)
		{
			SetPingMessage(e.Error.ToString(), false);
			return;
		}
        if (e.Reply != null)
        {
			if (e.Reply.Status == System.Net.NetworkInformation.IPStatus.Success)
			{
				SetPingMessage(e.Reply.RoundtripTime.ToString());
				return;
            }
			SetPingMessage(e.Reply.Status.ToString(), false);
			return;
        }

	}
}

