﻿using NETForum.Infrastructure.Database.Entities;
using NETForum.Infrastructure.Database.Repositories;
using NETForum.Models.Responses;
using NETForum.Models.Requests;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
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

    public async Task<ActionResult<TagResponse>> GetById(int id)
    {
        var tag = await _repository.GetByIdAsync(id);

        return tag is null ? NotFound() : _mapper.Map<TagResponse>(tag);
    }

    [HttpGet]
    public async Task<Page<TagResponse>> Search(int page, string? name) =>
        _mapper.Map<Page<TagResponse>>(await _repository.SearchAsync(page, name));

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] TagRequest model)
    {
        if (await _repository.Exists(model.Name))
            return Conflict();

        await _repository.AddAsync(new Tag { Name = model.Name });

        return Ok();
    }
    
    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateById(int id, [FromBody] TagRequest model)
    {
        var tag = await _repository.GetByIdAsync(id);

        if (tag is null)
            return NotFound();

        if (await _repository.Exists(model.Name))
            return Conflict();

        tag.Name = model.Name;

        await _repository.UpdateAsync(tag);

        return Ok();
    }
}