using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Windows.Navigation;

namespace PlanSwiftApi.Helpers
{
    public class JsonManager
    {

        public string GetConfigs()
        {
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "Config.json");

            if (!File.Exists(jsonPath)) 
            {
                
                return null;
            }
            else
            {
                string json = File.ReadAllText(jsonPath);

                var config = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>> (json);

                if (config != null &&
                    config.TryGetValue("Configs", out var nested) &&
                    nested.TryGetValue("Path", out var path))
                {
                    Console.WriteLine("Extraer storage path", path);
                    return path;

                }
            }

           
            return Constants.DefaultPath;
        }

    }
}
