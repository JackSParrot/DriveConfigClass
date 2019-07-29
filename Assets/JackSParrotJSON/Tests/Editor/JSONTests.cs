using System;
using NUnit.Framework;
using UnityEngine;

namespace JackSParrot.JSON.Test
{
    [TestFixture]
    public class JSONTests
    {
        string _rawLarge;
        string _rawTypes;

        [SetUp]
        public void Init()
        {
            string testFilesPath = System.IO.Path.Combine(Application.dataPath,
                                            System.IO.Path.Combine("JackSParrotJSON",
                                              System.IO.Path.Combine("Tests", "Editor")));
            string largeFilePath = System.IO.Path.Combine(testFilesPath, "example_large.json");
            string typesFilePath = System.IO.Path.Combine(testFilesPath, "example_types.json");

            _rawLarge = System.IO.File.ReadAllText(largeFilePath);
            _rawTypes = System.IO.File.ReadAllText(typesFilePath);
        }

        [Test]
        public void TestParseTypes()
        {
            var parsed = JSON.LoadString(_rawTypes);
            Assert.IsTrue(parsed != null);
            Assert.IsTrue(parsed.GetJSONType() == JSONType.Object);

            var parsedObj = parsed.AsObject();
            foreach(var kvp in parsedObj)
            {
                switch(kvp.Key)
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
        public void TestParse()
        {
            var parsed = JSON.LoadString(_rawLarge);
            Assert.IsTrue(parsed != null);
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
        }

        [Test]
        public void TestSerialize()
        {
            var parsed = JSON.LoadString(_rawLarge);
            var serialized = parsed.ToString();
            Assert.IsTrue(!string.IsNullOrEmpty(serialized));
        }

        [Test]
        public void TestEquals()
        {
            var parsed = JSON.LoadString(_rawLarge);
            var secondParsed = JSON.LoadString(_rawLarge);
            bool equals1 = parsed.Equals(secondParsed);
            bool equals2 = parsed.ToString().Equals(secondParsed.ToString(), StringComparison.InvariantCultureIgnoreCase);
            Assert.IsTrue(equals1);
            Assert.IsTrue(equals2);
        }

        [Test]
        public void TestCopy()
        {
            var parsed = JSON.LoadString(_rawLarge);
            var copy = parsed.Clone();
            Assert.IsTrue(copy.Equals(parsed));
            Assert.IsTrue(copy.ToString().Equals(parsed.ToString(), StringComparison.InvariantCultureIgnoreCase));
        }
    }
}

