using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System.IO;
using Unity.EditorCoroutines.Editor;
using System.Globalization;

namespace ChatGPTWrapper
{
    public enum Model
    {
        Davinci,
        Curie,
        None
    }

    public class ChatGPTHelper
    {
        private const string _uri = "https://api.openai.com/v1/completions";

        private List<(string, string)> reqHeaders;

        private Requests requests = new();
        private Prompt prompt = new();
        private string lastUserMsg;
        private string lastChatGPTMsg;
        private string selectedModel = "text-davinci-003";

        private string fileName = "Untitled";
        private string scriptType;

        [Space(15)]
        public UnityEvent<string> chatGPTResponse = new();

        public ChatGPTHelper (string fileName, string scriptType)
        {
            this.fileName = fileName;
            this.scriptType = scriptType;
        }

        public void SendToChatGPT(string message, ChatGPTSetting setting)
        {
            if (selectedModel == null)
            {
                Debug.LogWarning($"{nameof(ChatGPTConversation)} [SendToChatGPT] Model name for ChatGPT's API is not set up yet.");
                return;
            }

            lastUserMsg = message;

            string scriptPrompt = "";
            switch(scriptType)
            {
                case ScriptGeniusUtil.CSharpText:
                    prompt.CurrentPrompt = ScriptGeniusUtil.CSharpPrompt;
                    break;
                case ScriptGeniusUtil.ShaderText:
                    prompt.CurrentPrompt = ScriptGeniusUtil.ShaderPrompt;
                    break;
                default:
                    prompt.CurrentPrompt = ScriptGeniusUtil.DefaultPrompt;
                    break;
            }

            prompt.AppendText(Prompt.Speaker.User, message);

            Debug.Log(scriptPrompt);

            ChatGPTReq reqObj = new()
            {
                model = selectedModel,
                prompt = prompt.CurrentPrompt,
                max_tokens = setting.MaxToken,
                temperature = setting.Temperature,
            };

            string json = JsonUtility.ToJson(reqObj);

            reqHeaders = new List<(string, string)>
            {
                ("Authorization", $"Bearer {setting.APIKey}"),
                ("Content-Type", "application/json")
            };

            _ = EditorCoroutineUtility.StartCoroutine(requests.PostReq<ChatGPTRes>(_uri, json, ResolveResponse, reqHeaders), this);
        }

        private void ResolveResponse(ChatGPTRes res)
        {
            //Debug.Log($"{nameof(ChatGPTConversation)}: {res.choices[0].text}");
            lastChatGPTMsg = res.choices[0].text
                .TrimStart('\n')
                .Replace("<|im_end|>", "");

            GenerateScript(res.choices[0].text);

            prompt.AppendText(Prompt.Speaker.ChatGPT, lastChatGPTMsg);
            chatGPTResponse.Invoke(lastChatGPTMsg);
        }

        private void GenerateScript(string inputText)
        {

            string path = Path.Combine(Application.dataPath, fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllText(path, inputText);
            Debug.Log($"[{nameof(ChatGPTHelper)}] script created in: {path}");
        }
    }
}
