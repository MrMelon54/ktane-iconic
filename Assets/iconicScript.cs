using System;
using System.Collections;
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

    public MeshRenderer TheIcon;
    public Texture2D Empty, Blank, Banana;
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
	private List<string> ModuleList = new List<string>{ "Wires", "The Button", "Keypad", "Simon Says", "Who's on First", "Memory", "Morse Code", "Complicated Wires", "Wire Sequence", "Maze", "Password", "Needy Vent Gas", "Needy Capacitor", "Needy Knob", "Colour Flash", "Piano Keys", "Semaphore", "Needy Math", "Emoji Math", "Lights Out", "Switches", "Two Bits", "Word Scramble", "Anagrams", "Combination Lock", "Filibuster", "Motion Sense", "Square Button", "Simon States", "Round Keypad", "Listening", "Foreign Exchange Rates", "Answering Questions", "Orientation Cube", "Morsematics", "Connection Check", "Letter Keys", "Forget Me Not", "Rotary Phone", "Astrology", "Logic", "Crazy Talk", "Adventure Game", "Turn The Key", "Mystic Square", "Plumbing", "Cruel Piano Keys", "Safety Safe", "Tetris", "Cryptography", "Chess", "Turn The Keys", "Mouse In The Maze", "3D Maze", "Silly Slots", "Number Pad", "Laundry", "Probing", "Alphabet", "Resistors", "Skewed Slots", "Caesar Cipher", "Perspective Pegs", "Microcontroller", "Murder", "The Gamepad", "Tic Tac Toe", "Who's That Monsplode", "Monsplode, Fight!", "Shape Shift", "Follow the Leader", "Friendship", "The Bulb", "Blind Alley", "Sea Shells", "English Test", "Rock-Paper-Scissors-L.-Sp.", "Hexamaze", "Bitmaps", "Colored Squares", "Adjacent Letters", "Third Base", "Souvenir", "Word Search", "Broken Buttons", "Simon Screams", "Modules Against Humanity", "Complicated Buttons", "Battleship", "Text Field", "Symbolic Password", "Wire Placement", "Double-Oh", "Cheap Checkout", "Coordinates", "Light Cycle", "HTTP Response", "Rhythms", "Color Math", "Only Connect", "Neutralization", "Web Design", "Chord Qualities", "Creation", "Rubik's Cube", "FizzBuzz", "The Clock", "LED Encryption", "Edgework", "Bitwise Operations", "Fast Math", "Minesweeper", "Zoo", "Binary LEDs", "Boolean Venn Diagram", "Point of Order", "Ice Cream", "Hex To Decimal", "The Screw", "Yahtzee", "X-Ray", "QR Code", "Button Masher", "Random Number Generator", "Color Morse", "Mastermind Simple", "Mastermind Cruel", "Gridlock", "Big Circle", "Morse-A-Maze", "Colored Switches", "Perplexing Wires", "Monsplode Trading Cards", "Game of Life Simple", "Game of Life Cruel", "Nonogram", "S.E.T.", "Needy Beer Refill Mod", "Painting", "Color Generator", "Shape Memory", "Symbol Cycle", "Hunting", "Extended Password", "Curriculum", "Braille", "Mafia", "Festive Piano Keys", "Flags", "Timezone", "Polyhedral Maze", "Symbolic Coordinates", "Poker", "Sonic the Hedgehog", "Poetry", "Button Sequence", "Algebra", "Visual Impairment", "The Jukebox", "Identity Parade", "Maintenance", "Blind Maze", "Backgrounds", "Mortal Kombat", "Mashematics", "Faulty Backgrounds", "Radiator", "Modern Cipher", "LED Grid", "Sink", "The iPhone", "The Swan", "Waste Management", "Human Resources", "Skyrim", "Burglar Alarm", "Press X", "European Travel", "Error Codes", "Rapid Buttons", "LEGOs", "Rubik’s Clock", "Font Select", "The Stopwatch", "Pie", "The Wire", "The London Underground", "Logic Gates", "Forget Everything", "Grid Matching", "Color Decoding", "The Sun", "Playfair Cipher", "Tangrams", "The Number", "Cooking", "Superlogic", "The Moon", "The Cube", "Dr. Doctor", "Tax Returns", "The Jewel Vault", "Digital Root", "Graffiti Numbers", "Marble Tumble", "X01", "Logical Buttons", "The Code", "Tap Code", "Simon Sings", "Simon Sends", "Synonyms", "Greek Calculus", "Simon Shrieks", "Complex Keypad", "Subways", "Lasers", "Turtle Robot", "Guitar Chords", "Calendar", "USA Maze", "Binary Tree", "The Time Keeper", "Lightspeed", "Black Hole", "Simon's Star", "Morse War", "The Stock Market", "Mineseeker", "Maze Scrambler", "The Number Cipher", "Alphabet Numbers", "British Slang", "Double Color", "Maritime Flags", "Equations", "Determinants", "Pattern Cube", "Know Your Way", "Splitting The Loot", "Simon Samples", "Character Shift", "Uncolored Squares", "Dragon Energy", "Flashing Lights", "3D Tunnels", "Synchronization", "The Switch", "Reverse Morse", "Manometers", "Shikaku", "Wire Spaghetti", "Tennis", "Module Homework", "Benedict Cumberbatch", "Signals", "Horrible Memory", "Boggle", "Command Prompt", "Boolean Maze", "Sonic & Knuckles", "Quintuples", "The Sphere", "Coffeebucks", "Colorful Madness", "Bases", "Lion's Share", "Snooker", "Blackjack", "Party Time", "Accumulation", "The Plunger Button", "The Digit", "The Jack-O'-Lantern", "T-Words", "Divided Squares", "Connection Device", "Instructions", "Valves", "Encrypted Morse", "The Crystal Maze", "Cruel Countdown", "Countdown", "Catchphrase", "Blockbusters", "IKEA", "Retirement", "Periodic Table", "101 Dalmatians", "Schlag den Bomb", "Mahjong", "Kudosudoku", "The Radio", "Modulo", "Number Nimbleness", "Pay Respects", "Challenge & Contact", "The Triangle", "Sueet Wall", "Hot Potato", "Christmas Presents", "Hieroglyphics", "Functions", "Scripting", "Needy Mrs Bob", "Simon Spins", "Ten-Button Color Code", "Cursed Double-Oh", "Crackbox", "Street Fighter", "The Labyrinth", "Spinning Buttons", "Color Match", "The Festive Jukebox", "Skinny Wires", "The Hangover", "Factory Maze", "Binary Puzzle", "Broken Guitar Chords", "Regular Crazy Talk", "Hogwarts", "Dominoes", "Simon Speaks", "Discolored Squares", "Krazy Talk", "Numbers", "Flip The Coin", "Varicolored Squares", "Simon's Stages", "Free Parking", "Cookie Jars", "Alchemy", "Zoni", "Simon Squawks", "Unrelated Anagrams", "Mad Memory", "Bartending", "Question Mark", "Shapes And Bombs", "Flavor Text EX", "Flavor Text", "Decolored Squares", "Homophones", "DetoNATO", "Air Traffic Controller", "SYNC-125 [3]", "Westeros", "Morse Identification", "Pigpen Rotations", "LED Math", "Alphabetical Order", "Simon Sounds", "The Fidget Spinner", "Simon's Sequence", "Simon Scrambles", "Harmony Sequence", "Unfair Cipher", "Melody Sequencer", "Colorful Insanity", "Passport Control", "Left and Right", "Gadgetron Vendor", "Wingdings", "The Hexabutton", "Genetic Sequence", "Micro-Modules", "Module Maze", "Elder Futhark", "Tasha Squeals", "Forget This", "Digital Cipher", "Subscribe to Pewdiepie", "Grocery Store", "Draw", "Burger Alarm", "Purgatory", "Mega Man 2", "Lombax Cubes", "The Stare", "Graphic Memory", "Quiz Buzz", "Wavetapping", "The Hypercube", "Speak English", "Stack'em", "Seven Wires", "Colored Keys", "The Troll", "Planets", "The Necronomicon", "Four-Card Monte", "aa", "The Giant's Drink", "Digit String", "Alpha", "Snap!", "Hidden Colors", "Colour Code", "Vexillology", "Brush Strokes", "Odd One Out", "The Triangle Button", "Mazematics", "Equations X", "Maze³", "Gryphons", "Arithmelogic", "Roman Art", "Faulty Sink", "Simon Stops", "Morse Buttons", "Terraria Quiz", "Baba Is Who?", "Triangle Buttons", "Simon Stores", "Risky Wires", "Modulus Manipulation", "Daylight Directions", "Cryptic Password", "Stained Glass", "The Block", "Bamboozling Button", "Insane Talk", "Transmitted Morse", "A Mistake", "Red Arrows", "Green Arrows", "Yellow Arrows", "Encrypted Values", "Encrypted Equations", "Forget Them All", "Ordered Keys", "Blue Arrows", "Sticky Notes", "Unordered Keys", "Orange Arrows", "Hyperactive Numbers", "Reordered Keys", "Button Grid", "Find The Date", "Misordered Keys", "The Matrix", "Purple Arrows", "Bordered Keys", "The Dealmaker", "Seven Deadly Sins", "The Ultracube", "Symbolic Colouring", "Recorded Keys", "The Deck of Many Things", "Disordered Keys", "Character Codes", "Raiding Temples", "Bomb Diffusal", "Tallordered Keys", "Pong", "Ten Seconds", "Cruel Ten Seconds", "Double Expert", "Calculus", "Boolean Keypad", "Toon Enough", "Pictionary", "Qwirkle", "Antichamber", "Simon Simons", "Lucky Dice", "Forget Enigma", "Constellations", "Prime Checker", "Cruel Digital Root", "Faulty Digital Root", "Needy Crafting Table", "Boot Too Big", "Vigenère Cipher", "Langton's Ant", "Old Fogey", "Insanagrams", "Treasure Hunt", "Snakes and Ladders", "Module Movements", "Bamboozled Again", "Safety Square", "Roman Numerals", "Colo(u)r Talk", "Annoying Arrows", "Double Arrows", "Boolean Wires", "Block Stacks", "Vectors", "Partial Derivatives", "Caesar Cycle", "Needy Piano", "Forget Us Not", "Affine Cycle", "Pigpen Cycle", "Flower Patch", "Playfair Cycle", "Jumble Cycle", "Organization", "Forget Perspective", "Alpha-Bits", "Jack Attack", "Ultimate Cycle", "Hill Cycle", "Binary", "Chord Progressions", "Matchematics", "Bob Barks", "Simon's On First", "Weird Al Yankovic", "Forget Me Now", "Simon Selects", "The Witness", "Simon Literally Says", "Cryptic Cycle", "Bone Apple Tea", "Robot Programming", "Masyu", "Hold Ups", "Red Cipher", "Flash Memory", "A-maze-ing Buttons", "Desert Bus", "Orange Cipher", "Common Sense", "The Very Annoying Button", "Unown Cipher", "Needy Flower Mash", "TetraVex", "Meter", "Timing is Everything", "The Modkit", "The Rule", "Fruits", "Bamboozling Button Grid", "Footnotes", "Lousy Chess", "Module Listening", "Garfield Kart", "Yellow Cipher", "Kooky Keypad", "Green Cipher", "RGB Maze", "Blue Cipher", "The Legendre Symbol", "Keypad Lock", "Forget Me Later", "Übermodule", "Heraldry", "Faulty RGB Maze", "Indigo Cipher", "Violet Cipher", "Encryption Bingo", "Color Addition", "Chinese Counting", "Tower of Hanoi", "Keypad Combinations", "UltraStores", "Kanji", "Geometry Dash", "Ternary Converter", "N&Ms", "Eight Pages", "The Colored Maze", "White Cipher", "Gray Cipher", "The Hyperlink", "Black Cipher", "Loopover", "Divisible Numbers", "Corners", "The High Score", "Ingredients", "Jenga", "Intervals", "Cruel Boolean Maze", "Cheep Checkout", "Spelling Bee", "Memorable Buttons", "Thinking Wires", "Seven Choose Four", "Object Shows", "Lunchtime", "Natures", "Neutrinos", "Scavenger Hunt", "Polygons", "Ultimate Cipher", "Codenames", "Odd Mod Out", "Logic Statement", "Blinkstop", "Ultimate Custom Night", "Hinges", "Time Accumulation", "❖", "Forget It Not", "egg", "BuzzFizz", "Answering Can Be Fun", "3x3 Grid", "15 Mystic Lights", "14", "Rainbow Arrows", "Time Signatures", "Multi-Colored Switches", "Digital Dials", "Passcodes", "Hereditary Base Notation", "Lines of Code", "The cRule", "Prime Encryption", "Encrypted Dice", "Colorful Dials", "Naughty or Nice", "Following Orders", "Binary Grid", "Matrices", "Cruel Keypad", "The Black Page", "64", "Simon Forgets", "Greek Letter Grid", "Bamboozling Time Keeper", "Scalar Dials", "The World's Largest Button", "Keywords", "State of Aggregation", "Dreamcipher", "Brainf---", "Rotating Squares", "Red Light Green Light", "Marco Polo", "Hyperneedy", "Echolocation", "Boozleglyph Identification", "Topsy Turvy", "Boxing", "Railway Cargo Loading", "Conditional Buttons", "ASCII Art", "Semamorse", "Hide and Seek", "Symbolic Tasha", "Alphabetical Ruling", "Microphone", "Widdershins", "Lockpick Maze", "Dimension Disruption", "V", "Silhouettes", "A Message", "Alliances", "Dungeon", "Unicode", "Password Generator", "Baccarat", "Guess Who?", "Reverse Alphabetize", "Alphabetize", "Gatekeeper", "Light Bulbs", "1000 Words", "Five Letter Words", "Settlers of KTaNE", "The Hidden Value", "Red", "Blue", "Directional Button", "...?", "The Simpleton", "Misery Squares", "Not Wiresword", "Not Wire Sequence", "Not Who's on First", "Not Simaze", "Not Password", "Not Morse Code", "Not Memory", "Not Maze", "Not Keypad", "Not Complicated Wires", "Not Capacitor Discharge", "Not the Button", "Sequences", "Dungeon 2nd Floor", "Wire Ordering", "Vcrcs", "Quaternions", "Abstract Sequences", "osu!", "Shifting Maze", "Sorting", "Role Reversal", "Placeholder Talk", "Art Appreciation", "Shell Game", "Pattern Lock", "Quick Arithmetic", "Minecraft Cipher", "Cheat Checkout", "The Samsung", "Forget The Colors", "Etterna", "Recolored Switches", "Cruel Garfield Kart", "1D Maze", "Reverse Polish Notation", "Snowflakes", "Funny Numbers", "Exoplanets", "Simon Stages", "Not Venting Gas", "Forget Infinity", "Faulty Seven Segment Displays", "Stock Images", "Roger", "Malfunctions", "Minecraft Parody", "Shuffled Strings", "NumberWang", "Minecraft Survival", "RPS Judging", "Fencing", "Strike Solve", "Uncolored Switches", "The Twin", "Name Changer", "Just Numbers", "Lying Indicators", "Flag Identification", "Training Text", "Wonder Cipher", "Caesar's Maths", "Triamonds", "Stars", "Button Order", "Jukebox.WAV", "Elder Password", "Switching Maze", "Iconic", "Mystery Module", "Ladder Lottery", "Co-op Harmony Sequence", "Standard Crazy Talk", "Quote Crazy Talk End Quote", "Kilo Talk", "KayMazey Talk", "Jaden Smith Talk", "Deck Creating", "Crazy Talk With A K", "BoozleTalk", "Arrow Talk", "Siffron", "Red Herring", "Audio Morse", "Palindromes", "Pow", "Type Racer", "Chicken Nuggets", "Badugi", "Tetriamonds", "Spot the Difference", "Negativity", "Masher The Bottun", "Yes and No", "M&Ns", "Plant Identification", "Integer Trees", "Goofy's Game", "Module Rick", "Pickup Identification", "Earthbound", "3 LEDs", "Life Iteration", "Thread the Needle", "Encrypted Hangman", "Accelerando", "Reaction", "The Heart", "Color Braille", "Remote Math", "Reflex", "Password Destroyer", "Typing Tutor", "Multitask", "hexOS", "Simon Stashes", "Kyudoku", "Brawler Database", "Shortcuts", "More Code", "7", "OmegaForget", "Needy Game of Life", "Mental Math", "Kugelblitz", "Dictation", "Bloxx", "Basic Morse", "The Arena", "IPA", "Emotiguy Identification", "100 Levels of Defusal", "NeeDeez Nuts", "Jailbreak", "Dumb Waiters", "DACH Maze", "Birthdays", "Match 'em", "Navinums", "Gnomish Puzzle", "RGB Logic", "Bridges", "A>N<D", "Shifted Maze", "Juxtacolored Squares", "Wolf, Goat, and Cabbage", "The Missing Letter", "Amnesia", "Plug-Ins", "Synesthesia", "English Entries", "The Duck", "The Cruel Duck", "Identifying Soulless", "Ultimate Tic Tac Toe", "Factoring", "Lyrical Nonsense", "RGB Sequences", "Puzzword", "NOT NOT", "Repo Selector", "int##", "Deaf Alley", "Blind Arrows", "Sound Design", "RGB Arithmetic", "D-CODE", "Rapid Subtraction", "Fifteen", "Pixel Cipher", "Don't Touch Anything", "The Great Void", "21", "Prime Time", "Negation", "The Calculator", "SixTen", "ASCII Maze", "Ultralogic", "Spangled Stars", "Busy Beaver", "Digital Clock", "Cruel Match 'em", "Assembly Code", "Simon's Ultimate Showdown", "Boomdas", "Needlessly Complicated Button", "Color Numbers", "Chinese Strokes", "Chalices", "Reversed Edgework", "Pixel Art", "Faulty Accelerando", "0", "Pitch Perfect", "Increasing Indices", "Faulty Binary", "Cruel Binary", "Connected Monitors", "Broken Binary", "Totally Accurate Minecraft Simulator", "Tell Me When", "ReGret-B Filtering", "D-CRYPT", "Color-Cycle Button", "Alien Filing Colors", "The Kanye Encounter", "Entry Number Four", "D-CIPHER", "501", "42", "Color One Two", "Toolmods", "Spelling Buzzed", "Burnout", "Brown Bricks", "Mystic Maze", "Chinese Zodiac", "Four Lights", "Duck, Duck, Goose", "Not Knob", "Working Title", "Toolneedy", "One Links To All", "Rules", "Tenpins", "Double Listening", "Wack Game of Life", "Unfair's Revenge", "Unfair's Cruel Revenge", "Mindlock", "Golf", "Regular Hexpressions", "Literally Nothing", "Colored Buttons", "Censorship", "The Pentabutton", "Mechanus Cipher", "Digisibility", "Breaktime", "Space Invaders Extreme", "Mazery", "Kim's Game", "Three Cryptic Steps", "Popufur", "Tech Support", "Space", "9-Ball", "Metamem", "M&Ms", "The Console", "Pocket Planes", "Bridge", "Rotten Beans", "Long Beans", "Jellybeans", "Cool Beans", "Beans", "Beanboozled Again", "The Dials", "Butterflies", "Broken Karaoke", "Chamber No. 5", "Silenced Simon", "Teal Arrows", "Faulty Butterflies", "Keep Clicking", "Frankenstein's Indicator", "16 Coins", "Sea Bear Attacks", "Alphabet Tiles", "Literally Crying", "Double Pitch", "Devilish Eggs", "h", "Rune Match I", "Rune Match II", "Rune Match III", "Quick Time Events", "Iñupiaq Numerals", "The Bioscanner", "Ars Goetia Identification", "Pixel Number Base", "Silo Authorization", "Gradually Watermelon", "Mastermind Restricted", "Logical Operators", "Higher Or Lower", "Even Or Odd", "Digital Grid", "Reformed Role Reversal", "Whiteout", "N&Ns", "Gettin' Funky", "Cell Lab", "Lights On", "Color Hexagons", "Commuting", "Symmetries Of A Square", "Look and Say", "Currents", "Partitions", "Telepathy", "Cruel Stars", "Button Messer", "Taco Tuesday", "The Overflow", "Nomai", "Forget Any Color", "Table Madness", "Melodic Message", "Sugar Skulls", "Colour Catch", "Cosmic", "Semabols", "Mislocation", "Musher The Batten", "Tribal Council", "Simon Smiles", "Outrageous", "Faulty Chinese Counting", "Press The Shape", "Baybayin Words", "OmegaDestroyer", "Going Backwards", "Atbash Cipher", "Venn Diagrams", "Numbered Buttons", "Colored Hexabuttons", "Video Poker", "White Arrows", "Johnson Solids", "Bottom Gear", "Two Persuasive Buttons", "Keypad Directionality", "% Grey", "Towers", "Letter Layers", "The Exploding Pen", "Snack Attack", "ReGrettaBle Relay", "Security Council", "Standard Button Masher", "Musical Transposition", "Jackbox.TV", "The Furloid Jukebox", "The Close Button", "Updog", "Saimoe Pad", "B-Machine", "Addition", "What's on Second", "Quaver", "Another Keypad Module", "Think Fast", "Shoddy Chess", "Rhythm Test", "Validation", "Floor Lights", "Bad Wording", "Etch-A-Sketch", "Zener Cards", "Diophantine Equations", "Ternary Tiles", "Striped Keys", "Rullo", "Flashing Arrows", "Cruello", "Coloured Arrows", "Black Arrows", "Tetris Sprint", "Forget Maze Not", "Double Screen", "The Sequencyclopedia", "eeB gnillepS", "Pandemonium Cipher", "Mystery Widget", "Mineswapper", "Phosphorescence", "The Klaxon", "Valued Keys", "Numerical Knight Movement", "Bandboozled Again", "SpriteClub Betting Simulation", "Ramboozled Again", "Simon Subdivides", "Hole in One", "Hexiom", "Collapse", "Back Buttons", "Audio Keypad", "Saimoe Maze", "Kidney Beans", "Fake Beans", "Chilli Beans", "Big Bean", "Bean Sprouts", "Tell Me Why", "Quiplash", "Bowling", "Sporadic Segments", "Linq", "Entry Number One", "DNA Mutation", "RGB Hypermaze", "Boob Tube", "Regular Sudoku", "AAAAA", "Polyrhythms", "Drive-In Window", "The 12 Days of Christmas", "X", "Y", "The Xenocryst", "Rebooting M-OS", "Stacked Sequences", "Complexity", "Small Circle", "Fractal Maze", "Simon Stumbles", "Wild Side", "The Octadecayotton", "Colored Letters", "Kahoot!", "Forget's Ultimate Showdown", "Bomb Corp. Filing", "Ultra Digital Root", "Mii Identification", "81", "Simon Swindles", "Next In Line", "Functional Mapping", "Stable Time Signatures", "Keypad Maze", "Astrological", "XmORse Code", "Corridors", "Decay", "Crazy Speak", "Large Password", "Large Free Password", "Free Password", "Not Timer", "The Burnt", "Access Codes", "Cistercian Numbers", "Brown Cipher", "Code Cracker", "Indentation", "One-Line", "The Speaker", "Interpunct", "Double Knob", "Name Codes", "The 1, 2, 3 Game", "Papa's Pizzeria", "Keypad Magnified", "Hold On", "Diffusion", "Soy Beans", "The Shaker", "Coffee Beans", "Newline", "Letter Grid", "Ghost Movement", "+", "Transmission Transposition", "Screensaver", "RSA Cipher", "Amusement Parks", "Literally Something", "Icon Reveal", "Solitaire Cipher", "hexOrbits", "Matchmaker", "Ladders", "Hearthur", "Decimation", "Color Punch", "Count to 69420", "Mssngv Wls", "Netherite", "Naming Conventions", "I'm Not a Robot", "Emoticon Math", "Coinage", "Simon Supports", "Identifrac", "1D Chess", "1-2-3-2-1", "Not Murder", "Not Morsematics", "Not Crazy Talk", "Not Coordinates", "Not Connection Check", "Not Colour Flash", "Cruel Colour Flash", "Numpath", "The Logan Parody Jukebox", "Factoring Maze", "Binary Buttons", "The Assorted Arrangement", "The Alteran Trail", "Pathfinder", "Needy Wires", "Turn Four", "nya~", "Llama, Llama, Alpaca", "Voltorb Flip", "Cruel Synesthesia", "Polygrid", "Mischmodul", "amogus", "Connect Four"};
    private string[] CurrentData = { };
    private static Dictionary<string, Texture2D> LoadedTextures = new Dictionary<string, Texture2D>();

    private string SouvYes = "";
    private List<string> SouvNo = new List<string>{};
	private List<string> SouvParts = new List<string>{"Question", "Top-left answer", "Top-right answer", "Bottom-left answer", "Bottom-right answer"}; //Can definitely be optimized
    private bool SouvReady = false; //Souvenir will ask once this is true.
	private bool SouvStop = false; //Souvenir will NOT ask if this is true because there's multiple Iconics.

    //Logging
    static int ModuleIdCounter = 1;
    int ModuleId;
    private bool ModuleSolved;

    void Awake () {
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
    }

    // Use this for initialization
    void Start () {
        if (IgnoredModules == null) {
            IgnoredModules = Boss.GetIgnoredModules("Iconic", new string[]{
                "14", "Bamboozling Time Keeper", "Brainf---", "Forget Enigma", "Forget Everything", "Forget It Not", "Forget Me Later", "Forget Me Not", "Forget Perspective", "Forget The Colors", "Forget Them All", "Forget This", "Forget Us Not", "Iconic", "Kugelblitz", "Multitask", "Organization", "Password Destroyer", "Purgatory", "RPS Judging", "Simon Forgets", "Simon's Stages", "Souvenir", "Tallordered Keys", "The Time Keeper", "The Troll", "The Twin", "The Very Annoying Button", "Timing is Everything", "Turn The Key", "Ultimate Custom Night", "Übermodule"
            });
        }

        Module.OnActivate += delegate () {
            NonBosses = Bomb.GetSolvableModuleNames().Where(a => !IgnoredModules.Contains(a)).ToList().Count;
        };

		for (int i = 0; i < Bomb.GetModuleNames().Count(); i++) {
            if (Bomb.GetModuleNames()[i] == "Iconic") {
                NumberOfIconics += 1;
            } else {
                continue;
            }
        }
		if (NumberOfIconics > 1) {
			SouvStop = true;
		}

        AddNeedies();

        StartCoroutine(Delay());
	}

    private IEnumerator Delay () {
        yield return new WaitForSeconds(1f);
        ModuleReady = true;
		Debug.LogFormat("[Iconic #{0}] Iconic's starting process complete.", ModuleId);
    }

	// Update is called once per frame
	void Update () {
        if (ModuleSolved == false) {
            Solves = Bomb.GetSolvedModuleNames().Count();
            if (Solves > SolveList.Count()) {
                MostRecent = GetLatestSolve(Bomb.GetSolvedModuleNames(), SolveList);
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
                for (int i = 0; i < ModuleList.Count(); i++) {
                    if (ModuleList[i] == Queue[0] && FoundAModule == false) {
                        CurrentData = NameToData(ModuleList[i]).ToArray();
                        if (!LoadedTextures.ContainsKey(ModuleList[i]))
                        {
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
                            Color[] loadedPixels = Modules[index].GetPixels(x, y, 32, 32);
                            Texture2D loadedTexture = new Texture2D(32, 32)
                            {
                                filterMode = FilterMode.Point
                            };
                            loadedTexture.SetPixels(loadedPixels);
                            loadedTexture.Apply();
                            LoadedTextures.Add(ModuleList[i], loadedTexture);
                        }
                        TheIcon.material.mainTexture = LoadedTextures[ModuleList[i]];
                        CurrentModule = ModuleList[i];
                        FoundAModule = true;
                    }
                }
                if (FoundAModule == false) {
                    Debug.LogFormat("[Iconic #{0}] Adding blank because I don't recognize the following module: {1}", ModuleId, Queue[0]);
                    TheIcon.material.mainTexture = Blank;
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
                        "The string for " + Queue[0] + "is not 1024 characters long. (It's " + length + "characters long)",
                        "The string array for " + Queue[0] + "doesn't have enough arguments. (There's only " + CurrentData.Length +" strings.)" //For some reason this doesn't work...
                    };
                    Debug.LogFormat("[Iconic #{0}] {1} Autosolving the module.", ModuleId, messages[index]);
                    Phrase.text = Queue[0] + " error!";
                    ModuleSolved = true;
                    Module.HandlePass();
                    return;
                }

                Phrase.text = ModulePart;

                if (ModulePart.Length > 13) {
                    Phrase.transform.localScale = new Vector3(0.00025f, 0.001f, 0.01f);
                }   else if (ModulePart.Length > 7) {
                    Phrase.transform.localScale = new Vector3(0.0005f, 0.001f, 0.01f);
                }

                if (Bomb.GetSolvableModuleNames().Contains("Souvenir") && !SouvReady) {
                    if (SouvYes == "") {
                        SouvYes = ModulePart;
                    } else {
						if (SouvNo.Contains(ModulePart) || ModulePart == SouvYes) {
							goto Skip;
						}
                        SouvNo.Add(ModulePart);
						Skip:
						if (IgnoredsAdded) {
							for (int y = 0; y < SouvParts.Count(); y++) {
								if (SouvParts[y] != SouvYes && !SouvNo.Contains(SouvParts[y]) && SouvNo.Count != 3) {
									SouvNo.Add(SouvParts[y]);
								}
							}
						}
						if (SouvNo.Count() == 3) {
							SouvReady = true;
							Debug.LogFormat("<Iconic #{0}> Giving Souvenir Answers: {1} | {2} / {3} / {4}", ModuleId, SouvYes, SouvNo[0], SouvNo[1], SouvNo[2]);
						}
						else if (SouvNo.Count() > 3) {
							Debug.LogFormat("[Iconic #{0}] Something went wrong in Souvenir wrong answer generation. Please contact Blananas2.", ModuleId);
						}
                    }
                }

            }
            if ((SolveList.Count() - IgnoredSolved == NonBosses && Queue.Count() == 0) && ModuleReady) {
                Phrase.text = "GG!";
                Audio.PlaySoundAtTransform("GoodGame", transform);
                Debug.LogFormat("[Iconic #{0}] All icons shown, Module solved.", ModuleId);
                TheIcon.material.mainTexture = Banana;
                Module.HandlePass();
                ModuleSolved = true;
            } else if (SolveList.Count() - IgnoredSolved == NonBosses && IgnoredsAdded == false) {
                AddIgnoreds();
            }
        }
	}

    void AddNeedies () {
        for (int i = 0; i < Bomb.GetModuleNames().Count(); i++) {
            if (Bomb.GetSolvableModuleNames().Contains(Bomb.GetModuleNames()[i])) {
                continue;
            } else {
                Queue.Add(Bomb.GetModuleNames()[i]);
            }
        }
	}

    void AddIgnoreds () {
        for (int i = 0; i < Bomb.GetModuleNames().Count(); i++) {
            if (IgnoredModules.Contains(Bomb.GetModuleNames()[i]) && Bomb.GetModuleNames()[i] != "Iconic") {
                Queue.Add(Bomb.GetModuleNames()[i]);
            } else {
                continue;
            }
        }

        NumberOfIconics -= 1;
        for (int i = 0; i < NumberOfIconics; i++) {
            Queue.Add("Iconic");
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
                        debugMessage = debugMessage.Replace("Correct", "Incorrect") + " Strike!";
                        Module.HandleStrike();
                    }
                    Debug.LogFormat(debugMessage);
                    Queue.RemoveAt(0);
                    TheIcon.material.mainTexture = Empty;
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

    private string[] NameToData(string s) {
        switch (s) {
            case "Wires": return iconicData._Wires;
            case "The Button": return iconicData._TheButton;
            case "Keypad": return iconicData._Keypad;
            case "Simon Says": return iconicData._SimonSays;
            case "Who's on First": return iconicData._WhosOnFirst;
            case "Memory": return iconicData._Memory;
            case "Morse Code": return iconicData._MorseCode;
            case "Complicated Wires": return iconicData._ComplicatedWires;
            case "Wire Sequence": return iconicData._WireSequence;
            case "Maze": return iconicData._Maze;
            case "Password": return iconicData._Password;
            case "Needy Vent Gas": return iconicData._VentingGas;
            case "Needy Capacitor": return iconicData._CapacitorDischarge;
            case "Needy Knob": return iconicData._Knob;
            case "Colour Flash": return iconicData._ColourFlash;
            case "Piano Keys": return iconicData._PianoKeys;
            case "Semaphore": return iconicData._Semaphore;
            case "Needy Math": return iconicData._Math;
            case "Emoji Math": return iconicData._EmojiMath;
            case "Lights Out": return iconicData._LightsOut;
            case "Switches": return iconicData._Switches;
            case "Two Bits": return iconicData._TwoBits;
            case "Word Scramble": return iconicData._WordScramble;
            case "Anagrams": return iconicData._Anagrams;
            case "Combination Lock": return iconicData._CombinationLock;
            case "Filibuster": return iconicData._Filibuster;
            case "Motion Sense": return iconicData._MotionSense;
            case "Square Button": return iconicData._SquareButton;
            case "Simon States": return iconicData._SimonStates;
            case "Round Keypad": return iconicData._RoundKeypad;
            case "Listening": return iconicData._Listening;
            case "Foreign Exchange Rates": return iconicData._ForeignExchangeRates;
            case "Answering Questions": return iconicData._AnsweringQuestions;
            case "Orientation Cube": return iconicData._OrientationCube;
            case "Morsematics": return iconicData._Morsematics;
            case "Connection Check": return iconicData._ConnectionCheck;
            case "Letter Keys": return iconicData._LetterKeys;
            case "Forget Me Not": return iconicData._ForgetMeNot;
            case "Rotary Phone": return iconicData._RotaryPhone;
            case "Astrology": return iconicData._Astrology;
            case "Logic": return iconicData._Logic;
            case "Crazy Talk": return iconicData._CrazyTalk;
            case "Adventure Game": return iconicData._AdventureGame;
            case "Turn The Key": return iconicData._TurnTheKey;
            case "Mystic Square": return iconicData._MysticSquare;
            case "Plumbing": return iconicData._Plumbing;
            case "Cruel Piano Keys": return iconicData._CruelPianoKeys;
            case "Safety Safe": return iconicData._SafetySafe;
            case "Tetris": return iconicData._Tetris;
            case "Cryptography": return iconicData._Cryptography;
            case "Chess": return iconicData._Chess;
            case "Turn The Keys": return iconicData._TurnTheKeys;
            case "Mouse In The Maze": return iconicData._MouseInTheMaze;
            case "3D Maze": return iconicData._3DMaze;
            case "Silly Slots": return iconicData._SillySlots;
            case "Number Pad": return iconicData._NumberPad;
            case "Laundry": return iconicData._Laundry;
            case "Probing": return iconicData._Probing;
            case "Resistors": return iconicData._Resistors;
            case "Skewed Slots": return iconicData._SkewedSlots;
            case "Caesar Cipher": return iconicData._CaesarCipher;
            case "Perspective Pegs": return iconicData._PerspectivePegs;
            case "Microcontroller": return iconicData._Microcontroller;
            case "Murder": return iconicData._Murder;
            case "The Gamepad": return iconicData._TheGamepad;
            case "Tic Tac Toe": return iconicData._TicTacToe;
            case "Who's That Monsplode": return iconicData._WhosThatMonsplode;
            case "Monsplode, Fight!": return iconicData._MonsplodeFight;
            case "Shape Shift": return iconicData._ShapeShift;
            case "Follow the Leader": return iconicData._FollowTheLeader;
            case "Friendship": return iconicData._Friendship;
            case "The Bulb": return iconicData._TheBulb;
            case "Alphabet": return iconicData._Alphabet;
            case "Blind Alley": return iconicData._BlindAlley;
            case "Sea Shells": return iconicData._SeaShells;
            case "English Test": return iconicData._EnglishTest;
            case "Rock-Paper-Scissors-L.-Sp.": return iconicData._RockPaperScissorsLizardSpock;
            case "Hexamaze": return iconicData._Hexamaze;
            case "Bitmaps": return iconicData._Bitmaps;
            case "Colored Squares": return iconicData._ColoredSquares;
            case "Adjacent Letters": return iconicData._AdjacentLetters;
            case "Third Base": return iconicData._ThirdBase;
            case "Souvenir": return iconicData._Souvenir;
            case "Word Search": return iconicData._WordSearch;
            case "Broken Buttons": return iconicData._BrokenButtons;
            case "Simon Screams": return iconicData._SimonScreams;
            case "Modules Against Humanity": return iconicData._ModulesAgainstHumanity;
            case "Complicated Buttons": return iconicData._ComplicatedButtons;
            case "Battleship": return iconicData._Battleship;
            case "Text Field": return iconicData._TextField;
            case "Symbolic Password": return iconicData._SymbolicPassword;
            case "Wire Placement": return iconicData._WirePlacement;
            case "Double-Oh": return iconicData._DoubleOh;
            case "Cheap Checkout": return iconicData._CheapCheckout;
            case "Coordinates": return iconicData._Coordinates;
            case "Light Cycle": return iconicData._LightCycle;
            case "HTTP Response": return iconicData._HTTPResponse;
            case "Rhythms": return iconicData._Rhythms;
            case "Color Math": return iconicData._ColorMath;
            case "Only Connect": return iconicData._OnlyConnect;
            case "Neutralization": return iconicData._Neutralization;
            case "Web Design": return iconicData._WebDesign;
            case "Chord Qualities": return iconicData._ChordQualities;
            case "Creation": return iconicData._Creation;
            case "Rubik's Cube": return iconicData._RubiksCube;
            case "FizzBuzz": return iconicData._FizzBuzz;
            case "The Clock": return iconicData._TheClock;
            case "LED Encryption": return iconicData._LEDEncryption;
            case "Edgework": return iconicData._Edgework;
            case "Bitwise Operations": return iconicData._BitwiseOperations;
            case "Fast Math": return iconicData._FastMath;
            case "Minesweeper": return iconicData._Minesweeper;
            case "Zoo": return iconicData._Zoo;
            case "Binary LEDs": return iconicData._BinaryLEDs;
            case "Boolean Venn Diagram": return iconicData._BooleanVennDiagram;
            case "Point of Order": return iconicData._PointOfOrder;
            case "Ice Cream": return iconicData._IceCream;
            case "Hex To Decimal": return iconicData._HexToDecimal;
            case "The Screw": return iconicData._TheScrew;
            case "Yahtzee": return iconicData._Yahtzee;
            case "X-Ray": return iconicData._XRay;
            case "QR Code": return iconicData._QRCode;
            case "Button Masher": return iconicData._ButtonMasher;
            case "Random Number Generator": return iconicData._RandomNumberGenerator;
            case "Color Morse": return iconicData._ColorMorse;
            case "Mastermind Simple": return iconicData._MastermindSimple;
            case "Mastermind Cruel": return iconicData._MastermindCruel;
            case "Gridlock": return iconicData._Gridlock;
            case "Big Circle": return iconicData._BigCircle;
            case "Morse-A-Maze": return iconicData._MorseAMaze;
            case "Colored Switches": return iconicData._ColoredSwitches;
            case "Perplexing Wires": return iconicData._PerplexingWires;
            case "Monsplode Trading Cards": return iconicData._MonsplodeTradingCards;
            case "Game of Life Simple": return iconicData._GameOfLifeSimple;
            case "Game of Life Cruel": return iconicData._GameOfLifeCruel;
            case "Nonogram": return iconicData._Nonogram;
            case "S.E.T.": return iconicData._SET;
            case "Needy Beer Refill Mod": return iconicData._RefillThatBeer;
            case "Painting": return iconicData._Painting;
            case "Color Generator": return iconicData._ColorGenerator;
            case "Shape Memory": return iconicData._ShapeMemory;
            case "Symbol Cycle": return iconicData._SymbolCycle;
            case "Hunting": return iconicData._Hunting;
            case "Extended Password": return iconicData._ExtendedPassword;
            case "Curriculum": return iconicData._Curriculum;
            case "Braille": return iconicData._Braille;
            case "Mafia": return iconicData._Mafia;
            case "Festive Piano Keys": return iconicData._FestivePianoKeys;
            case "Flags": return iconicData._Flags;
            case "Timezone": return iconicData._Timezone;
            case "Polyhedral Maze": return iconicData._PolyhedralMaze;
            case "Symbolic Coordinates": return iconicData._SymbolicCoordinates;
            case "Poker": return iconicData._Poker;
            case "Sonic the Hedgehog": return iconicData._SonicTheHedgehog;
            case "Poetry": return iconicData._Poetry;
            case "Button Sequence": return iconicData._ButtonSequence;
            case "Algebra": return iconicData._Algebra;
            case "Visual Impairment": return iconicData._VisualImpairment;
            case "The Jukebox": return iconicData._TheJukebox;
            case "Identity Parade": return iconicData._IdentityParade;
            case "Maintenance": return iconicData._Maintenance;
            case "Blind Maze": return iconicData._BlindMaze;
            case "Backgrounds": return iconicData._Backgrounds;
            case "Mortal Kombat": return iconicData._MortalKombat;
            case "Mashematics": return iconicData._Mashematics;
            case "Faulty Backgrounds": return iconicData._FaultyBackgrounds;
            case "Radiator": return iconicData._Radiator;
            case "Modern Cipher": return iconicData._ModernCipher;
            case "LED Grid": return iconicData._LEDGrid;
            case "Sink": return iconicData._Sink;
            case "The iPhone": return iconicData._TheiPhone;
            case "The Swan": return iconicData._TheSwan;
            case "Waste Management": return iconicData._WasteManagement;
            case "Human Resources": return iconicData._HumanResources;
            case "Skyrim": return iconicData._Skyrim;
            case "Burglar Alarm": return iconicData._BurglarAlarm;
            case "Press X": return iconicData._PressX;
            case "European Travel": return iconicData._EuropeanTravel;
            case "Error Codes": return iconicData._ErrorCodes;
            case "Rapid Buttons": return iconicData._RapidButtons;
            case "LEGOs": return iconicData._LEGOs;
            case "Rubik’s Clock": return iconicData._RubiksClock;
            case "Font Select": return iconicData._FontSelect;
            case "The Stopwatch": return iconicData._TheStopwatch;
            case "Pie": return iconicData._Pie;
            case "The Wire": return iconicData._TheWire;
            case "The London Underground": return iconicData._TheLondonUnderground;
            case "Logic Gates": return iconicData._LogicGates;
            case "Forget Everything": return iconicData._ForgetEverything;
            case "Grid Matching": return iconicData._GridMatching;
            case "Color Decoding": return iconicData._ColorDecoding;
            case "The Sun": return iconicData._TheSun;
            case "Playfair Cipher": return iconicData._PlayfairCipher;
            case "Tangrams": return iconicData._Tangrams;
            case "The Number": return iconicData._TheNumber;
            case "Cooking": return iconicData._Cooking;
            case "Superlogic": return iconicData._Superlogic;
            case "The Moon": return iconicData._TheMoon;
            case "The Cube": return iconicData._TheCube;
            case "Dr. Doctor": return iconicData._DrDoctor;
            case "Tax Returns": return iconicData._TaxReturns;
            case "The Jewel Vault": return iconicData._TheJewelVault;
            case "Digital Root": return iconicData._DigitalRoot;
            case "Graffiti Numbers": return iconicData._GraffitiNumbers;
            case "Marble Tumble": return iconicData._MarbleTumble;
            case "X01": return iconicData._X01;
            case "Logical Buttons": return iconicData._LogicalButtons;
            case "The Code": return iconicData._TheCode;
            case "Tap Code": return iconicData._TapCode;
            case "Simon Sings": return iconicData._SimonSings;
            case "Simon Sends": return iconicData._SimonSends;
            case "Synonyms": return iconicData._Synonyms;
            case "Greek Calculus": return iconicData._GreekCalculus;
            case "Simon Shrieks": return iconicData._SimonShrieks;
            case "Complex Keypad": return iconicData._ComplexKeypad;
            case "Subways": return iconicData._Subways;
            case "Lasers": return iconicData._Lasers;
            case "Turtle Robot": return iconicData._TurtleRobot;
            case "Guitar Chords": return iconicData._GuitarChords;
            case "Calendar": return iconicData._Calendar;
            case "USA Maze": return iconicData._USAMaze;
            case "Binary Tree": return iconicData._BinaryTree;
            case "The Time Keeper": return iconicData._TheTimeKeeper;
            case "Lightspeed": return iconicData._Lightspeed;
            case "Black Hole": return iconicData._BlackHole;
            case "Simon's Star": return iconicData._SimonsStar;
            case "Morse War": return iconicData._MorseWar;
            case "The Stock Market": return iconicData._TheStockMarket;
            case "Mineseeker": return iconicData._Mineseeker;
            case "Maze Scrambler": return iconicData._MazeScrambler;
            case "The Number Cipher": return iconicData._TheNumberCipher;
            case "Alphabet Numbers": return iconicData._AlphabetNumbers;
            case "British Slang": return iconicData._BritishSlang;
            case "Double Color": return iconicData._DoubleColor;
            case "Maritime Flags": return iconicData._MaritimeFlags;
            case "Equations": return iconicData._Equations;
            case "Determinants": return iconicData._Determinants;
            case "Pattern Cube": return iconicData._PatternCube;
            case "Know Your Way": return iconicData._KnowYourWay;
            case "Splitting The Loot": return iconicData._SplittingTheLoot;
            case "Simon Samples": return iconicData._SimonSamples;
            case "Character Shift": return iconicData._CharacterShift;
            case "Uncolored Squares": return iconicData._UncoloredSquares;
            case "Dragon Energy": return iconicData._DragonEnergy;
            case "Flashing Lights": return iconicData._FlashingLights;
            case "3D Tunnels": return iconicData._3DTunnels;
            case "Synchronization": return iconicData._Synchronization;
            case "The Switch": return iconicData._TheSwitch;
            case "Reverse Morse": return iconicData._ReverseMorse;
            case "Manometers": return iconicData._Manometers;
            case "Shikaku": return iconicData._Shikaku;
            case "Wire Spaghetti": return iconicData._WireSpaghetti;
            case "Tennis": return iconicData._Tennis;
            case "Module Homework": return iconicData._ModuleHomework;
            case "Benedict Cumberbatch": return iconicData._BenedictCumberbatch;
            case "Signals": return iconicData._Signals;
            case "Horrible Memory": return iconicData._HorribleMemory;
            case "Boggle": return iconicData._Boggle;
            case "Command Prompt": return iconicData._CommandPrompt;
            case "Boolean Maze": return iconicData._BooleanMaze;
            case "Sonic & Knuckles": return iconicData._SonicKnuckles;
            case "Quintuples": return iconicData._Quintuples;
            case "The Sphere": return iconicData._TheSphere;
            case "Coffeebucks": return iconicData._Coffeebucks;
            case "Colorful Madness": return iconicData._ColorfulMadness;
            case "Bases": return iconicData._Bases;
            case "Lion's Share": return iconicData._LionsShare;
            case "Snooker": return iconicData._Snooker;
            case "Blackjack": return iconicData._Blackjack;
            case "Party Time": return iconicData._PartyTime;
            case "Accumulation": return iconicData._Accumulation;
            case "The Plunger Button": return iconicData._ThePlungerButton;
            case "The Digit": return iconicData._TheDigit;
            case "The Jack-O'-Lantern": return iconicData._TheJackOLantern;
            case "T-Words": return iconicData._TWords;
            case "Divided Squares": return iconicData._DividedSquares;
            case "Connection Device": return iconicData._ConnectionDevice;
            case "Instructions": return iconicData._Instructions;
            case "Valves": return iconicData._Valves;
            case "Encrypted Morse": return iconicData._EncryptedMorse;
            case "The Crystal Maze": return iconicData._TheCrystalMaze;
            case "Cruel Countdown": return iconicData._CruelCountdown;
            case "Countdown": return iconicData._Countdown;
            case "Catchphrase": return iconicData._Catchphrase;
            case "Blockbusters": return iconicData._Blockbusters;
            case "IKEA": return iconicData._IKEA;
            case "Retirement": return iconicData._Retirement;
            case "Periodic Table": return iconicData._PeriodicTable;
            case "101 Dalmatians": return iconicData._101Dalmatians;
            case "Schlag den Bomb": return iconicData._SchlagDenBomb;
            case "Mahjong": return iconicData._Mahjong;
            case "Kudosudoku": return iconicData._Kudosudoku;
            case "The Radio": return iconicData._TheRadio;
            case "Modulo": return iconicData._Modulo;
            case "Number Nimbleness": return iconicData._NumberNimbleness;
            case "Pay Respects": return iconicData._PayRespects;
            case "Challenge & Contact": return iconicData._ChallengeContact;
            case "The Triangle": return iconicData._TheTriangle;
            case "Sueet Wall": return iconicData._SueetWall;
            case "Hot Potato": return iconicData._HotPotato;
            case "Christmas Presents": return iconicData._ChristmasPresents;
            case "Hieroglyphics": return iconicData._Hieroglyphics;
            case "Functions": return iconicData._Functions;
            case "Scripting": return iconicData._Scripting;
            case "Needy Mrs Bob": return iconicData._NeedyMrsBob;
            case "Simon Spins": return iconicData._SimonSpins;
            case "Ten-Button Color Code": return iconicData._TenButtonColorCode;
            case "Cursed Double-Oh": return iconicData._CursedDoubleOh;
            case "Crackbox": return iconicData._Crackbox;
            case "Street Fighter": return iconicData._StreetFighter;
            case "The Labyrinth": return iconicData._TheLabyrinth;
            case "Spinning Buttons": return iconicData._SpinningButtons;
            case "Color Match": return iconicData._ColorMatch;
            case "The Festive Jukebox": return iconicData._TheFestiveJukebox;
            case "Skinny Wires": return iconicData._SkinnyWires;
            case "The Hangover": return iconicData._TheHangover;
            case "Factory Maze": return iconicData._FactoryMaze;
            case "Binary Puzzle": return iconicData._BinaryPuzzle;
            case "Broken Guitar Chords": return iconicData._BrokenGuitarChords;
            case "Regular Crazy Talk": return iconicData._RegularCrazyTalk;
            case "Hogwarts": return iconicData._Hogwarts;
            case "Dominoes": return iconicData._Dominoes;
            case "Simon Speaks": return iconicData._SimonSpeaks;
            case "Discolored Squares": return iconicData._DiscoloredSquares;
            case "Krazy Talk": return iconicData._KrazyTalk;
            case "Numbers": return iconicData._Numbers;
            case "Flip The Coin": return iconicData._FlipTheCoin;
            case "Varicolored Squares": return iconicData._VaricoloredSquares;
            case "Simon's Stages": return iconicData._SimonsStages;
            case "Free Parking": return iconicData._FreeParking;
            case "Cookie Jars": return iconicData._CookieJars;
            case "Alchemy": return iconicData._Alchemy;
            case "Zoni": return iconicData._Zoni;
            case "Simon Squawks": return iconicData._SimonSquawks;
            case "Unrelated Anagrams": return iconicData._UnrelatedAnagrams;
            case "Mad Memory": return iconicData._MadMemory;
            case "Bartending": return iconicData._Bartending;
            case "Question Mark": return iconicData._QuestionMark;
            case "Shapes And Bombs": return iconicData._ShapesAndBombs;
            case "Flavor Text EX": return iconicData._FlavorTextEX;
            case "Flavor Text": return iconicData._FlavorText;
            case "Decolored Squares": return iconicData._DecoloredSquares;
            case "Homophones": return iconicData._Homophones;
            case "DetoNATO": return iconicData._DetoNATO;
            case "Air Traffic Controller": return iconicData._AirTrafficController;
            case "SYNC-125 [3]": return iconicData._SYNC1253;
            case "Westeros": return iconicData._Westeros;
            case "Morse Identification": return iconicData._MorseIdentification;
            case "Pigpen Rotations": return iconicData._PigpenRotations;
            case "LED Math": return iconicData._LEDMath;
            case "Alphabetical Order": return iconicData._AlphabeticalOrder;
            case "Simon Sounds": return iconicData._SimonSounds;
            case "The Fidget Spinner": return iconicData._TheFidgetSpinner;
            case "Simon's Sequence": return iconicData._SimonsSequence;
            case "Simon Scrambles": return iconicData._SimonScrambles;
            case "Harmony Sequence": return iconicData._HarmonySequence;
            case "Unfair Cipher": return iconicData._UnfairCipher;
            case "Melody Sequencer": return iconicData._MelodySequencer;
            case "Colorful Insanity": return iconicData._ColorfulInsanity;
            case "Passport Control": return iconicData._PassportControl;
            case "Left and Right": return iconicData._LeftandRight;
            case "Gadgetron Vendor": return iconicData._GadgetronVendor;
            case "Wingdings": return iconicData._Wingdings;
            case "The Hexabutton": return iconicData._TheHexabutton;
            case "Genetic Sequence": return iconicData._GeneticSequence;
            case "Micro-Modules": return iconicData._MicroModules;
            case "Module Maze": return iconicData._ModuleMaze;
            case "Elder Futhark": return iconicData._ElderFuthark;
            case "Tasha Squeals": return iconicData._TashaSqueals;
            case "Forget This": return iconicData._ForgetThis;
            case "Digital Cipher": return iconicData._DigitalCipher;
            case "Subscribe to Pewdiepie": return iconicData._SubscribetoPewdiepie;
            case "Grocery Store": return iconicData._GroceryStore;
            case "Draw": return iconicData._Draw;
            case "Burger Alarm": return iconicData._BurgerAlarm;
            case "Purgatory": return iconicData._Purgatory;
            case "Mega Man 2": return iconicData._MegaMan2;
            case "Lombax Cubes": return iconicData._LombaxCubes;
            case "The Stare": return iconicData._TheStare;
            case "Graphic Memory": return iconicData._GraphicMemory;
            case "Quiz Buzz": return iconicData._QuizBuzz;
            case "Wavetapping": return iconicData._Wavetapping;
            case "The Hypercube": return iconicData._TheHypercube;
            case "Speak English": return iconicData._SpeakEnglish;
            case "Stack'em": return iconicData._Stackem;
            case "Seven Wires": return iconicData._SevenWires;
            case "Colored Keys": return iconicData._ColoredKeys;
            case "The Troll": return iconicData._TheTroll;
            case "Planets": return iconicData._Planets;
            case "The Necronomicon": return iconicData._TheNecronomicon;
            case "Four-Card Monte": return iconicData._FourCardMonte;
            case "aa": return iconicData._Aa;
            case "The Giant's Drink": return iconicData._TheGiantsDrink;
            case "Digit String": return iconicData._DigitString;
            case "Alpha": return iconicData._Alpha;
            case "Snap!": return iconicData._Snap;
            case "Hidden Colors": return iconicData._HiddenColors;
            case "Colour Code": return iconicData._ColourCode;
            case "Vexillology": return iconicData._Vexillology;
            case "Brush Strokes": return iconicData._BrushStrokes;
            case "Odd One Out": return iconicData._OddOneOut;
            case "The Triangle Button": return iconicData._TheTriangleButton;
            case "Mazematics": return iconicData._Mazematics;
            case "Equations X": return iconicData._EquationsX;
            case "Maze³": return iconicData._Maze3;
            case "Gryphons": return iconicData._Gryphons;
            case "Arithmelogic": return iconicData._Arithmelogic;
            case "Roman Art": return iconicData._RomanArt;
            case "Faulty Sink": return iconicData._FaultySink;
            case "Simon Stops": return iconicData._SimonStops;
            case "Morse Buttons": return iconicData._MorseButtons;
            case "Terraria Quiz": return iconicData._TerrariaQuiz;
            case "Baba Is Who?": return iconicData._BabaIsWho;
            case "Triangle Buttons": return iconicData._TriangleButtons;
            case "Simon Stores": return iconicData._SimonStores;
            case "Risky Wires": return iconicData._RiskyWires;
            case "Modulus Manipulation": return iconicData._ModulusManipulation;
            case "Daylight Directions": return iconicData._DaylightDirections;
            case "Cryptic Password": return iconicData._CrypticPassword;
            case "Stained Glass": return iconicData._StainedGlass;
            case "The Block": return iconicData._TheBlock;
            case "Bamboozling Button": return iconicData._BamboozlingButton;
            case "Insane Talk": return iconicData._InsaneTalk;
            case "Transmitted Morse": return iconicData._TransmittedMorse;
            case "A Mistake": return iconicData._AMistake;
            case "Red Arrows": return iconicData._RedArrows;
            case "Green Arrows": return iconicData._GreenArrows;
            case "Yellow Arrows": return iconicData._YellowArrows;
            case "Encrypted Values": return iconicData._EncryptedValues;
            case "Encrypted Equations": return iconicData._EncryptedEquations;
            case "Forget Them All": return iconicData._ForgetThemAll;
            case "Ordered Keys": return iconicData._OrderedKeys;
            case "Blue Arrows": return iconicData._BlueArrows;
            case "Sticky Notes": return iconicData._StickyNotes;
            case "Unordered Keys": return iconicData._UnorderedKeys;
            case "Orange Arrows": return iconicData._OrangeArrows;
            case "Hyperactive Numbers": return iconicData._HyperactiveNumbers;
            case "Reordered Keys": return iconicData._ReorderedKeys;
            case "Button Grid": return iconicData._ButtonGrid;
            case "Find The Date": return iconicData._FindTheDate;
            case "Misordered Keys": return iconicData._MisorderedKeys;
            case "The Matrix": return iconicData._TheMatrix;
            case "Purple Arrows": return iconicData._PurpleArrows;
            case "Bordered Keys": return iconicData._BorderedKeys;
            case "The Dealmaker": return iconicData._TheDealmaker;
            case "Seven Deadly Sins": return iconicData._SevenDeadlySins;
            case "The Ultracube": return iconicData._TheUltracube;
            case "Symbolic Colouring": return iconicData._SymbolicColouring;
            case "Recorded Keys": return iconicData._RecordedKeys;
            case "The Deck of Many Things": return iconicData._TheDeckofManyThings;
            case "Disordered Keys": return iconicData._DisorderedKeys;
            case "Character Codes": return iconicData._CharacterCodes;
            case "Raiding Temples": return iconicData._RaidingTemples;
            case "Bomb Diffusal": return iconicData._BombDiffusal;
            case "Tallordered Keys": return iconicData._TallorderedKeys;
            case "Pong": return iconicData._Pong;
            case "Ten Seconds": return iconicData._TenSeconds;
            case "Cruel Ten Seconds": return iconicData._CruelTenSeconds;
            case "Double Expert": return iconicData._DoubleExpert;
            case "Calculus": return iconicData._Calculus;
            case "Boolean Keypad": return iconicData._BooleanKeypad;
            case "Toon Enough": return iconicData._ToonEnough;
            case "Pictionary": return iconicData._Pictionary;
            case "Qwirkle": return iconicData._Qwirkle;
            case "Antichamber": return iconicData._Antichamber;
            case "Simon Simons": return iconicData._SimonSimons;
            case "Lucky Dice": return iconicData._LuckyDice;
            case "Forget Enigma": return iconicData._ForgetEnigma;
            case "Constellations": return iconicData._Constellations;
            case "Prime Checker": return iconicData._PrimeChecker;
            case "Cruel Digital Root": return iconicData._CruelDigitalRoot;
            case "Faulty Digital Root": return iconicData._FaultyDigitalRoot;
            case "Needy Crafting Table": return iconicData._TheCraftingTable;
            case "Boot Too Big": return iconicData._BootTooBig;
            case "Vigenère Cipher": return iconicData._VigenereCipher;
            case "Langton's Ant": return iconicData._LangtonsAnt;
            case "Old Fogey": return iconicData._OldFogey;
            case "Insanagrams": return iconicData._Insanagrams;
            case "Treasure Hunt": return iconicData._TreasureHunt;
            case "Snakes and Ladders": return iconicData._SnakesandLadders;
            case "Module Movements": return iconicData._ModuleMovements;
            case "Bamboozled Again": return iconicData._BamboozledAgain;
            case "Safety Square": return iconicData._SafetySquare;
            case "Roman Numerals": return iconicData._RomanNumerals;
            case "Colo(u)r Talk": return iconicData._ColourTalk;
            case "Annoying Arrows": return iconicData._AnnoyingArrows;
            case "Double Arrows": return iconicData._DoubleArrows;
            case "Boolean Wires": return iconicData._BooleanWires;
            case "Block Stacks": return iconicData._BlockStacks;
            case "Vectors": return iconicData._Vectors;
            case "Partial Derivatives": return iconicData._PartialDerivatives;
            case "Caesar Cycle": return iconicData._CaesarCycle;
            case "Needy Piano": return iconicData._NeedyPiano;
            case "Forget Us Not": return iconicData._ForgetUsNot;
            case "Affine Cycle": return iconicData._AffineCycle;
            case "Pigpen Cycle": return iconicData._PigpenCycle;
            case "Flower Patch": return iconicData._FlowerPatch;
            case "Playfair Cycle": return iconicData._PlayfairCycle;
            case "Jumble Cycle": return iconicData._JumbleCycle;
            case "Organization": return iconicData._Organization;
            case "Forget Perspective": return iconicData._ForgetPerspective;
            case "Alpha-Bits": return iconicData._AlphaBits;
            case "Jack Attack": return iconicData._JackAttack;
            case "Ultimate Cycle": return iconicData._UltimateCycle;
            case "Hill Cycle": return iconicData._HillCycle;
            case "Binary": return iconicData._Binary;
            case "Chord Progressions": return iconicData._ChordProgressions;
            case "Matchematics": return iconicData._Matchematics;
            case "Bob Barks": return iconicData._BobBarks;
            case "Simon's On First": return iconicData._SimonsOnFirst;
            case "Weird Al Yankovic": return iconicData._WeirdAlYankovic;
            case "Forget Me Now": return iconicData._ForgetMeNow;
            case "Simon Selects": return iconicData._SimonSelects;
            case "The Witness": return iconicData._TheWitness;
            case "Simon Literally Says": return iconicData._SimonLiterallySays;
            case "Cryptic Cycle": return iconicData._CrypticCycle;
            case "Bone Apple Tea": return iconicData._BoneAppleTea;
            case "Robot Programming": return iconicData._RobotProgramming;
            case "Masyu": return iconicData._Masyu;
            case "Hold Ups": return iconicData._HoldUps;
            case "Red Cipher": return iconicData._RedCipher;
            case "Flash Memory": return iconicData._FlashMemory;
            case "A-maze-ing Buttons": return iconicData._AmazeingButtons;
            case "Desert Bus": return iconicData._DesertBus;
            case "Orange Cipher": return iconicData._OrangeCipher;
            case "Common Sense": return iconicData._CommonSense;
            case "The Very Annoying Button": return iconicData._TheVeryAnnoyingButton;
            case "Unown Cipher": return iconicData._UnownCipher;
            case "Needy Flower Mash": return iconicData._NeedyFlowerMash;
            case "TetraVex": return iconicData._TetraVex;
            case "Meter": return iconicData._Meter;
            case "Timing is Everything": return iconicData._TimingIsEverything;
            case "The Modkit": return iconicData._TheModkit;
            case "The Rule": return iconicData._TheRule;
            case "Fruits": return iconicData._Fruits;
            case "Bamboozling Button Grid": return iconicData._BamboozlingButtonGrid;
            case "Footnotes": return iconicData._Footnotes;
            case "Lousy Chess": return iconicData._LousyChess;
            case "Module Listening": return iconicData._ModuleListening;
            case "Garfield Kart": return iconicData._GarfieldKart;
            case "Yellow Cipher": return iconicData._YellowCipher;
            case "Kooky Keypad": return iconicData._KookyKeypad;
            case "Green Cipher": return iconicData._GreenCipher;
            case "RGB Maze": return iconicData._RGBMaze;
            case "Blue Cipher": return iconicData._BlueCipher;
            case "The Legendre Symbol": return iconicData._TheLegendreSymbol;
            case "Keypad Lock": return iconicData._KeypadLock;
            case "Forget Me Later": return iconicData._ForgetMeLater;
            case "Übermodule": return iconicData._Ubermodule;
            case "Heraldry": return iconicData._Heraldry;
            case "Faulty RGB Maze": return iconicData._FaultyRGBMaze;
            case "Indigo Cipher": return iconicData._IndigoCipher;
            case "Violet Cipher": return iconicData._VioletCipher;
            case "Encryption Bingo": return iconicData._EncryptionBingo;
            case "Color Addition": return iconicData._ColorAddition;
            case "Chinese Counting": return iconicData._ChineseCounting;
            case "Tower of Hanoi": return iconicData._TowerofHanoi;
            case "Keypad Combinations": return iconicData._KeypadCombinations;
            case "UltraStores": return iconicData._UltraStores;
            case "Kanji": return iconicData._Kanji;
            case "Geometry Dash": return iconicData._GeometryDash;
            case "Ternary Converter": return iconicData._TernaryConverter;
            case "N&Ms": return iconicData._NMs;
            case "Eight Pages": return iconicData._EightPages;
            case "The Colored Maze": return iconicData._TheColoredMaze;
            case "White Cipher": return iconicData._WhiteCipher;
            case "Gray Cipher": return iconicData._GrayCipher;
            case "The Hyperlink": return iconicData._TheHyperlink;
            case "Black Cipher": return iconicData._BlackCipher;
            case "Loopover": return iconicData._Loopover;
            case "Divisible Numbers": return iconicData._DivisibleNumbers;
            case "Corners": return iconicData._Corners;
            case "The High Score": return iconicData._TheHighScore;
            case "Ingredients": return iconicData._Ingredients;
            case "Jenga": return iconicData._Jenga;
            case "Intervals": return iconicData._Intervals;
            case "Cruel Boolean Maze": return iconicData._CruelBooleanMaze;
            case "Cheep Checkout": return iconicData._CheepCheckout;
            case "Spelling Bee": return iconicData._SpellingBee;
            case "Memorable Buttons": return iconicData._MemorableButtons;
            case "Thinking Wires": return iconicData._ThinkingWires;
            case "Seven Choose Four": return iconicData._SevenChooseFour;
            case "Object Shows": return iconicData._ObjectShows;
            case "Lunchtime": return iconicData._Lunchtime;
            case "Natures": return iconicData._Natures;
            case "Neutrinos": return iconicData._Neutrinos;
            case "Scavenger Hunt": return iconicData._ScavengerHunt;
            case "Polygons": return iconicData._Polygons;
            case "Ultimate Cipher": return iconicData._UltimateCipher;
            case "Codenames": return iconicData._Codenames;
            case "Odd Mod Out": return iconicData._OddModOut;
            case "Logic Statement": return iconicData._LogicStatement;
            case "Blinkstop": return iconicData._Blinkstop;
            case "Ultimate Custom Night": return iconicData._UltimateCustomNight;
            case "Hinges": return iconicData._Hinges;
            case "Time Accumulation": return iconicData._TimeAccumulation;
            case "❖": return iconicData._nonverbalSimon;
            case "Forget It Not": return iconicData._ForgetItNot;
            case "egg": return iconicData._egg;
            case "BuzzFizz": return iconicData._BuzzFizz;
            case "Answering Can Be Fun": return iconicData._AnsweringCanBeFun;
            case "3x3 Grid": return iconicData._3x3Grid;
            case "15 Mystic Lights": return iconicData._15MysticLights;
            case "14": return iconicData._14;
            case "Rainbow Arrows": return iconicData._RainbowArrows;
            case "Time Signatures": return iconicData._TimeSignatures;
            case "Multi-Colored Switches": return iconicData._MultiColoredSwitches;
            case "Digital Dials": return iconicData._DigitalDials;
            case "Passcodes": return iconicData._Passcodes;
            case "Hereditary Base Notation": return iconicData._HereditaryBaseNotation;
            case "Lines of Code": return iconicData._LinesOfCode;
            case "The cRule": return iconicData._TheCRule;
            case "Prime Encryption": return iconicData._PrimeEncryption;
            case "Encrypted Dice": return iconicData._EncryptedDice;
            case "Colorful Dials": return iconicData._ColorfulDials;
            case "Naughty or Nice": return iconicData._NaughtyOrNice;
            case "Following Orders": return iconicData._FollowingOrders;
            case "Binary Grid": return iconicData._BinaryGrid;
            case "Matrices": return iconicData._Matrices;
            case "Cruel Keypad": return iconicData._CruelKeypad;
            case "The Black Page": return iconicData._TheBlackPage;
            case "64": return iconicData._64;
            case "Simon Forgets": return iconicData._SimonForgets;
            case "Greek Letter Grid": return iconicData._GreekLetterGrid;
            case "Bamboozling Time Keeper": return iconicData._BamboozlingTimeKeeper;
            case "Scalar Dials": return iconicData._ScalarDials;
            case "The World's Largest Button": return iconicData._TheWorldsLargestButton;
            case "Keywords": return iconicData._Keywords;
            case "State of Aggregation": return iconicData._StateofAggregation;
            case "Dreamcipher": return iconicData._Dreamcipher;
            case "Brainf---": return iconicData._Brainf;
            case "Rotating Squares": return iconicData._RotatingSquares;
            case "Red Light Green Light": return iconicData._RedLightGreenLight;
            case "Marco Polo": return iconicData._MarcoPolo;
            case "Hyperneedy": return iconicData._Hyperneedy;
            case "Echolocation": return iconicData._Echolocation;
            case "Boozleglyph Identification": return iconicData._BoozleglyphIdentification;
            case "Boxing": return iconicData._Boxing;
            case "Topsy Turvy": return iconicData._TopsyTurvy;
            case "Railway Cargo Loading": return iconicData._RailwayCargoLoading;
            case "Conditional Buttons": return iconicData._ConditionalButtons;
            case "ASCII Art": return iconicData._ASCIIArt;
            case "Semamorse": return iconicData._Semamorse;
            case "Hide and Seek": return iconicData._HideandSeek;
            case "Symbolic Tasha": return iconicData._SymbolicTasha;
            case "Alphabetical Ruling": return iconicData._AlphabeticalRuling;
            case "Microphone": return iconicData._Microphone;
            case "Widdershins": return iconicData._Widdershins;
            case "Lockpick Maze": return iconicData._LockpickMaze;
            case "Dimension Disruption": return iconicData._DimensionDisruption;
            case "V": return iconicData._V;
            case "Silhouettes": return iconicData._Silhouettes;
            case "A Message": return iconicData._AMessage;
            case "Alliances": return iconicData._Alliances;
            case "Dungeon": return iconicData._Dungeon;
            case "Unicode": return iconicData._Unicode;
            case "Password Generator": return iconicData._PasswordGenerator;
            case "Baccarat": return iconicData._Baccarat;
            case "Guess Who?": return iconicData._GuessWho;
            case "Reverse Alphabetize": return iconicData._ReverseAlphabetize;
            case "Alphabetize": return iconicData._Alphabetize;
            case "Gatekeeper": return iconicData._Gatekeeper;
            case "Light Bulbs": return iconicData._LightBulbs;
            case "1000 Words": return iconicData._1000Words;
            case "Five Letter Words": return iconicData._FiveLetterWords;
            case "Settlers of KTaNE": return iconicData._SettlersofKTaNE;
            case "The Hidden Value": return iconicData._TheHiddenValue;
            case "Red": return iconicData._Red;
            case "Blue": return iconicData._Blue;
            case "Directional Button": return iconicData._DirectionalButton;
            case "...?": return iconicData._dotDotDotQuestionMark;
            case "The Simpleton": return iconicData._TheSimpleton;
            case "Misery Squares": return iconicData._MiserySquares;
            case "Not Wiresword": return iconicData._NotWiresword;
            case "Not Wire Sequence": return iconicData._NotWireSequence;
            case "Not Who's on First": return iconicData._NotWhosOnFirst;
            case "Not Simaze": return iconicData._NotSimaze;
            case "Not Password": return iconicData._NotPassword;
            case "Not Morse Code": return iconicData._NotMorseCode;
            case "Not Memory": return iconicData._NotMemory;
            case "Not Maze": return iconicData._NotMaze;
            case "Not Keypad": return iconicData._NotKeypad;
            case "Not Complicated Wires": return iconicData._NotComplicatedWires;
            case "Not Capacitor Discharge": return iconicData._NotCapacitorDischarge;
            case "Not the Button": return iconicData._NotTheButton;
            case "Sequences": return iconicData._Sequences;
            case "Dungeon 2nd Floor": return iconicData._Dungeon2ndFloor;
            case "Wire Ordering": return iconicData._WireOrdering;
            case "Vcrcs": return iconicData._Vcrcs;
            case "Quaternions": return iconicData._Quaternions;
            case "Abstract Sequences": return iconicData._AbstractSequences;
            case "osu!": return iconicData._osu;
            case "Shifting Maze": return iconicData._ShiftingMaze;
            case "Sorting": return iconicData._Sorting;
            case "Role Reversal": return iconicData._RoleReversal;
            case "Placeholder Talk": return iconicData._PlaceholderTalk;
            case "Art Appreciation": return iconicData._ArtAppreciation;
            case "Shell Game": return iconicData._ShellGame;
            case "Pattern Lock": return iconicData._PatternLock;
            case "Quick Arithmetic": return iconicData._QuickArithmetic;
            case "Minecraft Cipher": return iconicData._MinecraftCipher;
            case "Cheat Checkout": return iconicData._CheatCheckout;
            case "The Samsung": return iconicData._TheSamsung;
            case "Forget The Colors": return iconicData._ForgetTheColors;
            case "Etterna": return iconicData._Etterna;
            case "Recolored Switches": return iconicData._RecoloredSwitches;
            case "Cruel Garfield Kart": return iconicData._CruelGarfieldKart;
            case "1D Maze": return iconicData._1DMaze;
            case "Reverse Polish Notation": return iconicData._ReversePolishNotation;
            case "Snowflakes": return iconicData._Snowflakes;
            case "Funny Numbers": return iconicData._FunnyNumbers;
            case "Exoplanets": return iconicData._Exoplanets;
            case "Simon Stages": return iconicData._SimonStages;
            case "Not Venting Gas": return iconicData._NotVentingGas;
            case "Forget Infinity": return iconicData._ForgetInfinity;
            case "Faulty Seven Segment Displays": return iconicData._FaultySevenSegmentDisplays;
            case "Stock Images": return iconicData._StockImages;
            case "Roger": return iconicData._Roger;
            case "Malfunctions": return iconicData._Malfunctions;
            case "Minecraft Parody": return iconicData._MinecraftParody;
            case "Shuffled Strings": return iconicData._ShuffledStrings;
            case "NumberWang": return iconicData._NumberWang;
            case "Minecraft Survival": return iconicData._MinecraftSurvival;
            case "RPS Judging": return iconicData._RPSJudging;
            case "Fencing": return iconicData._Fencing;
            case "Strike Solve": return iconicData._StrikeSolve;
            case "Uncolored Switches": return iconicData._UncoloredSwitches;
            case "The Twin": return iconicData._TheTwin;
            case "Name Changer": return iconicData._NameChanger;
            case "Just Numbers": return iconicData._JustNumbers;
            case "Lying Indicators": return iconicData._LyingIndicators;
            case "Flag Identification": return iconicData._FlagIdentification;
            case "Training Text": return iconicData._TrainingText;
            case "Wonder Cipher": return iconicData._WonderCipher;
            case "Caesar's Maths": return iconicData._CaesarsMaths;
            case "Triamonds": return iconicData._Triamonds;
            case "Stars": return iconicData._Stars;
            case "Button Order": return iconicData._ButtonOrder;
            case "Jukebox.WAV": return iconicData._JukeboxWAV;
            case "Elder Password": return iconicData._ElderPassword;
            case "Switching Maze": return iconicData._SwitchingMaze;
            case "Iconic": return iconicData._Iconic;
            case "Mystery Module": return iconicData._MysteryModule;
            case "Ladder Lottery": return iconicData._LadderLottery;
            case "Co-op Harmony Sequence": return iconicData._CoopHarmonySequence;
            case "Standard Crazy Talk": return iconicData._StandardCrazyTalk;
            case "Quote Crazy Talk End Quote": return iconicData._QuoteCrazyTalkEndQuote;
            case "Kilo Talk": return iconicData._KiloTalk;
            case "KayMazey Talk": return iconicData._KayMazeyTalk;
            case "Jaden Smith Talk": return iconicData._JadenSmithTalk;
            case "Deck Creating": return iconicData._DeckCreating;
            case "Crazy Talk With A K": return iconicData._CrazyTalkWithAK;
            case "BoozleTalk": return iconicData._BoozleTalk;
            case "Arrow Talk": return iconicData._ArrowTalk;
            case "Siffron": return iconicData._Siffron;
            case "Red Herring": return iconicData._RedHerring;
            case "Audio Morse": return iconicData._AudioMorse;
            case "Palindromes": return iconicData._Palindromes;
            case "Pow": return iconicData._Pow;
            case "Type Racer": return iconicData._TypeRacer;
            case "Chicken Nuggets": return iconicData._ChickenNuggets;
            case "Badugi": return iconicData._Badugi;
            case "Tetriamonds": return iconicData._Tetriamonds;
            case "Spot the Difference": return iconicData._SpotTheDifference;
            case "Negativity": return iconicData._Negativity;
            case "Masher The Bottun": return iconicData._MasherTheBottun;
            case "Yes and No": return iconicData._YesAndNo;
            case "M&Ns": return iconicData._MNs;
            case "Plant Identification": return iconicData._PlantIdentification;
            case "Integer Trees": return iconicData._IntegerTrees;
            case "Goofy's Game": return iconicData._GoofysGame;
            case "Module Rick": return iconicData._ModuleRick;
            case "Pickup Identification": return iconicData._PickupIdentification;
            case "Earthbound": return iconicData._Earthbound;
            case "3 LEDs": return iconicData._3LEDs;
            case "Life Iteration": return iconicData._LifeIteration;
            case "Thread the Needle": return iconicData._ThreadTheNeedle;
            case "Encrypted Hangman": return iconicData._EncryptedHangman;
            case "Accelerando": return iconicData._Accelerando;
            case "Reaction": return iconicData._Reaction;
            case "The Heart": return iconicData._TheHeart;
            case "Color Braille": return iconicData._ColorBraille;
            case "Remote Math": return iconicData._RemoteMath;
            case "Reflex": return iconicData._Reflex;
            case "Password Destroyer": return iconicData._PasswordDestroyer;
            case "Typing Tutor": return iconicData._TypingTutor;
            case "Multitask": return iconicData._Multitask;
            case "hexOS": return iconicData._hexOS;
            case "Simon Stashes": return iconicData._SimonStashes;
            case "Kyudoku": return iconicData._Kyudoku;
            case "Brawler Database": return iconicData._BrawlerDatabase;
            case "Shortcuts": return iconicData._Shortcuts;
            case "More Code": return iconicData._MoreCode;
            case "7": return iconicData._7;
            case "OmegaForget": return iconicData._OmegaForget;
            case "Needy Game of Life": return iconicData._NeedyGameOfLife;
            case "Mental Math": return iconicData._MentalMath;
            case "Kugelblitz": return iconicData._Kugelblitz;
            case "Dictation": return iconicData._Dictation;
            case "Bloxx": return iconicData._Bloxx;
            case "Basic Morse": return iconicData._BasicMorse;
            case "The Arena": return iconicData._TheArena;
            case "IPA": return iconicData._IPA;
            case "Emotiguy Identification": return iconicData._EmotiguyIdentification;
            case "100 Levels of Defusal": return iconicData._100LevelsOfDefusal;
            case "NeeDeez Nuts": return iconicData._NeeDeezNuts;
            case "Jailbreak": return iconicData._Jailbreak;
            case "Dumb Waiters": return iconicData._DumbWaiters;
            case "DACH Maze": return iconicData._DACHMaze;
            case "Birthdays": return iconicData._Birthdays;
            case "Match 'em": return iconicData._MatchEm;
            case "Navinums": return iconicData._Navinums;
            case "Gnomish Puzzle": return iconicData._GnomishPuzzle;
            case "RGB Logic": return iconicData._RGBLogic;
            case "Bridges": return iconicData._Bridges;
            case "A>N<D": return iconicData._ARightNLeftD;
            case "Shifted Maze": return iconicData._ShiftedMaze;
            case "Juxtacolored Squares": return iconicData._JuxtacoloredSquares;
            case "Wolf, Goat, and Cabbage": return iconicData._WolfGoatAndCabbage;
            case "The Missing Letter": return iconicData._TheMissingLetter;
            case "Amnesia": return iconicData._Amnesia;
            case "Plug-Ins": return iconicData._PlugIns;
            case "Synesthesia": return iconicData._Synesthesia;
            case "English Entries": return iconicData._EnglishEntries;
            case "The Duck": return iconicData._TheDuck;
            case "The Cruel Duck": return iconicData._TheCruelDuck;
            case "Identifying Soulless": return iconicData._IdentifyingSoulless;
            case "Ultimate Tic Tac Toe": return iconicData._UltimateTicTacToe;
            case "Factoring": return iconicData._Factoring;
            case "Lyrical Nonsense": return iconicData._LyricalNonsense;
            case "RGB Sequences": return iconicData._RGBSequences;
            case "Puzzword": return iconicData._Puzzword;
            case "NOT NOT": return iconicData._NOTNOT;
            case "Repo Selector": return iconicData._RepoSelector;
            case "int##": return iconicData._intHashHash;
            case "Deaf Alley": return iconicData._DeafAlley;
            case "Blind Arrows": return iconicData._BlindArrows;
            case "Sound Design": return iconicData._SoundDesign;
            case "RGB Arithmetic": return iconicData._RGBArithmetic;
            case "D-CODE": return iconicData._DCODE;
            case "Rapid Subtraction": return iconicData._RapidSubtraction;
            case "Fifteen": return iconicData._Fifteen;
            case "Pixel Cipher": return iconicData._PixelCipher;
            case "Don't Touch Anything": return iconicData._DontTouchAnything;
            case "The Great Void": return iconicData._TheGreatVoid;
            case "21": return iconicData._21;
            case "Prime Time": return iconicData._PrimeTime;
            case "Negation": return iconicData._Negation;
            case "The Calculator": return iconicData._TheCalculator;
            case "SixTen": return iconicData._SixTen;
            case "ASCII Maze": return iconicData._ASCIIMaze;
            case "Ultralogic": return iconicData._Ultralogic;
            case "Spangled Stars": return iconicData._SpangledStars;
            case "Busy Beaver": return iconicData._BusyBeaver;
            case "Digital Clock": return iconicData._DigitalClock;
            case "Cruel Match 'em": return iconicData._CruelMatchEm;
            case "Assembly Code": return iconicData._AssemblyCode;
            case "Simon's Ultimate Showdown": return iconicData._SimonsUltimateShowdown;
            case "Boomdas": return iconicData._Boomdas;
            case "Needlessly Complicated Button": return iconicData._NeedlesslyComplicatedButton;
            case "Color Numbers": return iconicData._ColorNumbers;
            case "Chinese Strokes": return iconicData._ChineseStrokes;
            case "Chalices": return iconicData._Chalices;
            case "Reversed Edgework": return iconicData._ReversedEdgework;
            case "Pixel Art": return iconicData._PixelArt;
            case "Faulty Accelerando": return iconicData._FaultyAccelerando;
            case "0": return iconicData._0;
            case "Pitch Perfect": return iconicData._PitchPerfect;
            case "Increasing Indices": return iconicData._IncreasingIndices;
            case "Faulty Binary": return iconicData._FaultyBinary;
            case "Cruel Binary": return iconicData._CruelBinary;
            case "Connected Monitors": return iconicData._ConnectedMonitors;
            case "Broken Binary": return iconicData._BrokenBinary;
            case "Totally Accurate Minecraft Simulator": return iconicData._TotallyAccurateMinecraftSimulator;
            case "Tell Me When": return iconicData._TellMeWhen;
            case "ReGret-B Filtering": return iconicData._ReGretBFiltering;
            case "D-CRYPT": return iconicData._DCRYPT;
            case "Color-Cycle Button": return iconicData._ColorCycleButton;
            case "Alien Filing Colors": return iconicData._AlienFilingColors;
            case "The Kanye Encounter": return iconicData._TheKanyeEncounter;
            case "Entry Number Four": return iconicData._EntryNumberFour;
            case "D-CIPHER": return iconicData._DCIPHER;
            case "501": return iconicData._501;
            case "42": return iconicData._42;
            case "Color One Two": return iconicData._ColorOneTwo;
            case "Toolmods": return iconicData._Toolmods;
            case "Spelling Buzzed": return iconicData._SpellingBuzzed;
            case "Burnout": return iconicData._Burnout;
            case "Brown Bricks": return iconicData._BrownBricks;
            case "Mystic Maze": return iconicData._MysticMaze;
            case "Chinese Zodiac": return iconicData._ChineseZodiac;
            case "Four Lights": return iconicData._FourLights;
            case "Duck, Duck, Goose": return iconicData._DuckDuckGoose;
            case "Not Knob": return iconicData._NotKnob;
            case "Working Title": return iconicData._WorkingTitle;
            case "Toolneedy": return iconicData._Toolneedy;
            case "One Links To All": return iconicData._OneLinksToAll;
            case "Rules": return iconicData._Rules;
            case "Tenpins": return iconicData._Tenpins;
            case "Double Listening": return iconicData._DoubleListening;
            case "Wack Game of Life": return iconicData._WackGameOfLife;
            case "Unfair's Revenge": return iconicData._UnfairsRevenge;
            case "Unfair's Cruel Revenge": return iconicData._UnfairsCruelRevenge;
            case "Mindlock": return iconicData._Mindlock;
            case "Golf": return iconicData._Golf;
            case "Regular Hexpressions": return iconicData._RegularHexpressions;
            case "Literally Nothing": return iconicData._LiterallyNothing;
            case "Colored Buttons": return iconicData._ColoredButtons;
            case "Censorship": return iconicData._Censorship;
            case "The Pentabutton": return iconicData._ThePentabutton;
            case "Mechanus Cipher": return iconicData._MechanusCipher;
            case "Digisibility": return iconicData._Digisibility;
            case "Breaktime": return iconicData._Breaktime;
            case "Space Invaders Extreme": return iconicData._SpaceInvadersExtreme;
            case "Mazery": return iconicData._Mazery;
            case "Kim's Game": return iconicData._KimsGame;
            case "Three Cryptic Steps": return iconicData._ThreeCrypticSteps;
            case "Popufur": return iconicData._Popufur;
            case "Tech Support": return iconicData._TechSupport;
            case "Space": return iconicData._Space;
            case "9-Ball": return iconicData._9Ball;
            case "Metamem": return iconicData._Metamem;
            case "M&Ms": return iconicData._MMs;
            case "The Console": return iconicData._TheConsole;
            case "Pocket Planes": return iconicData._PocketPlanes;
            case "Bridge": return iconicData._Bridge;
            case "Rotten Beans": return iconicData._RottenBeans;
            case "Long Beans": return iconicData._LongBeans;
            case "Jellybeans": return iconicData._Jellybeans;
            case "Cool Beans": return iconicData._CoolBeans;
            case "Beans": return iconicData._Beans;
            case "Beanboozled Again": return iconicData._BeanboozledAgain;
            case "The Dials": return iconicData._TheDials;
            case "Butterflies": return iconicData._Butterflies;
            case "Broken Karaoke": return iconicData._BrokenKaraoke;
            case "Chamber No. 5": return iconicData._ChamberNo5;
            case "Silenced Simon": return iconicData._SilencedSimon;
            case "Teal Arrows": return iconicData._TealArrows;
            case "Faulty Butterflies": return iconicData._FaultyButterflies;
            case "Keep Clicking": return iconicData._KeepClicking;
            case "Frankenstein's Indicator": return iconicData._FrankensteinsIndicator;
            case "16 Coins": return iconicData._16Coins;
            case "Sea Bear Attacks": return iconicData._SeaBearAttacks;
            case "Alphabet Tiles": return iconicData._AlphabetTiles;
            case "Literally Crying": return iconicData._LiterallyCrying;
            case "Double Pitch": return iconicData._DoublePitch;
            case "Devilish Eggs": return iconicData._DevilishEggs;
            case "h": return iconicData._h;
            case "Rune Match I": return iconicData._RuneMatchI;
            case "Rune Match II": return iconicData._RuneMatchII;
            case "Rune Match III": return iconicData._RuneMatchIII;
            case "Quick Time Events": return iconicData._QuickTimeEvents;
            case "Iñupiaq Numerals": return iconicData._InupiaqNumerals;
            case "The Bioscanner": return iconicData._TheBioscanner;
            case "Ars Goetia Identification": return iconicData._ArsGoetiaIdentification;
            case "Pixel Number Base": return iconicData._PixelNumberBase;
            case "Silo Authorization": return iconicData._SiloAuthorization;
            case "Gradually Watermelon": return iconicData._GraduallyWatermelon;
            case "Mastermind Restricted": return iconicData._MastermindRestricted;
            case "Logical Operators": return iconicData._LogicalOperators;
            case "Higher Or Lower": return iconicData._HigherOrLower;
            case "Even Or Odd": return iconicData._EvenOrOdd;
            case "Digital Grid": return iconicData._DigitalGrid;
            case "Reformed Role Reversal": return iconicData._ReformedRoleReversal;
            case "Whiteout": return iconicData._Whiteout;
            case "N&Ns": return iconicData._NNs;
            case "Gettin' Funky": return iconicData._GettinFunky;
            case "Cell Lab": return iconicData._CellLab;
            case "Lights On": return iconicData._LightsOn;
            case "Color Hexagons": return iconicData._ColorHexagons;
            case "Commuting": return iconicData._Commuting;
            case "Symmetries Of A Square": return iconicData._SymmetriesOfASquare;
            case "Look and Say": return iconicData._LookAndSay;
            case "Currents": return iconicData._Currents;
            case "Partitions": return iconicData._Partitions;
            case "Telepathy": return iconicData._Telepathy;
            case "Cruel Stars": return iconicData._CruelStars;
            case "Button Messer": return iconicData._ButtonMesser;
            case "Taco Tuesday": return iconicData._TacoTuesday;
            case "The Overflow": return iconicData._TheOverflow;
            case "Nomai": return iconicData._Nomai;
            case "Forget Any Color": return iconicData._ForgetAnyColor;
            case "Table Madness": return iconicData._TableMadness;
            case "Melodic Message": return iconicData._MelodicMessage;
            case "Sugar Skulls": return iconicData._SugarSkulls;
            case "Colour Catch": return iconicData._ColourCatch;
            case "Cosmic": return iconicData._Cosmic;
            case "Semabols": return iconicData._Semabols;
            case "Mislocation": return iconicData._Mislocation;
            case "Musher The Batten": return iconicData._MusherTheBatten;
            case "Tribal Council": return iconicData._TribalCouncil;
            case "Simon Smiles": return iconicData._SimonSmiles;
            case "Outrageous": return iconicData._Outrageous;
            case "Faulty Chinese Counting": return iconicData._FaultyChineseCounting;
            case "Press The Shape": return iconicData._PressTheShape;
            case "Baybayin Words": return iconicData._BaybayinWords;
            case "OmegaDestroyer": return iconicData._OmegaDestroyer;
            case "Going Backwards": return iconicData._GoingBackwards;
            case "Atbash Cipher": return iconicData._AtbashCipher;
            case "Venn Diagrams": return iconicData._VennDiagrams;
            case "Numbered Buttons": return iconicData._NumberedButtons;
            case "Colored Hexabuttons": return iconicData._ColoredHexabuttons;
            case "Video Poker": return iconicData._VideoPoker;
            case "White Arrows": return iconicData._WhiteArrows;
            case "Johnson Solids": return iconicData._JohnsonSolids;
            case "Bottom Gear": return iconicData._BottomGear;
            case "Two Persuasive Buttons": return iconicData._TwoPersuasiveButtons;
            case "Keypad Directionality": return iconicData._KeypadDirectionality;
            case "% Grey": return iconicData._Grey;
            case "Towers": return iconicData._Towers;
            case "Letter Layers": return iconicData._LetterLayers;
            case "The Exploding Pen": return iconicData._TheExplodingPen;
            case "Snack Attack": return iconicData._SnackAttack;
            case "ReGrettaBle Relay": return iconicData._ReGrettaBleRelay;
            case "Security Council": return iconicData._SecurityCouncil;
            case "Standard Button Masher": return iconicData._StandardButtonMasher;
            case "Musical Transposition": return iconicData._MusicalTransposition;
            case "Jackbox.TV": return iconicData._JackboxTV;
            case "The Furloid Jukebox": return iconicData._TheFurloidJukebox;
            case "The Close Button": return iconicData._TheCloseButton;
            case "Updog": return iconicData._Updog;
            case "Saimoe Pad": return iconicData._SaimoePad;
            case "B-Machine": return iconicData._BMachine;
            case "Addition": return iconicData._Addition;
            case "What’s on Second": return iconicData._WhatsOnSecond;
            case "Quaver": return iconicData._Quaver;
            case "Another Keypad Module": return iconicData._AnotherKeypadModule;
            case "Think Fast": return iconicData._ThinkFast;
            case "Shoddy Chess": return iconicData._ShoddyChess;
            case "Rhythm Test": return iconicData._RhythmTest;
            case "Validation": return iconicData._Validation;
            case "Floor Lights": return iconicData._FloorLights;
            case "Bad Wording": return iconicData._BadWording;
            case "Etch-A-Sketch": return iconicData._EtchASketch;
            case "Zener Cards": return iconicData._ZenerCards;
            case "Diophantine Equations": return iconicData._DiophantineEquations;
            case "Ternary Tiles": return iconicData._TernaryTiles;
            case "Striped Keys": return iconicData._StripedKeys;
            case "Rullo": return iconicData._Rullo;
            case "Flashing Arrows": return iconicData._FlashingArrows;
            case "Cruello": return iconicData._Cruello;
            case "Coloured Arrows": return iconicData._ColouredArrows;
            case "Black Arrows": return iconicData._BlackArrows;
            case "Tetris Sprint": return iconicData._TetrisSprint;
            case "Forget Maze Not": return iconicData._ForgetMazeNot;
            case "Double Screen": return iconicData._DoubleScreen;
            case "The Sequencyclopedia": return iconicData._TheSequencyclopedia;
            case "eeB gnillepS": return iconicData._eeBgnillepS;
            case "Pandemonium Cipher": return iconicData._PandemoniumCipher;
            case "Mystery Widget": return iconicData._MysteryWidget;
            case "Mineswapper": return iconicData._Minesweeper;
            case "Phosphorescence": return iconicData._Phosphorescence;
            case "The Klaxon": return iconicData._TheKlaxon;
            case "Valued Keys": return iconicData._ValuedKeys;
            case "Numerical Knight Movement": return iconicData._NumericalKnightMovement;
            case "Bandboozled Again": return iconicData._BandboozledAgain;
            case "SpriteClub Betting Simulation": return iconicData._SpriteClubBettingSimulation;
            case "Ramboozled Again": return iconicData._RamboozledAgain;
            case "Simon Subdivides": return iconicData._SimonSubdivides;
            case "Hole in One": return iconicData._HoleInOne;
            case "Hexiom": return iconicData._Hexiom;
            case "Collapse": return iconicData._Collapse;
            case "Back Buttons": return iconicData._BackButtons;
            case "Audio Keypad": return iconicData._AudioKeypad;
            case "Saimoe Maze": return iconicData._SaimoeMaze;
            case "Kidney Beans": return iconicData._KidneyBeans;
            case "Fake Beans": return iconicData._FakeBeans;
            case "Chilli Beans": return iconicData._ChilliBeans;
            case "Big Bean": return iconicData._BigBean;
            case "Bean Sprouts": return iconicData._BeanSprouts;
            case "Tell Me Why": return iconicData._TellMeWhy;
            case "Quiplash": return iconicData._Quiplash;
            case "Bowling": return iconicData._Bowling;
            case "Sporadic Segments": return iconicData._SporadicSegments;
            case "Linq": return iconicData._Linq;
            case "Entry Number One": return iconicData._EntryNumberOne;
            case "DNA Mutation": return iconicData._DNAMutation;
            case "RGB Hypermaze": return iconicData._RGBHypermaze;
            case "Boob Tube": return iconicData._BoobTube;
            case "Regular Sudoku": return iconicData._RegularSudoku;
            case "AAAAA": return iconicData._AAAAA;
            case "Polyrhythms": return iconicData._Polyrhythms;
            case "Drive-In Window": return iconicData._DriveInWindow;
            case "The 12 Days of Christmas": return iconicData._The12DaysOfChristmas;
            case "X": return iconicData._X;
            case "Y": return iconicData._Y;
            case "The Xenocryst": return iconicData._TheXenocryst;
            case "Rebooting M-OS": return iconicData._RebootingMOS;
            case "Stacked Sequences": return iconicData._StackedSequences;
            case "Complexity": return iconicData._Complexity;
            case "Small Circle": return iconicData._SmallCircle;
            case "Fractal Maze": return iconicData._FractalMaze;
            case "Simon Stumbles": return iconicData._SimonStumbles;
            case "Wild Side": return iconicData._WildSide;
            case "The Octadecayotton": return iconicData._TheOctadecayotton;
            case "Colored Letters": return iconicData._ColoredLetters;
            case "Kahoot!": return iconicData._Kahoot;
            case "Forget's Ultimate Showdown": return iconicData._ForgetsUltimateShowdown;
            case "Bomb Corp. Filing": return iconicData._BombCorpFiling;
            case "Ultra Digital Root": return iconicData._UltraDigitalRoot;
            case "Mii Identification": return iconicData._MiiIdentification;
            case "81": return iconicData._81;
            case "Simon Swindles": return iconicData._SimonSwindles;
            case "Next In Line": return iconicData._NextInLine;
            case "Functional Mapping": return iconicData._FunctionalMapping;
            case "Stable Time Signatures": return iconicData._StableTimeSignatures;
            case "Keypad Maze": return iconicData._KeypadMaze;
            case "Astrological": return iconicData._Astrological;
            case "XmORse Code": return iconicData._XmORseCode;
            case "Corridors": return iconicData._Corridors;
            case "Decay": return iconicData._Decay;
            case "Crazy Speak": return iconicData._CrazySpeak;
            case "Large Password": return iconicData._LargePassword;
            case "Large Free Password": return iconicData._LargeFreePassword;
            case "Free Password": return iconicData._FreePassword;
            case "Not Timer": return iconicData._NotTimer;
            case "The Burnt": return iconicData._TheBurnt;
            case "Access Codes": return iconicData._AccessCodes;
            case "Cistercian Numbers": return iconicData._CistercianNumbers;
            case "Brown Cipher": return iconicData._BrownCipher;
            case "Code Cracker": return iconicData._CodeCracker;
            case "Indentation": return iconicData._Indentation;
            case "One-Line": return iconicData._OneLine;
            case "The Speaker": return iconicData._TheSpeaker;
            case "Interpunct": return iconicData._Interpunct;
            case "Double Knob": return iconicData._DoubleKnob;
            case "Name Codes": return iconicData._NameCodes;
            case "The 1, 2, 3 Game": return iconicData._The123Game;
            case "Papa's Pizzeria": return iconicData._PapasPizzeria;
            case "Keypad Magnified": return iconicData._KeypadMagnified;
            case "Hold On": return iconicData._HoldOn;
            case "Diffusion": return iconicData._Diffusion;
            case "Soy Beans": return iconicData._SoyBeans;
            case "The Shaker": return iconicData._TheShaker;
            case "Coffee Beans": return iconicData._CoffeeBeans;
            case "Newline": return iconicData._Newline;
            case "Letter Grid": return iconicData._LetterGrid;
            case "Ghost Movement": return iconicData._GhostMovement;
            case "+": return iconicData._plus;
            case "Transmission Transposition": return iconicData._TransmissionTransposition;
            case "Screensaver": return iconicData._Screensaver;
            case "RSA Cipher": return iconicData._RSACipher;
            case "Amusement Parks": return iconicData._AmusementParks;
            case "Literally Something": return iconicData._LiterallySomething;
            case "Icon Reveal": return iconicData._IconReveal;
            case "Solitaire Cipher": return iconicData._SolitaireCipher;
            case "hexOrbits": return iconicData._hexOrbits;
            case "Matchmaker": return iconicData._Matchmaker;
            case "Ladders": return iconicData._Ladders;
            case "Hearthur": return iconicData._Hearthur;
            case "Decimation": return iconicData._Decimation;
            case "Color Punch": return iconicData._ColorPunch;
            case "Count to 69420": return iconicData._CountTo69420;
            case "Mssngv Wls": return iconicData._MssngvWls;
            case "Netherite": return iconicData._Netherite;
            case "Naming Conventions": return iconicData._NamingConventions;
            case "I'm Not a Robot": return iconicData._ImNotARobot;
            case "Emoticon Math": return iconicData._EmoticonMath;
            case "Coinage": return iconicData._Coinage;
            case "Simon Supports": return iconicData._SimonSupports;
            case "Identifrac": return iconicData._Identifrac;
            case "1D Chess": return iconicData._1DChess;
            case "1-2-3-2-1": return iconicData._12321;
            case "Not Murder": return iconicData._NotMurder;
            case "Not Morsematics": return iconicData._NotMorsematics;
            case "Not Crazy Talk": return iconicData._NotCrazyTalk;
            case "Not Coordinates": return iconicData._NotCoordinates;
            case "Not Connection Check": return iconicData._NotConnectionCheck;
            case "Not Colour Flash": return iconicData._NotColourFlash;
            case "Cruel Colour Flash": return iconicData._CruelColourFlash;
            case "Numpath": return iconicData._Numpath;
            case "The Logan Parody Jukebox": return iconicData._TheLoganParodyJukebox;
            case "Factoring Maze": return iconicData._FactoringMaze;
            case "Binary Buttons": return iconicData._BinaryButtons;
            case "The Assorted Arrangement": return iconicData._TheAssortedArrangement;
            case "The Alteran Trail": return iconicData._TheAlteranTrail;
            case "Pathfinder": return iconicData._Pathfinder;
            case "Needy Wires": return iconicData._NeedyWires;
            case "Turn Four": return iconicData._TurnFour;
            case "nya~": return iconicData._nya;
            case "Llama, Llama, Alpaca": return iconicData._LlamaLlamaAlpaca;
            case "Voltorb Flip": return iconicData._VoltorbFlip;
            case "Cruel Synesthesia": return iconicData._CruelSynesthesia;
            case "Polygrid": return iconicData._Polygrid;
            case "Mischmodul": return iconicData._Mischmodul;
            case "amogus": return iconicData._amogus;
            case "Connect Four": return iconicData._ConnectFour;

            /*
            case "Colour Flash ES": return iconicData._ColourFlash_ES;
            case "Adjacent Letters (Russian)": return iconicData._AdjacentLetters_RU;

            //from this point on, reflection is required

            case "Big Button Translated": return iconicData.BlankModule;
            case "Who's on First Translated": return iconicData.BlankModule;
            case "Morse Code Translated": return iconicData.BlankModule;
            case "Passwords Translated": return iconicData.BlankModule;
            case "Vent Gas Translated": return iconicData.BlankModule;
            case "Colour Flash Translated": return iconicData.BlankModule;
            */

            default: return iconicData.BlankModule;
        }
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
