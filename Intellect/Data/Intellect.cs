namespace Intellectual.Data
{
    public static class Intellect
    {
        public const int Size = 3;
        public static Tuple<int, int> GetBestMoveCoord(in int[,] table)
        {
            int who;
            int roundNum;
            try
            {
                CheckTable(table);
                ReadState(table, out who, out roundNum);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return GetBestMoveCoord(table, who, roundNum);
        }

        private static void CheckTable(in int[,] table)
        {
            if (table.GetLength(0) != Size && table.GetLength(1) != Size)
            {
                throw new ArgumentException("Invalid table size");
            }

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (table[i, j] != 0 &&
                        table[i, j] != 1 &&
                        table[i, j] != 2)
                    {
                        throw new ArgumentException("Invalid value in table");
                    }
                }
            }
        }

        private static void ReadState(in int[,] table, out int who, out int roundNum)
        {
            int Xcount = 0;
            int Ocount = 0;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (table[i, j] == 1)
                        Xcount++;
                    else if (table[i, j] == 2)
                        Ocount++;
                }
            }
            if (Xcount == Ocount)
                who = 1;
            else if (Xcount > Ocount)
                who = 2;
            else
                throw new Exception("Invalid Game State");

            roundNum = Ocount + 1;
        }

        private static Tuple<int, int> GetBestMoveCoord(in int[,] table, int who, int roundNum)
        {
            /*if (who == 1)
            {
                switch (roundNum)
                {
                    case 1:
                        return XBestR1(table);
                    case 2:
                        return XBestR2(table);
                    case 3:
                        return XBestR3(table);
                    case 4:
                        return XBestR4(table);
                    case 5:
                        return XBestR5(table);
                }
            }
            else
            {
                switch (roundNum)
                {
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                }
            }*/

            int row = 0, col = 0;
            // Mock
            for (int i = 0; i < table.GetLength(0); i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    if (table[i, j] == 0)
                    {
                        row = i;
                        col = j;
                        goto end;
                    }
                }
            }
        //
        end:
            return Tuple.Create(row, col);
        }

        /*private static Tuple<int, int> XBestR1(in int[,] table)
        {
            return Tuple.Create(1, 1);
        }
        private static Tuple<int, int> XBestR2(in int[,] table)
        {
            Random random = new Random();
            int cornerСellNum;
            while (true)
            {
                cornerСellNum = random.Next(0, 4);
                if (cornerСellNum == 0 && table[0,0] == 0)
                    return Tuple.Create(0, 0);
                if (cornerСellNum == 1 && table[0, 2] == 0)
                    return Tuple.Create(0, 2);
                if (cornerСellNum == 2 && table[2, 0] == 0)
                    return Tuple.Create(2, 0);
                if (cornerСellNum == 3 && table[2, 2] == 0)
                    return Tuple.Create(2, 0);
            }
        }
        private static Tuple<int, int> XBestR3(in int[,] table)
        {
            if (table[0, 0] == 1 && table[2, 2] == 0)
                return Tuple.Create(2, 2);
        }*/
    }
}
