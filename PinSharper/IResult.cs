namespace PinSharper
{
    public interface IResult<T>
    {
        int Code { get; set; }
        T Data { get; set; }
        string Message { get; set; }
        string MessageDetail { get; set; }
        bool Succeeded { get; set; }
    }
}