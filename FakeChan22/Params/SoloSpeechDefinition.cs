using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FakeChan22.Params
{
    [DataContract]
    public class SoloSpeechDefinition : INotifyPropertyChanged
    {
        private const int pastTimeMax = (2 * 24 * 60 * 60);
        public event PropertyChangedEventHandler PropertyChanged;

        [DataMember] public SpeakerList speakerList;

        private bool isUse;
        [DataMember] public bool IsUse {
            get
            {
                return isUse;
            }
            set
            {
                isUse = value;
                NotifyPropertyChanged();
            }
        }

        private string title;
        [DataMember] public string Title
        {
            get
            {
                return string.Format(@"{0}秒経過後の発声メッセージ", pastTime);
            }
            set
            {
                title = value;
            }
        }

        private int pastTime;
        [DataMember] public int PastTime
        {
            get
            {
                return pastTime;
            }

            set
            {
                pastTime = AdjustTimeRange(value);
                NotifyPropertyChanged();
            }
        }

        private List<SoloSpeechMessage> messages;
        [DataMember] public List<SoloSpeechMessage> Messages
        {
            get
            {
                return messages;
            }

            set
            {
                messages = value;
                NotifyPropertyChanged();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SoloSpeechDefinition(int spchTime, ref SpeakerList speakerList)
        {
            IsUse = true;
            PastTime = spchTime;
            Messages = new List<SoloSpeechMessage>();
            this.speakerList = speakerList;
        }

        private int AdjustTimeRange(int pastTime)
        {
            int ans = 60;

            if (pastTime > pastTimeMax)
            {
                ans = pastTimeMax;
            }
            else if (pastTime < 60)
            {
                ans = 60;
            }
            else
            {
                ans = pastTime;
            }

            return ans;
        }

    }
}
