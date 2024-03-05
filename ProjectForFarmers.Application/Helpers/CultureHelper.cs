using System.Globalization;
using System.Reflection;
using System.Resources;

namespace FarmersMarketplace.Application.Helpers
{
    public static class CultureHelper
    {
        private static readonly ResourceManager ExceptionManager;
        private static readonly ResourceManager FarmLogManager;
        private static readonly ResourceManager PropertyManager;

        static CultureHelper()
        {
            string exceptionManagerbaseName = $"{typeof(Resources.Exceptions.Exceptions)}";
            ExceptionManager = new ResourceManager(exceptionManagerbaseName, Assembly.GetExecutingAssembly());

            string farmLogManagerbaseName = $"{typeof(Resources.FarmLogs.FarmLogs)}";
            FarmLogManager = new ResourceManager(farmLogManagerbaseName, Assembly.GetExecutingAssembly());

            string oropertyManagerbaseName = $"{typeof(Resources.Properties.Properties)}";
            PropertyManager = new ResourceManager(oropertyManagerbaseName, Assembly.GetExecutingAssembly());
        }

        public static string Exception(string key, params string[] parameters) 
        {
            string str = ExceptionManager.GetString(key, CultureInfo.CurrentUICulture);

            if(parameters.Length > 0)
            {
                str = string.Format(str, parameters);
            }

            return str;
        }

        public static string FarmLog(string key, params string[] parameters)
        {
            string str = FarmLogManager.GetString(key, CultureInfo.CurrentUICulture);

            if (parameters.Length > 0)
            {
                str = string.Format(str, parameters);
            }

            return str;
        }

        public static string Property(string key)
        {
            string str = PropertyManager.GetString(key, CultureInfo.CurrentUICulture);

            return str;
        }
    }

}
