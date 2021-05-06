namespace KonsoleDotNet
{
    public interface IKonsoleScope : IKonsole
    {
        IKonsole Konsole { get; }
    }
}