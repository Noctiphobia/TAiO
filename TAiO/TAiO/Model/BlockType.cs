using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroMvvm;

namespace TAiO.Model
{
    /// <summary>
    /// Klasa reprezentująca typ klocka.
    /// Zawiera tablice kształtu klocka we wszystkich możliwych różnych obrotach
    /// oraz liczbę klocków, jakie mamy do dyspozycji
    /// </summary>
	public class BlockType:IComparable
    {
        /// <summary>
        /// Zmienna służąca do wyliczania nowego id dla nowego rodzaju klocka
        /// </summary>
	    private static int NextId = 0;
        /// <summary>
        /// Zmienna przechowująca id klocka (przypisywane przy tworzeniu instancji klocka)
        /// </summary>
		public readonly int Id;
        /// <summary>
        /// Wysokość klocka
        /// </summary>
		public int Height { get; set; }
        /// <summary>
        /// Szerokość klocka
        /// </summary>
		public int Width { get; set; }
        /// <summary>
        /// Lista tablic przechowujących możliwe kształty klocka (w możliwych rotacjach).
        /// Wszystkie poza pierwszą liczone są przy tworzeniu instancji klocka
        /// z pierwszego (oryginalnego, wczytanego) kształtu klocka.
        /// Pojedyncza tablica to tablica intów zawierająca zera i jedynki,
        /// zero oznacza brak klocka, jeden oznacza obecność (części) klocka
        /// </summary>
        public List<int[,]> Shape { get; set; }
        /// <summary>
        /// Liczba klocków tego typu, jakie mamy do dyspozycji
        /// </summary>
		public uint BlockNumber { get; set; }


        /// <summary>
        /// Konstruktor instancji klocka
        /// </summary>
        /// <param name="w">szerokość klocka</param>
        /// <param name="h">wysokość klocka</param>
        /// <param name="s">kształt klocka (w postaci zgodnej z opisem przy Shape)</param>
	    public BlockType(int w, int h, int[,] s)
	    {
		    Id = NextId;
		    NextId++;
	        Height = h;
	        Width = w;
		    Shape = new List<int[,]> {s};
		    CreateRotations90();
	        BlockNumber = 1;
	    }

        /// <summary>
        /// Funkcja tworząca kształty obróconego klocka (90*, 180*, 270*).
        /// Funkcja eliminuje duplikaty i upewnia się, że pierwszym kształtem (pierwszą tablicą)
        /// jest zawsze kształt "poziomy", tj. szerokość > wysokość
        /// </summary>
        private void CreateRotations90()
        {
            int[,] last = Shape[0];
            for (int i = 1; i < 4; i++)
            {
                last = Rotate90(last);
	            bool identical = false;
                for (int j = 0; j < i; j++)
	                if (CompareArrays(Shape[j], last)) // jeśli są takie same
	                {
		                identical = true;
		                break;
	                }
	            if(!identical)
					Shape.Add(last);
            }
	        if (Height > Width)		//zamieniamy, żeby zawsze był szerszy
	        {
		        int tmp = Height;
		        Height = Width;
		        Width = tmp;
				Shape.Add(Shape[0]);
				Shape.RemoveAt(0);
	        }
        }

        /// <summary>
        /// Funkcja obraca jedną pojedynczą tablicę o 90*
        /// i zwraca wynik
        /// </summary>
        /// <param name="t">tablica do obrócenia</param>
        /// <returns></returns>
        private int[,] Rotate90(int[,] t)
        {
            int[,] res = new int[t.GetLength(1), t.GetLength(0)];
            for (int i = 0; i < t.GetLength(0); i++)
                for (int j = 0; j < t.GetLength(1); j++)
                    res[t.GetLength(1) - 1 - j, i] = t[i, j];
            return res;
        }

        /// <summary>
        /// Funkcja porównująca dwie tablice (dwa kształty klocka, dwa obroty klocka)
        /// </summary>
        /// <param name="t1">pierwsza tablica</param>
        /// <param name="t2">druga tablica</param>
        /// <returns>czy tablice są identyczne</returns>
	    private bool CompareArrays(int[,] t1, int[,] t2)
        {
            if (t1.GetLength(0) != t2.GetLength(0) ||
                t1.GetLength(1) != t2.GetLength(1))
                return false;
            for (int i = 0; i < t1.GetLength(0); i++)
                for (int j = 0; j < t1.GetLength(1); j++)
                    if (t1[i, j] != t2[i, j])
                        return false;
            return true;
        }


        /// <summary>
        /// Funkcja porównująca na podstawie unikalnych id
        /// </summary>
        /// <param name="obj">obiekt porównywany</param>
        /// <returns>wynik porównania id</returns>
	    public int CompareTo(object obj)
	    {
		    BlockType bt = obj as BlockType;
		    if (bt == null)
		    {
			    return 1;
		    }
		    return this.Id.CompareTo(bt.Id);
	    }
        
        public override string ToString()
        {
            return "BT: Id = " + this.Id;
        }
    }
}
