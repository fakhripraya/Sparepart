using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Sparepart.Models;

[assembly: OwinStartupAttribute(typeof(Sparepart.Startup))]
namespace Sparepart
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {
                
                // first we create Admin role   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create an Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "Admin";
                user.Email = "Admin@sparepart.com";

                string userPWD = "AdminSD5";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
                dbsparepartEntities Dbcontext = new dbsparepartEntities();
                masteruser ADM = new masteruser();
                ADM.UserID = user.Id;
                ADM.NamaUser = "Admin";
                ADM.Username = "Admin";
                ADM.Password = userPWD;
                ADM.Email = user.Email;
                ADM.IsDelete = 1;
                Dbcontext.masterusers.Add(ADM);
                Dbcontext.SaveChanges();
            }

            //// creating Creating Manager role    
            //if (!roleManager.RoleExists("Manager"))
            //{
            //    var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
            //    role.Name = "Manager";
            //    roleManager.Create(role);

            //}

            //// creating Creating Employee role    
            //if (!roleManager.RoleExists("Employee"))
            //{
            //    var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
            //    role.Name = "Employee";
            //    roleManager.Create(role);

            //}
        }
    }
}
