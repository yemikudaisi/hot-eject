using Caliburn.Micro;
using Hardcodet.Wpf.TaskbarNotification;
using Sru.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Windows;

namespace Sru.Wpf
{
    public class AppBootstrapper : BootstrapperBase
    {
        private CompositionContainer container;

        public AppBootstrapper() : base(true)
        {
            base.Initialize();
        }

        protected override void Configure()
        {
            this.container = new CompositionContainer(new AggregateCatalog((
                from x in AssemblySource.Instance
                select new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()), new ExportProvider[0]);
            CompositionBatch compositionBatch = new CompositionBatch();
            compositionBatch.AddExportedValue<IWindowManager>(new WindowManager());
            compositionBatch.AddExportedValue<IEventAggregator>(new EventAggregator());
            compositionBatch.AddExportedValue<CompositionContainer>(this.container);
            this.container.Compose(compositionBatch);
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            string str = (string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key);
            IEnumerable<object> exportedValues = this.container.GetExportedValues<object>(str);
            if (exportedValues.Count<object>() <= 0)
            {
                throw new Exception(string.Format("Could not locate any instances of contract {0}.", str));
            }
            return exportedValues.First<object>();
        }

        TaskbarIcon _taskbarIcon;

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            var viewModel = IoC.Get<ITaskbarIconShell>();
            var windowManager = IoC.Get<TaskbarIconWindowManager>();
            var _mainWindow = windowManager.MainWindow(viewModel);
            _taskbarIcon = _mainWindow.TaskBarIcon;
            viewModel.TaskbarIcon = _taskbarIcon;
            _mainWindow.Hide();
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            _taskbarIcon.Dispose();
            base.OnExit(sender, e);
        }

    }
}
