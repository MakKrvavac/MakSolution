using MakApi.Data;
using MakApi.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Task = MakApi.Models.Domain.Task;

namespace MakApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : Controller
{
    private readonly MakDbContext _dbContext;
    private readonly ILogger<TasksController> _logger;

    public TasksController(MakDbContext dbContext, ILogger<TasksController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet]
    //[Authorize]
    public IActionResult GetAll()
    {
        var domainTasks = _dbContext.Tasks.ToList();
        var dtoTasks = new List<TaskDto>();

        foreach (var task in domainTasks)
            dtoTasks.Add(new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                ResponsibleUserId = task.ResponsibleUserId,
                ResponsibleUser = task.ResponsibleUser,
                StoryPoints = task.StoryPoints,
                EpicId = task.EpicId,
                Epic = task.Epic,
                TaskProgress = task.TaskProgress
            });

        return Ok(dtoTasks);
    }

    [HttpGet("{projectId:guid}")]
    //[Authorize]
    public IActionResult GetAllInProject(Guid projectId)
    {
        var domainProject = _dbContext.Projects.Find(projectId);

        var domainTasks = domainProject.Tasks;
        var dtoTasks = new List<TaskDto>();

        foreach (var task in domainTasks)
            dtoTasks.Add(new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                ResponsibleUserId = task.ResponsibleUserId,
                ResponsibleUser = task.ResponsibleUser,
                StoryPoints = task.StoryPoints,
                EpicId = task.EpicId,
                Epic = task.Epic,
                TaskProgress = task.TaskProgress
            });

        return Ok(dtoTasks);
    }

    [HttpGet("{id:guid}")]
    //[Authorize]
    public IActionResult GetById(Guid id)
    {
        var domainTask = _dbContext.Tasks.Find(id);

        if (domainTask == null) return NotFound();

        var dtoTask = new TaskDto
        {
            Id = domainTask.Id,
            Title = domainTask.Title,
            Description = domainTask.Description,
            ResponsibleUserId = domainTask.ResponsibleUserId,
            ResponsibleUser = domainTask.ResponsibleUser,
            StoryPoints = domainTask.StoryPoints,
            EpicId = domainTask.EpicId,
            Epic = domainTask.Epic,
            TaskProgress = domainTask.TaskProgress
        };

        return Ok(dtoTask);
    }

    [HttpPost]
    //[Authorize(Roles = "Writer")]
    public IActionResult Create([FromBody] CreateTaskDto createTaskDto)
    {
        var domainTask = new Task
        {
            Title = createTaskDto.Title,
            Description = createTaskDto.Description,
            ResponsibleUserId = createTaskDto.ResponsibleUserId,
            StoryPoints = createTaskDto.StoryPoints,
            EpicId = createTaskDto.EpicId
        };

        _dbContext.Tasks.Add(domainTask);
        _dbContext.SaveChanges();

        var domainProject = _dbContext.Projects.Find(createTaskDto.ProjectId);
        _logger.LogInformation($"Project: {domainProject.Title}");

        domainProject.Tasks.Add(domainTask);
        _dbContext.SaveChanges();

        var dtoTask = new TaskDto
        {
            Id = domainTask.Id,
            Title = domainTask.Title,
            Description = domainTask.Description,
            ResponsibleUserId = domainTask.ResponsibleUserId,
            StoryPoints = domainTask.StoryPoints,
            EpicId = domainTask.EpicId,
            TaskProgress = domainTask.TaskProgress
        };

        return CreatedAtAction(nameof(GetById), new { id = dtoTask.Id }, dtoTask);
    }

    [HttpPut("{id:guid}")]
    //[Authorize]
    public IActionResult Update(Guid id, [FromBody] UpdateTaskDto updateTaskDto)
    {
        var domainTask = _dbContext.Tasks.Find(id);

        if (domainTask == null) return NotFound();

        domainTask.Title = updateTaskDto.Title;
        domainTask.Description = updateTaskDto.Description;
        domainTask.ResponsibleUserId = updateTaskDto.ResponsibleUserId;
        domainTask.StoryPoints = updateTaskDto.StoryPoints;
        domainTask.EpicId = updateTaskDto.EpicId;

        _dbContext.SaveChanges();

        var dtoTask = new TaskDto
        {
            Id = domainTask.Id,
            Title = domainTask.Title,
            Description = domainTask.Description,
            ResponsibleUserId = domainTask.ResponsibleUserId,
            StoryPoints = domainTask.StoryPoints,
            EpicId = domainTask.EpicId,
            TaskProgress = domainTask.TaskProgress
        };

        return Ok(dtoTask);
    }

    [HttpDelete("{id:guid}")]
    //[Authorize(Roles = "Writer")]
    public IActionResult Delete(Guid id)
    {
        var domainTask = _dbContext.Tasks.Find(id);

        if (domainTask == null) return NotFound();

        _dbContext.Tasks.Remove(domainTask);
        _dbContext.SaveChanges();

        return NoContent();
    }
}