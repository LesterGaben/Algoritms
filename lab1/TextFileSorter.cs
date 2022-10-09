namespace lab1 {
    public class TextFileSorter : IFileSorter {
        public void Sort(string fileName, out string sortedFileName, int helpFilesCount = 3) {
            if (helpFilesCount < 2)
                throw new ArgumentException(null, nameof(helpFilesCount));
            string[] BHelpFiles = Enumerable.Range(1, helpFilesCount).Select(i => $"B{i}.txt").ToArray();
            string[] CHelpFiles = Enumerable.Range(1, helpFilesCount).Select(i => $"C{i}.txt").ToArray();
            SplitFile(fileName, BHelpFiles);
            SortHelper(BHelpFiles, CHelpFiles, out sortedFileName);
        }

        private void SortHelper(string[] BHelpFiles, string[] CHelpFiles, out string fileName) {
            var readers = BHelpFiles.Select(f => new StreamReader(f)).ToList();
            readers.Where(r => r.EndOfStream).ToList().ForEach(r =>
            {
                r.Dispose();
                readers.Remove(r);
            });

            if (readers.Count == 1) {
                fileName = ((FileStream)readers.First().BaseStream).Name;
                return;
            }

            var writers = CHelpFiles.Select(f => new StreamWriter(f, append: false)).ToList();
            var currentWriter = writers.First();
            var currentReader = readers.First();
            var nums = new List<int>();
            var nextNums = new List<int>();
            var readerAndPrevNum = readers.ToDictionary(r => r, _ => int.MinValue);

            while (readers.Count != 0) {
                while (currentReader.EndOfStream) {
                    var readerToRemove = currentReader;
                    currentReader = readers.NextAfter(currentReader);
                    readers.Remove(readerToRemove);
                    readerAndPrevNum.Remove(readerToRemove);
                    readerToRemove.Dispose();
                    if (readers.Count == 0) {
                        break;
                    }
                }

                if (readers.Count == 0) {
                    nums.Sort();
                    foreach (int n in nums) {
                        currentWriter.WriteLine(n);
                    }

                    currentWriter = writers.NextAfter(currentWriter);
                    nextNums.Sort();
                    foreach (int n in nextNums) {
                        currentWriter.WriteLine(n);
                    }

                    break;
                }

                int num = int.Parse(currentReader.ReadLine()!);
                if (num >= readerAndPrevNum[currentReader]) {
                    nums.Add(num);
                    readerAndPrevNum[currentReader] = num;
                }
                else {
                    nextNums.Add(num);
                    readerAndPrevNum[currentReader] = num;
                    currentReader = readers.NextAfter(currentReader);
                }

                if (nextNums.Count >= readers.Count) {
                    nums.Sort();
                    foreach (int n in nums) {
                        currentWriter.WriteLine(n);
                    }

                    currentWriter = writers.NextAfter(currentWriter);
                    nums.Clear();
                    nums.AddRange(nextNums);
                    nextNums.Clear();
                }
            }

            readers.ForEach(r => r.Dispose());
            writers.ForEach(w => w.Dispose());
            SortHelper(CHelpFiles, BHelpFiles, out fileName);
        }


        private void SplitFile(string fileName, string[] BHelpFiles) {
            var writers = BHelpFiles.Select(f => new StreamWriter(f, append: false)).ToList();
            var currentWriter = writers.First();
            using var reader = new StreamReader(fileName);
            int previousNum = int.MinValue;

            while (!reader.EndOfStream) {
                int num = int.Parse(reader.ReadLine()!);
                if (num >= previousNum) {
                    currentWriter.WriteLine(num);
                }
                else {
                    currentWriter = writers.NextAfter(currentWriter);
                    currentWriter.WriteLine(num);
                }

                previousNum = num;
            }

            writers.ForEach(w => w.Dispose());
        }
    }
}
