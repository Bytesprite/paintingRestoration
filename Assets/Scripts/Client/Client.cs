using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Client
{
   public class Client
   {
      public static void Send(string filePathName, string serverHostName = "ggj.atetkao.com", int serverPort = 11000)
      {
         // Connect to server
         IPAddress serverIPAddress = Dns.GetHostEntry(serverHostName).AddressList[0];
         IPEndPoint serverEndPoint = new IPEndPoint(serverIPAddress, serverPort);
         Socket serverSocket = new Socket(serverIPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
         serverSocket.Connect(serverEndPoint);
         try
         {
            // 1. Send clientID
            SendReadString(serverSocket, GetClientID());

            // 2. Send fileByteArray
            SendReadFile(serverSocket, filePathName);

            // Release the socket
            serverSocket.Shutdown(SocketShutdown.Both); serverSocket.Close(); System.Console.WriteLine(TimeStamp() + " | Gracefully closed connection.");
         }
         catch
         {
            // Release the socket
            serverSocket.Shutdown(SocketShutdown.Both); serverSocket.Close(); System.Console.WriteLine(TimeStamp() + " | Forcibly closed connection!");
         }
      }
      public static void SendReadString(Socket serverSocket, string clientMessage, int maxByteLength = 1024)
      {
         byte[] serverMessageByteArray = new byte[maxByteLength];
         serverSocket.Send(System.Text.Encoding.ASCII.GetBytes(clientMessage));
         int byteLength = serverSocket.Receive(serverMessageByteArray);
         System.Console.WriteLine(System.Text.Encoding.ASCII.GetString(serverMessageByteArray, 0, byteLength));
      }
      public static void SendReadFile(Socket serverSocket, string filePath, int maxByteLength = 1024)
      {
         byte[] imageByteArray = System.IO.File.ReadAllBytes(filePath);
         // Determine file's byte length
         SendReadString(serverSocket, imageByteArray.Length.ToString());
         // Send file
         serverSocket.Send(imageByteArray);
         // Received confirmation
         byte[] serverMessageByteArray = new byte[maxByteLength];
         int byteLength = serverSocket.Receive(serverMessageByteArray);
         System.Console.WriteLine(System.Text.Encoding.ASCII.GetString(serverMessageByteArray, 0, byteLength));
      }

      // ### HELPER FUNCTIONS ###
      public static string TimeStamp()
      {
         return DateTime.Now.ToString("yyyyMMddHHmmssffff");
      }
      public static string GetClientID()
      {
         string hostName = Dns.GetHostName(); // Retrive the Name of HOST    
         string myIP = Dns.GetHostAddresses(hostName)[0].ToString();

         string macAddress = "";
         foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
         {
            if (nic.OperationalStatus == OperationalStatus.Up)
            {
               macAddress += nic.GetPhysicalAddress().ToString();
               break;
            }
         }
         string clientID = Regex.Replace(macAddress + "_" + myIP, @"[^A-Za-z0-9_]+", "");
         return clientID;
      }
   }
}
