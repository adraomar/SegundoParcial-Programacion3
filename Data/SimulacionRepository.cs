using DronSimulator.Interfaces;
using DronSimulator.Models;
using Npgsql;

namespace DronSimulator.Data
{
    public class SimulacionRepository : ISimulacionRepository
    {
        private readonly string _connectionString;
        private readonly IOfuscador _ofuscador;

        public SimulacionRepository(string connectionString, IOfuscador ofuscador)
        {
            _connectionString = connectionString;
            _ofuscador = ofuscador;
        }

        public int Guardar(SimulacionCabecera cabecera, List<MovimientoDron> movimientos)
        {
            int masterIdGenerado = 0;

            using (NpgsqlConnection conexion = new NpgsqlConnection(_connectionString))
            {
                conexion.Open();

                using (NpgsqlTransaction transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        string sqlCabecera =
                            "INSERT INTO tb_master_control (fecha, n, coord_x, coord_y) " +
                            "VALUES (@fecha, @n, @x, @y) " +
                            "RETURNING id";

                        using (NpgsqlCommand cmdCabecera = new NpgsqlCommand(sqlCabecera, conexion, transaccion))
                        {
                            cmdCabecera.Parameters.AddWithValue("@fecha", cabecera.Fecha);
                            cmdCabecera.Parameters.AddWithValue("@n",     cabecera.N);
                            cmdCabecera.Parameters.AddWithValue("@x",     cabecera.CoordX);
                            cmdCabecera.Parameters.AddWithValue("@y",     cabecera.CoordY);

                            object? resultado = cmdCabecera.ExecuteScalar();
                            masterIdGenerado = Convert.ToInt32(resultado);
                        }

                        string sqlDetalle =
                            "INSERT INTO tb_det_log (master_id, paso, coord_x, coord_y) " +
                            "VALUES (@masterId, @paso, @x, @y)";

                        using (NpgsqlCommand cmdDetalle = new NpgsqlCommand(sqlDetalle, conexion, transaccion))
                        {
                            cmdDetalle.Parameters.Add(new NpgsqlParameter("@masterId", NpgsqlTypes.NpgsqlDbType.Integer));
                            cmdDetalle.Parameters.Add(new NpgsqlParameter("@paso",     NpgsqlTypes.NpgsqlDbType.Integer));
                            cmdDetalle.Parameters.Add(new NpgsqlParameter("@x",        NpgsqlTypes.NpgsqlDbType.Integer));
                            cmdDetalle.Parameters.Add(new NpgsqlParameter("@y",        NpgsqlTypes.NpgsqlDbType.Integer));

                            int i = 0;
                            while (i < movimientos.Count)
                            {
                                MovimientoDron mov = movimientos[i];

                                int pasoOfuscado = _ofuscador.Ofuscar(mov.Paso);

                                cmdDetalle.Parameters["@masterId"].Value = masterIdGenerado;
                                cmdDetalle.Parameters["@paso"].Value     = pasoOfuscado;
                                cmdDetalle.Parameters["@x"].Value        = mov.CoordX;
                                cmdDetalle.Parameters["@y"].Value        = mov.CoordY;

                                cmdDetalle.ExecuteNonQuery();

                                i++;
                            }
                        }

                        transaccion.Commit();
                        Console.WriteLine($"\n[BD] Transacción confirmada. ID de simulación generado: {masterIdGenerado}");
                    }
                    catch (Exception ex)
                    {
                        transaccion.Rollback();
                        Console.WriteLine($"\n[BD] Error al guardar. Transacción revertida. Detalle: {ex.Message}");
                        throw;
                    }
                }
            }

            return masterIdGenerado;
        }

        public List<MovimientoDron> ObtenerUltimos5(int masterId)
        {
            List<MovimientoDron> resultado = new();

            string sql =
                "SELECT paso, coord_x, coord_y " +
                "FROM tb_det_log " +
                "WHERE master_id = @masterId " +
                "ORDER BY id DESC " +
                "LIMIT 5";

            using (NpgsqlConnection conexion = new NpgsqlConnection(_connectionString))
            {
                conexion.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, conexion))
                {
                    cmd.Parameters.AddWithValue("@masterId", masterId);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultado.Add(new MovimientoDron
                            {
                                MasterId = masterId,
                                Paso   = reader.GetInt32(0),
                                CoordX = reader.GetInt32(1),
                                CoordY = reader.GetInt32(2)
                            });
                        }
                    }
                }
            }

            return resultado;
        }
    }
}
