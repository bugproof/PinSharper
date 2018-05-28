namespace PinSharper
{
    public class Result<T> : IResult<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string MessageDetail { get; set; }
        public int Code { get; set; }
        public T Data { get; set; }
    }
}
