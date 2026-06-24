using DronSimulator.Models;

namespace DronSimulator.Interfaces
{
    public interface IConsolaUI
    {
        int PedirDimensionTerreno();
        (int x, int y) PedirCoordenadas(int n);
        void MostrarTerreno(int[,] terreno, int n);
        void MostrarUltimos5(List<MovimientoDron> movimientos);
    }
}
