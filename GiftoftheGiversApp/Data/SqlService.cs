using GiftoftheGiversApp.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace GiftoftheGiversApp.Data
{
   
        public class SqlService
        {
            private readonly IConfiguration _configuration;
            private readonly string _connectionString;

            public SqlService(IConfiguration configuration)
            {
                _configuration = configuration;
                _connectionString = _configuration.GetConnectionString("Default");

            }

            public SqlConnection GetConnection()
            {
                return new SqlConnection(_connectionString);+
            }


        }
    }



