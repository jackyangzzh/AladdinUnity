using AladdinScriptGen;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class AladdinScriptGenEditor : EditorWindow
{
    [MenuItem("Test/Script Generation Editor")]
    public static void ShowWindow()
    {
        var window = GetWindow<AladdinScriptGenEditor>();
        window.titleContent = new GUIContent("Aladdin - Script Gen");
        window.Show();
    }

    public VisualTreeAsset uxml;

    private OpenAiSetting openAiSetting;

    private const string ScriptNameTextField = "ScriptNameTextField";
    private const string GenerateScriptButton = "GenerateScriptButton";
    private const string ScriptTypeDropdown = "ScriptTypeDropdown";
    private const string OpenAiSettingField = "OpenAiSettingField";
    private const string ScriptPromptTextField = "ScriptPromptTextField";

    private TextField scriptNameTextField;
    private Button generateScriptButton;
    private EnumField scriptTypeDropdown;
    private ObjectField openAiSettingField;
    private TextField scriptPromptTextField;

    private void CreateGUI()
    {
        uxml.CloneTree(rootVisualElement);

        // Bind UI
        scriptNameTextField = rootVisualElement.Q<TextField>(name = ScriptNameTextField);
        generateScriptButton = rootVisualElement.Q<Button>(name = GenerateScriptButton);
        scriptTypeDropdown = rootVisualElement.Q<EnumField>(name = ScriptTypeDropdown);
        openAiSettingField = rootVisualElement.Q<ObjectField>(name = OpenAiSettingField);
        scriptPromptTextField = rootVisualElement.Q<TextField>(name = ScriptPromptTextField);

        // Setup UI
        generateScriptButton.clicked += GenerateScript;

        scriptTypeDropdown.Init(AladdinUnityUtil.ScriptType.CSharp);

        openAiSettingField.objectType = typeof(OpenAiSetting);
        openAiSetting = Resources.Load("DefaultOpenAiSetting") as OpenAiSetting;
        openAiSettingField.value = openAiSetting;
    }

    private void OnInspectorUpdate()
    {
        bool canGenerate = !string.IsNullOrWhiteSpace(scriptNameTextField.text) &&
                            !string.IsNullOrWhiteSpace(scriptPromptTextField.text) &&
                            openAiSettingField.value != null;

        generateScriptButton.SetEnabled(canGenerate);
    }

    private void GenerateScript()
    {
        Debug.LogError("Generate script");

        
        ChatGPTHelper chatGPT = new(scriptNameTextField.text, (AladdinUnityUtil.ScriptType)scriptTypeDropdown.value);
        _ = EditorCoroutineUtility.StartCoroutine(chatGPT.GenerateScript(scriptPromptTextField.value, openAiSetting), this);
    }
}