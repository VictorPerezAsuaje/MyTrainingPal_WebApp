using MyTrainingPal.Domain.Common;

namespace MyTrainingPal.API.Interfaces
{
    public interface IMapper<T, TGet, TPost>
    {
        List<TGet> EntityListToGetDTOList(List<T> entityList); 
        TGet EntityToGetDTO(T entity);
        Result<T> PostDTOToEntity(TPost postDTO);
    }
}
