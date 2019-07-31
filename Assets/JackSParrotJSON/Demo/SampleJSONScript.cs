using JackSParrot.JSON;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SampleJSONScript : MonoBehaviour
{
    public class Person : IEquatable<Person>
    {
        public string Name;
        public int Age;
        public bool Equals(Person other)
        {
            return Name == other.Name && Age == other.Age;
        }
    }
    public class Classroom : IEquatable<Classroom>
    {
        public Person Teacher;
        public List<Person> Students;

        public bool Equals(Classroom other)
        {
            if (!Teacher.Equals(other.Teacher) || Students.Count != other.Students.Count)
            {
                return false;
            }
            foreach (var student in Students)
            {
                if(!other.Students.Contains(student))
                {
                    return false;
                }
            }            
            return true;
        }
    }

    void CreationExample()
    {
        const string jsonString = @"{""Students"":[{""Name"":""Paco"", ""Age"":14},{""Name"":""Joan"", ""Age"":8},{""Name"":""Frank"", ""Age"":10}],""Teacher"":{""Name"":""Arnold"", ""Age"":43}}";
        /*
        {
            "Teacher": {"Name": "Arnold,"Age": 43 },
            "Students":[
                {"Name": "Paco", "Age": 14}
                {"Name": "Joan", "Age": 8}
                {"Name": "Frank", "Age": 10}
            ]
        }
        */
        Classroom classObj = new Classroom
        {
            Teacher = new Person { Name = "Arnold", Age = 43 },
            Students = new List<Person>
            {
                new Person{ Name = "Paco", Age = 14 },
                new Person{ Name = "Joan", Age = 8 },
                new Person{ Name = "Frank", Age = 10 },
            }
        };
        JSON classroomJson = new JSONObject
        {
            {"Teacher", new JSONObject{ { "Name", "Arnold" },{ "Age", 43 } } },
            {"Students", new JSONArray{
                new JSONObject{ { "Name", "Paco" }, { "Age", 14 } },
                new JSONObject{ { "Name", "Joan" }, { "Age", 8 } },
                new JSONObject{ { "Name", "Frank" }, { "Age", 10 } }
            }}
        };
        JSON classroomJsonParsed = JSON.LoadString(jsonString);
        JSON classroomSerialized = JSON.ToJSON(classObj);
        Classroom classroomParsed = JSON.FromJSON<Classroom>(classroomJsonParsed);
        Debug.Log("The parsed JSON is equal to the control: " + (classroomJson == classroomJsonParsed));
        Debug.Log("The serialized object is equal to the control: " + (classroomJson == classroomSerialized));
        Debug.Log("The Parsed object is equal to the control: " + classroomParsed.Equals(classObj));
        Debug.Log("The serialized JSON is: " + JSON.DumpString(classroomJson));
        Debug.Log("First student's name is: " + classroomJson["Students"][0]["Name"]);
    }

    void Start()
    {
        CreationExample();
    }
}
