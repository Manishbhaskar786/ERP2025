using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace ERPack.EntityFrameworkCore
{
    public static class ERPackDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<ERPackDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<ERPackDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
