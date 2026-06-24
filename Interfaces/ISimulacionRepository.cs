using DronSimulator.Models;

namespace DronSimulator.Interfaces
{
    public interface ISimulacionRepository
    {
        int Guardar(SimulacionCabecera cabecera, List<MovimientoDron> movimientos);
        List<MovimientoDron> ObtenerUltimos5(int masterId);
    }
}
