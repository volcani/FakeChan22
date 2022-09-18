using FakeChan22.Tasks;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace FakeChan22
{
    public class TaskSocket : TaskBase, IDisposable
    {
        ListenerConfigSocket lsnrCfg = null;

        TcpListener tcpIpListener;

        public TaskSocket(ref ListenerConfigSocket lsrCfg, ref MessageQueueWrapper que)
        {
            lsnrCfg = lsrCfg;
            MessQueue = que;
            based = false;
        }

        public void Dispose()
        {
            lsnrCfg = null;
            MessQueue = null;
            tcpIpListener = null;
        }

        public override void TaskStart()
        {
            try
            {
                tcpIpListener = new TcpListener(IPAddress.Parse(lsnrCfg.Host), lsnrCfg.Port);
                tcpIpListener.Start();
                tcpIpListener.BeginAcceptTcpClient(new AsyncCallback(AcceptData), tcpIpListener);
            }
            catch (Exception e)
            {
                throw new Exception(string.Format(@"Socketリスナ起動でエラー : {0}", e.Message));
            }
        }

        public override void TaskStop()
        {
            try
            {
                tcpIpListener?.Stop();
            }
            catch(Exception)
            {
                //
            }
        }

        private void AcceptData(IAsyncResult result)
        {
            TcpListener listener = result.AsyncState as TcpListener;
            TcpClient client;

            // EndAcceptTcpClient()実行前にTcpListener.Stop()を実行するとコールバックされるのでトラップする
            try
            {
                client = listener.EndAcceptTcpClient(result);
            }
            catch(Exception)
            {
                return;
            }

            listener.BeginAcceptTcpClient(new AsyncCallback(AcceptData), listener);

            using (NetworkStream ns = client.GetStream())
            {
                using (BinaryReader br = new BinaryReader(ns))
                {
                    byte[] iCommandBuff = br.ReadBytes(2);
                    Int16 iCommand = BitConverter.ToInt16(iCommandBuff, 0); // コマンド

                    switch (iCommand)
                    {
                        case 0x0001: // 読み上げ指示

                            var sr = br;
                            MessageData talk = Parse0x0001(ref sr);

                            if (lsnrCfg.IsAsync)
                            {
                                AsyncTalk(talk);
                            }
                            else
                            {
                                SyncTalk(talk);
                            }

                            break;

                        case 0x0040: // 読み上げキャンセル
                            MessQueue.ClearQueue();
                            break;

                        case 0x0110: // 一時停止状態の取得
                            byte data1 = 0;
                            using (BinaryWriter bw = new BinaryWriter(ns))
                            {
                                bw.Write(data1);
                            }
                            break;

                        case 0x0120: // 音声再生状態の取得
                            byte data2 = MessQueue.count == 0 ? (byte)0 : (byte)1;
                            using (BinaryWriter bw = new BinaryWriter(ns))
                            {
                                bw.Write(data2);
                            }
                            break;

                        case 0x0130: // 残りタスク数の取得
                            byte[] data3 = BitConverter.GetBytes(MessQueue.count);
                            using (BinaryWriter bw = new BinaryWriter(ns))
                            {
                                bw.Write(data3);
                            }
                            break;

                        case 0x0010: // 一時停止
                        case 0x0020: // 一時停止の解除
                        case 0x0030: // 現在の行をスキップし次の行へ
                        default:
                            break;
                    }

                }
            }

            client.Close();
        }

        /// <summary>
        /// 0x0001コマンドのパース
        /// </summary>
        /// <param name="br">Socketのreader</param>
        /// <returns>パース後メッセージデータ</returns>
        private MessageData Parse0x0001(ref BinaryReader br)
        {
            byte[] iSpeedBuff = br.ReadBytes(2);
            byte[] iToneBuff = br.ReadBytes(2);
            byte[] iVolumeBuff = br.ReadBytes(2);
            byte[] iVoiceBuff = br.ReadBytes(2);
            byte[] bCode = br.ReadBytes(1); // 文字列エンコーディング
            byte[] iLengthBuff = br.ReadBytes(4);

            Int16 iSpeed = BitConverter.ToInt16(iSpeedBuff, 0);   // 速度
            Int16 iTone = BitConverter.ToInt16(iToneBuff, 0);     // トーン(ピッチ)
            Int16 iVolume = BitConverter.ToInt16(iVolumeBuff, 0); // 音量
            Int16 iVoice = BitConverter.ToInt16(iVoiceBuff, 0);   // 音声(声質)
            Int32 iLength = BitConverter.ToInt32(iLengthBuff, 0); // 文字列サイズ

            byte[] bMessage = br.ReadBytes(iLength);  // 文字列データ

            string TalkText = "";

            // エンコーディング適用
            switch (bCode[0])
            {
                case 0: // UTF8
                    TalkText = System.Text.Encoding.UTF8.GetString(bMessage, 0, iLength);
                    break;

                case 2: // CP932
                    TalkText = System.Text.Encoding.GetEncoding(932).GetString(bMessage, 0, iLength);
                    break;

                case 1: // 暫定で書いた
                    TalkText = System.Text.Encoding.Unicode.GetString(bMessage, 0, iLength);
                    break;
            }

            MessageData talk = new MessageData()
            {
                LsnrCfg = lsnrCfg,
                OrgMessage = TalkText,
                CompatSpeed = iSpeed,
                CompatTone = iTone,
                CompatVolume = iVolume,
                CompatVType = iVoice,
                TaskId = MessQueue.count + 1
            };

            return talk;
        }

    }
}
