using UnityEngine;
using UnityEditor;
using System.Collections;
using Unity.EditorCoroutines.Editor;
using ChatGPTWrapper;

class AladinTextureGen : EditorWindow
{
    [MenuItem("Aladdin Unity/Texture Generation")]
    public static void ShowWindow()
    {
        var window = GetWindow(typeof(AladinTextureGen));
        window.titleContent = new GUIContent("Aladdin - Texture Generation");
        window.Show();
    }

    private string textureName = "Untitled";
    private string texturePrompt;
    private SizeOptions textureSize;
    private ChatGPTSetting chatGPTSetting;

    private bool scriptGenerating = false;

    private enum SizeOptions
    {
        _256x256 = 256,
        _512x512 = 512,
        _1024x1024 = 1024,
    }

    private void OnEnable()
    {
        scriptGenerating = false;
        chatGPTSetting = Resources.Load("DefaultChatGPTSetting") as ChatGPTSetting;
    }

    void OnGUI()
    {
        textureName = EditorGUILayout.TextField("Texture Name", textureName);

        chatGPTSetting = EditorGUILayout.ObjectField("Setting", chatGPTSetting, typeof(ChatGPTSetting), false) as ChatGPTSetting;

        textureSize = (SizeOptions)EditorGUILayout.EnumPopup("Texture Size", textureSize);

        EditorGUILayout.LabelField("Texture Prompt");
        GUIStyle style = new GUIStyle(EditorStyles.textArea);
        style.wordWrap = true;
        texturePrompt = EditorGUILayout.TextArea(texturePrompt, style);

        if (scriptGenerating)
        {
            EditorGUILayout.LabelField("Script generating...");
        }
        else
        {
            if (GUILayout.Button("Generate Texture"))
            {
                if (string.IsNullOrWhiteSpace(texturePrompt))
                {
                    EditorGUILayout.LabelField("Prompt cannot be empty");
                    return;
                }
                GenerateScript();
            }
        }
    }

    private void GenerateScript()
    {
        DallEHelper dalle = new(texturePrompt, (int)textureSize);
        _ = EditorCoroutineUtility.StartCoroutine(dalle.GenerateTexture(chatGPTSetting, textureName), this);
    }

}