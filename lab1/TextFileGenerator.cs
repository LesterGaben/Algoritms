namespace lab1 {
    public class TextFileGenerator : IFileGenerator {
        private readonly Random random = new Random();

        public void GenerateBySize(string fileName, int megabytes, int minNum = 0, int maxNum = 1000000000) {
            using var writer = new StreamWriter(fileName, append: false);

            for (int i = 0; i % 10000 != 0 || !(new FileInfo(fileName).Length >= ByteConverter.MegabytesToBytes(megabytes)); i++) {
                writer.WriteLine(random.Next(minNum, maxNum));
            }
        }

        public void GenerateByLinesCount(string fileName, long linesCount, int minNum = 0, int maxNum = 1000000000) {
            using var writer = new StreamWriter(fileName, append: false);
            int currentCount = 0;

            while (currentCount++ != linesCount) {
                writer.WriteLine(random.Next(minNum, maxNum));
            }
        }
    }
}
