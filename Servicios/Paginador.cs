using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace inmobiliaria_Toloza_Lopez.Servicios
{
    public static class Paginador
    {

        //paginador generico
        //solo el esqueleto, falta mejorar 
        public static (IEnumerable<T> elementosPagina, int totalPaginas) PaginacionGenerica<T>(
            List<T> listaCompleta,
            int paginaActual,
            int elementosPorPagina)
        {
            if (paginaActual < 1)
            {
                paginaActual = 1;
            }

            int totalElementos = listaCompleta.Count;
            int totalPaginas = (int)Math.Ceiling((double)totalElementos / elementosPorPagina);

            if (paginaActual > totalPaginas)
            {
                paginaActual = totalPaginas;
            }

            int indiceInicial = (paginaActual - 1) * elementosPorPagina;
            var elementosPagina = listaCompleta.GetRange(indiceInicial, Math.Min(elementosPorPagina, totalElementos - indiceInicial));
            return (elementosPagina, totalPaginas);
        }
    }
}
