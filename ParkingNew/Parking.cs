using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParkingNew
{
    public partial class Parking : Form
    {
        FirebaseClient client = new FirebaseClient("https://heymotocarro-1a1d4.firebaseio.com/");
        Graphics G;
        Beacons beacons;
        Sensors sensors;
        Slots slots;
        Slot slot;
        Rectangle[] rect = new Rectangle[13];
        int[] map = new int[2];
        

        public Parking()
        {
            InitializeComponent();
        }

        private void Parking_Load(object sender, EventArgs e)
        {
            
            G = this.CreateGraphics();
            sensors = new Sensors(4);
          
            // Initialization for sensors, can be a module
            sensors.data[0].setCordinates(0, 5);
            sensors.data[1].setCordinates(9, 5);
            sensors.data[2].setCordinates(0, 0);
            sensors.data[3].setCordinates(9, 0);
            map[0] = 0;
            map[1] = 0;


        }

        private void loadBtn_Click(object sender, EventArgs e)
        {
            slots = new Slots(12, G);
            getPopulationAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          


        }

        private async void getPopulationAsync() // grabs population from database 
        {
            //******************** Get initial list of beacons ***********************//
            beacons = await client
               .Child("Beacons/")//Prospect list
               .OnceSingleAsync<Beacons>();
            //getBeacons(BeaconsSet);

            //******************** Get changes on beacons ***********************//
            onChildChanged();
        }

        private static async Task sendData(string key)
        {
            FormUrlEncodedContent content;
            HttpResponseMessage response;
            HttpClient httpclient = new HttpClient();
            string responseString;
            int companyId = 1;


            var dictionary = new Dictionary<string, string>{
                            { "key",key},
               
                            { "companyId",companyId.ToString()}
                        };

            content = new FormUrlEncodedContent(dictionary);
            response = await httpclient.PostAsync("https://us-central1-heymotocarro-1a1d4.cloudfunctions.net/sendData", content);
            responseString = await response.Content.ReadAsStringAsync();
            Response data = Newtonsoft.Json.JsonConvert.DeserializeObject<Response>(responseString);
            //Response message
            Console.WriteLine($"Key:{data.key}");
            Console.WriteLine($"ID:{data.companyId}");

        }
        private void onChildChanged()
        { 
            var child = client.Child("Beacons/data");
            var observable = child.AsObservable<Beacon>();
            var subscription = observable
                .Subscribe(x =>
                {
                    int key = Int32.Parse(x.Key);
                    beacons.data[key].update(x.Object);
                    Point p = beacons.data[key].getXY(sensors);

                    Console.WriteLine($"x is: {p.x}");
                    Console.WriteLine($"y is: {p.y}");
                    Console.WriteLine($"This spot is taken:{p.z}");
                   
                    map[key] = p.z+1;

                    if (map[key] != p.z)
                    {
                        slots.data[p.z].ColorB(G);
                        slots.data[p.z].ColorR(G);
                        map[key] = p.z + 1;
                    }
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(map, Formatting.Indented);
                  
                    sendData(json);


                    Console.WriteLine($"the Json:{json}");

                    

                });

        }

    }
}
