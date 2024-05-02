using STemplate.Persistence.Context;
using Microsoft.EntityFrameworkCore;
namespace STemplate.Persistence.Cdc.MssqlContext;

public static class MssqlDbContextAddAllCdcTable
{
    public static void AddAllCdc(CoreDbContext ctx)
    {
        ctx.Database.ExecuteSqlRaw("EXEC sys.sp_cdc_enable_table @source_schema = N'dbo', @source_name = N'Outbox', @role_name = NULL;");
        ctx.Database.ExecuteSqlRaw("EXEC sp_configure 'show advanced options', 1; RECONFIGURE; EXEC sp_configure 'max text repl size', -1; RECONFIGURE;");
    }
}
