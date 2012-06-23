using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    public static class UIDGen
    {
        private static ushort next = 0;
        private static Queue<ushort> heap = new Queue<ushort>();

        public static ushort Next()
        {
            if (heap.Count > 0)
                return heap.Dequeue();
            return next++;
        }

        public static void Recycle(ushort uid)
        {
            heap.Enqueue(uid);
        }
    }
}