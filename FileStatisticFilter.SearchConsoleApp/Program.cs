namespace FileStatisticFilter.SearchConsoleApp
{
    using FileStatisticsFilter.CommonLibrary;

    public static class Program
    {
        private static string? InputArg { get; set; }
        private static string? OutputArg { get; set; }
        private static bool RecursiveArg { get; set; }  // TODO

        public static void Main(string[] args)
        {
            if (!ParseArguments(args)) { return; }

            if (InputArg == null || OutputArg == null) { return; }

            var files = new SearchedFiles(Directory.EnumerateFiles(InputArg).Select(f => new FileInfo(f)));

            //files.LoadFromCsv("ahoj.csv");
            files.SaveToCsv(new FileInfo(OutputArg));
        }

        private static bool ParseArguments(string[] args)
        {
            string message = "Invalid input arguments!\n";

            if (!args.Contains("--input") || !args.Contains("--output"))
            {
                Console.Error.WriteLine($"{message}Missing required argument \"--input\" and/or \"--output\"");
                return false;
            }

            // parse input
            int index = Array.IndexOf(args, "--input");
            try
            {
                InputArg = args[index + 1];
                if (!Directory.Exists(InputArg))
                {
                    throw new Exception();
                }
            }
            catch
            {
                Console.Error.WriteLine($"{message}Invalid argument \"--input\"");
                return false;
            }

            // parse output
            index = Array.IndexOf(args, "--output");
            try
            {
                OutputArg = args[index + 1];
            }
            catch
            {
                Console.Error.WriteLine($"{message}Invalid argument \"--output\"");
                return false;
            }

            // parsing recursive
            RecursiveArg = args.Contains("--recursive");

            return true;
        }
    }
}