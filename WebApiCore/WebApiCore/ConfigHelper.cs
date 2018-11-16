using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace WebApiCore
{
    public static class ConfigHelper
    {
        public static ConfigAppSettings appsettings = new ConfigAppSettings();
        public static ConfigUsers users = new ConfigUsers();
    }

    public static class ConfigBuild
    {
        /// <summary>
        /// 创建配置文件
        /// .json 配置文件提前需要再startup 文件注册
        /// </summary>
        /// <param name="fileName">//在当前目录或者根目录中寻找.json文件</param>
        /// <returns></returns>
        public static IConfiguration Build(string fileName)
        {
            var directory = AppContext.BaseDirectory;
            directory = directory.Replace("\\", "/");

            var filePath = $"{directory}/{fileName}";
            if (!File.Exists(filePath))
            {
                var length = directory.IndexOf("/bin");
                filePath = $"{directory.Substring(0, length)}/{fileName}";
            }

            var builder = new ConfigurationBuilder()
                .AddJsonFile(filePath, false, true);

            return builder.Build();
        }

        /// <summary>
        /// 获取配置节点内容
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSectionValue(IConfiguration configuration, string key)
        {
            return configuration.GetSection(key).Value;
        }

    }


    public class ConfigBase
    {
        private IConfiguration _configuration { get; set; }
        public virtual string fileName { get; set; }

        public void BuildConfig(string fileName)
        {
            _configuration = Build(fileName);
        }

        public string GetSectionValue(string key)
        {
            return _configuration.GetSection(key).Value;
        }

        /// <summary>
        /// 创建配置文件
        /// .json 配置文件提前需要再startup 文件注册
        /// </summary>
        /// <param name="fileName">//在当前目录或者根目录中寻找.json文件</param>
        /// <returns></returns>
        private IConfiguration Build(string fileName)
        {
            var directory = AppContext.BaseDirectory;
            directory = directory.Replace("\\", "/");

            var filePath = $"{directory}/{fileName}";
            if (!File.Exists(filePath))
            {
                var length = directory.IndexOf("/bin");
                filePath = $"{directory.Substring(0, length)}/{fileName}";
            }

            var builder = new ConfigurationBuilder()
                .AddJsonFile(filePath, false, true);

            return builder.Build();
        }

    }
    public class ConfigAppSettings : ConfigBase
    {
        public new string fileName = "Configs/appsettings.json";

        public ConfigAppSettings()
        {
            BuildConfig(fileName);
        }

        /// <summary>
        /// 获取AppSettings节点
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetAppSettingValue(string key)
        {
            return GetSectionValue($"AppSettings:{key}");
        }
    }


    public class ConfigUsers : ConfigBase
    {
        public new string fileName = "Configs/users.json";
        public ConfigUsers()
        {
            BuildConfig(fileName);
        }
    }
}
