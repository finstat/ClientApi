using System;
using System.Collections.Generic;
using System.Text;

namespace FinstatApi.ViewModel
{
    public class KeyValue
    {
        public string Key { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0}:{1}", Key, Value);
        }
    }
}
