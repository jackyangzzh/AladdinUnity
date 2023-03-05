using AladdinTextureGen;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class AladdinTextureGenEditor : EditorWindow
{
    [MenuItem("Aladdin Unity/Texture Generation")]
    public static void ShowWindow()
    {
        var window = GetWindow<AladdinTextureGenEditor>();
        window.titleContent = new GUIContent("Aladdin - Texture Gen");
        window.Show();
    }

    public VisualTreeAsset uxml;

    private OpenAiSetting openAiSetting;

    private const string GenerateTextureButton = "GenerateTextureButton";
    private const string TextureSizeDropdown = "TextureSizeDropdown";
    private const string OpenAiSettingField = "OpenAiSettingField";
    private const string TexturePromptTextField = "TexturePromptTextField";

    private Button generateTextureButton;
    private EnumField textureSizeDropdown;
    private ObjectField openAiSettingField;
    private TextField texturePromptTextField;

    private void OnEnable()
    {
        uxml.CloneTree(rootVisualElement);

        // Bind UI
        textureSizeDropdown = rootVisualElement.Q<EnumField>(name = TextureSizeDropdown);
        openAiSettingField = rootVisualElement.Q<ObjectField>(name = OpenAiSettingField);
        texturePromptTextField = rootVisualElement.Q<TextField>(name = TexturePromptTextField);
        generateTextureButton = rootVisualElement.Q<Button>(name = GenerateTextureButton);

        // Setup UI
        generateTextureButton.clicked += GenerateTexture;
        generateTextureButton.SetEnabled(false);

        textureSizeDropdown.Init(AladdinUnityUtil.ImageSizes._256x256);

        openAiSettingField.objectType = typeof(OpenAiSetting);
        openAiSetting = Resources.Load("DefaultOpenAiSetting") as OpenAiSetting;
        openAiSettingField.value = openAiSetting;
    }

    private void OnInspectorUpdate()
    {
        bool canGenerate = !string.IsNullOrWhiteSpace(texturePromptTextField.text) &&
                            openAiSettingField.value != null;

        generateTextureButton.SetEnabled(canGenerate);
    }

    private void GenerateTexture()
    {
        DallEHelper dalle = new(texturePromptTextField.text, (int)(AladdinUnityUtil.ImageSizes)textureSizeDropdown.value);
        _ = EditorCoroutineUtility.StartCoroutine(dalle.GenerateTexture(texturePromptTextField.text, openAiSetting, true), this);
    }
}