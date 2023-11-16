using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class iconicScript : MonoBehaviour {

    public KMBombModule Module;
    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMBossModule Boss;

    public SpriteRenderer TheIcon;
    public Sprite Empty, Blank, Banana;
    public Texture2D[] Modules;
    public TextMesh Phrase;
    public GameObject[] Rows;

    private string[] IgnoredModules;
    private KMSelectable[] Pixels = new KMSelectable[] { };

    private int NonBosses = 1;
    private int Solves;
    private string MostRecent;
    private List<string> SolveList = new List<string>{};
    private List<string> Queue = new List<string>{};
    private bool QueuedUp = false;
    private bool FoundAModule = false;
    private bool IgnoredsAdded = false;
    private bool ModuleReady = false;
    private bool TPCorrect = false;
    private int NumberOfIconics = 0;
    private int NumberOfOptions = 0;
    private int SelectedOption = 0;
    private int IgnoredSolved = 0;
    private int[] TopLeftModule;
    private string ModulePart = "";
    private string CurrentModule = "";
    private string CharacterList = ".0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private OrderedDictionary ModuleList;
    // Due to switching to a Dictionary, we lose the indicies for each module as Dictionaries do not guarantee order.
    // An OrderedDictionary allows us to index a Dictionary in its intended order, but it has no IndexOf function for matching a name to an index.
    // By creating the ordered dictionary and then casting the keys, this gives us the list of names we can index for obtaining sprites.
    // The next version of the Sprite Generator will grab the module names from the repo, so this is the temporary fix until the new system is worked on.
    private List<string> moduleListIDs;
    private string[] CurrentData = { };
    private static Dictionary<string, Sprite> LoadedSprites = new Dictionary<string, Sprite>();

    //Logging
    static int ModuleIdCounter = 1;
    int ModuleId;
    private bool ModuleSolved;

    // Use this for initialization
    void Start () {
        ModuleList = iconicData.ModuleList;
        if (ModuleList == null) {
            ModuleList = new OrderedDictionary {
                {string.Empty, iconicData.BlankModule}
            };
        }

        ModuleId = ModuleIdCounter++;
        for (int i = 0; i < Rows.Length; i++)
        {
            Pixels = Pixels.Concat(Rows[i].GetComponentsInChildren<KMSelectable>()).ToArray();
        }
        var ModuleSelectable = GetComponent<KMSelectable>();
        ModuleSelectable.Children = Pixels;
        ModuleSelectable.UpdateChildren();
        foreach (KMSelectable ThePixel in Pixels) {
            ThePixel.OnInteract += delegate () { PixelPress(ThePixel); return false; };
        }
        TopLeftModule = new int[Modules.Length];
        for (int i = 0; i < Modules.Length; i++)
            TopLeftModule[i] = Modules[i].height - 32;
        moduleListIDs = ModuleList.Keys.Cast<string>().ToList();

        if (IgnoredModules == null) {
            IgnoredModules = Boss.GetIgnoredModules("iconic", new string[]{
                "MemoryV2", "TurnTheKey", "SouvenirModule", "HexiEvilFMN", "timeKeeper", "simonsStages", "forgetThis", "PurgatoryModule", "troll", "forgetThemAll", "tallorderedKeys", "forgetEnigma", "forgetUsNot", "organizationModule", "qkForgetPerspective", "veryAnnoyingButton", "timingIsEverything", "forgetMeLater", "ubermodule", "qkUCN", "forgetItNot", "14", "simonForgets", "bamboozlingTimeKeeper", "brainf", "ForgetTheColors", "RPSJudging", "TheTwinModule", "iconic", "pwDestroyer", "omegaForget", "kugelblitz", "ANDmodule", "dontTouchAnything", "busyBeaver", "whiteout", "ForgetAnyColor", "omegaDestroyer", "KeypadDirectionality", "SecurityCouncil", "ShoddyChessModule", "FloorLights", "kataZenerCards", "blackArrowsModule", "forgetMazeNot", "plus", "doomsdayButton", "soulscream", "qkCubeSynchronization", "OutOfTime", "tetrahedron", "BoardWalk", "gemory", "duckKonundrum", "ConcentrationModule", "TwisterModule", "forgetOurVoices", "soulsong", "idExchange", "redLightGreenLight", "GSEight", "SimpleBoss", "SimpleBossNot", "KritGrandPrix", "repeatAgain", "ForgetMeMaybeModule", "HyperForget", "qkBitwiseOblivion", "damoclesLumber", "top10nums", "queensWarModule", "forget_fractal", "pointerPointerModule", "slightGibberishTwistModule", "PianoParadoxModule", "Omission", "nobodysCodeModule", "inOrderModule", "perspectiveStackingModule", "ReportingAnomalies", "forgetle", "ActionsAndConsequences", "fizzBoss", "WatchTheClock", "solveShift", "BlackoutModule", "hickoryDickoryDockModule"
            });
        }

        Module.OnActivate += delegate () {
            NonBosses = Bomb.GetSolvableModuleIDs().Where(a => !IgnoredModules.Contains(a)).ToList().Count;
        };

		for (int i = 0; i < Bomb.GetModuleIDs().Count(); i++) {
            if (Bomb.GetModuleIDs()[i] == "iconic") {
                NumberOfIconics += 1;
            } else {
                continue;
            }
        }

        AddNeedies();

        StartCoroutine(Delay());
	}

    private IEnumerator Delay () {
        yield return new WaitUntil(() => iconicLoader.ListReady);
        ModuleReady = true;
		Debug.LogFormat("[Iconic #{0}] Iconic's starting process complete.", ModuleId);
    }

	// Update is called once per frame
	void Update () {
        if (ModuleSolved == false) {
            Solves = Bomb.GetSolvedModuleIDs().Count();
            if (Solves > SolveList.Count()) {
                MostRecent = GetLatestSolve(Bomb.GetSolvedModuleIDs(), SolveList);
                if (!(IgnoredModules.Contains(MostRecent)))
                {
                    Queue.Add(MostRecent);
                    SolveList.Add(MostRecent);
                } else {
                    Debug.LogFormat("[Iconic #{0}] The following ignored module has solved: {1}", ModuleId, MostRecent);
                    SolveList.Add(MostRecent);
                    IgnoredSolved += 1;
                }
            }
            if (QueuedUp == false && Queue.Count() > 0) {
                FoundAModule = false;
                Array.Clear(CurrentData, 0, CurrentData.Count());
                if (ModuleList.Contains(Queue[0]) && FoundAModule == false)
                {
                    CurrentData = ((string[])ModuleList[Queue[0]]).ToArray();
                    if (!LoadedSprites.ContainsKey(Queue[0]))
                    {
                        int i = moduleListIDs.IndexOf(Queue[0]);
                        // It is assumed that the max width and height for each spritesheet is the same,
                        // so assume we can get the max size by dividing my the width and height of the first master sheet.
                        // The size is based on icon count as opposed to pixel count, so divide the pixels by the number of pixels in a single icon.
                        // If the size of the icons are changed, this number will need to be adjusted accordingly.
                        int maxSize = Modules[0].width * Modules[0].height / 1024;
                        int index = i / maxSize;
                        // Since x is modulo'd and the width is (should be) always the same, we don't need to alter it.
                        int x = (i * 32) % Modules[index].width;
                        // Since we're dividing the rows by the number in each row, we have to divide by the width here.
                        // This should give us the row we're on.
                        int y = TopLeftModule[index] - (i % maxSize) / (Modules[index].width / 32) * 32;
                        Sprite loadedSprite = Sprite.Create(Modules[index], new Rect(x, y, 32, 32), new Vector2(0.5f, 0.5f));
                        LoadedSprites.Add(Queue[0], loadedSprite);
                    }
                    TheIcon.sprite = LoadedSprites[Queue[0]];
                    CurrentModule = Queue[0];
                    FoundAModule = true;
                }
                if (FoundAModule == false) {
                    Debug.LogFormat("[Iconic #{0}] Adding blank because I don't recognize the following module: {1}", ModuleId, Queue[0]);
                    TheIcon.sprite = Blank;
                    CurrentData = iconicData.BlankModule.ToArray();
                    CurrentModule = "(Blank)";
                    FoundAModule = true;
                }
                QueuedUp = true;

                NumberOfOptions = CurrentData.Count();

                SelectedOption = UnityEngine.Random.Range(1, NumberOfOptions);
                ModulePart = CurrentData[SelectedOption];

                int length = CurrentData.Length < 1 ? -1 : CurrentData[0].Length;
                bool[] conditions = new[]
                {
                    ModulePart == null,
                    length != 1024,
                    CurrentData.Length < 2
                };

                if (conditions.Any(x => x))
                {
                    int index = Array.IndexOf(conditions, true);
                    string[] messages = new[]
                    {
                        "A part of " + Queue[0] + " cannot be found.",
                        "The string for " + Queue[0] + "is not 1024 characters long. (It's " + length + " characters long)",
                        "The string array for " + Queue[0] + "doesn't have enough arguments. (There's only " + CurrentData.Length +" strings.)" //For some reason this doesn't work...
                    };
                    Debug.LogFormat("[Iconic #{0}] {1} Autosolving the module.", ModuleId, messages[index]);
                    Phrase.text = Queue[0] + " error!";
                    SquishText(Phrase.text);
                    ModuleSolved = true;
                    Module.HandlePass();
                    return;
                }

                Phrase.text = ModulePart;
                SquishText(ModulePart);

            }
            if ((SolveList.Count() - IgnoredSolved == NonBosses && Queue.Count() == 0) && ModuleReady) {
                Phrase.text = "GG!";
                Audio.PlaySoundAtTransform("GoodGame", transform);
                Debug.LogFormat("[Iconic #{0}] All icons shown, Module solved.", ModuleId);
                TheIcon.sprite = Banana;
                Module.HandlePass();
                ModuleSolved = true;
            } else if (SolveList.Count() - IgnoredSolved == NonBosses && IgnoredsAdded == false) {
                AddIgnoreds();
            }
        }
	}

    void AddNeedies () {
        for (int i = 0; i < Bomb.GetModuleIDs().Count(); i++) {
            if (Bomb.GetSolvableModuleIDs().Contains(Bomb.GetModuleIDs()[i])) {
                continue;
            } else {
                Queue.Add(Bomb.GetModuleIDs()[i]);
            }
        }
	}

    void AddIgnoreds () {
        for (int i = 0; i < Bomb.GetModuleIDs().Count(); i++) {
            if (IgnoredModules.Contains(Bomb.GetModuleIDs()[i]) && Bomb.GetModuleIDs()[i] != "iconic") {
                Queue.Add(Bomb.GetModuleIDs()[i]);
            } else {
                continue;
            }
        }

        NumberOfIconics -= 1;
        for (int i = 0; i < NumberOfIconics; i++) {
            Queue.Add("iconic");
        }

        IgnoredsAdded = true;
	}

    void PixelPress (KMSelectable ThePixel) {
        if (QueuedUp == true) {
            Audio.PlaySoundAtTransform("Blip", transform);
            for (int p = 0; p < 1024; p++) {
                if (ThePixel == Pixels[p]) {
                    string debugMessage = string.Format("[Iconic #{0}] Correct part of {1} selected, \"{2}\" at {3}.", ModuleId, CurrentModule, ModulePart, ConvertToCoordinate(p));
                    TPCorrect = CharacterList[SelectedOption] == CurrentData[0][p];
                    if (!TPCorrect)
                    {
                        debugMessage = debugMessage.Replace("Correct", "Incorrect").Replace("\" at", "\" is not at") + " Strike!";
                        Module.HandleStrike();
                    }
                    Debug.LogFormat(debugMessage);
                    Queue.RemoveAt(0);
                    TheIcon.sprite = Empty;
                    QueuedUp = false;
                    Phrase.transform.localScale = new Vector3(0.001f, 0.001f, 0.01f);
                    Phrase.text = "Iconic";
                }
            }
        }
    }

    private string GetLatestSolve(List<string> a, List<string> b)
    {
        string z = "";
        for(int i = 0; i < b.Count; i++)
        {
            a.Remove(b.ElementAt(i));
        }
        z = a.ElementAt(0);
        return z;
    }

    private void SquishText (string disp) {
        if (disp.Length > 29) {
            Phrase.transform.localScale = new Vector3(0.0002f, 0.001f, 0.01f);
        } else if (disp.Length > 13) {
            Phrase.transform.localScale = new Vector3(0.00025f, 0.001f, 0.01f);
        } else if (disp.Length > 7) {
            Phrase.transform.localScale = new Vector3(0.0005f, 0.001f, 0.01f);
        }
    }

    private string ConvertToCoordinate(int p) {
        int c = p % 32;
        int d = p / 32;
        if (c > 25) {
            return "A" + CharacterList[11+(c-26)] + (d+1).ToString();
        } else {
            return CharacterList[11+c] + (d+1).ToString();
        }
    }

    private int ConvertToNumber(string p)
    {
        string[] initials = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF" };
        string[] rows = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32" };
        string temp = "";
        int index = -1;
        int times = 0;
        for (int i = 31; i > -1; i--)
        {
            if (p.ToUpper().Contains(initials[i]) && times == 0)
            {
                index = i;
                times += 1;
                temp += initials[index];
            }
        }
        for (int i = 31; i > -1; i--)
        {
            if (p.Contains(rows[i]) && times == 1)
            {
                temp += rows[i];
                index = index + (i * 32);
                times += 1;
            }
        }
        if (times == 2 && temp.Equals(p.ToUpper()))
            return index;
        else
            return -1;
    }

    //twitch plays
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} press <coord> [Presses the pixel as the specified coordinate] | Valid coordinates are A1-AF32 (after Z it becomes AA)";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        string[] parameters = command.Split(' ');
        if (Regex.IsMatch(parameters[0], @"^\s*press\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (parameters.Length > 2)
            {
                yield return "sendtochaterror Too many parameters!";
            }
            else if (parameters.Length == 2)
            {
                int index = ConvertToNumber(parameters[1]);
                if (index != -1)
                {
                    Pixels[index].OnInteract();
                    if (TPCorrect) {
                      yield return "awardpoints 1";
                      TPCorrect = false;
                    }
                }
                else
                {
                    yield return "sendtochaterror The specified coordinate of the pixel you wish to press '" + parameters[1] + "' is invalid!";
                }
            }
            else if (parameters.Length == 1)
            {
                yield return "sendtochaterror Please specify the coordinate of the pixel you wish to press!";
            }
            yield break;
        }
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        while (!ModuleSolved)
        {
            while (Queue.Count() == 0) { yield return true; yield return new WaitForSeconds(0.1f); }
            List<int> allindexes = new List<int>();
            for (int i = 0; i < 1024; i++)
            {
                if (CharacterList[SelectedOption] == CurrentData[0][i])
                {
                    allindexes.Add(i);
                }
            }
            yield return new WaitForSeconds(0.001f);
            Pixels[allindexes[UnityEngine.Random.Range(0, allindexes.Count())]].OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
