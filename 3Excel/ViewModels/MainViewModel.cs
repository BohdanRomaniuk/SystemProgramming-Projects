using Excel.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Excel.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string formula;

        public string Formula
        {
            get => formula;
            set
            {
                formula = value;
                OnPropertyChanged(nameof(Formula));
            }
        }

        public ICommand ClearCommand { get; }
        public ICommand OpenCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CloseWindowCommand { get; }
        public ICommand AboutCommand { get; }
        public ICommand ApplyFormulaCommand { get; }

        public MainViewModel()
        {
            ClearCommand = new Command(Clear);
            OpenCommand = new Command(Open);
            SaveCommand = new Command(Save);
            CloseWindowCommand = new Command(CloseWindow);
            AboutCommand = new Command(About);
            ApplyFormulaCommand = new Command(ApplyFormula);
        }

        private void ApplyFormula(object obj)
        {
            throw new NotImplementedException();
        }

        private void Clear(object obj)
        {
            throw new NotImplementedException();
        }

        private void Open(object obj)
        {
            throw new NotImplementedException();
        }

        private void Save(object obj)
        {
            throw new NotImplementedException();
        }

        private void CloseWindow(object parameter)
        {
            (parameter as Window)?.Close();
        }

        private void About(object obj)
        {
            MessageBox.Show("Програму розробив студент групи ПМі-43 Романюк Богдан", "Про програму", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
