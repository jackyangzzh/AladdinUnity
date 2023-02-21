using ChatGPTWrapper;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


public class ScriptGenius : EditorWindow
{
    [MenuItem("Script Genius/Script Genius")]
    public static void ShowWindow()
    {
        var window = GetWindow<ScriptGenius>();
        window.titleContent = new GUIContent("Script Genius");
        window.Show();
    }

    private ScriptGeniusElement baseElement;

    private void OnEnable()
    {
        baseElement = new ScriptGeniusElement(this);
        rootVisualElement.Add(baseElement);
    }
}

public class ScriptGeniusElement : VisualElement
{
    public ScriptGeniusElement(EditorWindow window)
    {
        TextField scriptNameField = new()
        {
            label = "Script Name",
        };
        Add(scriptNameField);

        DropdownField scriptTypeField = new()
        {
            label = "Script Type",
            choices = { ScriptGeniusUtil.CSharpText, ScriptGeniusUtil.ShaderText },
            index = 0,
        };
        Add(scriptTypeField);

        ObjectField settingField = new()
        {
            label = "Setting",
            value = Resources.Load("DefaultChatGPTSetting"),
            objectType = typeof(ChatGPTSetting),

        };
        Add(settingField);

        TextField scriptPromptField = new()
        {
            label = "Script Prompt"
        };
        Add(scriptPromptField);

        Button generateScriptButton = new(GenerateScript)
        {
            text = "Generate Script"
        };
        Add(generateScriptButton);


        void GenerateScript()
        {
            ChatGPTHelper chatGPT = new();
            chatGPT.SendToChatGPT(scriptPromptField.text, settingField.value as ChatGPTSetting, scriptTypeField.value);
        }
    }

}
