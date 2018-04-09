using EasyNetQ;

namespace ImageCollections.Service.Infrastructure
{
    public interface IBusSubscriber
    {
        IBus Subscribe();
    }
}