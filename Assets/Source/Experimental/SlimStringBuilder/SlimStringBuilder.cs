using System;

#pragma warning disable 649

namespace Omega.Package.Experimental
{
    public unsafe struct SlimStringBuilder
    {
        private const int StackDataSize = 512;

        private fixed char _stackData[StackDataSize];

        private char[] _heapData;

        private int _currentIndex;
        private int _currentHeapSize;
        private bool _isHeapMode;

        public void Append(string str)
        {
            var newCurrentIndex = _currentIndex + str.Length;

            if (!_isHeapMode && newCurrentIndex >= StackDataSize)
                MoveToHeap();

            fixed (char* ptrSrc = str)
            {
                if (_isHeapMode)
                {
                    if (newCurrentIndex >= _currentHeapSize)
                        ResizeHeap(newCurrentIndex);

                    fixed (char* ptr = _heapData)
                        CharArrayCopy(ptrSrc, ptr + _currentIndex, str.Length);
                }
                else
                    fixed (char* ptr = _stackData)
                        CharArrayCopy(ptrSrc, ptr + _currentIndex, str.Length);
            }

            _currentIndex = newCurrentIndex;
        }

        public void Append(char t)
        {
            if (!_isHeapMode && _currentIndex + 1 >= StackDataSize)
                MoveToHeap();

            if (_isHeapMode)
            {
                if (_currentIndex == _currentHeapSize)
                    ResizeHeap(_currentIndex + 1);

                fixed (char* ptr = _heapData)
                    *(ptr + _currentIndex++) = t;
            }
            else
                fixed (char* ptr = _stackData)
                    *(ptr + _currentIndex++) = t;
        }

        public void AppendLine() => Append('\n');

        public void AppendLine(string text) => Append(text + '\n');

        public void Clear()
        {
            if (_isHeapMode)
                Array.Clear(_heapData, 0, _currentIndex + 1);
            _currentIndex = _currentHeapSize = 0;
            _isHeapMode = false;
        }

        public void Replace(char target, char value)
        {
            if (_isHeapMode)
                fixed (char* ptr = _heapData)
                    ReplaceChars(ptr, _currentIndex);
            else
                fixed (char* ptr = _stackData)
                    ReplaceChars(ptr, _currentIndex);

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

            fixed (char* ptr = _stackData)
                return new string(ptr, 0, _currentIndex);
        }

        private void MoveToHeap()
        {
            _isHeapMode = true;
            _currentHeapSize = StackDataSize * 2;
            _heapData = new char[_currentHeapSize];
            fixed (char* stackPtr = _stackData)
            fixed (char* heapPtr = _heapData)
                for (int i = 0; i < _heapData.Length; i++)
                    *(heapPtr + i) = *(stackPtr + i);
        }

        private void ResizeHeap(int targetSize)
        {
            while (_currentHeapSize < targetSize)
                _currentHeapSize *= 2;

            Array.Resize(ref _heapData, _currentHeapSize);
        }

        private static void CharArrayCopy(char* ptrSrc, char* destination, int length)
        {
            for (int i = 0; i < length; i++)
                *(destination + i) = *(ptrSrc + i);
        }
    }
}