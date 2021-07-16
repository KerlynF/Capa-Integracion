using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using IntegrationConsole.UserDSTableAdapters;

namespace IntegrationConsole
{
    class Servidor
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
            UserTableTableAdapter adapter = new UserTableTableAdapter();
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
                    log.Info("Se ha hecho una peticion de la caja");
                    data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                    //mesagges = Encoding.ASCII.GetChars(bytes,0,bytesRec);

                    stringMessages = new string[7];

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

                    if (data.IndexOf("fin") > -1)
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
                //byte[] buff = Encoding.ASCII.GetBytes("mamaguevo");
                //handler.Send(buff);
                //Mensaje devuelta al cliente (cambiar)
                //aqui se debe actualizar la devuelta al cliente en base al numero de opcion y debe devolver un error en caso de que se elija
                // una opcion y no se presenten los datos necesarios para completar la solicitud 
                

                ConnectToCoreAndGiveAnswer(stringMessages, handler, data, coreIpdress, coreEndPoint, clientLocalEndPoint, adapter);
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


        public static void ConnectToCoreAndGiveAnswer(string[] stringMessages, Socket handler, string data, IPAddress coreIpdress, IPEndPoint coreEndPoint, IPEndPoint clientLocalEndPoint, UserTableTableAdapter adapter)
        {
            
            
                switch (stringMessages[0])
                {
                    case "1":
                        if (stringMessages.Length != 3)
                        {
                            byte[] respuesta = Encoding.ASCII.GetBytes("Datos ingresados no validos");
                            handler.Send(respuesta);
                        }
                        else
                        {
                            
                            string[] dataDevueltaArray = new string[3];
                            byte[] streamDatos = new byte[1024];
                            byte[] backStreamDatos = new byte[1024];
                            streamDatos = Encoding.ASCII.GetBytes(data);
                            try
                            {
                                Socket clientServer = new Socket(coreIpdress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                                clientServer.Bind(clientLocalEndPoint);
                                clientServer.Connect(coreEndPoint);
                                clientServer.Send(streamDatos);
                                int x = clientServer.Receive(backStreamDatos);

                                handler.Send(backStreamDatos);
                                
                                
                            }
                            catch (Exception)
                            {
                                //resolverlo yo
                                byte[] respuesta1 = Encoding.ASCII.GetBytes("Error al conectarse con el core, no se puede validar el usuario");
                                handler.Send(respuesta1);
                                log.Info("Ha ocurrido un error al intentar conectarse con el core");

                            }

                            
                        }
                        break;
                    case "2":
                        if (stringMessages.Length != 7)
                        {
                            byte[] respuesta = Encoding.ASCII.GetBytes("Datos ingresados no válidos");
                            handler.Send(respuesta);
                        }
                        else
                        {
                            string[] dataDevueltaArray = new string[7];
                            byte[] streamDatos = new byte[1024];
                            byte[] backStreamDatos = new byte[1024];
                            streamDatos = Encoding.ASCII.GetBytes(data);
                            try
                            {
                                Socket clientServer = new Socket(coreIpdress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                                clientServer.Bind(clientLocalEndPoint);
                                clientServer.Connect(coreEndPoint);
                                clientServer.Send(streamDatos);
                                int x = clientServer.Receive(backStreamDatos);

                                handler.Send(backStreamDatos);
                                
                                
                            }
                            catch (Exception)
                            {
                                //resolverlo yo
                                byte[] respuesta1 = Encoding.ASCII.GetBytes("Error al conectarse con el core, no se puede validar el usuario");
                                handler.Send(respuesta1);
                                log.Info("Ha ocurrido un error al intentar conectarse con el core");

                            }
                        }
                        break;
                }


            
            
        }
    }


}

