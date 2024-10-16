using System.Data;
using System.Data.Common;
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
using Microsoft.EntityFrameworkCore;
using Xunit;
using Task = MakApi.Models.Domain.Task;

namespace MakTest;

public class TasksControllerTests
{
    private readonly MakDbContext _dbContext;
    private readonly ILogger<TasksController> _logger;
    private readonly TasksController _controller;

    public TasksControllerTests(DatabaseFixture fixture, ILogger<TasksController> logger)
    {
        _dbContext = fixture._DbContext;
        _logger = logger;
        _controller = new TasksController(_dbContext, _logger);
    }
    
    [Fact]
    public async void GetAll_ReturnsOkResult_WithListOfTasks()
    {
        // Arrange
        var tasks = new List<Task>
        {
            new Task { Id = Guid.NewGuid(), Title = "Task 1", Description = "Description 1" },
            new Task { Id = Guid.NewGuid(), Title = "Task 2", Description = "Description 2" }
        };

        // Act
        _dbContext.Tasks.AddRange(tasks);
        await _dbContext.SaveChangesAsync();
        
        // Assert
        var savedEntities = await _dbContext.Tasks.ToListAsync();
        Assert.NotNull(savedEntities);
        
        
        // Act
        var result = _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnTasks = Assert.IsType<List<TaskDto>>(okResult.Value);
        Assert.Equal(2, returnTasks.Count);
    }
}