namespace Core.Helper.Util
{
    public class OptionalObject<T> where T : class
    {
        private T _optionalObject;
        private bool _hasValue = false;

        public T Value => _optionalObject;
        public bool HasValue => _hasValue;

        public OptionalObject() : this(null) { }

        public OptionalObject(T value)
        {
            SetValue(value);
        }

        public void SetValue(T value)
        {
            if (value == null) _hasValue = false;
            else
                _hasValue = true;

            _optionalObject = value;
        }
    }
}