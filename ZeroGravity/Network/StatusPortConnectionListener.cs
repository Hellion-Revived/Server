using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ZeroGravity.Network;

public class StatusPortConnectionListener
{
	private volatile bool runThread;

	private Thread listeningThread;

	private TcpListener tcpListener;

	public void Stop()
	{
		runThread = false;
		tcpListener.Stop();
	}

	public void Start(int port)
	{
		runThread = true;
		tcpListener = new TcpListener(new IPEndPoint(IPAddress.Any, port));
		tcpListener.Start();
		listeningThread = new Thread(Listen);
		listeningThread.IsBackground = true;
		listeningThread.Priority = ThreadPriority.AboveNormal;
		listeningThread.Start();
	}

	private void Listen()
	{
		while (Server.IsRunning && runThread)
		{
			try
			{
				Socket soc = tcpListener.AcceptSocket();
				if (!runThread)
				{
					break;
				}
				StatusPortThread connection = new StatusPortThread(soc);
				connection.Start();
			}
			catch (Exception)
			{
			}
		}
	}
}
