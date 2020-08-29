#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[CustomEditor(typeof(GenerateMasterSprite))]
public class SpriteObject : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate Spritesheet"))
        {
            ((GenerateMasterSprite)target).Generate();
        }
    }
}

public class GenerateMasterSprite : MonoBehaviour
{
    public int cols = 21;
    public int w = 32;
    public int h = 32;
    public iconicScript ModuleScript;
    private static Dictionary<string, string> mismatchedNames = new Dictionary<string, string>
    {
        { "Needy Vent Gas", "Venting Gas" },
        { "Needy Capacitor", "Capacitor Discharge" },
        { "Needy Knob", "Knob" },
        { "Rock-Paper-Scissors-L.-Sp.", "Rock-Paper-Scissors-Lizard-Spock" },
        { "Needy Flower Mash", "Flower Mash" },
        { "Cruel Keypads", "Cruel Keypad" },
        { "Maze³", "Maze^3" },
        { "...?", "puncuationMarks" },
        { "Button Grid", "Button Grids" }
    };
    private static Regex regex = new Regex("[^a-zA-Z0-9❖]");
    public void Generate()
    {
        var iconsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Assets" + Path.DirectorySeparatorChar + "Icons");
        // This uses Reflection to grab the list of module names from the iconicScript
        // Since you won't be using this gameobject in the module itself it shouldn't matter if it's inefficient (or lazier than just copying the list to this file)
        List<string> ModuleNames = typeof(iconicScript).GetFields(BindingFlags.NonPublic | BindingFlags.Instance).First(x => x.Name == "ModuleList").GetValue(ModuleScript) as List<string>;
        // Switch out mismatched names with ones that will likely match
        ModuleNames = ModuleNames.Select(x => misMatched(x)).ToList();
        
        // Grab all of the file paths from the icons directories and sort them by the order in ModuleNames.
        // It is important that the names in ModuleNames matches the names of the files.
        var iconFiles = new DirectoryInfo(iconsDirectory).GetFiles("*.png", SearchOption.TopDirectoryOnly).OrderBy(x => ModuleNames.IndexOf(punct(x.Name))).ToList();
        // Remove icons that are not included in ModuleNames
        iconFiles = iconFiles.Where(x => ModuleNames.Contains(punct(x.Name))).ToList();
        
        // Determine the number of rows based on the number of columns
        var rows = (iconFiles.Count + cols - 1) / cols;
        // Create the Texture we'll use to make the final PNG
        // The size must be predetermined according to Texture2D.SetPixels()
        var fullImage = new Texture2D(cols * w, rows * h);
        // Note, allColors may only contain 128x128 module icons. A new sheet will need to be made if this is ever met.
        // We are currently at 32x32, so it will likely take a long time to hit this limit.
        // If 128x128 is not the proper limit, it'll simply be necessary to make multiple sheets.
        List<IEnumerable<Color>> allColors = new List<IEnumerable<Color>>();
        for (int i = 0; i < rows; i++)
        {
            // Load the pixels for each icon
            List<Color[]> eachColors = new List<Color[]>();
            FileStream fs;
            for (int j = 0; j < cols; j++)
            {
                // Texture2D.SetPixels requires an exact size, so fill the rest of the pixels with white
                if (i * cols + j >= iconFiles.Count)
                {
                    eachColors.Add(Enumerable.Repeat(Color.white, 32 * 32).ToArray());
                    continue;
                }
                // Grab the filestream from each fileinfo, and read the filestream to a byte array
                // Close the filestream so the file can be used by other programs if needed
                // Translate the image into a texture so that the pixels can be loaded from it
                // eachColors contains the pixels for each icon in a row
                fs = iconFiles[i * cols + j].OpenRead();
                var imageBytes = new byte[fs.Length];
                fs.Read(imageBytes, 0, System.Convert.ToInt32(fs.Length));
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
        fullImage.SetPixels(allColors.SelectMany(x => x).ToArray());
        var finalImage = fullImage.EncodeToPNG();
        File.WriteAllBytes(Path.Combine(iconsDirectory, Path.Combine("Extras","Master Sheet.png")), finalImage);
    }

    // Change Module Names to match their file names
    static string misMatched(string name)
    {
        var newName = name;
        if (mismatchedNames.ContainsKey(name))
            newName = mismatchedNames[name];
        return regex.Replace(newName.ToLowerInvariant(), string.Empty);
    }

    // Remove file extension and punctuation when comparing file names with module names
    static string punct(string original)
    {
        // .WAV could be detected as a file extension, so exclude that
        string notOriginal = original != "Jukebox.WAV" ? Path.GetFileNameWithoutExtension(original) : original;
        notOriginal = regex.Replace(notOriginal.ToLowerInvariant(), string.Empty);
        return notOriginal;
    }
}
#endif