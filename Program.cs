using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace home9_newStorage
{

    class Program
    {
        static public void writeIntofile(string fileName, string text)
        {
            if (!File.Exists(fileName))
            {
                File.Create(fileName);

            }
            using (StreamWriter stream = File.AppendText(fileName))
            {
                stream.WriteLine(text);
                
            }
        }
        public static void showMessage(string s)
        {
            Console.WriteLine(s);
        }
        static public bool parse(out Product element){
            string s = Console.ReadLine();
            
            
            while (!Product.tryParse(s,out element))
            {
                showMessage("Your input is uncorrect. Please,try again ");
                s = Console.ReadLine();
            }
            
            return true;
        }
       
        static void Main(string[] args)
        {
            Storage s = new Storage();
            s.writeUncorrectInput += writeIntofile;
            s.fileName = "incorrectInput.txt";
            s.notify += showMessage;
            s.parseElement += parse;
            s.readFromFile(@"C:\Users\Надія\Desktop\Sigma c#\projects\home1\home1\bin\Debug\ProductsList.txt");
            Console.WriteLine(s);
            


        }
    }
}
