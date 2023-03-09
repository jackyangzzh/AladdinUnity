using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

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
        private const string uri = "https://api.openai.com/v1/completions";

        private Prompt prompt = new();
        private string lastUserMsg;
        private string lastChatGPTMessage;
        private string selectedModel = "text-davinci-003";

        private AladdinUnityUtil.ScriptType scriptType;

        [Space(15)]
        public UnityEvent<string> chatGPTResponse = new();

        public ChatGPTHelper(AladdinUnityUtil.ScriptType scriptType)
        {
            this.scriptType = scriptType;
        }

        /// <summary>
        /// Generate strings based on message sent to ChatGPT
        /// </summary>
        /// <param name="message">Message to send to ChatGPT</param>
        /// <param name="setting">OpenAI settings</param>
        /// <param name="saveFile">Save generated string as a file</param>
        /// <param name="callback">Callback action</param>
        /// <returns></returns>
        public IEnumerator GenerateScript(string message, OpenAiSetting setting, bool saveFile = false, Action callback = null)
        {
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
                    lastChatGPTMessage = responseJson.choices[0].text.TrimStart('\n').Replace("<|im_end|>", "");

                    if (saveFile)
                    {
                        AladdinUnityUtil.CreateScriptFile(responseJson.choices[0].text, scriptType);
                    }

                    prompt.AppendText(Prompt.Speaker.ChatGPT, lastChatGPTMessage);
                    chatGPTResponse.Invoke(lastChatGPTMessage);
                }

                request.Dispose();
            }

            if (callback != null)
            {
                callback();
            }
        }
    }
}
