using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminIdentityService.Persistence.Constant
{
    internal static class PersistenceConstant
    {
        public static string ExceptionMessageTemplate => "Hata mesajı :";
        public static string MigratedDbContext => "Veritabanı yüklendi";
        public static string MigrationError => "Veritabanı yüklenirken hata";
    }
}
