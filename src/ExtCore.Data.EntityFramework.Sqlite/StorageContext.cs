﻿// Copyright © 2015 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ExtCore.Data.Abstractions;
using ExtCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ExtCore.Data.EntityFramework.Sqlite
{
  /// <summary>
  /// Implements the <see cref="IStorageContext">IStorageContext</see> interface and represents SQLite database
  /// with the Entity Framework Core as the ORM.
  /// </summary>
  public class StorageContext : DbContext, IStorageContext
  {
    private string connectionString { get; set; }
    private string migrationsAssembly { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StorageContext">StorageContext</see> class.
    /// </summary>
    /// <param name="connectionString">The connection string that is used to connect to the SQLite database.</param>
    public StorageContext(string connectionString, string migrationsAssembly)
    {
      this.connectionString = connectionString;
      this.migrationsAssembly = migrationsAssembly;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      base.OnConfiguring(optionsBuilder);
      optionsBuilder.UseSqlite(this.connectionString b => b.MigrationsAssembly(migrationsAssembly));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      foreach (IModelRegistrar modelRegistrar in ExtensionManager.GetInstances<IModelRegistrar>(a => a.FullName.ToLower().Contains("entityframework.sqlite")))
        modelRegistrar.RegisterModels(modelBuilder);
    }
  }
}
