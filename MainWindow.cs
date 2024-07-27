namespace BroLabz
{
    public partial class MainWindow : Form
    {
        public int server_port;
        public string server_secret;
        public MainWindow(int port)
        {
            server_port = port;
            InitializeComponent();

            var server_host = $"localhost:{server_port}";
            this.Text = server_host;

            // Ensure the WebView2 control is initialized on the main UI thread
            this.Load += async (sender, e) =>
            {
                
                webView.CoreWebView2InitializationCompleted += (s, args) =>
                {
                    if (args.IsSuccess)
                    {
                        //MessageBox.Show($"Webview is ready to access {server_host}");
                        webView.CoreWebView2.Navigate($"http://{server_host}/");
                    }
                    else
                    {
                        MessageBox.Show($"WebView2 initialization failed: {args.InitializationException}");
                    }
                };
                await webView.EnsureCoreWebView2Async();
            };
        }

    }
}
