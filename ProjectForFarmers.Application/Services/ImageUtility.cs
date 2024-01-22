using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Services
{
    public static class ImageUtility
    {
        public static readonly string ImagesDirectory;
        public static readonly string FarmsImagesDirectory;

        static ImageUtility()
        {
            ImagesDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName
                + "\\ProjectForFarmers.Persistence\\Images";
             FarmsImagesDirectory = Path.Combine(ImagesDirectory, "Farms");
        }
    }

}
