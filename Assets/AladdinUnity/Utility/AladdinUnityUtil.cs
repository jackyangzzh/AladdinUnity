using UnityEngine;

public class AladdinUnityUtil : MonoBehaviour
{
    [System.Serializable]
    public enum ScriptType
    {
        CSharp,
        Shader
    }

    [System.Serializable]
    public enum ImageSizes
    {
        _256x256 = 256,
        _512x512 = 512,
        _1024x1024 = 1024,
    }

    public const string CSharpExtension = "cs";
    public const string ShaderExtension = "shader";

    public const string CSharpPrompt = "You are ChatGPT, an expert in Unity C# coding.";
    public const string ShaderPrompt = "You are ChatGPT, an expert in Unity Shader.";

    public const string DefaultPrompt = "You are ChatGPT, a large language model trained by OpenAI.";

}
