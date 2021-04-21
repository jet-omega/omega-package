using UnityEngine;

namespace Omega.Text
{
    public sealed class RichTextFluent
    {
        private RichTextBuilder _builder;

        public static RichTextFluent New(int capacity = 30)
        {
            return new RichTextFluent(capacity);
        }

        public void Clear()
        {
            _builder.Clear();
        }
        
        public RichTextFluent()
        {
            _builder = new RichTextBuilder();
        }
        
        public RichTextFluent(int capacity)
        {
            _builder = new RichTextBuilder(capacity);
        }

        public RichTextFluent Default => DefaultColor.DefaultSize.DefaultStyle;

        public RichTextFluent NewLine
        {
            get
            {
                _builder.AppendWithoutRich("\n");
                return this;
            }
        }


        public RichTextFluent Color(Color color)
        {
            _builder.Color = color;
            return this;
        }

        public RichTextFluent DefaultColor
        {
            get
            {
                _builder.Color = null;
                return this;
            }
        }

        public RichTextFluent Size(int size)
        {
            _builder.Size = size;
            return this;
        }

        public RichTextFluent DefaultSize
        {
            get
            {
                _builder.Size = null;
                return this;
            }
        }

        public RichTextFluent DefaultStyle
        {
            get
            {
                _builder.FontStyle = FontStyle.Normal;
                return this;
            }
        }


        public RichTextFluent Bold
        {
            get
            {
                _builder.FontStyle = FontStyle.Bold;
                return this;
            }
        }

        public RichTextFluent Italic
        {
            get
            {
                _builder.FontStyle = FontStyle.Italic;
                return this;
            }
        }

        public RichTextFluent Style(FontStyle fontStyle)
        {
            _builder.FontStyle = fontStyle;
            return this;
        }

        public RichTextFluent Text(string s)
        {
            _builder.Append(s);
            return this;
        }

        public RichTextFluent Space(int spaceCount = 4)
        {
            for (int i = 0; i < spaceCount; i++)
                _builder.AppendWithoutRich(" ");

            return this;
        }
        
        public RichTextFluent UnstyledText(string s)
        {
            _builder.AppendWithoutRich(s);
            return this;
        }

        public override string ToString()
        {
            return _builder.ToString();
        }

        public string ToString(bool clear)
        {
            var s = _builder.ToString();
            if (clear)
                Clear();
            return s;
        }
    }
}