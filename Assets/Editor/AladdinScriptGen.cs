using UnityEngine;
using UnityEditor;
using System.Collections;
using Unity.EditorCoroutines.Editor;
using ChatGPTWrapper;

class AladdinScriptGen : EditorWindow
{
    [MenuItem("Aladdin Unity/Script Generation")]
    public static void ShowWindow()
    {
        var window = GetWindow(typeof(AladdinScriptGen));
        window.titleContent = new GUIContent("Aladdin - Script Generation");
        window.Show();
    }

    private string scriptName = "Untitled";
    private string scriptPrompt;
    private ChatGPTSetting chatGPTSetting;
    private AladdinUnityUtil.ScriptType scriptType;

    private bool scriptGenerating = false;

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
            if (GUILayout.Button("Generate Script"))
            {
                if(string.IsNullOrWhiteSpace(scriptPrompt))
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
        ChatGPTHelper chatGPT = new(scriptName, scriptType);
        _ = EditorCoroutineUtility.StartCoroutine(chatGPT.GenerateScript(scriptPrompt, chatGPTSetting), this);
    }

}