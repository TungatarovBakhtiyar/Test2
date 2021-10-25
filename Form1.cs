using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace Cryptography
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string cipher = "Binary";
        string Vkey = "LEMON",Key = "";
        int Ckey = 3, row, col;
        string message, Valphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string ValphabetT = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
        //string Valphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz123456789!?,.()[]{}/*-+ "; //For Vigener
        string[] tabulaRecta;
        string Ucirilica = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";//For Caesar
        string Lcirilica = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";//For Caesar
        string UcirilicaT = "АБВГДЕЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        int[] WhiteSpaceIndexMas = new int[1];
        string JposMas ;
        string[] table;
        int[] gamma = new int[1], Displacement = { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };
        string DES_Key = "";
        int p = 0, g = 3, x = 0, b = 0, n, e, d;
        int A, B;//Diffi-Hellman;
        Aes AES_Alg;
        byte[] encryptedByte;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private bool PrimeNumber(int n)
        {
            bool res = true;
            for(int i=2;i<= (int)Math.Sqrt(n) + 2; i++)
            {
                if (n % i == 0)
                {
                    res = false;
                    break;
                }
            }
            return res;
        }

        private bool LinSearch(char[,] mat,char find,int r,int coll,ref int indI,ref int indJ)
        {
            bool result = false;
            for(int i = 0; i < r; i++)
            {
                for(int j = 0; j < coll; j++)
                {
                    if (mat[i, j] == find)
                    {
                        indI = i;
                        indJ = j;
                        result = true;
                    }
                }
                if (result)
                {
                    break;
                }
            }
            return result;
        }

        private int LinSearchInt(int[] mat, int dim, int el )
        {
            for (int i = 0; i < dim; i++)
            {
                if (mat[i] == el) {
                    return i;
                }
            }
            return -1;
        }

        public bool check(string s, char c)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == c)
                {
                    return true;
                }
            }
            return false;
        }

        private bool checkInt(int[] mas,int num,int len)
        {
                for(int i=0;i< len; i++)
                {
                    if (mas[i] == num)
                    {
                    return true;
                    }
                }
            return false;
        }

        public string UnStringConstructor(string key, int type)
        {
            key = key.ToUpper();
            string[] w = key.Split(' ');
            key = "";
            for(int i = 0; i < w.Length; i++)
            {
                key += w[i];
            }
            switch (type) {
                case 0:
                    {
                        key += ValphabetT;
                        break;
                    }
                case 1:
                    {
                        key += UcirilicaT;
                        break;
                    }
                case 3:
                    {
                        key += Valphabet;
                        break;
                    }
                 }
            string result = "";
            for (int i = 0; i < key.Length; i++)
            {
                if (!check(result, key[i]))
                {
                    result += key[i];
                }
            }
            return result;
        }

        private char[,] MatrixConstructor(string k,int t)
        {
            string US = UnStringConstructor(k,t);
            char[,] result ;
            switch (t)
            {
                case 0:
                    {
                        result = new char[5, 5];
                        for (int i = 0; i < 5; i++)
                        {
                            for (int j = 0; j < 5; j++)
                            {
                                try
                                {   
                                    result[i, j] = US[i * 5 + j];
                                    log.Text += result[i, j] + " ";
                                }
                                catch (IndexOutOfRangeException)
                                {
                                    log.Text += "IndexOutofRange in MatrixConstruct";
                                }
                            }
                            log.Text += Environment.NewLine;
                        }
                        break;
                    }
                case 1:
                    {
                        result = new char[4, 8];
                        for (int i = 0; i < 4; i++)
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                try
                                {
                                    result[i, j] = US[i * 8 + j];
                                    log.Text += result[i, j] + " ";
                                }
                                catch (IndexOutOfRangeException)
                                {
                                    log.Text += "IndexOutofRange in MatrixConstruct";
                                }
                            }
                            log.Text += Environment.NewLine;
                        }
                        break;
                    }
                default:
                    {
                        log.Text += " Wrong branch of code " + Environment.NewLine;
                        result = new char[5, 5];
                        for (int i = 0; i < 5; i++)
                        {
                            for (int j = 0; j < 5; j++)
                            {
                                try
                                {
                                    result[i, j] = US[i * 5 + j];
                                    log.Text += result[i, j] + " ";
                                }
                                catch (IndexOutOfRangeException)
                                {
                                    log.Text += "IndexOutofRange in MatrixConstruct";
                                }
                            }
                            log.Text += Environment.NewLine;
                        }
                        break;
                    }
            }
            return result;
        }

        private bool Trisemus(string message, string key, ref string result, int op)
        {
            result = "";
            string t = message.ToUpper(), temp = "";
            int type = 0;
            char[,] table;
            key = key.ToUpper();
            for (int i = 0; i < key.Length; i++)
            {
                if (key[i] == 'J')
                {
                    temp += 'I';
                }
                else
                {
                    temp += key[i];
                }
            }
            key = temp;
            temp = "";
            for (int i = 0; i < t.Length; i++)
            {
                if (t[i] == 'J')
                {
                    temp += 'I';
                }
                else
                {
                    temp += t[i];
                }
            }
            t = temp;
            if (ValphabetT.IndexOf(key[0]) >= 0) {
                type = 0;
            }
            else if (UcirilicaT.IndexOf(key[0]) >= 0)
            {
                type = 1;
            }
            else
            {
                log.Text += "Wrong key !" + Environment.NewLine;
                return false;
            }
            switch (op)
            {
                case 0:
                    {
                        switch (type)
                        {
                            case 0:
                                {
                                    table = new char[5, 5];
                                    table = MatrixConstructor(key, type);
                                    for (int i = 0; i < t.Length; i++)
                                    {
                                        int ei = 0, ej = 0;
                                        if (LinSearch(table, t[i], 5, 5, ref ei, ref ej))
                                        {
                                            if (ei == 4)
                                            {
                                                ei = 0;
                                            }
                                            else
                                            {
                                                ei += 1;
                                            }
                                            result += table[ei, ej];
                                        }
                                        else
                                        {
                                            result += t[i];
                                            log.Text += "Message[" + i + "] not founded(Encryption) " + Environment.NewLine;
                                        }
                                    }
                                    break;
                                }
                            case 1:
                                {
                                    table = new char[4, 8];
                                    table = MatrixConstructor(key, type);
                                    for (int i = 0; i < t.Length; i++)
                                    {
                                        int ei = 0, ej = 0;
                                        if (LinSearch(table, t[i], 4, 8, ref ei, ref ej))
                                        {
                                            if (ei == 3)
                                            {
                                                ei = 0;
                                            }
                                            else
                                            {
                                                ei += 1;
                                            }
                                            result += table[ei, ej];
                                        }
                                        else
                                        {
                                            result += t[i];
                                            log.Text += "Message[" + i + "] not founded (Encryption)" + Environment.NewLine;
                                        }
                                    }
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        break;
                    }
                case 1:
                    {
                        switch (type)
                        {
                            case 0:
                                {
                                    table = new char[5, 5];
                                    table = MatrixConstructor(key, type);
                                    for (int i = 0; i < t.Length; i++)
                                    {
                                        int ei = 0, ej = 0;
                                        if (LinSearch(table, t[i], 5, 5, ref ei, ref ej))
                                        {
                                            if (ei == 0)
                                            {
                                                ei = 4;
                                            }
                                            else
                                            {
                                                ei -= 1;
                                            }
                                            result += table[ei, ej];
                                        }
                                        else
                                        {
                                            result += t[i];
                                            log.Text += "Message[" + i + "] not founded(Decryption) " + Environment.NewLine;
                                        }
                                    }
                                    break;
                                }
                            case 1:
                                {
                                    table = new char[4, 8];
                                    table = MatrixConstructor(key, type);
                                    for (int i = 0; i < t.Length; i++)
                                    {
                                        int ei = 0, ej = 0;
                                        if (LinSearch(table, t[i], 4, 8, ref ei, ref ej))
                                        {
                                            if (ei == 0)
                                            {
                                                ei = 3;
                                            }
                                            else
                                            {
                                                ei -= 1;
                                            }
                                            result += table[ei, ej];
                                        }
                                        else
                                        {
                                            result += t[i];
                                            log.Text += "Message[" + i + "] not founded (Decryption)" + Environment.NewLine;
                                        }
                                    }
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        log.Text += "Wrong operation" + Environment.NewLine;
                        return false;
                    }
            }
            return true;
        }

        private bool magicSquare(string message,ref string result,int dim,int op)
        {
            int[,] magic = new int[dim,dim];
            char[,] magicCp = new char[dim, dim];
              switch (dim)
                 {
                    case 3:
                        {
                        magic[0, 0] = 4;magic[0, 1] = 9;magic[0, 2] = 2;
                        magic[1, 0] = 3;magic[1, 1] = 5;magic[1, 2] = 7;
                        magic[2, 0] = 8;magic[2, 1] = 1;magic[2, 2] = 6;
                            break;
                        }
                    case 4:
                        {
                        magic[0, 0] = 7; magic[0, 1] = 12; magic[0, 2] = 1;magic[0, 3] = 14;
                        magic[1, 0] = 2; magic[1, 1] = 13; magic[1, 2] = 8;magic[1, 3] = 11;
                        magic[2, 0] = 16; magic[2, 1] = 3; magic[2, 2] = 10;magic[2, 3] = 5;
                        magic[3, 0] = 9; magic[3, 1] = 6; magic[3, 2] = 15; magic[3, 3] = 4;
                        break;
                        }
                    case 5:
                        {
                            break;
                        }
                    case 6:
                        {
                            break;
                        }
                     default:
                        {
                          return false;
                        }   
                }
            switch (op)
            {
                case 0:
                    {
                        for (int i = 0; i < dim; i++)
                        {
                            for (int j = 0; j < dim; j++)
                            {
                                magicCp[i, j] = message[magic[i, j] - 1];
                                result += magicCp[i, j];
                            }
                            result += " ";
                        }
                        break;
                    }
                case 1:
                    {
                        char[] resChar = new char[dim*dim];
                        for(int i = 0; i < dim;i++) {
                            for(int j=0;j< dim; j++)
                            {
                                magicCp[i, j] = message[i * dim + j];
                            }
                        }
                        char[,] sorted = new char[dim, dim];
                        //Recording table to log
                        log.Text += "Encrypted message: " + Environment.NewLine;
                        for (int i = 0; i < dim; i++)
                        {
                            for (int j = 0; j < dim; j++)
                            {
                                log.Text += magicCp[i, j] + " ";
                            }
                            log.Text += Environment.NewLine;
                        }
                        //Recording magic to log
                        log.Text += "Magic square:" + Environment.NewLine;
                        for (int i = 0; i < dim; i++)
                        {
                            for (int j = 0; j < dim; j++)
                            {
                                resChar[magic[i, j] - 1] = magicCp[i, j];//resChar - decrypted message without whitespaces 
                                log.Text += magic[i, j] + " ";
                            }
                            log.Text += Environment.NewLine;
                        }
                        int count = 0;
                        for (int i = 0; i < dim*dim; i++)
                        {
                                result += resChar[i];
                                if (count < WhiteSpaceIndexMas.Length && result.Length == WhiteSpaceIndexMas[count])
                                {
                                    result += ' ';
                                    count++;
                                }
                        }
                        break;
                    }
                default:
                    {
                        return false;
                    }
            }
            return true;
        }

        private bool Playfer(string message,string key,ref string result,int op)
        {
            result = "";
            string t = message.ToUpper(), temp = "";
            string[] w = t.Split(' ');
            int type = 0, r, c;
            char[,] table;
            key = key.ToUpper();
            for (int i = 0; i < key.Length; i++)
            {
                if (key[i] == 'J')
                {
                    temp += 'I';
                }
                else
                {
                    temp += key[i];
                }
            }
            key = temp;
            temp = "";
            for (int i = 0; i < t.Length; i++)
            {
                if (t[i] == 'J')
                {
                    temp += 'I';
                }
                else
                {
                    temp += t[i];
                }
            }
            t = temp;
            if (ValphabetT.IndexOf(key[0]) >= 0)
            {
                type = 0;
            }
            else if (UcirilicaT.IndexOf(key[0]) >= 0)
            {
                type = 1;
            }
            else
            {
                log.Text += "Wrong key !" + Environment.NewLine;
                return false;
            }
            if (type == 0)
            {
                table = new char[5, 5];
                r = 5; c = 5;
                table = MatrixConstructor(key, type);
            }
            else
            {
                table = new char[4, 8];
                r = 4; c = 8;
                table = MatrixConstructor(key, type);
            }
            switch (op)
            {
                case 0:
                    {
                        int SpaceNumber = 0;
                        log.Text += "Whitespace indeces:" + Environment.NewLine;
                        for (int i = 0; i < t.Length; i++)
                        {
                            if (SpaceNumber >= WhiteSpaceIndexMas.Length)
                            {
                                Array.Resize<int>(ref WhiteSpaceIndexMas, WhiteSpaceIndexMas.Length + 1);
                            }
                            if (t[i] == ' ')
                            {
                                WhiteSpaceIndexMas[SpaceNumber] = i;
                                log.Text += i + " ";
                                SpaceNumber++;
                            }
                        }
                        t = "";
                        for (int i = 0; i < w.Length; i++)
                        {
                            t += w[i];
                        }
                        if (t.Length % 2 == 1)
                        {
                            log.Text += "Length of string is odd ! " + t.Length + Environment.NewLine;
                            log.Text += "String is: " + t + Environment.NewLine;
                            return false;
                        }
                        int count = 0, n = t.Length / 2;
                        while (count < n)
                        {
                            char c1 = t[count * 2], c2 = t[count * 2 + 1], nc1, nc2;
                            int i1 = 0, j1 = 0, i2 = 0, j2 = 0;
                            if (LinSearch(table, c1, 5, 5, ref i1, ref j1) && LinSearch(table, c2, 5, 5, ref i2, ref j2))
                            {
                                if (i1 == i2)
                                {
                                    if (j1 == c - 1)
                                    {
                                        nc1 = table[i1, 0];
                                    }
                                    else
                                    {
                                        nc1 = table[i1, j1 + 1];
                                    }
                                    if (j2 == c - 1)
                                    {
                                        nc2 = table[i2, 0];
                                    }
                                    else
                                    {
                                        nc2 = table[i2, j2+1];
                                    }
                                    
                                }
                                else if (j1 == j2)
                                {
                                    if (i1 == r - 1)
                                    {
                                        nc1 = table[0, j1];
                                    }
                                    else
                                    {
                                        nc1 = table[i1+1, j1];
                                    }
                                    if (i2 == r - 1)
                                    {
                                        nc2 = table[0, j2];
                                    }
                                    else
                                    {
                                        nc2 = table[i2+1, j2];
                                    }
                                }
                                else
                                {
                                    nc1 = table[i1, j2];
                                    nc2 = table[i2, j1];
                                }
                                if (count != n - 1)
                                {
                                    result += nc1 + "" + nc2 + " ";
                                    log.Text += nc1 + "" + nc2 + " ";
                                }
                                else
                                {
                                    result += nc1 + "" + nc2;
                                    log.Text += nc1 + "" + nc2 + Environment.NewLine;
                                }
                                count++;
                            }
                            else
                            {
                                log.Text += "One or more symbols not founded !" + Environment.NewLine;
                                return false;
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        int count = 0;
                        for (int i = 0; i < w.Length; i++)
                        {
                            char c1 = w[i][0], c2 = w[i][1], nc1, nc2;
                            int i1 = 0, j1 = 0, i2 = 0, j2 = 0;
                            if (LinSearch(table, c1, 5, 5, ref i1, ref j1) && LinSearch(table, c2, 5, 5, ref i2, ref j2))
                            {
                                if (i1 == i2)
                                {
                                    if (j1 == 0)
                                    {
                                        nc1 = table[i1, c - 1];
                                    }
                                    else
                                    {
                                        nc1 = table[i1, j1 - 1];
                                    }
                                    if (j2 == 0)
                                    {
                                        nc2 = table[i2, c - 1];
                                    }
                                    else
                                    {
                                        nc2 = table[i2, j2 - 1];
                                    }

                                }
                                else if (j1 == j2)
                                {
                                    if (i1 == 0)
                                    {
                                        nc1 = table[r - 1, j1];
                                    }
                                    else
                                    {
                                        nc1 = table[i1 - 1, j1];
                                    }
                                    if (i2 == 0)
                                    {
                                        nc2 = table[r - 1, j2];
                                    }
                                    else
                                    {
                                        nc2 = table[i2 - 1, j2];
                                    }
                                }
                                else
                                {
                                    nc1 = table[i1, j2];
                                    nc2 = table[i2, j1];
                                }

                                try
                                {
                                    if (result.Length == WhiteSpaceIndexMas[count])
                                    {
                                        result += ' ';
                                        count++;
                                    }
                                    result += nc1;
                                    if (result.Length == WhiteSpaceIndexMas[count])
                                    {
                                        result += ' ';
                                        count++;
                                    }
                                    result += nc2;
                                }
                                catch (IndexOutOfRangeException)
                                {
                                    log.Text += "IndexOutOfRange in Widston(Decryption)";
                                    log.Text += "WhiteSpaceIndexMas.Length = " + WhiteSpaceIndexMas.Length + "When index = " + count + Environment.NewLine;
                                }
                                log.Text += nc1 + "" + nc2 + " ";
                            }
                        }
                        break;
                    }
            }
            return true;
        }

        
        private bool  Gronsfeld(string message,string key, ref string result,int op)
        {
            string Gkey = "";
            while (Gkey.Length <= message.Length)
            {
                Gkey += key;
            }
            switch (op)
            {
                case 0:
                    {
                        for (int i = 0; i < message.Length; i++)
                        {
                            if (Valphabet.IndexOf(message[i]) != -1)
                            {
                                string num = "";
                                num += Gkey[i];
                                int k;
                                if (Int32.TryParse(num, out k))
                                {
                                    log.Text += k + " ";
                                    int ind = (Valphabet.IndexOf(message[i]) + k)%26;
                                    try
                                    {
                                        result += Valphabet[ind];
                                    }
                                    catch (IndexOutOfRangeException)
                                    {
                                        log.Text += "IndexOutOfRange in Valphabet(Grosfeld) Encryption !" + Environment.NewLine;
                                        return false;
                                    }
                                }
                                else
                                {
                                    log.Text += "Can't parse string to int" + Environment.NewLine;
                                }
                            }
                            else
                            {
                                result += message[i];
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        for (int i = 0; i < message.Length; i++)
                        {
                            if (Valphabet.IndexOf(message[i]) != -1)
                            {
                                string num = "";
                                num += Gkey[i];
                                int k;
                                if (Int32.TryParse(num, out k))
                                {
                                    log.Text += k + " ";
                                    int ind = (Valphabet.IndexOf(message[i]) - k);
                                    if (ind < 0)
                                    {
                                        ind += 26;
                                    }
                                    try
                                    {
                                        result += Valphabet[ind];
                                    }
                                    catch (IndexOutOfRangeException)
                                    {
                                        log.Text += "IndexOutOfRange in Valphabet(Grosfeld) Decryption !" + Environment.NewLine;
                                        return false;
                                    }
                                }else
                                {
                                    log.Text += "Can't parse "+ num +" string to int" + Environment.NewLine;
                                }
                            }
                            else
                            {
                                result += message[i];
                            }
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return true;
        }

        private bool Caesar(string s, int key, ref string res, int op) {
            res = "";
            if (op == 0)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    char c;
                    if (s[i] >= 'A' && s[i] <= 'Z')
                    {
                        int num = (Convert.ToInt32(s[i]) - 65 + key) % 26;
                        c = Convert.ToChar(num + 65);
                        res += c;
                    }
                    else if (s[i] >= 'a' && s[i] <= 'z')
                    {
                        int num = (Convert.ToInt32(s[i]) - 97 + key) % 26;
                        c = Convert.ToChar(num + 97);
                        res += c;
                    }
                    else if (s[i] >= '0' && s[i] <= '9')
                    {
                        int num = (Convert.ToInt32(s[i]) + key) % 58;
                        if (num < 48)
                        {
                            num += 48;
                        }
                        c = Convert.ToChar(num);
                        res += c;
                    }
                    else if (Lcirilica.IndexOf(s[i]) != -1)
                    {
                        int pos = Lcirilica.IndexOf(s[i]);
                        c = Lcirilica[((pos + key) % 33)];
                        res += c;
                    }
                    else if (Ucirilica.IndexOf(s[i]) != -1)
                    {
                        int pos = Ucirilica.IndexOf(s[i]);
                        c = Ucirilica[((pos + key) % 33)];
                        res += c;
                    }
                    else
                    {
                        res += s[i];
                    }
                }
            }
            else if (op == 1) {
                for (int i = 0; i < s.Length; i++)
                {
                    char c;
                    if (s[i] >= 'A' && s[i] <= 'Z')
                    {
                        int num = Convert.ToInt32(s[i]) - 65 - key;
                        if (num < 0)
                        {
                            num = num + 91;
                        }
                        else {
                            num += 65;
                        }
                        c = Convert.ToChar(num);
                        res += c;
                    }
                    else if (s[i] >= 'a' && s[i] <= 'z')
                    {
                        int num = Convert.ToInt32(s[i]) - 97 - key;
                        if (num < 0)
                        {
                            num = num + 123;
                        }
                        else {
                            num += 97;
                        }
                        c = Convert.ToChar(num);
                        res += c;
                    }
                    else if (s[i] >= '0' && s[i] <= '9')
                    {
                        int num = Convert.ToInt32(s[i]) - key - 48;
                        if (num < 0)
                        {
                            num = num += 58;
                        }
                        else {

                        }
                        c = Convert.ToChar(num);
                        res += c;
                    }
                    else if (Lcirilica.IndexOf(s[i]) != -1)
                    {
                        int pos = Lcirilica.IndexOf(s[i]);
                        pos = pos - key;
                        while (pos < 0)
                        {
                            pos = pos + 33;
                        }
                        c = Lcirilica[pos];
                        res += c;
                    }
                    else if (Ucirilica.IndexOf(s[i]) != -1)
                    {
                        int pos = Ucirilica.IndexOf(s[i]);
                        pos -= key;
                        while (pos < 0)
                        {
                            pos = pos + 33;
                        }
                        c = Ucirilica[pos];
                        res += c;
                    }
                    else
                    {
                        res += s[i];
                    }
                }
            }
            else {
                log.Text += "Wrong code of operation " + op + " , it must be either 0 or 1." + Environment.NewLine;
            }
            log.Text += "Generated Caesar Key = " + key + Environment.NewLine;
            return true;
        }

        private bool CWK(string message,string key,int num,ref string result,int op)
        {
            string a = UnStringConstructor(key,3);
            log.Text += a + Environment.NewLine;
            string t = message.ToUpper();
            string alp;
            char[] temp = a.ToCharArray(),alphabet = new char[a.Length];
            for(int i = 0; i < a.Length; i++)
            {
                try {
                    alphabet[i] = temp[(i - num+26)%26];
                }
                catch (IndexOutOfRangeException)
                {
                    alphabet[i] = temp[i - num + 26];
                    log.Text += "OutOfRangeException in CWK alphabet" + Environment.NewLine;
                }
            }
            alp = "";
            for(int i =0;i<alphabet.Length; i++)
            {
                alp += alphabet[i];
            }
            log.Text += alp+Environment.NewLine;
            log.Text += t + Environment.NewLine;
            switch (op)
            {
                case 0:
                    {
                        for(int i = 0; i < t.Length; i++)
                        {
                            try {
                                result+= alp[Valphabet.IndexOf(t[i])];
                                log.Text += Valphabet.IndexOf(t[i]) + Environment.NewLine;
                            }
                            catch (IndexOutOfRangeException)
                            {
                                log.Text += "OutOfRange in " + i + Environment.NewLine;
                                result += t[i];
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        for (int i = 0; i < t.Length; i++)
                        {
                            try {
                                result += Valphabet[alp.IndexOf(t[i])];
                                    }
                            catch (IndexOutOfRangeException)
                            {
                                result += t[i];
                            }
                        }
                        break;
                    }
            }
            return true;
        }

        private char[,] PSquare()
        {
            char[,] result = new char[5, 5];
            int count = 0;
            for(int i = 0; i < 5; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    if (Valphabet.Length>count&& Valphabet[count] != 'J')
                    {
                        result[i, j]= Valphabet[count];
                        log.Text += result[i, j] + " ";
                        count++;
                    }
                    else
                    {
                        count++;
                        result[i, j] = Valphabet[count];
                        log.Text += result[i, j] + " ";
                    }
                }
                log.Text += Environment.NewLine;
            }
            return result;
        }

        private bool Polibian(string message,ref string result,int op)
        {
            char[,] pol = new char[5, 5];
            string nMes = message.ToUpper(),temp = "";
            JposMas = "";
            pol = PSquare();
            for(int i = 0; i < message.Length; i++)
            {
                if (nMes[i] == 'J')
                {
                    temp += 'I';
                }
                else
                {
                    temp += nMes[i];
                }
            }
            log.Text += "Jpos = " + JposMas + Environment.NewLine;
            nMes = temp;
            switch (op)
            {
                case 0:
                    {
                        for(int i = 0; i < nMes.Length; i++)
                        {
                            int indI = 0,indJ = 0;
                            if (LinSearch(pol,nMes[i],5,5,ref indI,ref indJ))
                            {
                                char nc;
                                if (indI < 4)
                                {
                                    nc = pol[indI + 1, indJ];
                                }
                                else
                                {
                                    nc = pol[0, indJ];
                                }
                                result += nc;
                            }
                            else
                            {
                                result += nMes[i];
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        for (int i = 0; i < nMes.Length; i++)
                        {
                            int indI = 0, indJ = 0;
                            if (LinSearch(pol, nMes[i],5,5, ref indI, ref indJ))
                            {
                                char nc;
                                if (indI != 0)
                                {
                                    nc = pol[indI - 1, indJ];
                                }
                                else
                                {
                                    nc = pol[4, indJ];
                                }
                                result += nc;
                            }
                            else
                            {
                                result += nMes[i];
                            }
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return true;
        }

        private bool Witston(string mes,string key, ref string result,int op)
        {
            result = "";
            string t = mes.ToUpper();
            string[] w = t.Split(' '),ks;
            key = key.ToUpper();
            ks = key.Split(' ');
            string key1 = ks[0], key2 = ks[1];
            char[,] table1 = new char[5, 5], table2 = new char[5, 5];
            table1 = MatrixConstructor(key1, 0);
            table2 = MatrixConstructor(key2, 0);
            switch (op)
            {
                case 0:
                    {
                        int SpaceNumber = 0;
                        log.Text += "Whitespace indeces:" + Environment.NewLine;
                        for(int i = 0; i < t.Length; i++)
                        {
                            if (SpaceNumber >= WhiteSpaceIndexMas.Length)
                            {
                                Array.Resize<int>(ref WhiteSpaceIndexMas, WhiteSpaceIndexMas.Length + 1);
                            }
                            if(t[i]==' ')
                            {
                                WhiteSpaceIndexMas[SpaceNumber] = i;
                                log.Text += i + " ";
                                SpaceNumber++;
                            }
                        }
                        t = "";
                        for (int i = 0; i < w.Length; i++)
                        {
                            t += w[i];
                        }
                        if (t.Length % 2 == 1)
                        {
                            log.Text += "Length of string is odd ! "+ t.Length  + Environment.NewLine;
                            log.Text += "String is: " + t + Environment.NewLine;
                            return false;
                        }
                        int count = 0, n = t.Length / 2;
                        while (count < n)
                        {
                            char c1 = t[count * 2], c2 = t[count * 2 + 1], nc1, nc2;
                            int i1 = 0, j1 = 0, i2 = 0, j2 = 0;
                            if(LinSearch(table1,c1,5,5,ref i1,ref j1)&&LinSearch(table2,c2,5,5,ref i2,ref j2))
                            {
                                if (i1 == i2)
                                {
                                    nc1 = table1[i1, j2];
                                    nc2 = table2[i2, j1];
                                }
                                else
                                {
                                    nc1 = table1[i2, j1];
                                    nc2 = table2[i1, j2];
                                }
                                if (count != n - 1)
                                {
                                    result += nc1 + "" + nc2 + " ";
                                    log.Text += nc1 + "" + nc2 + " ";
                                }
                                else
                                {
                                    result += nc1 + "" + nc2;
                                    log.Text += nc1 + "" + nc2 + Environment.NewLine;
                                }
                                count++;
                            }
                            else
                            {
                                log.Text += "One or more symbols not founded !" + Environment.NewLine;
                                return false;
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        int count = 0;
                        for(int i = 0; i < w.Length; i++)
                        {
                            char c1 = w[i][0], c2 = w[i][1], nc1, nc2;
                            int i1 = 0, j1 = 0, i2 = 0, j2 = 0;
                            if (LinSearch(table1, c1, 5, 5, ref i1, ref j1) && LinSearch(table2, c2, 5, 5, ref i2, ref j2))
                            {
                                if (i1 == i2)
                                {
                                    nc1 = table1[i1, j2];
                                    nc2 = table2[i2, j1];
                                }
                                else
                                {
                                    nc1 = table1[i2, j1];
                                    nc2 = table2[i1, j2];
                                }
                                try {
                                    if (result.Length == WhiteSpaceIndexMas[count])
                                    {
                                        result += ' ';
                                        count++;
                                    }
                                    result += nc1;
                                    if (result.Length == WhiteSpaceIndexMas[count])
                                    {
                                        result += ' ';
                                        count++;
                                    }
                                    result += nc2;
                                }
                                catch (IndexOutOfRangeException)
                                {
                                    log.Text += "IndexOutOfRange in Widston(Decryption)";
                                    log.Text += "WhiteSpaceIndexMas.Length = " + WhiteSpaceIndexMas.Length + "When index = " + count + Environment.NewLine;
                                }
                                log.Text += nc1 + "" + nc2 + " ";
                            }
                            else
                            {
                                log.Text += "One or more symbols not founded !" + Environment.NewLine;
                                return false;
                            }
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return true;
        }

        private string xor(string a,string b)
        {
            string result = "";
            for (int j = 0; j < a.Length; j++)
            {
                if (a[j] == '0')
                {
                    if (b[j] == '0')
                    {
                        result += '0';
                    }
                    else
                    {
                        result += '1';
                    }
                }
                else
                {
                    if (b[j] == '1')
                    {
                        result += '0';
                    }
                    else
                    {
                        result += '1';
                    }
                }
            }
            return result;
        }

        private bool XOR(string Message, string Xkey, ref string result)
        {
            result = "";
            string XOR_Key = "";
            int i = 0;
            while (XOR_Key.Length < Message.Length)
            {
                XOR_Key += Xkey;
            }
            while (i < Message.Length)
            {
                log.Text += "Encrypting " + i + Environment.NewLine;
                try
                {
                    if (Message[i] == ' ')
                    {
                        log.Text += "XOR(Encryption): whitespace in " + i + Environment.NewLine;
                        result += Message[i];
                    }
                
                   
                    else {
                        int num = (int)Message[i];
                        int knum = (int)XOR_Key[i];
                        string res = "";
                        string temp = Convert.ToString(num, 2).PadLeft(7, '0');
                        log.Text += "Binary string for " + Message[i] + " is " + temp + Environment.NewLine;
                        string kbyte = Convert.ToString(knum, 2).PadLeft(7, '0');
                        log.Text += "Binary string for key " + XOR_Key[i] + " is " + kbyte + Environment.NewLine;
                        res = xor(temp, kbyte);
                        log.Text += "Encrypted binary string for " + Message + " is " + res + Environment.NewLine;
                        int ind = Convert.ToInt32(res, 2);
                        log.Text += "Symbol in " + i + " encrypted in " + Convert.ToChar(ind) + Environment.NewLine;
                        result += Convert.ToChar(ind);
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    log.Text += "XOR(Encryption): unexpected error in " + i + Environment.NewLine;
                    return false;
                }
                i++;
            }
            return true;
        }

        private bool GammaCipher(string message,ref string result,ref int[] gamma,int op)
        {
            message = message.ToUpper();
            string alphabet;
            if (Valphabet.IndexOf(message[0]) >= 0)
            {
                alphabet = Valphabet;
            }
            else
            {
                alphabet = Ucirilica;
            }
            int n = alphabet.Length;
            switch (op)
            {
                case 0:
                    {
                        Array.Resize<int>(ref gamma, message.Length);
                        Random rand = new Random();
                        log.Text += "Gamma :" + Environment.NewLine;
                        for(int i = 0; i < message.Length; i++)
                        {
                            gamma[i] = rand.Next(0, n);
                            log.Text += " " + gamma[i];
                        }
                        for(int i = 0; i < message.Length; i++)
                        {
                            int ind = alphabet.IndexOf(message[i]);
                            if (ind == -1)
                            {
                                result += message[i];
                            }
                            else
                            {
                                ind = (ind + gamma[i]) % n;
                                result += alphabet[ind];
                            }
                        }
                        break;
                    }
                case 1:
                    {
                        for(int i = 0; i < message.Length; i++)
                        {
                            int ind = alphabet.IndexOf(message[i]);
                            if (ind == -1)
                            {
                                result += message[i];
                            }
                            else
                            {
                                ind = (ind + n - gamma[i]) % n;
                                result += alphabet[ind];
                            }
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            return true;
        }

        private bool checkInt2D(int[,] mas,int num,int r,int len)
        {
            for(int i=0;i< len; i++)
            {
                if (mas[r, i] == num)
                {
                    return true;
                }
            }
            return false;
        }

        private string LeftShift(string s, int d)
        {
            string res = "";
            for (int i = 0; i < s.Length; i++)
            {
                res += s[(i + d) % s.Length];
            }
            return res;
        }

        private string[] kGen(string k)
        {
            string[] s = new string[16];
            for (int i = 0; i < 16; i++)
            {
                k = LeftShift(k, Displacement[i]);
                s[i] = k;
            }
            return s;
        }


        private bool DES(string mes, string key,ref string res, int op)
        {
            string L0, R0, L = "", R = "", BinMes = "", temp = "", r = "";
            string[] k = kGen(key);
            int i;
            res = "";
            i = 0;
            if (op == 0)
            {
                while (mes.Length % 8 != 0)
                {
                    mes += '-';
                }
            }
            while (i < mes.Length)
            {
                int n = (int)mes[i];
                BinMes += Convert.ToString(n, 2).PadLeft(16, '0');
                i++;
            }
            i = 0;
            while (i < mes.Length / 8)
            {
                temp = BinMes.Substring(i * 128, 128);
                L0 = temp.Substring(0, 64);
                R0 = temp.Substring(64, 64);
                if (op == 0)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        L = R0;
                        R = xor(L0, xor(R0, k[j]));
                        R0 = R;
                        L0 = L;
                    }
                    temp = L + R;
                }
                else if (op == 1)
                {
                    for (int j = 15; j >= 0; j--)
                    {
                        R = L0;
                        L = xor(R0, xor(L0, k[j]));
                        R0 = R;
                        L0 = L;
                    }
                    temp = L + R;
                }
                r += temp;
                i++;
            }
            for (i = 0; i < r.Length / 16; i++)
            {
                res += (char)Convert.ToInt32(r.Substring(i * 16, 16), 2);
            }
            i = res.Length - 1;
            while (res[i] == '-')
            {
                i--;
            }
            res = res.Substring(0, i + 1);
            return true;
        }

        private static int modPow(int num, int pow, int mod)
        {
            int res = 1, np = 0;
            while (np < pow)
            {
                res = (res * num) % mod;
                np++;
            }
            return res;
        }

        private bool Elgamal(string mes,ref string res, int op)//Elgamal algorithm
        {
            res = "";
            int num;
            if (op == 0)
            {

                Random rand = new Random();
                p = 0;
                while (!PrimeNumber(p))
                {
                    p = rand.Next(1401, 2000);
                }
                int a, b, y, k;
                x = rand.Next(1, p - 1);
                y = modPow(g, x, p);
                for (int i = 0; i < mes.Length; i++)
                {
                    num = (int)mes[i];
                    k = rand.Next(1, p - 1);
                    a = modPow(g, k, p);
                    b = (modPow(y, k, p) * num) % p;
                    res += a + "." + b;
                    res += " ";
                }
            }
            else if (op == 1)
            {
                int i = 0, a = 0, b = 0;
                string nstr = "";
                while (i < mes.Length)
                {
                    if (i < mes.Length && mes[i] == '.')
                    {
                        a = int.Parse(nstr);
                        nstr = "";
                        i++;
                    }
                    else if (i < mes.Length && mes[i] == ' ')
                    {
                        b = int.Parse(nstr);
                        int r = (b * modPow(a, p - 1 - x, p)) % p;
                        res += (char)r;
                        nstr = "";
                        i++;
                    }
                    while (i < mes.Length && mes[i] != '.' && mes[i] != ' ')
                    {
                        nstr += mes[i];
                        i++;
                    }
                }
            }
            return true;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void VigenerKey_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void SKL_Click(object sender, EventArgs e)
        {

        }

        private void NKL_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private byte[] EncryptStringToBytesAes(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;

        }

        static string DecryptStringFromBytesAes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string plaintext;

            Aes aesAlg = Aes.Create();
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {

                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }


            return plaintext;

        }

        private bool AES_E(string mes, ref string res) {

            AES_Alg = Aes.Create();
            encryptedByte = EncryptStringToBytesAes(mes, AES_Alg.Key, AES_Alg.IV);
            using (var ms = new MemoryStream(encryptedByte)) {
                using (var sr = new StreamReader(ms)) {
                    res = sr.ReadToEnd();
                }
            }
                return true;
        }

        private bool AES_D(byte[] byteArray, ref string res) {
            res = DecryptStringFromBytesAes(encryptedByte, AES_Alg.Key, AES_Alg.IV);
            return true;
        }

        private bool RSA(string message,ref string res, int op)
        {
            int p = 0, q = 0, f;
            res = "";
            if (op == 0)
            {
                n = 0;
                e = 0;
                d = 1;
                Random rand = new Random();
                while (!PrimeNumber(p))
                {
                    p = rand.Next(11, 101);
                }
                while (!PrimeNumber(q) || p == q)
                {
                    q = rand.Next(11, 101);
                }
                n = p * q;
                log.Text += "p = " + p + Environment.NewLine;
                log.Text += "q = " + q + Environment.NewLine;
                log.Text += "N = " + n + Environment.NewLine;
                f = (p - 1) * (q - 1);
                log.Text += "f = " + f + Environment.NewLine;
                while (!PrimeNumber(e))
                {
                    e = rand.Next(2, f);
                }
                log.Text += "e = " + e + Environment.NewLine;
                while (d * e % f != 1) d++;
                log.Text += "d = " + d + Environment.NewLine;
                for (int i = 0; i < message.Length; i++)
                {
                    int num = (int)message[i];
                    num = modPow(num, e, n);
                    res += (char)num;
                }
            }
            else if (op == 1)
            {
                for (int i = 0; i < message.Length; i++)
                {
                    int num = (int)message[i];
                    res += (char)modPow(num, d, n);
                }
            }
            return true;
        }

        private bool DiffiHellman(string mes, ref string res, int op) {
            if (op == 0)
            {
                Random r = new Random();
                int a = r.Next(11, 500);
                b = r.Next(11, 500);
                p = 0;
                while (!PrimeNumber(p))
                {
                    p = r.Next(1, 70);
                }
                log.Text += p + Environment.NewLine;
                A = modPow(g, a, p);
                B = modPow(g, b, p);
                int k = modPow(B, a, p);
                log.Text += "p = " + p + Environment.NewLine + "a = " + a + Environment.NewLine + "b = " + b + Environment.NewLine + "A = " + A + Environment.NewLine +
                    "B = " + B + Environment.NewLine + "(Encrypt)K = " + k + Environment.NewLine;
                return Caesar(mes, k, ref res, op);
            }
            else if (op == 1)
            {
                log.Text += p + Environment.NewLine;
                int k = modPow(A, b, p);
                log.Text += "p = " + p + Environment.NewLine + "b = " + b + Environment.NewLine + "A = " + A + Environment.NewLine +
                "B = " + B + Environment.NewLine + "(Encrypt)K = " + k + Environment.NewLine;
                log.Text += "(Decrypt)K = " + k + Environment.NewLine;
                return Caesar(mes, k, ref res, op);
            }
            else {
                log.Text += "Wrong code(Diffi-Hellman)" + Environment.NewLine;
                return false;
            }
        }

        private void Encrypt(object sender, EventArgs e)
        {
            string res = "";
            //BINARY
            if (cipher == "Binary")
            {
                if (!String.IsNullOrWhiteSpace(textBox3.Text))
                {
                    message = textBox3.Text;
                }
                else
                {
                    textBox3.Text = "Enter your message here !!!";
                    return;
                }
                int i = 0;
                while (i < message.Length)
                {
                    char c = message[i];
                    i++;
                    res += Convert.ToString(c, 2).PadLeft(12, '0');
                }
                textBox4.Text = res;

            }
            //CAESAR
            else if (cipher == "Caesar")
            {
                if (!String.IsNullOrEmpty(CaesarKey.Text))
                {
                    Ckey = Convert.ToInt32(CaesarKey.Text);
                }
                string s;
                if (!String.IsNullOrWhiteSpace(textBox3.Text))
                {
                    s = textBox3.Text;
                }
                else
                {
                    textBox3.Text = "Enter your message here !!!";
                    return;
                }
                if (Caesar(s, Ckey, ref res, 0))
                {
                    textBox4.Text = res;
                }

            }
            //VIGENER
            else if (cipher == "Vigener")
            {
                int len = Valphabet.Length;
                tabulaRecta = new string[len];
                int i;
                for (i = 0; i < len; i++)
                {
                    tabulaRecta[i] = "";
                    for (int j = 0; j < len; j++)
                    {
                        tabulaRecta[i] += Valphabet[(len - i + j) % len];
                    }
                }
                //End constructing
                //initializing Vkey - key to encrypt
                if (!String.IsNullOrWhiteSpace(VigenerKey.Text))
                {
                    Vkey = VigenerKey.Text;
                }
                if (!String.IsNullOrWhiteSpace(textBox3.Text))
                {
                    message = textBox3.Text;
                }
                else
                {
                    textBox3.Text = "Enter your message here !!!";
                    return;
                }
                //Constructing string Key to cipher
                Key = "";
                while (Key.Length <= message.Length)
                {
                    Key += Vkey;
                }
                //End constructing
                i = 0;
                while (i < message.Length)
                {
                    if (Valphabet.IndexOf(message[i]) != -1)
                    {
                        int ci, cj;
                        ci = Valphabet.IndexOf(Key[i]);
                        cj = Valphabet.IndexOf(message[i]);
                        res += tabulaRecta[ci][cj];
                    }
                    else
                    {
                        res += message[i];
                    }
                    i++;
                }
                textBox4.Text = res;
            }
            //TABLE 
            else if (cipher == "Table")
            {
                Array.Resize<int>(ref WhiteSpaceIndexMas, 1);
                string m;
                if (textBox3.Text != "")
                {
                    m = textBox3.Text;
                }
                else
                {
                    textBox3.Text = "Enter your message";
                    return;
                }
                int leng = m.Length, SpaceNumber = 0;
                for (int i = 0; i < leng; i++)
                {
                    if (m[i] == ' ')
                    {
                        if (SpaceNumber == WhiteSpaceIndexMas.Length)
                        {
                            Array.Resize<int>(ref WhiteSpaceIndexMas, WhiteSpaceIndexMas.Length + 1);
                        }
                        WhiteSpaceIndexMas[SpaceNumber] = i;
                        SpaceNumber++;
                    }
                }
                string[] arr = m.Split(' ');
                m = "";
                for (int i = 0; i < arr.Length; i++)
                {
                    m += arr[i];
                }
                leng = m.Length;
                if (PrimeNumber(leng))
                {
                    while (PrimeNumber(leng))
                    {
                        m += "/";
                        leng++;
                    }
                }
                for (int i = (int)Math.Sqrt(leng) + 1; i > 0; i--)
                {
                    if (leng % i == 0)
                    {
                        row = i;
                        col = leng / i;
                        break;
                    }
                }
                string[] tableE = new string[row];
                table = new string[row];
                int l = 0;
                for (int i = 0; i < row; i++)
                {
                    tableE[i] = "";
                    table[i] = "";
                }
                while (l < leng)
                {
                    for (int i = 0; i < row; i++)
                    {
                        tableE[i] += m[l];
                        table[i] += m[l];
                        l++;
                    }
                }

                int le = 0;
                for (int i = 0; i < row; i++)
                {
                    int j = 0;
                    while (j < tableE[i].Length)
                    {
                        res += tableE[i][j];
                        le++;
                        if (le == row)
                        {
                            le = 0;
                            res += ' ';
                        }
                        j++;
                    }
                }
                textBox4.Text = res;
                //TABLE WITH KEY 
            }
            else if (cipher == "Table with key")
            {
                Array.Resize<int>(ref WhiteSpaceIndexMas, 1);
                string m;
                if (textBox3.Text != "")
                {
                    m = textBox3.Text;
                }
                else
                {
                    textBox3.Text = "Enter your message";
                    return;
                }
                int leng = m.Length, SpaceNumber = 0;
                for (int i = 0; i < leng; i++)
                {
                    if (m[i] == ' ')
                    {
                        if (SpaceNumber == WhiteSpaceIndexMas.Length)
                        {
                            Array.Resize<int>(ref WhiteSpaceIndexMas, WhiteSpaceIndexMas.Length + 1);
                        }
                        WhiteSpaceIndexMas[SpaceNumber] = i;
                        SpaceNumber++;
                    }
                }
                string[] arr = m.Split(' ');
                m = "";
                for (int i = 0; i < arr.Length; i++)
                {
                    m += arr[i];
                }
                leng = m.Length;
                if (PrimeNumber(leng))
                {
                    while (PrimeNumber(leng))
                    {
                        m += "/";
                        leng++;
                    }
                }
                for (int i = (int)Math.Sqrt(leng); i > 0; i--)
                {
                    if (leng % i == 0)
                    {
                        row = i;
                        col = leng / i;
                        break;
                    }
                }
                char[,] tableE = new char[row, col];
                for (int i = 0; i < col; i++)
                {
                    for (int j = 0; j < row; j++)
                    {
                        tableE[j, i] = m[i * row + j];
                    }
                }
                string key;
                char[] Key = new char[col];
                if (!String.IsNullOrWhiteSpace(VigenerKey.Text))
                {
                    key = VigenerKey.Text;
                }
                else
                {
                    VigenerKey.Text = "Enter key here";
                    return;
                }
                string t = key;
                while (col > key.Length)
                {
                    key += t;
                }
                for (int i = 0; i < col; i++)
                {
                    Key[i] = key[i];
                }
                int cp, ep = 0, N = col, Letter;
                if (Convert.ToInt32(Key[0]) >= 'A' && Convert.ToInt32(Key[0]) <= 'Z')
                {
                    Letter = 'A';
                }
                else if (Convert.ToInt32(Key[0]) >= 'a' && Convert.ToInt32(Key[0]) <= 'z')
                {
                    Letter = 'a';
                }
                else
                {
                    VigenerKey.Text = "Wrong key";
                    return;
                }
                int end = Letter + 26;
                while (Letter < end && ep < N)
                {
                    cp = ep;
                    while (cp < N)
                    {
                        if (Convert.ToInt32(Key[cp]) == Letter)
                        {
                            char temp;
                            for (int i = 0; i < row; i++)
                            {
                                temp = tableE[i, ep];
                                tableE[i, ep] = tableE[i, cp];
                                tableE[i, cp] = temp;
                            }
                            temp = Key[ep];
                            Key[ep] = Key[cp];
                            Key[cp] = temp;
                            ep++;
                        }
                        cp++;
                    }
                    Letter++;
                }
                int le = 0;
                for (int i = 0; i < row; i++)
                {
                    int j = 0;
                    while (j < col)
                    {
                        res += tableE[i, j];
                        le++;
                        if (le == row)
                        {
                            le = 0;
                            res += ' ';
                        }
                        j++;
                    }
                }
                textBox4.Text = res;
                for (int i = 0; i < N; i++)
                {
                    textBox5.Text += Key[i];
                }
            }
            //MAGIC SQUARE
            else if (cipher == "Magic")
            {
                Array.Resize<int>(ref WhiteSpaceIndexMas, 1);
                log.Text = "";
                string s = textBox3.Text;
                int sql, SpaceNumber = 0;
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == ' ')
                    {
                        if (SpaceNumber == WhiteSpaceIndexMas.Length)
                        {
                            Array.Resize<int>(ref WhiteSpaceIndexMas, WhiteSpaceIndexMas.Length + 1);
                        }
                        WhiteSpaceIndexMas[SpaceNumber] = i;
                        SpaceNumber++;
                    }
                }
                string[] w = s.Split(' ');
                s = "";
                for (int i = 0; i < w.Length; i++)
                {
                    s += w[i];
                }
                log.Text = s + s.Length;
                sql = (int)Math.Sqrt(s.Length);
                if (!(sql >= 3 && sql <= 6))
                {
                    textBox4.Text = "Wrong length of message";
                    return;
                }
                if (magicSquare(s, ref res, sql, 0))
                {
                    textBox4.Text = res;
                }
            }
            //GRONSFELD
            else if (cipher == "Gronsfeld")
            {
                log.Text = "";
                string key, m;
                if (!String.IsNullOrWhiteSpace(VigenerKey.Text))
                {
                    key = VigenerKey.Text;
                }
                else
                {
                    log.Text += "Enter key !" + Environment.NewLine;
                    return;
                }
                if (!String.IsNullOrWhiteSpace(textBox3.Text))
                {
                    m = textBox3.Text;
                }
                else
                {
                    log.Text += "Enter message !" + Environment.NewLine;
                    return;
                }
                m = m.ToUpper();
                if (Gronsfeld(m, key, ref res, 0))
                {
                    textBox4.Text = res;
                    log.Text += "Gronsfeld Encryption of " + m + " OK" + Environment.NewLine;
                }
                else
                {
                    textBox4.Text = "Error.See log for more information";
                    log.Text += "Error was found, see previous messages for detail. " + Environment.NewLine;
                }
            }
            //WIDSTON
            else if (cipher == "Witston")
            {
                Array.Resize<int>(ref WhiteSpaceIndexMas, 1);
                string key, m;
                if (!String.IsNullOrWhiteSpace(VigenerKey.Text))
                {
                    key = VigenerKey.Text;
                }
                else
                {
                    log.Text += "Enter key !" + Environment.NewLine;
                    return;
                }
                if (!String.IsNullOrWhiteSpace(textBox3.Text))
                {
                    m = textBox3.Text;
                }
                else
                {
                    log.Text += "Press encrypt first !" + Environment.NewLine;
                    return;
                }
                if (Witston(m, key, ref res, 0))
                {
                    textBox4.Text = res;
                }
                else
                {
                    log.Text += "Bad news " + Environment.NewLine;
                }
            }
            else if (cipher == "Polibian")//POLYBIAN
            {
                string m;
                if (!String.IsNullOrWhiteSpace(textBox3.Text))
                {
                    m = textBox3.Text;
                }
                else
                {
                    log.Text += "Enter message !" + Environment.NewLine;
                    return;
                }
                m = m.ToUpper();
                if (Polibian(m, ref res, 0))
                {
                    textBox4.Text = res;
                }
                else
                {
                    log.Text += "Bad news " + Environment.NewLine;
                }
            }
            else if (cipher == "Trisemus")//TRISEMUS
            {
                string m, key;
                if (!String.IsNullOrWhiteSpace(textBox3.Text))
                {
                    m = textBox3.Text;
                }
                else
                {
                    log.Text += "Enter message !" + Environment.NewLine;
                    return;
                }
                if (!string.IsNullOrWhiteSpace(VigenerKey.Text))
                {
                    key = VigenerKey.Text;
                }
                else
                {
                    log.Text += "Enter key for Trisemus !" + Environment.NewLine;
                    return;
                }
                if (Trisemus(m, key, ref res, 0))
                {
                    textBox4.Text = res;
                }
                else
                {
                    log.Text += "Bad news " + Environment.NewLine;
                }
            }
            else if (cipher == "Caesar with key")//ANOTHER CAESAR
            {
                string key, m;
                int num = 0;
                try
                {
                    num = Convert.ToInt32(CaesarKey.Text);
                }
                catch (InvalidCastException)
                {
                    log.Text += "InvalidCast in Caesar with key(Encryption)" + Environment.NewLine;
                }
                catch (FormatException)
                {
                    log.Text += "Format in Caesar with key(Decryption)" + Environment.NewLine;
                }
                m = textBox3.Text;
                key = VigenerKey.Text;
                if (String.IsNullOrEmpty(m) || String.IsNullOrEmpty(key))
                {
                    log.Text += "Enter message and key" + Environment.NewLine;
                    return;
                }
                if (CWK(m, key, num, ref res, 0))
                {
                    textBox4.Text = res;
                }
                else
                {
                    log.Text += "It's not good in CWK(Encrypt)" + Environment.NewLine;
                }
            }
            else if (cipher == "Playfair")//PLAYFAIR
            {
                string key, m;
                m = textBox3.Text;
                key = VigenerKey.Text;
                if (String.IsNullOrEmpty(m) || String.IsNullOrEmpty(key))
                {
                    log.Text += "Enter message and key" + Environment.NewLine;
                    return;
                }
                if (Playfer(m, key, ref res, 0))
                {
                    textBox4.Text = res;
                }
                else
                {
                    log.Text += "It's not good in Playfer(Encrypt)" + Environment.NewLine;
                }
            }
            else if (cipher == "XOR")//XOR
            {
                if (!String.IsNullOrWhiteSpace(textBox3.Text))
                {
                    message = textBox3.Text;
                }
                else
                {
                    textBox3.Text = "Enter your message here !!!";
                    return;
                }
                if (!String.IsNullOrWhiteSpace(VigenerKey.Text))
                {
                    Key = VigenerKey.Text;
                }
                else
                {
                    textBox4.Text = "Enter key";
                    return;
                }
                if (XOR(message, Key, ref res))
                {
                    textBox4.Text = res;
                }
                else
                {
                    textBox4.Text = "Error.Read Log";
                }
            }
            else if (cipher == "Gamma")
            {
                Array.Resize<int>(ref gamma, 1);
                if (!String.IsNullOrWhiteSpace(textBox3.Text))
                {
                    message = textBox3.Text;
                }
                else
                {
                    textBox3.Text = "Enter your message here !!!";
                    return;
                }
                if (GammaCipher(message, ref res, ref gamma, 0))
                {
                    textBox4.Text = res;
                }
                else
                {
                    textBox4.Text = "Error.Read Log";
                }
            }
            else if (cipher == "DES")
            {
                DES_Key = "";
                Random rand = new Random();
                for (int i = 0; i < 128; i++)
                {
                    DES_Key += rand.Next(0, 2);
                }
                if (!String.IsNullOrWhiteSpace(textBox3.Text))
                {
                    message = textBox3.Text;
                }
                else
                {
                    textBox3.Text = "Enter your message here !!!";
                    return;
                }
                if (DES(message, DES_Key, ref res, 0))
                {
                    textBox4.Text = res;
                }
                else
                {
                    textBox4.Text = "Error(DES).Read Log";
                }
            }
            else if (cipher == "Elgamal")
            {
                if (!String.IsNullOrWhiteSpace(textBox3.Text))
                {
                    message = textBox3.Text;
                }
                else
                {
                    textBox3.Text = "Enter your message here !!!";
                    return;
                }
                if (Elgamal(message, ref res, 0))
                {
                    textBox4.Text = res;
                }
                else
                {
                    textBox4.Text = "Error(Elgamal).Read Log";
                }
            }
            else if (cipher == "AES")
            {
                if (!String.IsNullOrWhiteSpace(textBox3.Text))
                {
                    message = textBox3.Text;
                }
                else
                {
                    textBox3.Text = "Enter your message here !!!";
                    return;
                }
                try
                {
                    if (AES_E(message, ref res))
                    {
                        textBox4.Text = res;
                    }
                    else
                    {
                        textBox4.Text = "Error(AES).Read Log";
                    }
                }
                catch (Exception exception)
                {
                    log.Text += exception.Message + Environment.NewLine;
                }
            }
            else if (cipher == "RSA")
            {
                if (!String.IsNullOrWhiteSpace(textBox3.Text))
                {
                    message = textBox3.Text;
                }
                else
                {
                    textBox3.Text = "Enter your message here !!!";
                    return;
                }
                try
                {
                    if (RSA(message, ref res, 0))
                    {
                        textBox4.Text = res;
                    }
                    else
                    {
                        textBox4.Text = "Error(RSA).Read Log";
                    }
                }
                catch (Exception exception)
                {
                    log.Text += exception.Message + Environment.NewLine;
                }
            }
            else if (cipher == "Diffi-Hellman") {
                if (!String.IsNullOrWhiteSpace(textBox3.Text))
                {
                    message = textBox3.Text;
                }
                else
                {
                    textBox3.Text = "Enter your message here !!!";
                    return;
                }
                try
                {
                    if (DiffiHellman(message, ref res, 0))
                    {
                        textBox4.Text = res;
                    }
                    else
                    {
                        textBox4.Text = "Error(Diffi-Hellman).Read Log";
                    }
                }
                catch (Exception exception)
                {
                    log.Text += exception.Message + Environment.NewLine;
                }
            }
            else
            {
                log.Text += "Choose cipher first" + Environment.NewLine;
                return;
            }
        }

        private void show_hide(object sender, EventArgs e)
        {
            if (log.Visible)
            {
                log.Visible = false;
                return;
            }
            log.Visible = true;
        }

        private void Clr(object sender, EventArgs e)
        {
            textBox3.Text = "";
            message = "";
            textBox4.Text = "";
            textBox5.Text = "";
            VigenerKey.Text = "";
            Vkey = "LEMON";
            CaesarKey.Text = "";
            Ckey = 3;
            Key = "";
            log.Text = "";
            log.Visible = false;
            DES_Key = "";
        }

        private void Exit(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Decrypt(object sender, EventArgs e)
        {
            string res = "";
            if (cipher == "Binary")
            {
                string s;
                if (!String.IsNullOrWhiteSpace(textBox4.Text))
                {
                    s = textBox4.Text;
                }
                else
                {
                    textBox4.Text = "Press encrypt first !!!";
                    return;
                }
                int l = 0;
                while (l < s.Length)
                {
                    char c = Convert.ToChar(Convert.ToInt32(s.Substring(l, 12), 2));
                    res += c;
                    l += 12;
                }
                textBox5.Text = res;
            }
            else if (cipher == "Caesar")
            {
                string s;
                if (!String.IsNullOrWhiteSpace(textBox4.Text))
                {
                    s = textBox4.Text;
                }
                else
                {
                    textBox4.Text = "Press encrypt first !!!";
                    return;
                }
                if (Caesar(s, Ckey, ref res, 1))
                {
                    textBox5.Text = res;
                }
                else {
                    log.Text += "Somethig goes wrong in Caesar Decryption" + Environment.NewLine;
                    textBox5.Text = "Error !";
                }

            }
            else if (cipher == "Vigener")
            {
                //initializing Vkey - key to encrypt
                string s;
                if (!String.IsNullOrWhiteSpace(textBox4.Text))
                {
                    s = textBox4.Text;
                }
                else
                {
                    textBox4.Text = "Press encrypt first !!!";
                    return;
                }
                int i = 0;
                while (i < s.Length)
                {
                    if (Valphabet.IndexOf(s[i]) != -1)
                    {
                        int ci, r;
                        ci = Valphabet.IndexOf(Key[i]);
                        r = tabulaRecta[ci].IndexOf(s[i]);
                        res += Valphabet[r];
                    }
                    else
                    {
                        res += s[i];
                    }
                    i++;
                }
                textBox5.Text = res;
            }
            else if (cipher == "Table")
            {
                string str = textBox4.Text;
                string[] SplitArr = new string[1];
                int p = 0, WordCounter = 0;
                string element = "";
                while (p < str.Length)
                {
                    element = "";
                    while (str[p] != ' ' && p < str.Length)
                    {
                        element += str[p];
                        p++;
                    }
                    if (WordCounter == SplitArr.Length)
                    {
                        Array.Resize<string>(ref SplitArr, SplitArr.Length + 1);
                    }
                    SplitArr[WordCounter] = element;
                    WordCounter++;
                    p++;
                }
                int col1 = SplitArr.Length, row1 = SplitArr[0].Length;
                if (col1 != col || row1 != row)
                {
                    textBox5.Text = "Wrong dimensions";
                    return;
                }
                string[] tableD = new string[row1];
                str = "";//Trim
                for (int i = 0; i < SplitArr.Length; i++)
                {
                    str += SplitArr[i];
                }
                //End
                //Constructing table
                for (int i = 0; i < row1; i++)
                {
                    tableD[i] = "";
                    for (int j = 0; j < col1; j++)
                    {
                        tableD[i] += str[i * col1 + j];
                    }
                }
                int count = 0;
                for (int i = 0; i < col; i++)
                {
                    for (int j = 0; j < row; j++)
                    {
                        if (count < WhiteSpaceIndexMas.Length && res.Length == WhiteSpaceIndexMas[count])
                        {
                            count++;
                            res += ' ';
                        }
                        if (tableD[j][i] == '/')
                        {
                            break;
                        }
                        else
                        {
                            res += tableD[j][i];
                        }
                    }
                }
                textBox5.Text = res;
            }
            else if (cipher == "Table with key")
            {
                string str = textBox4.Text;
                string[] SplitArr = new string[1];
                int p = 0, WordCounter = 0;
                string element = "";
                while (p < str.Length)
                {
                    element = "";
                    while (str[p] != ' ' && p < str.Length)
                    {
                        element += str[p];
                        p++;
                    }
                    if (WordCounter == SplitArr.Length)
                    {
                        Array.Resize<string>(ref SplitArr, SplitArr.Length + 1);
                    }
                    SplitArr[WordCounter] = element;
                    WordCounter++;
                    p++;
                }
                int col1 = SplitArr.Length, row1 = SplitArr[0].Length;
                if (col1 != col || row1 != row)
                {
                    textBox5.Text = "Wrong dimensions";
                    return;
                }
                char[,] tableD = new char[row1, col1];
                str = "";//Trim
                for (int i = 0; i < SplitArr.Length; i++)
                {
                    str += SplitArr[i];
                }
                //End
                //Constructing table
                for (int i = 0; i < row1; i++)
                {
                    for (int j = 0; j < col1; j++)
                    {
                        tableD[i, j] = str[i * col1 + j];
                    }
                }
                string vkey = VigenerKey.Text;
                char[] key = new char[col1], key1 = new char[col1];
                for (int i = 0; i < col1; i++)
                {
                    key[i] = vkey[i % vkey.Length];
                }
                int cp, ep = 0, N = col1, Letter;
                if (Convert.ToInt32(key[0]) >= 'A' && Convert.ToInt32(key[0]) <= 'Z')
                {
                    Letter = 'A';
                }
                else if (Convert.ToInt32(key[0]) >= 'a' && Convert.ToInt32(key[0]) <= 'z')
                {
                    Letter = 'a';
                }
                else
                {
                    VigenerKey.Text = "Wrong key";
                    return;
                }
                int end = Letter + 26, counter = 0;
                while (Letter < end && counter < col1)
                {
                    ep = 0;
                    while (ep < col1)
                    {
                        if (Convert.ToInt32(key[ep]) == Letter)
                        {
                            key1[counter] = key[ep];
                            counter++;
                        }
                        ep++;
                    }
                    Letter++;
                }
                ep = 0;
                while (ep < N)
                {
                    if (key[ep] != key1[ep])
                    {
                        cp = ep;
                        while (cp < N)
                        {
                            if (key1[cp] == key[ep])
                            {
                                break;
                            }
                            cp++;
                        }
                        char temp;
                        for (int i = 0; i < row1; i++)
                        {
                            temp = tableD[i, cp];
                            tableD[i, cp] = tableD[i, ep];
                            tableD[i, ep] = temp;
                        }
                        temp = key1[ep];
                        key1[ep] = key1[cp];
                        key1[cp] = temp;
                    }
                    ep++;
                }
                int count = 0;
                for (int i = 0; i < col; i++)
                {
                    for (int j = 0; j < row; j++)
                    {
                        if (count < WhiteSpaceIndexMas.Length && res.Length == WhiteSpaceIndexMas[count])
                        {
                            count++;
                            res += ' ';
                        }
                        if (tableD[j, i] == '/')
                        {
                            break;
                        }
                        else
                        {
                            res += tableD[j, i];
                        }
                    }
                }
                textBox5.Text = res;
            }
            else if (cipher == "Magic")
            {
                string s = textBox4.Text;
                string[] w = s.Split(' ');
                int len;
                s = "";
                for (int i = 0; i < w.Length; i++)
                {
                    s += w[i];
                }
                len = (int)Math.Sqrt(s.Length);
                log.Text += len + Environment.NewLine;
                if (magicSquare(s, ref res, len, 1))
                {
                    textBox5.Text = res;
                }
                else
                {
                    log.Text = res;
                    textBox5.Text = "Something goes wrong";
                }
            }
            else if (cipher == "Gronsfeld")
            {
                string key, m;
                if (!String.IsNullOrWhiteSpace(VigenerKey.Text))
                {
                    key = VigenerKey.Text;
                }
                else
                {
                    log.Text += "Enter key !" + Environment.NewLine;
                    return;
                }
                if (!String.IsNullOrWhiteSpace(textBox4.Text))
                {
                    m = textBox4.Text;
                }
                else
                {
                    log.Text += "Press encrypt first !" + Environment.NewLine;
                    return;
                }
                if (Gronsfeld(m, key, ref res, 1))
                {
                    textBox5.Text = res;
                    log.Text += "Grosfeld Decryption of cipher code " + m + " OK" + Environment.NewLine;
                }
                else
                {
                    textBox5.Text = "Error. See log for more information";
                    log.Text += "Error was found, see previous messages for detail. " + Environment.NewLine;
                }
            }
            else if (cipher == "Witston")
            {
                string key, m;
                if (!String.IsNullOrWhiteSpace(VigenerKey.Text))
                {
                    key = VigenerKey.Text;
                }
                else
                {
                    log.Text += "Enter key !" + Environment.NewLine;
                    return;
                }
                if (!String.IsNullOrWhiteSpace(textBox4.Text))
                {
                    m = textBox4.Text;
                }
                else
                {
                    log.Text += "Press encrypt first !" + Environment.NewLine;
                    return;
                }
                if (Witston(m, key, ref res, 1))
                {
                    textBox5.Text = res;
                }
                else
                {
                    log.Text += "Bad news (Witston)" + Environment.NewLine;
                }
            }
            else if (cipher == "Polibian")
            {
                string m;
                if (!String.IsNullOrWhiteSpace(textBox4.Text))
                {
                    m = textBox4.Text;
                }
                else
                {
                    log.Text += "Press encrypt first!" + Environment.NewLine;
                    return;
                }
                m = m.ToUpper();
                if (Polibian(m, ref res, 1))
                {
                    textBox5.Text = res;
                }
                else
                {
                    log.Text += "Bad news (Polibian)" + Environment.NewLine;
                }
            }
            else if (cipher == "Trisemus")
            {
                string key, m;
                if (!String.IsNullOrWhiteSpace(VigenerKey.Text))
                {
                    key = VigenerKey.Text;
                }
                else
                {
                    log.Text += "Enter key !" + Environment.NewLine;
                    return;
                }
                if (!String.IsNullOrWhiteSpace(textBox4.Text))
                {
                    m = textBox4.Text;
                }
                else
                {
                    log.Text += "Press encrypt first !" + Environment.NewLine;
                    return;
                }
                if (Trisemus(m, key, ref res, 1))
                {
                    textBox5.Text = res;
                }
                else
                {
                    log.Text += "Bad news (Trisemus)" + Environment.NewLine;
                }
            }
            else if (cipher == "Caesar with key")
            {
                string key, m;
                int num = 0;
                try
                {
                    num = Convert.ToInt32(CaesarKey.Text);
                }
                catch (InvalidCastException)
                {
                    log.Text += "InvalidCast in Caesar with key(Decryption)" + Environment.NewLine;
                    return;
                }
                catch (FormatException)
                {
                    log.Text += "Format in Caesar with key(Decryption)" + Environment.NewLine;
                    return;
                }
                m = textBox4.Text;
                key = VigenerKey.Text;
                if (String.IsNullOrEmpty(m) || String.IsNullOrEmpty(key))
                {
                    log.Text += "Enter message and key" + Environment.NewLine;
                    return;
                }
                if (CWK(m, key, num, ref res, 1))
                {
                    textBox5.Text = res;
                }
                else
                {
                    log.Text += "It's not good in CWK(Decrypt)" + Environment.NewLine;
                }
            }
            else if (cipher == "Playfair")
            {
                string key, m;
                m = textBox4.Text;
                key = VigenerKey.Text;
                if (String.IsNullOrEmpty(m) || String.IsNullOrEmpty(key))
                {
                    log.Text += "Enter message and key" + Environment.NewLine;
                    return;
                }
                if (Playfer(m, key, ref res, 1))
                {
                    textBox5.Text = res;
                }
                else
                {
                    log.Text += "It's not good in Playfair(Decrypt)" + Environment.NewLine;
                }
            }
            else if (cipher == "XOR")
            {
                if (!String.IsNullOrWhiteSpace(textBox4.Text))
                {
                    message = textBox4.Text;
                }
                else
                {
                    textBox5.Text = "There is no encrypted message !!!" + Environment.NewLine;
                    return;
                }
                if (!String.IsNullOrWhiteSpace(VigenerKey.Text))
                {
                    Key = VigenerKey.Text;
                }
                else
                {
                    textBox5.Text += "Enter key";
                    return;
                }
                if (XOR(message, Key, ref res))
                {
                    textBox5.Text = res;
                }
                else
                {
                    textBox5.Text = "Error.Read Log";
                }
            }
            else if (cipher == "Gamma")
            {
                if (!String.IsNullOrWhiteSpace(textBox4.Text))
                {
                    message = textBox4.Text;
                }
                else
                {
                    textBox5.Text = "There is no encrypted message !!!" + Environment.NewLine;
                    return;
                }
                if (GammaCipher(message, ref res, ref gamma, 1))
                {
                    textBox5.Text = res;
                }
                else
                {
                    textBox5.Text = "Error.Read Log";
                }
            }
            else if (cipher == "DES")
            {
                if (!String.IsNullOrWhiteSpace(textBox4.Text))
                {
                    message = textBox4.Text;
                }
                else
                {
                    textBox5.Text = "There is no encrypted message !!!" + Environment.NewLine;
                    return;
                }
                if (DES(message, DES_Key, ref res, 1))
                {
                    textBox5.Text = res;
                }
                else
                {
                    textBox5.Text = "Error.Read Log";
                }
            }
            else if (cipher == "Elgamal")
            {
                if (!String.IsNullOrWhiteSpace(textBox4.Text))
                {
                    message = textBox4.Text;
                }
                else
                {
                    textBox5.Text = "There is no encrypted message !!!" + Environment.NewLine;
                    return;
                }
                if (Elgamal(message, ref res, 1))
                {
                    textBox5.Text = res;
                }
                else
                {
                    textBox5.Text = "Error.Read Log";
                }
            }
            else if (cipher == "AES")
            {
                if (!String.IsNullOrWhiteSpace(textBox4.Text))
                {
                    message = textBox4.Text;
                }
                else
                {
                    textBox5.Text = "There is no encrypted message !!!" + Environment.NewLine;
                    return;
                }
                try
                {
                    if (AES_D(encryptedByte, ref res))
                    {
                        textBox5.Text = res;
                    }
                    else
                    {
                        textBox5.Text = "Error.Read Log";
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
            else if (cipher == "RSA")
            {
                if (!String.IsNullOrWhiteSpace(textBox4.Text))
                {
                    message = textBox4.Text;
                }
                else
                {
                    textBox5.Text = "There is no encrypted message !!!" + Environment.NewLine;
                    return;
                }
                try
                {
                    if (RSA(message, ref res, 1))
                    {
                        textBox5.Text = res;
                    }
                    else
                    {
                        textBox5.Text = "Error.Read Log";
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
            else if (cipher == "Diffi-Hellman") {
                if (!String.IsNullOrWhiteSpace(textBox4.Text))
                {
                    message = textBox4.Text;
                }
                else
                {
                    textBox5.Text = "There is no encrypted message !!!" + Environment.NewLine;
                    return;
                }
                try
                {
                    if (DiffiHellman(message, ref res, 1))
                    {
                        textBox5.Text = res;
                    }
                    else
                    {
                        textBox5.Text = "Error.Read Log";
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
            else
            {
                return;
            }
        }

        private void SelectionChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(comboBox1.Text))
            {
                cipher = comboBox1.SelectedItem.ToString();
            }
            log.Text = "";
            VigenerKey.Visible = false;
            CaesarKey.Visible = false;
            SKL.Visible = false;
            NKL.Visible = false;
            log.Visible = false;
            if (cipher == "Caesar")
            {
                CaesarKey.Visible = true;
                NKL.Visible = true;
            }
            else if (cipher == "Vigener")
            {
                VigenerKey.Visible = true;
                SKL.Visible = true;
            }
            else if (cipher == "Table with key")
            {
                VigenerKey.Visible = true;
                SKL.Visible = true;
            }
            else if (cipher == "Gronsfeld")
            {
                VigenerKey.Visible = true;
                SKL.Visible = true;
            }
            else if (cipher == "Witston")
            {
                VigenerKey.Visible = true;
                CaesarKey.Visible = false;
                SKL.Visible = true;
                log.Visible = false;
                SKL.Text = "Two words:";
            }
            else if (cipher == "Trisemus")
            {
                VigenerKey.Visible = true;
                SKL.Visible = true;
            }
            else if (cipher == "Caesar with key")
            {
                VigenerKey.Visible = true;
                CaesarKey.Visible = true;
                SKL.Visible = true;
                NKL.Visible = true;
            }
            else if (cipher == "Playfair")
            {
                VigenerKey.Visible = true;
                SKL.Visible = true;
            }
            else if (cipher == "XOR")
            {
                SKL.Visible = true;
                VigenerKey.Visible = true;
            }
            else if (cipher == "DES")
            {
                DES_Key = "";
            }
            else if (cipher == "Elgamal")
            {
            }
            else if (cipher == "RSA")
            {
            }
            else if (cipher == "Diffi-Hellman")
            {
            }
            else {
                return;
            }
        }
    }
}
