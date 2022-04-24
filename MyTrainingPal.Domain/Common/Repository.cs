namespace MyTrainingPal.Domain.Common
{
    public abstract class BaseRepository
    {
        protected string connectionString = "Server=(localdb)\\mssqllocaldb;Database=MyTrainingPalDb;Trusted_Connection=True;MultipleActiveResultSets=true";
    }
}
