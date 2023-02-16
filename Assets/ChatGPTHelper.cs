using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System.IO;
using UnityEditor;
using Unity.EditorCoroutines.Editor;

namespace ChatGPTWrapper
{
    public class ChatGPTHelper 
    {
        private const string _apiKey = "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";

        private enum Model
        {
            Davinci,
            Curie,
            None
        }

        private const Model _model = Model.Davinci;

        private const int _maxTokens = 3072;

        private const float _temperature = 0.6f;

        private const string _uri = "https://api.openai.com/v1/completions";

        private List<(string, string)> _reqHeaders;

        private Requests requests = new();
        private Prompt _prompt = new();
        private string _lastUserMsg;
        private string _lastChatGPTMsg;
        private string _selectedModel = "text-davinci-003";

        [Space(15)]
        public UnityEvent<string> chatGPTResponse = new();

        public void SendToChatGPT(string message)
        {
            if (_selectedModel == null)
            {
                Debug.LogWarning($"{nameof(ChatGPTConversation)} [SendToChatGPT] Model name for ChatGPT's API is not set up yet.");
                return;
            }

            _lastUserMsg = message;
            _prompt.AppendText(Prompt.Speaker.User, message);

            ChatGPTReq reqObj = new()
            {
                model = _selectedModel,
                prompt = _prompt.CurrentPrompt,
                max_tokens = _maxTokens,
                temperature = _temperature,
            };

            string json = JsonUtility.ToJson(reqObj);

            _reqHeaders = new List<(string, string)>
            {
                ("Authorization", $"Bearer {_apiKey}"),
                ("Content-Type", "application/json")
            };

            EditorCoroutineUtility.StartCoroutine(requests.PostReq<ChatGPTRes>(_uri, json, ResolveResponse, _reqHeaders), this);
        }

        private void ResolveResponse(ChatGPTRes res)
        {
            //Debug.Log($"{nameof(ChatGPTConversation)}: {res.choices[0].text}");
            _lastChatGPTMsg = res.choices[0].text
                .TrimStart('\n')
                .Replace("<|im_end|>", "");

            GenerateShaderCode(res.choices[0].text);

            _prompt.AppendText(Prompt.Speaker.ChatGPT, _lastChatGPTMsg);
            chatGPTResponse.Invoke(_lastChatGPTMsg);
        }

        private void GenerateShaderCode(string inputText)
        {
            string path = Application.dataPath + "/shader.shader";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllText(path, inputText);
        }
    }
}
