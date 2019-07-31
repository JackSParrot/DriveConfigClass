using System;
using System.Collections.Generic;

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

    [Serializable]
    public abstract class JSON : IEquatable<JSON>
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

        public static JSONObject ToJSON<T>(T obj)
        {
            return JSONClass.ToJSON(obj).AsObject();
        }

        public static T FromJSON<T>(JSON obj)
        {
            return JSONClass.FromJSON<T>(obj.AsObject());
        }

        public static implicit operator int(JSON data) => data.ToValue().ToInt();
        public static implicit operator long(JSON data) => data.ToValue().ToLong();
        public static implicit operator float(JSON data) => data.ToValue().ToFloat();
        public static implicit operator bool(JSON data) => data.ToValue().ToBool();
        public static implicit operator string(JSON data) => data.ToString();

        public static implicit operator int[] (JSON data) => data.AsArray().ToIntArray();
        public static implicit operator long[] (JSON data) => data.AsArray().ToLongArray();
        public static implicit operator float[] (JSON data) => data.AsArray().ToFloatArray();
        public static implicit operator bool[] (JSON data) => data.AsArray().ToBoolArray();
        public static implicit operator string[] (JSON data) => data.AsArray().ToStringArray();
        public static implicit operator List<int>(JSON data) => data.AsArray().ToIntList();
        public static implicit operator List<long>(JSON data) => data.AsArray().ToLongList();
        public static implicit operator List<float>(JSON data) => data.AsArray().ToFloatList();
        public static implicit operator List<bool>(JSON data) => data.AsArray().ToBoolList();
        public static implicit operator List<string>(JSON data) => data.AsArray().ToStringList();

        public static implicit operator Dictionary<string, int>(JSON data) => data.AsObject().ToIntDictionary();
        public static implicit operator Dictionary<string, long>(JSON data) => data.AsObject().ToLongDictionary();
        public static implicit operator Dictionary<string, float>(JSON data) => data.AsObject().ToFloatDictionary();
        public static implicit operator Dictionary<string, bool>(JSON data) => data.AsObject().ToBoolDictionary();
        public static implicit operator Dictionary<string, string>(JSON data) => data.AsObject().ToStringDictionary();

        public static implicit operator JSON(int data) => new JSONInt(data);
        public static implicit operator JSON(long data) => new JSONLong(data);
        public static implicit operator JSON(float data) => new JSONFloat(data);
        public static implicit operator JSON(bool data) => new JSONBool(data);
        public static implicit operator JSON(string data) => new JSONString(data);

        public static implicit operator JSON(JSON[] data) => new JSONArray(data);
        public static implicit operator JSON(int[] data) => new JSONArray(data);
        public static implicit operator JSON(long[] data) => new JSONArray(data);
        public static implicit operator JSON(float[] data) => new JSONArray(data);
        public static implicit operator JSON(bool[] data) => new JSONArray(data);
        public static implicit operator JSON(string[] data) => new JSONArray(data);
        public static implicit operator JSON(List<JSON> data) => new JSONArray(data);
        public static implicit operator JSON(List<int> data) => new JSONArray(data);
        public static implicit operator JSON(List<long> data) => new JSONArray(data);
        public static implicit operator JSON(List<float> data) => new JSONArray(data);
        public static implicit operator JSON(List<bool> data) => new JSONArray(data);
        public static implicit operator JSON(List<string> data) => new JSONArray(data);

        public static implicit operator JSON(Dictionary<string, JSON> data) => new JSONObject(data);
        public static implicit operator JSON(Dictionary<string, int> data) => new JSONObject(data);
        public static implicit operator JSON(Dictionary<string, long> data) => new JSONObject(data);
        public static implicit operator JSON(Dictionary<string, float> data) => new JSONObject(data);
        public static implicit operator JSON(Dictionary<string, bool> data) => new JSONObject(data);
        public static implicit operator JSON(Dictionary<string, string> data) => new JSONObject(data);

        public JSON this[string key]
        {
            get
            {
                if (GetJSONType() == JSONType.Object)
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

        public int Count
        {
            get
            {
                if (GetJSONType() == JSONType.Array)
                {
                    return AsArray().GetCount();
                }
                if (GetJSONType() == JSONType.Object)
                {
                    return AsObject().GetCount();
                }
                return 1;
            }
        }

        public Dictionary<string, JSON>.KeyCollection Keys
        {
            get
            {
                if(GetJSONType() == JSONType.Object)
                {
                    return AsObject().GetKeys();
                }
                return null;
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

        public bool IsEmpty()
        {
            return GetJSONType() == JSONType.Value && AsValue().GetValueType() == JSONValueType.Empty;
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