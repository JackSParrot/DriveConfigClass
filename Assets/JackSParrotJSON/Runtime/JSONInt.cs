namespace JackSParrot.JSON
{
    [System.Serializable]
    public class JSONInt : JSONValue
    {
        int _value;

        public static implicit operator int(JSONInt data) => data.ToInt();
        public static implicit operator JSONInt(int data) => new JSONInt(data);

        public JSONInt(int value = 0) : base(JSONValueType.Int)
        {
            SetInt(value);
        }

        public override JSONValue SetInt(int value)
        {
            _value = value;
            return this;
        }

        public override JSONValue SetLong(long value)
        {
            return SetInt((int)value);
        }

        public override JSONValue SetFloat(float value)
        {
            return SetInt((int)value);
        }

        public override JSONValue SetBool(bool value)
        {
            return SetInt(value ? 1 : 0);
        }

        public override JSONValue SetString(string value)
        {
            int toParse;
            if (!int.TryParse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out toParse))
            {
                toParse = 0;
            }
            return SetInt(toParse);
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public override void Serialize(System.Text.StringBuilder sb)
        {
            sb.Append(_value);
        }

        public override bool ToBool()
        {
            return _value != 0;
        }

        public override int ToInt()
        {
            return _value;
        }

        public override long ToLong()
        {
            return (long)_value;
        }

        public override float ToFloat()
        {
            return (float)_value;
        }

        public override JSON Clone()
        {
            return new JSONInt(_value);
        }

        public override bool Equals(JSON other)
        {
            return base.Equals(other) || (other.GetJSONType() == JSONType.Value && other.AsValue().GetValueType() == JSONValueType.Int && (other.AsValue() as JSONInt)._value == _value);
        }
    }
}