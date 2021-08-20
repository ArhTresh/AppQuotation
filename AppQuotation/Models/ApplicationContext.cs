using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace AppQuotation.Models
{
    public class ApplicationContext : DbContext
    {
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            string json = System.IO.File.ReadAllText("appsettings.json");
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            string connection = jsonObj["ConnectionStrings"]["DefaultConnection"];

            optionsBuilder.UseSqlServer(connection);
        }


        public DbSet<Quotation> Quotations { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Source> Sources { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }



        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public ApplicationContext() : base()
        {
                
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string userRoleName = "user";

            string adminEmail = "admin@mail.ru";
            string adminPassword = "admin";

            string userEmail = "admin@mail.ru";
            string userPassword = "user";

            // добавляем роли
            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role userRole = new Role { Id = 2, Name = userRoleName };
            User adminUser = new User { Id = 1, Email = adminEmail, Password = adminPassword, RoleId = adminRole.Id };
            User userUser = new User { Id = 2, Email = userEmail, Password = userPassword, RoleId = userRole.Id };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser, userUser });

            string companyNameSber = "Sberbank";
            string companyTickerSber = "SBER";
            string companyNameYndx = "Yandex";
            string companyTickerYndx = "YNDX";
            string companyNameAapl = "Apple";
            string companyTickerAapl = "AAPL";

            Company sberCompany = new Company { Id = 1, Name = companyNameSber, Ticker = companyTickerSber };
            Company yndxCompany = new Company { Id = 2, Name = companyNameYndx, Ticker = companyTickerYndx };
            Company aaplCompany = new Company { Id = 3, Name = companyNameAapl, Ticker = companyTickerAapl };

            modelBuilder.Entity<Company>().HasData(new Company[] { sberCompany, yndxCompany, aaplCompany });


            string sourceNameFinnhub = "Finnhub";
            string sourceUrlFinnhub = "https://finnhub.io/api/v1/quote?token=sandbox_c1pugf2ad3icph06sba0&symbol=";
            string sourceNameMOEX = "MOEX";
            string sourceUrlMOEX = "https://iss.moex.com/iss/engines/stock/markets/shares/boards/TQBR/securities.xml?iss.meta=off&iss.only=securities&securities.columns=SECID,PREVADMITTEDQUOTE";

            Source finnhubSource = new Source { Id = 1, Name = sourceNameFinnhub, BaseApiUrl = sourceUrlFinnhub };
            Source moexSource = new Source { Id = 2, Name = sourceNameMOEX, BaseApiUrl = sourceUrlMOEX };

            modelBuilder.Entity<Source>().HasData(new Source[] { finnhubSource, moexSource });



            base.OnModelCreating(modelBuilder);
        }

    }
}
