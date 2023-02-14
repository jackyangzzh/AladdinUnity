using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System.IO;

namespace ChatGPTWrapper
{
    public class ChatGPTConversation : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField]
        private string _apiKey;

        private enum Model
        {
            Davinci,
            Curie,
            None
        }

        [SerializeField]
        private Model _model = Model.Davinci;

        [SerializeField]
        private int _maxTokens = 3072;

        [SerializeField]
        private float _temperature = 0.6f;

        private const string _uri = "https://api.openai.com/v1/completions";

        private List<(string, string)> _reqHeaders;

        private Requests requests = new();
        private Prompt _prompt = new();
        private string _lastUserMsg;
        private string _lastChatGPTMsg;
        private string _selectedModel;

        [Space(15)]
        public UnityEvent<string> chatGPTResponse = new();

        private void Start()
        {
            SendToChatGPT("Write a Unity water shader");
        }

        private void OnEnable()
        {
            _reqHeaders = new List<(string, string)>
            {
                ("Authorization", $"Bearer {_apiKey}"),
                ("Content-Type", "application/json")
            };

            switch (_model)
            {
                case Model.Davinci:
                    _selectedModel = "text-davinci-003";
                    break;
                case Model.Curie:
                    _selectedModel = "text-curie-001";
                    break;
                case Model.None:
                    _selectedModel = null;
                    break;
            }
        }

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

            StartCoroutine(requests.PostReq<ChatGPTRes>(_uri, json, ResolveResponse, _reqHeaders));
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
