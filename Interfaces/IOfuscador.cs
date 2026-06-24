namespace DronSimulator.Interfaces
{
    public interface IOfuscador
    {
        int Ofuscar(int pasoReal);
        int Reconstruir(int valorGuardado);
    }
}
