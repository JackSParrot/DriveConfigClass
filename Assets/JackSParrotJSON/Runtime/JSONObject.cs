using System.Collections.Generic;

namespace JackSParrot.JSON
{
    [System.Serializable]
    public class JSONObject : JSON
    {
        Dictionary<string, JSON> _values;

        public Dictionary<string, JSON>.KeyCollection Keys
        {
            get
            {
                return _values.Keys;
            }
        }

        public Dictionary<string, JSON>.ValueCollection Values
        {
            get
            {
                return _values.Values;
            }
        }

        public int Count
        {
            get
            {
                return _values.Count;
            }
        }

        public JSONObject() : base(JSONType.Object)
        {
            _values = new Dictionary<string, JSON>();
        }

        public Dictionary<string, JSON>.Enumerator GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public void Add(string key, JSON value)
        {
            JSON item = value != null ? value : new JSONEmpty();
            if(!_values.ContainsKey(key))
            {
                _values.Add(key, item);
            }
            else
            {
                _values[key] = item;
            }
            SetChild(this, item);
        }

        public void Add(KeyValuePair<string, JSON> pair)
        {
            Add(pair.Key, pair.Value);
        }

        public bool ContainsKey(string key)
        {
            return Has(key);
        }

        public bool Has(string key)
        {
            return _values.ContainsKey(key);
        }

        public JSON Get(string key)
        {
            return _values.ContainsKey(key) ? _values[key] : new JSONEmpty();
        }

        public void Remove(string key)
        {
            if(Has(key))
            {
                var data = _values[key];
                _values.Remove(key);
                SetChild(null, data);
            }
        }

        public void Remove(JSON child)
        {
            using(var itr = GetEnumerator())
            {
                while(itr.MoveNext())
                {
                    var current = itr.Current.Value;
                    if(ReferenceEquals(current, child))
                    {
                        Remove(itr.Current.Key);
                        return;
                    }
                }
            }
        }

        public override JSONArray ToArray()
        {
            var retVal = new JSONArray();
            using(var itr = GetEnumerator())
            {
                while(itr.MoveNext())
                {
                    retVal.Add(itr.Current.Value.Clone());
                }
            }
            return retVal;
        }

        public override JSONObject ToObject()
        {
            return Clone().AsObject();
        }

        public override JSONObject AsObject()
        {
            return this;
        }

        public override JSONArray AsArray()
        {
            return ToArray();
        }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();
            Serialize(sb);
            return sb.ToString();
        }

        public override void Serialize(System.Text.StringBuilder sb)
        {
            sb.Append('{');
            bool first = true;
            using(var itr = GetEnumerator())
            {
                while(itr.MoveNext())
                {
                    var pair = itr.Current;
                    if(!first)
                    {
                        sb.Append(',');
                    }
                    sb.Append('\"');
                    sb.Append(pair.Key);
                    sb.Append('\"');
                    sb.Append(':');
                    pair.Value.Serialize(sb);
                    first = false;
                }
            }
            sb.Append('}');
        }

        public override JSON Clone()
        {
            var retVal = new JSONObject();
            retVal._values = new  Dictionary<string, JSON>(_values);
            retVal._type = _type;
            return retVal;
        }

        public override bool Equals(JSON other)
        {
            if(ReferenceEquals(this, other))
            {
                return true;
            }
            if(other.GetJSONType() != _type || other.AsObject().Count != Count)
            {
                return false;
            }
            var otherObj = other.AsObject();
            bool retVal = true;
            using(var itr = GetEnumerator())
            {
                while(retVal && itr.MoveNext())
                {
                    var current = itr.Current;
                    retVal &= otherObj.Has(current.Key) && current.Value.Equals(otherObj.Get(current.Key));
                }
            }
            return retVal;
        }
    }
}

