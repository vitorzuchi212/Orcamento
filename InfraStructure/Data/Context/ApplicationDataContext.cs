﻿using Microsoft.EntityFrameworkCore;
using Orcamento.Models; 

namespace Orcamento.InfraStructure.Data.Context
{
    public class ApplicationDataContext : DbContext
    {
        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : base(options) 
        {
        
        } 
        public DbSet<OrcamentoP> OrcamentoP { get; set; }
    }
}