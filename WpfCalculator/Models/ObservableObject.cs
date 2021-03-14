using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCalculator.Models
{
    /// <summary>
    /// Model for any ViewModel to consume, which provides implementation of INotifyPropertyChanged
    /// for observable property changes. Having this as a standalone class drys up the code in a
    /// scenario where multiple View-Models have 'observable' property changes.
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Implementation of INotifyPropertyChanged, for notifying subscribers to property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property that has changed</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
