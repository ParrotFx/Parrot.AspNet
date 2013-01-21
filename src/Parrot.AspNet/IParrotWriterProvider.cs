namespace Parrot.AspNet
{
    using Parrot.Infrastructure;

    public interface IParrotWriterProvider
    {
        IParrotWriter CreateWriter();
    }
}