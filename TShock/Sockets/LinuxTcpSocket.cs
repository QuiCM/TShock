/*
TShock, a server mod for Terraria
Copyright (C) 2011-2019 Pryaxis & TShock Contributors

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Extensions.Logging;
using Steamworks;
using Terraria;
using Terraria.Localization;
using Terraria.Net;
using Terraria.Net.Sockets;

namespace TShock.Sockets
{
	public class LinuxTcpSocket : ISocket
	{
		private ILogger<LinuxTcpSocket> _logger;

		public int _messagesInQueue;

		public TcpClient _connection;

		public TcpListener? _listener;

		public SocketConnectionAccepted? _listenerCallback;

		public RemoteAddress? _remoteAddress;

		public bool _isListening;

		public int MessagesInQueue => _messagesInQueue;

		public LinuxTcpSocket(ILogger<LinuxTcpSocket> logger)
		{
			_logger = logger;
			_connection = new TcpClient();
			_connection.NoDelay = true;
		}

		public LinuxTcpSocket(TcpClient tcpClient, ILogger<LinuxTcpSocket> logger)
		{
			_logger = logger;
			_connection = tcpClient;
			_connection.NoDelay = true;

			IPEndPoint iPEndPoint = (tcpClient.Client.RemoteEndPoint as IPEndPoint) ??
			                        throw new NullReferenceException(
				                        "Failed to create LinuxTcpSocket, no valid IP endpoint found");
			_remoteAddress = new TcpAddress(iPEndPoint.Address, iPEndPoint.Port);
		}

		void ISocket.Close()
		{
			_remoteAddress = null;
			_connection.Close();
		}

		bool ISocket.IsConnected()
		{
			return _connection.Connected;
		}

		void ISocket.Connect(RemoteAddress address)
		{
			TcpAddress tcpAddress = (TcpAddress)address;
			_connection.Connect(tcpAddress.Address, tcpAddress.Port);
			_remoteAddress = address;
		}

		private void ReadCallback(IAsyncResult result)
		{
			if (result.AsyncState is not Tuple<SocketReceiveCallback, object> tuple)
			{
				return;
			}

			try
			{
				tuple.Item1(tuple.Item2, _connection.GetStream().EndRead(result));
			}
			catch (InvalidOperationException ex)
			{
				// This is common behaviour during client disconnects
				((ISocket)this).Close();
				_logger.LogDebug(ex,
					"Failed to end read on client socket - it is likely that the remote client closed the connection");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to end read on client socket");
			}
		}

		private void SendCallback(IAsyncResult result)
		{
			if (result.AsyncState is not object[] state)
			{
				return;
			}

			LegacyNetBufferPool.ReturnBuffer((byte[])state[1]);
			Tuple<SocketSendCallback, object> tuple = (Tuple<SocketSendCallback, object>)state[0];
			try
			{
				_connection.GetStream().EndWrite(result);
				tuple.Item1(tuple.Item2);
			}
			catch (Exception ex)
			{
				((ISocket)this).Close();
				_logger.LogDebug(ex,
					"Failed to end write on client socket - it is likely that the remote client closed the connection");
			}
		}

		void ISocket.SendQueuedPackets()
		{
		}

		void ISocket.AsyncSend(byte[] data, int offset, int size, SocketSendCallback callback, object state)
		{
			byte[] array = LegacyNetBufferPool.RequestBuffer(data, offset, size);
			_connection.GetStream()
				.BeginWrite(array, 0, size, SendCallback, new object[]
				{
					new Tuple<SocketSendCallback, object>(callback, state),
					array
				});
		}

		void ISocket.AsyncReceive(byte[] data, int offset, int size, SocketReceiveCallback callback, object state)
		{
			_connection.GetStream()
				.BeginRead(
					data,
					offset,
					size,
					ReadCallback,
					new Tuple<SocketReceiveCallback, object>(callback, state));
		}

		bool ISocket.IsDataAvailable()
		{
			return _connection.GetStream().DataAvailable;
		}

		RemoteAddress? ISocket.GetRemoteAddress()
		{
			return _remoteAddress;
		}

		bool ISocket.StartListening(SocketConnectionAccepted callback)
		{
			IPAddress? any = IPAddress.Any;
			if (Program.LaunchParameters.TryGetValue("-ip", out string? ipString) &&
			    !IPAddress.TryParse(ipString, out any))
			{
				any = IPAddress.Any;
			}

			_isListening = true;
			_listenerCallback = callback;
			_listener ??= new TcpListener(any, Netplay.ListenPort);

			try
			{
				_listener.Start();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to start socket listener");
				return false;
			}

			ThreadPool.QueueUserWorkItem(ListenLoop);
			return true;
		}

		void ISocket.StopListening()
		{
			_isListening = false;
		}

		private void ListenLoop(object? unused)
		{
			while (_isListening && !Netplay.Disconnect)
			{
				try
				{
					ISocket socket = new LinuxTcpSocket(_listener!.AcceptTcpClient(), _logger);
					Console.WriteLine(Language.GetTextValue("Net.ClientConnecting", socket.GetRemoteAddress()));
					_listenerCallback?.Invoke(socket);
				}
				catch (Exception)
				{
					//log error
				}
			}
			_listener?.Stop();

			// currently vanilla will stop listening when the slots are full, however it appears that this Netplay.IsListening
			// flag is still set, making the server loop believe it's still listening when it's actually not.
			// clearing this flag when we actually have stopped will allow the ServerLoop to start listening again when
			// there are enough slots available.
			Netplay.IsListening = false;
		}
	}
}
