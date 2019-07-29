namespace JackSParrot.JSON
{
    [System.Serializable]
    public class JSONEmpty : JSONValue
    {
        public JSONEmpty() : base(JSONValueType.Empty)
        {
        }

        public override string ToString()
        {
            return kNull;
        }

        public override void Serialize(System.Text.StringBuilder sb)
        {
            sb.Append(kNull);
        }

        public override JSON Clone()
        {
            return new JSONEmpty();
        }
    }
}