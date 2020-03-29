using System.Text;
using Omega.Package.Internal;
using UnityEngine;

namespace Omega.Text
{
    public sealed class RichTextBuilder
    {
        private const string BeginBoldTag = "<b>";
        private const string EndBoldTag = "</b>";

        private const string BeginItalicTag = "<i>";
        private const string EndItalicTag = "</i>";

        private const string BeginColorTagBeforeHex = "<color=";
        private const string BeginColorTagAfterHex = ">";
        private const string EndColorTag = "</color>";

        private const string BeginSizeTagBeforeValue = "<size=";
        private const string BeginSizeTagAfterValue = ">";
        private const string EndSizeTag = "</size>";

        private readonly StringBuilder _stringBuilder;

        private Color? _color;
        private int _size = -1;
        private FontStyle _fontStyle;

        private string _colorHex;
        private string _sizeString;

        public FontStyle FontStyle
        {
            get => _fontStyle;
            set => _fontStyle = value;
        }

        public int? Size
        {
            get => _size < 0 ? (int?) null : _size;
            set
            {
                _size = value.HasValue
                    ? value.Value
                    : -1;

                if (_size >= 0)
                    _sizeString = _size.ToString();
            }
        }

        public Color? Color
        {
            get => _color;
            set
            {
                _color = value;
                if (_color.HasValue)
                {
                    unsafe
                    {
                        var colorValue = (Color32) _color.Value;
                        //#RRGGBBAA
                        var colorHexCharArray = stackalloc char[9];
                        colorHexCharArray[0] = '#';

                        colorHexCharArray[1] = HexSupport.HighByteToChar(colorValue.r);
                        colorHexCharArray[2] = HexSupport.LowByteToChar(colorValue.r);
                        colorHexCharArray[3] = HexSupport.HighByteToChar(colorValue.g);
                        colorHexCharArray[4] = HexSupport.LowByteToChar(colorValue.g);
                        colorHexCharArray[5] = HexSupport.HighByteToChar(colorValue.b);
                        colorHexCharArray[6] = HexSupport.LowByteToChar(colorValue.b);
                        colorHexCharArray[7] = HexSupport.HighByteToChar(colorValue.a);
                        colorHexCharArray[8] = HexSupport.LowByteToChar(colorValue.a);

                        _colorHex = new string(colorHexCharArray, 0, 9);
                    }
                }
            }
        }

        public RichTextBuilder()
            : this(20)
        {
        }

        public RichTextBuilder(int capacity)
        {
            _stringBuilder = new StringBuilder(capacity);
        }

        public void Append(string s)
        {
            // @formatter:off
            if (_fontStyle != FontStyle.Normal)
                BeginFontStyle(_fontStyle);

                if (_color.HasValue)
                    BeginColor();
                    
                    if (_size >= 0)
                        BeginSize();

                        _stringBuilder.Append(s);
                        
                    if (_size >= 0)
                        EndSize();

                if (_color.HasValue)
                    EndColor();
            
            if (_fontStyle != FontStyle.Normal)
                EndFontStyle(_fontStyle);
            // @formatter:on
        }

        public void AppendWithoutRich(string s)
        {
            _stringBuilder.Append(s);
        }

        public override string ToString() => _stringBuilder.ToString();

        public void Clear() => _stringBuilder.Clear();

        private void BeginSize()
        {
            _stringBuilder.Append(BeginSizeTagBeforeValue);
            _stringBuilder.Append(_sizeString);
            _stringBuilder.Append(BeginSizeTagAfterValue);
        }

        private void EndSize()
        {
            _stringBuilder.Append(EndSizeTag);
        }

        private void BeginColor()
        {
            _stringBuilder.Append(BeginColorTagBeforeHex);
            _stringBuilder.Append(_colorHex);
            _stringBuilder.Append(BeginColorTagAfterHex);
        }

        private void EndColor()
        {
            _stringBuilder.Append(EndColorTag);
        }

        private void BeginFontStyle(FontStyle style)
        {
            if (style == FontStyle.Bold)
                _stringBuilder.Append(BeginBoldTag);
            else if (style == FontStyle.Italic)
                _stringBuilder.Append(BeginItalicTag);
        }

        private void EndFontStyle(FontStyle style)
        {
            if (style == FontStyle.Bold)
                _stringBuilder.Append(EndBoldTag);
            else if (style == FontStyle.Italic)
                _stringBuilder.Append(EndItalicTag);
        }
    }
}