namespace DronSimulator.Models
{
    public class ResultadoSimulacion
    {
        public bool Exitoso { get; set; }
        public int[,] Terreno { get; set; } = new int[0, 0];

        public List<MovimientoDron> Movimientos { get; set; } = new();

        public int ParcelasAlcanzables { get; set; }
    }
}
