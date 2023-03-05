using AladdinTextureGen;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static TreeEditor.TextureAtlas;

public class AladdinTextureGenEditor : EditorWindow
{
    [MenuItem("Test/Texture Generation Editor")]
    public static void ShowWindow()
    {
        var window = GetWindow<AladdinTextureGenEditor>();
        window.titleContent = new GUIContent("Aladdin - Texture Gen");
        window.Show();
    }

    public VisualTreeAsset uxml;

    private OpenAiSetting openAiSetting;

    private const string TextureNameTextField = "TextureNameTextField";
    private const string GenerateTextureButton = "GenerateTextureButton";
    private const string TextureSizeDropdown = "TextureSizeDropdown";
    private const string OpenAiSettingField = "OpenAiSettingField";
    private const string TexturePromptTextField = "TexturePromptTextField";

    private TextField textureNameTextField;
    private Button generateTextureButton;
    private EnumField textureSizeDropdown;
    private ObjectField openAiSettingField;
    private TextField texturePromptTextField;

    private void OnEnable()
    {
        uxml.CloneTree(rootVisualElement);

        // Bind UI
        textureNameTextField = rootVisualElement.Q<TextField>(name = TextureNameTextField);
        generateTextureButton = rootVisualElement.Q<Button>(name = GenerateTextureButton);
        textureSizeDropdown = rootVisualElement.Q<EnumField>(name = TextureSizeDropdown);
        openAiSettingField = rootVisualElement.Q<ObjectField>(name = OpenAiSettingField);
        texturePromptTextField = rootVisualElement.Q<TextField>(name = TexturePromptTextField);

        // Setup UI
        generateTextureButton.clicked += GenerateTexture;

        textureSizeDropdown.Init(AladdinUnityUtil.ImageSizes._256x256);

        openAiSettingField.objectType = typeof(OpenAiSetting);
        openAiSetting = Resources.Load("DefaultOpenAiSetting") as OpenAiSetting;
        openAiSettingField.value = openAiSetting;
    }

    private void GenerateTexture()
    {
        Debug.LogError("Generate texture");
        DallEHelper dalle = new(texturePromptTextField.text, (int)(AladdinUnityUtil.ImageSizes)textureSizeDropdown.value);
        _ = EditorCoroutineUtility.StartCoroutine(dalle.GenerateTexture(openAiSetting, texturePromptTextField.text), this);
    }
}