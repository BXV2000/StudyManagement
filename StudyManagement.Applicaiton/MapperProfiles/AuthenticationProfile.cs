using AutoMapper;
using StudyManagement.Contracts.Authentication;
using StudyManagement.Domain.Models;

namespace StudyManagement.Applicaiton.MapperProfiles
{
    public class AuthenticationProfile:Profile
    {
        public AuthenticationProfile()
        {
            CreateMap<LoginModel, LoginModelDTO>();
            CreateMap<LoginModelDTO, LoginModel>();
        }
    }
}
