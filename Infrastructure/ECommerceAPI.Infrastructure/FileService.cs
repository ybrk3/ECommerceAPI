using ECommerceAPI.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using ECommerceAPI.Infrastructure.Operations;

namespace ECommerceAPI.Infrastructure
{
    public class FileService
    {
        readonly IWebHostEnvironment _webHostEnvironment;
        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        async Task<string> FileRenameAsync(string path, string fileName, bool first = true, int tryNo = 0)
        {
            string newFileName = await Task.Run<string>(async () =>
            {
                //Get the extension of file
                string extension = Path.GetExtension(fileName);
                string newFileName = string.Empty;

                if (first)
                {
                    string oldName = Path.GetFileNameWithoutExtension(fileName);
                    newFileName = $"{NameOperation.CharacterRegulatory(oldName)}{extension}";
                }
                else
                {
                    newFileName = fileName;
                    //check it's first check/adjustment on the name or not
                    int indexNo = newFileName.IndexOf("-");

                    if (indexNo == -1)
                    {
                        //if it's first adjustment, just add "-2" on the end of the fileName
                        newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}-2{extension}";
                    }
                    else
                    {
                        int lastIndex = 0;
                        while (true)
                        {
                            lastIndex = indexNo;
                            indexNo = newFileName.IndexOf("-", indexNo + 1); //find index after last index
                            if (indexNo == -1)
                            {
                                //When there's no more index, it will return "-1", so we will set our indexNo as latest lastIndex
                                indexNo = lastIndex;
                                break;
                            }
                        }
                        if (tryNo == 0) { newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}-2{extension}"; tryNo++; }
                        else
                        {
                            string fileRawName = newFileName.Substring(0, indexNo);
                            string fileNo = newFileName.Split("-").Last().Split(".")[0];
                            if (int.TryParse(fileNo, out int _fileNo)) { _fileNo++; newFileName = $"{fileRawName}-{_fileNo}{extension}"; }
                            tryNo++;
                        }
                    }
                }
                //Check whether file name exists or not
                if (File.Exists($"{path}\\{newFileName}")) return await FileRenameAsync(path, newFileName, false, tryNo);
                else return newFileName;
            });
            return newFileName;
        }
    }

}


