using ApiProject.Model.UserModel;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApiProject.Context
{
    public class TokenContext : DbContext
    {
        public TokenContext(DbContextOptions<TokenContext> options) : base(options)
        {

        }

        public virtual DbSet<UserRefreshTokens> UserRefreshToken { get; set; }
    }
}

