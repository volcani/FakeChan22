using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeChan22.Tasks
{
    public interface ITask
    {
        void Dispose();

        void TaskStart();

        void TaskStop();


    }
}
