using N5NowChallenge.Application.DTOs;

namespace N5NowChallenge.Application.Kafka;

public interface IProducerService
{
    Task<string> ProduceMessageAsync(ProducerDTO entity);
}
