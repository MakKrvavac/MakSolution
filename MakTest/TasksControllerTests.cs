using MakApi.Controllers;
using MakApi.Data;
using MakApi.Models.Domain;
using MakApi.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Task = MakApi.Models.Domain.Task;

namespace MakTest;

public class TasksControllerTests
{
    private readonly Mock<MakDbContext> _mockDbContext;
    private readonly Mock<ILogger<TasksController>> _mockLogger;
    private readonly TasksController _controller;

    public TasksControllerTests()
    {
        _mockDbContext = new Mock<MakDbContext>();
        _mockLogger = new Mock<ILogger<TasksController>>();
        _controller = new TasksController(_mockDbContext.Object, _mockLogger.Object);
    }

    [Fact]
    public void GetAll_ReturnsOkResult_WithListOfTasks()
    {
        // Arrange
        var tasks = new List<Task>
        {
            new Task { Id = Guid.NewGuid(), Title = "Task 1", Description = "Description 1" },
            new Task { Id = Guid.NewGuid(), Title = "Task 2", Description = "Description 2" }
        };
        _mockDbContext.Setup(db => db.Tasks.ToList()).Returns(tasks);

        // Act
        var result = _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnTasks = Assert.IsType<List<TaskDto>>(okResult.Value);
        Assert.Equal(2, returnTasks.Count);
    }

    [Fact]
    public void GetAllInProject_ReturnsOkResult_WithListOfTasks()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var project = new Project
        {
            Id = projectId,
            Tasks = new List<Task>
            {
                new Task { Id = Guid.NewGuid(), Title = "Task 1", Description = "Description 1" },
                new Task { Id = Guid.NewGuid(), Title = "Task 2", Description = "Description 2" }
            }
        };
        _mockDbContext.Setup(db => db.Projects.Find(projectId)).Returns(project);

        // Act
        var result = _controller.GetAllInProject(projectId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnTasks = Assert.IsType<List<TaskDto>>(okResult.Value);
        Assert.Equal(2, returnTasks.Count);
    }

    [Fact]
    public void GetById_ReturnsOkResult_WithTask()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var task = new Task { Id = taskId, Title = "Task 1", Description = "Description 1" };
        _mockDbContext.Setup(db => db.Tasks.Find(taskId)).Returns(task);

        // Act
        var result = _controller.GetById(taskId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnTask = Assert.IsType<TaskDto>(okResult.Value);
        Assert.Equal(taskId, returnTask.Id);
    }

    [Fact]
    public void GetById_ReturnsNotFound_WhenTaskDoesNotExist()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        _mockDbContext.Setup(db => db.Tasks.Find(taskId)).Returns((Task)null);

        // Act
        var result = _controller.GetById(taskId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}