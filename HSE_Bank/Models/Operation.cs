using System;
using CsvHelper.Configuration.Attributes;
using Newtonsoft.Json;
namespace HSE_Bank.Models
{
    public class Operation
    {
        [Name("id")]
        public Guid Id { get; init; }
        [Name("bank_account_id")]
        public Guid BankAccountId { get; init; }
        [Name("amount")]
        public decimal Amount { get; init; }
        [Name("time")]
        public DateTime Time { get; init; }
        [Name("category_id")]
        public Guid CategoryId { get; init; }
        [Name("type")]
        public OperationType Type { get; init; }
        [Name("description")]
        public string? Description { get; init; }

        public Operation() {}
        public Operation(Guid id, Guid bank_account_id, Guid category_id, OperationType type, decimal amount, DateTime time, string? description = null) 
        {
            Id = id;
            BankAccountId = bank_account_id;
            Amount = amount;
            CategoryId = category_id;
            Type = type;
            Time = time;
            Description = description;
        }

        public Operation(Guid bankAccountId, Guid categoryId, OperationType type, decimal amount, DateTime time, string? description = null) 
        {
            Id = Guid.NewGuid();
            BankAccountId = bankAccountId;
            Amount = amount;
            CategoryId = categoryId;
            Type = type;
            Time = time;
            Description = description;
        }
    }
} 