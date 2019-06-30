using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windkraftanlage.Mathematikwerkzeuge
{
    public struct Vektor2 : IEquatable<Vektor2>
    {

        public double x { get; set; }
        public double y;


        public Vektor2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Vektor2(Vektor2 v1, Vektor2 v2)
        {
            x = v1.x - v2.x;
            y = v1.y - v2.y;
        }

        public static Vektor2 Addiere(Vektor2 v1, Vektor2 v2)
        {
            v1.x += v2.x;
            v1.y += v2.y;
            return v1;
        }

        public double Norm()
        {
            return Math.Sqrt(x * x + y * y);
        }

        public static Vektor2 Zero()
        {
            return new Vektor2(0,0);
        }

        public bool Equals(Vektor2 v)
        {
            return (x == v.x) && (y == v.y);
        }
        
        public static Vektor2 operator +(Vektor2 v1, Vektor2 v2)
        {
            return Addiere(v1, v2);
        }

        public static Vektor2 operator -(Vektor2 v1, Vektor2 v2)
        {
            v1.x -= v2.x;
            v1.y -= v2.y;
            return v1;
        }

        public static Vektor2 operator -(Vektor2 v)
        {
            v.x = -v.x;
            v.y = -v.y;
            return v;
        }

        public static Vektor2 operator *(double lambda, Vektor2 v)
        {
            v.x *= lambda;
            v.y *= lambda;
            return v;
        }

        public static Vektor2 operator *(Vektor2 v, double lambda)
        {
            return lambda * v;
        }
              
    }
}
