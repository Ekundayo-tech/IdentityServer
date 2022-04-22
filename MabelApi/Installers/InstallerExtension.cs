using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MabelApi.Installers
{
    public static class InstallerExtension
    {
        public static void InstallServiceAssembly(this IServiceCollection services, IConfiguration Configuration)
        {
            var installerimplement = typeof(Startup).Assembly.ExportedTypes.Where(x =>
                 typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).Select(Activator.CreateInstance).Cast<IInstaller>().ToList();

            installerimplement.ForEach(Installer => Installer.InstallerServices(services, Configuration));
        }
    }
}
