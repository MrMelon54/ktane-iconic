using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(GenerateMasterSprite))]
public class SpriteObject : Editor
{
    /*int _moduleChoiceIndex = 0;
    int section = 0;
    int range = 0;
    int iconChanged;*/
    bool hasChecked;
    int selectedIndex;
    public override void OnInspectorGUI()
    {
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RepoDir")))
            if (!SetDir(!hasChecked))
            {
                if (GUILayout.Button("Select Icon Directory"))
                    SetDir(true);
                return;
            }
        DrawDefaultInspector();
        GenerateMasterSprite generateObject = target as GenerateMasterSprite;
        GenerateMasterSprite.WorkingDirectory = Directory.GetParent(Application.dataPath).FullName;
        GenerateMasterSprite.iconsDirectory = Environment.GetEnvironmentVariable("RepoDir");

        if (GenerateMasterSprite.ModuleJsonPath == null)
            GenerateMasterSprite.ModuleJsonPath = Path.GetFullPath(Path.Combine(GenerateMasterSprite.WorkingDirectory, "Assets/Resources/iconicData.json"));
        
        switch (selectedIndex = EditorGUILayout.Popup(selectedIndex, new[] { "Iconic", "Module Maze" }))
        {
            case 1:
                GenerateMasterSprite.ModuleJsonPath = Path.Combine(Path.GetDirectoryName(GenerateMasterSprite.ModuleJsonPath), "ModuleMazeSetup.json");
                break;
            default:
                GenerateMasterSprite.ModuleJsonPath = Path.Combine(Path.GetDirectoryName(GenerateMasterSprite.ModuleJsonPath), "iconicData.json");
                break;
        }

        if (File.Exists(GenerateMasterSprite.ModuleJsonPath))
        {
            // read json file and extract module keys to ModuleList
            string lines = File.ReadAllText(GenerateMasterSprite.ModuleJsonPath);
            iconicJson.iconicData j = iconicLoader.ParseJson(lines);
            List<string> ModuleList = j.modules.Select(x => x.icon).ToList();
            generateObject.ModuleList = ModuleList;

            /*var ranges = new List<string>();
            // We want to include object 1024 in the first list.
            for (int i = 0; i < (ModuleList.Count - 1)/ 1024 + 1; i++)
            {
                ranges.Add((i * 1024 + 1) + "-" + Mathf.Min((i + 1) * 1024, ModuleList.Count));
            }
            if (ModuleList.Count > 1024)
                range = EditorGUILayout.Popup(range, ranges.ToArray());
            int sections = ModuleList.Count / 32;
            List<string> moduleSections = new List<string>();
            for (int i = 0; i <= sections; i++)
            {
                if (i * 32 == ModuleList.Count)
                    break;
                int max = i * 32 + 31;
                if (i * 32 + 31 >= ModuleList.Count)
                {
                    max = ModuleList.Count - 1;
                }
                moduleSections.Add(ModuleList[i * 32] + " - " + ModuleList[max]);
            }
            if (range * 32 + section > moduleSections.Count)
                section = 0;
            if (range * 1024 + section * 32 + _moduleChoiceIndex > ModuleList.Count)
                _moduleChoiceIndex = 0;
            section = EditorGUILayout.Popup(section, moduleSections.Skip(range * 32).Take(32).ToArray());
            _moduleChoiceIndex = EditorGUILayout.Popup(_moduleChoiceIndex, ModuleList.Skip((range * 1024) + section * 32).Take(32).ToArray());
            generateObject.ModuleIndex = range * 1024 + section * 32 + _moduleChoiceIndex;


            if (generateObject.ModuleIcon != null)
            {
                if (section * 32 + _moduleChoiceIndex != iconChanged)
                {
                    iconChanged = section * 32 + _moduleChoiceIndex;
                    generateObject.Verify();
                }
            }*/
        }

        if (GUILayout.Button("Generate Spritesheet"))
        {
            // Don't generate sprites if we don't have access to the repo.
            if (Startup.Modules == null)
                return;
            generateObject.Generate();
        }

        if (GUILayout.Button("Change Icon Directory"))
        {
            SetDir(true);
        }

        /* Verifying is no longer possible as the original icons are no longer included in the project.
        if (GUILayout.Button("Verify Sprites"))
        {
            var MasterSheets = Directory.GetFiles(GenerateMasterSprite.MasterSheetPath, "Master Sheet?.png");
            if (MasterSheets.Length == 0)
                throw new Exception("Generated master sheet is/are inaccessible, please try generating it again");
            generateObject.Verify();
        }

        if (GUILayout.Button("Destroy Clones"))
        {
            generateObject.DestroyClones();
        }*/
    }
    bool SetDir(bool firstTry)
    {
        if (!firstTry)
            return false;
        var repoDir = EditorUtility.OpenFolderPanel("Select icon directory", "KtaneContent", "Icons");
        hasChecked = true;
        if (string.IsNullOrEmpty(repoDir) || !Directory.Exists(repoDir) || Directory.GetFiles(repoDir, "*.png", SearchOption.TopDirectoryOnly).Length < 1)
        {
            Debug.LogError("Invalid icon folder chosen.");
            return false;
        }
        // Write it locally so that the original check only gets called once.
        // Write it to the machine so that when Unity restarts, it still works
        Environment.SetEnvironmentVariable("RepoDir", repoDir);
        Environment.SetEnvironmentVariable("RepoDir", repoDir, EnvironmentVariableTarget.User);
        return true;
    }
}

[CreateAssetMenu(fileName = "Generate Master Sprite", menuName = "ScriptableObjects/Sprite Generator")]
public class GenerateMasterSprite : ScriptableObject
{
    // Allow this to be customizable
    // Lower is more compatible with low end devices (such as mobile)
    // Higher is less sheets but requires more resources.
    // Make sure this value matches one of the possible values for the max size or else it will default to 4096.
    public int maxSize = 4096;
    public int cols = 21;
    public int w = 32;
    public int h = 32;
    [HideInInspector]
    public int ModuleIndex;
    [HideInInspector]
    public List<string> ModuleList;
    public static string ModuleJsonPath;
    public static string MasterSheetPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets" + Path.DirectorySeparatorChar + "Sprites");
    public static string WorkingDirectory;
    public static string iconsDirectory;
    public Texture2D ModuleIcon;
    public iconicScript ModuleScript;
    class GeneratorData
    {
        public GeneratorData(Startup.RepoModule data, FileInfo f)
        {
            repoData = data;
            file = f;
        }
        public static List<GeneratorData> files = new List<GeneratorData>();
        public FileInfo file;
        public Startup.RepoModule repoData;
        public string name;
    }
    public void Generate()
    {
        // In case there is more than one sheet, use the wildcard to match 0 or 1 times.
        var sheets = Directory.GetFiles(MasterSheetPath, "Master Sheet?.png");
        // Unity will keep the image in memory if we change it, so delete the old files
        foreach (string sheet in sheets)
        {
            File.Delete(sheet);
            File.Delete(sheet + ".meta");
        }
        var ModuleNames = ModuleList.Select(x => Startup.Modules[x]).ToList();
        // Grab all of the file paths from the icons directory
        var iconFiles = new DirectoryInfo(iconsDirectory).GetFiles("*.png", SearchOption.TopDirectoryOnly);
        GeneratorData.files = ModuleNames.Select(x => new GeneratorData(x, iconFiles.First(y => x.filename.Equals(rmExt(y.Name), StringComparison.InvariantCultureIgnoreCase)))).ToList();
        var files = GeneratorData.files;
        // Determine the number of rows based on the number of columns
        var rows = new List<int> { (files.Count + cols - 1) / cols };
        while (rows.Last() * cols > maxSize)
        {
            var count = rows.Count;
            rows[count - 1] = cols;
            rows.Add((files.Count - (count * maxSize) + cols - 1) / cols);
        }
        // Create the Texture we'll use to make the final PNG
        // The size must be predetermined according to Texture2D.SetPixels()
        var fullImages = new List<Texture2D>();
        var spriteData = new SpriteData();
        var infos = spriteData.sprites;
        for (int k = 0; k < rows.Count; k++)
        {
            fullImages.Add(new Texture2D(cols * w, rows[k] * h));
            List<IEnumerable<Color>> allColors = new List<IEnumerable<Color>>();
            for (int i = 0; i < rows[k]; i++)
            {
                // Load the pixels for each icon
                List<Color[]> eachColors = new List<Color[]>();
                FileStream fs;
                for (int j = 0; j < cols; j++)
                {
                    // Texture2D.SetPixels requires an exact size, so fill the rest of the pixels with empty
                    if (k * maxSize + i * cols + j >= files.Count)
                    {
                        eachColors.Add(Enumerable.Repeat(Color.clear, w * h * (cols - j)).ToArray());
                        continue;
                    }
                    var moduleSelected = files[k * maxSize + i * cols + j];
                    // Grab the filestream from each fileinfo, and read the filestream to a byte array
                    // Close the filestream so the file can be used by other programs if needed
                    // Translate the image into a texture so that the pixels can be loaded from it
                    // eachColors contains the pixels for each icon in a row
                    fs = moduleSelected.file.OpenRead();
                    var info = new SpriteInfo
                    {
                        name = moduleSelected.repoData.name,
                        sortkey = moduleSelected.repoData.sortkey,
                        x = j,
                        y = i,
                        sheet = k
                    };
                    infos.Add(info.name.ToLowerInvariant(), info);
                    var imageBytes = new byte[fs.Length];
                    fs.Read(imageBytes, 0, Convert.ToInt32(fs.Length));
                    fs.Close();
                    var image = new Texture2D(2, 2);
                    image.LoadImage(imageBytes);
                    eachColors.Add(image.GetPixels());
                }
                // Since the pixels are a single dimension array, there's no way to set each icon next to one another
                // As such, take each row of icons and process them from top to bottom.
                List<Color> rowColors = new List<Color>();
                for (int j = 0; j < h; j++)
                {
                    // SelectMany joins an array with multiple options into a single option
                    var eachImageRow = eachColors.Select(x => x.Skip(j * w).Take(w)).SelectMany(x => x);
                    rowColors.AddRange(eachImageRow);
                }
                // allColors will be added to the main texture, so add each row of icons to this list
                allColors.Add(rowColors);
            }
            // SetPixels apparently apply from bottom to top for whatever reason. Reverse the rows so that they're from top to bottom
            allColors.Reverse();
            fullImages[k].SetPixels(allColors.SelectMany(x => x).ToArray());
            var finalImage = fullImages[k].EncodeToPNG();
            var sheetPath = Path.Combine(MasterSheetPath, "Master Sheet" + (k + 1) + ".png");
            File.WriteAllBytes(sheetPath, finalImage);
            sheetPath = sheetPath.Replace(WorkingDirectory, "").Substring(1).Replace(Path.DirectorySeparatorChar, '/');
            Func<Texture2D> tryloadingAsset = () => AssetDatabase.LoadAssetAtPath<Texture2D>(sheetPath);
            // If we're creating a new file, let Unity know it's been created.
            // (This will also tell unity the old spreadsheet was deleted and that pixels should be reset)
            if (tryloadingAsset() == null)
                AssetDatabase.Refresh();
            var loadedAsset = tryloadingAsset();
            if (ModuleScript.Modules.Length <= k)
                ModuleScript.Modules = ModuleScript.Modules.Concat(new[] { loadedAsset }).ToArray();
            else
                ModuleScript.Modules[k] = loadedAsset;
            TextureImporter importer = AssetImporter.GetAtPath(sheetPath) as TextureImporter;
            importer.maxTextureSize = maxSize;
            importer.isReadable = true;
            importer.filterMode = FilterMode.Point;
            importer.textureType = TextureImporterType.Sprite;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            AssetDatabase.ImportAsset(sheetPath, ImportAssetOptions.ForceUpdate);
        }
        File.WriteAllText(Path.Combine(MasterSheetPath, "spriteData.json"), JsonConvert.SerializeObject(spriteData, Formatting.Indented));
        // Backwards compatibility (merging two sheets into one)
        if (ModuleScript.Modules.Length > rows.Count)
            ModuleScript.Modules = ModuleScript.Modules.Take(rows.Count).ToArray();
        // Tell Unity the old spritesheet is deleted and to reset the pixels in memory
        AssetDatabase.Refresh();
        // Sync the new data to the hierarchy
        var rootObjects = new List<GameObject>();
        var scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(rootObjects);
        var iconicPrefab = rootObjects.First(x => x.name.Contains("iconic"));
        iconicPrefab.GetComponent<iconicScript>().Modules = ModuleScript.Modules;
        // Dirty "Apply" button for updating the spritesheet in the iconic script.
        // Keep in mind that this will save all changes made to the iconic prefab.
        PrefabUtility.ReplacePrefab(iconicPrefab, AssetDatabase.LoadAssetAtPath<GameObject>("Assets/iconic.prefab"), ReplacePrefabOptions.ConnectToPrefab);
    }

    Func<string, string> rmExt = s => Path.GetFileNameWithoutExtension(s);

   /* [HideInInspector]
    public GameObject givenIcon;
    [HideInInspector]
    public GameObject generatedIcon;
    public void Verify()
    {
        if (ModuleIcon == null)
        {
            throw new Exception("Please make sure an icon has been selected.");
        }

        if (ModuleScript == null)
            throw new Exception("Script of type \"iconicScript\" necessary for verification");
        GameObject TheIcon = ModuleScript.TheIcon.gameObject;

        if (givenIcon == null)
        {
            givenIcon = Instantiate(TheIcon);
            Vector3 p = givenIcon.transform.localPosition;
            givenIcon.transform.localPosition = new Vector3(-.14f, p.y + .01f, p.z);
        }
        if (generatedIcon == null)
        {
            generatedIcon = Instantiate(TheIcon);
            Vector3 p = generatedIcon.transform.localPosition;
            generatedIcon.transform.localPosition = new Vector3(.1f, p.y + .01f, p.z);
        }
        SpriteRenderer LeftIcon = givenIcon.GetComponent<SpriteRenderer>();
        SpriteRenderer RightIcon = generatedIcon.GetComponent<SpriteRenderer>();
        LeftIcon.sprite = Sprite.Create(ModuleIcon, new Rect(0, 0, ModuleIcon.width, ModuleIcon.height), new Vector2(0.5f, 0.5f));
        int TopLeftModule = ModuleScript.Modules[ModuleIndex / maxSize].height - h;
        int x = (ModuleIndex * 32) % ModuleScript.Modules[ModuleIndex / maxSize].width;
        int y = TopLeftModule - (ModuleIndex % maxSize) / (ModuleScript.Modules[ModuleIndex / maxSize].width / 32) * 32;
        RightIcon.sprite = Sprite.Create(ModuleScript.Modules[ModuleIndex / maxSize], new Rect(x, y, w, h), new Vector2(0.5f, 0.5f));
    }

    public void DestroyClones()
    {
        DestroyImmediate(givenIcon);
        DestroyImmediate(generatedIcon);
        ModuleIcon = null;
    }*/
}