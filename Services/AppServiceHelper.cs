namespace PuppyLearn.Services
{
    public static class AppServiceHelper
    {
        public static IConfiguration configuration;
        public static void Initialize(IConfiguration configuration_)
        {
            configuration = configuration_;
        }
    }
}
