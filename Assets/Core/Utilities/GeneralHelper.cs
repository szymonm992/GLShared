using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GLShared.General
{
    public static class GeneralHelper 
    {
        public static void QuickSortList<T>(this List<T> inputList, int left, int right)
            where T : IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T> //we only want number types so we add some constraints
        {
            if(left<right)
            {
                var pivot = inputList.PartitionInternalList(left, right);
                inputList.QuickSortList(0, pivot - 1);
                inputList.QuickSortList(pivot+1,right);
            }
        }

       
        public static void SwapElementsInList<T>(this List<T> inputList, int i, int j)
            where T : IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T> 
        {
            var temp = inputList[i];
            inputList[i] = inputList[j];
            inputList[j] = temp;
        }

        private static int PartitionInternalList<T>(this List<T> inputList, int left, int right)
             where T : IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            int index = left;
            T pivot = inputList[left];
            for(int i = left+1; i <= right; i++)
            {
                if(inputList[i].CompareTo(pivot) <= 0)
                {
                    index++;
                    inputList.SwapElementsInList(index, i);
                }
            }
            inputList.SwapElementsInList(index, left);
            return index;
        }

        public static float ClampAngle(this float angle, float min, float max)
        {
            angle = Mathf.Repeat(angle, 360);
            min = Mathf.Repeat(min, 360);
            max = Mathf.Repeat(max, 360);

            bool inverse = false;
            var tmin = min;
            var tangle = angle;

            if (min > 180)
            {
                inverse = !inverse;
                tmin -= 180;
            }
            if (angle > 180)
            {
                inverse = !inverse;
                tangle -= 180;
            }

            var result = !inverse ? tangle > tmin : tangle < tmin;
            if (!result)
            {
                angle = min;
            }

            inverse = false;
            tangle = angle;
            var tmax = max;

            if (angle > 180)
            {
                inverse = !inverse;
                tangle -= 180;
            }
            if (max > 180)
            {
                inverse = !inverse;
                tmax -= 180;
            }

            result = !inverse ? tangle < tmax : tangle > tmax;
            if (!result)
            {
                angle = max;
            }
            return angle;
        }

        public static bool InRange(this float input, float min, float max)
        {
            return input >= min && input <= max;
        }

        public static bool IsInLayerMask(this int layer, LayerMask layermask)
        {
            return layermask == (layermask | (1 << layer));
        }

        public static long GenerateTimestamp()
        {
            DateTime epoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)((DateTime.UtcNow - epoch).TotalMilliseconds);
        }

    }
}
