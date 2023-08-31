using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using System.Linq;

namespace NZWalks.API.Repositories
{
    public class SqlWalkRepository : IWalkRepository
    {
        private readonly NzWalksDbContext _dbContext;

        public SqlWalkRepository(NzWalksDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
           await _dbContext.Walks.AddAsync(walk);
           await _dbContext.SaveChangesAsync();
           return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
           var existingWalk= await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

           if (existingWalk == null)
           {
               return null;
           }

           _dbContext.Walks.Remove(existingWalk);
           await _dbContext.SaveChangesAsync();
            return existingWalk;
        }

      

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNo = 1, int? pageSize = 1000)
        {
            var walks = _dbContext.Walks.Include("Region").Include("Difficulty").AsQueryable();

            // filtering 
            if (string.IsNullOrWhiteSpace(filterOn) == false || string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn != null && filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => filterQuery != null && x.Name.Contains(filterQuery));
                }
            }

            // sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }

            }

            // pagination
            var skipResults = (pageNo - 1) * pageSize;
            return await walks.Skip((int)skipResults).Take((int)pageSize).ToListAsync();
            // return await _dbContext.Walks.Include("Region").Include("Difficulty").ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
          return await _dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x=>x.Id ==id);
        }

        public async Task<Walk?> UpdateAsync(Guid id,Walk walk)
        {
            var existingWalk = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalk == null)
            {
                return null;
            }

            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKm=walk.LengthInKm;
            existingWalk.WalkImageUrl=walk.WalkImageUrl;
            existingWalk.DifficultyId=walk.DifficultyId;
            existingWalk.RegionId=walk.RegionId;

            await _dbContext.SaveChangesAsync();

            return existingWalk;
        }

        //public Task<Walk> UpdateAsync(Walk walk)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
