using FakeChan22.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace FakeChan22.Configs
{
    public class FakeChanTypesCollector
    {
        public List<Assembly> ListenerAssemblyList { get; private set; } = new List<Assembly>();

        public Dictionary<string, Type> ListenerConfigTypeDictionary { get; private set; } = new Dictionary<string, Type>();

        public Dictionary<string, Type> TaskTypeDictionary { get; private set; } = new Dictionary<string, Type>();

        public FakeChanTypesCollector()
        {
            string targetPath = string.Format(@"{0}\Extend", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            ListenerConfigTypeDictionary.Add(typeof(ListenerConfigIpc).FullName, typeof(ListenerConfigIpc));
            ListenerConfigTypeDictionary.Add(typeof(ListenerConfigSocket).FullName, typeof(ListenerConfigSocket));
            ListenerConfigTypeDictionary.Add(typeof(ListenerConfigHttp).FullName, typeof(ListenerConfigHttp));
            ListenerConfigTypeDictionary.Add(typeof(ListenerConfigClipboard).FullName, typeof(ListenerConfigClipboard));
            ListenerConfigTypeDictionary.Add(typeof(ListenerConfigTwitter).FullName, typeof(ListenerConfigTwitter));

            TaskTypeDictionary.Add(typeof(TaskIpc).FullName, typeof(TaskIpc));
            TaskTypeDictionary.Add(typeof(TaskSocket).FullName, typeof(TaskSocket));
            TaskTypeDictionary.Add(typeof(TaskHttp).FullName, typeof(TaskHttp));
            TaskTypeDictionary.Add(typeof(TaskClipboard).FullName, typeof(TaskClipboard));
            TaskTypeDictionary.Add(typeof(TaskTwitter).FullName, typeof(TaskTwitter));

            // Extendフォルダのassembly収集

            if (Directory.Exists(targetPath))
            {
                foreach (var item in Directory.EnumerateFiles(targetPath))
                {
                    if (Regex.IsMatch(Path.GetFileName(item), @"^Task.+\.[Dd][Ll][Ll]$"))
                    {
                        try
                        {
                            ListenerAssemblyList.Add(Assembly.LoadFrom(item));
                        }
                        catch(Exception)
                        {
                            //
                        }
                    }
                }
            }

            foreach (var item in ListenerAssemblyList)
            {
                var lsnrType = item.ExportedTypes.FirstOrDefault(v=>v.BaseType.Name== "ListenerConfigBase");
                if ((lsnrType != null)&&(Regex.IsMatch(lsnrType.FullName, @"^FakeChan22\.Tasks\.ListenerConfig"))) ListenerConfigTypeDictionary.Add(lsnrType.FullName, lsnrType);

                var taskType = item.ExportedTypes.FirstOrDefault(v => v.BaseType.Name == "TaskBase");
                if ((taskType != null)&&(Regex.IsMatch(taskType.FullName, @"^FakeChan22\.Tasks\.Task"))) TaskTypeDictionary.Add(taskType.FullName, taskType);
            }

        }
    }
}
