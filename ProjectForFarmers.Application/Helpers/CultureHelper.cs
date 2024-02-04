using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Helpers
{
    public static class CultureHelper
    {
        private static readonly ResourceManager Manager;

        static CultureHelper()
        {
            string baseName = $"{typeof(Resources.Exceptions)}";
            Manager = new ResourceManager(baseName, Assembly.GetExecutingAssembly());
        }

        public static string GetString(string key, params string[] parameters) 
        {
            string str = Manager.GetString(key, CultureInfo.CurrentUICulture);

            if(parameters.Length > 0)
            {
                str = string.Format(str, parameters);
            }

            return str;
        }
    }

}
