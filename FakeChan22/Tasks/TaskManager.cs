using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace FakeChan22.Tasks
{
    public class TaskManager
    {
        Dictionary<ListenerConfig, TaskBase> tasks;
        MessageQueueWrapper messQue;
        FakeChanConfig config;
        ScAPIs api = new ScAPIs();
        Random r = new Random();

        ListenerConfigClipboard listenerConfigClipboard;
        TaskClipboard taskClipboard;

        TaskSyncTalk talkTask;

        public TaskManager(ref List<ListenerConfig> list, ref MessageQueueWrapper que, ref FakeChanConfig cfg)
        {
            tasks = new Dictionary<ListenerConfig, TaskBase>();
            messQue = que;
            config = cfg;

            // キュー＆発声タスク
            talkTask = new TaskSyncTalk(ref messQue, ref config);

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
                        listenerConfigClipboard = item as ListenerConfigClipboard;
                        taskClipboard = new TaskClipboard(ref listenerConfigClipboard, ref que);
                        tasks.Add(item, taskClipboard);
                        break;
                }

                tasks[item].OnCallAsyncTalk += talkTask.AsyncTalk;
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

    }
}
