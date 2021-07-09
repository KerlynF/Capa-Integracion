using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace IntegrationConsole
{
    class Servidor
    {
        public static int Main(string[] args)
        {
            StartServer();

            return 0;
        }


        public static void StartServer()
        {
            // obtiene la direccion ip del host para establecer la conexion
            // este ejemplo es de localhost  

            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);


            try
            {

                // Crea un Socket usando el protocolo Tcp       
                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // Asocio el socket con el end point de destino usando el metodo Bing 
                listener.Bind(localEndPoint);
               
               
                // La cantidad de request que acepta el socket a la vez antes de devolver que está ocupado 
                // (Puede estar sujeto a cambios) 
                listener.Listen(100);

                Console.WriteLine("Esperando una conexión...");
                Socket handler = listener.Accept();

                // Obteniendo la data del cliente    
                string data = null;
                byte[] bytes = null;

                while (true)
                {
                    bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);
                    
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    if (data.IndexOf("\0") > -1)
                    {
                        break;
                    }
                }

                Console.WriteLine("Información recibida : {0}", data);

                byte[] msg = Encoding.ASCII.GetBytes(data);
                handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\n Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}

