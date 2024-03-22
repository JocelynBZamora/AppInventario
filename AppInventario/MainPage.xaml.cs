using AppInventario.Data;
using AppInventario.Models;

namespace AppInventario
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }


        private async void ContentPage_Loaded(object sender, EventArgs e)
        {
            var db = new ArticuloDBContext();//se crea la tabla
            //var aticulo3 = new Articulos()
            //{
            //    descripcion = "Angelito 600gr.",
            //    precio = 5,
            //    existencia = 1
            //};
            //db.Agregar(aticulo3);
            var articulo = await db.GetById(1);
            //articulo.precio = 48;
            await db.Eliminar(1);
            var lista = db.GetAll();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {

        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {

        }

        private void Button_Clicked_3(object sender, EventArgs e)
        {

        }
    }

}
