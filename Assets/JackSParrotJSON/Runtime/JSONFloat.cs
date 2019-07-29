namespace JackSParrot.JSON
{
    [System.Serializable]
    public class JSONFloat : JSONValue
    {
        float _value;

        public JSONFloat(float value) : base(JSONValueType.Float)
        {
            _value = value;
        }

        public override void SetString(string value)
        {
            if(!float.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out _value))
            {
                _value = 0;
            }
        }

        public override void SetInt(int value)
        {
            SetLong((long)value);
        }

        public override void SetBool(bool value)
        {
            SetFloat(value ? 1.0f : 0.0f);
        }

        public override void SetLong(long value)
        {
            SetFloat((float)value);
        }

        public override void SetFloat(float value)
        {
            _value = value;
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
    }
}