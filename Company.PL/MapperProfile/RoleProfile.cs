using AutoMapper;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Company.PL.MapperProfile
{
    public class RoleProfile:Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleViewModel,IdentityRole>()
                .ForMember(D=>D.Name,o=>o.MapFrom(s=>s.RoleName)).ReverseMap();
        }
    }
}
