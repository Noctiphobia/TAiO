using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAiO.Model
{
    /// <summary>
    /// Klasa opisująca jeden z wyników kroku algorytmu
    /// </summary>
    public class PartialSolution
    {
        /// <summary>
        /// Ostatnio dołożony klocek (zawierający wskaźnik do poprzedniego stanu planszy)
        /// </summary>
        public BlockInstance Move { get; set; }

        /// <summary>
        /// Wartość funkcji jakości położenie klocka
        /// </summary>
        public int Cost { get; set; }
    }
}
