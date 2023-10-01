using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DofusGPS
{

    // Classe servant à parser une requête faite à Dofus Map
    // Est censé pouvoir en sortir une matrice
    internal class Parser
    {       
        public Parser(){}

        public Map Parse(string url)
        {
            // -------
            // url : url de l'API à contacter
            // -------

            // Init de la Map
            Map map = new Map();

            string result;

            using(var client = new HttpClient())
            {
                var uri = new Uri(url);
                // Lecture de la réponse 
                result = client.GetAsync(uri).Result.Content.ReadAsStringAsync().Result;
            }

            // On enlève le " à la fin
            result = result.Substring(0,result.Length - 2);

            // Parsing à proprement parler

            List<KeyValuePair<int,string>> list = new List<KeyValuePair<int,string>>();

            // Pré-process de la réponse principale en sous chaines de clés (poids) / valeurs (coordonnées à parser)
            int last_cut = result.Length - 1;
            
            // Première passe pour récupérer la chaine qui nous intéresse
            for(int i = result.Length-1; i > 0; i--)
            {
                if (result[i].Equals('&'))
                {
                    result = result.Substring(i + 1);
                    last_cut = result.Length - 1;
                    break;
                }
            }
            // Deuxième passe pour split en fonction du nombre
            for (int i = last_cut; i > 0; i--)
            {                    
                if (result[i].Equals('*'))
                {                   
                    list.Add(new KeyValuePair<int, string>(Int32.Parse(result[i-1].ToString()),result.Substring(i+1,last_cut-(i+1)+1)));
                    last_cut = i - 3;
                }
            }
            

            // Process des chaines de caractères en liste de coordonnées
            List<KeyValuePair<int, int[]>> coordinates = new List<KeyValuePair<int, int[]>>();
            foreach(var e in list)
            {
                int cursor = 0, x = 0, y = 0;
                for(int i = cursor; i < e.Value.Length; i++)
                {
                    if (e.Value[i].Equals(':'))
                    {
                        x = Int32.Parse(e.Value.Substring(cursor, i - cursor));
                        cursor = i + 1;
                    }
                    else if (e.Value[i].Equals(' ') || e.Value[i].Equals('+'))
                    {
                        y = Int32.Parse(e.Value.Substring(cursor, i - cursor));
                        cursor = i + 1;
                        coordinates.Add(new KeyValuePair<int,int[]>(e.Key,new int[]{x,y}));
                    }
                    else if (i + 1 >= e.Value.Length)
                    {
                        y = Int32.Parse(e.Value.Substring(cursor, e.Value.Length - cursor));
                        cursor = i + 1;
                        coordinates.Add(new KeyValuePair<int, int[]>(e.Key, new int[]{x,y}));
                    }
                }
            }
            
            // Ecriture de la map
            foreach(var e in coordinates)
            {
                map.setElementAt(e.Value[1], e.Value[0], e.Key);
            }

            return map;
        }
    }
}
