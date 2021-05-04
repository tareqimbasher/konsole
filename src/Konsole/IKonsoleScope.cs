namespace Konsole
{
    public interface IKonsoleScope : IKonsole
    {
        IKonsole Konsole { get; }
    }
}