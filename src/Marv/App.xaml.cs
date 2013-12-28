using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Bootstrap;
using Bootstrap.Extensions.StartupTasks;
using Bootstrap.StructureMap;

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
                        /*
                            .UsingThisExecutionOrder(s => s
                                .First().TheRest()
                                .Then<ShowMainWindow>())
                         */
                        .Start();
        }
    }
}
