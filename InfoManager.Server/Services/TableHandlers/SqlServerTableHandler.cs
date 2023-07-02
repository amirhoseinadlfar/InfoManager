using InfoManager.Server.Models;

using Microsoft.EntityFrameworkCore;

using OneOf.Types;

using System.Data.Common;
using System.Text;

using static InfoManager.Server.Ulitis.DbContextExtension;

namespace InfoManager.Server.Services.TableHandlers;

public class SqlServerTableHandler : ITableHandler
{
    private readonly DbContext dbContext;

    public SqlServerTableHandler(DbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    async Task ExcCommand(string command)
    {
        using DbConnection connection = dbContext.Database.GetDbConnection();
        using DbCommand cmd = connection.CreateCommand();
        cmd.CommandText = command;
        await connection.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
        await connection.CloseAsync();
    }
    public Task CreateTableAsync(Table table)
    {
        return ExcCommand($""""
                CREATE TABLE [dbo].[{GetTblName(table)}](
                    Id int not null IDENTITY(1,1) PRIMARY KEY,
                    )
                """");
    }
    public Task AddFieldAsync(Table table,TableField tableField)
    {
        return ExcCommand($"""
                ALTER TABLE {GetTblName(table)}
                ADD {GetFieldName(tableField)} {GetSqlType(tableField.FieldType,tableField.Limit)};
                """);
    }

    static string? GetSqlType(FieldType fieldType, int limit)
    {
        return fieldType switch
        {
            FieldType.ShortString => $"nchar({limit})",
            FieldType.LongString => $"nvarchar({limit})",
            FieldType.Enum => "tinyint",
            FieldType.Bool => "bit",
            FieldType.Byte => "tinyint",
            FieldType.Short => "smallint",
            FieldType.Int => "int",
            FieldType.Long => "bigint",
            FieldType.Float => "real",
            FieldType.Double => "float",
            FieldType.Decimal => "decimal",
            FieldType.Date => "date",
            FieldType.Time => "time",
            FieldType.DateTime => "datetime",
            _ => null
        };
    }
    public Task ChangeFieldTypeAsync(Table table, TableField field)
    {
        return ExcCommand($"""
                ALTER TABLE {GetTblName(table)}
                ALTER COLUMN {GetFieldName(field)} {GetSqlType(field.FieldType, field.Limit)};
                """);
    }
    public Task DropFieldAsync(Table table,TableField field)
    {
        return ExcCommand($"""
                ALTER TABLE {GetTblName(table)}
                DROP COLUMN {GetFieldName(field)};
                """);
    }
    public Task DropTableAsync(Table table)
    {
        return ExcCommand($"""
                DROP TABLE {GetTblName(table)};
                """);
    }
    object ConvertValues(FieldType fieldType,object value)
    {
        return fieldType switch
        {
            FieldType.ShortString => $"'{value.ToString().Replace(@"'", @"''")}'",
            FieldType.LongString => $"'{value.ToString().Replace(@"'", @"''")}'",
            FieldType.Enum => Convert.ToByte(value),
            FieldType.Bool => Convert.ToBoolean(value) ? 1 : 0,
            FieldType.Byte => Convert.ToByte(value),
            FieldType.Short => Convert.ToInt16(value),
            FieldType.Int => Convert.ToInt32(value),
            FieldType.Long => Convert.ToInt64(value),
            FieldType.Float => Convert.ToSingle(value),
            FieldType.Double => Convert.ToDouble(value),
            FieldType.Decimal => Convert.ToDecimal(value),
            FieldType.Date => Convert.ToDateTime(value).ToString("YYYY-MM-DD"),
            FieldType.Time => Convert.ToDateTime(value).ToString("hh:mm:ss"),
            FieldType.DateTime => Convert.ToDateTime(value).ToString("YYYY-MM-DDThh:mm:ss"),
        };
    }
    public async Task<int> AddRecordAsync(Table table, Dictionary<TableField,object> cells)
    {
        using DbConnection connection = dbContext.Database.GetDbConnection();
        using DbCommand cmd = connection.CreateCommand();

        List<object> values = new List<object>();
        List<string> fields = new List<string>();

        foreach (var item in cells)
        {
            fields.Add(GetFieldName(item.Key));
            values.Add(ConvertValues(item.Key.FieldType,item.Value));
        }

        cmd.CommandText = $"""
            insert into {GetTblName(table)} ({string.Join(',', fields)})values({string.Join(',', values.Select(x=>$"'{x}'"))});
            SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY];
            """;
        await connection.OpenAsync();
        int result = (int)await cmd.ExecuteScalarAsync();
        await connection.CloseAsync();
        return result;
    }
    public Task EditRecordAsync(Table table,int id,Dictionary<TableField,object> cells)
    {
        List<string> sets = new List<string>();
        foreach(var item in cells)
        {
            sets.Add($"{GetFieldName(item.Key)} = '{ConvertValues(item.Key.FieldType,item.Value)}'");
        }

        return ExcCommand($"""
        update {GetTblName(table)} set {string.Join(',',sets)} where Id = {id};
        """);
    }
    public Task DeleteRecordAsync(Table table,int id)
    {
        return ExcCommand($"""
            delete from {GetTblName(table)} where Id = {id}
            """);
    }
}
