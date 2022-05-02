using MyTrainingPal.Domain.Common;

namespace MyTrainingPal.Service.Interfaces
{
    public interface IMapper<T, TGet, TPost, TPut>
    {
        List<TGet> EntityListToGetDTOList(List<T> entityList); 
        TGet EntityToGetDTO(T entity);
        Result<T> PostDTOToEntity(TPost postDTO);
        Result<T> PutDTOToEntity(T currentEntity, TPut putDTO);
    }
}
