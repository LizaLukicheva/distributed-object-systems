using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Polska;

namespace server
{
    class Calculator
    {
        public Calculator(String expression)
        {
            polishString = new PolishString(expression);
        }

        public int getResult()
        {
            return polishString.calculatePolska();
        }

        private PolishString polishString;
    }

    class Listener
    {
        private TcpListener server = null;

        Int32 port;
        IPAddress localAddr;

        List<TcpClient> clientList;

        public Listener(Int16 port, String localAddr)
        {
            this.port = port;
            this.localAddr = IPAddress.Parse(localAddr);

            clientList = new List<TcpClient>();
        }

        public void startServer()
        {
            server = new TcpListener(localAddr, port);

            server.Start();

            // Define maximum threads count
            int maxThreadCount = Environment.ProcessorCount * 4;

            // Set max count of working threads
            ThreadPool.SetMaxThreads(maxThreadCount, maxThreadCount);

            // Set min count of working threads.
            ThreadPool.SetMinThreads(2, 2);
        }

        static void getExpressionFromClient(Object stateInfo)
        {
            TcpClient client =  (TcpClient)stateInfo;

            // Buffer for reading data
            Byte[] buffer = new Byte[256];
            String data = null;

            // Get a stream object for reading and writing
            NetworkStream stream = client.GetStream();

            stream.Read(buffer, 0, buffer.Length);

            // Translate data bytes to ASCII string.
            data = System.Text.Encoding.ASCII.GetString(buffer, 0, buffer.Length);
            System.Console.WriteLine("Resieved: {0}", data);

            // Process the data sent by client (calculate polish).
            Calculator calculator = new Calculator(data);
            int resultOfExpression = calculator.getResult();
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(Convert.ToString(resultOfExpression));

            // Send back a response.
            stream.Write(msg, 0, msg.Length);
            System.Console.WriteLine("Sent: {0} ", resultOfExpression);
        }

        public void listen()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(getExpressionFromClient), 
                server.AcceptTcpClient()); 

            // Perfom a blocking call to accept requests.
            // You could also use server.AcceptSocket() here.
            //this.getExpressionFromClient(server.AcceptTcpClient());
        }

        public void endServer()
        {
            server.Stop();
        }
    }

    class Server
    {
        static void Main(string[] args)
        {
            Listener listener = null;
            try
            {
                listener = new Listener(13000, "127.0.0.1");
                listener.startServer();

                // Listening loop
                while (true)
                {
                    System.Console.WriteLine("Waiting for connection...");
                    listener.listen();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            finally
            {
                listener.endServer();
            }
        }
    }
}
