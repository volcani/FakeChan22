using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace FakeChan22.Tasks
{
    public class TaskBase
    {
        public bool based = true;

        public MessageQueueWrapper MessQueue = null;

        public delegate void CallEventHandlerCallTalk(MessageData talk);
        public event CallEventHandlerCallTalk OnCallAsyncTalk;

        internal void AsyncTalk(MessageData talk)
        {
            OnCallAsyncTalk?.Invoke(talk);
        }

        internal void SyncTalk(MessageData talk)
        {
            MessQueue.AddQueue(talk);
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
