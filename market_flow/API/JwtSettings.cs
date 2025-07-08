namespace API
{
    public class JwtSettings
    {
        public static string Key { get; set; } = "gBtZaCByLqA7jddOShsF+4uX1MLw0mZ8T20D4ZhC7qg=";
        public static string Issuer { get; set; } = "BTA_WMS";
        public static string Audience { get; set; } = "BTA_WMS_Frontend";
        public static int ExpireMinutes { get; set; } = 60;
    }
}
