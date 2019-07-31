namespace JackSParrot.JSON
{
    [System.Serializable]
    public class JSONFloat : JSONValue
    {
        float _value;

        public static implicit operator float(JSONFloat data) => data.ToFloat();
        public static implicit operator JSONFloat(float data) => new JSONFloat(data);

        public JSONFloat(float value = 0f) : base(JSONValueType.Float)
        {
            SetFloat(value);
        }

        public override JSONValue SetInt(int value)
        {
            return SetFloat((float)value);
        }

        public override JSONValue SetLong(long value)
        {
            return SetFloat((float)value);
        }

        public override JSONValue SetFloat(float value)
        {
            _value = value;
            return this;
        }

        public override JSONValue SetBool(bool value)
        {
            return SetFloat(value ? 1.0f : 0.0f);
        }

        public override JSONValue SetString(string value)
        {
            float outVal;
            if (!float.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out outVal))
            {
                outVal = 0f;
            }
            return SetFloat(outVal);
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
            return System.Math.Abs(_value) > float.Epsilon;
        }

        public override int ToInt()
        {
            return (int)_value;
        }

        public override long ToLong()
        {
            return (long)_value;
        }

        public override float ToFloat()
        {
            return _value;
        }

        public override JSON Clone()
        {
            return new JSONFloat(_value);
        }

        public override bool Equals(JSON other)
        {
            return base.Equals(other) || (other.GetJSONType() == JSONType.Value && other.AsValue().GetValueType() == JSONValueType.Float && (other.AsValue() as JSONFloat)._value == _value);
        }
    }
}