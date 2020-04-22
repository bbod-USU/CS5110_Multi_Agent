using System;
using System.IO;
using System.Linq;
using BattleFieldSimulator.SimRunner;

namespace BattleFieldSimulator.ConsoleClient
{
    public class ConsoleClient
    {
        private ISimRunner _simRunner;
        
        public ConsoleClient(ISimRunner simRunner)
        {
            _simRunner = simRunner;
        }

        public void Run(string[] args)
        {
            while(true)
            {
                var inputValid = false;
                var input = "";
                while (!inputValid)
                {
                    DisplayWelcome();
                    input = Console.ReadLine();
                    inputValid = ValidInput(input.Split(' ').ToList()[0]);
                }

                var inString = input.Split(' ').ToList();
                switch (inString[0])
                {
                    case "-h":
                        PrintHelpMenu();
                        break;
                    case "-r":
                        _simRunner.RunSimulation(inString[1], inString[2], inString[3]);
                        break;
                    default:
                        return;
                }
            }
        }

        private void PrintHelpMenu()
        {
            Console.WriteLine($"<-r mapFileName.json TroopFileName.json> Will execute the program on the given files. \n" +
                              $"A properly formatted man and troop json file are required.  The files must be placed  \n" +
                              $"in their respective folder locations.  There are examples of what the files should \n" +
                              $"look like in there. \n \n" +
                              $"<-q> Will exit the program");
        }

        private bool ValidInput(string input) => input == "-h" || input == "-r" || input == "-q";

        private void DisplayWelcome()
        {
            Console.WriteLine("Welcome to the battlefield simulator! \n \n " +
                              "Please make a selection: \n" +
                              "\t <-h> help menu \n" +
                              "\t <-r mapFileName.json TroopFileName.json outFile.txt> run program \n" +
                              "\t <-q> exit program \n");
        }
    }
}