namespace ChatGPTWrapper {
    public class Prompt
    {
        private string currentPrompt = "You are ChatGPT, a large language model trained by OpenAI.";
        public string CurrentPrompt => currentPrompt;

        public enum Speaker {
            User,
            ChatGPT
        }

        public void AppendText(Speaker speaker, string text)
        {
            switch (speaker)
            {
                case Speaker.User:
                    currentPrompt += " \n User: " + text + " \n ChatGPT: ";
                    break;
                case Speaker.ChatGPT:
                    currentPrompt += text;
                    break;
            }
        }
    }
}
