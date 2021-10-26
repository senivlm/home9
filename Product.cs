using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace home9_newStorage
{

    class Product
    {
        public Product(string name, double cost, double weight, DateTime manufactureDate, TimeSpan expireDays)
           : this(name, cost, weight)
        {

            this.manufactureDate = manufactureDate;
            this.expireDays = expireDays;
        }
        public override string ToString()
        {
            return "class Product: name = " + name + ", cost = " + Cost + ", weight= " + weight +
                ", manufactureDate = " + manufactureDate + ", days to expire = " + expireDays.TotalDays + "\n\n";
        }
        public Product(string name, double cost, double weight)
        {
            this.name = name;
            this.Cost = cost;
            this.weight = weight;

        }
        public DateTime manufactureDate;
        public TimeSpan expireDays;
        public Product() { }
        public string name;
        protected double cost;
        
        public static Product Parse(string s)
        {
            Product product=new Product();
            string[] words = s.Split();

            if (words.Length != 5)
            {
                throw new FormatException("Invalid format to parse Product\n");
            }
            product.name = words[0];
            double temp;
            if (double.TryParse(words[1], out temp))
            {
                product.Cost = temp;
            }
            if (double.TryParse(words[2], out temp))
            {
                product.weight = temp;
            }
            if (words[3].Count(ch => ch == '/') == 2)
            {
                if (!DateTime.TryParse(words[3], out product.manufactureDate))
                {
                    throw new ArgumentException("Invalid parameters\n");
                }
            }
            if (!TimeSpan.TryParse(words[4], out product.expireDays))
            {
                throw new ArgumentException("Invalid parameters\n");

            }
            return product;

        }
        public static bool tryParse(string s,out Product product)
        {
            product=new Product();
            try
            {
                product = Product.Parse(s);
                return true;

            }catch(Exception ex)
            { 
                return false;
            }
        }
        virtual public void changeCost(double percent)
        {
            cost *= percent;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Product);
        }
        public bool isExpired()
        {
            DateTime currentDate = DateTime.Now;
            return (manufactureDate + expireDays) < currentDate;
        }
        public bool Equals(Product other)
        {
            return other != null && name == other.name;
        }
        public int CompareTo(Product other)
        {
            if (other is null)
            {
                throw new ArgumentNullException("sjdfskdfjsdf");
            }
            return this.cost > other.cost ? 1 :
                this.cost < other.cost ? -1 : 0;
        }

        public override int GetHashCode()
        {
            var hashCode = -1674748306;
            hashCode = hashCode * -1521134295 + manufactureDate.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<TimeSpan>.Default.GetHashCode(expireDays);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            hashCode = hashCode * -1521134295 + cost.GetHashCode();
            hashCode = hashCode * -1521134295 + Cost.GetHashCode();
            hashCode = hashCode * -1521134295 + weight.GetHashCode();
            return hashCode;
        }

        public double Cost
        {
            set
            {
                if (value >= 0)
                {
                    cost = value;
                }
                else
                {
                    cost = -value;
                }
            }
            get
            {
                return cost;
            }
        }
        public double weight;
        public static Product operator +(Product p1, Product p2)
        {
            return new Product { cost = p1.cost + p2.cost };
        }
        
        public static bool operator >(Product p1, Product p2)
        {
            
            return p1.CompareTo(p2) > 0;
            //return p1.cost > p2.cost;
        }
        public static bool operator ==(Product p1, Product p2)
        {

            return p1.CompareTo(p2) == 0;
            //return p1.cost > p2.cost;
        }
        public static bool operator !=(Product p1, Product p2)
        {
            return p1?.CompareTo(p2) != 0;
            //return p1.cost > p2.cost;
        }
        public static bool operator <(Product p1, Product p2)
        {
            return p1?.CompareTo(p2) < 0;
            //  return p1.cost < p2.cost;
        }
        /* public static explicit operator Product(double val)
          {
              return new Product { cost = val };
          }*/
        public static implicit operator Product(double val)
        {
            return new Product { cost = val };
        }
        public static explicit operator double(Product p1)
        {
            return p1.cost;
        }

    }
}
