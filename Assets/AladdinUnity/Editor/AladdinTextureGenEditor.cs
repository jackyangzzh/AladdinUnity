using Codice.Client.Common.GameUI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AladdinTextureGenEditor : EditorWindow
{
    [MenuItem("Aladdin Unity/Texture Generation Editor")]
    public static void ShowWindow()
    {
        var window = GetWindow<AladdinTextureGenEditor>();
        window.titleContent = new GUIContent("Aladdin - Texture Gen");
        window.Show();
    }

    public VisualTreeAsset uxml;

    private TextureGenElement baseElement;

    private void OnEnable()
    {
        //baseElement = new TextureGenElement(this);
        //rootVisualElement.Add(baseElement);

        uxml.CloneTree(rootVisualElement);

        

    }
}

public class TextureGenElement : VisualElement
{
    private const string CSharpText = "C#";
    private const string ShaderText = "Shader";

    public TextureGenElement(EditorWindow window)
    {
        var root = new VisualElement();

    }

}