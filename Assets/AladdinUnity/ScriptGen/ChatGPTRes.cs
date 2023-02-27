using System;
using System.Collections.Generic;

namespace AladdinScriptGen
{
    [Serializable]
    public class Choices
    {
        public string text;
    }

    [Serializable]
    public class ChatGPTRes
    {
        public string id;
        public List<Choices> choices;
    }
}
