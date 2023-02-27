using AladdinScriptGen;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/OpenAiSetting", order = 1)]
public class OpenAiSetting : ScriptableObject
{
    public string APIKey = "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";
    public Model Model = Model.Davinci;
    public int MaxToken = 3072;
    public float Temperature = 0.6f;
}