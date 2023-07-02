using InfoManager.Server.Models;

namespace InfoManager.Server.Services.TableHandlers
{
    public interface ITableHandler
    {
        

        public Task CreateTableAsync(Table table);
        public Task AddFieldAsync(Table table, TableField field);
        public Task ChangeFieldTypeAsync(Table table,TableField field);
        public Task DropFieldAsync(Table table, TableField field);
        public Task DropTableAsync(Table table);

        public Task<int> AddRecordAsync(Table table,Dictionary<TableField, object> cells);
        public Task EditRecordAsync(Table table,int id, Dictionary<TableField, object> cells);
        public Task DeleteRecordAsync(Table table,int id);
        public Task<object[][]> GetRecordsAsync(Table table, int offset, int limit, Dictionary<TableField, object> filters, TableField[] selectedFields);
        
    }
}
