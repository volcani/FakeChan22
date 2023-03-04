using FakeChan22.Filters;
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

        public Dictionary<string, Type> FilterConfigTypeDictionary { get; private set; } = new Dictionary<string, Type>();

        public Dictionary<string, Type> FilterProcTypeDictionary { get; private set; } = new Dictionary<string, Type>();

        public List<Type> FilterProcTypeSortedList { get; private set; } = new List<Type>();

        public FakeChanTypesCollector()
        {
            string targetPath = string.Format(@"{0}\Extend", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            ListenerConfigTypeDictionary.Add(typeof(ListenerConfigIpc).FullName, typeof(ListenerConfigIpc));
            ListenerConfigTypeDictionary.Add(typeof(ListenerConfigSocket).FullName, typeof(ListenerConfigSocket));
            ListenerConfigTypeDictionary.Add(typeof(ListenerConfigHttp).FullName, typeof(ListenerConfigHttp));
            ListenerConfigTypeDictionary.Add(typeof(ListenerConfigClipboard).FullName, typeof(ListenerConfigClipboard));
            //ListenerConfigTypeDictionary.Add(typeof(ListenerConfigTwitter).FullName, typeof(ListenerConfigTwitter));

            TaskTypeDictionary.Add(typeof(TaskIpc).FullName, typeof(TaskIpc));
            TaskTypeDictionary.Add(typeof(TaskSocket).FullName, typeof(TaskSocket));
            TaskTypeDictionary.Add(typeof(TaskHttp).FullName, typeof(TaskHttp));
            TaskTypeDictionary.Add(typeof(TaskClipboard).FullName, typeof(TaskClipboard));
            //TaskTypeDictionary.Add(typeof(TaskTwitter).FullName, typeof(TaskTwitter));

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
                        catch (Exception)
                        {
                            //
                        }
                    }
                }
            }

            FilterConfigTypeDictionary.Add(typeof(FilterConfigApplauseWord).FullName, typeof(FilterConfigApplauseWord));
            FilterConfigTypeDictionary.Add(typeof(FilterConfigCleanupURL).FullName, typeof(FilterConfigCleanupURL));
            FilterConfigTypeDictionary.Add(typeof(FilterConfigCutString).FullName, typeof(FilterConfigCutString));
            FilterConfigTypeDictionary.Add(typeof(FilterConfigEmojiCleaner).FullName, typeof(FilterConfigEmojiCleaner));
            FilterConfigTypeDictionary.Add(typeof(FilterConfigEmojiReplace).FullName, typeof(FilterConfigEmojiReplace));
            FilterConfigTypeDictionary.Add(typeof(FilterConfigGrassWord).FullName, typeof(FilterConfigGrassWord));
            FilterConfigTypeDictionary.Add(typeof(FilterConfigReplaceText).FullName, typeof(FilterConfigReplaceText));
            FilterConfigTypeDictionary.Add(typeof(FilterConfigSplitUser).FullName, typeof(FilterConfigSplitUser));
            FilterConfigTypeDictionary.Add(typeof(FilterConfigZen2HanChar).FullName, typeof(FilterConfigZen2HanChar));
            FilterConfigTypeDictionary.Add(typeof(FilterConfigZen2HanNum).FullName, typeof(FilterConfigZen2HanNum));

            FilterProcTypeDictionary.Add(typeof(FilterProcApplauseWord).FullName, typeof(FilterProcApplauseWord));
            FilterProcTypeDictionary.Add(typeof(FilterProcCleanupURL).FullName, typeof(FilterProcCleanupURL));
            FilterProcTypeDictionary.Add(typeof(FilterProcCutString).FullName, typeof(FilterProcCutString));
            FilterProcTypeDictionary.Add(typeof(FilterProcEmojiCleaner).FullName, typeof(FilterProcEmojiCleaner));
            FilterProcTypeDictionary.Add(typeof(FilterProcEmojiReplace).FullName, typeof(FilterProcEmojiReplace));
            FilterProcTypeDictionary.Add(typeof(FilterProcGrassWord).FullName, typeof(FilterProcGrassWord));
            FilterProcTypeDictionary.Add(typeof(FilterProcReplaceText).FullName, typeof(FilterProcReplaceText));
            FilterProcTypeDictionary.Add(typeof(FilterProcSplitUser).FullName, typeof(FilterProcSplitUser));
            FilterProcTypeDictionary.Add(typeof(FilterProcZen2HanChar).FullName, typeof(FilterProcZen2HanChar));
            FilterProcTypeDictionary.Add(typeof(FilterProcZen2HanNum).FullName, typeof(FilterProcZen2HanNum));

            FilterProcTypeSortedList.Add(typeof(FilterProcSplitUser));
            FilterProcTypeSortedList.Add(typeof(FilterProcCleanupURL));
            FilterProcTypeSortedList.Add(typeof(FilterProcGrassWord));
            FilterProcTypeSortedList.Add(typeof(FilterProcApplauseWord));
            FilterProcTypeSortedList.Add(typeof(FilterProcEmojiReplace));
            FilterProcTypeSortedList.Add(typeof(FilterProcEmojiCleaner));
            FilterProcTypeSortedList.Add(typeof(FilterProcZen2HanChar));
            FilterProcTypeSortedList.Add(typeof(FilterProcZen2HanNum));
            FilterProcTypeSortedList.Add(typeof(FilterProcReplaceText));
            FilterProcTypeSortedList.Add(typeof(FilterProcCutString));

            // Extendフォルダのassembly収集

            if (Directory.Exists(targetPath))
            {
                foreach (var item in Directory.EnumerateFiles(targetPath))
                {
                    if (Regex.IsMatch(Path.GetFileName(item), @"^FilterProc.+\.[Dd][Ll][Ll]$"))
                    {
                        try
                        {
                            ListenerAssemblyList.Add(Assembly.LoadFrom(item));
                        }
                        catch (Exception)
                        {
                            //
                        }
                    }
                }
            }

            foreach (var item in ListenerAssemblyList)
            {
                var lsnrType = item.ExportedTypes.FirstOrDefault(v => v.BaseType.Name == "ListenerConfigBase");
                if ((lsnrType != null) && (Regex.IsMatch(lsnrType.FullName, @"^FakeChan22\.Tasks\.ListenerConfig"))) ListenerConfigTypeDictionary.Add(lsnrType.FullName, lsnrType);

                var taskType = item.ExportedTypes.FirstOrDefault(v => v.BaseType.Name == "TaskBase");
                if ((taskType != null) && (Regex.IsMatch(taskType.FullName, @"^FakeChan22\.Tasks\.Task"))) TaskTypeDictionary.Add(taskType.FullName, taskType);

                var filterConfType = item.ExportedTypes.FirstOrDefault(v => v.BaseType.Name == "FilterConfigBase");
                if ((filterConfType != null) && (Regex.IsMatch(filterConfType.FullName, @"^FakeChan22\.Filters\.FilterConfig"))) FilterConfigTypeDictionary.Add(filterConfType.FullName, filterConfType);

                var filterProcType = item.ExportedTypes.FirstOrDefault(v => v.BaseType.Name == "FilterProcBase");
                if ((filterProcType != null) && (Regex.IsMatch(filterProcType.FullName, @"^FakeChan22\.Filters\.FilterProc")))
                {
                    FilterProcTypeDictionary.Add(filterProcType.FullName, filterProcType);
                    FilterProcTypeSortedList.Add(filterProcType);
                }

            }

        }
    }
}
