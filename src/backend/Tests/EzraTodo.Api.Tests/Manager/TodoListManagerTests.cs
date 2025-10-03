using EzraTodoApi.Manager;
using EzraTodoApi.Models.DbModels;
using EzraTodoApi.Models.RequestModels;
using EzraTodoApi.Repository;
using FluentResults;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EzraTodo.Api.Tests.Managers;

public class TodoListManagerTests
{
    private readonly Mock<ILogger<TodoListManager>> _loggerMock;
    private readonly Mock<ITodoListRepository> _repositoryMock;
    private readonly TodoListManager _manager;

    public TodoListManagerTests()
    {
        _loggerMock = new Mock<ILogger<TodoListManager>>();
        _repositoryMock = new Mock<ITodoListRepository>();
        _manager = new TodoListManager(_loggerMock.Object, _repositoryMock.Object);
    }

    [Fact]
    public async Task CreateTodoListAsync_ShouldReturnSuccess_WhenValidModel()
    {
        // Arrange
        var requestModel = new CreateTodoListRequestModel { Name = "Test List" };
        var dbModel = new TodoListDbModel { TodoListId = 1, Name = "Test List" };
        _repositoryMock
            .Setup(repo => repo.CreateTodoListAsync(It.IsAny<TodoListDbModel>()))
            .ReturnsAsync(Result.Ok(dbModel));

        // Act
        var result = await _manager.CreateTodoListAsync(requestModel);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Test List", result.Value.Name);
    }

    [Fact]
    public async Task UpdateTodoListAsync_ShouldReturnSuccess_WhenListExists()
    {
        // Arrange
        var requestModel = new UpdateTodoListRequestModel { Name = "Updated List" };
        var existingDbModel = new TodoListDbModel { TodoListId = 1, Name = "Old List" };
        var updatedDbModel = new TodoListDbModel { TodoListId = 1, Name = "Updated List" };

        _repositoryMock
            .Setup(repo => repo.GetTodoListAsync(1))
            .ReturnsAsync(Result.Ok(existingDbModel));
        _repositoryMock
            .Setup(repo => repo.UpdateTodoListAsync(It.IsAny<TodoListDbModel>()))
            .ReturnsAsync(Result.Ok(updatedDbModel));

        // Act
        var result = await _manager.UpdateTodoListAsync(requestModel, 1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Updated List", result.Value.Name);
    }

    [Fact]
    public async Task UpdateTodoListAsync_ShouldReturnFail_WhenListDoesNotExist()
    {
        // Arrange
        var requestModel = new UpdateTodoListRequestModel { Name = "Updated List" };
        _repositoryMock
            .Setup(repo => repo.GetTodoListAsync(1))
            .ReturnsAsync(Result.Fail("Not Found"));

        // Act
        var result = await _manager.UpdateTodoListAsync(requestModel, 1);

        // Assert
        Assert.True(result.IsFailed);
    }
}
