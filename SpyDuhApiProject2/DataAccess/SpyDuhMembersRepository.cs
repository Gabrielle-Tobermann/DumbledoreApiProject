using SpyDuhApiProject2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace SpyDuhApiProject2.DataAccess
{
    public class SpyDuhMembersRepository
    {
        string _connectionString;

        public SpyDuhMembersRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("SpyDuh");
        }

        static List<SpyDuhMember> _spyDuhMembers = new List<SpyDuhMember>
        {
            new SpyDuhMember
            {
                Alias = "Harry",
                Id = Guid.Parse("e7c4af1e-c8d4-4998-a611-0b31cc62d312"),
                AboutMe = "I'm super nosy so I became a spy.",
                Skills = new List<string> { "Stealth", "Investigation", "camouflage" },
                Services = new List<string> {"Breaking in to read people's diaries", "Infiltrating an organization"},
                Friends = new List<Guid>
                {
                    Guid.Parse("0a0498c4-6d99-4de8-b687-127a0b89bb2a")
                },
                Enemies = new List<Guid>
                {
                    Guid.Parse("14d2d829-a609-4fe4-82ad-5dab2444e274")
                }
            },
             new SpyDuhMember
            {
                Alias = "Larry",
                Id = Guid.Parse("11692ac4-9d81-4be5-8fd3-5154b1579a94"),
                AboutMe = "I became a spy to take down evil corporations.",
                Skills = new List<string> { "hacking", "investigation", "impersonation" },
                Services = new List<string> {"Breaking in to read people's diaries", "Hacking into a corporation's sensitive data"},
                Friends = new List<Guid>
                {
                    Guid.Parse("14d2d829-a609-4fe4-82ad-5dab2444e274")
                },
                Enemies = new List<Guid>
                {
                    Guid.Parse("0a0498c4-6d99-4de8-b687-127a0b89bb2a")
                }
             }
        };

        internal void Add(SpyDuhMember spyDuhMember)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"insert into SpyduhMembers";
        }

        internal IEnumerable<SpyDuhMember> GetAll()
        {
            return _spyDuhMembers;
        }

        internal IEnumerable<SpyDuhMember> FindBySkill(string skill)
        {
            var foundBySkill = _spyDuhMembers.Where(member => member.Skills.ConvertAll(skill => skill.ToLower()).Contains(skill.ToLower()));
            return foundBySkill;
        }

        internal SpyDuhMember FindByService(string service)
        {
            var foundByService = _spyDuhMembers.FirstOrDefault(member => member.Services.ConvertAll(service => service.ToLower()).Contains(service.ToLower()));
            return foundByService;
        }

        internal SpyDuhMember GetById(Guid spyDuhId)
        {
            return _spyDuhMembers.FirstOrDefault(spyDuhMember => spyDuhMember.Id == spyDuhId);
        }

        internal void  AddFriend(Friend newFriend)
        {
            // Adding a friend to the Friends table with:
            // SpyId, FriendId, RelationshipId (pk)

            var db = new SqlConnection(_connectionString);

            var sql = @"insert into Friends(SpyId, FriendId)
                        output inserted.RelationshipId
                        values (@SpyId, @FriendId)";

            var id = db.ExecuteScalar<Guid>(sql, newFriend);
            newFriend.RelationshipId = id;
        }
        //internal void RemoveFriendFromSpyDuhAccount(Guid accountId, Guid friendId)
        //{
        //    var repo = new SpyDuhMembersRepository();
        //    var spyDuhMember = repo.GetById(accountId);
        //    spyDuhMember.Friends.Remove(friendId);
        //}
        internal void AddEnemy(Enemy newEnemy)
        {
            var db = new SqlConnection(_connectionString);

            var sql = @"insert into Enemies(SpyId, EnemyId)
                         output inserted.RelationshipId
                         values (@SpyId, @EnemyId)";

            var id = db.ExecuteScalar<Guid>(sql, newEnemy);
            newEnemy.RelationshipId = id;
        }
        //internal void RemoveEnemyFromSpyDuhAccount(Guid accountId, Guid enemyId)
        //{
        //    var repo = new SpyDuhMembersRepository();
        //    var spyDuhMember = repo.GetById(accountId);
        //    spyDuhMember.Enemies.Remove(enemyId);
        // }

        //internal List<Guid> ShowAccountEnemies(Guid accountId)
        //{
        //    var repo = new SpyDuhMembersRepository();
        //    var spyDuhMember = repo.GetById(accountId);
        //    return spyDuhMember.Enemies;
        //}

        //internal List<Guid> ShowAccountFriends(Guid accountId)
        //{
        //    var repo = new SpyDuhMembersRepository();
        //    var spyDuhMember = repo.GetById(accountId);
        //    return spyDuhMember.Friends;
        //}

        internal List<string> GetMemberSkills(Guid accountId)
        {
            var singleMember = _spyDuhMembers.FirstOrDefault(member => member.Id == accountId);
            return singleMember.Skills;
        }

        internal List<string> GetMemberServices(Guid accountId)
        {
            var singleMember = _spyDuhMembers.FirstOrDefault(member => member.Id == accountId);
            return singleMember.Services;
        }

        internal void AddSkill(Skill newSkill)
        {
            var db = new SqlConnection(_connectionString);

            var sql = @"insert into Skills(Name, SpyId)
                        output inserted.SkillId
                        values (@Name, @SpyId)";

            var id = db.ExecuteScalar<Guid>(sql, newSkill);
            newSkill.SkillId = id;
        }

        internal List<string> RemoveSkill(Guid accountId, string skill)
        {
            var member = _spyDuhMembers.FirstOrDefault(member => member.Id == accountId);
            member.Skills.Remove(skill);
            return (member.Skills);
        }

        internal void AddService(Service newService)
        {
            var db = new SqlConnection(_connectionString);

            var sql = @"insert into MemberServices(Description, SpyId)
                        output inserted.ServiceId
                        values (@Description, @SpyId)";

            var id = db.ExecuteScalar<Guid>(sql, newService);
            newService.ServiceId = id;
        }

        internal List<string> RemoveService(Guid accountId, string service)
        {
            var member = _spyDuhMembers.FirstOrDefault(member => member.Id == accountId);
            member.Services.Remove(service);
            return (member.Services);
        }
    }
}
