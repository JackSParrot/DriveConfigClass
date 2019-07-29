using System.Collections.Generic;

namespace JackSParrot.JSON
{
    [System.Serializable]
    public class JSONArray : JSON
    {
        List<JSON> _values;

        public int Count
        {
            get
            {
                return _values.Count;
            }
        }

        public JSONArray() : base(JSONType.Array)
        {
            _values = new List<JSON>();
        }

        public List<JSON>.Enumerator GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public JSON GetAt(int index)
        {
            return _values[index];
        }

        public void SetAt(int index, JSON value)
        {
            _values[index] = value;
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