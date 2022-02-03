using AftebiClassroom.Data;
using AftebiClassroom.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoFinal.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context
            , UserManager<AppUser> userManager)
        {
            context.Database.EnsureCreated();

            // Look for any utilizadores.

            if (context.ClassroomUsers.Any())
            {
                return;   // DB has been seeded
            }

            var adminAppUserId = await CriarAdministrador(userManager);
            CriarClassrooms(context, userManager, adminAppUserId);
            var alunos = await CriarAlunos(context, userManager);
            var professores = await CriarProfessores(context, userManager);
            CriarPulicacoes(context, userManager, alunos, professores);
            CriarComentarios(context, userManager, alunos, professores);
        }

        private static async Task<string> CriarAdministrador(UserManager<AppUser> userManager)
        {
            var user = new AppUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
            };

            string adminAppUserId = "";

            var result = await userManager.CreateAsync(user, "Password1!");
            if (result.Succeeded)
            {
                adminAppUserId = user.Id;
            }

            return adminAppUserId;
        }

        private static void CriarClassrooms(ApplicationDbContext context, UserManager<AppUser> userManager, string adminAppUserId)
        {
            var classrooms = new Classroom[]
            {
            new Classroom{ID=1, AppUserID=adminAppUserId, title="Classroom 1", description="Desc classroom 1", time_created=System.DateTime.Now.AddDays(-20)},
            new Classroom{ID=2, AppUserID=adminAppUserId, title="Classroom 2", description="Desc classroom 2", time_created=System.DateTime.Now.AddDays(-20)},
            new Classroom{ID=3, AppUserID=adminAppUserId, title="Classroom 3", description="Desc classroom 3", time_created=System.DateTime.Now.AddDays(-10)},
            new Classroom{ID=4, AppUserID=adminAppUserId, title="Classroom 4", description="Desc classroom 4", time_created=System.DateTime.Now.AddDays(-2)},
            };
            foreach (Classroom s in classrooms)
            {
                context.Classrooms.Add(s);
            }
            context.SaveChanges();
        }

        private static async Task<ClassroomUser[]> CriarAlunos(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            var nomes = new[] { "Rui", "Rodrigo", "Fabio", "Maria", "Mafalda", "Sergio", "Paula", "Vitoria", "Mariana", "Andre" };
            var alunos = new List<ClassroomUser>();
            var classroomId = 1;
            foreach (var nome in nomes)
            {

                var email = nome.ToLower().Replace(" ", ".") + "@gmail.com";
                var user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                };

                
                var result = await userManager.CreateAsync(user, "Password1!");

                if (result.Succeeded)
                {
                    var classroomUser = new ClassroomUser { Role = "Student" };
                    classroomUser.AppUserId = user.Id;
                    classroomUser.ClassroomId = classroomId++;
                    if (classroomId == 4) classroomId = 1; // reset if we reach the last classroom
                    context.ClassroomUsers.Add(classroomUser);

                    alunos.Add(classroomUser);
                }

            }
            context.SaveChanges();

            return alunos.ToArray();
        }

        private static async Task<ClassroomUser[]> CriarProfessores(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            var nomes = new[] { "Sara", "Catarina", "Ricardo", "Jorge", "John" };
            var mentores = new List<ClassroomUser>();
            var classroomId = 1;
            foreach (var nome in nomes)
            {

                var email = nome.ToLower().Replace(" ", ".") + "@gmail.com";
                var user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                };


                var result = await userManager.CreateAsync(user, "Password1!");

                if (result.Succeeded)
                {
                    var classroomUser = new ClassroomUser { Role = "Mentor" };
                    classroomUser.AppUserId = user.Id;
                    classroomUser.ClassroomId = classroomId++;
                    if (classroomId == 4) classroomId = 1; // reset if we reach the last classroom
                    context.ClassroomUsers.Add(classroomUser);

                    mentores.Add(classroomUser);
                }

            }
            context.SaveChanges();

            return mentores.ToArray();
        }

        private static void CriarPulicacoes(ApplicationDbContext context, UserManager<AppUser> userManager, ClassroomUser[] alunos, ClassroomUser[] professores)
        {
            var publicacoes = new BlackBoard[]
            {
            new BlackBoard{content="Publicaçao 1",ClassroomId=1,AppUserId=professores[0].AppUserId},
            new BlackBoard{content="Publicaçao 2",ClassroomId=2,AppUserId=professores[1].AppUserId},
            new BlackBoard{content="Publicaçao 3",ClassroomId=3,AppUserId=professores[2].AppUserId},
            };
            foreach (BlackBoard s in publicacoes)
            {
                context.BlackBoards.Add(s);
            }
            context.SaveChanges();
        }

        private static void CriarComentarios(ApplicationDbContext context, UserManager<AppUser> userManager, ClassroomUser[] alunos, ClassroomUser[] professores)
        {
            var comentario = new Comment[]
            {
            new Comment{Content="Comentário 1, a publicacao 1", BlackBoardId=1,AppUserId=alunos[0].AppUserId},
            new Comment{Content="Comentário 2, a publicacao 1", BlackBoardId=1,AppUserId=professores[0].AppUserId},
            new Comment{Content="Comentário 1, a publicacao 2", BlackBoardId=2,AppUserId=alunos[1].AppUserId},
            new Comment{Content="Comentário 1, a publicacao 3", BlackBoardId=3,AppUserId=alunos[1].AppUserId},
            new Comment{Content="Comentário 2, a publicacao 3", BlackBoardId=3,AppUserId=professores[2].AppUserId},
            new Comment{Content="Comentário 3, a publicacao 3", BlackBoardId=3,AppUserId=alunos[3].AppUserId},
            };
            foreach (Comment s in comentario)
            {
                context.Comments.Add(s);
            }
            context.SaveChanges();
        }

    }
}
