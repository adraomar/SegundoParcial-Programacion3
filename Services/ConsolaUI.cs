using DronSimulator.Interfaces;
using DronSimulator.Models;

namespace DronSimulator.Services
{
    public class ConsolaUI : IConsolaUI
    {
        private readonly IOfuscador _ofuscador;

        public ConsolaUI(IOfuscador ofuscador)
        {
            _ofuscador = ofuscador;
        }

        public int PedirDimensionTerreno()
        {
            int n = 0;
            bool valido = false;

            while (!valido)
            {
                Console.Write("\nIngrese el tamaño del terreno (N >= 1): ");
                string? entrada = Console.ReadLine();

                if (int.TryParse(entrada, out n) && n >= 1)
                    valido = true;
                else
                    Console.WriteLine("  [ERROR] Valor inválido. N debe ser un entero mayor o igual a 1.");
            }

            return n;
        }

        public (int x, int y) PedirCoordenadas(int n)
        {
            int x = LeerCoordenada("fila (X)", n);
            int y = LeerCoordenada("columna (Y)", n);
            return (x, y);
        }

        private int LeerCoordenada(string nombre, int n)
        {
            int valor = -1;
            bool valido = false;

            while (!valido)
            {
                Console.Write($"  Ingrese la {nombre} de despegue [0, {n - 1}]: ");
                string? entrada = Console.ReadLine();

                if (int.TryParse(entrada, out valor) && valor >= 0 && valor < n)
                    valido = true;
                else
                    Console.WriteLine($"  [ERROR] La {nombre} debe estar en el rango [0, {n - 1}].");
            }

            return valor;
        }

        public void MostrarTerreno(int[,] terreno, int n)
        {
            Console.WriteLine("\n--- Matriz del recorrido ---");

            int maxVal = n * n - 1;
            int ancho  = maxVal.ToString().Length + 1;

            int fila = 0;
            while (fila < n)
            {
                int col = 0;
                while (col < n)
                {
                    if (terreno[fila, col] == -1)
                        Console.Write(".".PadLeft(ancho));
                    else
                        Console.Write(terreno[fila, col].ToString().PadLeft(ancho));
                    col++;
                }
                Console.WriteLine();
                fila++;
            }
        }

        public void MostrarUltimos5(List<MovimientoDron> movimientos)
        {
            Console.WriteLine("\n--- Últimos 5 movimientos (reconstruidos desde BD) ---");
            Console.WriteLine($"  {"Valor BD (ofuscado)",-25} {"Paso real",-12} {"Fila",-8} {"Columna"}");
            Console.WriteLine("  " + new string('-', 55));

            int i = 0;
            while (i < movimientos.Count)
            {
                MovimientoDron mov = movimientos[i];

                int pasoReal = _ofuscador.Reconstruir(mov.Paso);

                Console.WriteLine($"  {mov.Paso,-25} {pasoReal,-12} {mov.CoordX,-8} {mov.CoordY}");
                i++;
            }
        }
    }
}
