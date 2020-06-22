using UnityEngine;

namespace Omega.Text
{
    public sealed class RichTextFactory
    {
        private RichTextBuilder _builder;

        public static RichTextFactory New(int capacity = 30)
        {
            return new RichTextFactory(capacity);
        }

        public void Clear()
        {
            _builder.Clear();
        }
        
        public RichTextFactory()
        {
            _builder = new RichTextBuilder();
        }
        
        public RichTextFactory(int capacity)
        {
            _builder = new RichTextBuilder(capacity);
        }

        public RichTextFactory Default => DefaultColor.DefaultSize.DefaultStyle;

        public RichTextFactory NewLine
        {
            get
            {
                _builder.AppendWithoutRich("\n");
                return this;
            }
        }


        public RichTextFactory Color(Color color)
        {
            _builder.Color = color;
            return this;
        }

        public RichTextFactory DefaultColor
        {
            get
            {
                _builder.Color = null;
                return this;
            }
        }

        public RichTextFactory Size(int size)
        {
            _builder.Size = size;
            return this;
        }

        public RichTextFactory DefaultSize
        {
            get
            {
                _builder.Size = null;
                return this;
            }
        }

        public RichTextFactory DefaultStyle
        {
            get
            {
                _builder.FontStyle = FontStyle.Normal;
                return this;
            }
        }


        public RichTextFactory Bold
        {
            get
            {
                _builder.FontStyle = FontStyle.Bold;
                return this;
            }
        }

        public RichTextFactory Italic
        {
            get
            {
                _builder.FontStyle = FontStyle.Italic;
                return this;
            }
        }

        public RichTextFactory Style(FontStyle fontStyle)
        {
            _builder.FontStyle = fontStyle;
            return this;
        }

        public RichTextFactory Text(string s)
        {
            _builder.Append(s);
            return this;
        }

        public RichTextFactory Space(int spaceCount = 4)
        {
            for (int i = 0; i < spaceCount; i++)
                _builder.AppendWithoutRich(" ");

            return this;
        }
        
        public RichTextFactory UnstyledText(string s)
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