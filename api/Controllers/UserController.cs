namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserRepository userRepository) : ControllerBase
{
    
}
