namespace MatchingLibrary.Tests.Utils;

public static class IoUtils
{
    private const string TestBinDirectoryPath = "./../../../";

    public static void WriteStringToFile(string json, string path, string pathToRoot = TestBinDirectoryPath)
    {
        using (var outputFile = new StreamWriter(TestBinDirectoryPath + path))
        {
            outputFile.Write(json);
        }
    }

    public static string ReadFileAsString(string path, string pathToRoot = TestBinDirectoryPath)
    {
        var json = File.ReadAllText(pathToRoot + path);
        return json;
    }
}