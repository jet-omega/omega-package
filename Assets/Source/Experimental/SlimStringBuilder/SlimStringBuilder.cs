using System;
using System.Text;

#pragma warning disable 649

namespace Omega.Package.Experimental
{
    public unsafe struct SlimStringBuilder
    {
        private const int StackDataSize = 500;

        private fixed char _stackData[StackDataSize];

        private char[] _heapData;

        private int _currentIndex;
        private int _currentHeapSize;
        private bool _isHeapMode;

        public void Append(string value)
        {
            var newCurrentIndex = _currentIndex + value.Length;

            if (!_isHeapMode && newCurrentIndex >= StackDataSize)
                MoveToHeap();

            fixed (char* ptrStr = value)
            {
                if (_isHeapMode)
                {
                    if (newCurrentIndex >= _currentHeapSize)
                        ResizeHeap(newCurrentIndex);

                    fixed (char* ptrHeap = _heapData)
                        CharArrayCopy(ptrStr, ptrHeap + _currentIndex, value.Length);
                }
                else
                    fixed (char* ptrStack = _stackData)
                        CharArrayCopy(ptrStr, ptrStack + _currentIndex, value.Length);
            }

            _currentIndex = newCurrentIndex;
        }

        public void Append(char value)
        {
            if (!_isHeapMode && _currentIndex + 1 >= StackDataSize)
                MoveToHeap();

            if (_isHeapMode)
            {
                if (_currentIndex == _currentHeapSize)
                    ResizeHeap(_currentIndex + 1);

                fixed (char* ptrHeap = _heapData)
                    *(ptrHeap + _currentIndex++) = value;
            }
            else
                fixed (char* ptrStack = _stackData)
                    *(ptrStack + _currentIndex++) = value;
        }

        public void AppendLine() => Append(Environment.NewLine);

        public void AppendLine(string value) => Append(value + Environment.NewLine);

        public void Clear()
        {
            _currentIndex = _currentHeapSize = 0;
            _isHeapMode = false;
            _heapData = null;
        }

        public void Replace(char target, char value)
        {
            if (_isHeapMode)
                fixed (char* ptrHeap = _heapData)
                    ReplaceChars(ptrHeap, _currentIndex);
            else
                fixed (char* ptrStack = _stackData)
                    ReplaceChars(ptrStack, _currentIndex);

            void ReplaceChars(char* ptr, int length)
            {
                for (int i = 0; i < length; i++)
                    if (*(ptr + i) == target)
                        *(ptr + i) = value;
            }
        }

        public override string ToString()
        {
            if (_isHeapMode)
                return new string(_heapData, 0, _currentIndex);

            fixed (char* ptrStack = _stackData)
                return new string(ptrStack, 0, _currentIndex);
        }

        private void MoveToHeap()
        {
            _isHeapMode = true;
            _currentHeapSize = StackDataSize * 2;
            _heapData = new char[_currentHeapSize];
            fixed (char* ptrStack = _stackData)
            fixed (char* ptrHeap = _heapData)
                for (int i = 0; i < _heapData.Length; i++)
                    *(ptrHeap + i) = *(ptrStack + i);
        }

        private void ResizeHeap(int targetSize)
        {
            while (_currentHeapSize < targetSize)
                _currentHeapSize *= 2;

            Array.Resize(ref _heapData, _currentHeapSize);
        }

        private static void CharArrayCopy(char* ptrSrc, char* ptrDst, int length)
        {
            for (int i = 0; i < length; i++)
                *(ptrDst + i) = *(ptrSrc + i);
        }
    }
}