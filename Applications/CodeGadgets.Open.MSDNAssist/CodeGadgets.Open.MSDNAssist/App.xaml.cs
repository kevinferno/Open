using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CodeGadgets.Open.MSDNAssist.Properties;
using CodeGadgets.Open.MSDNAssist.View;
using CodeGadgets.Open.MSDNAssist.ViewModel;

namespace CodeGadgets.Open.MSDNAssist
{
	public partial class App : Application
	{
		private MainWindowView MW;
		private MainWindowViewModel MWVM;

		private void Application_Startup(object sender, StartupEventArgs e)
		{
			MW = new MainWindowView();
			MWVM = new MainWindowViewModel(MW);
			MW.DataContext = MWVM;

			LoadSettings();
			Logic.Do.Initialize();
			MW.ShowDialog();
		}
		private void Application_Exit(object sender, ExitEventArgs e)
		{
			Logic.Do.Terminate();
			SaveSettings();
		}
		private void Application_SessionEnding(object sender, SessionEndingCancelEventArgs e)
		{

		}

		private void LoadSettings()
		{
			MW.Left = Settings.Default.LastX;
			MW.Top = Settings.Default.LastY;
			if (Settings.Default.LastWidth >= 100) MW.Width = Settings.Default.LastWidth;
			if (Settings.Default.LastHeight >= 100) MW.Height = Settings.Default.LastHeight;
		}

		private void SaveSettings()
		{
			Settings.Default.LastHeight = MW.Height;
			Settings.Default.LastWidth = MW.Width;
			Settings.Default.LastX = MW.Left;
			Settings.Default.LastY = MW.Top;
			Settings.Default.Save();
		}


	}
}
