using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeChan22
{
    public class MessageQueueWrapper
    {
        BlockingCollection<MessageData> MessQue = null;

        public delegate void CallEventHandlerAddQueError(MessageData talk);
        public event CallEventHandlerAddQueError OnAddQueueError;

        public bool IsSyncTaking { get; set; }

        public int NowtaskId { get; set; }

        public MessageQueueWrapper()
        {
            MessQue = new BlockingCollection<MessageData>();
            ClearQueue();
        }

        public void ClearQueue()
        {
            BlockingCollection<MessageData>[] t = { MessQue };
            BlockingCollection<MessageData>.TryTakeFromAny(t, out MessageData item);
        }

        public bool AddQueue(MessageData item)
        {
            bool f = MessQue.TryAdd(item, 1000);

            if (!f) OnAddQueueError?.Invoke(item);

            return f;
        }

        public ref BlockingCollection<MessageData> QueueRef()
        {
            return ref MessQue;
        }

        public int count
        {
            get
            {
                return MessQue.Count;
            }
        }
    }
}
