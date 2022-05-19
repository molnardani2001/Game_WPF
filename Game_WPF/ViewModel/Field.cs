using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Game_WPF.ViewModel
{
    //Egy mező osztálya
    public class Field : ViewModelBase
    {
        #region fields
        //mező háttérszíne
        private Brush _color;
        #endregion

        #region properties
        public Brush Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged("Color");
            }
        }

        public Int32 X { get; set; }

        public Int32 Y { get; set; }

        public Int32 Number { get; set; }

        #endregion
    }
}
