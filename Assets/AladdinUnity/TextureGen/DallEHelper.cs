using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.IO;
using Unity.VisualScripting;
using System;

public class Prompt
{
    public string prompt;
    public int n;
    public string size;
}

namespace AladdinTextureGen
{
    public class DallEHelper
    {
        private string url;
        private Prompt prompt;

        private bool isCompleted = true;
        public bool IsCompleted => isCompleted;

        public DallEHelper(string imagePrompt, int imageSize)
        {
            prompt = new Prompt();
            prompt.prompt = imagePrompt;
            prompt.n = 1;
            prompt.size = $"{imageSize}x{imageSize}";
        }

        public IEnumerator GenerateTexture(OpenAiSetting setting, string textureName, Action callback = null)
        {
            Debug.Log($"{nameof(DallEHelper)} generating image...");
            isCompleted = false;

            string json = JsonUtility.ToJson(prompt);

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
                    callback();
                    yield break;
                }

                url = request.downloadHandler.text.Split('"')[7];
            }

            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();

            Texture texture = DownloadHandlerTexture.GetContent(www);
            Texture2D tex = texture as Texture2D;

            string path = Path.Combine(Application.dataPath, $"{textureName}.jpg");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllBytes(path, tex.EncodeToJPG());

            isCompleted = true;

            if(callback != null)
            {
                callback();
            }

            Debug.Log($"{nameof(DallEHelper)} completed");
        }
    }
}