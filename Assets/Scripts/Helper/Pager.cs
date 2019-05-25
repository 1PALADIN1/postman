using System.Collections.Generic;

namespace Core.Helper
{
    public class Pager<T>
    {
        private int _pageSize;
        private T[] _elements;
        private int _currentPage;

        /// <summary>
        /// Всего страниц
        /// </summary>
        public int TotalPages
        {
            get
            {
                if (_elements.Length % _pageSize == 0)
                    return _elements.Length / _pageSize;
                else
                    return _elements.Length / _pageSize + 1;
            }
        }

        /// <summary>
        /// Номер текущей страницы
        /// </summary>
        public int CurrentPageNumber
        {
            get => _currentPage;
        }

        public Pager(T[] elements, int pageSize)
        {
            _elements = elements;
            _pageSize = pageSize;

            _currentPage = 0;
        }

        /// <summary>
        /// Получение страницы по номеру
        /// </summary>
        /// <param name="pageNumber">Номер страницы</param>
        /// <returns>Возвращает список элементов на странице</returns>
        public T[] GetPage(int pageNumber)
        {
            List<T> result = new List<T>();
            _currentPage = pageNumber;

            int startElement = pageNumber * _pageSize;

            if (startElement < _elements.Length)
            {
                int endElement = startElement + _pageSize;
                if (endElement > _elements.Length)
                    endElement = _elements.Length;

                for (int i = startElement; i < endElement; i++)
                    result.Add(_elements[i]);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получение первой страницы
        /// </summary>
        /// <returns>Возвращает список элементов с первой страницы</returns>
        public T[] GetFirstPage()
        {
            return GetPage(0);
        }

        /// <summary>
        /// Получение следующего номера страницы
        /// </summary>
        /// <returns>Возвращает следущий номер страницы</returns>
        public int GetNextPageNumber()
        {
            _currentPage++;
            if (_currentPage > TotalPages - 1)
                _currentPage = 0;
            return _currentPage;
        }

        /// <summary>
        /// Получение предыдущего номера страницы
        /// </summary>
        /// <returns>Возвращает предыдущий номер страницы</returns>
        public int GetPrevPageNumber()
        {
            _currentPage--;
            if (_currentPage < 0)
                _currentPage = TotalPages - 1;
            return _currentPage;
        }
    }
}