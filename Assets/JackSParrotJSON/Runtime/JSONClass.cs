using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace JackSParrot.JSON
{
    public static class JSONClass
    {
        public static JSON ToJSON<T>(T obj)
        {
            if(obj == null)
            {
                return new JSONEmpty();
            }
            var type = obj.GetType();
            if (type == typeof(int))
            {
                return new JSONInt(Convert.ToInt32(obj));
            }
            if (type == typeof(long))
            {
                return new JSONLong(Convert.ToInt64(obj));
            }
            if (type == typeof(float))
            {
                return new JSONFloat((float)Convert.ToDouble(obj));
            }
            if (type == typeof(bool))
            {
                return new JSONBool(Convert.ToBoolean(obj));
            }
            if (type == typeof(string))
            {
                return new JSONString(Convert.ToString(obj));
            }
            JSONObject retVal = new JSONObject();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(int))
                {
                    retVal.Add(field.Name, (int)field.GetValue(obj));
                }
                else if (field.FieldType == typeof(long))
                {
                    retVal.Add(field.Name, (long)field.GetValue(obj));
                }
                else if (field.FieldType == typeof(float))
                {
                    retVal.Add(field.Name, (float)field.GetValue(obj));
                }
                else if (field.FieldType == typeof(bool))
                {
                    retVal.Add(field.Name, (bool)field.GetValue(obj));
                }
                else if (field.FieldType == typeof(string))
                {
                    retVal.Add(field.Name, (string)field.GetValue(obj));
                }
                else if (field.FieldType == typeof(long))
                {
                    retVal.Add(field.Name, (long)field.GetValue(obj));
                }
                else if (field.FieldType.IsArray || (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>)))
                {
                    var arr = new JSONArray();
                    foreach (var item in field.GetValue(obj) as IEnumerable)
                    {
                        var jsonItem = ToJSON(item);
                        if (!jsonItem.IsEmpty())
                        {
                            arr.Add(jsonItem);
                        }
                    }
                    if (arr.Count > 0)
                    {
                        retVal.Add(field.Name, arr);
                    }
                }
                else if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    var keyType = field.FieldType.GetGenericArguments()[0];
                    var valueType = field.FieldType.GetGenericArguments()[1];

                    if (keyType != typeof(string))
                    {
                        continue;
                    }
                    JSONObject dic = new JSONObject();
                    foreach (DictionaryEntry item in field.GetValue(obj) as IDictionary)
                    {
                        var jsonItem = ToJSON(item.Value);
                        if (!jsonItem.IsEmpty())
                        {
                            dic.Add((string)item.Key, jsonItem);
                        }
                    }
                    if (dic.Count > 0)
                    {
                        retVal.Add(field.Name, dic);
                    }
                }
                else
                {
                    var item = ToJSON(field.GetValue(obj));
                    if (!item.IsEmpty())
                    {
                        retVal.Add(field.Name, item);
                    }
                }
            }
            return retVal;
        }

        public static T FromJSON<T>(JSON json)
        {
            var retVal = (T)Activator.CreateInstance(typeof(T));
            var type = retVal.GetType();

            var fields = new List<FieldInfo>();
            var parent = type;
            do
            {
                fields.AddRange(parent.GetFields(BindingFlags.Public | BindingFlags.Instance));
                parent = parent.BaseType;
            }
            while (parent != null);
            foreach (var field in fields)
            {
                var value = json[field.Name];
                if(value.IsEmpty())
                {
                    continue;
                }
                if (field.FieldType == typeof(int))
                {
                    if (value.GetJSONType() == JSONType.Value && value.AsValue().GetValueType() == JSONValueType.Int)
                    {
                        if(type.IsValueType)
                        {
                            object refObj = retVal;
                            field.SetValue(refObj, value.AsValue().ToInt());
                            retVal = (T)refObj;
                            continue;
                        }
                        field.SetValue(retVal, value.AsValue().ToInt());
                    }
                    continue;
                }
                if (field.FieldType == typeof(long))
                {
                    if (value.GetJSONType() == JSONType.Value && value.AsValue().GetValueType() == JSONValueType.Long)
                    {
                        if (type.IsValueType)
                        {
                            object refObj = retVal;
                            field.SetValue(refObj, value.AsValue().ToLong());
                            retVal = (T)refObj;
                            continue;
                        }
                        field.SetValue(retVal, value.AsValue().ToLong());
                    }
                    continue;
                }
                if (field.FieldType == typeof(float))
                {
                    if (value.GetJSONType() == JSONType.Value && value.AsValue().GetValueType() == JSONValueType.Float)
                    {
                        if (type.IsValueType)
                        {
                            object refObj = retVal;
                            field.SetValue(refObj, value.AsValue().ToFloat());
                            retVal = (T)refObj;
                            continue;
                        }
                        field.SetValue(retVal, value.AsValue().ToFloat());
                    }
                    continue;
                }
                if (field.FieldType == typeof(bool))
                {
                    if (value.GetJSONType() == JSONType.Value && value.AsValue().GetValueType() == JSONValueType.Bool)
                    {
                        if (type.IsValueType)
                        {
                            object refObj = retVal;
                            field.SetValue(refObj, value.AsValue().ToBool());
                            retVal = (T)refObj;
                            continue;
                        }
                        field.SetValue(retVal, value.AsValue().ToBool());
                    }
                    continue;
                }
                if (field.FieldType == typeof(string))
                {
                    if (value.GetJSONType() == JSONType.Value && value.AsValue().GetValueType() == JSONValueType.String)
                    {
                        if (type.IsValueType)
                        {
                            object refObj = retVal;
                            field.SetValue(refObj, value.AsValue().ToString());
                            retVal = (T)refObj;
                            continue;
                        }
                        field.SetValue(retVal, value.AsValue().ToString());
                    }
                    continue;
                }
                if (field.FieldType.IsArray)
                {
                    if (value.GetJSONType() == JSONType.Array)
                    {
                        var arr = Array.CreateInstance(field.FieldType.GetElementType(), value.Count) as IList;
                        for (int i = 0; i < arr.Count; ++i)
                        {
                            if (field.FieldType.GetElementType() == typeof(int))
                            {
                                arr[i] = value[i].AsValue().ToInt();
                                continue;
                            }
                            if (field.FieldType.GetElementType() == typeof(long))
                            {
                                arr[i] = value[i].AsValue().ToLong();
                                continue;
                            }
                            if (field.FieldType.GetElementType() == typeof(float))
                            {
                                arr[i] = value[i].AsValue().ToFloat();
                                continue;
                            }
                            if (field.FieldType.GetElementType() == typeof(bool))
                            {
                                arr[i] = value[i].AsValue().ToBool();
                                continue;
                            }
                            if (field.FieldType.GetElementType() == typeof(string))
                            {
                                arr[i] = value[i].AsValue().ToString();
                                continue;
                            }
                            var method = typeof(JSONClass).GetMethod("FromJSON");
                            var genericMethod = method.MakeGenericMethod(field.FieldType.GetElementType());
                            arr[i] = genericMethod.Invoke(null, new [] { value[i] });
                        }
                        if (type.IsValueType)
                        {
                            object refObj = retVal;
                            field.SetValue(refObj, arr);
                            retVal = (T)refObj;
                            continue;
                        }
                        field.SetValue(retVal, arr);
                    }
                    continue;
                }
                if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    if (value.GetJSONType() == JSONType.Array)
                    {
                        var fieldType = field.FieldType;
                        var genericTypeDef = fieldType.GetGenericTypeDefinition();
                        var elementTypes = field.FieldType.GetGenericArguments();
                        var listType = genericTypeDef.MakeGenericType(elementTypes);
                        var arr = Activator.CreateInstance(listType) as IList;
                        foreach (var val in value.AsArray())
                        {
                            if (elementTypes[0] == typeof(int))
                            {
                                arr.Add(val.AsValue().ToInt());
                                continue;
                            }
                            if (elementTypes[0] == typeof(long))
                            {
                                arr.Add(val.AsValue().ToLong());
                                continue;
                            }
                            if (elementTypes[0] == typeof(float))
                            {
                                arr.Add(val.AsValue().ToFloat());
                                continue;
                            }
                            if (elementTypes[0] == typeof(bool))
                            {
                                arr.Add(val.AsValue().ToBool());
                                continue;
                            }
                            if (elementTypes[0] == typeof(string))
                            {
                                arr.Add(val.AsValue().ToString());
                                continue;
                            }
                            var method = typeof(JSONClass).GetMethod("FromJSON");
                            var genericMethod = method.MakeGenericMethod(elementTypes[0]);
                            arr.Add(genericMethod.Invoke(null, new [] { val }));
                        }
                        if (type.IsValueType)
                        {
                            object refObj = retVal;
                            field.SetValue(refObj, arr);
                            retVal = (T)refObj;
                            continue;
                        }
                        field.SetValue(retVal, arr);
                    }
                    continue;
                }
                if ((field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(Dictionary<,>)))
                {
                    if (value.GetJSONType() == JSONType.Object)
                    {
                        var fieldType = field.FieldType;
                        var genericTypeDef = fieldType.GetGenericTypeDefinition();
                        var elementTypes = field.FieldType.GetGenericArguments();
                        var dicType = genericTypeDef.MakeGenericType(elementTypes);
                        var dic = Activator.CreateInstance(dicType) as IDictionary;
                        foreach (var kvp in value.AsObject())
                        {
                            if (elementTypes[1] == typeof(int))
                            {
                                dic.Add(kvp.Key, kvp.Value.AsValue().ToInt());
                                continue;
                            }
                            if (elementTypes[1] == typeof(long))
                            {
                                dic.Add(kvp.Key, kvp.Value.AsValue().ToLong());
                                continue;
                            }
                            if (elementTypes[1] == typeof(float))
                            {
                                dic.Add(kvp.Key, kvp.Value.AsValue().ToFloat());
                                continue;
                            }
                            if (elementTypes[1] == typeof(bool))
                            {
                                dic.Add(kvp.Key, kvp.Value.AsValue().ToBool());
                                continue;
                            }
                            if (elementTypes[1] == typeof(string))
                            {
                                dic.Add(kvp.Key, kvp.Value.AsValue().ToString());
                                continue;
                            }
                            var method = typeof(JSONClass).GetMethod("FromJSON");
                            var genericMethod = method.MakeGenericMethod(kvp.Value.GetType());
                            var val = genericMethod.Invoke(null, new[] { kvp.Value });
                            dic.Add(kvp.Key, val);
                        }
                        if (type.IsValueType)
                        {
                            object refObj = retVal;
                            field.SetValue(refObj, dic);
                            retVal = (T)refObj;
                            continue;
                        }
                        field.SetValue(retVal, dic);
                    }
                    continue;
                }
                if(value.GetJSONType() == JSONType.Object)
                {
                    var method = typeof(JSONClass).GetMethod("FromJSON");
                    var genericMethod = method.MakeGenericMethod(field.FieldType);
                    if (type.IsValueType)
                    {
                        object refObj = retVal;
                        field.SetValue(refObj, genericMethod.Invoke(null, new[] { value }));
                        retVal = (T)refObj;
                        continue;
                    }
                    field.SetValue(retVal, genericMethod.Invoke(null, new [] { value }));
                }
            }
            return retVal;
        }
    }
}
