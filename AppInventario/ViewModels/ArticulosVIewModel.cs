using AppInventario.Data;
using AppInventario.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppInventario.ViewModels
{
    public class ArticulosVIewModel : INotifyPropertyChanged
    {

        public ObservableCollection<Articulos> productos { get; set; }
        ArticuloDBContext context = new();

        public ArticulosVIewModel()
        {
            llenarProductos();
        }

        private async void llenarProductos()
        {
            productos = new(await context.GetAll());   
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
