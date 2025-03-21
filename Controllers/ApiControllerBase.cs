namespace portal.Controllers;

using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]s")] // Pluralizes controller names
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public abstract class ApiControllerBase : ControllerBase { }
