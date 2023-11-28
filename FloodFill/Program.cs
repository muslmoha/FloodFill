using FloodFill;
using System.Collections;
using System.Drawing;

FileStream _file;
Bitmap bitMap;
const string folderPath = "C:\\Users\\admin\\source\\repos\\FloodFill\\FloodFill\\bin\\Debug\\net6.0\\original\\";

FloodFillService floodFillService = new FloodFillService();
Dictionary<Bitmap, string> imgs = new Dictionary<Bitmap, string>();

try
{
    // Check if the directory exists
    if (Directory.Exists(folderPath))
    {
        // Get all files in the directory
        string[] files = Directory.GetFiles(folderPath);

        // Display file names
        foreach (string filePath in files)
        {
            Console.WriteLine($"File Name: {Path.GetFileName(filePath)}");
            string fileName = Path.GetFileName(filePath);
            FileStream file = new FileStream(folderPath + fileName, FileMode.Open);
            bitMap = new Bitmap(file);
            imgs.Add(bitMap, fileName);
        }

        // Parallel equivalent
        Parallel.ForEach(imgs, item => floodFillService.FloodFill(item.Key, item.Value));
    }
    else
    {
        Console.WriteLine("The specified directory does not exist.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}