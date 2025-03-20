using System;
using CsvHelper.Configuration.Attributes;
using Newtonsoft.Json;
namespace HSE_Bank.Models
{
    public class BankAccount
    {
        [Name("id")]
        public Guid Id { get; init; }
        [Name("name")]
        public string Name { get; init; }
        [Name("balance")]
        public decimal Balance { get; set; }

        public BankAccount() {}
        public BankAccount(Guid id, string name, decimal balance)
        {
            Id = id;
            Name = name;
            Balance = balance;
        }

        public BankAccount(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Balance = 0;
        }
    }
} 