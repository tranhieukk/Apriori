using System;
using System.IO;
using System.Threading;

namespace Apriori
{
    class Program
    {

        const int SoCot = 100;
        const int Shang = 500;
        const float R = (float)0.002;
        const float Dr = (float)0.002;


        static string S1, S, Sf;
        static string Str1, Str2;
        static StreamWriter F_Ketqua;
        static int NTT;
        static long Ts;

        static StreamReader RestartFile( StreamReader reader)
        {
            string path = (reader.BaseStream as FileStream)?.Name;
            reader.Close();
            return new StreamReader(path);
        }



        //---------------------Tinh Support cho tung Item-------------------------}
        static void Support(string Sf, int NTT)
        {
            int[] M = new int[Program.SoCot + 1];
            string S = "";
            string F1 = "", F2 = "", F3 = "";
            int k = 0, C = 0, Code = 0, SMDL = 0;


            StreamReader F01 = new StreamReader(@"Data_" + Sf + ".Txt");
            StreamWriter F02 = new StreamWriter(@"TTX_" + Sf + "_1_" + NTT + ".Txt", false);
            StreamWriter F03 = new StreamWriter(@"SupData_" + Sf + ".Txt", false);
             

            for (C = 1; C <= Program.SoCot; C++)
                M[C] = 0;
            Console.WriteLine();
            Console.WriteLine("\t ♦ Dang tinh Support");
            while ((S = F01.ReadLine()) != null)
            {
                ++k;

                if (k % 1000 == 0) Console.Write(k + "\t");
                
                while (!";".Contains(S))
                {
                    S1 = S.Substring(0, S.IndexOf(";"));

                    C = Int32.Parse(S1);
                    M[C] = M[C] + 1;
                    if (C > SMDL) SMDL = C;
                    S = S.Remove(0, S.IndexOf(";") + 1);
                }
            }
            Console.WriteLine("");
            F_Ketqua.WriteLine( "Tap tin co " + k + " giao tac") ;
            F_Ketqua.WriteLine("Tap tin co " + SMDL + " muc du lieu");
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
            F_Ketqua.WriteLine("Co " + k + " muc du lieu thuong xuyen");

            F01.Close();
            F02.Close();
            F03.Close();
        }


        static void TinhTUV2(string Sf, ref long Ts, int Ntt)
        {
            StreamReader F_Ttx;
            StreamWriter F_Tuv, F_TKN;
            int i, L1;
            string S1, S2;

            F_Ttx = new StreamReader(@"TTX_" + Sf + "_1_" + Ntt + ".Txt");
            F_Tuv = new StreamWriter(@"TUV_" + Sf + "_2_" + Ntt + ".txt", false);
            F_TKN = new StreamWriter(@"TKN_" + Sf + "_2_" + Ntt + ".txt", false);


            Console.WriteLine("\t ♦ Dang tinh tap ung vien 2 muc du lieu");
            Ts = 0;
            L1 = 0;
            while ((S1 = F_Ttx.ReadLine()) != null)
            {
                while ((S2 = F_Ttx.ReadLine()) != null)
                {
                    F_Tuv.WriteLine(S1 + ";" + S2 + ";");
                    F_TKN.WriteLine(S1 + ";" + S2 + ";");

                    Ts++;
                    if (Ts % 1000 == 0) Console.Write(Ts + "\t");
                }
                L1++;
                F_Ttx.BaseStream.Seek(0, SeekOrigin.Begin);
                for (i = 1; i <= L1; i++)
                    S1 = F_Ttx.ReadLine();

            }
            F_Ketqua.WriteLine("Co " + Ts + " tap ung vien 2 muc du lieu");
            F_Ttx.Close();
            F_Tuv.Close();
            F_TKN.Close();
        }

        static void TinhCK_Ghep(string Sf, int k, int Ntt, ref long Ts)
        {
            StreamReader F_In;
            StreamWriter F_O;
            string Ss1, Ss2, Ss3, S, S1, S2, S3, S4, S5;
            int j, L1;
            bool Tiep;

            Ss1 = "" + (k - 1);
            Ss2 = "" + Ntt;
            Ss3 = "" + k;

            F_In = new StreamReader(@"TTX_" + Sf + "_" + Ss1 + "_" + Ss2 + ".Txt");
            F_O = new StreamWriter(@"TKN_" + Sf + "_" + Ss3 + "_" + Ss2 + ".Txt", false);

            L1 = 0;
            Ts = 0;
            Console.WriteLine("\t ♦ Dang ghep cac tap thuong xuyen ", k - 1, " muc du lieu");

            while ((S1 = F_In.ReadLine()) != null)
            {
                S2 = S1;
                S3 = "";
                for (j = 1; j <= k - 2; j++)
                {
                    S3 = S3 + S2.Substring(0, S2.IndexOf(";")+1);

                   S2= S2.Remove(0, S2.IndexOf(";") + 1);
                }
                Tiep = !F_In.EndOfStream;
                while ((!F_In.EndOfStream) && Tiep)
                {
                    S4 = F_In.ReadLine();
                    S5 = "";
                    for (j = 1; j <= k - 2; j++)
                    {
                        S5 = S5 + S4.Substring(0, S4.IndexOf(";")+1);
                       S4= S4.Remove(0, S4.IndexOf(";")+1);
                    }
                    if (S3 == S5)
                    {
                        Ts = Ts + 1;
                        F_O.WriteLine(S1 + S4);
                        Tiep = true;
                        if (Ts % 1000 == 0) Console.Write(Ts + "\t");
                    }
                    else Tiep = false;
                }
                F_In.BaseStream.Seek(0, SeekOrigin.Begin);
                L1 = L1 + 1;
                for (j = 1; j <= L1; j++) S = F_In.ReadLine();
            }
            F_Ketqua.WriteLine("Co " + Ts + " tap " + k + " muc du lieu duoc ghep");
            F_In.Close();
            F_O.Close();
        }

        //------------Tim tap thuong xuyen -------------------  
        static void TimTapTX(int kk, string Sf, int Ntt, ref long Ts)
        {
            StreamReader F_Data, F_Tuv;
            StreamWriter F_Ttx;
            string SUV, S1, SS, S, Gtac, SMtx, SNtt;
            int I, Sup;
            bool Tiep, co;
            string[] MS;

            SMtx = "" + kk;
            SNtt = "" + Ntt;

            F_Tuv = new StreamReader(@"TUV_" + Sf + "_" + SMtx + "_" + SNtt + ".Txt");
            F_Ttx = new StreamWriter(@"TTX_" + Sf + "_" + SMtx + "_" + SNtt + ".Txt", false);
            F_Data = new StreamReader(@"Data_" + Sf + ".Txt");

            Str1 = DateTime.Now.ToString("h:mm:ss tt - dd/MM/yyyy");

            Console.WriteLine();
            Console.WriteLine("\t ♦ Dang tinh tap thuong xuyen ", kk, " muc du lieu");
            Ts = 0;
          
            while ((SUV = F_Tuv.ReadLine()) != null)
            {

                if (SUV == "19;20;")
                        SUV = SUV;
                Tiep = true;
                Sup = 0;
                F_Data = RestartFile(F_Data);
                while ((Gtac = F_Data.ReadLine())!=null && Tiep )
                {
                   
                    S = SUV;
                    co = true;
                    Gtac = ";" + Gtac;
                    while ((S != "") && co)
                    {
                        S1 = ";" + S.Substring(0, S.IndexOf(";")+1);
                        co = Gtac.IndexOf(S1) >-1;
                        S= S.Remove(0, S.IndexOf(";")+1);
                   
                    }
                    if (co) 
                        Sup = Sup + 1;
                    if (Sup >= Ntt)
                    {
                        F_Ttx.WriteLine(SUV);
                        Tiep = false;
                        Ts = Ts + 1;
                        if (Ts % 1000 == 0) Console.Write(Ts + "\t");

                    }
                }
            }
            F_Tuv.Close();
            F_Data.Close();
            F_Ttx.Close();
            Console.WriteLine();
            F_Ketqua.WriteLine("Co " + Ts + " tap thuong xuyen " + kk + " muc du lieu");

        }

        static void TinhTapUngVien(int kk, string Sf, long Ntt, ref long TS)
        {
            StreamReader F_Ck, F_Ttx;
            StreamWriter F_Tuv;
            string SUv, s, SS, STx, STck, SNtt;
            int i, j, K;
            bool Co;
            string[] MS = new string[Program.SoCot + 1];

            STx = "" + (kk - 1);
            STck = "" + kk;
            SNtt = "" + Ntt;

            F_Ttx = new StreamReader(@"TTX_" + Sf + "_" + STx + "_" + SNtt + ".Txt");
            F_Ck = new StreamReader(@"TKN_" + Sf + "_" + STck + "_" + SNtt + ".Txt");
            F_Tuv = new StreamWriter(@"TUV_" + Sf + "_" + STck + "_" + SNtt + ".Txt", false);

            Console.WriteLine();
            Console.WriteLine("\t ♦ Dang tinh tap ung vien ", kk, " muc du lieu");
            Console.WriteLine();
            TS = 0;
            for (i = 1; i <= SoCot; i++) MS[i] = "";

            while ((STck = F_Ck.ReadLine()) != null)
            {

                S = STck;

                // đặt item ketnoi vào array
                for (i = 1; i <= kk; i++)
                {
                    MS[i] = S.Substring(0, S.IndexOf(";")+1);

                    S = S.Remove(0, S.IndexOf(";") + 1);
                }
                Co = true;

                for (K = 1; K <= kk - 2; K++)
                {
                    SUv = "";
                    for (i = 1; i <= kk; i++)
                    {
                        if (i != K)
                        {
                            SUv = SUv + MS[i];
                        }
                    }

                    F_Ttx = RestartFile(F_Ttx);
                    Co = false;
                    while ((!F_Ttx.EndOfStream) && (!Co))
                    {
                        SS = F_Ttx.ReadLine();
                        Co = (SS == SUv);
                    }
                }
                if (Co)
                {
                    F_Tuv.WriteLine(STck);
                    TS = TS + 1;
                    if (TS % 1000 == 0) Console.Write(TS + "\t");

                }
            }


            //        Close(F_Ttx);
            F_Ck.Close();
            F_Tuv.Close();
            F_Ketqua.WriteLine("Co " + Ts + " tap ung vien " + kk + " muc du lieu");
            Console.WriteLine();

        }
        
        static void Apriori(string Sf,int Ntt)
    {
            F_Ketqua = new StreamWriter(@"KQ_" + Sf + "_" + NTT + ".Txt", false);
            string SNtt;
            int  LL;

            Str1 = DateTime.Now.ToString("h:mm:ss tt - dd/MM/yyyy");
           
            SNtt=""+Ntt;
            Console.WriteLine("Chuong trinh bat dau luc: "+ Str1);

            F_Ketqua.WriteLine("Chuong trinh bat dau luc "+ Str1);
            F_Ketqua.WriteLine("Tap giao tac: Data_"+ Sf+".Txt");
            F_Ketqua.WriteLine("Nguong toi thieu: "+ SNtt);
            Support(Sf, NTT);
        TinhTUV2(Sf,ref Ts, Ntt);
        TimTapTX(2, Sf, NTT, ref Ts);
        LL=3;
            do
            {
                TinhCK_Ghep(Sf, LL, Ntt, ref Ts);
                if (Ts > 0)
                {
                    TinhTapUngVien(LL, Sf, Ntt, ref Ts);
                }
                if (Ts > 0)
                {
                    TimTapTX(LL, Sf, Ntt, ref Ts);
                }
                LL = LL + 1;
            }
            while (!(Ts == 0)); // Ts=0;
            Str1 = Str1 = DateTime.Now.ToString("h:mm:ss tt - dd/MM/yyyy ") + Environment.NewLine;
            Console.WriteLine();
            Console.WriteLine("Chuong trinh ket thuc luc:" + Str1);
            F_Ketqua.WriteLine("Chuong trinh ket thuc luc:" + Str1);

            F_Ketqua.Close();


    }
        static void Main(string[] args)
        {
            Console.WriteLine("Chon so du lieu lam viec ");
            Program.Sf = "2_CH19_11";// Console.ReadLine();
            Console.WriteLine("Nhap nguong toi thieu: ");
            NTT= Convert.ToInt32(Console.ReadLine());

            if (NTT > 0)
            {
                Apriori(Sf, NTT);
            }
            Console.ReadKey();
         
        }
    }
}
