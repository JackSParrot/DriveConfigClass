namespace JackSParrot.JSON
{
    [System.Serializable]
    public class JSONBool : JSONValue
    {
        bool _value;

        public static implicit operator bool(JSONBool data) => data.ToBool();
        public static implicit operator JSONBool(bool data) => new JSONBool(data);

        public JSONBool(bool value = false) : base(JSONValueType.Bool)
        {
            SetBool(value);
        }

        public override JSONValue SetInt(int value)
        {
           return  SetBool(value != 0);
        }

        public override JSONValue SetLong(long value)
        {
            return SetBool(value != 0);
        }

        public override JSONValue SetFloat(float value)
        {
            return SetBool(System.Math.Abs(value) > float.Epsilon);
        }

        public override JSONValue SetBool(bool value)
        {
            _value = value;
            return this;
        }

        public override JSONValue SetString(string value)
        {
            return SetBool(value.Equals(kTrue, System.StringComparison.InvariantCultureIgnoreCase) ||
                value.Equals(kOne, System.StringComparison.InvariantCultureIgnoreCase));
        }

        public override string ToString()
        {
            return _value ? kTrue : kFalse;
        }

        public override void Serialize(System.Text.StringBuilder sb)
        {
            sb.Append(_value ? kTrue : kFalse);
        }

        public override bool ToBool()
        {
            return _value;
        }

        public override int ToInt()
        {
            return _value ? 1 : 0;
        }

        public override long ToLong()
        {
            return _value ? 1 : 0;
        }

        public override float ToFloat()
        {
            return _value ? 1.0f : 0.0f;
        }

        public override JSON Clone()
        {
            return new JSONBool(_value);
        }

        public override bool Equals(JSON other)
        {
            return base.Equals(other) || (other.GetJSONType() == JSONType.Value && other.AsValue().GetValueType() == JSONValueType.Bool && (other.AsValue() as JSONBool)._value == _value);
        }
    }
}