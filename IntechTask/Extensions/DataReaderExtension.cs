using System;
using System.Collections.Generic;
using System.Data;

namespace IntechTask.Extensions
{
    internal static class DataReaderExtension
    {
        public static IEnumerable<T> AsEnumerable<T>(this T source) where T : IDataReader =>
            BuildEnumerable(source.Read, () => source);

        private static IEnumerable<T> BuildEnumerable<T>(Func<bool> moveNext, Func<T> current)
        {
            var wrapper = new EnumeratorWrapper<T>(moveNext, current);

            foreach (var item in wrapper)
            {
                yield return item;
            }
        }

        private sealed class EnumeratorWrapper<T>
        {
            private readonly Func<bool> _moveNext;
            private readonly Func<T> _current;

            public EnumeratorWrapper(Func<bool> moveNext, Func<T> current)
            {
                _moveNext = moveNext;
                _current = current;
            }

            public T Current => _current();

            public EnumeratorWrapper<T> GetEnumerator() => this;

            public bool MoveNext() => _moveNext();
        }
    }
}