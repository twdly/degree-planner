using DegreePlanner.Data;
using DegreePlanner.Services;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DegreePlanner.Tests;

[TestFixture]
public class UserServiceTest
{
	private DatabaseContext databaseContext;
	private UserService userService;
	private DatabaseResetService databaseResetService;

	[SetUp]
	public void SetUp()
	{
		var connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("SQLDBConnection");
		var dbOptions = new DbContextOptionsBuilder<DatabaseContext>().UseSqlServer(connectionString);
			
		databaseContext = new DatabaseContext(dbOptions.Options);
		userService = new UserService(databaseContext);
		databaseResetService = new DatabaseResetService(databaseContext);

		databaseResetService.ResetDatabase();
	}

	[Test]
	public void TestGetUserFromId()
	{
		var user = userService.GetUserFromId(10004);
		Assert.That(user.Id, Is.EqualTo(10004));
		Assert.That(user.Name, Is.EqualTo("Tai"));
	}

	[Test]
	public void TestGetUserMajor()
	{
		var major = userService.GetUserMajor(10004);
		Assert.That(major.Name, Is.EqualTo("Enterprise Software Development"));
	}

	[Test]
	public void TestGetUserDegree()
	{
		var degree = userService.GetDegreeForUser(10004);
		Assert.That(degree.Name, Is.EqualTo("Bachelor of Information Technology"));
	}

	[TearDown]
	public void TearDown()
	{
		databaseContext.DisposeAsync();
	}
}