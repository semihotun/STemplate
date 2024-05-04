namespace STemplate.Domain.SeedWork
{
    public interface ISeed<T> where T : IEntity
    {
        List<T> GetSeedData();
    }
}
