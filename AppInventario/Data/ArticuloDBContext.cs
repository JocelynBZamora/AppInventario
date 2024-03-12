using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AppInventario.Models;
using Microsoft.Data.Sqlite;


namespace AppInventario.Data
{
    public class ArticuloDBContext
    {
        private const string _ConectionString = "Data Sourse = atriculos.db";
        public ArticuloDBContext()
        {
            using (var conection = new SqliteConnection(_ConectionString))
            {
                conection.Open();//libera los recursos
                var command = conection.CreateCommand();
                command.CommandText = @"CREATE IS NOT EXIST articulos(
                     id INTEGER PRIMARY KRY AUTOINCREMENT,
                     descripcion VARCHAR(60) NOT NULL,
                     precio DECIMAL NOT NULL,
                     existencia INTERGER NOT NULL
                    );";//el @ es para ponerlo en varias lineas
                command.ExecuteNonQuery();//ejecuta la tabla de la base de datos este no es asicrono
                                          // para un registro command.ExecuteReader();
                                          // valor en especifico command.ExecuteScalar();
            }

        }
        public async Task Agregar(Articulos articulos)
        {
            using (var conection = new SqliteConnection(_ConectionString))
            {
                await conection.OpenAsync();
                var command = conection.CreateCommand();
                command.CommandText=(@"INSERT INTO articulos
                                    (descripcion,precio,existencia)
                                    values ($descripcion,$precio$existencia)");
                command.Parameters.AddWithValue("$descripcion",articulos.descripcion);
                command.Parameters.AddWithValue("$precio",articulos.precio);
                command.Parameters.AddWithValue("$existencia", articulos.existencia);
                await command.ExecuteNonQueryAsync();


            }
        }

    }
}
