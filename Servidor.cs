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
            IPAddress coreIpdress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
            IPEndPoint clientLocalEndPoint = new IPEndPoint(ipAddress, 11009);
            IPEndPoint coreEndPoint = new IPEndPoint(coreIpdress, 11015);

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
                Console.WriteLine(handler.RemoteEndPoint);
                // Obteniendo la data del cliente    
                string data = null;
                byte[] bytes = null;
                //char[] mesagges = null;
                string[] stringMessages = null;
                int iterador = 0;
                while (true)
                {
                    bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);

                     data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    //mesagges = Encoding.ASCII.GetChars(bytes,0,bytesRec);

                    stringMessages = new string[5];

                    //foreach (char x in mesagges)
                    //{
                    //    data += x;
                    //}

                    foreach(char i in data)
                    {
                        if (i == '|')
                        {
                            iterador++;
                            
                        }
                        else
                        {
                            stringMessages[iterador] += i.ToString();
                        }

                        
                        
                    }

                    if (data.IndexOf("\0") > -1)
                    {
                        break;
                    }


                   
                }

                //Console.WriteLine("Información recibida : {0}", data);
                foreach(string x in stringMessages)
                {
                    Console.WriteLine(x);
                }
                // Formato del stream de informacion [numero de la funcionalidad]| [informacion con respecto a la funcionalidad]|[Resto del mensaje]
                byte[] streamDatos = new byte[1024];
                byte[] backStreamDatos = new byte[1024];
                streamDatos = Encoding.ASCII.GetBytes(data);
                Socket clientServer = new Socket(coreIpdress.AddressFamily,SocketType.Stream ,ProtocolType.Tcp );
                clientServer.Bind(clientLocalEndPoint);
                clientServer.Connect(coreEndPoint);
                clientServer.Send(streamDatos);
                clientServer.Receive(backStreamDatos);
                //Mensaje devuelta al cliente (cambiar)
                //aqui se debe actualizar la devuelta al cliente en base al numero de opcion y debe devolver un error en caso de que se elija
                // una opcion y no se presenten los datos necesarios para completar la solicitud 
                handler.Send(backStreamDatos);
                
                //byte[] msg = Encoding.ASCII.GetBytes(data);
                //handler.Send(msg);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine( "Error de conexión" + e.ToString());
                
                
            }

            
            Console.WriteLine("\n Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }


}

