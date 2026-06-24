using DronSimulator.Interfaces;

namespace DronSimulator.Services
{
    /// <summary>
    /// SOLID - SRP: implementa únicamente la lógica de ofuscación y
    /// reconstrucción de pasos, según las reglas de la Parte D y E.
    /// </summary>
    public class OfuscadorPasos : IOfuscador
    {
        /// <summary>
        /// PAR  -> paso * 2   (ej: 4 -> 8)
        /// IMPAR -> paso * -1  (ej: 3 -> -3)
        /// </summary>
        public int Ofuscar(int pasoReal)
        {
            if (pasoReal % 2 == 0)
                return pasoReal * 2;
            else
                return pasoReal * -1;
        }

        /// <summary>
        /// Negativo -> real = valor * -1   (era impar)
        /// >= 0     -> real = valor / 2    (era par)
        /// </summary>
        public int Reconstruir(int valorGuardado)
        {
            if (valorGuardado < 0)
                return valorGuardado * -1;
            else
                return valorGuardado / 2;
        }
    }
}
