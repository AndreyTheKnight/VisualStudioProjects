using System.Collections;
using System.Text;

namespace Lab2
{
    public static class Desx
    {
        private struct Bits
        {
            public BitArray l;
            public BitArray r;
        }

        private static readonly int[] ipTable =
        {
            58, 50, 42, 34, 26, 18, 10,  2, 60, 52, 44, 36, 28, 20, 12,  4,
            62, 54, 46, 38, 30, 22, 14,  6, 64, 56, 48, 40, 32, 24, 16,  8,
            57, 49, 41, 33, 25, 17,  9,  1, 59, 51, 43, 35, 27, 19, 11,  3,
            61, 53, 45, 37, 29, 21, 13,  5, 63, 55, 47, 39, 31, 23, 15,  7,
        };
        private static readonly int[] ipReverseTable =
        {
            40,  8, 48, 16, 56, 24, 64, 32, 39,  7, 47, 15, 55, 23, 63, 31,
            38,  6, 46, 14, 54, 22, 62, 30, 37,  5, 45, 13, 53, 21, 61, 29,
            36,  4, 44, 12, 52, 20, 60, 28, 35,  3, 43, 11, 51, 19, 59, 27,
            34,  2, 42, 10, 50, 18, 58, 26, 33,  1, 41,  9, 49, 17, 57, 25,
        };
        private static readonly int[] eTable =
        {
            32,  1,  2,  3,  4,  5,
             4,  5,  6,  7,  8,  9,
             8,  9, 10, 11, 12, 13,
            12, 13, 14, 15, 16, 17,
            16, 17, 18, 19, 20, 21,
            20, 21, 22, 23, 24, 25,
            24, 25, 26, 27, 28, 29,
            28, 29, 30, 31, 32,  1
        };
        private static readonly int[,,] sTable =
        {
            {
                { 14,  4, 13,  1,  2, 15, 11,  8,  3, 10,  6, 12,  5,  9,  0,  7 },
                {  0, 15,  7,  4, 14,  2, 13,  1, 10,  6, 12, 11,  9,  5,  3,  8 },
                {  4,  1, 14,  8, 13,  6,  2, 11, 15, 12,  9,  7,  3, 10,  5,  0 },
                { 15, 12,  8,  2,  4,  9,  1,  7,  5, 11,  3, 14, 10,  0,  6, 13 }
            },
            {
                { 15,  1,  8, 14,  6, 11,  3,  4,  9,  7,  2, 13, 12,  0,  5, 10 },
                {  3, 13,  4,  7, 15,  2,  8, 14, 12,  0,  1, 10,  6,  9, 11,  5 },
                {  0, 14,  7, 11, 10,  4, 13,  1,  5,  8, 12,  6,  9,  3,  2, 15 },
                { 13,  8, 10,  1,  3, 15,  4,  2, 11,  6,  7, 12,  0,  5, 14,  9 }
            },
            {
                { 10,  0,  9, 14,  6,  3, 15,  5,  1, 13, 12,  7, 11,  4,  2,  8 },
                { 13,  7,  0,  9,  3,  4,  6, 10,  2,  8,  5, 14, 12, 11, 15,  1 },
                { 13,  6,  4,  9,  8, 15,  3,  0, 11,  1,  2, 12,  5, 10, 14,  7 },
                {  1, 10, 13,  0,  6,  9,  8,  7,  4, 15, 14,  3, 11,  5,  2, 12 }
            },
            {
                {  7, 13, 14,  3,  0,  6,  9, 10,  1,  2,  8,  5, 11, 12,  4, 15 },
                { 13,  8, 11,  5,  6, 15,  0,  3,  4,  7,  2, 12,  1, 10, 14,  9 },
                { 10,  6,  9,  0, 12, 11,  7, 13, 15,  1,  3, 14,  5,  2,  8,  4 },
                {  3, 15,  0,  6, 10,  1, 13,  8,  9,  4,  5, 11, 12,  7,  2, 14 }
            },
            {
                {  2, 12,  4,  1,  7, 10, 11,  6,  8,  5,  3, 15, 13,  0, 14,  9 },
                { 14, 11,  2, 12,  4,  7, 13,  1,  5,  0, 15, 10,  3,  9,  8,  6 },
                {  4,  2,  1, 11, 10, 13,  7,  8, 15,  9, 12,  5,  6,  3,  0, 14 },
                { 11,  8, 12,  7,  1, 14,  2, 13,  6, 15,  0,  9, 10,  4,  5,  3 }
            },
            {
                { 12,  1, 10, 15,  9,  2,  6,  8,  0, 13,  3,  4, 14,  7,  5, 11 },
                { 10, 15,  4,  2,  7, 12,  9,  5,  6,  1, 13, 14,  0, 11,  3,  8 },
                {  9, 14, 15,  5,  2,  8, 12,  3,  7,  0,  4, 10,  1, 13, 11,  6 },
                {  4,  3,  2, 12,  9,  5, 15, 10, 11, 14,  1,  7,  6,  0,  8, 13 }
            },
            {
                {  4, 11,  2, 14, 15,  0,  8, 13,  3, 12,  9,  7,  5, 10,  6,  1 },
                { 13,  0, 11,  7,  4,  9,  1, 10, 14,  3,  5, 12,  2, 15,  8,  6 },
                {  1,  4, 11, 13, 12,  3,  7, 14, 10, 15,  6,  8,  0,  5,  9,  2 },
                {  6, 11, 13,  8,  1,  4, 10,  7,  9,  5,  0, 15, 14,  2,  3, 12 }
            },
            {
                { 13,  2,  8,  4,  6, 15, 11,  1, 10,  9,  3, 14,  5,  0, 12,  7 },
                {  1, 15, 13,  8, 10,  3,  7,  4, 12,  5,  6, 11,  0, 14,  9,  2 },
                {  7, 11,  4,  1,  9, 12, 14,  2,  0,  6, 10, 13, 15,  3,  5,  8 },
                {  2,  1, 14,  7,  4, 10,  8, 13, 15, 12,  9,  0,  3,  5,  6, 11 }
            }
        };
        private static readonly int[] pTable =
        {
            16, 7,  20, 21, 29, 12, 28, 17,
            1,  15, 23, 26, 5,  18, 31, 10,
            2,  8,  24, 14, 32, 27, 3,  9,
            19, 13, 30, 6,  22, 11, 4,  25
        };
        private static readonly int[] pc1Table =
        {
            57, 49, 41, 33, 25, 17, 9,
            1,  58, 50, 42, 34, 26, 18,
            10, 2,  59, 51, 43, 35, 27,
            19, 11, 3,  60, 52, 44, 36,
            63, 55, 47, 39, 31, 23, 15,
            7,  62, 54, 46, 38, 30, 22,
            14, 6,  61, 53, 45, 37, 29,
            21, 13, 5,  28, 20, 12, 4
        };
        private static readonly int[] shiftLeftTable =
        {
            1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1
        };
        private static readonly int[] pc2Table =
        {
            14, 17, 11, 24, 1,  5,  3,  28, 15, 6,  21, 10, 23, 19, 12, 4,
            26, 8,  16, 7,  27, 20, 13, 2,  41, 52, 31, 37, 47, 55, 30, 40,
            51, 45, 33, 48, 44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32
        };

        private static Bits IpInversion(this BitArray blockBits)
        {
            var bits = new Bits
            {
                l = new BitArray(32),
                r = new BitArray(32)
            };
            for (var i = 0; i < 32; i++)
            {
                bits.l[i] = blockBits[ipTable[i] - 1];
                bits.r[i] = blockBits[ipTable[i + 32] - 1];
            }
            return bits;
        }
        private static BitArray IpReverseInversion(this Bits bits)
        {
            var blockBits = new BitArray(64);
            for (var i = 0; i < 64; i++)
                blockBits[i] = ((ipReverseTable[i] - 1) < 32) ? 
                    bits.l[ipReverseTable[i] - 1] : 
                    bits.r[ipReverseTable[i] - 1 - 32];
            return blockBits;
        }
        private static BitArray[] GenerateSubKeys(this BitArray key, bool encMode)
        {
            var subKeys = new BitArray[16];
            var preSubKey = new Bits()
            {
                l = new BitArray(28),
                r = new BitArray(28)
            };
            for (var i = 0; i < 28; i++)
            {
                preSubKey.l[i] = key[pc1Table[i] - 1];
                preSubKey.r[i] = key[pc1Table[i + 28] - 1];
            }
            for (var i = 0; i < 16; i++)
            {
                for (var j = 0; j < shiftLeftTable[i]; j++)
                {
                    var tmpL = preSubKey.l[0];
                    var tmpR = preSubKey.r[0];
                    for (var k = 1; k < 28; k++)
                    {
                        preSubKey.l[k - 1] = preSubKey.l[k];
                        preSubKey.r[k - 1] = preSubKey.r[k];
                    }
                    preSubKey.l[27] = tmpL;
                    preSubKey.r[27] = tmpR;
                }
                var idx = encMode ? i : 15 - i;
                subKeys[idx] = new BitArray(48);
                for (var j = 0; j < 48; j++)
                    subKeys[idx][j] = pc2Table[j] < 29 ?
                        preSubKey.l[pc2Table[j] - 1] :
                        preSubKey.r[pc2Table[j] - 29];
            }
            return subKeys;
        }
        private static Bits Rounds(this Bits bits, BitArray[] subKeys)
        {
            for (var i = 0; i < 16; i++)
            {
                var newLBits = bits.r;
                var newRBits = bits.l.Xor(FeistelFunc(bits.r, subKeys[i]));
                bits.l = newLBits;
                bits.r = newRBits;
            }
            var tmp = new BitArray(bits.l);
            bits.l = bits.r;
            bits.r = tmp;
            return bits;
        }
        private static BitArray FeistelFunc(BitArray bits, BitArray subKey)
        {
            var extBits = new BitArray(48);
            for (var i = 0; i < 48; i++)
                extBits[i] = bits[eTable[i] - 1];
            extBits = extBits.Xor(subKey);
            var newBits = new BitArray(32);
            for (var i = 0; i < 8; i++)
            {
                var sRowIdx = (extBits[i * 6].ToInt()) * 2 + (extBits[i * 6 + 5].ToInt());
                var sColIdx = (extBits[i * 6 + 1].ToInt()) * 8 + (extBits[i * 6 + 2].ToInt() * 4)
                    + (extBits[i * 6 + 3].ToInt()) * 2 + (extBits[i * 6 + 4].ToInt());
                for (int j = 3, sI = sTable[i, sRowIdx, sColIdx]; j >= 0; j--, sI /= 2)
                    newBits[i * 4 + j] = (sI % 2 == 1);
            }
            var result = new BitArray(32);
            for (var i = 0; i < result.Count; i++)
                result[i] = newBits[pTable[i] - 1];
            return result;
        }
        private static BitArray ToBits(this string str)
        {
            return new BitArray(Encoding.Default.GetBytes(str));
        }
        private static string ToStr(this BitArray bits)
        {
            var block = new byte[8];
            bits.CopyTo(block, 0);
            return Encoding.Default.GetString(block);
        }
        private static int ToInt(this bool bit)
        {
            return bit ? 1 : 0;
        }
        
        public static string Encrypt(this string plaintext, string key)
        {
            var result = new StringBuilder();
            var subKeys = key.Substring(0, 8).ToBits().GenerateSubKeys(true);
            var k1 = key.Substring(8, 8).ToBits();
            var k2 = key.Substring(16, 8).ToBits();
            while (plaintext.Length % 8 != 0)
                plaintext += " ";
            for (var i = 0; i < plaintext.Length; i += 8)
                result.Append(plaintext.Substring(i, 8)
                    .ToBits()
                    .Xor(k1)
                    .IpInversion()
                    .Rounds(subKeys)
                    .IpReverseInversion()
                    .Xor(k2)
                    .ToStr());
            return result.ToString();
        }
        public static string Decrypt(this string ciphertext, string key)
        {
            var result = new StringBuilder();
            var subKeys = key.Substring(0, 8).ToBits().GenerateSubKeys(false);
            var k1 = key.Substring(8, 8).ToBits();
            var k2 = key.Substring(16, 8).ToBits();
            while (ciphertext.Length % 8 != 0)
                ciphertext += " ";
            for (var i = 0; i < ciphertext.Length; i += 8)
                result.Append(ciphertext.Substring(i, 8)
                    .ToBits()
                    .Xor(k2)
                    .IpInversion()
                    .Rounds(subKeys)
                    .IpReverseInversion()
                    .Xor(k1)
                    .ToStr());
            return result.ToString();
        }
    }
}
