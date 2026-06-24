using DronSimulator.Interfaces;
using DronSimulator.Models;

namespace DronSimulator.Algorithm
{
    public class AlgoritmoVueloDron : IAlgoritmoVuelo
    {
        private static readonly int[] DeltaFila = { -2, -2, 2, 2, -1, -1, 1, 1 };
        private static readonly int[] DeltaColumna = { -1, 1, -1, 1, -2, 2, -2, 2 };

        public ResultadoSimulacion Ejecutar(int n, int inicioX, int inicioY)
        {
            int[,] terreno = new int[n, n];
            for (int f = 0; f < n; f++)
                for (int c = 0; c < n; c++)
                    terreno[f, c] = -1;

            int parcelasAlcanzables = ContarAlcanzables(n, inicioX, inicioY);

            List<MovimientoDron> movimientos = new();

            terreno[inicioX, inicioY] = 0;
            movimientos.Add(new MovimientoDron { Paso = 0, CoordX = inicioX, CoordY = inicioY });

            bool exito;

            if (parcelasAlcanzables == 1)
            {
                exito = true;
            }
            else
            {
                exito = Resolver(terreno, n, inicioX, inicioY, 1, parcelasAlcanzables, movimientos);
            }

            return new ResultadoSimulacion
            {
                Exitoso = exito,
                Terreno = terreno,
                Movimientos = movimientos,
                ParcelasAlcanzables = parcelasAlcanzables
            };
        }
        private bool Resolver(int[,] terreno, int n, int filaActual, int colActual,
                              int pasoActual, int objetivo, List<MovimientoDron> movimientos)
        {
            if (pasoActual == objetivo)
                return true;

            List<(int fila, int col, int grado)> candidatos =
                ObtenerCandidatosOrdenados(terreno, n, filaActual, colActual);

            int i = 0;
            while (i < candidatos.Count)
            {
                (int siguienteFila, int siguienteCol, _) = candidatos[i];

                terreno[siguienteFila, siguienteCol] = pasoActual;
                movimientos.Add(new MovimientoDron
                {
                    Paso = pasoActual,
                    CoordX = siguienteFila,
                    CoordY = siguienteCol
                });

                if (Resolver(terreno, n, siguienteFila, siguienteCol,
                             pasoActual + 1, objetivo, movimientos))
                    return true;

                terreno[siguienteFila, siguienteCol] = -1;
                movimientos.RemoveAt(movimientos.Count - 1);

                i++;
            }

            return false; 
        }

        private List<(int fila, int col, int grado)> ObtenerCandidatosOrdenados(
            int[,] terreno, int n, int filaActual, int colActual)
        {
            List<(int fila, int col, int grado)> candidatos = new();

            int v = 0;
            while (v < 8)
            {
                int nf = filaActual + DeltaFila[v];
                int nc = colActual + DeltaColumna[v];

                if (EsDentroDelTerreno(n, nf, nc) && terreno[nf, nc] == -1)
                {
                    int grado = CalcularGrado(terreno, n, nf, nc);
                    candidatos.Add((nf, nc, grado));
                }

                v++;
            }

            candidatos.Sort((a, b) => a.grado.CompareTo(b.grado));

            return candidatos;
        }

        private int CalcularGrado(int[,] terreno, int n, int fila, int col)
        {
            int grado = 0;
            int v = 0;

            while (v < 8)
            {
                int nf = fila + DeltaFila[v];
                int nc = col + DeltaColumna[v];

                if (EsDentroDelTerreno(n, nf, nc) && terreno[nf, nc] == -1)
                    grado++;

                v++;
            }

            return grado;
        }

        private int ContarAlcanzables(int n, int inicioX, int inicioY)
        {
            bool[,] visitado = new bool[n, n];
            Queue<(int, int)> cola = new();
            cola.Enqueue((inicioX, inicioY));
            visitado[inicioX, inicioY] = true;
            int contador = 1;

            while (cola.Count > 0)
            {
                (int fila, int col) = cola.Dequeue();

                int v = 0;
                while (v < 8)
                {
                    int nf = fila + DeltaFila[v];
                    int nc = col + DeltaColumna[v];

                    if (EsDentroDelTerreno(n, nf, nc) && !visitado[nf, nc])
                    {
                        visitado[nf, nc] = true;
                        cola.Enqueue((nf, nc));
                        contador++;
                    }

                    v++;
                }
            }

            return contador;
        }

        private bool EsDentroDelTerreno(int n, int fila, int col)
        {
            return fila >= 0 && fila < n && col >= 0 && col < n;
        }
    }
}
