using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeChan22.Backends
{
    internal class BackendItem
    {
        public string Name { get; private set; }
        public bool Connected { get; private set; }
        public object BackendObject { get; private set; }

        public BackendItem(string name, bool status, object backendobj)
        {
            Name = name;
            Connected = status;
            BackendObject = backendobj;
        }
    }
}
