using ChatGPTWrapper;
using Codice.Client.Common.GameUI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    private const string CSharpText = "C#";
    private const string ShaderText = "Shader";

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
            choices = { CSharpText, ShaderText },
            index = 0,
        };
        Add(scriptTypeField);

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
            chatGPT.SendToChatGPT(scriptPromptField.text);
            Debug.Log(scriptPromptField.text);
        }
    }

}
