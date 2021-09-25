using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SpyDuhApiProject2.DataAccess;
using SpyDuhApiProject2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpyDuhApiProject2.Controllers
{
    [Route("api/spyDuhMembers")]
    [ApiController]
    public class SpyDuhMembersController : ControllerBase
    {
        SpyRepository _spiesRepository;
        SpyDuhMembersRepository _spyDuhMembersRepository;

        public SpyDuhMembersController(SpyDuhMembersRepository repo)
        {
            _spiesRepository = new SpyRepository();
            _spyDuhMembersRepository = repo;
        }

        [HttpPost]
        public IActionResult CreateSpyDuhMember(CreateSpyDuhMemberCommand command)
        {
            var spy = _spiesRepository.GetById(command.SpyId);

            if (spy == null) 
                return NotFound("There was no matching spy in the database");
            var spyDuhMember = new SpyDuhMember 
            {
                Id = spy.Id,
                Alias = spy.Alias,
                AboutMe = spy.AboutMe,
                Skills = command.Skills,
                Services = command.Services,
                Friends = command.Friends,
                Enemies = command.Enemies,
            };
            
            _spyDuhMembersRepository.Add(spyDuhMember);

            return Created($"/api/spyDuhMembers/{spy.Id}", spyDuhMember); 

        }

        [HttpGet]
        public IActionResult GetAllSpyDuhMembers()
        {
            return Ok(_spyDuhMembersRepository.GetAll());
        }

        [HttpGet("membersBySkill")]
        public IActionResult GetMembersBySkill(string skill)
        {
            var foundBySkill = _spyDuhMembersRepository.FindBySkill(skill);

            if (foundBySkill.Any() == false)
                return NotFound("No members possess this skill.");

            return Ok(foundBySkill);      
        }
        

        [HttpPost("addFriend")]
        public IActionResult AddFriendToSpyDuhAccount(Friend newFriend)
        {
            _spyDuhMembersRepository.AddFriend(newFriend);
            return Created($"api/spyDuhMembers/addFriend/{newFriend.RelationshipId}", newFriend);
        }

        //[HttpPatch("removeFriend/{accountId}")]
        //public IActionResult RemoveFriendFromSpyDuhAccount(Guid accountId, Guid friendId)
        //{
        //    var member = _spyDuhMembersRepository.GetById(accountId);
        //    if (!(member.Friends.Any()) || !(member.Friends.Contains(friendId)))
        //    {
        //        return NotFound("No friends exist, or friend does not exist under this member.");
        //    }
        //    _spyDuhMembersRepository.RemoveFriendFromSpyDuhAccount(accountId, friendId);
        //    return Ok(member);
        //}

        [HttpPost("addEnemy")]
        public IActionResult AddEnemyToSpyDuhAccount(Enemy newEnemy)
        {
            _spyDuhMembersRepository.AddEnemy(newEnemy);
            return Created($"api/spyDuhMembers/AddEnemy/{newEnemy.RelationshipId}", newEnemy);
        }

        [HttpDelete("deleteEnemy/{id}")]
        public IActionResult RemoveEnemy(Guid id)
        {
            _spyDuhMembersRepository.RemoveEnemy(id);
            return Ok();
        }

        //[HttpGet("enemies/{accountId}")]
        //public IActionResult ShowEnemiesOfAccount(Guid accountId)
        //{
        //    return Ok(_spyDuhMembersRepository.ShowAccountEnemies(accountId));
        //}
        //[HttpGet("friends/{accountId}")]
        //public IActionResult ShowFriendsOfAccount(Guid accountId)
        //{
        //    return Ok(_spyDuhMembersRepository.ShowAccountFriends(accountId));
        //}

        [HttpGet("skills")]
        public IActionResult GetSpyDuhMemberSkills(Guid accountId)
        {
            return Ok(_spyDuhMembersRepository.GetMemberSkills(accountId));
        }

        [HttpGet("services")]
        public IActionResult GetSpyDubMemberServices(Guid accountId)
        {
            return Ok(_spyDuhMembersRepository.GetMemberServices(accountId));
        }

        [HttpPost("addSkill")]
        public IActionResult AddMemberSkill(Skill newSkill)
        {
            _spyDuhMembersRepository.AddSkill(newSkill);
            return Created($"api/spyduhMembers/addSkill/{newSkill.SkillId}", newSkill);
        }

        [HttpPatch("removeSkill/{accountId}")]
        public IActionResult RemoveMemberSkill(Guid accountId, string skill)
        {
            var member = _spyDuhMembersRepository.GetById(accountId);
            if (!(member.Skills.Any()) || !(member.Skills.Contains(skill)))
            {
                return NotFound("No skills exist, or skill does not exist under this member.");
            }
            return Ok(_spyDuhMembersRepository.RemoveSkill(accountId, skill));
        }

        [HttpPost("addService")]
        public IActionResult AddMemberService(Service newService)
        {
            _spyDuhMembersRepository.AddService(newService);
            return Created($"api/spyduhMembers/AddService/{newService.ServiceId}", newService);
        }

        [HttpPatch("removeService/{accountId}")]
        public IActionResult RemoveMemberService(Guid accountId, string service)
        {
            var member = _spyDuhMembersRepository.GetById(accountId);
            if (!(member.Services.Any()) || !(member.Services.Contains(service)))
            {
                return NotFound("No services exist, or service does not exist under this member.");
            }
            return Ok(_spyDuhMembersRepository.RemoveService(accountId, service));
        }

    }
}
