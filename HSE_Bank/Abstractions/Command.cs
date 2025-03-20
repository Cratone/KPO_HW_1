using System;
using Microsoft.Extensions.DependencyInjection;
namespace HSE_Bank.Abstractions
{
    public abstract class Command
    {
        public abstract string Execute(ServiceProvider serviceProvider);
    }
} 