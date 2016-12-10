using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using System.IO;
using VkNet.Enums.Filters;
using System.ComponentModel;
using System.Threading;
using VkNet.Model.RequestParams;

namespace SyDrome
{
    class Program
    {
        static string configName = "config.conf";
        VkApi vk = new VkApi();
        static bool work = false;

        static void Main(string[] args)
        {

            if (!File.Exists(configName) || (File.ReadAllText(configName).Equals("")))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Заполни config.conf");
                File.Create(configName);

                Console.Read();
                Environment.Exit(0);
            }
            
            string[] config = File.ReadAllText(configName).Split('|');
            try
            {
                var vk = new VkApi();
                vk.Authorize(new ApiAuthParams
                {
                    ApplicationId = 5741211,   // ID приложения
                    Login = config[0],         // email или телефон
                    Password = config[1],      // пароль для авторизации
                    Settings = Settings.Friends// Приложение имеет доступ к друзьям
                });
            

            TimerCallback tc = new TimerCallback(GetDataFormVk);
            Timer timer = new Timer(tc,vk,1000,int.Parse(config[2]));
            }
            catch (Exception e)
            {
                Logger.WriteLog(e.Message, "Error");
            }
            Logger.WriteLog("Успешно вошли на сайт", "Mess");
            while (true)
            {
            if(Console.ReadLine().Equals("exit"))
            {
                    Environment.Exit(0);
            }
            }
           

        }
        static void GetDataFormVk(object sendor)
        
        {
            try
            
            {
                
                if (!work)
                {
                    work = true;
                    Logger.WriteLog("Ok", "Mess");
                    
                    
                    
                    Console.WriteLine("------------------------------------------------------------------------");
                    VkApi vk = (VkApi)sendor;
                    ProfileFields[] pf = { ProfileFields.LastName, ProfileFields.FirstName };
                    var online = vk.Friends.GetOnline(new FriendsGetOnlineParams
                    {
                        UserId = vk.UserId
                    });
                    foreach (var id in online)
                    {
                        var user = vk.Users.Get(id);
                        Console.WriteLine(user.FirstName + " " + user.LastName);
                        String[] lines = { user.FirstName + "," + user.LastName + "," + id + "," + DateTime.Now.Hour + "," + DateTime.Now.Minute + "," + DateTime.Now.Second };
                        File.AppendAllLines("result_"+ DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + ".csv",lines );
                    }
                    Console.WriteLine("------------------------------------------------------------------------");
                    work = false;
                }
            }catch(Exception e)
            {
                Logger.WriteLog(e.Message, "Error");
            }

        }

      
        
    }
}