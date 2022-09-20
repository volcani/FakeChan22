using System.Collections.Concurrent;

namespace FakeChan22
{
    public class MessageQueueWrapper
    {
        BlockingCollection<MessageData> MessQue = null;

        public bool IsSyncTaking { get; set; }

        public int NowtaskId { get; set; }

        public MessageQueueWrapper()
        {
            MessQue = new BlockingCollection<MessageData>();
            ClearQueue();
            IsSyncTaking = false;
        }

        public void ClearQueue()
        {
            while (MessQue.Count > 0)
            {
                _ = MessQue.TryTake(out MessageData item);
            }
        }

        public bool AddQueue(MessageData item)
        {
            return MessQue.TryAdd(item, 1000);
        }
        public MessageData TakeQueue()
        {
            MessageData item;

            if (!MessQue.TryTake(out item))
            {
                item = null;
            }

            return item;
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
