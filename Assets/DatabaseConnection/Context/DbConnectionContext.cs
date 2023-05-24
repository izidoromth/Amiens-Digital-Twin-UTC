using Microsoft.EntityFrameworkCore;
using System;

namespace DatabaseConnection.Context
{
    public static class DbConnectionContext
    {
        public static T GetContext<T>
        (
            Func<DbContextOptions<T>, string, T> func,
            string userID,
            string password,
            string database,
            string host = "localhost",
            string port = "5432",
            string defaultSchema = "public"
        ) where T : DbContext
        {
            string connectionString = $"User ID={userID};Password={password};Host={host};Port={port};Database={database}";
            var options = new DbContextOptionsBuilder<T>();
            options.UseNpgsql(connectionString);
            return func(options.Options, defaultSchema);
        }
    }
}
