using System;
using System.Net.Sockets;
using System.Text;

namespace SmartEL
{
    public class Client
    {
        /// <summary>
        /// 主机地址 本机为127.0.0.1
        /// </summary>
        private String _address;
        /// <summary>
        /// 端口号
        /// </summary>
        private UInt16 _uport;

        /// <summary>
        /// Client构造函数用于初始化接受者的主机地址和端口号
        /// address为主机地址 本机为127.0.0.1
        /// port为端口号
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        public Client(String address, UInt16 port)
        {
            _address = address;
            _uport = port;
        }

        /// <summary>
        /// msg发送的内容
        /// </summary>
        /// <param name="msgs"></param>
        /// <returns>true发送成功 flase发送失败</returns>
        public void Sendmessage(String msgs)
        {
//            String responseData = null;
            TcpClient client = null;
            NetworkStream stream = null;
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                client = new TcpClient(_address, _uport);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = Encoding.ASCII.GetBytes(msgs);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", msgs);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
//                data = new Byte[256];

                // String to store the response ASCII representation.
//                responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes. 
//                Int32 bytes = stream.Read(data, 0, data.Length);
//                responseData = Encoding.ASCII.GetString(data, 0, bytes);
//                Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
                stream.Close();
                client.Close();
                throw;
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                throw;
            }
        }


    }
}
