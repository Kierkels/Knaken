using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Kierkels.Knaken.Domain.Entities;
using Kierkels.Knaken.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Kierkels.Knaken.Infrastructure.Seeders;

public static class TransactionSeeder
    {
        public static async Task SeedTransactionsAsync(ApplicationDbContext context, string csvFilePath)
        {
            // Check if the Transactions table is empty
            if (await context.Transactions.AnyAsync())
                return; // Exit if data already exists

            // Read the CSV file
            var transactions = ParseCsvFile(csvFilePath);

            // Add the transactions to the database
            await context.AddRangeAsync(transactions);
            await context.SaveChangesAsync();

            Console.WriteLine($"Seeded {transactions.Count} transactions into the database.");
        }

        private static List<TransactionEntity> ParseCsvFile(string csvFilePath)
        {
            var transactions = new List<TransactionEntity>();

            // Configure CSV parsing options
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";", // CSV delimiter based on the example provided
                HasHeaderRecord = true, // The file has headers
                TrimOptions = TrimOptions.Trim,
            };

            using var reader = new StreamReader(csvFilePath);
            using var csv = new CsvReader(reader, csvConfig);

            // Skip headers and parse rows
            var records = csv.GetRecords<dynamic>();

            foreach (var record in records)
            {
                transactions.Add(new TransactionEntity
                {
                    Date = DateTime.ParseExact(record.Datum, "yyyyMMdd", CultureInfo.InvariantCulture),
                    Description = record.NaamOmschrijving,
                    Account = record.Rekening,
                    CounterAccount = record.Tegenrekening,
                    Code = record.Code,
                    DebitCredit = record.AfBij,
                    Amount = decimal.Parse(record.BedragEUR.Replace(",", "."), CultureInfo.InvariantCulture),
                    MutationType = record.Mutatiesoort,
                    Remarks = record.Mededelingen,
                    BalanceAfterMutation = decimal.Parse(record.Saldonamutatie.Replace(",", "."), CultureInfo.InvariantCulture),
                    Tag = record.Tag
                });
            }

            return transactions;
        }
    }
