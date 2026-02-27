using FluentResults;
using Mercury.Modules.BookKeeping.Domain.Transactions;
using Mercury.Modules.BookKeeping.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

IConfiguration configuration = new ConfigurationBuilder()
	.SetBasePath(AppContext.BaseDirectory)
	.AddJsonFile("appsettings.json", optional: true)
	.AddEnvironmentVariables()
	.Build();

string? connectionString = configuration["Databases:BookKeeping"];

if (string.IsNullOrWhiteSpace(connectionString) || connectionString == "[ENV]")
	throw new InvalidOperationException("Missing database connection string. Configure Databases:BookKeeping.");

DbContextOptions<BookKeepingDbContext> options = new DbContextOptionsBuilder<BookKeepingDbContext>()
	.UseNpgsql(connectionString)
	.Options;

using BookKeepingDbContext dbContext = new(options);

Console.WriteLine("Applying migrations...");
dbContext.Database.Migrate();

if (dbContext.Transactions.Any())
{
	Console.WriteLine("Seed skipped: transactions already exist.");
	return;
}

DateTime now = DateTime.UtcNow;

List<(DateOnly Date, string Description, decimal Amount, TransactionType Type)> seedData =
[
	(DateOnly.FromDateTime(now.AddDays(-20)), "Salary", 3500.00m, TransactionType.Income),
	(DateOnly.FromDateTime(now.AddDays(-19)), "Rent", 1150.00m, TransactionType.Expense),
	(DateOnly.FromDateTime(now.AddDays(-18)), "Groceries", 132.45m, TransactionType.Expense),
	(DateOnly.FromDateTime(now.AddDays(-17)), "Freelance", 850.00m, TransactionType.Income),
	(DateOnly.FromDateTime(now.AddDays(-16)), "Internet", 59.99m, TransactionType.Expense),
	(DateOnly.FromDateTime(now.AddDays(-15)), "Electricity", 96.20m, TransactionType.Expense),
	(DateOnly.FromDateTime(now.AddDays(-14)), "Transport", 42.30m, TransactionType.Expense),
	(DateOnly.FromDateTime(now.AddDays(-13)), "Dining", 74.90m, TransactionType.Expense),
	(DateOnly.FromDateTime(now.AddDays(-12)), "Dividend", 120.50m, TransactionType.Income),
	(DateOnly.FromDateTime(now.AddDays(-11)), "Phone", 24.99m, TransactionType.Expense)
];

List<Transaction> transactions = [];

for (int i = 0; i < seedData.Count; i++)
{
	(DateOnly date, string description, decimal amount, TransactionType type) = seedData[i];

	Result<Transaction> creationResult = Transaction.Create(date, description, amount, type);

	if (creationResult.IsFailed)
	{
		string message = string.Join("; ", creationResult.Errors.Select(e => e.Message));
		throw new InvalidOperationException($"Failed creating seed transaction '{description}': {message}");
	}

	Transaction transaction = creationResult.Value;
	transaction.CreatedAt = now.AddMinutes(-seedData.Count + i);
	transaction.UpdatedAt = transaction.CreatedAt;

	transactions.Add(transaction);
}

dbContext.Transactions.AddRange(transactions);
dbContext.SaveChanges();

Console.WriteLine($"Seed complete: inserted {transactions.Count} transactions.");