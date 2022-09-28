using FakeChan22.Params;
using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;

namespace FakeChan22.Tasks
{
    public class TaskIpc : TaskBase, IDisposable
    {
        ListenerConfigIpc LsnrConfig = null;

        FNF.Utility.BouyomiChanRemoting ShareIpcObject;
        IpcServerChannel IpcCh = null;

        public TaskIpc(ref ListenerConfigIpc lsnrCfg, ref MessageQueueWrapper que)
        {
            LsnrConfig = lsnrCfg; 
            MessQueue = que;
            based = false;

            ShareIpcObject = new FNF.Utility.BouyomiChanRemoting();
            ShareIpcObject.OnAddTalkTask01 += new FNF.Utility.BouyomiChanRemoting.CallEventHandlerAddTalkTask01(IPCAddTalkTask01);
            ShareIpcObject.OnAddTalkTask02 += new FNF.Utility.BouyomiChanRemoting.CallEventHandlerAddTalkTask02(IPCAddTalkTask02);
            ShareIpcObject.OnAddTalkTask03 += new FNF.Utility.BouyomiChanRemoting.CallEventHandlerAddTalkTask03(IPCAddTalkTask03);
            ShareIpcObject.OnAddTalkTask21 += new FNF.Utility.BouyomiChanRemoting.CallEventHandlerAddTalkTask21(IPCAddTalkTask21);
            ShareIpcObject.OnAddTalkTask23 += new FNF.Utility.BouyomiChanRemoting.CallEventHandlerAddTalkTask23(IPCAddTalkTask23);
            ShareIpcObject.OnClearTalkTask += new FNF.Utility.BouyomiChanRemoting.CallEventHandlerSimpleTask(IPCClearTalkTask);
            ShareIpcObject.OnSkipTalkTask += new FNF.Utility.BouyomiChanRemoting.CallEventHandlerSimpleTask(IPCSkipTalkTask);
            ShareIpcObject.MessQue = MessQueue;
        }

        public void Dispose()
        {
            LsnrConfig = null;
            MessQueue = null;
            ShareIpcObject = null;
            IpcCh = null;
        }

        public override void TaskStart()
        {
            try
            {
                if (IpcCh is null)
                {
                    IpcCh = new IpcServerChannel(LsnrConfig.ChannelName);
                }

                IpcCh.IsSecured = false;
                ChannelServices.RegisterChannel(IpcCh, false);
                RemotingServices.Marshal(ShareIpcObject, LsnrConfig.ObjURI, typeof(FNF.Utility.BouyomiChanRemoting));
                IsRunning = true;
            }
            catch (Exception e)
            {
                Logging(String.Format(@"IPC, {0}", e.Message));
                //throw new Exception(string.Format(@"IPCリスナ起動でエラー : {0}",e.Message));
            }
        }

        public override void TaskStop()
        {
            if (IpcCh != null)
            {
                try
                {
                    RemotingServices.Disconnect(ShareIpcObject);
                    IpcCh.StopListening(null);
                    ChannelServices.UnregisterChannel(IpcCh);
                }
                catch (Exception)
                {
                    //
                }
            }

            IsRunning = false;
        }

        private void IPCAddTalkTask01(string TalkText)
        {
            IPCAddTalkTask03(TalkText, -1, -1, -1, 0);
        }

        private void IPCAddTalkTask02(string TalkText, int iSpeed, int iVolume, int vType)
        {
            IPCAddTalkTask03(TalkText, iSpeed, -1, iVolume, vType);
        }

        private void IPCAddTalkTask03(string TalkText, int iSpeed, int iTone, int iVolume, int vType)
        {
            MessageData talk = new MessageData()
            {
                LsnrCfg = this.LsnrConfig,
                OrgMessage = TalkText,
                CompatSpeed = iSpeed,
                CompatTone = iTone,
                CompatVolume = iVolume,
                CompatVType= vType,
                TaskId = MessQueue.count + 1
            };
            
            if (LsnrConfig.IsAsync)
            {
                AsyncTalk(talk);
            }
            else
            {
                SyncTalk(talk);
            }
        }

        private int IPCAddTalkTask21(string TalkText)
        {
            IPCAddTalkTask01(TalkText);
            return 0;
        }

        private int IPCAddTalkTask23(string TalkText, int iSpeed, int iTone, int iVolume, int vType)
        {
            IPCAddTalkTask02(TalkText, iSpeed, iVolume, vType);
            return 0;
        }

        private void IPCClearTalkTask()
        {
            MessQueue.ClearQueue();
        }

        private void IPCSkipTalkTask()
        {
        }

    }
}
