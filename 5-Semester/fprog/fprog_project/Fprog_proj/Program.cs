using System.Collections.Immutable;
public class Program
{
    public static void Main(string[] args)
    {
        Predicate<string[]> isValidArgumentCount = (args) => args.Length == 2;
        Predicate<string> isValidDirectory = (dir) => Directory.Exists(dir);
        Predicate<string> isValidExtension = (extension) => Path.HasExtension(extension);

        Func<string, string, string> calculateFileCount = (dirPath, fileExtension) =>
        {
            // Recursively enumerate all files in the directory matching the supplied file extension
            var filePaths = Directory.EnumerateFiles(dirPath, "*" + fileExtension, SearchOption.AllDirectories);

            // Compute the list of all words in the files and the number of occurrences for each word
            var wordCounts = filePaths
                .SelectMany(filePath => File.ReadAllText(filePath).Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
                .GroupBy(word => word)
                //.Take(50)
                .ToImmutableDictionary(group => group.Key, group => group.Count());

            var dictionaryEntries = wordCounts.OrderBy(kvp => kvp.Key).Select(kvp => string.Format("{0} -> {1}", kvp.Key, kvp.Value));

            return string.Join(Environment.NewLine, dictionaryEntries);
        };

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (!isValidArgumentCount(args))
        {
            Console.WriteLine("Missing command line arguments: directory path and file extension");
            return;
        }

        if (!isValidDirectory(args[0]))
        {
            Console.WriteLine(args[0] + " is not a valid directory");
            return;
        }


        if (!isValidExtension(args[1]))
        {
            Console.WriteLine(args[1] + " is not a valid file extension");
            return;
        }

        // Sort the list by word count decreasingly and print the result
        Console.WriteLine(calculateFileCount(args[0], args[1]));
    }
}



