/*
 * C# Program to Establish Client Server Relationship
 */
 
//CLIENT PROGRAM
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
 
namespace MyClient4104
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
				// 1. Create an object to represent the client:
				TcpClient myClient = new TcpClient();
				
				// 2. Connect this client to a server:
				//    Note: for this client to work there must be already a server connected to 
				//    the same IP address and port combination.
				Console.WriteLine("[Client] Connecting to server.....");
				myClient.Connect("127.0.0.1", 63000);  //192.168.0.3              
				Console.WriteLine("[Client] Server is connected");
				
				// 3. Capture a message from keyboard and store it in a byte array:
				Console.Write("[Client] Enter a message for server: ");
				String aMessage = Console.ReadLine();
				ASCIIEncoding ascii = new ASCIIEncoding();
				byte[] messageBytes = ascii.GetBytes(aMessage);
				
				// 4. The GetStream method of a TcpClient object, myClient, returns a NetworkStream object,
				//    which inherits from the Stream class. NetworkStream has methods to send (using Write())
				//    and receive (using Read()) data in network communications. 
				Stream aStream = myClient.GetStream();
				Console.WriteLine("[Client] Transmitting.....");
				aStream.Write(messageBytes, 0, messageBytes.Length);		// Write() to server
				
				// 5. Stand by to listen if server send an acknowledge back:
				byte[] serverMessageBytes = new byte[500];
				int totalBytes = aStream.Read(serverMessageBytes, 0, 500);	// Read() from server
				
				// 6. Display server's message
				char aChar = new char();
				for (int i = 0; i < totalBytes; i++)
				{
					aChar = Convert.ToChar(serverMessageBytes[i]);
					Console.Write(aChar);								// Display one character to command prompt
				}
				
				// 7. Stop the client and recycle the TcpClient object:
				myClient.Close();              
                Console.WriteLine("\n\nBye!");
            }
            catch (SocketException se)
            {
                Console.WriteLine("\n[Client] Socket Error Code: " + se.ErrorCode.ToString());
				Console.WriteLine("       " + se.StackTrace);
            }
        }
    }
}
