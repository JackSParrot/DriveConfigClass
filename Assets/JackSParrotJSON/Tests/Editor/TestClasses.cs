using System;
using System.Collections.Generic;

namespace JackSParrot.JSON.Test
{
    public static class TestClasses
    {
        public class TestClassParent
        {
            public int BInt = -1;
        }

        public class TestClass : TestClassParent, IEquatable<TestClass>
        {
            public int AInt = 1;
            public long ALong = 1L;
            public float AFloat = 1f;
            public bool ABool = true;
            public string AString = "AString";
            public int[] AArray = { 0, 1, 2, 3 };
            public List<int> AList = new List<int> { 0, 1, 2, 3 };
            public Dictionary<string, int> ADict = new Dictionary<string, int> {
                {"zero", 0 },
                {"one", 1 },
                {"two", 2 },
                {"three", 3 }
            };
            public TestClass TestClassInstance;

            public bool Equals(TestClass other)
            {
                var equals = AInt == other.AInt;
                equals &= BInt == other.BInt;
                equals &= ALong == other.ALong;
                equals &= AFloat == other.AFloat;
                equals &= ABool == other.ABool;
                equals &= AString == other.AString;
                equals &= AArray.Length == other.AArray.Length;
                equals &= AList.Count == other.AList.Count;
                equals &= ADict.Count == other.ADict.Count;
                if (TestClassInstance != null && other.TestClassInstance != null)
                {
                    equals &= TestClassInstance.Equals(other.TestClassInstance);
                }

                if (equals)
                {
                    for (int i = 0; i < AArray.Length; ++i)
                    {
                        equals &= AArray[i] == other.AArray[i];
                    }
                    for (int i = 0; i < AList.Count; ++i)
                    {
                        equals &= AList[i] == other.AList[i];
                    }
                    foreach (var k in ADict.Keys)
                    {
                        equals &= ADict[k] == other.ADict[k];
                    }
                }

                return equals;
            }

            public override string ToString()
            {
                var retVal = "{" + "AInt: " + AInt + ", BInt: " + BInt + ", ALong: " + ALong + ", AFloat: " + AFloat + ", ABool: " + ABool + ", AString: " + AString + ", AArray:[";
                foreach (var item in AArray)
                {
                    retVal += item.ToString() + ", ";
                }
                retVal = retVal.Substring(0, retVal.Length - 2);
                retVal += "], AList: [";
                retVal += "], ADict: {";
                retVal += "}, TestClassInstance: " + (TestClassInstance == null ? "null" : TestClassInstance.ToString());
                retVal += "}";
                return retVal;
            }
        }

        public static TestClass CreateTestClass()
        {
            var tclass = new TestClass();
            tclass.TestClassInstance = new TestClass();
            return tclass;
        }

        public static JSONObject CreateTestClassJson()
        {
            var retVal = new JSONObject
            {
                { "BInt", -1 },
                { "AInt", 1 },
                { "ALong", 1L },
                { "AFloat", 1f },
                { "ABool", true },
                { "AString", "AString" },
                { "AArray", new int[]{ 0, 1, 2, 3 } },
                { "AList", new List<int>{ 0, 1, 2, 3 } },
                { "ADict", new Dictionary<string, int> {
                    { "zero", 0 },
                    { "one", 1 },
                    { "two", 2 },
                    { "three", 3 }}}
            };
            retVal["TestClassInstance"] = retVal.Clone();
            return retVal;
        }
    }
}
