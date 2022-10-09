namespace lab1 {
    public interface IFileGenerator {
        void GenerateBySize(string fileName, int megabytes, int minNum = 0, int maxNum = 1000000000);
        void GenerateByLinesCount(string fileName, long linesCount, int minNum = 0, int maxNum = 1000000000);
    }
}
