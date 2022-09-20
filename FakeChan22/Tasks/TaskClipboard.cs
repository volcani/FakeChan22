using FakeChan22.Tasks;
using System;

namespace FakeChan22
{
    public class TaskClipboard : TaskBase, IDisposable
    {
        ListenerConfigClipboard lsnrCfg = null;

        public delegate void CallEventHandlerOperateClipboardChain();
        public event CallEventHandlerOperateClipboardChain OnSetClipboardChain;
        public event CallEventHandlerOperateClipboardChain OnRemoveClipboardChain;

        public TaskClipboard(ref ListenerConfigClipboard lsrCfg, ref MessageQueueWrapper que)
        {
            lsnrCfg = lsrCfg;
            MessQueue = que;
            based = false;
        }

        public void Dispose()
        {
            lsnrCfg = null;
            MessQueue = null;
        }

        public override void TaskStart()
        {
            try
            {
                OnSetClipboardChain?.Invoke();
            }
            catch (Exception e)
            {
                Logging(String.Format(@"CLIPBOARD, {0}", e.Message));
                throw new Exception(string.Format(@"クリップボードリスナ起動でエラー : {0}", e.Message));
            }
        }

        public override void TaskStop()
        {
            try
            {
                OnRemoveClipboardChain?.Invoke();
            }
            catch(Exception)
            {
                //
            }
        }

        public void AcceptData(string talkText)
        {
            MessageData talk = new MessageData()
            {
                LsnrCfg = lsnrCfg,
                OrgMessage = talkText,
                CompatSpeed = -1,
                CompatTone = -1,
                CompatVolume = -1,
                CompatVType = -1,
                TaskId = MessQueue.count + 1
            };

            if (lsnrCfg.IsAsync)
            {
                AsyncTalk(talk);
            }
            else
            {
                SyncTalk(talk);
            }
        }

    }
}
