using System.Diagnostics;

namespace lab1 {
    internal class Program {
        private static string fileName = "";
        private static IFileGenerator fileGenerator = null!;
        private static IFileSorter fileSorter = null!;
        private const int linesCount = 100000000;

        public static void Main(string[] args) {
            Console.WriteLine("Classical Algorithm:");
            ClassicalAlgorithm();
            Console.WriteLine("-------------------------------");
            Console.WriteLine("Modified Algorithm:");
            ModifiedAlgorithm();
        }

        private static void ClassicalAlgorithm() {
            fileName = @"TextFile.txt";

            fileGenerator = new TextFileGenerator();
            fileGenerator.GenerateBySize(fileName, 10);
            Console.WriteLine("Text file was Generated");
            fileSorter = new TextFileSorter();
            Stopwatch stopWatch = Stopwatch.StartNew();
            fileSorter.Sort(fileName, out string sortedFileName);
            stopWatch.Stop();
            Console.WriteLine($"Sorted, file: {sortedFileName}, seconds: {stopWatch.Elapsed.TotalSeconds}");
        }

        private static void ModifiedAlgorithm() {
            fileName = @"BinaryFile.dat";

            fileGenerator = new BinaryFileGenerator();
            fileGenerator.GenerateByLinesCount(fileName, linesCount);
            Console.WriteLine("Binary file was generated");
            fileSorter = new BinaryFileSorter();
            Stopwatch stopWatch = Stopwatch.StartNew();
            ((BinaryFileSorter)fileSorter).SortParts(fileName, "sorted.dat", linesCount, linesCount / 8);
            fileSorter.Sort("sorted.dat", out string sortedFileName);
            stopWatch.Stop();
            Console.WriteLine($"Sorted, file: {sortedFileName}, seconds: {stopWatch.Elapsed.TotalSeconds}");
            FileWorker.ShowContent(sortedFileName, 100);
        }
    }
}
