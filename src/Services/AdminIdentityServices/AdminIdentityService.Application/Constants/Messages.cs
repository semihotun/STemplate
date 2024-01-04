namespace AdminIdentityService.Application.Constants
{
    /// <summary>
    /// Maybe there will language support for messages in the future.
    /// </summary>
    public static class Messages
    {
        public static string Added => "Eklendi";
        public static string Deleted => "Silindi";
        public static string Updated => "Güncellendi";
        public static string ClaimExists => "İlgili Yetki Yok";
        public static string UserNotLogged => "Kullanıcı Giriş yapmadı";
        public static string AuthorizationDenied => "Giriş Reddedildi";
        public static string UserNotFound => "Bu Maile Sahip Kullanıcı Yok";
        public static string PasswordError => "Şifre Yanlış";
        public static string NameAlreadyExist => "İsim Kullanılıyor";
        public static string PasswordEmpty => "Şifre Boştu";
        public static string PasswordSpecialCharacter => "Şifre özel karakterler içeriyor";
    }
}
