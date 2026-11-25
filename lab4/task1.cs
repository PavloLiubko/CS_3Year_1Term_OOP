using System;

namespace Vector3FLab {

    enum VectorState
    {
        Ok = 0,
        NullArray = 1,
        ArrayTooShort = 2,
        DivideByZero = 3,
        IndexOutOfRange = 4,
        InvalidInput = 5
    }

    class Vector3F {

        private static int objectCount;

        private float x;
        private float y;
        private float z;

        public VectorState State { get; private set; }

        public static int Count
        {
            get { return objectCount; }
        }

        public Vector3F(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            objectCount++;
        }

        public Vector3F() : this(0f, 0f, 0f)
        {
        }

        public Vector3F(float value) : this(value, value, value)
        {
        }

        public Vector3F(float[] source)
        {
            if (source == null)
            {
                this.State = VectorState.NullArray;
            }
            else if (source.Length < 3)
            {
                this.State = VectorState.ArrayTooShort;
            }
            else
            {
                this.x = source[0];
                this.y = source[1];
                this.z = source[2];
            }
            objectCount++;
        }

        ~Vector3F()
        {
            objectCount--;
            Console.WriteLine($"Vector3F {ToString()} was deleted, State = {State}");
        }

        public float this[int index]
        {
            get
            {
                if (index == 0)
                {
                    return x;
                }
                if (index == 1)
                {
                    return y;
                }
                if (index == 2)
                {
                    return z;
                }
                State = VectorState.IndexOutOfRange;
                return z;
            }
            set
            {
                if (index == 0)
                {
                    x = value;
                }
                else if (index == 1)
                {
                    y = value;
                }
                else
                {
                    if (index != 2)
                    {
                        State = VectorState.IndexOutOfRange;
                    }
                    z = value;
                }
            }
        }

        public Vector3F Assign(Vector3F other)
        {
            this.x = other.x;
            this.y = other.y;
            this.z = other.z;
            return this;
        }

        private Vector3F CopyWithState(VectorState state)
        {
            Vector3F copy = new Vector3F(x, y, z);
            copy.State = state;
            return copy;
        }

        private static float SquareRoot(float number)
        {
            if (number <= 0f)
            {
                return 0f;
            }
            float guess = number;
            for (int step = 0; step < 100; step++)
            {
                float next = (guess + number / guess) / 2f;
                if (next == guess)
                {
                    break;
                }
                guess = next;
            }
            return guess;
        }

        private static bool TryParseNumber(string text, out float result)
        {
            result = 0f;
            int position = 0;
            bool isNegative = false;
            bool hasDigits = false;
            bool hasSeparator = false;
            float scale = 1f;

            if (text.Length > 0 && (text[0] == '-' || text[0] == '+'))
            {
                isNegative = text[0] == '-';
                position = 1;
            }

            while (position < text.Length)
            {
                char symbol = text[position];
                if (symbol >= '0' && symbol <= '9')
                {
                    int digit = symbol - '0';
                    hasDigits = true;
                    if (hasSeparator)
                    {
                        scale = scale / 10f;
                        result = result + digit * scale;
                    }
                    else
                    {
                        result = result * 10f + digit;
                    }
                }
                else if ((symbol == '.' || symbol == ',') && !hasSeparator)
                {
                    hasSeparator = true;
                }
                else
                {
                    result = 0f;
                    return false;
                }
                position++;
            }

            if (!hasDigits)
            {
                result = 0f;
                return false;
            }
            if (isNegative)
            {
                result = -result;
            }
            return true;
        }

        public static Vector3F operator ++(Vector3F vector)
        {
            return new Vector3F(vector.x + 1f, vector.y + 1f, vector.z + 1f);
        }

        public static Vector3F operator --(Vector3F vector)
        {
            return new Vector3F(vector.x - 1f, vector.y - 1f, vector.z - 1f);
        }

        public static bool operator !(Vector3F vector)
        {
            return vector.x != 0f && vector.y != 0f && vector.z != 0f;
        }

        public static Vector3F operator ~(Vector3F vector)
        {
            if (vector.x != 0f || vector.y != 0f)
            {
                return new Vector3F(-vector.y, vector.x, 0f);
            }
            return new Vector3F(vector.z, 0f, 0f);
        }

        public static Vector3F operator -(Vector3F vector)
        {
            return new Vector3F(-vector.x, -vector.y, -vector.z);
        }

        public static Vector3F operator +(Vector3F left, Vector3F right)
        {
            return new Vector3F(left.x + right.x, left.y + right.y, left.z + right.z);
        }

        public static Vector3F operator -(Vector3F left, Vector3F right)
        {
            return new Vector3F(left.x - right.x, left.y - right.y, left.z - right.z);
        }

        public static Vector3F operator *(Vector3F vector, float number)
        {
            return new Vector3F(vector.x * number, vector.y * number, vector.z * number);
        }

        public static Vector3F operator *(float number, Vector3F vector)
        {
            return vector * number;
        }

        public static Vector3F operator /(Vector3F vector, float number)
        {
            if (number == 0f)
            {
                return vector.CopyWithState(VectorState.DivideByZero);
            }
            return new Vector3F(vector.x / number, vector.y / number, vector.z / number);
        }

        public static Vector3F operator %(Vector3F vector, int divisor)
        {
            if (divisor == 0)
            {
                return vector.CopyWithState(VectorState.DivideByZero);
            }
            return new Vector3F(
                (float)((int)vector.x % divisor),
                (float)((int)vector.y % divisor),
                (float)((int)vector.z % divisor));
        }

        public static Vector3F operator &(Vector3F left, Vector3F right)
        {
            return new Vector3F(
                left.y * right.z - left.z * right.y,
                left.z * right.x - left.x * right.z,
                left.x * right.y - left.y * right.x);
        }

        public static bool operator ==(Vector3F left, Vector3F right)
        {
            if ((object)left == (object)right)
            {
                return true;
            }
            if ((object)left == null || (object)right == null)
            {
                return false;
            }
            return left.x == right.x && left.y == right.y && left.z == right.z;
        }

        public static bool operator !=(Vector3F left, Vector3F right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            Vector3F other = obj as Vector3F;
            if (other == null)
            {
                return false;
            }
            return this == other;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
        }

        public static bool operator >(Vector3F left, Vector3F right)
        {
            return left.x > right.x && left.y > right.y && left.z > right.z;
        }

        public static bool operator <(Vector3F left, Vector3F right)
        {
            return left.x < right.x && left.y < right.y && left.z < right.z;
        }

        public static bool operator >=(Vector3F left, Vector3F right)
        {
            return left.x >= right.x && left.y >= right.y && left.z >= right.z;
        }

        public static bool operator <=(Vector3F left, Vector3F right)
        {
            return left.x <= right.x && left.y <= right.y && left.z <= right.z;
        }

        public float Invoke()
        {
            return SquareRoot(x * x + y * y + z * z);
        }

        public override string ToString()
        {
            return $"({x}, {y}, {z})";
        }

        public void Print()
        {
            Console.Write(ToString());
        }

        public void Read()
        {
            ReadFromText(Console.ReadLine());
        }

        public void ReadFromText(string text)
        {
            float newX;
            float newY;
            float newZ;
            string[] parts = null;

            if (text != null)
            {
                parts = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }

            if (parts != null && parts.Length == 3
                && TryParseNumber(parts[0], out newX)
                && TryParseNumber(parts[1], out newY)
                && TryParseNumber(parts[2], out newZ))
            {
                this.x = newX;
                this.y = newY;
                this.z = newZ;
            }
            else
            {
                this.State = VectorState.InvalidInput;
            }
        }

    }

    class Program {

        public static void Main(string[] args) {

            Demo();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine($"Vectors left: {Vector3F.Count}");

        }

        private static void Demo()
        {
            Vector3F zero = new Vector3F();
            Vector3F filled = new Vector3F(7);
            Vector3F first = new Vector3F(new float[] { 1, 2, 3 });
            Vector3F second = new Vector3F(4, 5, 6);

            float[] missingArray = null;
            Vector3F broken = new Vector3F(missingArray);

            Console.WriteLine($"zero = {zero}, filled = {filled}, first = {first}, second = {second}");
            Console.WriteLine($"broken: State = {broken.State}");
            Console.WriteLine($"Vectors alive: {Vector3F.Count}");
            Console.WriteLine();

            Vector3F counter = new Vector3F(1, 1, 1);
            Vector3F previous = counter++;
            Console.WriteLine($"counter++ : previous = {previous}, counter = {counter}");
            Console.WriteLine($"--counter : {--counter}");
            Console.WriteLine($"!first = {!first}, !zero = {!zero}");
            Console.WriteLine($"~first = {~first}");
            Console.WriteLine($"-first = {-first}");
            Console.WriteLine();

            Console.WriteLine($"first + second = {first + second}");
            Console.WriteLine($"first - second = {first - second}");
            Console.WriteLine($"first * 2 = {first * 2f}");
            Console.WriteLine($"2 * first = {2f * first}");
            Console.WriteLine($"second / 2 = {second / 2f}");
            Console.WriteLine($"second % 4 = {second % 4}");
            Console.WriteLine($"first & second = {first & second}");
            Console.WriteLine();

            Vector3F accumulator = new Vector3F(1, 2, 3);
            accumulator += second;
            Console.WriteLine($"accumulator += second : {accumulator}");
            accumulator -= second;
            Console.WriteLine($"accumulator -= second : {accumulator}");
            accumulator *= 3f;
            Console.WriteLine($"accumulator *= 3 : {accumulator}");
            accumulator /= 3f;
            Console.WriteLine($"accumulator /= 3 : {accumulator}");
            accumulator &= second;
            Console.WriteLine($"accumulator &= second : {accumulator}");
            accumulator = new Vector3F(7, 8, 9);
            accumulator %= 4;
            Console.WriteLine($"accumulator %= 4 : {accumulator}");
            Console.WriteLine();

            Vector3F divided = first / 0f;
            Console.WriteLine($"first / 0 : State = {divided.State}");
            Vector3F remainder = first % 0;
            Console.WriteLine($"first % 0 : State = {remainder.State}");

            Vector3F indexed = new Vector3F(1, 2, 3);
            indexed[0] = 10f;
            Console.WriteLine($"indexed[0] = {indexed[0]}, indexed[1] = {indexed[1]}");
            float invalidValue = indexed[10];
            Console.WriteLine($"indexed[10] = {invalidValue}, State = {indexed.State}");
            Console.WriteLine();

            Vector3F copy = new Vector3F();
            copy.Assign(first);
            Console.WriteLine($"copy = {copy}, copy == first : {copy == first}, first == second : {first == second}");
            Console.WriteLine($"first != second : {first != second}");
            Console.WriteLine($"first < second : {first < second}, first > second : {first > second}");
            Console.WriteLine($"first <= copy : {first <= copy}, first >= copy : {first >= copy}");
            Console.WriteLine();

            Vector3F triangle = new Vector3F(3, 4, 0);
            Console.WriteLine($"Length of {triangle} = {triangle.Invoke()}");

            Vector3F parsed = new Vector3F();
            parsed.ReadFromText("1.5 2,5 -3");
            Console.WriteLine($"parsed = {parsed}");
            Vector3F invalid = new Vector3F();
            invalid.ReadFromText("abc");
            Console.WriteLine($"invalid input: State = {invalid.State}");
            Console.WriteLine();
        }

    }

}
