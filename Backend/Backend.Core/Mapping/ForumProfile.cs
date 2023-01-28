using AutoMapper;

using Backend.Core.Database.Entities;
using Backend.Core.Identity;
using Backend.Core.Models;
using Backend.Core.Models.Comment;
using Backend.Core.Models.Thread;
using Backend.Core.Models.User;
using Backend.Core.Models.User.Account;
using Thread = Backend.Core.Database.Entities.Thread;

namespace Backend.Core.Mapping;

public class ForumProfile : Profile
{
    public ForumProfile()
    {
        CreateMap<ApplicationUser, UserResponse>()
            .AfterMap<AdminResolverAction<UserResponse>>();
        CreateMap<ApplicationUser, AccountResponse>()
            .ForMember(r => r.Confirmed, cfg => cfg.MapFrom(src => src.EmailConfirmed))
            .AfterMap<AdminResolverAction<AccountResponse>>();

        CreateMap<Thread, ThreadResponse>();
        CreateMap<Comment, CommentResponse>();

        CreateMap(typeof(Page<>), typeof(Page<>));
    }
}