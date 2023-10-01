using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DofusGPS
{

    internal class DofusGPS
    {
        // Classe principale depuis laquelle seront faits tous les appels
        //
        // 1. 
        // 2.
        // 3. Récupération des données de Dofus Map et parsing
        // 4. Conversion en matrice pondérée
        // 5. Dijkstra
        static void Main(string[] args)
        {
            Parser parser = new Parser();
            Map map = parser.Parse("https://dofus-map.com/getRessourceData.php?ressourceId=36&groupId=0");
        }
    }
}
