namespace KonsoleDotNet
{
    /// <summary>
    /// Represents a scope of an <see cref="IKonsole"/> that enables scoped options changes without changing 
    /// options on the parent <see cref="IKonsole"/> instance.
    /// </summary>
    public interface IKonsoleScope : IKonsole
    {
        /// <summary>
        /// The <see cref="IKonsole"/> this scope was created from.
        /// </summary>
        IKonsole Konsole { get; }
    }
}