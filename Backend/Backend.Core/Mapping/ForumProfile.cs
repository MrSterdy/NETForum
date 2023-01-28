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
            .ForMember(r => r.Admin, opt => opt.MapFrom<AdminResolver<UserResponse>>());
        CreateMap<ApplicationUser, AccountResponse>()
            .ForMember(r => r.Admin, opt => opt.MapFrom<AdminResolver<AccountResponse>>());

        CreateMap<Thread, ThreadResponse>()
            .ForMember(r => r.CommentCount, opt => opt.MapFrom<CommentCountResolver>());
        CreateMap<Comment, CommentResponse>();

        CreateMap(typeof(Page<>), typeof(Page<>));
    }
}