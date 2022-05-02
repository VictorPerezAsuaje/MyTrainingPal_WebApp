namespace MyTrainingPal.Domain.Entities
{
    public class WorkoutHistory
    {
        public Workout Workout { get; private set; }
        public DateTime _CompletionDate { get; private set; }
        public string CompletionDate { get => _CompletionDate.ToShortDateString(); }

        public WorkoutHistory(Workout workout, DateTime completionDate)
        {
            Workout = workout;
            _CompletionDate = completionDate;
        }
    }
}
