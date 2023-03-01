
using UnityEngine;
using UnityEditor;
using Unity.EditorCoroutines.Editor;

namespace AladdinScriptGen
{
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
        private OpenAiSetting openAiSetting;
        private AladdinUnityUtil.ScriptType scriptType;


        private void OnEnable()
        {
            openAiSetting = Resources.Load("DefaultOpenAiSetting") as OpenAiSetting;
        }

        void OnGUI()
        {
            scriptName = EditorGUILayout.TextField("Script Name", scriptName);
            scriptType = (AladdinUnityUtil.ScriptType)EditorGUILayout.EnumPopup("Script Type", scriptType);

            openAiSetting = EditorGUILayout.ObjectField("Setting", openAiSetting, typeof(OpenAiSetting), false) as OpenAiSetting;

            EditorGUILayout.LabelField("Script Prompt");
            GUIStyle style = new GUIStyle(EditorStyles.textArea);
            style.wordWrap = true;
            scriptPrompt = EditorGUILayout.TextArea(scriptPrompt, style);

            if (GUILayout.Button("Generate Script"))
            {
                if (string.IsNullOrWhiteSpace(scriptPrompt))
                {
                    EditorGUILayout.LabelField("Prompt cannot be empty");
                    return;
                }
                GenerateScript();
            }
        }

        private void GenerateScript()
        {
            ChatGPTHelper chatGPT = new(scriptName, scriptType);
            _ = EditorCoroutineUtility.StartCoroutine(chatGPT.GenerateScript(scriptPrompt, openAiSetting), this);
        }
    }
}