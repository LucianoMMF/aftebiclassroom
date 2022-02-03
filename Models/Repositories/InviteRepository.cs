using AftebiClassroom.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AftebiClassroom.Models.Repositories
{
    public interface IInviteRepository
    {
        Invite Get(int inviteId);
        Invite Add(Invite invite);
        IEnumerable<Invite> GetAllInvites(int ClassroomId);
        IEnumerable<Invite> GetUserInvites(string email);
        Invite Delete(int Classid, string email);
        Invite Delete(int Id);
    }
    public class SQLInviteRepository : IInviteRepository
    {
        private readonly ApplicationDbContext context;
        public SQLInviteRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        Invite IInviteRepository.Get(int inviteId)
        {
            return context.Invites.FirstOrDefault(i => i.Id == inviteId);
        }
        Invite IInviteRepository.Add(Invite invite)
        {
            context.Invites.Add(invite);
            context.SaveChanges();
            return invite;
        }
        IEnumerable<Invite> IInviteRepository.GetAllInvites(int ClassroomId)
        {
            return context.Invites.Where(i => i.ClassroomId == ClassroomId).Include(c => c.Classroom);
        }
        IEnumerable<Invite> IInviteRepository.GetUserInvites(string email)
        {
            return context.Invites.Where(invite => invite.Email == email).Include(c => c.Classroom);
        }

        Invite IInviteRepository.Delete(int Classid, string email)
        {
            Invite invite = context.Invites.Where(i => i.Email == email && i.ClassroomId == Classid).FirstOrDefault();
            if (invite != null)
            {
                context.Invites.Remove(invite);
                context.SaveChanges();
            }
            return invite;
        }
        Invite IInviteRepository.Delete(int inviteId)
        {
            Invite invite = context.Invites.Where(i => i.Id == inviteId).FirstOrDefault();
            if (invite != null)
            {
                context.Invites.Remove(invite);
                context.SaveChanges();
            }
            return invite;
        }
    }
}
