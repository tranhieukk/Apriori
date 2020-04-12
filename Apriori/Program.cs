using System;
using System.IO;
using System.Reflection.Metadata;

namespace Apriori
{
    class Program
    {

        const int SoCot = 100;
        const int Shang = 500;
        const float R = (float)0.002;
        const float Dr = (float)0.002;


        static  string S1, S, Sf;
        static DateTime TDateTime;
        static string Str1, Str2;
        static string F_Ketqua;
        static int NTT;
        static long Ts;





        //---------------------Tinh Support cho tung Item-------------------------}
       static void  Support(string Sf, int NTT)
        {
            int[] M = new int[Program.SoCot+1];
            string S="";
            string F1 = "", F2 = "", F3 = "";
            int k = 0, C = 0, Code = 0, SMDL = 0;
            
            
            StreamReader F01 = new StreamReader(@"Data_" + Sf + ".Txt");
            StreamWriter F02 = new StreamWriter(@"Data_" + Sf + "_1_" + NTT + ".Txt", false);
            StreamWriter F03 = new StreamWriter(@"SupData_" + Sf + ".Txt", false);

            for (C = 1; C <= Program.SoCot; C++)
                M[C] = 0;

            Console.WriteLine("Dang tinh Support");
            while ((S = F01.ReadLine()) != null)
            {
                ++k;

                if (k % 1000 == 0) Console.Write(k + "\t");

                while (!";".Contains(S))
                {
                    S1 = S.Substring(0, S.IndexOf(';'));

                    C = Int32.Parse(S1);
                    M[C] = M[C] + 1;
                    if (C > SMDL) SMDL = C;
                    S=S.Remove(0, S.IndexOf(';')+1);
                }
            }
            Console.WriteLine("");
            F_Ketqua += ("Tap tin co " + k + " giao tac /n");
            F_Ketqua += ("Tap tin co " + SMDL + " muc du lieu /n");
            k = 0;
            for (C = 1; C <= SMDL; C++)
            {
                F03.WriteLine(C + ":" + M[C]);
                if (M[C] >= NTT)

                {
                    F02.WriteLine(C);
                    ++k;
                };
            };
            F_Ketqua += ("Co"+ k+ " muc du lieu thuong xuyen");
            Console.WriteLine("");
            Console.WriteLine("Tinh support xong!");

            F01.Close();
            F02.Close();
            F03.Close();

        }

           







        static void Main(string[] args)
        {
            Program.Support("CH19_11", 2);
        }
    }
}
