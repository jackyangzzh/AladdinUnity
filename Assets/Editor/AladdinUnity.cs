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
    
    private void OnEnable()
    {
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

        if (GUILayout.Button("Generate Script"))
        {
            GenerateScript();
        }
    }

    private void GenerateScript()
    {
        Debug.Log("Generating Script");
        _ = EditorCoroutineUtility.StartCoroutine(GenerateScriptAsync(), this);
    }

    private IEnumerator GenerateScriptAsync()
    {
        ChatGPTHelper chatGPT = new(scriptName, scriptType);
        chatGPT.SendToChatGPT(scriptPrompt, chatGPTSetting);

        //generateScriptButton.text = "Generating script...";
        //generateScriptButton.SetEnabled(false);

        while (!chatGPT.IsCompleted)
        {
            yield return null;
        }

        //generateScriptButton.text = "Generate Script";
        //generateScriptButton.SetEnabled(true);
    }

}