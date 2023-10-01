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

            // Parsing à proprement parler

            List<KeyValuePair<int,string>> list = new List<KeyValuePair<int,string>>();

            // Pré-process de la réponse principale en sous chaines de clés (poids) / valeurs (coordonnées à parser)
            int last_cut = result.Length - 1;
            while (last_cut > 0)
            {
                for (int i = result.Length-1; i > 0; i--)
                {
                    if (result[i].Equals('&'))
                    {
                        result = result.Substring(i + 1);
                        last_cut = result.Length-1;
                        break;
                    }
                    else if (result[i].Equals('*'))
                    {
                        last_cut = i - 2;
                        list.Add(new KeyValuePair<int, string>(result[i-1],result.Substring(i+1,(i+1)-last_cut+1)));
                    }
                }
            }

            // Process des chaines de caractères en liste de coordonnées
            List<KeyValuePair<int, int[]>> coordinates = new List<KeyValuePair<int, int[]>>();
            foreach(var e in list)
            {
                int cursor = 0, x = 0, y = 0;
                for(int i = 0; i < e.Value.Length; i++)
                {
                    if (e.Value[i].Equals(':'))
                    {
                        x = Int32.Parse(e.Value.Substring(cursor, i - cursor));
                        cursor = i + 1;
                    }
                    else if (e.Value[i].Equals(' '))
                    {
                        y = Int32.Parse(e.Value.Substring(cursor, i - cursor));
                        cursor = i + 1;
                        int[] test = new int[]{1,2};
                        coordinates.Add(new KeyValuePair<int,int[]>(e.Key,new int[]{x,y}));
                    }
                    else if (e.Value[i].Equals('+'))
                    {
                        cursor = i + 1;
                    }
                }
            }
            
            return map;
        }
    }
}
