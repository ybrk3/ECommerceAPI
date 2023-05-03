using ECommerceAPI.Infrastructure.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Services.Storage
{
    /*It contains methods which are used in storage concrete classes in common however there are some differences such as exitense check of file name in storage
    that's why these method not defined in StorageService*/
    public class Storage
    {
        protected delegate bool HasFile(string fileName, string pathOrContainerName);
        //It's delegate because it can be used as parameter and also HasFile() method in storage concrete classes can be used on behalf of this
        protected async Task<string> FileRenameAsync(string pathOrContainerName, string fileName, HasFile hasFileMethod, bool first = true, int tryNo = 0)
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
                        tryNo++;
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
                /*if (File.Exists($"{pathOrContainerName}\\{newFileName}")) return await FileRenameAsync(pathOrContainerName, newFileName, false, tryNo);
                else return newFileName;*/
                //above exitense control is different for each storage type local, aws etc.

                if (hasFileMethod(newFileName, pathOrContainerName)) return await FileRenameAsync(pathOrContainerName, newFileName, hasFileMethod, false, tryNo);
                else return newFileName;
            });
            return newFileName;
        }
    }
}
