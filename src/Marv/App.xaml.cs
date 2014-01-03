using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Bootstrap;
using Bootstrap.Extensions.StartupTasks;
using Bootstrap.StructureMap;
using Marv.StartupTasks;

namespace Marv
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Bootstrapper.With
                        .StartupTasks()
                            .UsingThisExecutionOrder(s => s
                                .First<InitializeLogging>()
                                .Then().TheRest()
                                .Then<ShowMainWindow>())
                        .Start();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            Bootstrapper.Reset();
        }
    }
}
