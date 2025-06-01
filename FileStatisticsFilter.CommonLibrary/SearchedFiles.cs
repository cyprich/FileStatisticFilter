using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStatisticsFilter.CommonLibrary
{
    public class SearchedFiles
    {
        public SearchedFile[] Files { get; set; }

        public SearchedFiles()
        {
            Files = [];
        }

        public SearchedFiles(IEnumerable<FileInfo> files)
        {
            Files = [.. files.Select(file => new SearchedFile(file))];
        }

        public void LoadFromCsv(FileInfo file)
        {
            try
            {
                var lines = File.ReadAllLines(file.FullName);
                var result = new List<SearchedFile>();

                foreach (var line in lines[1..])  // ignoring header
                {
                    result.Add(new SearchedFile(line));
                }

                Files = [.. result];
            }
            catch (UnauthorizedAccessException e)
            {
                Console.Error.WriteLine($"Unauthorized access to {file.FullName}\n{e}");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }

        public void SaveToCsv(FileInfo file)
        {
            var lines = new List<string>();
            lines.Add(SearchedFile.ToCsvHeaderLine());

            foreach (var f in Files)
            {
                lines.Add($"{f.ToCsvLine()}");
            }

            File.WriteAllLines(file.FullName, lines);
        }

    }
}
