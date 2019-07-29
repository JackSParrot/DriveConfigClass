namespace JackSParrot.JSON
{
    [System.Serializable]
    public class JSONInt : JSONValue
    {
        int _value;

        public JSONInt(int value) : base(JSONValueType.Int)
        {
            _value = value;
        }

        public override void SetString(string value)
        {
            if(!int.TryParse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out _value))
            {
                _value = 0;
            }
        }

        public override void SetInt(int value)
        {
            _value = value;
        }

        public override void SetBool(bool value)
        {
            SetInt(value ? 1 : 0);
        }

        public override void SetLong(long value)
        {
            SetInt((int)value);
        }

        public override void SetFloat(float value)
        {
            SetInt((int)value);
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
    }
}