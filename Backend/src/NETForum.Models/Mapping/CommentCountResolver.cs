using AutoMapper;

using NETForum.Infrastructure.Database.Repositories;
using NETForum.Models.Responses;
using Thread = NETForum.Infrastructure.Database.Entities.Thread;

namespace NETForum.Models.Mapping;

public class CommentCountResolver : IValueResolver<Thread, ThreadResponse, int>
{
    private readonly ICommentRepository _commentRepository;

    public CommentCountResolver(ICommentRepository commentRepository) =>
        _commentRepository = commentRepository;

    public int Resolve(Thread source, ThreadResponse destination, int destMember, ResolutionContext context) =>
        _commentRepository.GetCountByThreadIdAsync(source.Id).GetAwaiter().GetResult();
}