using System.Collections;
using System.Collections.Generic;

namespace JackSParrot.JSON
{
    [System.Serializable]
    public class JSONArray : JSON, IEnumerable<JSON>
    {
        List<JSON> _values;

        public JSONArray() : base(JSONType.Array)
        {
            _values = new List<JSON>();
        }

        public JSONArray(List<JSON> items) : base(JSONType.Array)
        {
            _values = new List<JSON>(items);
        }

        public JSONArray(List<int> items) : base(JSONType.Array)
        {
            _values = new List<JSON>();
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public JSONArray(List<long> items) : base(JSONType.Array)
        {
            _values = new List<JSON>();
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public JSONArray(List<float> items) : base(JSONType.Array)
        {
            _values = new List<JSON>();
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public JSONArray(List<bool> items) : base(JSONType.Array)
        {
            _values = new List<JSON>();
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public JSONArray(List<string> items) : base(JSONType.Array)
        {
            _values = new List<JSON>();
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public JSONArray(JSON[] items) : base(JSONType.Array)
        {
            _values = new List<JSON>(items);
        }

        public JSONArray(int[] items) : base(JSONType.Array)
        {
            _values = new List<JSON>();
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public JSONArray(long[] items) : base(JSONType.Array)
        {
            _values = new List<JSON>();
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public JSONArray(float[] items) : base(JSONType.Array)
        {
            _values = new List<JSON>();
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public JSONArray(bool[] items) : base(JSONType.Array)
        {
            _values = new List<JSON>();
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public JSONArray(string[] items) : base(JSONType.Array)
        {
            _values = new List<JSON>();
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public int[] ToIntArray()
        {
            var retVal = new int[_values.Count];
            for (int i = 0; i < _values.Count; ++i)
            {
                retVal[i] = _values[i].AsValue().ToInt();
            }
            return retVal;
        }

        public long[] ToLongArray()
        {
            var retVal = new long[_values.Count];
            for (int i = 0; i < _values.Count; ++i)
            {
                retVal[i] = _values[i].AsValue().ToLong();
            }
            return retVal;
        }

        public float[] ToFloatArray()
        {
            var retVal = new float[_values.Count];
            for (int i = 0; i < _values.Count; ++i)
            {
                retVal[i] = _values[i].AsValue().ToFloat();
            }
            return retVal;
        }

        public bool[] ToBoolArray()
        {
            var retVal = new bool[_values.Count];
            for (int i = 0; i < _values.Count; ++i)
            {
                retVal[i] = _values[i].AsValue().ToBool();
            }
            return retVal;
        }

        public string[] ToStringArray()
        {
            var retVal = new string[_values.Count];
            for (int i = 0; i < _values.Count; ++i)
            {
                retVal[i] = _values[i].AsValue().ToString();
            }
            return retVal;
        }

        public List<int> ToIntList()
        {
            var retVal = new List<int>();
            foreach (var value in _values)
            {
                retVal.Add(value.AsValue().ToInt());
            }
            return retVal;
        }

        public List<long> ToLongList()
        {
            var retVal = new List<long>();
            foreach (var value in _values)
            {
                retVal.Add(value.AsValue().ToLong());
            }
            return retVal;
        }

        public List<float> ToFloatList()
        {
            var retVal = new List<float>();
            foreach (var value in _values)
            {
                retVal.Add(value.AsValue().ToFloat());
            }
            return retVal;
        }

        public List<bool> ToBoolList()
        {
            var retVal = new List<bool>();
            foreach (var value in _values)
            {
                retVal.Add(value.AsValue().ToBool());
            }
            return retVal;
        }

        public List<string> ToStringList()
        {
            var retVal = new List<string>();
            foreach (var value in _values)
            {
                retVal.Add(value.AsValue().ToString());
            }
            return retVal;
        }

        public int GetCount()
        {
            return _values.Count;
        }

        public IEnumerator<JSON> GetEnumerator() => _values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        public void SetAt(int index, JSON value)
        {
            JSON item = value != null ? value : new JSONEmpty();
            SetChild(null, _values[index]);
            SetChild(this, item);
            _values[index] = item;
        }

        public JSON GetAt(int index)
        {
            return _values[index];
        }

        public void Add(JSON value)
        {
            JSON item = value != null ? value : new JSONEmpty();
            _values.Add(item);
            SetChild(this, item);
        }

        public void Remove(JSON data)
        {
            _values.Remove(data);
            SetChild(null, data);
        }

        public override JSONArray ToArray()
        {
            return Clone().AsArray();
        }

        public override JSONObject ToObject()
        {
            var retVal = new JSONObject();
            for(int i = 0; i < Count; ++i)
            {
                retVal.Add(i.ToString(), _values[i].Clone());
            }
            return retVal;
        }

        public override JSONObject AsObject()
        {
            return ToObject();
        }

        public override JSONArray AsArray()
        {
            return this;
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            Serialize(sb);
            return sb.ToString();
        }

        public override void Serialize(System.Text.StringBuilder sb)
        {
            sb.Append('[');
            bool first = true;
            for(int i = 0; i < _values.Count; ++i)
            {
                var current = _values[i];
                if(!first)
                {
                    sb.Append(',');
                }
                current.Serialize(sb);
                first = false;
            }
            sb.Append(']');
        }

        public override JSON Clone()
        {
            var retVal = new JSONArray();
            retVal._values = new List<JSON>(_values);
            retVal._type = _type;
            return retVal;
        }

        public override bool Equals(JSON other)
        {
            if(ReferenceEquals(this, other))
            {
                return true;
            }
            if(other.GetJSONType() != _type || other.AsArray().Count != Count)
            {
                return false;
            }
            var otherarray = other.AsArray();
            bool retVal = true;
            for(int i = 0; retVal && i < _values.Count; ++i)
            {
                retVal &= otherarray._values.Contains(_values[i]);
            }
            return retVal;
        }
    }
}