using Excel.Helpers;
using Excel.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Excel.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<DataModel> Table { get; set; }

        private string formula;
        private DataGridCellInfo selectedColumn;

        public string Formula
        {
            get => formula;
            set
            {
                formula = value;
                OnPropertyChanged(nameof(Formula));
            }
        }

        public DataGridCellInfo SelectedColumn
        {
            get => selectedColumn;
            set
            {
                if (value.IsValid)
                {
                    selectedColumn = value;
                }
                OnPropertyChanged(nameof(SelectedColumn));
            }
        }

        public ICommand ClearCommand { get; }
        public ICommand OpenCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CloseWindowCommand { get; }
        public ICommand AboutCommand { get; }
        public ICommand ApplyFormulaCommand { get; }
        public ICommand NewRowCommand { get; }
        public ICommand PasteCommand { get; }

        public MainViewModel()
        {
            Table = new ObservableCollection<DataModel>();
            Clear(null);
            //Formula = "=SUM(A1:A2)";
            Formula = "=SUM(A1,A2,A4)";
            ClearCommand = new Command(Clear);
            OpenCommand = new Command(Open);
            SaveCommand = new Command(Save);
            CloseWindowCommand = new Command(CloseWindow);
            AboutCommand = new Command(About);
            ApplyFormulaCommand = new Command(ApplyFormula);
            NewRowCommand = new Command(NewRow);
            PasteCommand = new Command(Paste);
        }

        private void ApplyFormula(object obj)
        {
            var props = Regex.Match(Formula, @"\((.*?)\)")?.Value?.Replace("(", string.Empty)?.Replace(")", string.Empty);
            switch (Formula.Substring(1, Formula.IndexOf('(') - 1))
            {
                case "SUM":
                    if (props.Contains(':'))
                    {
                        var splitted = props.Split(':');
                        var col = Regex.Match(splitted[0], "[A-Z]")?.Value;
                        int.TryParse(Regex.Match(splitted[0], "[0-9]+")?.Value, out var fromRow);
                        int.TryParse(Regex.Match(splitted[1], "[0-9]+")?.Value, out var toRow);

                        double result = 0;
                        if (fromRow != 0 && toRow != 0)
                        {
                            for (int i = fromRow; i <= toRow; ++i)
                            {
                                result += Table[i - 1].GetValue(col);
                            }
                        }
                        (SelectedColumn.Item as DataModel).SetValue((string)SelectedColumn.Column.Header, result.ToString());
                    }
                    else if (props.Contains(','))
                    {
                        var splitted = props.Split(',');
                        double result = 0;
                        foreach (var split in splitted)
                        {
                            var col = Regex.Match(split, "[A-Z]")?.Value;
                            int.TryParse(Regex.Match(split, "[0-9]+")?.Value, out var row);
                            if (col != null && row != 0)
                            {
                                result += Table[row - 1].GetValue(col);
                            }
                        }
                        (SelectedColumn.Item as DataModel).SetValue((string)SelectedColumn.Column.Header, result.ToString());
                    }
                    break;
                case "MIN":
                    if (props.Contains(':'))
                    {
                        var splitted = props.Split(':');
                        var col = Regex.Match(splitted[0], "[A-Z]")?.Value;
                        int.TryParse(Regex.Match(splitted[0], "[0-9]+")?.Value, out var fromRow);
                        int.TryParse(Regex.Match(splitted[1], "[0-9]+")?.Value, out var toRow);

                        double minimum = Table[fromRow - 1].GetValue(col);
                        if (fromRow != 0 && toRow != 0)
                        {
                            for (int i = fromRow; i <= toRow; ++i)
                            {
                                if (Table[i - 1].GetValue(col) < minimum)
                                {
                                    minimum = Table[i - 1].GetValue(col);
                                }
                            }
                        }
                        (SelectedColumn.Item as DataModel).SetValue((string)SelectedColumn.Column.Header, minimum.ToString());
                    }
                    else if (props.Contains(','))
                    {
                        var splitted = props.Split(',');
                        var col = Regex.Match(splitted[0], "[A-Z]")?.Value;
                        int.TryParse(Regex.Match(splitted[0], "[0-9]+")?.Value, out var row);
                        double minimum = Table[row - 1].GetValue(col);
                        foreach (var split in splitted)
                        {
                            var coll = Regex.Match(split, "[A-Z]")?.Value;
                            int.TryParse(Regex.Match(split, "[0-9]+")?.Value, out var rowl);
                            if (coll != null && rowl != 0)
                            {
                                if (Table[rowl - 1].GetValue(coll) < minimum)
                                {
                                    minimum = Table[rowl - 1].GetValue(col);
                                }
                            }
                        }
                        (SelectedColumn.Item as DataModel).SetValue((string)SelectedColumn.Column.Header, minimum.ToString());
                    }
                    break;
                case "MAX":
                    if (props.Contains(':'))
                    {
                        var splitted = props.Split(':');
                        var col = Regex.Match(splitted[0], "[A-Z]")?.Value;
                        int.TryParse(Regex.Match(splitted[0], "[0-9]+")?.Value, out var fromRow);
                        int.TryParse(Regex.Match(splitted[1], "[0-9]+")?.Value, out var toRow);

                        double maximum = Table[fromRow - 1].GetValue(col);
                        if (fromRow != 0 && toRow != 0)
                        {
                            for (int i = fromRow; i <= toRow; ++i)
                            {
                                if (Table[i - 1].GetValue(col) > maximum)
                                {
                                    maximum = Table[i - 1].GetValue(col);
                                }
                            }
                        }
                        (SelectedColumn.Item as DataModel).SetValue((string)SelectedColumn.Column.Header, maximum.ToString());
                    }
                    else if (props.Contains(','))
                    {
                        var splitted = props.Split(',');
                        var col = Regex.Match(splitted[0], "[A-Z]")?.Value;
                        int.TryParse(Regex.Match(splitted[0], "[0-9]+")?.Value, out var row);
                        double maximum = Table[row - 1].GetValue(col);
                        foreach (var split in splitted)
                        {
                            var coll = Regex.Match(split, "[A-Z]")?.Value;
                            int.TryParse(Regex.Match(split, "[0-9]+")?.Value, out var rowl);
                            if (coll != null && rowl != 0)
                            {
                                if (Table[rowl - 1].GetValue(coll) > maximum)
                                {
                                    maximum = Table[rowl - 1].GetValue(col);
                                }
                            }
                        }
                        (SelectedColumn.Item as DataModel).SetValue((string)SelectedColumn.Column.Header, maximum.ToString());
                    }
                    break;
            }
        }

        private void Paste(object parameter)
        {
            var copiyedRow = Clipboard.GetText();
            var splitted = copiyedRow.Split('\t').Select(c => c.Replace("\n", string.Empty).Replace("\r", string.Empty)).ToArray();
            var props = typeof(DataModel).GetProperties();
            for (int i = 0; i < props.Count(); ++i)
            {
                (SelectedColumn.Item as DataModel).SetValue(props[i].Name, splitted[i]);
            }
        }

        private void Clear(object obj)
        {
            Table.Clear();
            for (int i = 0; i < 20; ++i)
            {
                Table.Add(new DataModel());
            }
        }

        private void Open(object obj)
        {
            Table.Clear();
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "xls(*.xls)|*.xls";
            if (ofd.ShowDialog() ?? true)
            {
                using (Stream reader = File.Open(ofd.FileName, FileMode.Open))
                {
                    BinaryFormatter ser = new BinaryFormatter();
                    Table = (ObservableCollection<DataModel>)ser.Deserialize(reader);
                    OnPropertyChanged(nameof(Table));
                }
            }
        }

        private void Save(object obj)
        {
            Microsoft.Win32.SaveFileDialog svd = new Microsoft.Win32.SaveFileDialog();
            svd.Filter = "xls(*.xls)|*.xls";
            if (svd.ShowDialog() ?? true)
            {
                using (FileStream fileStr = new FileStream(svd.FileName, FileMode.Create))
                {
                    BinaryFormatter binFormater = new BinaryFormatter();
                    binFormater.Serialize(fileStr, new ObservableCollection<DataModel>(Table.Where(d => !d.IsEmpty())));

                }
            }
        }

        private void CloseWindow(object parameter)
        {
            (parameter as Window)?.Close();
        }

        private void NewRow(object parameter)
        {
            Table.Add(new DataModel());
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
