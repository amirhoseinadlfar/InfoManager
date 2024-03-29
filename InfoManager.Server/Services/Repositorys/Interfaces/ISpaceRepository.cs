﻿using InfoManager.Server.Models;

namespace InfoManager.Server.Services.Repositorys.Interfaces
{
    public interface ISpaceRepository
    {
        Task AddAsync(Space space);
        Task DeleteAsync(Space space);
        Task<Space?> FindAsync(int id);
        Task LoadTables(Space item2);
    }
}
