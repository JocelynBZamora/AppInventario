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
                command.CommandText = (@"INSERT INTO articulos
                                    (descripcion,precio,existencia)
                                    values ($descripcion,$precio$existencia)");
                command.Parameters.AddWithValue("$descripcion", articulos.descripcion);
                command.Parameters.AddWithValue("$precio", articulos.precio);
                command.Parameters.AddWithValue("$existencia", articulos.existencia);
                await command.ExecuteNonQueryAsync();


            }
        }
        //CRUD
        //optiene solo un resultado sacado por medio del id y se utiliza reader para obtener el valor en especifico de la tabla
        public async Task<Articulos?> GetById(int id)
        {
            Articulos? articulo = null;
            if (id <= 0)
            {
                throw new ArgumentException("El id no es mayor a 0");
            }
            using (var conection = new SqliteConnection(_ConectionString))
            {
                await conection.OpenAsync();
                var command = conection.CreateCommand();
                command.CommandText = @"SELECT * FROM articulos 
                                                WHERE id = $id";
                command.Parameters.AddWithValue("$id", id);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    await reader.ReadAsync();
                    articulo = new Articulos
                    {
                        //todos tienen un tipo de get donde se especifica tipo de variable dependiendo de la variable que sea, y los nums es el puesto de columna que optienen
                        id = reader.GetInt32(0),
                        descripcion = reader.GetString(1),
                        precio = reader.GetDecimal(2),
                        existencia = reader.GetInt32(3)
                    };


                }
            }
            return articulo;
        }
        //obtiene todos los resultados
        public async Task<IEnumerable<Articulos>> GetAll()
        {
            List<Articulos> ?listarticulo = null;
            using (var connection = new SqliteConnection(_ConectionString)) 
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"SELECT id,descripcion,precio,existencia
                                        FORM articulos";
                var rader = await command.ExecuteReaderAsync();
                while(await rader.ReadAsync()) 
                {
                    if (listarticulo == null) 
                    {
                        listarticulo = new List<Articulos> { };
                    }
                    listarticulo.Add(new Articulos
                    {
                        id = rader.GetInt32(0),
                        descripcion = rader.GetString(1),
                        precio = rader.GetDecimal(2),
                        existencia = rader.GetInt32(3)
                    });
                }
                return listarticulo;
            }
        }
        //Falta revisarlos en clase pero parece que estan bien escritos, ya que las acciones se hacen con medio de querys 
        public async Task Eliminar(int id) 
        {
            using (var connection = new SqliteConnection(_ConectionString)) 
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = (@"DELTE FROM articulo WHERE id = $id");
                command.Parameters.AddWithValue("$id", id);
                //command.Parameters.Remove("id"); //este creo que esta de mas la vdd ignorar
                await command.ExecuteScalarAsync();
            }
        }
        public async Task Actualizar(Articulos articulos) 
        {
            using(var connection =  new SqliteConnection(_ConectionString)) 
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = (@"UPDATE articulos
                                        SET descripcion = $descripcio,
                                        precio = $precio,
                                        existencia =$existencia
                                        WHERE id = $id");
                command.Parameters.AddWithValue("$descripcion",articulos.descripcion);
                command.Parameters.AddWithValue("$precio", articulos.precio);
                command.Parameters.AddWithValue("$existencia", articulos.existencia);
                command.Parameters.AddWithValue("$id",articulos.id);
                await command.ExecuteScalarAsync(); 

            }
        }

    }
}
