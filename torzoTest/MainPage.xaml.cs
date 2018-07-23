using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;
using Plugin.Connectivity;
using System.Threading;


namespace torzoTest
{
    public partial class MainPage : ContentPage
    {
        CancellationTokenSource tokenSource;

        public MainPage()
        {
            InitializeComponent();
            tokenSource = new CancellationTokenSource();
           



        }

        public void Handle_Clicked(object sender, System.EventArgs e)
        {
           // await DisplayAlert("Invio Richiesta Token", "allerta", "OK");
            loadToken2();
            Console.WriteLine("inizio ----");



        }
        public async void loadToken()
        {
            if (CrossConnectivity.Current.IsConnected){
                var client = new HttpClient();
                string rest = await client.GetStringAsync("http://192.168.10.71:8081/command=get?token");
                await DisplayAlert("Messaggio Leto", rest, "OK");
            }
            else{
                await DisplayAlert("messaggio", "Connettività assente", "OK");
            }


        }

        public async void loadToken2()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                Indicator1.IsRunning = true;
                label_testo.Text = "loading......";
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri("http://192.168.10.71:8081/command=get?token");
                request.Method = HttpMethod.Get;
                request.Headers.Add("Accept", "application/json");

                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(3);
                try{
                    HttpResponseMessage reposponse = await client.SendAsync(request, tokenSource.Token);
                    Indicator1.IsRunning = false;
                    if (reposponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        HttpContent content = reposponse.Content;
                        var risposta = await content.ReadAsStringAsync();
                        label_testo.Text = risposta;
                        //await DisplayAlert("Messaggio 2",risposta.ToString(), "OK");

                    }
                }catch(Exception exception){
                    //Console.WriteLine(exception.ToString());
                    Indicator1.IsRunning = false;
                    label_testo.Text = "Controllo http";
                    await DisplayAlert("messaggio", "problemi di connessione al servert", "OK");
                }

            }
            else
            {
                await DisplayAlert("messaggio", "Connettività assente", "OK");
            }
           
        }

        async Task<int> AccessTheWebAsync()
        {
            // You need to add a reference to System.Net.Http to declare client.
            HttpClient client = new HttpClient();

            // GetStringAsync returns a Task<string>. That means that when you await the 
            // task you'll get a string (urlContents).
            Task<string> getStringTask = client.GetStringAsync("http://msdn.microsoft.com");

            // You can do work here that doesn't rely on the string from GetStringAsync.
            DoIndependentWork();

            // The await operator suspends AccessTheWebAsync. 
            //  - AccessTheWebAsync can't continue until getStringTask is complete. 
            //  - Meanwhile, control returns to the caller of AccessTheWebAsync. 
            //  - Control resumes here when getStringTask is complete.  
            //  - The await operator then retrieves the string result from getStringTask. 
            string urlContents = await getStringTask;

            // The return statement specifies an integer result. 
            // Any methods that are awaiting AccessTheWebAsync retrieve the length value. 
            return urlContents.Length;
        }

        public void DoIndependentWork(){
            
        }
    }
}
