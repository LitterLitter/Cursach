using System;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Server server = new Server();
            try
            {
                var listenThread = new Thread(server.Process);
                listenThread.Start();
            }
            catch (Exception ex)
            {
                server.Dispose();
                Console.WriteLine(ex.Message);
            }
        }
    }
}