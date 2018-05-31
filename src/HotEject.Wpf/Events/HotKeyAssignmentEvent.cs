using HotEject.Core.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotEject.Wpf.Events
{
    public class HotKeyAssignmentEvent
    {
        public SerializableHotKey HotKey { get; set; }
        public HotKeyAssignmentEventType Type { get; set; }

        public HotKeyAssignmentEvent(SerializableHotKey hotKey, HotKeyAssignmentEventType type)
        {
            HotKey = hotKey;
            Type = type;
        }
    }
}
