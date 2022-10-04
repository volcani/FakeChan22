using FakeChan22.Params;
using System;

namespace FakeChan22.Tasks
{
    public class TaskBase
    {
        public bool based = true;

        public bool IsRunning { get; set; } = false;

        public MessageQueueWrapper MessQueue = null;

        public delegate void CallEventHandlerCallTalk(MessageData talk);
        public delegate void CallEventHandlerLogging(string logtext);
        public event CallEventHandlerCallTalk OnCallAsyncTalk;
        public event CallEventHandlerLogging OnLogging;

        [Obsolete("AsTalk()を利用してください", false)]
        public void AsyncTalk(MessageData talk)
        {
            OnCallAsyncTalk?.Invoke(talk);
        }

        [Obsolete("AsTalk()を利用してください", false)]
        public void SyncTalk(MessageData talk)
        {
            MessQueue.AddQueue(talk);
        }

        public void AsTalk(MessageData talk)
        {
            if (talk.LsnrCfg.IsAsync)
            {
                OnCallAsyncTalk?.Invoke(talk);
            }
            else
            {
                MessQueue.AddQueue(talk);
            }
        }

        public void Logging(string logText)
        {
            OnLogging?.Invoke(logText);
        }

        public virtual void TaskStart()
        {
            //
        }

        public virtual void TaskStop()
        {
            //
        }
    }
}
