using MakApi.Data;
using MakApi.Models.Domain;
using MakApi.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MakApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly MakDbContext _dbContext;

    public ProjectsController(MakDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [Authorize]
    public IActionResult GetAll()
    {
        var domainProjects = _dbContext.Projects.ToList();
        var dtoProjects = new List<ProjectDto>();

        foreach (var project in domainProjects)
            dtoProjects.Add(new ProjectDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                Start = project.Start,
                End = project.End,
                PrototypeLink = project.PrototypeLink,
                DesignLink = project.DesignLink,
                GithubLink = project.GithubLink,
                LiveDemoLink = project.LiveDemoLink,
                Progress = project.Progress,
                FileIds = project.FileIds,
                Files = project.Files,
                TaskIds = project.TaskIds,
                Tasks = project.Tasks
            });

        return Ok(dtoProjects);
    }

    [HttpGet]
    [Route("{id:guid}")]
    [Authorize]
    public IActionResult GetById([FromRoute] Guid id)
    {
        var domainProject = _dbContext.Projects.Find(id);

        if (domainProject == null) return NotFound();

        //TODO: Implement Converter Class
        var dtoProject = new ProjectDto
        {
            Id = domainProject.Id,
            Title = domainProject.Title,
            Description = domainProject.Description,
            Start = domainProject.Start,
            End = domainProject.End,
            PrototypeLink = domainProject.PrototypeLink,
            DesignLink = domainProject.DesignLink,
            GithubLink = domainProject.GithubLink,
            LiveDemoLink = domainProject.LiveDemoLink,
            Progress = domainProject.Progress,
            FileIds = domainProject.FileIds,
            Files = domainProject.Files,
            TaskIds = domainProject.TaskIds,
            Tasks = domainProject.Tasks
        };

        return Ok(dtoProject);
    }

    [HttpPost]
    [Authorize(Roles = "Writer")]
    public IActionResult Create([FromBody] CreateProjectDto createProjectDto)
    {
        var domainProject = new Project
        {
            Title = createProjectDto.Title,
            Description = createProjectDto.Description,
            Start = createProjectDto.Start,
            End = createProjectDto.End
        };

        _dbContext.Projects.Add(domainProject);
        _dbContext.SaveChanges();

        var dtoProject = new ProjectDto
        {
            Id = domainProject.Id,
            Title = domainProject.Title,
            Description = domainProject.Description,
            Start = domainProject.Start,
            End = domainProject.End
        };

        return CreatedAtAction(nameof(GetById), new { id = dtoProject.Id }, dtoProject);
    }

    [HttpPut]
    [Route("{id:guid}")]
    [Authorize(Roles = "Writer")]
    public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateProjectDto updateProjectDto)
    {
        var domainProject = _dbContext.Projects.Find(id);

        if (domainProject == null) return NotFound();

        domainProject.Title = updateProjectDto.Title;
        domainProject.Description = updateProjectDto.Description;
        domainProject.Start = updateProjectDto.Start;
        domainProject.End = updateProjectDto.End;
        domainProject.PrototypeLink = updateProjectDto.PrototypeLink;
        domainProject.DesignLink = updateProjectDto.DesignLink;
        domainProject.GithubLink = updateProjectDto.GithubLink;
        domainProject.LiveDemoLink = updateProjectDto.LiveDemoLink;
        domainProject.Progress = updateProjectDto.Progress;
        domainProject.FileIds = updateProjectDto.FileIds;
        domainProject.TaskIds = updateProjectDto.TaskIds;

        _dbContext.SaveChanges();

        var dtoProject = new ProjectDto
        {
            Id = domainProject.Id,
            Title = domainProject.Title,
            Description = domainProject.Description,
            Start = domainProject.Start,
            End = domainProject.End,
            PrototypeLink = domainProject.PrototypeLink,
            DesignLink = domainProject.DesignLink,
            GithubLink = domainProject.GithubLink,
            LiveDemoLink = domainProject.LiveDemoLink,
            Progress = domainProject.Progress,
            FileIds = domainProject.FileIds,
            Files = domainProject.Files,
            TaskIds = domainProject.TaskIds,
            Tasks = domainProject.Tasks
        };

        return Ok(dtoProject);
    }

    [HttpDelete]
    [Route("{id:guid}")]
    [Authorize(Roles = "Writer")]
    public IActionResult Delete([FromRoute] Guid id)
    {
        var domainProject = _dbContext.Projects.Find(id);

        if (domainProject == null) return NotFound();

        _dbContext.Projects.Remove(domainProject);
        _dbContext.SaveChanges();

        return NoContent();
    }
}