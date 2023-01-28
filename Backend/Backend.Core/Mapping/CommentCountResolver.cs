using AutoMapper;

using Backend.Core.Database.Repositories;
using Backend.Core.Models.Thread;
using Thread = Backend.Core.Database.Entities.Thread;

namespace Backend.Core.Mapping;

public class CommentCountResolver : IValueResolver<Thread, ThreadResponse, int>
{
    private readonly ICommentRepository _commentRepository;

    public CommentCountResolver(ICommentRepository commentRepository) =>
        _commentRepository = commentRepository;

    public int Resolve(Thread source, ThreadResponse destination, int destMember, ResolutionContext context) =>
        _commentRepository.GetCountByThreadIdAsync(source.Id).GetAwaiter().GetResult();
}