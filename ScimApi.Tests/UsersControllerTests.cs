using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OData.UriParser;
using Moq;
using ScimApi.Controllers;
using ScimApi.Core.Models;

namespace ScimApi.Tests;

//[ExcludeFromCodeCoverage]
[TestClass]
public class UsersControllerTests
{
    private readonly ILogger<UsersController> _logger;

    public UsersControllerTests()
    {
        _logger = new Mock<ILogger<UsersController>>().Object;
    }

    [TestMethod]
    public async Task GetAllReturnsUsersList()
    {
        // Arrange
        ODataQueryOptions<User> mockODataQueryOptions = CreateUserMockQueryOptions();

        var controller = new UsersController(_logger);

        // Act
        var data = await controller.GetUsers(mockODataQueryOptions);

        // Assert
        Assert.IsInstanceOfType(data, typeof(OkObjectResult), $"return should be of type OkObjectResult but is actually type '{data.GetType().Name}'.");
        var dataValue = ((OkObjectResult)data).Value;
        Assert.IsInstanceOfType(dataValue, typeof(UserList), $"returned data should be type UserList but is actually type '{dataValue?.GetType().Name}'");
        Assert.IsNotNull(dataValue, "returned data is null");

        var userName = ((UserList)dataValue).Resources[0]?.UserName ?? "";
        Assert.AreNotEqual(userName, "", "First User's name is null or empty but should have a value.");
    }

    [TestMethod]
    public void CreateUserReturnsUser()
    {
        // Arrange
        var controller = new UsersController(_logger);

        // Act
        var data = controller.CreateUser();

        // Assert
        Assert.IsInstanceOfType(data, typeof(OkObjectResult), $"return should be of type OkObjectResult but is actually type '{data.GetType().Name}'.");
        var dataValue = ((OkObjectResult)data).Value;
        Assert.IsInstanceOfType(dataValue, typeof(User), $"returned data should be type User but is actually type '{dataValue?.GetType().Name}'");
        Assert.IsNotNull(dataValue, "returned data is null");

        //string userName = ((User)dataValue)?.UserName ?? "";
        //Assert.AreNotEqual(userName, "", "User name is null or empty but should have a value.");
    }

    #region Helpers
    private static ODataQueryOptions<User> CreateUserMockQueryOptions()
    {
        var modelBuilder = new ODataConventionModelBuilder();
        modelBuilder.EntitySet<User>("User");
        var edmModel = modelBuilder.GetEdmModel();

        const string routeName = "odata";
        IEdmEntitySet entitySet = edmModel.EntityContainer.FindEntitySet("User");
        ODataPath path = new ODataPath(new EntitySetSegment(entitySet));

        var request = HttpRequestFactory.Create("GET", "http://localhost/whatever",
            dataOptions => dataOptions.AddRouteComponents(routeName, edmModel));

        request.ODataFeature().Model = edmModel;
        request.ODataFeature().Path = path;
        request.ODataFeature().RoutePrefix = routeName;

        var oDataQueryContext = new ODataQueryContext(edmModel, typeof(User), new ODataPath());
        var oDataQueryOptions = new ODataQueryOptions<User>(oDataQueryContext, request);
        return oDataQueryOptions;
    }

    private static class HttpRequestFactory
    {
        // lifted from https://stackoverflow.com/questions/66554114/mocking-or-creating-pragmatically-odataqueryoptions-for-net-5-core-controller-u
        /// <summary>
        /// Creates the <see cref="HttpRequest"/> with OData configuration.
        /// </summary>
        /// <param name="method">The http method.</param>
        /// <param name="uri">The http request uri.</param>
        /// <param name="setupAction"></param>
        /// <returns>The HttpRequest.</returns>
        public static HttpRequest Create(string method, string uri, Action<ODataOptions> setupAction)
        {
            HttpContext context = new DefaultHttpContext();
            HttpRequest request = context.Request;

            IServiceCollection services = new ServiceCollection();
            services.Configure(setupAction);
            context.RequestServices = services.BuildServiceProvider();

            request.Method = method;
            var requestUri = new Uri(uri);
            request.Scheme = requestUri.Scheme;
            request.Host = requestUri.IsDefaultPort 
                ? new HostString(requestUri.Host) 
                : new HostString(requestUri.Host, requestUri.Port);
            request.QueryString = new QueryString(requestUri.Query);
            request.Path = new PathString(requestUri.AbsolutePath);

            return request;
        }
    }
    #endregion Helpers

}