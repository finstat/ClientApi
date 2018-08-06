using System;
using System.Collections.Generic;
using System.Text;

namespace FinstatApi.ViewModel
{
    public class KeyValue
    {
        public string Key { get; set; }
        public object Value { get; set; }

        public KeyValue() : this(null, null)
        {}

        public KeyValue(string key = null, object value = null)
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", Key, Value);
        }
    }
}
