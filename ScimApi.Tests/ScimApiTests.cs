using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Logging;
using Moq;
using ScimApi.Controllers;
using ScimApi.Core.Models;

namespace ScimApi.Tests;

//[ExcludeFromCodeCoverage]
[TestClass]
public class ScimApiTests
{
    private readonly ILogger<UsersController> _logger;
    private readonly ODataQueryOptions<User> _odataOptions;

    public ScimApiTests()
    {
        _logger = new Mock<ILogger<UsersController>>().Object;

        /////// fails here because no parameterless constructor and haven't yet figured out the parameters
        _odataOptions = new Mock<ODataQueryOptions<User>>().Object;
    }

    [TestMethod]
    public async Task GetAllReturnsSuccess()
    {
        // Arrange
        var controller = new UsersController(_logger);

        // Act
        var data = await controller.GetUsers(_odataOptions);
        //var result = data.re

        // Assert
        Assert.IsInstanceOfType(data, typeof(UserList));    
    }


}