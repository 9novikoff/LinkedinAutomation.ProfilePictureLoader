namespace LinkedinAutomation;

public static class Utils
{
    public static void AddOrUpdateAppSetting<T>(string key, T value) 
    {
        try 
        {
            var filePath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName, "appsettings.json");
            string json = File.ReadAllText(filePath);
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json) ?? throw new InvalidOperationException();
                
            var sectionPath = key.Split(":")[0];

            if (!string.IsNullOrEmpty(sectionPath)) 
            {
                var keyPath = key.Split(":")[1];
                jsonObj[sectionPath][keyPath] = value;
            }
            else 
            {
                jsonObj[sectionPath] = value;
            }

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(filePath, output);
        }
        catch (Exception) 
        {
            Console.WriteLine("Error writing app settings");
        }
    }
    
}