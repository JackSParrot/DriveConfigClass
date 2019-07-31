namespace JackSParrot.JSON
{
    [System.Serializable]
    public class JSONLong : JSONValue
    {
        long _value;

        public static implicit operator long(JSONLong data) => data.ToLong();
        public static implicit operator JSONLong(long data) => new JSONLong(data);

        public JSONLong(long value = 0L) : base(JSONValueType.Long)
        {
            SetLong(value);
        }

        public override JSONValue SetInt(int value)
        {
            return SetLong((long)value);
        }

        public override JSONValue SetLong(long value)
        {
            _value = value;
            return this;
        }

        public override JSONValue SetFloat(float value)
        {
            return SetLong((long)value);
        }

        public override JSONValue SetBool(bool value)
        {
            return SetInt(value ? 1 : 0);
        }

        public override JSONValue SetString(string value)
        {
            long outVal;
            if (!long.TryParse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out outVal))
            {
                outVal = 0;
            }
            return SetLong(outVal);
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
            return (int)_value;
        }

        public override long ToLong()
        {
            return _value;
        }

        public override float ToFloat()
        {
            return (float)_value;
        }

        public override JSON Clone()
        {
            return new JSONLong(_value);
        }

        public override bool Equals(JSON other)
        {
            return base.Equals(other) || (other.GetJSONType() == JSONType.Value && other.AsValue().GetValueType() == JSONValueType.Long && (other.AsValue() as JSONLong)._value == _value);
        }
    }
}

