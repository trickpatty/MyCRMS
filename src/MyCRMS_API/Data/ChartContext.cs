using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyCRMS_API.Models;

namespace MyCRMS_API.Data
{
    public class ChartContext : DbContext
    {
        public ChartContext(DbContextOptions<ChartContext> options) : base(options)
        {
        }

        public DbSet<Chart> Charts { get; set; }
    }
}
