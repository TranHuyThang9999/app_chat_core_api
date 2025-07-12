using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentCoreServiceApi.Services;

namespace PaymentCoreServiceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectController : ControllerBase
{
    private readonly ICurrentUser _currentUser;

    public ProjectController(ICurrentUser currentUser)
    {
        _currentUser = currentUser;
    }

    [HttpGet("my-projects")]
    public IActionResult GetUserProjects()
    {
        // Simulate getting projects for the current user
        var userProjects = new[]
        {
            new {
                Id = 1,
                Name = "Project A",
                OwnerId = _currentUser.Id,
                OwnerName = _currentUser.UserName,
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            },
            new {
                Id = 2,
                Name = "Project B",
                OwnerId = _currentUser.Id,
                OwnerName = _currentUser.UserName,
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            }
        };

        return Ok(new
        {
            UserId = _currentUser.Id,
            UserEmail = _currentUser.Email,
            Projects = userProjects
        });
    }

    [HttpGet("{projectId}")]
    public IActionResult GetProjectById(int projectId)
    {
        // Simulate getting a specific project with validation
        if (!_currentUser.IsAuthenticated)
        {
            return Unauthorized();
        }

        // Simulate project data (in real app, this would come from a database)
        var project = new
        {
            Id = projectId,
            Name = $"Project {projectId}",
            Description = "Project description here",
            OwnerId = _currentUser.Id,
            OwnerName = _currentUser.UserName,
            OwnerEmail = _currentUser.Email,
            CreatedAt = DateTime.UtcNow,
            Status = "Active",
            Members = new[]
            {
                new { Id = _currentUser.Id, Name = _currentUser.UserName, Role = "Owner" }
            }
        };

        return Ok(project);
    }

    [HttpPost("create")]
    public IActionResult CreateProject([FromBody] CreateProjectRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Simulate creating a new project
        var newProject = new
        {
            Id = new Random().Next(100, 999), // Simulate generated ID
            Name = request.Name,
            Description = request.Description,
            OwnerId = _currentUser.Id,
            OwnerName = _currentUser.UserName,
            CreatedAt = DateTime.UtcNow,
            Status = "Active"
        };

        return CreatedAtAction(nameof(GetProjectById), new { projectId = newProject.Id }, newProject);
    }
}

public class CreateProjectRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
