using Confluent.Kafka;
using N5NowChallenge.Application.DTOs;
using Newtonsoft.Json;

namespace N5NowChallenge.Application.Kafka;

public class ProducerService : IProducerService
{ 
    private readonly ProducerConfig _producerConfig;
    private readonly string _topic;
    public ProducerService(ProducerConfig producerConfig, string topic)
    {
        _producerConfig = producerConfig;
        _topic = topic;
    }
    public async Task<string> ProduceMessageAsync(ProducerDTO entity)
    {
        using (var producer = new ProducerBuilder<Null, string>(_producerConfig).Build())
        {
            var message = new Message<Null, string>
            {
                Value = JsonConvert.SerializeObject(entity)
            };
            var result = await producer.ProduceAsync(_topic, message);
            
            //TODO:Agregar Enum status
            return result.Message.Value;
        }
    }
}
