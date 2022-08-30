using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ScimApi.Core.Models;
using System.Linq;

namespace ScimApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {

        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetUsers(ODataQueryOptions<User> odataOptions)
        {
            var users = await Task.FromResult(Enumerable.Range(1, 5).Select(index => new User
            {
                Id = index.ToString(),
                Active = true,
                UserName = $"User{index}"
            })
            .AsQueryable());

            var results = odataOptions
                .ApplyTo(users)
                .Cast<User>()
                .ToList();

            UserList list = new UserList();
            list.Schemas = new List<string>();
            list.Schemas.Add("urn:ietf:params:scim:api:messages:2.0:ListResponse");
            list.TotalResults = results.Count();
            list.ItemsPerPage = results.Count();
            list.StartIndex = 1;
            list.Resources = results;
            return Ok(list);
        }

        [HttpPost]
        public ActionResult CreateUser()
        {
            return Ok();
        }

        [HttpPut]
        public ActionResult ReplaceUser()
        {
            return Ok();
        }

        [HttpPatch]
        public ActionResult UpdateUser()
        {
            return Ok();
        }

        [HttpDelete]
        public ActionResult DeleteUser()
        {
            return Ok();
        }
    }
}