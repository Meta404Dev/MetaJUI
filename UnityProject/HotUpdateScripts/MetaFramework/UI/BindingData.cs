namespace MetaFramework.UI
{
    public class BindingData<T>
    {
        public delegate void ValueChangeHandler(T oldValue, T newValue);
        public event ValueChangeHandler ValueChange;

        private T _value = default;
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (!Equals(_value, value))
                {
                    T old = _value;
                    _value = value;
                    OnValueChange(old, _value);
                }
            }
        }

        public BindingData(T value)
        {
            Value = value;
        }

        public void OnValueChange(T oldValue, T newValue)
        {
            ValueChange?.Invoke(oldValue, newValue);
        }

        public static implicit operator T(BindingData<T> value)
        {
            return value.Value;
        }
    }
}