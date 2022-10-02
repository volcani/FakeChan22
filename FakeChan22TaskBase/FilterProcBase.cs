using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FakeChan22.Filters
{
    [DataContract]
    public class FilterProcBase
    {

        /// <summary>
        /// フィルタ処理を実行
        /// </summary>
        /// <param name="fp">処理対象のデータ</param>
        public virtual void Processing(ref FilterParams fp)
        {
            //
        }

        public FilterProcBase Clone()
        {
            return (FilterProcBase)MemberwiseClone();
        }

    }
}
