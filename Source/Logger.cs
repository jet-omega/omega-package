using Omega.Text;
using UnityEngine;

namespace Omega.Package
{
    internal class Logger
    {
        private RichTextFactory _richTextFactory;

        public readonly string Title;
        public readonly Color TitleColor;
        public readonly FontStyle TitleStyle;
        private readonly string TitleBacked;

        public Logger(string title, Color titleColor, FontStyle titleStyle = FontStyle.Normal)
        {
            Title = title;
            TitleColor = titleColor;
            TitleStyle = titleStyle;

            _richTextFactory = new RichTextFactory(2048);
            TitleBacked = _richTextFactory
                .Style(TitleStyle).Color(TitleColor).Text(Title)
                .ToString(true);
        }

        private Logger(string prefix, string title, Color titleColor, FontStyle titleStyle = FontStyle.Normal)
        {
            Title = title;
            TitleColor = titleColor;
            TitleStyle = titleStyle;

            _richTextFactory = new RichTextFactory(2048);
            TitleBacked = _richTextFactory
                .UnstyledText(prefix)
                .Style(TitleStyle).Color(TitleColor).Text(Title)
                .ToString(true);
        }

        public void Log(string message) => Log(message, LogType.Log, LogOption.NoStacktrace);
        public void Log(string message, LogType logType) => Log(message, logType, LogOption.NoStacktrace);
        public void Log(string message, LogOption logOption) => Log(message, LogType.Log, logOption);

        public void Log(string message, LogType logType, LogOption logOption, Object context = null)
        {
            var fullMessage = _richTextFactory
                .UnstyledText(TitleBacked)
                .UnstyledText(message).ToString();

            _richTextFactory.Clear();

            Debug.LogFormat(logType, logOption, context, fullMessage);
        }

        public Logger CreateSubLogger(string subtitle, Color subtitleColor, FontStyle subtitleStyle = FontStyle.Normal)
        {
            return new Logger(TitleBacked, subtitle, subtitleColor, subtitleStyle);
        }
    }
}