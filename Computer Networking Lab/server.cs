/*
 * C# Program to Establish Client Server Relationship
 */

//SERVER PROGRAM
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace MyServer4104
{
    class Program
    {
        static void Main(string[] args)
        {
			//Declare the server queue:
			TcpListener serverQueue = null;
            try
            {
				// 1. Set up an IP object to represent one IP of your computer:
                IPAddress ipAd = IPAddress.Parse("127.0.0.1"); //Or the wired or WiFi IP of your computer
				
                // 2. Create the queue to listen for and accept incoming connection requests:
				serverQueue = new TcpListener(ipAd, 63000);
				// TCP port numbers range from 0 to 65536, i.e., 16 bits.
				// DO NOT use a port number in the range from 0 to 1023. They are the well-known ports or 
				// system ports.
				// Port numbers from 1024 to 49151 are the registered ports. You may use them like 63000 
				// if they are not registered or registered but the program is not running in your computer.
				
				// Use the Start method to begin listening for incoming connection requests. 
				// It will queue incoming connections until the Stop method is called.
                serverQueue.Start();
                Console.WriteLine("[Server] The server is running at port 63000");
                Console.WriteLine("[Server] So, the server's local end point is: " + serverQueue.LocalEndpoint);
                Console.WriteLine("[Server] Waiting for a client's connection........");
                
				// 3. Pull a connection from the queue to make a socket:
				Socket aSocket = serverQueue.AcceptSocket();
                Console.WriteLine("\n[Server] Connection accepted from a client " + 
				                  "whose end point is: " + aSocket.RemoteEndPoint);
                
				// 4. Get the message sent by client through socket:
				byte[] incomingDataBuffer = new byte[1000];
				char aChar = new char();
				StringBuilder vowels = new StringBuilder(); // string to store vowels in message.
				
				// I found the documentation on the StringBuilder class here: 
				// https://docs.microsoft.com/en-us/dotnet/standard/base-types/stringbuilder
				
                int totalBytes = aSocket.Receive(incomingDataBuffer);		// Receive from client, byte by byte
				
				// 5. Display the received message:
                Console.WriteLine("[Server] Message of client recieved");
                for (int i = 0; i < totalBytes; i++)
                {
					aChar = Convert.ToChar(incomingDataBuffer[i]);
                    Console.Write(aChar);
					
					if (Char.ToUpper(aChar) == 'A'|| Char.ToUpper(aChar) == 'E' || Char.ToUpper(aChar) == 'I' || 
						Char.ToUpper(aChar) == 'O' || Char.ToUpper(aChar) == 'U') 
					{
						vowels.Append(aChar);
					}
                }
				
				// 6. Print seperate message for vowels.
				
				if (vowels.Length > 0) 
				{
					Console.WriteLine("\n[Server] Vowel characters detected = " + vowels.ToString());
				}
				
				// 6. Tell client its message was received:
                ASCIIEncoding ascii = new ASCIIEncoding();
				Console.Write("\n[Server] Hit 'Enter' to send acknowledgement to clicnt and end the session....");
				Console.ReadLine();
                aSocket.Send(ascii.GetBytes("[Server] Your message was recieved."));   // Send to client, byte by byte through socket
                
                aSocket.Close();
            }
            catch (SocketException se)
            {
                Console.WriteLine("\n[Server] Socket Error Code: " + se.ErrorCode.ToString());
				Console.WriteLine("       " + se.StackTrace);
            }
			finally
			{
				// 7. Stop listening request and recycle the queue:
				serverQueue.Stop();
				Console.WriteLine("\nBye!");
			}
        }
    }
}
