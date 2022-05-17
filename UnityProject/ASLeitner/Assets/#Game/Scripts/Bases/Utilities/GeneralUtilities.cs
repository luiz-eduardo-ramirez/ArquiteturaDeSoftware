using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Extensions.Utilites
{
    public static class GeneralUtilities
    {
        private static System.Random s_random = new System.Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int count = list.Count;
            while (count > 1)
            {
                count--;
                int randomIndex = s_random.Next(count + 1);
                T value = list[randomIndex];
                list[randomIndex] = list[count];
                list[count] = value;
            }
        }
        public static int CycleIndex(int index, int length)
        {
            if(length <= 0 || index < 0) { Debug.LogError("Divisao por zero!!!"); return 0; }
            return index - (length * (index / length));
        }
    }
}
