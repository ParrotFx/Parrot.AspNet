namespace Parrot.AspNet
{
    using Parrot.Infrastructure;

    public class StandardWriterProvider : IParrotWriterProvider
    {
        public IParrotWriter CreateWriter()
        {
            return new StandardWriter();
        }
    }
}