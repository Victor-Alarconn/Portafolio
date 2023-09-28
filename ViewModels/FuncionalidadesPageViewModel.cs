using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Portafolio.ViewModels
{
    public class FuncionalidadesPageViewModel : INotifyPropertyChanged
    {

        public ICommand ToggleAnswerCommand { get; private set; }
        private bool isAnswerVisible;
        public bool IsAnswerVisible
        {
            get { return isAnswerVisible; }
            set
            {
                isAnswerVisible = value;
                OnPropertyChanged(nameof(IsAnswerVisible));
            }
        }


        public FuncionalidadesPageViewModel()
        {
            ToggleAnswerCommand = new Command(ToggleAnswer);
        }

        private void ToggleAnswer()
        {
            IsAnswerVisible = !IsAnswerVisible;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
