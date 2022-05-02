using MyTrainingPal.Service.Interfaces;
using MyTrainingPal.Domain.Common;
using MyTrainingPal.Domain.Entities;
using MyTrainingPal.Service.DTO.User;

namespace MyTrainingPal.Service.Services
{
    public interface IUserMapper : IMapper<User, UserGetDTO, UserPostDTO, UserPutDTO>
    {    }

    public class UserMapper : IUserMapper
    {
        public List<UserGetDTO> EntityListToGetDTOList(List<User> entityList)
            => entityList.Select(e => EntityToGetDTO(e)).ToList();

        public UserGetDTO EntityToGetDTO(User entity)
            => new UserGetDTO
            {
                FullName = entity.FullName,
                Email = entity.Email,
                IsPremium = entity.IsPremium,
                IsAdmin = entity.IsAdmin,
                RegistrationDate = entity.RegistrationDate,
                CompletedWorkouts = entity.CompletedWorkouts
            };

        public Result<User> PostDTOToEntity(UserPostDTO postDTO)
            => throw new NotImplementedException();

        public Result<User> PutDTOToEntity(User currentEntity, UserPutDTO putDTO)
        {
            throw new NotImplementedException();
        }
    }
}
