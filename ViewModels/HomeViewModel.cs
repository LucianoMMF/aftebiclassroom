using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AftebiClassroom.Models;
namespace AftebiClassroom.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Invite> Invites { get; set; }
        public IEnumerable<ClassroomUser> UserClassrooms { get; set; }
    }
}
