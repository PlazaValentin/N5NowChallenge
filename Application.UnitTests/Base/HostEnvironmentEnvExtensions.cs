using Microsoft.Extensions.Hosting;

namespace Application.UnitTests.Base;

public class HostEnvironmentEnvExtensions
{
    public static bool IsDevelopment(IHostEnvironment hostEnvironment)
    {
        return hostEnvironment.EnvironmentName == "Development";
    }
}
