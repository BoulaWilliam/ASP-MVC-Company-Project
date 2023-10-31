using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Company.PL.Helpers
{
    public class DocumentSettings
    {
        public static string UploadImage(IFormFile file, string FolderName)
        {
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files",FolderName);
            string fileName = $"{Guid.NewGuid()}{file.FileName}";
            string FilePath = Path.Combine(FolderPath,fileName);
            var fs = new FileStream(FilePath,FileMode.Create);
            file.CopyTo(fs);
            return fileName;
        }

        public static void DeleteFile(string fileName, string FolderName)
        {
            if(fileName is not null && FolderName is not null)
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", FolderName, fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }
    }
}
