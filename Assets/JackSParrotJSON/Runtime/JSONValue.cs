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

        public virtual void SetString(string value)
        {
        }

        public virtual void SetInt(int value)
        {
        }

        public virtual void SetBool(bool value)
        {
            SetInt(value ? 1 : 0);
        }

        public virtual void SetLong(long value)
        {
            SetInt((int)value);
        }

        public virtual void SetFloat(float value)
        {
            SetInt((int)value);
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

        public override bool Equals(JSON other)
        {
            if(other.GetJSONType() != JSONType.Value)
            {
                return false;
            }

            var otherValue = other.AsValue();
            if(otherValue._valueType != _valueType)
            {
                return false;
            }
            switch(_valueType)
            {
            case JSONValueType.Bool:
                return otherValue.ToBool() == ToBool();
            case JSONValueType.String:
                return otherValue.ToString().Equals(ToString(), System.StringComparison.InvariantCultureIgnoreCase);
            case JSONValueType.Int:
                return otherValue.ToInt() == ToInt();
            case JSONValueType.Long:
                return otherValue.ToLong() == ToLong();
            case JSONValueType.Float:
                return System.Math.Abs(otherValue.ToFloat() - ToFloat()) < float.Epsilon;
            case JSONValueType.Empty:
                return true;
            }
            return base.Equals(other);
        }
    }
}

