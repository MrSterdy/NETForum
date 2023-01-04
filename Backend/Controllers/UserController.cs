using Backend.Database.Entities;
using Backend.Database.Repositories;

using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _repository;

    public UserController(IUserRepository repository) =>
        _repository = repository;
    
    [HttpGet("id/{id:long}")]
    public async Task<User?> GetByIdAsync(long id) => 
        await _repository.GetByIdAsync(id);
}