// using Microsoft.AspNetCore.Mvc;
// using Mono.Api.Contracts.Users;

// namespace Mono.Api.Controllers;

// [ApiController]
// [Route("api/users")]
// public sealed class UsersController : ControllerBase
// {
//     [HttpPost]
//     public IActionResult Create(CreateUserRequest request)
//     {
//         var response = new UserResponse
//         {
//             Guid = Guid.NewGuid(),
//             FirstName = request.FirstName,
//             LastName = request.LastName,
//             Surname = request.Surname,
//             Birthday = request.Birthday,
//             Description = request.Description,
//             Created = DateTimeOffset.UtcNow
//         };
//
//         return Created($"/api/users/{response.Guid}", response);
//     }
// }