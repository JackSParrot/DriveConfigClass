namespace JackSParrot.JSON
{
    [System.Serializable]
    public class JSONString : JSONValue
    {
        string _value;

        public JSONString(string value) : base(JSONValueType.String)
        {
            _value = value;
        }

        public override void SetString(string value)
        {
            _value = value;
        }

        public override void SetInt(int value)
        {
            SetString(value.ToString());
        }

        public override void SetBool(bool value)
        {
            SetString(value ? kTrue : kFalse);
        }

        public override void SetLong(long value)
        {
            SetString(value.ToString());
        }

        public override void SetFloat(float value)
        {
            SetString(value.ToString());
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
    }
}

