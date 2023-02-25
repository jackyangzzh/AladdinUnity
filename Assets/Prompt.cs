namespace ChatGPTWrapper {
    public class Prompt
    {
        public string CurrentPrompt = AladdinUnityUtil.DefaultPrompt;

        public enum Speaker {
            User,
            ChatGPT
        }

        public void AppendText(Speaker speaker, string text)
        {
            switch (speaker)
            {
                case Speaker.User:
                    CurrentPrompt += " \n User: " + text + " \n ChatGPT: ";
                    break;
                case Speaker.ChatGPT:
                    CurrentPrompt += text;
                    break;
            }
        }
    }
}
