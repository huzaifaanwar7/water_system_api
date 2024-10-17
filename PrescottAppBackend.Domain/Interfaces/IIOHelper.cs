using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrescottAppBackend.Domain
{
    public interface IIOHelper
    {
        string SaveFile(string base64File, string fileName, string folderName = "upload");
    }
}