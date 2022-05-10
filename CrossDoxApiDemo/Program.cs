using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace CrossDoxApiDemo
{
    class Program
    {
        private static void ExampleToXLSX(CrossDoxApiConsumer crossDox, FileInfo[] pdfFiles, DirectoryInfo outputDir)
        {
            CrossDoxParsedData parsedData = crossDox.ParseFilesToXLSX(pdfFiles);

            foreach (var info in parsedData.ParseResponseMetadata)
            {
                Console.WriteLine($"{info.FileName}: {info.Status}");
                if (info.Status == "Failure")
                {
                    // Handle errors
                }
            }

            if (parsedData.ZipCount == 0) return;

            string tmpZip = Path.GetTempFileName();
            using (BinaryWriter writer = new BinaryWriter(File.Open(tmpZip, FileMode.Create)))
            {
                writer.Write(Convert.FromBase64String(parsedData.ZipBase64));
                writer.Close();
            }

            ZipFile.ExtractToDirectory(tmpZip, outputDir.FullName, overwriteFiles: true);

            File.Delete(tmpZip);
        }

        private static void ExampleToJSON(CrossDoxApiConsumer crossDox, FileInfo[] pdfFiles, DirectoryInfo outputDir)
        {
            ParseInfoResponse parsedData = crossDox.ParseFilesToJSON(pdfFiles);

            Console.WriteLine(JsonConvert.SerializeObject(parsedData, Formatting.Indented));
        }

        static void Main(string[] args)
        {
            string ApiToken = ConfigurationManager.AppSettings["ApiToken"];
            string username = ConfigurationManager.AppSettings["ApiUsername"];
            string password = ConfigurationManager.AppSettings["ApiPassword"];

            DirectoryInfo sourceDir = Directory.CreateDirectory(@"C:\temp\CrossDoxDemo\Source");
            DirectoryInfo outputDir = Directory.CreateDirectory(@"C:\temp\CrossDoxDemo\Output");

            FileInfo[] pdfFiles = sourceDir.GetFiles().Where(file => file.Extension.ToLower() == ".pdf").ToArray();
            if (pdfFiles.Length == 0)
            {
                Console.WriteLine("Source directory is empty");
                return;
            }

            CrossDoxApiConsumer crossDox = new CrossDoxApiConsumer(ApiToken, username, password);
            crossDox.UpdateUserPreferences(new() { MergeFiles = false });

            //ExampleToXLSX(crossDox, pdfFiles, outputDir);
            ExampleToJSON(crossDox, pdfFiles, outputDir);

            Console.WriteLine();
            Console.WriteLine("Done. Press and key to Exit.");
            Console.ReadKey();
        }
    }
}
