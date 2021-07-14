using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace IntegrationServer
{
    class Cliente
    {
        public static int Main(string[] args)
        {
            StartClient();
            //connection.connectionTest();
            return 0;
        }


        public static void StartClient()
        {
            byte[] bytes = new byte[1024];

            try
            {
                // Se conecta a un server remoto  
                // obtiene la direccion ip del host para establecer la conexion
                // este ejemplo es de localhost 
                
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);
                IPAddress ipAddress1 = host.AddressList[0];
                IPEndPoint remoteEP1 = new IPEndPoint(ipAddress1, 11001);

                // Crea un TCP/IP socket.    
                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);


                sender.Bind(remoteEP1);
                // Usando try catch para conectarse al end point y capturar cualquier error.    
                try
                {
                    // conectándose al end point remoto  
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket conectado a la IP: {0}",
                        sender.RemoteEndPoint.ToString());

                    // codifica la data para enviar    
                    byte[] msg = Encoding.ASCII.GetBytes("8|Kerlyn:12345|stream|stream2|stream3\0");

                    // envía la data     
                    int bytesSent = sender.Send(msg);

                    // Recibe la respuesta del servidor    
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("Echoed test = {0}",
                        Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    // suelta el socket.    
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }

    class connection
    {
        public static DateTime issueDate;
        public static String pacientID;
        public static String operationType;
        public static int monetaryAmount;

        static byte[] bytes = new byte[1024];


        public connection(string _pacientID, string _operationType, int _monetaryAmount)
        {
            issueDate = DateTime.Now;
            pacientID = _pacientID;
            operationType = _operationType;
            monetaryAmount = _monetaryAmount;
        }

        public static void connectionTest()
        {
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

            Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

            byte[] ToEncode = Encoding.ASCII.GetBytes("Suck my dick.. \0"); 

            try
            {
                // conectándose al end point remoto
                sender.Connect(remoteEP);

                Console.WriteLine("Socket conectado a la IP: {0}",
                    sender.RemoteEndPoint.ToString());

                // codifica la data para enviar
                //byte[] msg = Encoding.ASCII.GetBytes("Probando...\0");

                // envía la data
                int bytesSent = sender.Send(ToEncode);

                // Recibe la respuesta del servidor
                int bytesRec = sender.Receive(bytes);
                Console.WriteLine("Echoed test = {0}",
                    Encoding.ASCII.GetString(bytes, 0, bytesRec));

                // suelta el socket.
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();

            }
            catch
            {

            }


        }
    }
}
