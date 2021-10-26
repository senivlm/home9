using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace home9_newStorage
{
    public delegate void stringOperating(string str);
    public delegate bool doWithElement<T>( T element);
    public delegate bool changeElement<T>(out T  element);
    public delegate void writeToFile(string filename, string uncrorrectInput);
    
    class Storage: IEnumerable
    {
        public event stringOperating notify;
        public event changeElement<Product> parseElement;
        public event doWithElement<Product> check;
        public event writeToFile writeUncorrectInput;
        public string fileName;
        public List<Product> listProducts { get; private set; }
        public Storage(IEnumerable <Product>colection)
        {
            listProducts = new List<Product>(colection);
            
            
        }
        public Storage()
        {
            listProducts = new List<Product>();
        }
        public Product this[int index]
        {
            get => listProducts[index];
            set => listProducts[index] = value;
        }
        public IEnumerator GetEnumerator()
        {
            return new Enumerator(this);
        }
        class Enumerator : IEnumerator<Product>
        {
            int position = -1;

            List<Product> products;

            public Enumerator(Storage storage)
            {
                this.products = storage.listProducts;
                
            }
            public Product Current => products[position];

            object IEnumerator.Current => products.ElementAt(position);

            public void Dispose()
            {

            }

            public bool MoveNext()
            {
                if (position + 1 <products.Count() )
                {
                    position += 1;
                    return true;
                }
                return false;

            }

            public void Reset()
            {
                position = -1;
            }
        }
        public override string ToString()
        {
            StringBuilder value = new StringBuilder();
            
            foreach (Product a in listProducts)
            {
                value.Append( a);
            }
            List<Product> expired = find((x) => x.isExpired());
            if(expired.Count>0)
                value.Append("The terminated productList\n");

            foreach(Product a in expired)
            {
                value.Append(a);
            }
            return value.ToString();
        }
        public void deleteExpireDairyProduct()
        {
            DateTime currentDate = DateTime.Now;
            int amount = 0,size=listProducts.Count();
            
            for (int i = 0; i < size; i++)
            {
                if (listProducts[i].manufactureDate + listProducts[i].expireDays > currentDate)
                {
                    listProducts.Remove(listProducts[i]);
                }
            }
        }

        public void readFromFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                string[] lines = File.ReadAllLines(fileName);
                for (int i = 0, j = 0;i < lines.Length; i++)
                {
                    if (lines[i] == "")
                        continue;
                    try {
                        listProducts.Add(Product.Parse(lines[i]));
                        ++j;
                    }
                    catch (Exception ex){
                        notify?.Invoke(ex.Message+$" in the line {i}.\nIf you want to fix it,please, press the 'f'key." +
                            $"If you want write uncorrect string into file, press 'w'.");
                        string readed=Console.ReadLine();
                        while(!(readed=="w" || readed == "f"))
                        {
                            Console.WriteLine("You press uncorrect key. Please, try again");
                            readed = Console.ReadLine();
                        }
                        if (readed == "f")
                        {

                            Product temp;
                            parseElement(out temp);
                            listProducts.Add(temp);
                            ++j;
                        }
                        else
                        {
                            writeUncorrectInput?.Invoke(this.fileName, lines[i] + DateTime.Now+"\n");
                        }
                    }
                }
            }
        }

        public void addProduct(Product product)
        {
            listProducts.Add(product);
        }
        public void removeProduct(Product product)
        {
            listProducts.Remove(product);
        }
        public void removeProduct(string name)
        {
            foreach(Product product in listProducts)
            {
                if (product.name == name)
                {
                    listProducts.Remove(product);
                }
            } 
        }
        List<Product> find(doWithElement<Product> func)
        {
            List<Product> products=new List<Product>();
            foreach(var el in listProducts)
            {
                if (func(el))
                {
                    products.Add(el);
                }
            }
            return products;
        }
    }


}
