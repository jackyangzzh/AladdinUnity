using UnityEngine;
using UnityEditor;
using System.Collections;
using Unity.EditorCoroutines.Editor;
using ChatGPTWrapper;

class AladdinUnity : EditorWindow
{
    [MenuItem("Aladdin Unity/Script Generation")]
    public static void ShowWindow()
    {
        var window = GetWindow(typeof(AladdinUnity));
        window.titleContent = new GUIContent("Aladdin - Script Generation");
        window.Show();
    }

    private string scriptName;
    private string scriptPrompt;
    private ChatGPTSetting chatGPTSetting;
    private AladdinUnityUtil.ScriptType scriptType;

    private bool scriptGenerating = false;
    private string generateScriptButtonText = "Generate Script";

    private void OnEnable()
    {
        scriptGenerating = false;
        chatGPTSetting = Resources.Load("DefaultChatGPTSetting") as ChatGPTSetting;
    }

    void OnGUI()
    {
        scriptName = EditorGUILayout.TextField("Script Name", scriptName);
        scriptType = (AladdinUnityUtil.ScriptType)EditorGUILayout.EnumPopup("Script Type", scriptType);

        chatGPTSetting = EditorGUILayout.ObjectField("Setting", chatGPTSetting, typeof(ChatGPTSetting), false) as ChatGPTSetting;

        EditorGUILayout.LabelField("Script Prompt");
        GUIStyle style = new GUIStyle(EditorStyles.textArea);
        style.wordWrap = true;
        scriptPrompt = EditorGUILayout.TextArea(scriptPrompt, style);

        if (scriptGenerating)
        {
            EditorGUILayout.LabelField("Script generating...");
        }
        else
        {
            if (GUILayout.Button(generateScriptButtonText))
            {
                GenerateScript();
            }
        }
    }

    private void GenerateScript()
    {
        _ = EditorCoroutineUtility.StartCoroutine(GenerateScriptAsync(), this);
    }

    private IEnumerator GenerateScriptAsync()
    {
        scriptGenerating = true;

        ChatGPTHelper chatGPT = new(scriptName, scriptType);
        chatGPT.SendToChatGPT(scriptPrompt, chatGPTSetting);

        while (!chatGPT.IsCompleted)
        {
            yield return null;
        }

        scriptGenerating = false;
    }

}