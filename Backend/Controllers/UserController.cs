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
    
    [HttpGet("id/{id:int}")]
    public async Task<User?> GetByIdAsync(int id) => 
        await _repository.GetByIdAsync(id);
}