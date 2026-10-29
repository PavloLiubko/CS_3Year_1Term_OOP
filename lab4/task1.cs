using System.Drawing;
using System.Numerics;

namespace lab3
{

    class Vector3F
    {

        private double x;
        private double y;
        private double z;
        private int state; // 0 - good, 1 - error 

        public Vector3F(double X, double Y, double Z, int State)
        {
            this.x = X;
            this.y = Y;
            this.z = Z;
            this.state = State;
        }

        public Vector3F()
        {

            this.x = 0;
            this.y = 0;
            this.z = 0;
            this.state = 0;

        }

        public Vector3F(double num)
        {

            this.x = num;
            this.y = num;
            this.z = num;
            this.state = 0;

        }

        Vector3F(double[] nums)
        {

            this.x = nums[0];
            this.y = nums[1];
            this.z = nums[2];
            this.state = 0;

        }

        public Vector3F(Vector3F OtherVector)
        {

            this.x = OtherVector.x;
            this.y = OtherVector.y;
            this.z = OtherVector.z;

        }

        ~Vector3F()
        {

            Console.WriteLine($"Vector was deleted. Destructor`s state had code {this.state}");

        }

        public void print()
        {

            Console.Write($"Vector`s coordinates: \n x: {this.x} \n y: {this.y} \n z: {this.z} \n Vector condition code: {this.state}");
            Console.WriteLine(' ');

        }

        public static Vector3F SumVector(Vector3F Vector1, Vector3F Vector2)
        {

            return new Vector3F(Vector1.x + Vector2.x, Vector1.y + Vector2.y, Vector1.z + Vector2.z, 0);

        }

        public static Vector3F MinusVector(Vector3F Vector1, Vector3F Vector2)
        {

            return new Vector3F(Vector1.x - Vector2.x, Vector1.y - Vector2.y, Vector1.z - Vector2.z, 0);

        }

        public static Vector3F TimeVector(Vector3F Vector1, Vector3F Vector2)
        {

            return new Vector3F(Vector1.y * Vector2.z - Vector1.z * Vector2.y, Vector1.z * Vector2.x - Vector1.x * Vector2.z, Vector1.x * Vector2.y - Vector1.y * Vector2.x, 0);

        }

        public static Vector3F DivisionByNumber(Vector3F Vector, Int16 Num)
        {

            if (Num == 0)
            {

                return new Vector3F(Vector.x, Vector.y, Vector.z, 1);

            }

            return new Vector3F((Vector.x / Num), (Vector.y / Num), (Vector.z / Num), 0);


        }

        public static bool CompareVectors(Vector3F Vector1, Vector3F Vector2)
        {
            // true -> bigger, false -> lesser
            if (Vector1.x > Vector2.x && Vector1.y > Vector2.y && Vector1.z > Vector2.z)
            {

                return true;

            }

            else
            {

                return false;

            }
        }

        public static Vector3F operator ++(Vector3F vector) {
            return new(vector.x++, vector.y++, vector.z++, vector.state = 0);
        }

        public static Vector3F operator --(Vector3F vector)
        {
            return new(vector.x--, vector.y--, vector.z--, vector.state = 0);
        }

        public static bool operator !(Vector3F vector) {

            if (vector.x != 0 && vector.y != 0 && vector.z != 0) {
                return true;
            }
            return false;
        
        }

        public static Vector3F operator ~(Vector3F vector) {  // припустимо шукаємо перпендикулярну орту

            return new(

                (-(vector.y) / Math.Pow(Math.Pow(vector.x, 2) + Math.Pow(vector.y, 2), 0.5)),
                (vector.x / Math.Pow(Math.Pow(vector.x, 2) + Math.Pow(vector.y, 2), 0.5)),
                0, 
                0

                );

        }





        public static void Main(string[] args)
        {

           
        }

    }

}
