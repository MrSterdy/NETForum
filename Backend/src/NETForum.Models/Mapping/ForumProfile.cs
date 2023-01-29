using AutoMapper;

using NETForum.Models.Responses;
using NETForum.Infrastructure.Identity;
using NETForum.Infrastructure.Database.Entities;
using Thread = NETForum.Infrastructure.Database.Entities.Thread;

namespace NETForum.Models.Mapping;

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