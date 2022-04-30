namespace MyTrainingPal.Domain.Entities
{
    public class MuscleGroup
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public MuscleGroup(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
