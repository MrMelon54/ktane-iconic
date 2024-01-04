using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

public class Startup {
    public static Dictionary<string, RepoModule> Modules = new Dictionary<string, RepoModule>();
    /*private static Texture2D[,] icons;
    private static string mainpath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "Assets"), "repo.json");
    private static string resourcesPath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "Assets"), "Resources");*/

    [InitializeOnLoadMethod]
    private static void LoadModules()
    {
        var http = SendRequest("https://ktane.timwi.de/json/raw", "information");
        JArray allModules;
        // This is based on the number of modules per row on the repo.
        // At the time of writing this it's set to 47, but I'm not hardcoding it just in case.
        //int cols = 0;
        if ((allModules = JObject.Parse(http.downloadHandler.text)["KtaneModules"] as JArray) == null)
            Debug.LogError("Website did not respond with a JSON array at \"KtaneModules\" key.");
        else
        {
            Debug.Log("Information retrieved from the repo. Currently loading modules...");
            foreach (JObject module in allModules)
            {
                var name = module["Name"] as JValue;
                var displayName = module["DisplayName"] as JValue;
                var filename = module["FileName"] as JValue;
                var sortkey = module["SortKey"] as JValue;
                var uploadDate = module["Published"] as JValue;
                var x = module["X"] as JValue;
                var y = module["Y"] as JValue;
                if (name.Value is string && x.Value is long && y.Value is long)
                {
                    long w = x.ToObject<long>();
                    // While parsing each width, find the largest width and use that as the number of columns.
                    //if (cols < w)
                        //cols = System.Convert.ToInt32(w);
                    RepoModule processedModule = new RepoModule(name.Value, displayName == null ? null : displayName.Value, filename == null ? name.Value : filename.Value, sortkey == null ? Regex.Replace(name.Value.ToString().ToUpperInvariant(), "^THE ", "").Where(ch => (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9')).Join("") : sortkey.Value, uploadDate == null ? null : uploadDate.Value, x.ToObject<long>(), y.ToObject<long>());
                    if (!Modules.ContainsKey(processedModule.filename))
                        Modules.Add(processedModule.filename, processedModule);
                }
            }
            // Increase cols by 1 to account for index 0.
            //cols++;
            //File.WriteAllText(mainpath, JsonConvert.SerializeObject(loadedModules, Formatting.Indented));
        }
        if (Modules.Count == 0)
        {
            Debug.LogError("Repo information is not available. Please start or refresh Unity while connected to the internet at least once.");
            return;
        }
        //var processedModules = JsonConvert.DeserializeObject<Dictionary<string, RepoModule>>(File.ReadAllText(mainpath)).Values.OrderBy(x => x.uploadDate);
        //modules = processedModules.Select(x => x.iconName).ToList();
        //displayNames = processedModules.ToDictionary(x => x.iconName, x => x.name);
        Debug.Log("Module loading complete.");
        /*var time = System.DateTime.Now;
        while ((System.DateTime.Now - time).TotalSeconds < 10);
        time = System.DateTime.Now;
        http = SendRequest("https://ktane.timwi.de/iconsprite", "icon sheet");
        var p = http.downloadHandler.data;
        if (p != null)
        {
            if (cols == 0)
            {
                Debug.Log("Panic");
                return;
            }
            File.WriteAllBytes(Path.Combine(resourcesPath, "RepoSheet.png"), p);
            icons = new Texture2D[p.Length / cols / 1024, cols];
            for (int i = 0; i < p.Length / 1024 / cols; i++)
            {
                Texture2D row = new Texture2D(2, 2);
                var data = p.Skip(i * 1024 * cols).Take(1024 * cols).ToArray();
                row.LoadImage(data);
                File.WriteAllBytes("Uh.png", row.EncodeToPNG());
                File.WriteAllBytes("uh2.png", data);
                Debug.Log(cols);
                Debug.Log(i);
                Debug.Log(data.Length);
                Debug.Log(row.width + "+" + row.height);
                for (int j = 0; j < cols; j++)
                    icons[i, j] = Sprite.Create(row, new Rect(j * 32, 0, 32, 32), new Vector2(0.5f, 0.5f)).texture;
                if ((System.DateTime.Now - time).TotalSeconds > 20)
                {
                    Debug.Log(i);
                    return;
                }
            }
            File.WriteAllBytes("Test1.png", icons[0,0].EncodeToPNG());
            File.WriteAllBytes("Test2.png", icons[0,1].EncodeToPNG());
            File.WriteAllBytes("Test3.png", icons[1,0].EncodeToPNG());
        }
        else
            Debug.Log("Failed to download spritesheet.");*/
    }

    static UnityWebRequest SendRequest(string url, string log)
    {
        Debug.LogFormat("Attempting to retrieve {0} from the repo...", log);
        var http = UnityWebRequest.Get(url);
        http.SendWebRequest();
        var time = System.DateTime.UtcNow;
        while (!http.isDone && !http.isHttpError && !http.isNetworkError)
        {
            var t = System.DateTime.UtcNow - time;
            if (t.TotalSeconds > 20.0)
            {
                Debug.LogError("Took more than 20 seconds to retrieve repo info.");
                break;
            }
        }
        Debug.Log((System.DateTime.UtcNow - time).TotalSeconds);
        if (http.isNetworkError)
            Debug.LogErrorFormat("Unable to connect to the website, {0}", http.error);
        else if (http.responseCode != 200)
            Debug.LogErrorFormat("Website responded with code: {0}", http.responseCode);
        return http;
    }
    public class RepoModule
    {
        public string name;
        public string displayName;
        public string filename;
        public string sortkey;
        public string uploadDate;
        //public string iconName;
        // Newtonsoft.Json.Linq.JValue returns numbers as Int64, can't be bothered to force it as int since they're just coordinates.
        public long X;
        public long Y;
        public RepoModule(object Name, object DisplayName, object Filename, object Sortkey, object UploadDate, long x, long y)
        {
            name = Name as string;
            displayName = DisplayName as string;
            filename = Filename as string;
            sortkey = Sortkey as string;
            uploadDate = UploadDate as string;
            X = x;
            Y = y;
        }

        /*public bool Contains(IEnumerable<string> names)
        {
            return names.Contains(iconName = name) || 
                names.Contains(iconName = displayName) || 
                names.Contains(iconName = filename);
        }*/
    }
}
