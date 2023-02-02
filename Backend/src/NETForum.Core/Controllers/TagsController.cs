using NETForum.Infrastructure.Database.Entities;
using NETForum.Infrastructure.Database.Repositories;
using NETForum.Models.Responses;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;

namespace NETForum.Core.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly ITagRepository _repository;

    private readonly IMapper _mapper;

    public TagsController(ITagRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<Page<TagResponse>> Search(int page, string? name) =>
        _mapper.Map<Page<TagResponse>>(await _repository.SearchAsync(page, name));
}