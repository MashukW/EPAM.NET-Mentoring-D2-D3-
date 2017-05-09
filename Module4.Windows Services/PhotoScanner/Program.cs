using System;
using System.IO;
using Topshelf;

namespace PhotoProcessingService
{
    class Program
    {
        static void Main(string[] args)
        {
            var _currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var _inDirectory = Path.Combine(_currentDirectory, "input");
            var _outDirectory = Path.Combine(_currentDirectory, "output");
            var _tempDirectory = Path.Combine(_currentDirectory, "temp");

            HostFactory.Run(x =>
            {
                x.UseNLog();

                x.Service(() => new PhotoProcessingService(_inDirectory, _outDirectory, _tempDirectory));

                x.SetServiceName("Photo Processing Service");
                x.SetDisplayName("Photo Service");
                x.SetDescription("Photo Service");
                x.StartAutomaticallyDelayed();
                x.RunAsLocalSystem();
            });
        }
    }
}
