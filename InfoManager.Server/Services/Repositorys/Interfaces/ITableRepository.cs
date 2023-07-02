using InfoManager.Server.Models;

namespace InfoManager.Server.Services.Repositorys.Interfaces
{
    public interface ITableRepository
    {
        Task AddTable(Table table);
        Task AddField(TableField field);
    }
}
