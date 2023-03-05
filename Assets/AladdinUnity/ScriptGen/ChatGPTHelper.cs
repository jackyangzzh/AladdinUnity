using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System.IO;
using Unity.EditorCoroutines.Editor;
using System.Globalization;
using System.Collections;
using System.Text;
using UnityEngine.Networking;
using System;

namespace AladdinScriptGen
{
    public enum Model
    {
        Davinci,
        Curie,
        None
    }

    public class ChatGPTReq
    {
        public string model;
        public string prompt;
        public int max_tokens;
        public float temperature;
    }

    [Serializable]
    public class Choices
    {
        public string text;
    }

    [Serializable]
    public class ChatGPTRes
    {
        public string id;
        public List<Choices> choices;
    }

    public class ChatGPTHelper
    {
        public bool IsCompleted => isCompleted;

        private bool isCompleted = false;
        private const string uri = "https://api.openai.com/v1/completions";

        private Prompt prompt = new();
        private string lastUserMsg;
        private string lastChatGPTMessage;
        private string selectedModel = "text-davinci-003";

        private string fileName = "Untitled";
        private AladdinUnityUtil.ScriptType scriptType;

        [Space(15)]
        public UnityEvent<string> chatGPTResponse = new();

        public ChatGPTHelper(string fileName, AladdinUnityUtil.ScriptType scriptType)
        {
            isCompleted = false;
            this.fileName = fileName;
            this.scriptType = scriptType;
        }

        public IEnumerator GenerateScript(string message, OpenAiSetting setting, Action callback = null)
        {
            isCompleted = false;

            if (selectedModel == null)
            {
                Debug.LogWarning($"{nameof(ChatGPTHelper)} [SendToChatGPT] Model name for ChatGPT's API is not set up yet.");
                //return;
            }

            Debug.Log($"[{nameof(ChatGPTHelper)}]: script generation starting...");

            lastUserMsg = message;

            switch (scriptType)
            {
                case AladdinUnityUtil.ScriptType.CSharp:
                    prompt.CurrentPrompt = AladdinUnityUtil.CSharpPrompt;
                    break;
                case AladdinUnityUtil.ScriptType.Shader:
                    prompt.CurrentPrompt = AladdinUnityUtil.ShaderPrompt;
                    break;
                default:
                    prompt.CurrentPrompt = AladdinUnityUtil.DefaultPrompt;
                    break;
            }

            prompt.AppendText(Prompt.Speaker.User, message);

            ChatGPTReq reqObj = new()
            {
                model = selectedModel,
                prompt = prompt.CurrentPrompt,
                max_tokens = setting.MaxToken,
                temperature = setting.Temperature,
            };

            string json = JsonUtility.ToJson(reqObj);

            using (var request = new UnityWebRequest(uri, "POST"))
            {
                request.SetRequestHeader("Authorization", $"Bearer {setting.APIKey}");
                request.SetRequestHeader("Content-Type", "application/json");

                request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
                request.downloadHandler = new DownloadHandlerBuffer();
                request.disposeDownloadHandlerOnDispose = true;
                request.disposeUploadHandlerOnDispose = true;

                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError(request.error);
                    callback();
                    yield break;
                }
                else
                {
                    var responseJson = JsonUtility.FromJson<ChatGPTRes>(request.downloadHandler.text);
                    ResolveResponse(responseJson);
                }

                request.Dispose();
            }

            if (callback != null)
            {
                callback();
            }
        }

        private void ResolveResponse(ChatGPTRes res)
        {
            lastChatGPTMessage = res.choices[0].text.TrimStart('\n').Replace("<|im_end|>", "");

            CreateScriptFile(res.choices[0].text);

            prompt.AppendText(Prompt.Speaker.ChatGPT, lastChatGPTMessage);
            chatGPTResponse.Invoke(lastChatGPTMessage);
        }

        private void CreateScriptFile(string inputText)
        {
            string fileExtension = "";
            switch (scriptType)
            {
                case AladdinUnityUtil.ScriptType.CSharp:
                    fileExtension = AladdinUnityUtil.CSharpExtension;
                    break;
                case AladdinUnityUtil.ScriptType.Shader:
                    fileExtension = AladdinUnityUtil.ShaderExtension;
                    break;
            }

            string path = Path.Combine(Application.dataPath, $"{fileName}.{fileExtension}");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllText(path, inputText);
            Debug.Log($"[{nameof(ChatGPTHelper)}] script created in: {path}");

            isCompleted = true;
        }
    }
}
