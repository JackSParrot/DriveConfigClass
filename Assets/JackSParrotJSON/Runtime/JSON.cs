namespace JackSParrot.JSON
{
    public enum JSONType
    {
        Object,
        Array,
        Value
    }

    public enum JSONValueType
    {
        Empty,
        String,
        Bool,
        Int,
        Long,
        Float
    }

    [System.Serializable]
    public abstract class JSON : System.IEquatable<JSON>
    {
        public static JSONObject LoadString(string data)
        {
            return new JSONParser().Parse(data) as JSONObject;
        }

        public static string DumpString(JSON data)
        {
            var sb = new System.Text.StringBuilder();
            data.Serialize(sb);
            return sb.ToString();
        }

        public static implicit operator string(JSON data) => data.ToString();
        public static implicit operator int(JSON data) => data.ToValue().ToInt();
        public static implicit operator long(JSON data) => data.ToValue().ToLong();
        public static implicit operator float(JSON data) => data.ToValue().ToFloat();
        public static implicit operator bool(JSON data) => data.ToValue().ToBool();

        public static implicit operator JSON(string data) => new JSONString(data);
        public static implicit operator JSON(int data) => new JSONInt(data);
        public static implicit operator JSON(long data) => new JSONLong(data);
        public static implicit operator JSON(float data) => new JSONFloat(data);
        public static implicit operator JSON(bool data) => new JSONBool(data);

        public JSON this[string key]
        {
            get
            {
                if(GetJSONType() == JSONType.Object)
                {
                    return AsObject().Get(key);
                }
                return this;
            }
            set
            {
                if (GetJSONType() == JSONType.Object)
                {
                    AsObject().Add(key, value);
                }
            }
        }

        public JSON this[int index]
        {
            get
            {
                if (GetJSONType() == JSONType.Array)
                {
                    return AsArray().GetAt(index);
                }
                return this;
            }
            set
            {
                if (GetJSONType() == JSONType.Array)
                {
                    AsArray().SetAt(index, value);
                }
            }
        }

        protected JSON _parent;
        public JSON Parent
        {
            get
            {
                return _parent;
            }
        }

        protected JSONType _type;

        protected JSON(JSONType type)
        {
            _parent = null;
            _type = type;
        }

        public JSONType GetJSONType()
        {
            return _type;
        }

        public virtual JSONValue ToValue()
        {
            return new JSONEmpty();
        }

        public virtual JSONObject ToObject()
        {
            return new JSONObject();
        }

        public virtual JSONArray ToArray()
        {
            return new JSONArray();
        }

        public virtual JSONValue AsValue()
        {
            return new JSONEmpty();
        }

        public virtual JSONObject AsObject()
        {
            return new JSONObject();
        }

        public virtual JSONArray AsArray()
        {
            return new JSONArray();
        }

        public virtual void Serialize(System.Text.StringBuilder sb)
        {

        }

        public static bool operator != (JSON json1, JSON json2)
        {
            if(ReferenceEquals(json1, json2))
            {
                return false;
            }
            if(!(json1 is null) && !(json2 is null))
            {
                return !json1.Equals(json2);
            }
            else
            {
                return true;
            }
        }

        public static bool operator == (JSON json1, JSON json2)
        {
            if(ReferenceEquals(json1, json2))
            {
                return true;
            }
            if(!(json1 is null) && !(json2 is null))
            {
                return json1.Equals(json2);
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            var otherAsJSON = obj as JSON;
            return otherAsJSON != null && Equals(otherAsJSON);
        }

        public virtual bool Equals(JSON other)
        {
            return ReferenceEquals(this, other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public virtual JSON Clone()
        {
            return new JSONEmpty();
        }

        public override string ToString()
        {
            return string.Empty;
        }

        protected static void SetChild(JSON parent, JSON child)
        {
            if(child.Parent != null && parent != null)
            {
                throw new System.Exception("Multiparent is not supported, consider cloning the object");
            }
            child._parent = parent;
        }
    }
}