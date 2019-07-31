namespace JackSParrot.JSON
{
    [System.Serializable]
    public abstract class JSONValue : JSON
    {
        protected const string kTrue = "true";
        protected const string kFalse = "false";
        protected const string kNull = "null";
        protected const string kZero = "0";
        protected const string kOne = "1";

        JSONValueType _valueType;

        protected JSONValue(JSONValueType valueType) : base(JSONType.Value)
        {
            _valueType = valueType;
        }

        public JSONValueType GetValueType()
        {
            return _valueType;
        }

        public virtual JSONValue SetInt(int value)
        {
            return new JSONInt(value);
        }

        public virtual JSONValue SetLong(long value)
        {
            return new JSONLong(value);
        }

        public virtual JSONValue SetFloat(float value)
        {
            return new JSONFloat(value);
        }

        public virtual JSONValue SetBool(bool value)
        {
            return new JSONBool(value);
        }

        public virtual JSONValue SetString(string value)
        {
            return new JSONString(value);
        }

        public virtual int ToInt()
        {
            return 0;
        }

        public virtual bool ToBool()
        {
            return ToInt() != 0;
        }

        public virtual long ToLong()
        {
            return (long)ToInt();
        }

        public virtual float ToFloat()
        {
            return (float)ToInt();
        }

        public override JSONValue AsValue()
        {
            return this;
        }

        public override JSONValue ToValue()
        {
            return Clone().AsValue();
        }
    }
}

