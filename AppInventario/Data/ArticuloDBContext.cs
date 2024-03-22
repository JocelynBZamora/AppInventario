using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
//using Android.Hardware;
using AppInventario.Models;
//using Javax.Security.Auth;
using Microsoft.Data.Sqlite;
using SQLite;

namespace AppInventario.Data
{
    public class ArticuloDBContext
    {
        //Forma rapida para poder crear la tabla en sql y poder hacer el CRUD
        private const string _ConectionString = "Data Source=atriculos.db";

        private string _dataBaseFilName = "inventario.db";
        const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            //crea la base de datos si no existe
            SQLite.SQLiteOpenFlags.Create |
            //abilita multi-threaded acces
            SQLite.SQLiteOpenFlags.SharedCache;

        public string DataBasePath => Path.Combine(FileSystem.AppDataDirectory, _dataBaseFilName);
        private string _conectionString = "";



        SQLiteAsyncConnection db;

        //Crea la tabla en sql
        public async Task Init()
        {
            if (db != null)
            {
                return;
            }
            db = new SQLiteAsyncConnection(DataBasePath, Flags);
            SQLite.CreateFlags createFlags = SQLite.CreateFlags.ImplicitPK | SQLite.CreateFlags.AutoIncPK;
            var resulr = await db.CreateTableAsync<Articulos>(createFlags);
        }


        public async Task Add(Articulos articulo)
        {
            await Init();
            await db.InsertAsync(articulo);
        }
        public async Task<Articulos?> GetById(int id)
        {
            await Init();
            return await db.Table<Articulos>().Where(x => x.id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Articulos>?> GetAll()
        {
            await Init();
            return await db.Table<Articulos>().ToListAsync();
        }
        public async Task Actualizar(Articulos articulo)
        {
            await Init();
            await db.UpdateAsync(articulo);
        }
        public async Task Eliminar(int id)
        {
            await Init();
            var art = await GetById(id);
            if (art != null)
            {
                await db.DeleteAsync(art);
            }

        }

    }
    //Forma larga de hacer el CRUD

    //    public ArticuloDBContext()
    //    {

    //        using (var conection = new SqliteConnection(_ConectionString))
    //        {
    //            conection.Open();//libera los recursos
    //            var command = conection.CreateCommand();
    //            command.CommandText = @"CREATE TABLE IF NOT EXISTS articulos(
    //                 id INTEGER PRIMARY KEY AUTOINCREMENT,
    //                 descripcion VARCHAR(60) NOT NULL,
    //                 precio DECIMAL NOT NULL,
    //                 existencia INTEGER NOT NULL
    //                );";//el @ es para ponerlo en varias lineas
    //            command.ExecuteNonQuery();//ejecuta la tabla de la base de datos este no es asicrono
    //                                      // para un registro command.ExecuteReader();
    //                                      // valor en especifico command.ExecuteScalar();
    //        }

    //    }
    //    public async Task Agregar(Articulos articulos)
    //    {
    //        using (var conection = new SqliteConnection(_ConectionString))
    //        {
    //            await conection.OpenAsync();
    //            var command = conection.CreateCommand();
    //            command.CommandText = (@"INSERT INTO articulos
    //                                (descripcion,precio,existencia)
    //                                values ($descripcion,$precio,$existencia)");
    //            command.Parameters.AddWithValue("$descripcion", articulos.descripcion);
    //            command.Parameters.AddWithValue("$precio", articulos.precio);
    //            command.Parameters.AddWithValue("$existencia", articulos.existencia);
    //            await command.ExecuteNonQueryAsync();


    //        }
    //    }
    //    //CRUD
    //    //optiene solo un resultado sacado por medio del id y se utiliza reader para obtener el valor en especifico de la tabla
    //    public async Task<Articulos?> GetById(int id)
    //    {
    //        Articulos? articulo = null;
    //        using (var connection = new SqliteConnection(_ConectionString))
    //        {

    //            await connection.OpenAsync();
    //            var command = connection.CreateCommand();
    //            command.CommandText = @"SELECT * FROM articulos WHERE id= $id";
    //            command.Parameters.AddWithValue("$id", id);

    //            using (var reader = await command.ExecuteReaderAsync())
    //            {
    //                if (reader.HasRows)
    //                {
    //                    await reader.ReadAsync();

    //                    articulo = new Articulos()
    //                    {
    //                        id = reader.GetInt32(0),
    //                        descripcion = reader.GetString(1),
    //                        precio = reader.GetDecimal(2),
    //                        existencia = reader.GetInt32(3),
    //                    };

    //                }
    //            }
    //        }
    //        return articulo;
    //    }
    //    //obtiene todos los resultados
    //    public async Task<IEnumerable<Articulos>> GetAll()
    //    {
    //        List<Articulos> articulos = new();
    //        using (var connection = new SqliteConnection(_ConectionString))
    //        {
    //            await connection.OpenAsync();
    //            var command = connection.CreateCommand();
    //            command.CommandText = @"SELECT * FROM articulos";
    //            var reader = await command.ExecuteReaderAsync();
    //            while (await reader.ReadAsync())
    //            {
    //                if (articulos.Count == 0)
    //                {
    //                    articulos = new List<Articulos>();
    //                }
    //                articulos.Add(new Articulos()
    //                {
    //                    id = reader.GetInt32(0),
    //                    descripcion = reader.GetString(1),
    //                    precio = reader.GetDecimal(2),
    //                    existencia = (int)reader.GetInt32(3),
    //                });
    //            }
    //        }


    //        return articulos;
    //    }
    //    //Falta revisarlos en clase pero parece que estan bien escritos, ya que las acciones se hacen con medio de querys 
    //    public async Task Eliminar(int id)
    //    {
    //        using (var connection = new SqliteConnection(_ConectionString))
    //        {
    //            await connection.OpenAsync();
    //            var command = connection.CreateCommand();
    //            command.CommandText = (@"DELETE FROM articulos WHERE id = $id");
    //            command.Parameters.AddWithValue("$id", id);
    //            //command.Parameters.Remove("id"); //este creo que esta de mas la vdd ignorar
    //            await command.ExecuteNonQueryAsync();
    //        }
    //    }
    //    public async Task Actualizar(Articulos articulos)
    //    {
    //        using (var connection = new SqliteConnection(_ConectionString))
    //        {
    //            await connection.OpenAsync();
    //            var command = connection.CreateCommand();
    //            command.CommandText = (@"UPDATE articulos
    //                                    SET descripcion = $descripcio,
    //                                    precio = $precio,
    //                                    existencia =$existencia
    //                                    WHERE id = $id");
    //            command.Parameters.AddWithValue("$descripcion", articulos.descripcion);
    //            command.Parameters.AddWithValue("$precio", articulos.precio);
    //            command.Parameters.AddWithValue("$existencia", articulos.existencia);
    //            command.Parameters.AddWithValue("$id", articulos.id);
    //            await command.ExecuteNonQueryAsync();

    //        }
    //    }

    //}
}
