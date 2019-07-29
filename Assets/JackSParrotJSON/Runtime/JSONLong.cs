namespace JackSParrot.JSON
{
    [System.Serializable]
    public class JSONLong : JSONValue
    {
        long _value;

        public JSONLong(long value) : base(JSONValueType.Long)
        {
            _value = value;
        }

        public override void SetString(string value)
        {
            if(!long.TryParse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out _value))
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
            SetInt(value ? 1 : 0);
        }

        public override void SetLong(long value)
        {
            _value = value;
        }

        public override void SetFloat(float value)
        {
            SetLong((long)value);
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
    }
}

