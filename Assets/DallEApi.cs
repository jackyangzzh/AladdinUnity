using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class Prompt
{
    public string prompt;
    public int n;
    public string imageSize;
}

public class DallEApi
{
    private string url;
    private Prompt prompt;

    private bool isCompleted = true;
    public bool IsCompleted => isCompleted;

    public DallEApi (string imagePrompt, int imageSize)
    {
        prompt = new Prompt();
        prompt.prompt = imagePrompt;
        prompt.n = 1;
        prompt.imageSize = $"{imageSize}x{imageSize}";
    }

    public IEnumerator GenerateTexture(ChatGPTSetting setting)
    {
        Debug.Log($"{nameof(DallEApi)} generating image...");
        isCompleted = false;

        string json = JsonUtility.ToJson(prompt);

        Debug.Log(json);

        using (var request = new UnityWebRequest("https://api.openai.com/v1/images/generations", "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {setting.APIKey}");
            request.SetRequestHeader("Accept", " text/plain");

            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
                yield break;
            }

            url = request.downloadHandler.text.Split('"')[7];
        }

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        Texture result = DownloadHandlerTexture.GetContent(www);
        isCompleted = true;
        Debug.Log($"{nameof(DallEApi)} completed");
    }
}