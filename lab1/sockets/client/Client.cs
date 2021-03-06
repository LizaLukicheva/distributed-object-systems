﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace client
{
    class Expression
    {
        private String expression;

        // Expression factory method. If string can be an expression,
        // then create it. Otherwise return null.
        public static Expression createExpression(String expression)
        {
            expression.Trim(' ');
            if (checkExpression(expression))
            {
                return new Expression(expression);
            }
            else
            {
                System.Console.WriteLine("Wrong expression.");
                return null;
            }
        }

        static private Boolean isOpenBracket(char c)
        {
            return (c == '(');
        }

        static private Boolean isClosedBracket(char c)
        {
            return (c == ')');
        }

        static private Boolean isDigit(char c)
        {
            return (c >= '0' && c<='9');
        }

        static private Boolean isSign(char c)
        {
            return (c == '+' || c == '-' || c == '*' || c == '/');
        }

        private static Boolean checkExpression(String expression)
        {
            // Check digits expression
            String withoutDigits = expression.Trim('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');
            if (withoutDigits == expression)
                return false;


            // Open/close bracket 

            int i = 0;
            foreach (char c in expression)
            {

                if (isOpenBracket(c))
                {
                    i++;
                }
                else if (isClosedBracket(c))
                {
                    i--;
                }
                else if (!(isDigit(c) || isSign(c)))
                    return false;

                if (i < 0)
                {
                    return false;
                }
            }

            if (i != 0) 
            {
                return false;
            }

            // Check order

            if (expression.Length == 0 ||  isSign(expression[0]))
            {
                return false;
            }

            for (i = 1; i < expression.Length; i++)
            {
                char current = expression[i], prev = expression[i - 1];

                if ((isSign(current) && (isSign(prev) || isOpenBracket(prev)))
                    || (isOpenBracket(current) && isDigit(prev))
                    || (isClosedBracket(current) && isSign(prev))
                    || (isDigit(current) && isClosedBracket(prev)))
                {
                    return false;
                }
            }

            return true;
        }

        private Expression(String expression)
        {
            this.expression = expression;
        }

        public String getExpession()
        {
            return expression;
        }
    }

    class Client
    {
        private String name;
        private TcpClient client;

        public Client(String name)
        {
            this.name = name;
        }

        public Boolean connectToServer(Int32 port, String server)
        {
            try
            {
                client = new TcpClient(server, port);
            }
            catch (SocketException e)
            {
                System.Console.WriteLine("Error on server connection: ", e.Data);
                return false;
            }

            return true;
        }

        public void closeConnection()
        {
            client.Close();
        }

        public Boolean sendString(String expression, ref int result)
        {
            NetworkStream stream = null;
            try
            {
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(expression);

                // Get a clnt stream for reading and writing.
                stream = client.GetStream();

                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);

                // Recieve TcpServer.response

                // Buffer to store the response bytes
                data = new Byte[246];

                // Read the first bartch of the TcpServer response bytes
                Int32 bytes = stream.Read(data, 0, data.Length);
                String responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                result = Convert.ToInt32(responseData);
            }
            catch (ArgumentNullException e)
            {
                System.Console.WriteLine("ArgumentNullException: ", e);
                return false;
            }
            catch (SocketException e)
            {
                System.Console.WriteLine("SocketException: ", e);
                return false;
            }
            finally
            {
                stream.Close();
            }

            return true;
        }
    }

    class StringSender
    {
        static private Client client;
        static private Expression expression;

        static private void readExpression()
        {
            System.Console.Write("Write your expression: ");
            String str = null;
            do
            {
                str = System.Console.ReadLine();
            } while ((expression = Expression.createExpression(str)) == null);
        }

        static private void connectToServer()
        {
            Boolean isConnected = false;
            do
            {
                String port = "13000";
                String address = "127.0.0.1";
                isConnected = client.connectToServer(Convert.ToInt32(port), address);
                if (!isConnected)
                {
                    System.Threading.Thread.Sleep(5000);
                }
            } while (!isConnected);
        }

        static private void sendString()
        {
            int res = 0;
            if (client.sendString(expression.getExpession(), ref res))
            {
                System.Console.Write("Answer: ");
                System.Console.WriteLine(res);
            }
            else
            {
                System.Console.WriteLine("Some error on string sending occured.");
            }
            client.closeConnection();
        }

        static void Main(string[] args)
        {
            String newClientName = "client";
            client = new Client(newClientName);

            while (true)
            {
                // Read and check expression.
                readExpression();

                // Trying to connect to server. If cannot wait fo 45 seconds then try again.
                connectToServer();

                // Send string to server and get answer.
                sendString();
            }
        }
    }
}
