﻿using FakeChan22.Params;
using System;

namespace FakeChan22.Tasks
{
    public class TaskClipboard : TaskBase, IDisposable
    {
        ListenerConfigClipboard LsnrConfig = null;

        public delegate void CallEventHandlerOperateClipboardChain();
        public event CallEventHandlerOperateClipboardChain OnSetClipboardChain;
        public event CallEventHandlerOperateClipboardChain OnRemoveClipboardChain;

        public TaskClipboard(ref ListenerConfigClipboard lsrCfg, ref MessageQueueWrapper que)
        {
            LsnrConfig = lsrCfg;
            MessQueue = que;
            based = false;
        }

        public void Dispose()
        {
            LsnrConfig = null;
            MessQueue = null;
        }

        public override void TaskStart()
        {
            try
            {
                OnSetClipboardChain?.Invoke();
                IsRunning = true;
            }
            catch (Exception e)
            {
                Logging(String.Format(@"CLIPBOARD, {0}", e.Message));
                //throw new Exception(string.Format(@"クリップボードリスナ起動でエラー : {0}", e.Message));
            }
        }

        public override void TaskStop()
        {
            try
            {
                OnRemoveClipboardChain?.Invoke();
            }
            catch (Exception)
            {
                //
            }

            IsRunning = false;
        }

        public void AcceptData(string talkText)
        {
            MessageData talk = new MessageData()
            {
                LsnrCfg = LsnrConfig,
                OrgMessage = talkText,
                CompatSpeed = -1,
                CompatTone = -1,
                CompatVolume = -1,
                CompatVType = -1,
                TaskId = MessQueue.count + 1
            };

            AsTalk(talk);

            //if (LsnrConfig.IsAsync)
            //{
            //    AsyncTalk(talk);
            //}
            //else
            //{
            //    SyncTalk(talk);
            //}
        }

    }
}
