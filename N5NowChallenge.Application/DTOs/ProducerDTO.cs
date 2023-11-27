namespace N5NowChallenge.Application.DTOs;

public class ProducerDTO
{
    public Guid Id { get; set; }
    public string NameOperation { get; set; }
    public ProducerDTO(Guid id, string nameOperation)
    {
        Id = id;
        NameOperation = nameOperation;
    }
}
