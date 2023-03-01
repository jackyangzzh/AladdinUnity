using UnityEngine;
using UnityEditor;
using Unity.EditorCoroutines.Editor;

namespace AladdinTextureGen
{
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
        private AladdinUnityUtil.ImageSizes textureSize = AladdinUnityUtil.ImageSizes._256x256;
        private OpenAiSetting openAiSetting;

        private bool scriptGenerating = false;

        private void OnEnable()
        {
            scriptGenerating = false;
            openAiSetting = Resources.Load("DefaultOpenAiSetting") as OpenAiSetting;
        }

        void OnGUI()
        {
            textureName = EditorGUILayout.TextField("Texture Name", textureName);

            textureSize = (AladdinUnityUtil.ImageSizes)EditorGUILayout.EnumPopup("Texture Size", textureSize);

            openAiSetting = EditorGUILayout.ObjectField("Setting", openAiSetting, typeof(OpenAiSetting), false) as OpenAiSetting;

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
            _ = EditorCoroutineUtility.StartCoroutine(dalle.GenerateTexture(openAiSetting, textureName), this);
        }
    }
}