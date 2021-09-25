using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpyDuhApiProject2.DataAccess;
using SpyDuhApiProject2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpyDuhApiProject2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HiresController : ControllerBase
    {
        HireRepository _hireRepo;
        SpyDuhMembersRepository _memberRepo;
        //public HiresController()
        //{
        //    _hireRepo = new HireRepository();
        //    _memberRepo = new SpyDuhMembersRepository();
        //}
        [HttpPost]
        public IActionResult HireASpy(HireASpyCommand command) 
        {
            var findSpy = _memberRepo.FindByService(command.Service);

            if (findSpy == null)
                return NotFound("No spies provide this service.");

            var hire = new Hire
            {
                EmployerId = command.EmployerId,
                HireeId = findSpy.Id,
                HireeAlias = findSpy.Alias,
                Price = command.Price,
                Service = command.Service,
                HireDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(command.days)
            };

            _hireRepo.Add(hire);
            return Created($"/api/hires/{hire.Id}", hire);
        }
    }
}
