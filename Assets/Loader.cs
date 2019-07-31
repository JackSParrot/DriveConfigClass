using System.Collections;
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
                GameDataHolder.Config = JSON.FromJSON<GameConfig>(json["data"]);
            }
        }
    }
}
