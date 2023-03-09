using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

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

        public DallEHelper(string imagePrompt, int imageSize)
        {
            prompt = new()
            {
                prompt = imagePrompt,
                n = 1,
                size = $"{imageSize}x{imageSize}"
            };
        }

        /// <summary>
        /// Generate an image based on prompt
        /// </summary>
        /// <param name="setting">OpenAI settings</param>
        /// <param name="saveFile">Save generated string as a file</param>
        /// <param name="callback">Callback action</param>
        /// <returns></returns>
        public IEnumerator GenerateTexture(OpenAiSetting setting, bool saveFile = false, Action callback = null)
        {
            Debug.Log($"{nameof(DallEHelper)} generating image...");

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

            if (saveFile)
            {
                AladdinUnityUtil.CreateTextureFile(tex);
            }

            if (callback != null)
            {
                callback();
            }
        }
    }
}