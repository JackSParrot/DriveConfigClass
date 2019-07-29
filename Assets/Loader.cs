using System.Collections;
using System.Collections.Generic;
using JackSParrot.JSON;
using UnityEngine;
using UnityEngine.Networking;

public class Loader : MonoBehaviour
{
    const string kUrl = "https://script.google.com/macros/s/AKfycbxYPp9rr10JUMOacruvfJdUguRGNouSz-92ttfG-6LDj0jopOBX/exec?version=";

    public GameData GameDataHolder;

    void Start()
    {
		StartCoroutine(LoadConfig(GameDataHolder.Version));
    }

    IEnumerator LoadConfig(string version)
    {
        var downloadHandler = new DownloadHandlerBuffer();

        var request = new UnityWebRequest(kUrl+version);
        request.method = UnityWebRequest.kHttpVerbGET;
        request.useHttpContinue = false;
        request.chunkedTransfer = false;
        request.redirectLimit = 5;
        request.timeout = 60;
        request.downloadHandler = downloadHandler;

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("Network error: " + request.error);
        }
        else
        {
            var response = request.downloadHandler.text;
            Debug.Log("Network response: " + response);
			var json = JSON.LoadString(response);
            string result = json["result"];
            if(result == "new")
            {
                GameDataHolder.Version = json["version"];
                LoadGameData(json["data"].AsObject());
            }
        }
    }

    void LoadGameData(JSONObject data)
    {
        var globalData = data["Global"].AsArray();
        GameDataHolder.GlobalData.Clear();
        foreach (var entry in globalData)
        {
            var obj = entry.AsObject();
            GameDataHolder.GlobalData.Add(new GlobalDataEntry
            {
                Key = obj["Property"],
                Value = obj["Value"]
            });
        }
        var raritiesData = data["Rarities"].AsArray();
        GameDataHolder.Rarities.Clear();
        foreach (var entry in raritiesData)
        {
            var obj = entry.AsObject();
            GameDataHolder.Rarities.Add(new Rarity
            {
                Id = obj["Id"],
                Name = obj["Name"],
                Color = obj["Color"]
            });
        }
        var skillsData = data["Skills"].AsArray();
        GameDataHolder.Skills.Clear();
        foreach (var entry in skillsData)
        {
            var obj = entry.AsObject();
            GameDataHolder.Skills.Add(new Skill
            {
                Id = obj["Id"],
                Effect = obj["Effect"],
                Description = obj["Description"]
            });
        }
        var cardsData = data["Cards"].AsArray();
        GameDataHolder.Cards.Clear();
        foreach (var entry in cardsData)
        {
            var obj = entry.AsObject();
            var card = new Card
            {
                Id = obj["Id"],
                Title = obj["Title"],
                Text = obj["Text"],
                Cost = obj["Cost"],
                Attack = obj["Attack"],
                Defence = obj["Defence"],
                Rarity = obj["Rarity"],
                Skills = new List<int>()
            };
            var skillsArr = obj["Skills"].AsArray();
            foreach(var skill in skillsArr)
            {
                card.Skills.Add(skill);
            }
            GameDataHolder.Cards.Add(card);
        }
    }
}
