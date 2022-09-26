using FakeChan22.Params;

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

        public void AsyncTalk(MessageData talk)
        {
            OnCallAsyncTalk?.Invoke(talk);
        }

        public void SyncTalk(MessageData talk)
        {
            MessQueue.AddQueue(talk);
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
