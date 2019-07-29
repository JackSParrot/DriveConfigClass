namespace JackSParrot.JSON
{
    [System.Serializable]
    public class JSONBool : JSONValue
    {
        bool _value;

        public JSONBool(bool value) : base(JSONValueType.Bool)
        {
            _value = value;
        }

        public override void SetString(string value)
        {
            _value = value.Equals(kTrue, System.StringComparison.InvariantCultureIgnoreCase) ||
                value.Equals(kOne, System.StringComparison.InvariantCultureIgnoreCase);
        }

        public override void SetInt(int value)
        {
            SetBool(value != 0);
        }

        public override void SetBool(bool value)
        {
            _value = value;
        }

        public override void SetLong(long value)
        {
            SetBool(value != 0);
        }

        public override void SetFloat(float value)
        {
            SetBool(System.Math.Abs(value) > float.Epsilon);
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
    }
}