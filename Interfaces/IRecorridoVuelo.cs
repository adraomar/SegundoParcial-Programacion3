using DronSimulator.Models;

namespace DronSimulator.Interfaces
{
    public interface IAlgoritmoVuelo
    {
        ResultadoSimulacion Ejecutar(int n, int inicioX, int inicioY);
    }
}
