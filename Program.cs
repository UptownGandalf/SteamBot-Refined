using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamKit2;

namespace SteamBot2
{
    class Program
    {
        static SteamClient steamClient;
        static CallbackManager manager;
        static SteamUser steamUser;
        static bool isRunning;
        static string username, password;


        static void Main(string[] args)
        {
            steamClient = new SteamClient();
            manager = new CallbackManager(steamClient);
            steamUser = steamClient.GetHandler<SteamUser>();
            Console.Title = "Gandalf Steam Bot";
            Console.WriteLine("GANDALF STEAM BOT 1.0");
            Console.WriteLine("***************************");
            Console.WriteLine("Please enter your username");
            username = Console.ReadLine();
            Console.WriteLine("Please enter your password.");
            password = Console.ReadLine();
            manager.Subscribe<SteamClient.ConnectedCallback>(OnConnected);
            manager.Subscribe<SteamClient.DisconnectedCallback>(OnDisconnected);
            manager.Subscribe<SteamUser.LoggedOnCallback>(OnLoggedOn);
            manager.Subscribe<SteamUser.LoggedOffCallback>(OnLoggedOff);
            isRunning = true;
            steamClient.Connect();
            while (isRunning)
            {
                manager.RunWaitCallbacks(TimeSpan.FromSeconds(1));
            }




        }



        static void OnConnected(SteamClient.ConnectedCallback callback)
        {

            Console.WriteLine("Connected to Steam! Logging in...");
            steamUser.LogOn(new SteamUser.LogOnDetails { Username = username, Password = password });


        }

        static void OnDisconnected(SteamClient.DisconnectedCallback callback)
        {

            Console.WriteLine("Unable to connect to Steam.");
            isRunning = false;
            Console.ReadLine();


        }
        static void OnLoggedOn(SteamUser.LoggedOnCallback callback)
        {

            if(callback.Result != EResult.OK)
            {

                if(callback.Result == EResult.AccountLogonDenied)
                {

                    Console.WriteLine("Steam Guard protected");
                    isRunning = false;
                    Console.ReadLine();

                }

                Console.WriteLine("Login failed");
                isRunning = false;
                Console.ReadLine();
                

            }
            else
            {

                Console.WriteLine("Login successful");
                steamUser.LogOff();
                Console.ReadLine();

            }


        }

        static void OnLoggedOff(SteamUser.LoggedOffCallback callback)
        {

            Console.WriteLine("Logged off:" + callback.Result);

        }

    }
}
