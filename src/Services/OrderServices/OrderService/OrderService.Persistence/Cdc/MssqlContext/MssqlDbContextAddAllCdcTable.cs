﻿using Microsoft.EntityFrameworkCore;
using OrderService.Persistence.Context;

namespace OrderService.Persistence.Cdc.MssqlContext
{
    public static class MssqlDbContextAddAllCdcTable
    {
        public static void AddAllCdc(CoreDbContext ctx)
        {
            ctx.Database.ExecuteSqlRaw("EXEC sys.sp_cdc_enable_table @source_schema = N'dbo', @source_name = N'Outbox', @role_name = NULL;");
        }
    }
}
