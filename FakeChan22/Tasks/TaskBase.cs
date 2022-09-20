namespace FakeChan22.Tasks
{
    public class TaskBase
    {
        public bool based = true;

        public MessageQueueWrapper MessQueue = null;

        public delegate void CallEventHandlerCallTalk(MessageData talk);
        public delegate void CallEventHandlerLogging(string logtext);
        public event CallEventHandlerCallTalk OnCallAsyncTalk;
        public event CallEventHandlerLogging OnLogging;

        internal void AsyncTalk(MessageData talk)
        {
            OnCallAsyncTalk?.Invoke(talk);
        }

        internal void SyncTalk(MessageData talk)
        {
            MessQueue.AddQueue(talk);
        }

        internal void Logging(string logText)
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
