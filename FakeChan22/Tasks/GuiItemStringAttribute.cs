using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace FakeChan22.Plugins
{
    [DataContract]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    internal sealed class GuiItemAttribute : Attribute
    {
        [DataMember]
        public string ParamName { get; set; }

        [DataMember]
        public string Description { get; set; } = "";

        public static GuiItemAttribute Get(MemberInfo p)
        {
            if (p == null) return null;

            ICustomAttributeProvider provider = p as ICustomAttributeProvider;

            var attributes = provider.GetCustomAttributes(typeof(GuiItemAttribute), false) as GuiItemAttribute[];

            return ( attributes != null && attributes.Any()) ? attributes[0] : null;
        }
    }
}
