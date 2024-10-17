using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using PrescottAppBackend.Domain;


namespace PrescottAppBackend.Infrastructure
{
    public static class IOHelper
    {
        // private readonly IWebHostEnvironment _env;
        // private readonly string _webFolder;
        // private readonly string _rootFolder;

        // public IOHelper(IWebHostEnvironment env)
        // {
        //     _env = env;
        //     _rootFolder = _env.ContentRootPath + "/" + "www";
        //     _webFolder = _env.WebRootPath;
        // }


        public static string SaveFile(string base64File, string fileName, string folderName = "upload")
        {

            // Combine the web root path with the desired folder name
            string folderPath = "www/" + folderName; //Path.Combine(_rootFolder, folderName);

            // Check if the directory exists; if not, create it
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Remove the metadata (everything before the Base64 string)
            string base64Data = base64File.Split(",")[1];

            // Convert Base64 string to byte array
            byte[] fileBytes = Convert.FromBase64String(base64Data);

            // Define the file path where the file will be saved
            string filePath = folderPath + "/" + fileName;

            // Write the byte array to a file
            File.WriteAllBytes(filePath, fileBytes);

            return filePath;
        }
    }
}