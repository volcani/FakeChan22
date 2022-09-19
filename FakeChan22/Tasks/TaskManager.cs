using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using static FakeChan22.Tasks.TaskBase;

namespace FakeChan22.Tasks
{
    public class TaskManager
    {
        Dictionary<ListenerConfig, TaskBase> tasks;
        MessageQueueWrapper messQue;
        FakeChanConfig config;
        TaskTalks talkTask;
        TaskClipboard taskClipboard;

        public delegate void CallEventHandlerLogging(string logtext);
        public event CallEventHandlerLogging OnLogging;

        public TaskManager(ref List<ListenerConfig> list, ref MessageQueueWrapper que, ref FakeChanConfig cfg)
        {
            tasks = new Dictionary<ListenerConfig, TaskBase>();
            messQue = que;
            config = cfg;

            // キュー＆発声タスク
            talkTask = new TaskTalks(ref messQue, ref config);
            talkTask.OnLogging += Logging;

            // 受信タスク
            foreach (var item in list)
            {
                switch (item.LsnrType)
                {
                    case ListenerType.ipc:
                        var lsnrIpc = item as ListenerConfigIpc;
                        tasks.Add(item, new TaskIpc(ref lsnrIpc, ref que));
                        break;

                    case ListenerType.socket:
                        var lsnrSocket = item as ListenerConfigSocket;
                        tasks.Add(item, new TaskSocket(ref lsnrSocket, ref que));
                        break;

                    case ListenerType.http:
                        var lsnrHttp = item as ListenerConfigHttp;
                        tasks.Add(item, new TaskHttp(ref lsnrHttp, ref que));
                        break;

                    case ListenerType.clipboard:
                        var lsnrClip = item as ListenerConfigClipboard;
                        taskClipboard = new TaskClipboard(ref lsnrClip, ref que);
                        tasks.Add(item, taskClipboard);
                        break;
                }

                tasks[item].OnCallAsyncTalk += talkTask.AsyncTalk;
                tasks[item].OnLogging += Logging;
            }

        }

        public void TaskShutdown()
        {
            foreach (var item in tasks)
            {
                item.Value.TaskStop();
            }
        }

        public void TaskBoot()
        {
            foreach (var item in tasks)
            {
                if(item.Key.IsEnable) item.Value.TaskStart();
            }
        }

        public void TaskReBoot(ref ListenerConfig lsnr)
        {
            if(tasks.ContainsKey(lsnr))
            {
                tasks[lsnr].TaskStop();

                if (lsnr.IsEnable) tasks[lsnr].TaskStart();
            }
        }

        public TaskClipboard ClipboardTask
        {
            get
            {
                return taskClipboard;
            }
        }

        private void Logging(string logText)
        {
            OnLogging?.Invoke(logText);
        }
    }
}
