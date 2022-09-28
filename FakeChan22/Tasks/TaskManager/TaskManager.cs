using FakeChan22.Config;
using FakeChan22.Configs;
using FakeChan22.Params;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FakeChan22.Tasks
{
    public class TaskManager
    {
        Dictionary<ListenerConfigBase, TaskBase> tasks;
        MessageQueueWrapper messQue;
        FakeChanConfig config;
        SubTaskTalks talkTask;
        TaskClipboard taskClipboard;

        public delegate void CallEventHandlerLogging(string logtext);
        public event CallEventHandlerLogging OnLogging;

        public TaskClipboard ClipboardTask
        {
            get
            {
                return taskClipboard;
            }
        }

        public SubTaskCommentGen CommenGenTask
        {
            get
            {
                return talkTask.CommentGenSubTask;
            }
        }

        public TaskManager(ref List<ListenerConfigBase> list, ref MessageQueueWrapper que, ref FakeChanConfig cfg, ref FakeChanTypesCollector typeCollection)
        {
            tasks = new Dictionary<ListenerConfigBase, TaskBase>();
            messQue = que;
            config = cfg;

            // キュー＆発声タスク
            talkTask = new SubTaskTalks(ref messQue, ref config);
            talkTask.OnLogging += Logging;

            // 受信タスクのインスタンス生成
            foreach (var item in list)
            {
                if (typeCollection.TaskTypeDictionary.TryGetValue(item.TaskTypeFullName, out Type t))
                {
                    ListenerConfigBase lsnr = item;
                    object taskObj = Activator.CreateInstance(t, new object[] { lsnr, que });

                    tasks.Add(item, taskObj as TaskBase);

                    tasks[item].OnCallAsyncTalk += talkTask.AsyncTalk;
                    tasks[item].OnLogging += Logging;

                    if (item.GetType() == typeof(ListenerConfigClipboard)) taskClipboard = taskObj as TaskClipboard;
                }
            }

        }

        public void TaskShutdown()
        {
            foreach (var item in tasks)
            {
                if (item.Key.IsEnable)
                {
                    item.Value.TaskStop();

                    if(item.Value.IsRunning)
                    {
                        LoggingTM(String.Format(@"{0}, 処理停止", item.Key.LabelName));
                    }
                    else
                    {
                        LoggingTM(String.Format(@"{0}, 処理停止（念のための呼び出し）", item.Key.LabelName));
                    }
                }
            }
        }

        public void TaskBoot()
        {
            foreach (var item in tasks)
            {
                if (item.Key.IsEnable)
                {
                    item.Value.TaskStart();
                    if (item.Value.IsRunning)
                    {
                        LoggingTM(String.Format(@"{0}, 処理開始", item.Key.LabelName));
                    }
                    else
                    {
                        LoggingTM(String.Format(@"{0}, 処理停止", item.Key.LabelName));
                    }

                }
            }
        }

        public void TaskReBoot(ref ListenerConfigBase lsnr)
        {
            if(tasks.ContainsKey(lsnr))
            {
                tasks[lsnr].TaskStop();

                if (tasks[lsnr].IsRunning)
                {
                    LoggingTM(String.Format(@"{0}, 処理停止", lsnr.LabelName));
                }
                else
                {
                    LoggingTM(String.Format(@"{0}, 処理停止（念のための呼び出し）", lsnr.LabelName));
                }

                if (lsnr.IsEnable)
                {
                    tasks[lsnr].TaskStart();
                    LoggingTM(String.Format(@"{0}, 処理開始", lsnr.LabelName));
                }
            }
        }

        private void Logging(string logText)
        {
            OnLogging?.Invoke(string.Format(@"{0} {1}", DateTime.Now, logText));
        }
        private void LoggingTM(string logText)
        {
            OnLogging?.Invoke(string.Format(@"{0} TMGR, {1}", DateTime.Now, logText));
        }
    }
}
