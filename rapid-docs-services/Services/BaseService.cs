using AutoMapper;
using Microsoft.EntityFrameworkCore;
using rapid_docs_core.Authentication;
using rapid_docs_models.DataAccess;

namespace rapid_docs_services.Services
{
    public class BaseService
    {
        protected VidaDocsDbContext dbContext;
        protected VidaDocsContext ctx;
        protected IMapper mapper;

        public BaseService(VidaDocsDbContext dbContext, IMapper mapper, VidaDocsContext ctx)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.ctx = ctx;

            if (this.ctx != null && !string.IsNullOrEmpty(this.ctx.Email))
            {
                var user = this.dbContext.Users.AsNoTracking().FirstOrDefault(x => x.Email == this.ctx.Email);
                if (user != null)
                {
                    this.ctx.UserId = user.Id;
                }
            }
        }
    }
}
