using System.Data;
using Dapper;

namespace Selah.Infrastructure.Repository;

public class BaseRepository: IBaseRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public BaseRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<T> AddAsync<T>(string sql, object parameters)
    {
        using (IDbConnection connection = await _dbConnectionFactory.CreateConnectionAsync())
        {
            return await connection.ExecuteScalarAsync<T>(sql, parameters);
        }
    }

    public async Task<int> AddManyAsync<T>(string sql, IReadOnlyCollection<DynamicParameters> objectsToSave)
    {
        int rowsInserted = 0;
        using (IDbConnection connection = await _dbConnectionFactory.CreateConnectionAsync())
        {
            try
            {
                foreach (DynamicParameters obj in objectsToSave)
                {
                    await connection.ExecuteScalarAsync(sql, obj);
                    rowsInserted++;
                }

                return rowsInserted;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }

    public async Task<bool> DeleteAsync(string sql, object parameters)
    {
        using (IDbConnection connection = await _dbConnectionFactory.CreateConnectionAsync())
        {
            int recordsDeleted = await connection.ExecuteAsync(sql, parameters);
            return recordsDeleted > 0;
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync<T>(string sql, object parameters)
    {
        using (IDbConnection connection = await _dbConnectionFactory.CreateConnectionAsync())
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            return await connection.QueryAsync<T>(sql, parameters);
        }
    }

    public async Task<T> GetFirstOrDefaultAsync<T>(string sql, object parameters)
    {
        using (IDbConnection connection = await _dbConnectionFactory.CreateConnectionAsync())
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            return (await connection.QueryAsync<T>(sql, parameters)).FirstOrDefault();
        }
    }

    public async Task<bool> UpdateAsync(string sql, object parameters)
    {
        using (IDbConnection connection = await _dbConnectionFactory.CreateConnectionAsync())
        {
            var rowsUpdated = await connection.ExecuteAsync(sql, parameters);
            return rowsUpdated > 0;
        }
    }

    public async Task PerformTransaction<T>(List<(string, object)> transactions)
    {
        using (IDbConnection connection = await _dbConnectionFactory.CreateConnectionAsync())
        {
            using (var dbTransaction = connection.BeginTransaction())
            {
                foreach (var transaction in transactions)
                {
                    try
                    {
                        connection.Execute(transaction.Item1, transaction.Item2);
                        dbTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                    }
                }
            }
        }
    }
}