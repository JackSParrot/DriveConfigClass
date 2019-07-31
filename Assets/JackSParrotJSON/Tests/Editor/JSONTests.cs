using System;
using NUnit.Framework;

namespace JackSParrot.JSON.Test
{
    [TestFixture]
    public class JSONTests
    {
        [SetUp]
        public void Init()
        {
        }

        [Test]
        public void TestParse()
        {
            var parsed = JSON.LoadString(StoredTestJsons.kLarge);
            Assert.IsTrue(parsed != null && !parsed.IsEmpty());
        }

        [Test]
        public void TestSerialize()
        {
            var serialized = TestClasses.CreateTestClassJson().ToString();
            Assert.IsTrue(!string.IsNullOrEmpty(serialized));
        }

        [Test]
        public void TestEquals()
        {
            var parsed = JSON.LoadString(StoredTestJsons.kLarge);
            var secondParsed = JSON.LoadString(StoredTestJsons.kLarge);
            bool equals1 = parsed.Equals(secondParsed);
            bool equals2 = parsed.ToString().Equals(secondParsed.ToString(), StringComparison.InvariantCultureIgnoreCase);
            Assert.IsTrue(equals1);
            Assert.IsTrue(equals2);
        }

        [Test]
        public void TestCopy()
        {
            var parsed = JSON.LoadString(StoredTestJsons.kLarge);
            var copy = parsed.Clone();
            Assert.IsTrue(copy.Equals(parsed));
            Assert.IsTrue(copy.ToString().Equals(parsed.ToString(), StringComparison.InvariantCultureIgnoreCase));
        }

        [Test]
        public void TestParseTypes()
        {
            var parsed = JSON.LoadString(StoredTestJsons.kTypes);
            Assert.IsTrue(parsed != null);
            Assert.IsTrue(parsed.GetJSONType() == JSONType.Object);

            foreach (var kvp in parsed.AsObject())
            {
                switch (kvp.Key)
                {
                    case "int":
                        Assert.IsTrue(kvp.Value.GetJSONType() == JSONType.Array);
                        foreach (var item in kvp.Value.AsArray())
                        {
                            Assert.IsTrue(item.AsValue().GetValueType() == JSONValueType.Int);
                        }
                        break;
                    case "long":
                        Assert.IsTrue(kvp.Value.GetJSONType() == JSONType.Array);
                        foreach (var item in kvp.Value.AsArray())
                        {
                            Assert.IsTrue(item.AsValue().GetValueType() == JSONValueType.Long);
                        }
                        break;
                    case "float":
                        Assert.IsTrue(kvp.Value.GetJSONType() == JSONType.Array);
                        foreach (var item in kvp.Value.AsArray())
                        {
                            Assert.IsTrue(item.AsValue().GetValueType() == JSONValueType.Float);
                        }
                        break;
                    case "bool":
                        Assert.IsTrue(kvp.Value.GetJSONType() == JSONType.Array);
                        foreach (var item in kvp.Value.AsArray())
                        {
                            Assert.IsTrue(item.AsValue().GetValueType() == JSONValueType.Bool);
                        }
                        break;
                    case "string":
                        Assert.IsTrue(kvp.Value.GetJSONType() == JSONType.Array);
                        foreach (var item in kvp.Value.AsArray())
                        {
                            Assert.IsTrue(item.AsValue().GetValueType() == JSONValueType.String);
                        }
                        break;
                    case "null":
                        foreach (var item in kvp.Value.AsArray())
                        {
                            Assert.IsTrue(item.AsValue().GetValueType() == JSONValueType.Empty);
                        }
                        break;
                }
            }
        }

        [Test]
        public void TestValues()
        {
            int int_const = 1234;
            JSON int_json = int_const;
            int int_value = int_json;
            Assert.IsTrue(int_json.GetJSONType() == JSONType.Value);
            Assert.IsTrue(int_json.AsValue().GetValueType() == JSONValueType.Int);
            Assert.IsTrue(int_json.AsValue().ToInt() == int_const);
            Assert.IsTrue(int_json == int_const);
            Assert.IsTrue(int_json == int_value);

            long long_const = (long)int.MaxValue + 1;
            JSON long_json = long_const;
            long long_value = long_json;
            Assert.IsTrue(long_json.GetJSONType() == JSONType.Value);
            Assert.IsTrue(long_json.AsValue().GetValueType() == JSONValueType.Long);
            Assert.IsTrue(long_json.AsValue().ToLong() == long_const);
            Assert.IsTrue(long_json == long_const);
            Assert.IsTrue(long_json == long_value);

            float float_const = 0.0f;
            JSON float_json = float_const;
            float float_value = float_json;
            Assert.IsTrue(float_json.GetJSONType() == JSONType.Value);
            Assert.IsTrue(float_json.AsValue().GetValueType() == JSONValueType.Float);
            Assert.IsTrue(float_json.AsValue().ToFloat() == float_const);
            Assert.IsTrue(float_json == float_const);
            Assert.IsTrue(float_json == float_value);

            string string_const = "this is a string";
            JSON string_json = string_const;
            string string_value = string_json;
            Assert.IsTrue(string_json.GetJSONType() == JSONType.Value);
            Assert.IsTrue(string_json.AsValue().GetValueType() == JSONValueType.String);
            Assert.IsTrue(string_json.AsValue().ToString() == string_const);
            Assert.IsTrue(string_json == string_const);
            Assert.IsTrue(string_json == string_value);
            
            bool bool_const = false;
            JSON bool_json = bool_const;
            bool bool_value = bool_json;
            Assert.IsTrue(bool_json.GetJSONType() == JSONType.Value);
            Assert.IsTrue(bool_json.AsValue().GetValueType() == JSONValueType.Bool);
            Assert.IsTrue(bool_json.AsValue().ToBool() == bool_const);
            Assert.IsTrue(bool_json == bool_const);
            Assert.IsTrue(bool_json == bool_value);

            var values = new JSONObject();
            {
                JSON item = 1;
                JSONInt explicitItem = 1;
                JSONInt created = new JSONInt(1);
                var assignedInt = new JSONInt().SetInt(1);
                var assignedLong = new JSONInt().SetLong(1L);
                var assignedfloat = new JSONInt().SetFloat(1f);
                var assignedBool = new JSONInt().SetBool(true);
                var assignedString = new JSONInt().SetString("1");
                JSON array = new JSONArray { 1, item, explicitItem, created, assignedInt, assignedLong, assignedfloat, assignedBool, assignedString };
                values["Int"] = array;
            }
            {
                JSON item = 1L;
                JSONLong explicitItem = 1L;
                JSONLong created = new JSONLong(1L);
                var assignedInt = new JSONLong().SetInt(1);
                var assignedLong = new JSONLong().SetLong(1L);
                var assignedfloat = new JSONLong().SetFloat(1f);
                var assignedBool = new JSONLong().SetBool(true);
                var assignedString = new JSONLong().SetString("1");
                JSON array = new JSONArray { 1L, item, explicitItem, created, assignedInt, assignedLong, assignedfloat, assignedBool, assignedString };
                values["Long"] = array;
            }
            {
                JSON item = 1f;
                JSONFloat explicitItem = 1f;
                JSONFloat created = new JSONFloat(1f);
                var assignedInt = new JSONFloat().SetInt(1);
                var assignedLong = new JSONFloat().SetLong(1L);
                var assignedfloat = new JSONFloat().SetFloat(1f);
                var assignedBool = new JSONFloat().SetBool(true);
                var assignedString = new JSONFloat().SetString("1");
                JSON array = new JSONArray { 1f, item, explicitItem, created, assignedInt, assignedLong, assignedfloat, assignedBool, assignedString };
                values["Float"] = array;
            }
            {
                JSON item = true;
                JSONBool explicitItem = true;
                JSONBool created = new JSONBool(true);
                var assignedInt = new JSONBool().SetInt(1);
                var assignedLong = new JSONBool().SetLong(1L);
                var assignedfloat = new JSONBool().SetFloat(1f);
                var assignedBool = new JSONBool().SetBool(true);
                var assignedString = new JSONBool().SetString("true");
                JSON array = new JSONArray { true, item, explicitItem, created, assignedInt, assignedLong, assignedfloat, assignedBool, assignedString };
                values["Bool"] = array;
            }
            {
                JSON item = "1";
                JSONString explicitItem = "1";
                JSONString created = new JSONString("1");
                var assignedInt = new JSONString().SetInt(1);
                var assignedLong = new JSONString().SetLong(1L);
                var assignedfloat = new JSONString().SetFloat(1f);
                var assignedBool = new JSONString().SetBool(true);
                var assignedString = new JSONString().SetString("1");
                JSON array = new JSONArray { "1", item, explicitItem, created, assignedInt, assignedLong, assignedfloat, assignedBool, assignedString };
                values["String"] = array;
            }
            foreach (var kvp in values)
            {
                for (int i = 0; i < kvp.Value.Count - 1; ++i)
                {
                    var a = kvp.Value[i];
                    var b = kvp.Value[i + 1];
                    var equal = a == b;
                    if (kvp.Key == "String")
                    {
                        if ((a == "true" && b == "1") || (a == "1" && b == "true"))
                        {
                            equal = true;
                        }
                    }
                    Assert.IsTrue(equal);
                }
            }
        }

        [Test]
        public void TestToJSON()
        {
            var tclass = TestClasses.CreateTestClass();
            var item = JSON.ToJSON(tclass);
            var control = TestClasses.CreateTestClassJson();
            Assert.IsTrue(item == control);
        }

        [Test]
        public void TestFromJSON()
        {
            var original = TestClasses.CreateTestClass();
            var control = TestClasses.CreateTestClassJson();
            var tclass = JSON.FromJSON<TestClasses.TestClass>(control);
            Assert.IsTrue(original.Equals(tclass));
        }
    }
}

