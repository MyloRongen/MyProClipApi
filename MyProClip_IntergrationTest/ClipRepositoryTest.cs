using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyProClip_BLL.Enums;
using MyProClip_BLL.Models;
using MyProClip_DAL.Data;
using MyProClip_DAL.Repositories;

namespace MyProClip_IntergrationTest
{
    public class ClipRepositoryTest
    {
        private ApplicationDbContext _context;
        private ClipRepository _clipRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();
            SeedData();

            _clipRepository = new ClipRepository(_context);
        }

        private void SeedData()
        {
            ApplicationUser defaultUser = new()
            {
                Id = "0206A018-5AC6-492D-AB99-10105193D384",
                Email = "test@gmail.com",
                NormalizedEmail = "TEST@GMAIL.COM",
                UserName = "TestUser",
                NormalizedUserName = "TESTUSER",
                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "Password123!"),
                Points = 0,
                IsBanned = false
            };

            _context.Users.Add(defaultUser);
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }


        [Test]
        public async Task GetClipsByUserId_Returns_CorrectClips()
        {
            // Arrange
            string userId = "0206A018-5AC6-492D-AB99-10105193D384";

            List<Clip> expectedClips =
            [
                new Clip
                {
                    UserId = userId,
                    Title = "Clip 1",
                    VideoUrl = "https://www.example.com/clip1",
                    ThumbnailUrl = "https://www.example.com/clip1_thumbnail",
                    Privacy = PrivacyType.Private,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                },
                new Clip
                {
                    UserId = userId,
                    Title = "Clip 2",
                    VideoUrl = "https://www.example.com/clip2",
                    ThumbnailUrl = "https://www.example.com/clip2_thumbnail",
                    Privacy = PrivacyType.Public,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                }
            ];

            await _context.AddRangeAsync(expectedClips);
            await _context.SaveChangesAsync();

            // Act
            List<Clip> result = await _clipRepository.GetClipsByUserId(userId);

            // Assert
            Assert.That(result, Has.Count.EqualTo(expectedClips.Count));
            foreach (Clip expectedClip in expectedClips)
            {
                Assert.That(result.Exists(c => c.Title == expectedClip.Title), Is.True);
            }
        }

        [Test]
        public async Task GetPublicClips_Returns_CorrectPublicClips()
        {
            // Arrange
            List<Clip> publicClips =
            [
                new Clip
                {
                    UserId = "0206A018-5AC6-492D-AB99-10105193D384",
                    Title = "Public Clip 1",
                    VideoUrl = "https://www.example.com/public_clip1",
                    ThumbnailUrl = "https://www.example.com/public_clip1_thumbnail",
                    Privacy = PrivacyType.Public,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                },
                new Clip
                {
                    UserId = "0206A018-5AC6-492D-AB99-10105193D384",
                    Title = "Public Clip 2",
                    VideoUrl = "https://www.example.com/public_clip2",
                    ThumbnailUrl = "https://www.example.com/public_clip2_thumbnail",
                    Privacy = PrivacyType.Public,
                    UpdatedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                }
            ];

            await _context.AddRangeAsync(publicClips);
            await _context.SaveChangesAsync();

            // Act
            List<Clip> result = await _clipRepository.GetPublicClips();

            // Assert
            Assert.That(result, Has.Count.EqualTo(publicClips.Count));
            foreach (Clip publicClip in publicClips)
            {
                Assert.That(result.Exists(c => c.Title == publicClip.Title), Is.True);
            }
        }

        [Test]
        public void AddClip_SavesClipToDatabase()
        {
            // Arrange
            Clip newClip = new()
            {
                UserId = "0206A018-5AC6-492D-AB99-10105193D384",
                Title = "New Clip",
                VideoUrl = "https://www.example.com/new_clip",
                ThumbnailUrl = "https://www.example.com/new_clip_thumbnail",
                Privacy = PrivacyType.Public,
                UpdatedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            // Act
            _clipRepository.AddClip(newClip);

            // Assert
            Clip? savedClip = _context.Clips.FirstOrDefault(c => c.Title == newClip.Title);
            Assert.That(savedClip, Is.Not.Null);
        }

        [Test]
        public async Task GetClipById_Returns_Clip_WhenExists()
        {
            // Arrange
            Clip newClip = new()
            {
                UserId = "0206A018-5AC6-492D-AB99-10105193D384",
                Title = "Existing Clip",
                VideoUrl = "https://www.example.com/existing_clip",
                ThumbnailUrl = "https://www.example.com/existing_clip_thumbnail",
                Privacy = PrivacyType.Public,
                UpdatedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Clips.AddAsync(newClip);
            await _context.SaveChangesAsync();

            // Act
            Clip? retrievedClip = await _clipRepository.GetClipById(newClip.Id);

            // Assert
            Assert.That(retrievedClip, Is.Not.Null);
            Assert.That(retrievedClip.Title, Is.EqualTo(newClip.Title));
        }

        [Test]
        public async Task GetClipById_Returns_Null_WhenNotExists()
        {
            // Act
            Clip? retrievedClip = await _clipRepository.GetClipById(-1);

            // Assert
            Assert.That(retrievedClip, Is.Null);
        }

        [Test]
        public async Task DeleteClipAsync_DeletesClipFromDatabase()
        {
            // Arrange
            Clip clipToDelete = new()
            {
                UserId = "0206A018-5AC6-492D-AB99-10105193D384",
                Title = "Clip to Delete",
                VideoUrl = "https://www.example.com/clip_to_delete",
                ThumbnailUrl = "https://www.example.com/clip_to_delete_thumbnail",
                Privacy = PrivacyType.Public,
                UpdatedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Clips.AddAsync(clipToDelete);
            await _context.SaveChangesAsync();

            // Act
            await _clipRepository.DeleteClipAsync(clipToDelete);

            // Assert
            Clip? deletedClip = await _context.Clips.FindAsync(clipToDelete.Id);
            Assert.That(deletedClip, Is.Null);
        }
    }
}