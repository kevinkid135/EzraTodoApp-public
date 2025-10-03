using EzraTodoApi.Manager;
using EzraTodoApi.Models.DbModels;
using EzraTodoApi.Models.RequestModels;
using EzraTodoApi.Repository;
using FluentResults;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EzraTodo.Api.Tests.Managers;

public class TodoItemManagerTests
{
    private readonly Mock<ILogger<TodoItemManager>> _loggerMock;
    private readonly Mock<ITodoItemRepository> _repositoryMock;
    private readonly TodoItemManager _manager;

    public TodoItemManagerTests()
    {
        _loggerMock = new Mock<ILogger<TodoItemManager>>();
        _repositoryMock = new Mock<ITodoItemRepository>();
        _manager = new TodoItemManager(_loggerMock.Object, _repositoryMock.Object);
    }

    [Fact]
    public async Task CreateTodoItemAsync_ShouldReturnSuccess_WhenValidModel()
    {
        // Arrange
        var requestModel = new CreateTodoItemRequestModel { Title = "Test Item" };
        var dbModel = new TodoItemDbModel { TodoItemId = 1, Title = "Test Item", TodoListId = 1 };
        _repositoryMock
            .Setup(repo => repo.CreateTodoItemAsync(It.IsAny<TodoItemDbModel>()))
            .ReturnsAsync(Result.Ok(dbModel));

        // Act
        var result = await _manager.CreateTodoItemAsync(requestModel, 1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Test Item", result.Value.Title);
    }

    [Fact]
    public async Task UpdateTodoItemAsync_ShouldReturnSuccess_WhenItemExists()
    {
        // Arrange
        var requestModel = new UpdateTodoItemRequestModel { Title = "Updated Item" };
        var existingDbModel = new TodoItemDbModel { TodoItemId = 1, Title = "Old Item", TodoListId = 1 };
        var updatedDbModel = new TodoItemDbModel { TodoItemId = 1, Title = "Updated Item", TodoListId = 1 };

        _repositoryMock
            .Setup(repo => repo.GetTodoItemAsync(1, 1))
            .ReturnsAsync(Result.Ok(existingDbModel));
        _repositoryMock
            .Setup(repo => repo.UpdateTodoItemAsync(It.IsAny<TodoItemDbModel>()))
            .ReturnsAsync(Result.Ok(updatedDbModel));

        // Act
        var result = await _manager.UpdateTodoItemAsync(requestModel, 1, 1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Updated Item", result.Value.Title);
    }

    [Fact]
    public async Task UpdateTodoItemAsync_ShouldReturnFail_WhenItemDoesNotExist()
    {
        // Arrange
        var requestModel = new UpdateTodoItemRequestModel { Title = "Updated Item" };
        _repositoryMock
            .Setup(repo => repo.GetTodoItemAsync(1, 1))
            .ReturnsAsync(Result.Fail("Not Found"));

        // Act
        var result = await _manager.UpdateTodoItemAsync(requestModel, 1, 1);

        // Assert
        Assert.True(result.IsFailed);
    }
}
