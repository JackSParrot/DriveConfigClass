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
            GameDataHolder.Rarities.Add(JSON.FromJSON<Rarity>(obj));
        }
        var skillsData = data["Skills"].AsArray();
        GameDataHolder.Skills.Clear();
        foreach (var entry in skillsData)
        {
            var obj = entry.AsObject();
            GameDataHolder.Skills.Add(JSON.FromJSON<Skill>(obj));
        }
        var cardsData = data["Cards"].AsArray();
        GameDataHolder.Cards.Clear();
        foreach (var entry in cardsData)
        {
            var obj = entry.AsObject();
            GameDataHolder.Cards.Add(JSON.FromJSON<Card>(obj));
        }
    }
}
