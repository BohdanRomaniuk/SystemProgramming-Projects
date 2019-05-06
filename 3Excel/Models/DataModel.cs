using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Excel.Models
{
    [Serializable]
    public class DataModel : INotifyPropertyChanged
    {
        private string a;
        private string b;
        private string c;
        private string d;
        private string e;
        private string f;
        private string g;
        private string h;
        private string i;
        private string j;

        public string A
        {
            get => a;
            set
            {
                a = value;
                OnPropertyChanged(nameof(A));
            }
        }

        public string B
        {
            get => b;
            set
            {
                b = value;
                OnPropertyChanged(nameof(B));
            }
        }

        public string C
        {
            get => c;
            set
            {
                c = value;
                OnPropertyChanged(nameof(C));
            }
        }

        public string D
        {
            get => d;
            set
            {
                d = value;
                OnPropertyChanged(nameof(D));
            }
        }

        public string E
        {
            get => e;
            set
            {
                e = value;
                OnPropertyChanged(nameof(E));
            }
        }

        public string F
        {
            get => f;
            set
            {
                f = value;
                OnPropertyChanged(nameof(F));
            }
        }

        public string G
        {
            get => g;
            set
            {
                g = value;
                OnPropertyChanged(nameof(G));
            }
        }

        public string H
        {
            get => h;
            set
            {
                h = value;
                OnPropertyChanged(nameof(H));
            }
        }

        public string I
        {
            get => i;
            set
            {
                i = value;
                OnPropertyChanged(nameof(I));
            }
        }

        public string J
        {
            get => j;
            set
            {
                j = value;
                OnPropertyChanged(nameof(J));
            }
        }

        public DataModel(string _a = null, string _b = null, string _c = null, string _d = null, string _e = null,
                         string _f = null, string _g = null, string _h = null, string _i = null, string _j = null)
        {
            A = _a;
            B = _b;
            C = _c;
            D = _d;
            E = _e;
            F = _f;
            G = _g;
            H = _h;
            I = _i;
            J = _j;
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
