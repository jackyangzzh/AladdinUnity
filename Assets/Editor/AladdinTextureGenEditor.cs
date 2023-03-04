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

    private void OnEnable()
    {
        uxml.CloneTree(rootVisualElement);

        Debug.Log(rootVisualElement.Q<TextField>(name = TextureNameTextField).text);
        rootVisualElement.Q<Button>(name = GenerateTextureButton).clicked += GenerateTexture;

        rootVisualElement.Q<EnumField>(name = TextureSizeDropdown).Init(AladdinUnityUtil.ImageSizes._256x256);
        
        rootVisualElement.Q<ObjectField>(name = OpenAiSettingField).objectType = typeof(OpenAiSetting);
        openAiSetting = Resources.Load("DefaultOpenAiSetting") as OpenAiSetting;
        rootVisualElement.Q<ObjectField>(name = OpenAiSettingField).value = openAiSetting;
    }

    private void GenerateTexture()
    {
        Debug.LogError("Generate texture");
    }
}