using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;

namespace CrossDoxApiDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string ApiToken = ConfigurationManager.AppSettings["ApiToken"];
            string username = ConfigurationManager.AppSettings["ApiUsername"];
            string password = ConfigurationManager.AppSettings["ApiPassword"];

            var sourceDir = GetSourceDirectory();
            var outputDir = GetOutputDirectory();
            Console.WriteLine($"Source folder: {sourceDir.FullName}");
            Console.WriteLine($"Output folder: {outputDir.FullName}");

            FileInfo[] files = sourceDir.GetFiles();
            if (files.Length == 0)
            {
                Console.WriteLine("Source directory is empty");
                throw new Exception();
            }

            List<FileInfo> pdfs = new List<FileInfo>();
            foreach (FileInfo file in files)
            {
                if (file.Extension.ToLower() != ".pdf") continue;

                pdfs.Add(file);
            }

            CrossDoxApiConsumer crossDox = new CrossDoxApiConsumer(ApiToken, username, password);

            CrossDoxParsedData parsedData = crossDox.ParseFiles(pdfs.ToArray());

            foreach(var info in parsedData.ParseResponseMetadata)
            {
                if (info.Status == "Failure")
                {
                    Console.WriteLine($"Conversion error: {info.FileName} failed parse");

                    // Handle errors
                }
            }

            if (parsedData.ZipCount == 0) return;

            string tmp = Path.GetTempFileName();
            BinaryWriter writer = new BinaryWriter(File.Open(tmp, FileMode.Create));
            writer.Write(Convert.FromBase64String(parsedData.ZipBase64));
            writer.Close();

            ZipFile.ExtractToDirectory(tmp, outputDir.FullName);

            File.Delete(tmp);

        }

        static DirectoryInfo GetSourceDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), "CrossDoxApiDemo_" + "Source");
            return Directory.CreateDirectory(tempDirectory);
        }

        static DirectoryInfo GetOutputDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), "CrossDoxApiDemo_" + "Output");
            return Directory.CreateDirectory(tempDirectory);
        }
    }
}
