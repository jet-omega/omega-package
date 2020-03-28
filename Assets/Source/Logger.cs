using Omega.Text;
using UnityEngine;

namespace Omega.Package
{
    public class Logger
    {
        private RichTextFactory _richTextFactory;

        public string Title;
        public Color TitleColor;
        public FontStyle TitleStyle;

        public Logger(string title, Color titleColor, FontStyle titleStyle = FontStyle.Normal)
        {
            _richTextFactory = new RichTextFactory(2048);
            Title = title;
            TitleColor = titleColor;
            TitleStyle = titleStyle;
        }

        public void Log(string message)
        {
            var fullMessage = _richTextFactory
                .Style(TitleStyle).Color(TitleColor).Text(Title)
                .UnstyledText(message).ToString();
            
            Debug.Log(fullMessage);
        }
    }
    
    
}