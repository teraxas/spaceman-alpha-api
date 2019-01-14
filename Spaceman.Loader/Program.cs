using Microsoft.Extensions.CommandLineUtils;
using System;

namespace Spaceman.Loader
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.Name = "ninja";
            app.HelpOption("-?|-h|--help");
        }
    }
}
