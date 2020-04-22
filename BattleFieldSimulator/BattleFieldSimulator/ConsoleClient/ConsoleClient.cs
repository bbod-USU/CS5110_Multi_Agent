using System;
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
                    inputValid = ValidInput(input);
                }

                switch (input)
                {
                    case "-h":
                        PrintHelpMenu();
                        break;
                    case "-r":
                        _simRunner.RunSimulation("SimpleMap.json", "SimpleTroopFile.json");
                        break;
                    default:
                        return;
                }
            }
        }

        private void PrintHelpMenu()
        {
            throw new NotImplementedException();
        }

        private bool ValidInput(string input) => input == "-h" || input == "-r" || input == "-q";

        private void DisplayWelcome()
        {
            Console.WriteLine("Welcome to the battlefield simulator! \n \n " +
                              "Please make a selection: \n" +
                              "\t <-h> help menu \n" +
                              "\t <-r> run program \n" +
                              "\t <-q> exit program \n");
        }
    }
}