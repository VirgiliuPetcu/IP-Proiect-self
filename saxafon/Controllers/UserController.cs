using Microsoft.AspNetCore.Mvc;
using saxafon.Models;

namespace saxafon.Controllers
{
    public class UserController : ControllerBase
    {


        [HttpGet]
        public ActionResult<IEnumerable<User>> getUsers()
        {
            return 

        }


    }
}
