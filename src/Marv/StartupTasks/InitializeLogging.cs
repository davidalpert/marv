using System;
using System.Linq;
using System.Collections.Generic;
using Bootstrap.Extensions.StartupTasks;
using Common.Logging;

namespace Marv.StartupTasks
{
    public class InitializeLogging : IStartupTask
    {
        private readonly ILog _log;

        public InitializeLogging()
        {
            _log = LogManager.GetCurrentClassLogger();
        }

        public void Run()
        {
            _log.Info(m => m("-----------------------------------------------------------------------------"));
            _log.Info(m => m("Starting up in: {0}", Environment.CurrentDirectory));
        }

        public void Reset()
        {
            _log.Info(m => m("Shutting down."));
        }
    }
}