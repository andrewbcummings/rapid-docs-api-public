using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using rapid_docs_core.Authentication;
using rapid_docs_models.DataAccess;
using rapid_docs_models.DbModels;
using rapid_docs_viewmodels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rapid_docs_services.Services.Account
{
    public class AccountService : BaseService, IAccountService
    {
        public AccountService(VidaDocsDbContext dbContext, IMapper mapper, VidaDocsContext ctx) : base(dbContext, mapper, ctx)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.ctx = ctx;
        }

        public async Task<int> UserDetails(UserViewModel user)
        {
            var existingUser = this.dbContext.Users.AsNoTracking().FirstOrDefault(x => x.NormalizedEmail == user.Email.ToUpper());
            var model = this.mapper.Map<User>(user);
            if (existingUser != null)
                model.Id = existingUser.Id;
            model.NormalizedUserName = model.UserName.ToUpper();
            model.NormalizedEmail = model.Email.ToUpper();
            model.SecurityStamp = Guid.NewGuid().ToString();
            this.dbContext.Users.Update(model);
            return await this.dbContext.SaveChangesAsync();
        }
    }
}
