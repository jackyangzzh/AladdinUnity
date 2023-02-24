using UnityEditor;

public class ScriptGenius : EditorWindow
{
    //private string scriptName;

    [MenuItem("Window/ScriptGenius")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ScriptGenius));
    }

    //[MenuItem("Script Genius/Script Genius")]
    //public static void ShowWindow()
    //{
    //    GetWindow<ScriptGenius>();
    //    //var window = GetWindow<ScriptGenius>();
    //    //window.titleContent = new GUIContent("Script Genius");
    //    //window.Show();
    //}

    //private string scriptPrompt;
    //private ChatGPTSetting chatGPTSetting;
    //private Button generateScriptButton;


    //private ScriptGeniusElement baseElement;

    private void OnEnable()
    {
        //baseElement = new ScriptGeniusElement(this);
        //rootVisualElement.Add(baseElement);
        //chatGPTSetting =  Resources.Load("DefaultChatGPTSetting") as ChatGPTSetting;
    }

    void OnGUI()
    {
        //scriptName = EditorGUILayout.TextField("Script Name", scriptName);
        //chatGPTSetting = EditorGUILayout.ObjectField("Setting", chatGPTSetting, typeof(ChatGPTSetting)) as ChatGPTSetting;
        //scriptPrompt = EditorGUILayout.TextArea("Script Prompt", scriptPrompt);

        //if (GUILayout.Button("Generate Script"))
        //{
        //    GenerateScript();
        //}
    }

    //void GenerateScript()
    //{
    //    _ = EditorCoroutineUtility.StartCoroutine(GenerateScriptAsync(), this);
    //}

    //IEnumerator GenerateScriptAsync()
    //{
    //    ChatGPTHelper chatGPT = new(scriptName, ScriptGeniusUtil.CSharpExtension);
    //    chatGPT.SendToChatGPT(scriptPrompt, chatGPTSetting);

    //    generateScriptButton.text = "Generating script...";
    //    generateScriptButton.SetEnabled(false);

    //    while (!chatGPT.IsCompleted)
    //    {
    //        yield return null;
    //    }

    //    generateScriptButton.text = "Generate Script";
    //    generateScriptButton.SetEnabled(true);
    //}
}

//public class ScriptGeniusElement : VisualElement
//{
//    public ScriptGeniusElement(EditorWindow window)
//    {
//        TextField scriptNameField = new()
//        {
//            label = "Script Name",
//        };
//        Add(scriptNameField);

//        DropdownField scriptTypeField = new()
//        {
//            label = "Script Type",
//            choices = { ScriptGeniusUtil.CSharpText, ScriptGeniusUtil.ShaderText },
//            index = 0,
//        };
//        Add(scriptTypeField);

//        ObjectField settingField = new()
//        {
//            label = "Setting",
//            value = Resources.Load("DefaultChatGPTSetting"),
//            objectType = typeof(ChatGPTSetting),

//        };
//        Add(settingField);

//        TextField scriptPromptField = new()
//        {
//            label = "Script Prompt"
//        };
//        Add(scriptPromptField);

//        Action action = () => GenerateScript();

//        Button generateScriptButton = new(action)
//        {
//            text = "Generate Script"
            
//        };
//        Add(generateScriptButton);

//        void GenerateScript()
//        {
//            //_ = EditorCoroutineUtility.StartCoroutine(GenerateScriptAsync(), this);
//        }

//        IEnumerator GenerateScriptAsync()
//        {
//            ChatGPTHelper chatGPT = new(scriptNameField.value, scriptTypeField.value);
//            chatGPT.SendToChatGPT(scriptPromptField.text, settingField.value as ChatGPTSetting);

//            generateScriptButton.text = "Generating script...";
//            generateScriptButton.SetEnabled(false);

//            while (!chatGPT.IsCompleted)
//            {
//                yield return null;
//            }

//            generateScriptButton.text = "Generate Script";
//            generateScriptButton.SetEnabled(true);
//        }
//    }
//}
