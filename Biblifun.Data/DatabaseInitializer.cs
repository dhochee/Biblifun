// =============================
// Email: info@ebenmonney.com
// www.ebenmonney.com/templates
// =============================

using Biblifun.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblifun.Data.Core;
using Biblifun.Data.Core.Interfaces;

namespace Biblifun.Data
{
    public interface IDatabaseInitializer
    {
        Task SeedAsync();
    }

    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccountManager _accountManager;
        private readonly ILogger _logger;

        public DatabaseInitializer(ApplicationDbContext context, IAccountManager accountManager, ILogger<DatabaseInitializer> logger)
        {
            _accountManager = accountManager;
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync().ConfigureAwait(false);

            if (!await _context.Users.AnyAsync())
            {
                _logger.LogInformation("Generating inbuilt accounts");

                const string adminRoleName = "administrator";
                const string userRoleName = "user";

                await EnsureRoleAsync(adminRoleName, "Default administrator", ApplicationPermissions.GetAllPermissionValues());
                await EnsureRoleAsync(userRoleName, "Default user", new string[] { });

                await CreateUserAsync("admin", "tempP@ss123", "Inbuilt Administrator", "admin@ebenmonney.com", "+1 (123) 000-0000", new string[] { adminRoleName });
                await CreateUserAsync("user", "tempP@ss123", "Inbuilt Standard User", "user@ebenmonney.com", "+1 (123) 000-0001", new string[] { userRoleName });

                _logger.LogInformation("Inbuilt account generation completed");
            }

            if(!await _context.BibleBooks.AnyAsync())
            {
                PopulateBibleMetadata();
            }


            await _context.SaveChangesAsync();

            _logger.LogInformation("Seeding initial data completed");
        }

        private void PopulateBibleMetadata()
        {
            var metadataBuilder = new BibleMetadataBuilder();
            var bibleMetadata = metadataBuilder.LoadMetaDataFromFile();

            foreach(var book in bibleMetadata)
            {
                _context.BibleBooks.Add(new BibleBook
                {
                    BibleBookId = book.BibleBookId,
                    Name = book.Name,
                    TotalChapters = book.TotalChapters
                });

                // save at each step to preserve order ... lame
                _context.SaveChanges();

                foreach(var chapter in book.Chapters)
                {
                    _context.BibleChapter.Add(new BibleChapter
                    {
                        BibleBookId = book.BibleBookId,
                        ChapterNumber = chapter.ChapterNumber,
                        TotalVerses = chapter.TotalVerses
                    });

                    _context.SaveChanges();
                }
            }

            // now add book names for each language so they're grouped together
            var languages = new List<string>() { "en", "es" };

            foreach (var language in languages)
            {
                foreach (var book in bibleMetadata)
                {
                    foreach (var bookNames in book.BookNames
                                                  .Where(bn => bn.Language == language)
                                                  .OrderBy(bn => bn.BibleBookId))
                    {
                        _context.BibleBookNames.Add(new BibleBookName
                        {
                            BibleBookId = bookNames.BibleBookId,
                            Name = bookNames.Name,
                            Language = bookNames.Language,
                            AlternateIdentifiers = bookNames.AlternateIdentifiers
                        });

                        _context.SaveChanges();
                    }
                }
            }
        }

        private void AddBibleBook(int id, string name, string abbreviations, int chapters)
        {
            _context.BibleBooks.Add(new BibleBook
            {
                BibleBookId = id,
                Name = name,
                TotalChapters = chapters
            });
        }

        private async Task EnsureRoleAsync(string roleName, string description, string[] claims)
        {
            if ((await _accountManager.GetRoleByNameAsync(roleName)) == null)
            {
                ApplicationRole applicationRole = new ApplicationRole(roleName, description);

                var result = await this._accountManager.CreateRoleAsync(applicationRole, claims);

                if (!result.Succeeded)
                    throw new Exception($"Seeding \"{description}\" role failed. Errors: {string.Join(Environment.NewLine, result.Errors)}");
            }
        }

        private async Task<ApplicationUser> CreateUserAsync(string userName, string password, string fullName, string email, string phoneNumber, string[] roles)
        {
            ApplicationUser applicationUser = new ApplicationUser
            {
                UserName = userName,
                FullName = fullName,
                Email = email,
                PhoneNumber = phoneNumber,
                EmailConfirmed = true,
                IsEnabled = true
            };

            var result = await _accountManager.CreateUserAsync(applicationUser, roles, password);

            if (!result.Succeeded)
                throw new Exception($"Seeding \"{userName}\" user failed. Errors: {string.Join(Environment.NewLine, result.Errors)}");


            return applicationUser;
        }
    }
}
