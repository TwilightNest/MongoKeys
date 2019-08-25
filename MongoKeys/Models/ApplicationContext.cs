using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using MySql.Data.Entity;

namespace MongoKeys.Models
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("MongoKeysContext")
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u._Games)
                .WithMany(g => g._Users)
                .Map(t =>
                {
                    t.MapLeftKey("_UserId");
                    t.MapRightKey("_GameId");
                    t.ToTable("UsersGames");
                });
        }
    }

    [Table("Games")]
    public class Game
    {
        public Game()
        {
            _Users = new List<User>();
        }

        [Key] [Required] public int _Id { get; set; }
        [Required] public int _SteamId { get; set; }
        [Required] public string _Review { get; set; }
        [Required] public string _Name { get; set; }
        [Required] public string _ShortDescription { get; set; }
        [Required] public string _FullDescription { get; set; }
        [Required] public float _Price { get; set; }
        [Required] public DateTime _DateOfRelease { get; set; }
        [Required] public float _Sale { get; set; }
        [Required] public string _Genre { get; set; }
        public virtual List<User> _Users { get; set; }
    }

    [Table("Users")]
    public class User
    {
        public User()
        {
            _Games = new List<Game>();
        }

        [Key] public int _Id { get; set; }
        [Required] public bool _IsAdmin { get; set; }
        [Required] public string _Login { get; set; }
        [Required] public string _Email { get; set; }
        [Required] public string _Password { get; set; }
        public virtual List<Game> _Games { get; set; }
    }

    public class LoginModel
    {
        [Required] public string Login { get; set; }
        [Required] public string Password { get; set; }
    }

    public class RegisterModel
    {
        [Required] public int Id { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string Login { get; set; }
        [Required] public string Password { get; set; }
        [Required] public bool IsAdmin { get; set; }
        [Required] public string ImagePath { get; set; }
    }

    public class BuyModel
    {
        [Required] public int GameId { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string UserName { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string Address { get; set; }
        [Required] public string Country { get; set; }
        [Required] public string ZipCode { get; set; }
        [Required] public string OwnerName { get; set; }
        [Required] public string CardNumber { get; set; }
        [Required] public DateTime CardExpiration { get; set; }
        [Required] public int CardCvv { get; set; }
    }

    public class GameModel
    {
        [Required] public int Id { get; set; }
        [Required] public int SteamId { get; set; }
        [Required] public string Review { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string ShortDescription { get; set; }
        [Required] public string FullDescription { get; set; }
        [Required] public float Price { get; set; }
        [Required] public DateTime DateOfRelease { get; set; }
        [Required] public float Sale { get; set; }
        [Required] public string Genre { get; set; }
        [Required] public string ImagePath { get; set; }
    }
}