using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MorabarabaV2;

namespace Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameSession _gameSession;

        public MainWindow()
        {
            InitializeComponent();

            _gameSession = new GameSession();
            DataContext = _gameSession;
        }

        private void onClick_Input(object sender, RoutedEventArgs e)
        {
            _gameSession.currentInput = inBox.Text;
            inBox.Clear();
            _gameSession.performAction();

        }
    }
}
