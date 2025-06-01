namespace FileStatisticsFilter.CommonLibrary
{
    public class SearchedFile
    {
        public DateTime CreatedTime { get; set; } = new DateTime();
        public string Directory { get; set; }
        public string Extension { get; set; }
        public string FileName => $"{FileNameWithoutExtension}{Extension}";
        public string FileNameWithoutExtension { get; set; }
        public string FullName => Path.Combine(Directory, $"{FileNameWithoutExtension}{Extension}");
        public bool IsReadOnly { get; set; }
        public DateTime ModifiedTime { get; set; }
        public long Size { get; set; }
        public string SizeReadable { get; }

        public SearchedFile(FileInfo file)
        {
            Directory = file.DirectoryName ?? "";
            FileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FullName);
            Extension = file.Extension;
            Size = file.Length;
            SizeReadable = GetReadableSize(Size);
            CreatedTime = file.CreationTime;
            ModifiedTime = file.LastWriteTime;
            IsReadOnly = file.IsReadOnly;
        }

        public SearchedFile(string csvLine, char delimiter = '\t')
        {
            try
            {
                var fields = csvLine.Split(delimiter);
                if (fields.Length < 7)
                {
                    throw new Exception($"Invalid format of line");
                }

                Directory = fields[0];
                FileNameWithoutExtension = fields[1];
                Extension = fields[2];

                if (long.TryParse(fields[3], out long size))
                {
                    Size = size;
                    SizeReadable = GetReadableSize(size);
                }
                else
                {
                    throw new Exception($"Couldn't parse the size of file");
                }

                if (DateTime.TryParse(fields[4], out var createdTime))
                {
                    CreatedTime = createdTime;
                }
                else
                {
                    throw new Exception("Couldn't parse CreatedTime");
                }

                if (DateTime.TryParse(fields[5], out var modifiedTime))
                {
                    ModifiedTime = modifiedTime;
                }
                else
                {
                    throw new Exception("Couldn't parse ModifiedTime");
                }

                IsReadOnly = string.Equals(fields[6], "true", StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error!\nSearchedFile constructor, line {csvLine}\n{e}");
                Directory = string.Empty;
                FileNameWithoutExtension = string.Empty;
                Extension = string.Empty;
                SizeReadable = string.Empty;
            }
        }

        public static string ToCsvHeaderLine(char delimiter = '\t')
        {
            return $"Directory{delimiter}" +
                   $"FileNameWithoutExtension{delimiter}" +
                   $"Extension{delimiter}" +
                   $"Size{delimiter}" +
                   $"CreatedTime{delimiter}" +
                   $"ModifiedTime{delimiter}" +
                   $"IsReadOnly{delimiter}";
        }

        public string ToCsvLine(char delimiter = '\t')
        {
            return $"{Directory}{delimiter}" +
                   $"{FileNameWithoutExtension}{delimiter}" +
                   $"{Extension}{delimiter}" +
                   $"{Size}{delimiter}" +
                   $"{CreatedTime}{delimiter}" +
                   $"{ModifiedTime}{delimiter}" +
                   $"{IsReadOnly}{delimiter}";
        }

        private static string GetReadableSize(long size)
        {
            string[] sizes = ["B", "KB", "MB", "GB", "TB"];  // TODO add more if needed
            double currentSize = size;
            int index = 0;

            while (currentSize > 1024 && index < sizes.Length - 1)
            {
                index++;
                currentSize /= 1024;
            }

            return $"{Math.Round(currentSize, 3)} {sizes[index]}";
        }
    }
}
