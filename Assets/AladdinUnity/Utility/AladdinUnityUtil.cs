using AladdinScriptGen;
using AladdinTextureGen;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public static class AladdinUnityUtil
{
    [System.Serializable]
    public enum ScriptType
    {
        Csharp,
        Shader,
        TextFile,
    }

    [System.Serializable]
    public enum ImageSizes
    {
        _256x256 = 256,
        _512x512 = 512,
        _1024x1024 = 1024,
    }

    private const string CSharpExtension = "cs";
    private const string ShaderExtension = "shader";
    private const string TextFileExtension = "txt"; 

    public const string CSharpPrompt = "You are ChatGPT, an expert in Unity C# coding.";
    public const string ShaderPrompt = "You are ChatGPT, an expert in Unity Shader.";
    public const string DefaultPrompt = "You are ChatGPT, a large language model trained by OpenAI.";

    public static void CreateScriptFile(string inputText, AladdinUnityUtil.ScriptType scriptType)
    {
        string fileExtension = "";
        switch (scriptType)
        {
            case ScriptType.Csharp:
                fileExtension = CSharpExtension;
                break;
            case ScriptType.Shader:
                fileExtension = ShaderExtension;
                break;
            case ScriptType.TextFile:
                fileExtension = TextFileExtension;
                break;
        }

        var path = EditorUtility.SaveFilePanel("Save Script", Application.dataPath, $"Untitled.{fileExtension}", fileExtension);

        if (path.Length > 0)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllText(path, inputText);
            Debug.Log($"[{nameof(AladdinScriptGenerator)}] script created in: {path}");
        }
    }

    public static void CreateTextureFile(Texture2D tex)
    {
        var path = EditorUtility.SaveFilePanel("Save Texture", Application.dataPath, $"Untitled.jpg", ".jpg");

        if (path.Length > 0)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllBytes(path, tex.EncodeToJPG());
            Debug.Log($"[{nameof(AladdinTextureGenerator)}] texture created in: {path}");
        }
    }
}
