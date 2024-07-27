using System.Net.Sockets;
using System.Net;
using System.Reflection;
using System.Windows.Forms;

namespace BroLabz
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var port = GetFreePort();

            var server = new Server();
            Task.Run(async () => await  server.StartAsync(port));

            // Initialize the Windows Forms Application
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            //ApplicationConfiguration.Initialize();

            var mainWindow = new MainWindow(port);
            mainWindow.FormClosing += async (sender, e) => await server.StopAsync();

            Application.Run(mainWindow);
            
        }

        public static int GetFreePort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }

    }

}