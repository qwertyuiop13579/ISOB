using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class Program
    {
        const string englishalfabet = "abcdefghijklmnopqrstuvwxyz";
        const string russianalfabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";

        private static string EncryptCaesar(string text, int k)
        {
            string Alfabet = "";
            int lettercount = 0;
            string resultstr = "";
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (1040 <= c && c <= 1071)        //Русский заглавный
                {
                    Alfabet = russianalfabet.ToUpper();
                    lettercount = russianalfabet.Length;
                }
                else if (1072 <= c && c <= 1103)          //Русский прописной
                {
                    Alfabet = russianalfabet.ToLower();
                    lettercount = russianalfabet.Length;
                }
                else if (65 <= c && c <= 90)      //Английский заглавный
                {
                    Alfabet = englishalfabet.ToUpper();
                    lettercount = englishalfabet.Length;
                }
                else if (97 <= c && c <= 122)         //Английский прописной           
                {
                    Alfabet = englishalfabet.ToLower();
                    lettercount = englishalfabet.Length;
                }
                int index = Alfabet.IndexOf(c);
                if (index < 0)
                {
                    resultstr += c.ToString();
                }
                else
                {
                    var codeIndex = (lettercount + index + k) % lettercount;
                    resultstr += Alfabet[codeIndex];
                }
            }

            return resultstr;
        }


        //генерация повторяющегося пароля
        private static string GetRepeatKey(string s, int n)
        {
            var p = s;
            while (p.Length < n)
            {
                p += p;
            }

            return p.Substring(0, n);
        }

        private static string Vigenere(string text, string password, bool encrypting)
        {
            string gamma = GetRepeatKey(password, text.Length);           
            string resultstr = "";

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                string Alfabet = "";
                int lettercount = 0;
                if (1040 <= c && c <= 1071)        //Русский заглавный
                {
                    Alfabet = russianalfabet.ToUpper();
                    lettercount = russianalfabet.Length;
                }
                else if (1072 <= c && c <= 1103)          //Русский прописной
                {
                    Alfabet = russianalfabet.ToLower();
                    lettercount = russianalfabet.Length;
                }
                else if (65 <= c && c <= 90)      //Английский заглавный
                {
                    Alfabet = englishalfabet.ToUpper();
                    lettercount = englishalfabet.Length;
                }
                else if (97 <= c && c <= 122)         //Английский прописной           
                {
                    Alfabet = englishalfabet.ToLower();
                    lettercount = englishalfabet.Length;
                }

                var letterIndex = Alfabet.IndexOf(text[i]);
                var codeIndex = Alfabet.IndexOf(gamma[i]);
                if (letterIndex < 0)         //если буква не найдена, добавляем её в исходном виде
                {
                    resultstr += text[i].ToString();
                }
                else
                {
                    resultstr += Alfabet[(lettercount + letterIndex + ((encrypting ? 1 : -1) * codeIndex)) % lettercount].ToString();
                }
            }

            return resultstr;
        }



        static void Main(string[] args)
        {
            int t = 0;
            bool good = false;
            while (t < 5)
            {
                do
                {
                    Console.WriteLine("Что сделать? 1-Зашифровать Цезарем 2-Дешифровать цезаря 3-Зашифровать Виженером 4-Дешифровать Виженера 5-Выход");
                    try
                    {
                        t = int.Parse(Console.ReadLine());
                        good = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Повторите ввод");
                        continue;
                    }
                }
                while (!good);
                
                switch (t)
                {
                    case 1:
                        {
                            string text = File.ReadAllText("input.txt");
                            Console.WriteLine("Исходный текст: " + text);
                            Console.Write("Введите ключ: ");
                            int key = 0;
                            try
                            {
                                key = int.Parse(Console.ReadLine());
                            }
                            catch(Exception ex)
                            {
                                break;
                            }
                            string result = EncryptCaesar(text, key);
                            Console.WriteLine("Зашифрованный текст: " + result);
                            File.WriteAllText("output.txt", result);
                            break;
                        }
                    case 2:
                        {
                            string text = File.ReadAllText("input.txt");
                            Console.WriteLine("Зашифрованный текст: " + text);
                            Console.Write("Введите ключ: ");
                            int key = 0;
                            try
                            {
                                key = int.Parse(Console.ReadLine());
                            }
                            catch (Exception ex)
                            {
                                break;
                            }
                            string result = EncryptCaesar(text, -key);
                            Console.WriteLine("Исходный текст: " + result);
                            File.WriteAllText("output.txt", result);
                            break;
                        }
                    case 3:
                        {
                            string text = File.ReadAllText("input.txt");
                            Console.WriteLine("Исходный текст: " + text);
                            Console.Write("Введите ключ: ");
                            string key = Console.ReadLine();
                            string result = Vigenere(text,key,true);
                            Console.WriteLine("Зашифрованный текст: " + result);
                            File.WriteAllText("output.txt", result);
                            break;
                        }
                    case 4:
                        {
                            string text = File.ReadAllText("input.txt");
                            Console.WriteLine("Зашифрованный текст: " + text);
                            Console.Write("Введите ключ: ");
                            string key = Console.ReadLine();
                            string result = Vigenere(text, key, false);
                            Console.WriteLine("Исходный текст: " + result);
                            File.WriteAllText("output.txt", result);
                            break;
                        }
                }


            }
            Console.ReadLine();
        }
    }
}
