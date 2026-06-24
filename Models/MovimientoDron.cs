namespace DronSimulator.Models
{
    public class MovimientoDron
    {
        public int MasterId { get; set; }
        public int Paso { get; set; }   // valor real, sin ofuscar
        public int CoordX { get; set; }
        public int CoordY { get; set; }
    }
}
