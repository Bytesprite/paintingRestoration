using System;
using System.Net.Sockets;
using System.Drawing;
using System.IO;

namespace Client
{
   class Client
   {
      const int maxByteArray = 102400; 
      const string serverIPAddress = "ggj.atetkao.com"; const int serverPort = 11000; //const string serverIPAddress = "127.0.0.1"; const int serverPort = 11000;
      string byteArrayLength = null;
      // static void Main(string[] args)
      // {
      //    Send(".\\images\\BENCHMARK_CLIENT.jpg");
      //    Read(".\\images\\BENCHMARK_SERVER.jpg", "BENCHMARK_SERVER");
      // }
      static void Send(string filePathName)
      {
         // filePathName: Where do you want image to be saved, remember Windows filepath slashes:  ".\\images\\1.jpg"
         TcpClient client = new TcpClient(serverIPAddress, serverPort);
         NetworkStream stream = client.GetStream();
         try
         {
            SendReadOnStreamString(stream, "<SEND>", maxByteArray);
            SendReadOnStreamImage(stream, filePathName);
            stream.Close(); client.Close(); System.Console.WriteLine(TimeStamp() + " | Gracefully closed connection.");
         }
         catch
         {
            stream.Close(); client.Close(); System.Console.WriteLine(TimeStamp() + " | Forcibly closed connection!");
         }
      }
      static void Read(string filePathName, string levelID = "BENCHMARK_SERVER") 
      { 
         // filePathName: Where do you want image to be saved, remember Windows filepath slashes:  ".\\images\\1.jpg"
         // levelID: BENCHMARK returns an image from server, use this to send levelID to retreive appropriate images
         
         TcpClient client = new TcpClient(serverIPAddress, serverPort);
         NetworkStream stream = client.GetStream();
         try
         {
            SendReadOnStreamString(stream, "<READ>", maxByteArray);
            SendReadOnStreamString(stream, levelID, maxByteArray);
            ReadSendOnStreamImage(stream, filePathName, maxByteArray);
            stream.Close(); client.Close(); System.Console.WriteLine(TimeStamp() + " | Gracefully closed connection.");
         }
         catch
         {
            stream.Close(); client.Close(); System.Console.WriteLine(TimeStamp() + " | Forcibly closed connection!");
         }
      }
      static void SendReadOnStreamString(NetworkStream stream, string clientMessageString, int byteArraySize)
      {
         // Send to server
         WriteStreamString(stream, clientMessageString);
         System.Console.WriteLine(TimeStamp() + " | Sent: " + clientMessageString);
         // Received from server
         string serverMessageString = ReadStreamString(stream, byteArraySize);
         System.Console.WriteLine(serverMessageString);
      }
      static void ReadSendOnStreamImage(NetworkStream stream, string filePathName, int byteArraySize)
      {
         // Must write
         WriteStreamString(stream, "<GO>");
         // Received from server
         Byte[] byteArray = new Byte[byteArraySize];
         int bytes = stream.Read(byteArray, 0, byteArray.Length);
         MemoryStream ms = new MemoryStream(byteArray);
         Image serverImage = Image.FromStream(ms);
         // Save out
         serverImage.Save(filePathName);
         // Send back to server
         string clientMessageString = TimeStamp() + " | Received image save as: " + filePathName;
         WriteStreamString(stream, clientMessageString);
         Console.WriteLine(clientMessageString);
      }
      static void SendReadOnStreamImage(NetworkStream stream, string filePath)
      {
         // Read in image locally
         MemoryStream ms = new MemoryStream();
         Image.FromFile(filePath).Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
         Byte[] imageByteArray = ms.ToArray();
         // Send to server
         WriteStreamByteArray(stream, imageByteArray);
         System.Console.WriteLine(TimeStamp() + " | Sent: " + filePath);
         // Received from server
         string serverMessageString = ReadStreamString(stream, maxByteArray);
         System.Console.WriteLine(serverMessageString);
      }
      static void ReadSendOnStreamString(NetworkStream stream, int byteArraySize)
      {
         // Received from server
         string serverMessageString = ReadStreamString(stream, byteArraySize);
         // Send back to server
         string clientMessageString = TimeStamp() + " | Received: " + serverMessageString;
         WriteStreamString(stream, clientMessageString);
         Console.WriteLine(clientMessageString);
      }
      static string ReadStreamString(NetworkStream stream, int byteArraySize)
      {
         Byte[] byteArray = new Byte[byteArraySize];
         int bytes = stream.Read(byteArray, 0, byteArray.Length);
         return System.Text.Encoding.ASCII.GetString(byteArray, 0, bytes);
      }
      static byte[] ReadStreamByteArray(NetworkStream stream, int byteArraySize)
      {
         Byte[] byteArray = new Byte[byteArraySize];
         int bytes = stream.Read(byteArray, 0, byteArray.Length);
         return byteArray;
      }
      static void WriteStreamString(NetworkStream stream, string message)
      {
         Byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(message);
         stream.Write(byteArray, 0, byteArray.Length);
      }
      static void WriteStreamByteArray(NetworkStream stream, byte[] byteArray)
      {
         stream.Write(byteArray, 0, byteArray.Length);
      }
      public static string TimeStamp()
      {
         return DateTime.Now.ToString("yyyyMMddHHmmssffff");
      }
   }
}
