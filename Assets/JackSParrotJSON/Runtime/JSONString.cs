namespace JackSParrot.JSON
{
    [System.Serializable]
    public class JSONString : JSONValue
    {
        string _value;

        public static implicit operator string(JSONString data) => data.ToString();
        public static implicit operator JSONString(string data) => new JSONString(data);

        public JSONString(string value = "") : base(JSONValueType.String)
        {
            SetString(value);
        }

        public override JSONValue SetInt(int value)
        {
            return SetString(value.ToString());
        }

        public override JSONValue SetLong(long value)
        {
            return SetString(value.ToString());
        }

        public override JSONValue SetFloat(float value)
        {
            return SetString(value.ToString());
        }

        public override JSONValue SetBool(bool value)
        {
            return SetString(value ? kTrue : kFalse);
        }

        public override JSONValue SetString(string value)
        {
            _value = value;
            return this;
        }

        public override string ToString()
        {
            return _value;
        }

        public override void Serialize(System.Text.StringBuilder sb)
        {
            sb.Append('\"');
            sb.Append(_value);
            sb.Append('\"');
        }

        public override bool ToBool()
        {
            return _value.Equals(kTrue, System.StringComparison.InvariantCultureIgnoreCase) ||
                _value.Equals(kOne, System.StringComparison.InvariantCultureIgnoreCase);
        }

        public override int ToInt()
        {
            int retVal = 0;
            return int.TryParse(_value, out retVal) ? retVal : 0;
        }

        public override long ToLong()
        {
            long retVal = 0;
            return long.TryParse(_value, out retVal) ? retVal : 0;
        }

        public override float ToFloat()
        {
            float retVal = 0.0f;
            return float.TryParse(_value, out retVal) ? retVal : 0.0f;
        }

        public override JSON Clone()
        {
            return new JSONString(_value);
        }

        public override bool Equals(JSON other)
        {
            return base.Equals(other) || (other.GetJSONType() == JSONType.Value && other.AsValue().GetValueType() == JSONValueType.String && (other.AsValue() as JSONString)._value.Equals(_value));
        }
    }
}

