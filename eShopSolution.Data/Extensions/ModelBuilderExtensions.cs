using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
         public static void Seed(this ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<AppConfig>().HasData(
                    new AppConfig() { Value="This is home page of eShopSolution"},
                    new AppConfig() { Value = "This is keyword of eShopSolution" },
                    new AppConfig() { Value = "This is descroption of eShopSolution" }
                );
        }
    }
}
