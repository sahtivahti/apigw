namespace apigw.Cache
{
    public class CacheValue<T>
    {
        public bool HasValue { get; }
        public T Value { get; }

        public CacheValue(T value)
        {
            HasValue = value != null;
            Value = value;
        }
    }
}
