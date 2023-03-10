using AladdinScriptGen;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class AladdinScriptGenEditor : EditorWindow
{
    [MenuItem("Aladdin Unity/Script Generation")]
    public static void ShowWindow()
    {
        var window = GetWindow<AladdinScriptGenEditor>();
        window.titleContent = new GUIContent("Aladdin - Script Gen");
        window.Show();
    }

    public VisualTreeAsset uxml;

    private OpenAiSetting openAiSetting;

    private const string GenerateScriptButton = "GenerateScriptButton";
    private const string ScriptTypeDropdown = "ScriptTypeDropdown";
    private const string OpenAiSettingField = "OpenAiSettingField";
    private const string ScriptPromptTextField = "ScriptPromptTextField";

    private Button generateScriptButton;
    private EnumField scriptTypeDropdown;
    private ObjectField openAiSettingField;
    private TextField scriptPromptTextField;

    private void CreateGUI()
    {
        uxml.CloneTree(rootVisualElement);

        // Bind UI
        scriptTypeDropdown = rootVisualElement.Q<EnumField>(name = ScriptTypeDropdown);
        openAiSettingField = rootVisualElement.Q<ObjectField>(name = OpenAiSettingField);
        scriptPromptTextField = rootVisualElement.Q<TextField>(name = ScriptPromptTextField);
        generateScriptButton = rootVisualElement.Q<Button>(name = GenerateScriptButton);

        // Setup UI
        generateScriptButton.clicked += GenerateScript;

        scriptTypeDropdown.Init(AladdinUnityUtil.ScriptType.CSharp);

        openAiSettingField.objectType = typeof(OpenAiSetting);
        openAiSetting = Resources.Load("DefaultOpenAiSetting") as OpenAiSetting;
        openAiSettingField.value = openAiSetting;
    }

    private void OnInspectorUpdate()
    {
        bool canGenerate = !string.IsNullOrWhiteSpace(scriptPromptTextField.text) &&
                            openAiSettingField.value != null;

        generateScriptButton.SetEnabled(canGenerate);
    }

    private void GenerateScript()
    {
        AladdinScriptGenerator aladdin = new(openAiSetting);
        _ = EditorCoroutineUtility.StartCoroutine(aladdin.GenerateScript(scriptPromptTextField.value, (AladdinUnityUtil.ScriptType)scriptTypeDropdown.value, true), this);
    }
}