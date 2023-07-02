using InfoManager.Server.DbContexts;
using InfoManager.Server.Models;
using InfoManager.Server.Services.Repositorys.Interfaces;

using Microsoft.EntityFrameworkCore;

using System.Data.Common;

namespace InfoManager.Server.Services.Repositorys;

public class TableRepository : ITableRepository
{
    // TODO : add my sql and sqlite support
    private readonly MainDbContext dbContext;

    public TableRepository(MainDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddTable(Table table)
    {
        await dbContext.Tables.AddAsync(table);
    }

    public async Task AddField(TableField tableField)
    {
        await dbContext.TableFields.AddAsync(tableField);
    }
}
