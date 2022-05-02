using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using ParseciniLibrary;

namespace Parsecini
{
    class Program
    {
        static void Main(string[] args)
        {
            string dirPath = Directory.GetCurrentDirectory();
            string themeFilepath = "";
            string inputDirectory = "";
            string outputDirectory = "";
            string headerFilepath = "";

            ProgramFlowParts programFlow = new ProgramFlowParts();
            InputBuilder INPUT = new InputBuilder(programFlow);
            UIParts UI = new UIParts(Console.WindowWidth);
            TestSupport TESTSUPPORT = new TestSupport();
            ThemeBuilder ThemeBuilder = new ThemeBuilder();


            TESTSUPPORT.WriteTestFile("test1", ".thm");
            TESTSUPPORT.WriteTestFile("test2", ".thm");
            TESTSUPPORT.WriteTestFile("gallagallagalla", ".thm");
            TESTSUPPORT.WriteTestFile("header1", ".hed");
            TESTSUPPORT.WriteTestFile("header2", ".hed");
            TESTSUPPORT.WriteTestFile("bendigo", ".hed");
            TESTSUPPORT.WriteTestDirectory("inputFolder");
            TESTSUPPORT.WriteTestDirectory("outputFolder");

            while (programFlow.CurrentPart.myType != FlowPart.FlowPartType.Quit)
            {
                string[] getDirectories = Directory.GetDirectories(dirPath);
                UI.UpdateWidth(Console.WindowWidth);
                switch (programFlow.CurrentPart.myType)
                {
                    case FlowPart.FlowPartType.MainMenu:
                        UI.DrawMainMenu();
                        UI.DrawSelectOption(string.IsNullOrWhiteSpace(themeFilepath), "Enter 1 to select the theme for your site.", $"Theme File set is: {themeFilepath}");
                        UI.DrawSelectOption(string.IsNullOrWhiteSpace(inputDirectory), "Enter 2 to select the input folder that holds the markdown files.", $"Input directory set is: {inputDirectory}");
                        UI.DrawSelectOption(string.IsNullOrWhiteSpace(outputDirectory), "Enter 3 to select the output folder that you want to save the HTML pages to.", $"Output directory set is: {outputDirectory}");
                        UI.DrawSelectOption(string.IsNullOrWhiteSpace(headerFilepath), "Enter 4 to select the header file template for your site (optional).", $"Header template File set is: {headerFilepath}");
                        UI.DrawSelectOption(true,
                            "Enter PROCESS to validate and process your templates!",
                            "Congratulations! Your template has finished processing.",
                            (string.IsNullOrWhiteSpace(themeFilepath)
                                || string.IsNullOrWhiteSpace(inputDirectory)
                                || string.IsNullOrWhiteSpace(outputDirectory)));
                        UI.DrawSelectOption(string.IsNullOrWhiteSpace(headerFilepath), "Enter 5 to quit.");

                        INPUT.MainMenuInput();
                        break;
                    case FlowPart.FlowPartType.SelectTheme:
                        string[] getTHMFiles = Directory.GetFiles(dirPath, "*.thm");

                        UI.DrawSelectTheme();
                        if (getTHMFiles.Length == 0)
                        {
                            UI.DrawSelectOption(true, "Enter 1 to return to the main menu");
                            INPUT.EmptySubMenuInput();
                        }
                        else
                        {
                            Array.Sort(getTHMFiles);   //not necessary since the order we output is the same as passed into the INPUT function, just looks nicer
                            int counter = 0;
                            foreach (string s in getTHMFiles)
                            {
                                counter++;
                                UI.DrawSelectOption(true, $"Enter {counter} for {s}");
                            }
                            UI.DrawSelectOption(true, $"Enter {counter + 1} to return to the main menu.");
                            themeFilepath = INPUT.CreateInputFromPathArray(getTHMFiles);
                        }
                        break;
                    case FlowPart.FlowPartType.SelectInputFolder:
                        UI.DrawSelectInput();
                        if (getDirectories.Length == 0)
                        {
                            UI.DrawSelectOption(true, "Enter 1 to return to the main menu");
                            INPUT.EmptySubMenuInput();
                        }
                        else
                        {
                            Array.Sort(getDirectories);   //not necessary since the order we output is the same as passed into the INPUT function, just looks nicer
                            int counter = 0;
                            foreach (string s in getDirectories)
                            {
                                counter++;
                                UI.DrawSelectOption(true, $"Enter {counter} for {s}");
                            }
                            UI.DrawSelectOption(true, $"Enter {counter + 1} to return to the main menu.");
                            inputDirectory = INPUT.CreateInputFromPathArray(getDirectories);
                        }
                        break;
                    case FlowPart.FlowPartType.SelectOutputFolder:
                        UI.DrawSelectOutput();
                        if (getDirectories.Length == 0)
                        {
                            UI.DrawSelectOption(true, "Enter 1 to return to the main menu");
                            INPUT.EmptySubMenuInput();
                        }
                        else
                        {
                            Array.Sort(getDirectories);   //not necessary since the order we output is the same as passed into the INPUT function, just looks nicer
                            int counter = 0;
                            foreach (string s in getDirectories)
                            {
                                counter++;
                                UI.DrawSelectOption(true, $"Enter {counter} for {s}");
                            }
                            UI.DrawSelectOption(true, $"Enter {counter + 1} to return to the main menu.");
                            outputDirectory = INPUT.CreateInputFromPathArray(getDirectories);
                        }
                        break;
                    case FlowPart.FlowPartType.SelectHeaderTemplate:
                        string[] getHEDFiles = Directory.GetFiles(dirPath, "*.hed");

                        UI.DrawSelectHeader();
                        if (getHEDFiles.Length == 0)
                        {
                            UI.DrawSelectOption(true, "Enter 1 to return to the main menu");
                            INPUT.EmptySubMenuInput();
                        }
                        else
                        {
                            Array.Sort(getHEDFiles);   //not necessary since the order we output is the same as passed into the INPUT function, just looks nicer
                            int counter = 0;
                            foreach (string s in getHEDFiles)
                            {
                                counter++;
                                UI.DrawSelectOption(true, $"Enter {counter} for {s}");
                            }
                            UI.DrawSelectOption(true, $"Enter {counter + 1} to return to the main menu.");
                            headerFilepath = INPUT.CreateInputFromPathArray(getHEDFiles);
                        }
                        break;

                    case FlowPart.FlowPartType.Processing:
                        break;

                }
            }
        }
    }


    class InputBuilder
    {
        ProgramFlowParts ProgramFlow;

        public InputBuilder(ProgramFlowParts _programFlow)
        {
            ProgramFlow = _programFlow;
        }

        public void MainMenuInput()
        {
            string getInput = Console.ReadLine();
            switch (getInput)
            {
                case "1":
                    ProgramFlow.FlowSwapToTheme(FlowPart.FlowPartType.SelectTheme);
                    break;
                case "2":
                    ProgramFlow.FlowSwapToTheme(FlowPart.FlowPartType.SelectInputFolder);
                    break;
                case "3":
                    ProgramFlow.FlowSwapToTheme(FlowPart.FlowPartType.SelectOutputFolder);
                    break;
                case "4":
                    ProgramFlow.FlowSwapToTheme(FlowPart.FlowPartType.SelectHeaderTemplate);
                    break;
                case "5":
                    ProgramFlow.FlowSwapToTheme(FlowPart.FlowPartType.Quit);
                    break;
                case "PROCESS":
                    ProgramFlow.FlowSwapToTheme(FlowPart.FlowPartType.Processing);
                    break;

            }
        }

        public void EmptySubMenuInput()
        {
            string getInput = Console.ReadLine();
            switch (getInput)
            {
                case "1":
                    ProgramFlow.FlowSwapToTheme(FlowPart.FlowPartType.MainMenu);
                    break;
            }
        }

        public string CreateInputFromPathArray(string[] pathArray)
        {
            string getInput = Console.ReadLine();
            int getNumber;

            bool success = int.TryParse(getInput, out getNumber);
            if (success)
            {
                if (getNumber <= pathArray.Length && getNumber > 0)
                {
                    ProgramFlow.FlowSwapToTheme(FlowPart.FlowPartType.MainMenu);
                    return pathArray[getNumber - 1];
                }
                if (getNumber == pathArray.Length + 1)
                {
                    ProgramFlow.FlowSwapToTheme(FlowPart.FlowPartType.MainMenu);
                    return "";
                }
            }
            return "";
        }
    }

    class TestSupport
    {
        public void WriteTestFile(string filename, string extension)
        {
            string path = Directory.GetCurrentDirectory() + "/" + filename + extension;

            try
            {
                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file.");
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void WriteTestDirectory(string directoryName)
        {
            string path = Directory.GetCurrentDirectory() + "/" + directoryName;

            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {
                    return;
                }

                DirectoryInfo di = Directory.CreateDirectory(path);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }

    class UIParts
    {
        private static ConsoleColor mainMenuTitleBackground = ConsoleColor.DarkMagenta;
        private static ConsoleColor subMenuTitleBackground = ConsoleColor.DarkBlue;
        private static ConsoleColor menuOptionIncompleteBackground = ConsoleColor.DarkGray;
        private static ConsoleColor menuOptionIncompleteForeground = ConsoleColor.White;
        private static ConsoleColor menuOptionCompletedBackground = ConsoleColor.Black;
        private static ConsoleColor menuOptionCompletedForeground = ConsoleColor.Green;
        private static ConsoleColor menuOptionCompletedForeground2 = ConsoleColor.Gray;
        private static ConsoleColor menuOptionDisabledBackground = ConsoleColor.DarkRed;
        private int width = 0;

        public UIParts(int _width)
        {
            width = _width;
        }

        public void UpdateWidth(int _width)
        {
            width = _width;
        }

        public void DrawMainMenu()
        {
            Console.Clear();
            Console.BackgroundColor = mainMenuTitleBackground;
            DrawColorText("|", '|');
            DrawColorText("Welcome to Parsecini, the lightweight cross-platform markdown parser for static blogging websites!");
            DrawColorText("|", '|');
            Console.BackgroundColor = subMenuTitleBackground;
            DrawColorText("MAIN MENU", '_');
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public void DrawSelectTheme()
        {
            Console.Clear();
            Console.BackgroundColor = mainMenuTitleBackground;
            DrawColorText("|", '|');
            DrawColorText("Please Select a .thm file from the current directory:");
            DrawColorText("|", '|');
            Console.BackgroundColor = subMenuTitleBackground;
            DrawColorText("Currently available themes:", '-');
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public void DrawSelectHeader()
        {
            Console.Clear();
            Console.BackgroundColor = mainMenuTitleBackground;
            DrawColorText("|", '|');
            DrawColorText("Please Select a .hed file from the current directory:");
            DrawColorText("|", '|');
            Console.BackgroundColor = subMenuTitleBackground;
            DrawColorText("Currently available themes:", '-');
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public void DrawSelectInput()
        {
            Console.Clear();
            Console.BackgroundColor = mainMenuTitleBackground;
            DrawColorText("|", '|');
            DrawColorText("Please Select the directory that holds your theme files:");
            DrawColorText("|", '|');
            Console.BackgroundColor = subMenuTitleBackground;
            DrawColorText("Currently available directories:", '-');
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public void DrawSelectOutput()
        {
            Console.Clear();
            Console.BackgroundColor = mainMenuTitleBackground;
            DrawColorText("|", '|');
            DrawColorText("Please Select the directory that you wish to output the generated HTML files to:");
            DrawColorText("|", '|');
            Console.BackgroundColor = subMenuTitleBackground;
            DrawColorText("Currently available directories:", '-');
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public void DrawSelectOption(bool notCompleted, string text, string completedText = "", bool isDisabled = false)
        {
            if (isDisabled)
            {
                Console.BackgroundColor = menuOptionDisabledBackground;
                Console.ForegroundColor = menuOptionIncompleteForeground;
                DrawColorText("Currently disabled, requirements not met.");
            }
            else if (notCompleted)
            {
                Console.BackgroundColor = menuOptionIncompleteBackground;
                Console.ForegroundColor = menuOptionIncompleteForeground;

                DrawColorText(text);
            }
            else
            {
                Console.BackgroundColor = menuOptionIncompleteBackground;
                Console.ForegroundColor = menuOptionCompletedForeground;
                DrawColorText(text, ' ');
                if (!string.IsNullOrWhiteSpace(completedText))
                {
                    Console.BackgroundColor = menuOptionCompletedBackground;
                    Console.ForegroundColor = menuOptionCompletedForeground2;
                    DrawColorText(completedText.Replace(Directory.GetCurrentDirectory(), string.Empty));
                }
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public void DrawColorText(string text, char paddingChar = ' ')
        {
            List<string> lines = BuildLines(text);

            foreach (string s in lines)
            {
                int widthDifference = width - s.Length;
                string holdS = s;

                if (s.Length < width)
                {
                    for (int i = 0; i < widthDifference / 2; i++)
                    {
                        holdS = paddingChar + holdS + paddingChar;
                    }

                    if (widthDifference % 2 != 0)
                    {
                        holdS = paddingChar + holdS;
                    }
                }
                Console.WriteLine(holdS);
            }
        }

        private List<string> BuildLines(string text)
        {
            List<string> subStrings = text.Split(' ').ToList();
            List<string> lines = new List<string>();

            foreach (string s in subStrings)
            {
                if (lines.Count == 0)
                {
                    lines.Add(s);
                }
                else if (lines.Last().Length + (s.Length + 1) > width) //we need to add +1 to account for the space we add
                {
                    lines.Add(s);
                }
                else
                {
                    lines[lines.Count - 1] = lines.Last() + " " + s;
                }
            }

            return lines;
        }
    }

    class ProgramFlowParts
    {
        private FlowPart MainMenu = new FlowPart(FlowPart.FlowPartType.MainMenu);
        private FlowPart SelectTheme = new FlowPart(FlowPart.FlowPartType.SelectTheme);
        private FlowPart SelectInputFolder = new FlowPart(FlowPart.FlowPartType.SelectInputFolder);
        private FlowPart SelectOutputFolder = new FlowPart(FlowPart.FlowPartType.SelectOutputFolder);
        private FlowPart SelectHeaderTemplate = new FlowPart(FlowPart.FlowPartType.SelectHeaderTemplate);
        private FlowPart Processing = new FlowPart(FlowPart.FlowPartType.Processing);
        private FlowPart Success = new FlowPart(FlowPart.FlowPartType.Success);
        private FlowPart Quit = new FlowPart(FlowPart.FlowPartType.Quit);

        public FlowPart CurrentPart;

        public ProgramFlowParts()
        {
            CurrentPart = MainMenu;
        }

        public void FlowSwapToTheme(FlowPart.FlowPartType flowPartType)
        {
            switch (flowPartType)
            {
                case FlowPart.FlowPartType.MainMenu:
                    CurrentPart = MainMenu;
                    break;
                case FlowPart.FlowPartType.SelectTheme:
                    CurrentPart = SelectTheme;
                    break;
                case FlowPart.FlowPartType.SelectInputFolder:
                    CurrentPart = SelectInputFolder;
                    break;
                case FlowPart.FlowPartType.SelectOutputFolder:
                    CurrentPart = SelectOutputFolder;
                    break;
                case FlowPart.FlowPartType.SelectHeaderTemplate:
                    CurrentPart = SelectHeaderTemplate;
                    break;
                case FlowPart.FlowPartType.Processing:
                    CurrentPart = Processing;
                    break;
                case FlowPart.FlowPartType.Success:
                    CurrentPart = Success;
                    break;
                case FlowPart.FlowPartType.Quit:
                    CurrentPart = Quit;
                    break;
            }
        }
    }

    class FlowPart
    {
        public enum FlowPartType { MainMenu, SelectTheme, SelectInputFolder, SelectOutputFolder, SelectHeaderTemplate, Processing, Success, Quit };
        public FlowPartType myType;

        public FlowPart(FlowPartType setType)
        {
            myType = setType;
        }
    }
}

