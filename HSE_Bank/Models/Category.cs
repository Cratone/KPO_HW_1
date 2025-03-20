using System;
using CsvHelper.Configuration.Attributes;
using Newtonsoft.Json;
namespace HSE_Bank.Models
{
    public class Category
    {
        [Name("id")]
        public Guid Id { get; init; }
        [Name("name")]
        public string Name { get; init; }
        [Name("type")]
        public OperationType Type { get; init; }

        public Category() {}
        public Category(Guid id, string name, OperationType type)
        {
            Id = id;
            Type = type;
            Name = name;
        }   

        public Category(string name, OperationType type)
        {
            Id = Guid.NewGuid();
            Name = name;
            Type = type;
        }
    }
} 