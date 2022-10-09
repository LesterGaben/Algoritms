namespace lab1 {
    public interface IFileSorter {
        void Sort(string fileName, out string sortedFileName, int helpFilesCount = 3);
    }
}
