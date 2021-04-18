using Xamarin.Essentials;

namespace TodaysManna.Services
{
    public class PermissionService : IPermissionService
    {
        public NetworkAccess CheckNetwork()
        {
            return Connectivity.NetworkAccess;
        }
    }

    public interface IPermissionService
    {
        NetworkAccess CheckNetwork();
    }
}
