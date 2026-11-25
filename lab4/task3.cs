using System;

namespace MatrixShortLab {

    enum ErrorCode
    {
        Ok = 0,
        NoMemory = 1,
        InvalidSize = 2,
        IndexOutOfRange = 3,
        SizeMismatch = 4,
        DivideByZero = 5,
        NullArray = 6,
        InvalidInput = 7
    }

    class VectorShort {

        private static int objectCount;

        private short[] data;

        public ErrorCode State { get; internal set; }

        public static int Count
        {
            get { return objectCount; }
        }

        public int Length
        {
            get
            {
                if (data == null)
                {
                    return 0;
                }
                return data.Length;
            }
        }

        public VectorShort(int length, short value)
        {
            objectCount++;
            if (length < 0)
            {
                State = ErrorCode.InvalidSize;
                data = new short[0];
                return;
            }
            try
            {
                data = new short[length];
                for (int i = 0; i < length; i++)
                {
                    data[i] = value;
                }
            }
            catch (OutOfMemoryException)
            {
                State = ErrorCode.NoMemory;
                data = new short[0];
            }
        }

        public VectorShort() : this(0, (short)0)
        {
        }

        public VectorShort(int length) : this(length, (short)0)
        {
        }

        public VectorShort(VectorShort other) : this(other == null ? 0 : other.Length, (short)0)
        {
            if (other == null)
            {
                State = ErrorCode.NullArray;
                return;
            }
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = other.data[i];
            }
        }

        ~VectorShort()
        {
            objectCount--;
        }

        public short this[int index]
        {
            get
            {
                if (data == null || data.Length == 0)
                {
                    State = ErrorCode.IndexOutOfRange;
                    return 0;
                }
                if (index < 0 || index >= data.Length)
                {
                    State = ErrorCode.IndexOutOfRange;
                    return data[data.Length - 1];
                }
                return data[index];
            }
            set
            {
                if (data == null || data.Length == 0)
                {
                    State = ErrorCode.IndexOutOfRange;
                    return;
                }
                if (index < 0 || index >= data.Length)
                {
                    State = ErrorCode.IndexOutOfRange;
                    data[data.Length - 1] = value;
                    return;
                }
                data[index] = value;
            }
        }

        public override string ToString()
        {
            string text = "(";
            for (int i = 0; i < Length; i++)
            {
                if (i > 0)
                {
                    text += ", ";
                }
                text += data[i];
            }
            return text + ")";
        }

    }

    class MatrixShort {

        private static int objectCount;

        protected VectorShort[] shortArray;
        protected int n;
        protected int size;

        public ErrorCode State { get; private set; }

        public static int Count
        {
            get { return objectCount; }
        }

        public int N
        {
            get { return n; }
        }

        public int Size
        {
            get { return size; }
        }

        public MatrixShort(int rows, int columns, short value)
        {
            objectCount++;
            Allocate(rows, columns, value);
        }

        public MatrixShort() : this(0, 0, (short)0)
        {
        }

        public MatrixShort(int size) : this(size, size, (short)0)
        {
            if (shortArray != null)
            {
                for (int i = 0; i < this.size; i++)
                {
                    shortArray[i][i] = 1;
                }
            }
        }

        public MatrixShort(int rows, int columns) : this(rows, columns, (short)0)
        {
        }

        public MatrixShort(MatrixShort other)
            : this(other == null ? 0 : other.n, other == null ? 0 : other.size, (short)0)
        {
            if (other == null)
            {
                State = ErrorCode.NullArray;
                return;
            }
            CopyValues(other);
        }

        ~MatrixShort()
        {
            objectCount--;
        }

        private void Allocate(int rows, int columns, short value)
        {
            shortArray = null;
            n = 0;
            size = 0;
            if (rows < 0 || columns < 0)
            {
                State = ErrorCode.InvalidSize;
                return;
            }
            if (rows == 0 || columns == 0)
            {
                return;
            }
            try
            {
                VectorShort[] created = new VectorShort[rows];
                for (int i = 0; i < rows; i++)
                {
                    created[i] = new VectorShort(columns, value);
                    if (created[i].State != ErrorCode.Ok)
                    {
                        State = created[i].State;
                        return;
                    }
                }
                shortArray = created;
                n = rows;
                size = columns;
            }
            catch (OutOfMemoryException)
            {
                State = ErrorCode.NoMemory;
            }
        }

        private void CopyValues(MatrixShort other)
        {
            if (shortArray == null || other.shortArray == null)
            {
                return;
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    shortArray[i][j] = other.shortArray[i][j];
                }
            }
        }

        private MatrixShort CopyWithState(ErrorCode state)
        {
            MatrixShort copy = new MatrixShort(this);
            copy.State = state;
            return copy;
        }

        private bool SameSize(MatrixShort other)
        {
            return n == other.n && size == other.size;
        }

        public MatrixShort Assign(MatrixShort other)
        {
            if (other == null)
            {
                State = ErrorCode.NullArray;
                return this;
            }
            if ((object)this == (object)other)
            {
                return this;
            }
            Allocate(other.n, other.size, 0);
            CopyValues(other);
            return this;
        }

        public VectorShort this[int index]
        {
            get
            {
                if (shortArray == null)
                {
                    State = ErrorCode.IndexOutOfRange;
                    return null;
                }
                if (index < 0 || index >= n)
                {
                    State = ErrorCode.IndexOutOfRange;
                    return shortArray[n - 1];
                }
                return shortArray[index];
            }
        }

        // Аналог operator() з C++: доступ до елемента за двома індексами
        public short Invoke(int row, int column)
        {
            VectorShort line = this[row];
            if (line == null)
            {
                return 0;
            }
            return line[column];
        }

        public static MatrixShort operator ++(MatrixShort matrix)
        {
            MatrixShort result = new MatrixShort(matrix.n, matrix.size);
            for (int i = 0; i < matrix.n; i++)
            {
                for (int j = 0; j < matrix.size; j++)
                {
                    result.shortArray[i][j] = (short)(matrix.shortArray[i][j] + 1);
                }
            }
            return result;
        }

        public static MatrixShort operator --(MatrixShort matrix)
        {
            MatrixShort result = new MatrixShort(matrix.n, matrix.size);
            for (int i = 0; i < matrix.n; i++)
            {
                for (int j = 0; j < matrix.size; j++)
                {
                    result.shortArray[i][j] = (short)(matrix.shortArray[i][j] - 1);
                }
            }
            return result;
        }

        public static bool operator !(MatrixShort matrix)
        {
            return matrix.n != 0 && matrix.size != 0;
        }

        public static MatrixShort operator ~(MatrixShort matrix)
        {
            MatrixShort result = new MatrixShort(matrix.n, matrix.size);
            for (int i = 0; i < matrix.n; i++)
            {
                for (int j = 0; j < matrix.size; j++)
                {
                    result.shortArray[i][j] = (short)~matrix.shortArray[i][j];
                }
            }
            return result;
        }

        public static MatrixShort operator -(MatrixShort matrix)
        {
            MatrixShort result = new MatrixShort(matrix.n, matrix.size);
            for (int i = 0; i < matrix.n; i++)
            {
                for (int j = 0; j < matrix.size; j++)
                {
                    result.shortArray[i][j] = (short)-matrix.shortArray[i][j];
                }
            }
            return result;
        }

        public static MatrixShort operator +(MatrixShort left, MatrixShort right)
        {
            if (!left.SameSize(right))
            {
                return left.CopyWithState(ErrorCode.SizeMismatch);
            }
            MatrixShort result = new MatrixShort(left.n, left.size);
            for (int i = 0; i < left.n; i++)
            {
                for (int j = 0; j < left.size; j++)
                {
                    result.shortArray[i][j] = (short)(left.shortArray[i][j] + right.shortArray[i][j]);
                }
            }
            return result;
        }

        public static MatrixShort operator +(MatrixShort matrix, short number)
        {
            MatrixShort result = new MatrixShort(matrix.n, matrix.size);
            for (int i = 0; i < matrix.n; i++)
            {
                for (int j = 0; j < matrix.size; j++)
                {
                    result.shortArray[i][j] = (short)(matrix.shortArray[i][j] + number);
                }
            }
            return result;
        }

        public static MatrixShort operator -(MatrixShort left, MatrixShort right)
        {
            if (!left.SameSize(right))
            {
                return left.CopyWithState(ErrorCode.SizeMismatch);
            }
            MatrixShort result = new MatrixShort(left.n, left.size);
            for (int i = 0; i < left.n; i++)
            {
                for (int j = 0; j < left.size; j++)
                {
                    result.shortArray[i][j] = (short)(left.shortArray[i][j] - right.shortArray[i][j]);
                }
            }
            return result;
        }

        public static MatrixShort operator -(MatrixShort matrix, short number)
        {
            MatrixShort result = new MatrixShort(matrix.n, matrix.size);
            for (int i = 0; i < matrix.n; i++)
            {
                for (int j = 0; j < matrix.size; j++)
                {
                    result.shortArray[i][j] = (short)(matrix.shortArray[i][j] - number);
                }
            }
            return result;
        }

        public static MatrixShort operator *(MatrixShort left, MatrixShort right)
        {
            if (left.size != right.n)
            {
                return left.CopyWithState(ErrorCode.SizeMismatch);
            }
            MatrixShort result = new MatrixShort(left.n, right.size);
            for (int i = 0; i < left.n; i++)
            {
                for (int j = 0; j < right.size; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < left.size; k++)
                    {
                        sum += left.shortArray[i][k] * right.shortArray[k][j];
                    }
                    result.shortArray[i][j] = (short)sum;
                }
            }
            return result;
        }

        public static VectorShort operator *(MatrixShort matrix, VectorShort vector)
        {
            VectorShort result = new VectorShort(matrix.n);
            if (matrix.size != vector.Length)
            {
                result.State = ErrorCode.SizeMismatch;
                return result;
            }
            for (int i = 0; i < matrix.n; i++)
            {
                int sum = 0;
                for (int j = 0; j < matrix.size; j++)
                {
                    sum += matrix.shortArray[i][j] * vector[j];
                }
                result[i] = (short)sum;
            }
            return result;
        }

        public static MatrixShort operator *(MatrixShort matrix, short number)
        {
            MatrixShort result = new MatrixShort(matrix.n, matrix.size);
            for (int i = 0; i < matrix.n; i++)
            {
                for (int j = 0; j < matrix.size; j++)
                {
                    result.shortArray[i][j] = (short)(matrix.shortArray[i][j] * number);
                }
            }
            return result;
        }

        public static MatrixShort operator *(short number, MatrixShort matrix)
        {
            return matrix * number;
        }

        public static MatrixShort operator /(MatrixShort matrix, short number)
        {
            if (number == 0)
            {
                return matrix.CopyWithState(ErrorCode.DivideByZero);
            }
            MatrixShort result = new MatrixShort(matrix.n, matrix.size);
            for (int i = 0; i < matrix.n; i++)
            {
                for (int j = 0; j < matrix.size; j++)
                {
                    result.shortArray[i][j] = (short)(matrix.shortArray[i][j] / number);
                }
            }
            return result;
        }

        public static MatrixShort operator %(MatrixShort matrix, short number)
        {
            if (number == 0)
            {
                return matrix.CopyWithState(ErrorCode.DivideByZero);
            }
            MatrixShort result = new MatrixShort(matrix.n, matrix.size);
            for (int i = 0; i < matrix.n; i++)
            {
                for (int j = 0; j < matrix.size; j++)
                {
                    result.shortArray[i][j] = (short)(matrix.shortArray[i][j] % number);
                }
            }
            return result;
        }

        public static MatrixShort operator |(MatrixShort left, MatrixShort right)
        {
            if (!left.SameSize(right))
            {
                return left.CopyWithState(ErrorCode.SizeMismatch);
            }
            MatrixShort result = new MatrixShort(left.n, left.size);
            for (int i = 0; i < left.n; i++)
            {
                for (int j = 0; j < left.size; j++)
                {
                    result.shortArray[i][j] = (short)(left.shortArray[i][j] | right.shortArray[i][j]);
                }
            }
            return result;
        }

        public static MatrixShort operator |(MatrixShort matrix, short number)
        {
            MatrixShort result = new MatrixShort(matrix.n, matrix.size);
            for (int i = 0; i < matrix.n; i++)
            {
                for (int j = 0; j < matrix.size; j++)
                {
                    result.shortArray[i][j] = (short)(matrix.shortArray[i][j] | number);
                }
            }
            return result;
        }

        public static MatrixShort operator ^(MatrixShort left, MatrixShort right)
        {
            if (!left.SameSize(right))
            {
                return left.CopyWithState(ErrorCode.SizeMismatch);
            }
            MatrixShort result = new MatrixShort(left.n, left.size);
            for (int i = 0; i < left.n; i++)
            {
                for (int j = 0; j < left.size; j++)
                {
                    result.shortArray[i][j] = (short)(left.shortArray[i][j] ^ right.shortArray[i][j]);
                }
            }
            return result;
        }

        public static MatrixShort operator ^(MatrixShort matrix, short number)
        {
            MatrixShort result = new MatrixShort(matrix.n, matrix.size);
            for (int i = 0; i < matrix.n; i++)
            {
                for (int j = 0; j < matrix.size; j++)
                {
                    result.shortArray[i][j] = (short)(matrix.shortArray[i][j] ^ number);
                }
            }
            return result;
        }

        public static MatrixShort operator &(MatrixShort left, MatrixShort right)
        {
            if (!left.SameSize(right))
            {
                return left.CopyWithState(ErrorCode.SizeMismatch);
            }
            MatrixShort result = new MatrixShort(left.n, left.size);
            for (int i = 0; i < left.n; i++)
            {
                for (int j = 0; j < left.size; j++)
                {
                    result.shortArray[i][j] = (short)(left.shortArray[i][j] & right.shortArray[i][j]);
                }
            }
            return result;
        }

        public static MatrixShort operator &(MatrixShort matrix, short number)
        {
            MatrixShort result = new MatrixShort(matrix.n, matrix.size);
            for (int i = 0; i < matrix.n; i++)
            {
                for (int j = 0; j < matrix.size; j++)
                {
                    result.shortArray[i][j] = (short)(matrix.shortArray[i][j] & number);
                }
            }
            return result;
        }

        public static MatrixShort operator <<(MatrixShort matrix, int shift)
        {
            MatrixShort result = new MatrixShort(matrix.n, matrix.size);
            for (int i = 0; i < matrix.n; i++)
            {
                for (int j = 0; j < matrix.size; j++)
                {
                    result.shortArray[i][j] = (short)(matrix.shortArray[i][j] << shift);
                }
            }
            return result;
        }

        public static MatrixShort operator >>(MatrixShort matrix, int shift)
        {
            MatrixShort result = new MatrixShort(matrix.n, matrix.size);
            for (int i = 0; i < matrix.n; i++)
            {
                for (int j = 0; j < matrix.size; j++)
                {
                    result.shortArray[i][j] = (short)(matrix.shortArray[i][j] >> shift);
                }
            }
            return result;
        }

        public static bool operator ==(MatrixShort left, MatrixShort right)
        {
            if ((object)left == (object)right)
            {
                return true;
            }
            if ((object)left == null || (object)right == null)
            {
                return false;
            }
            if (!left.SameSize(right))
            {
                return false;
            }
            for (int i = 0; i < left.n; i++)
            {
                for (int j = 0; j < left.size; j++)
                {
                    if (left.shortArray[i][j] != right.shortArray[i][j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool operator !=(MatrixShort left, MatrixShort right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            MatrixShort other = obj as MatrixShort;
            if (other == null)
            {
                return false;
            }
            return this == other;
        }

        public override int GetHashCode()
        {
            int hash = n ^ (size << 8);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    hash = hash * 31 + shortArray[i][j];
                }
            }
            return hash;
        }

        public static bool operator >(MatrixShort left, MatrixShort right)
        {
            if (!left.SameSize(right))
            {
                return false;
            }
            for (int i = 0; i < left.n; i++)
            {
                for (int j = 0; j < left.size; j++)
                {
                    if (!(left.shortArray[i][j] > right.shortArray[i][j]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool operator <(MatrixShort left, MatrixShort right)
        {
            if (!left.SameSize(right))
            {
                return false;
            }
            for (int i = 0; i < left.n; i++)
            {
                for (int j = 0; j < left.size; j++)
                {
                    if (!(left.shortArray[i][j] < right.shortArray[i][j]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool operator >=(MatrixShort left, MatrixShort right)
        {
            if (!left.SameSize(right))
            {
                return false;
            }
            for (int i = 0; i < left.n; i++)
            {
                for (int j = 0; j < left.size; j++)
                {
                    if (!(left.shortArray[i][j] >= right.shortArray[i][j]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool operator <=(MatrixShort left, MatrixShort right)
        {
            if (!left.SameSize(right))
            {
                return false;
            }
            for (int i = 0; i < left.n; i++)
            {
                for (int j = 0; j < left.size; j++)
                {
                    if (!(left.shortArray[i][j] <= right.shortArray[i][j]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override string ToString()
        {
            if (shortArray == null)
            {
                return "(empty)";
            }
            string text = "";
            for (int i = 0; i < n; i++)
            {
                if (i > 0)
                {
                    text += Environment.NewLine;
                }
                text += shortArray[i].ToString();
            }
            return text;
        }

        public void Print()
        {
            Console.WriteLine(ToString());
        }

        public void Read()
        {
            string text = "";
            for (int i = 0; i < n; i++)
            {
                text += Console.ReadLine() + ";";
            }
            ReadFromText(text);
        }

        // Рядки матриці розділяються символом ';', числа в рядку - пробілами
        public void ReadFromText(string text)
        {
            if (text == null)
            {
                State = ErrorCode.NullArray;
                return;
            }

            string[] lines = text.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length != n)
            {
                State = ErrorCode.InvalidInput;
                return;
            }

            short[,] parsed = new short[n, size];
            for (int i = 0; i < n; i++)
            {
                string[] parts = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != size)
                {
                    State = ErrorCode.InvalidInput;
                    return;
                }
                for (int j = 0; j < size; j++)
                {
                    if (!short.TryParse(parts[j], out parsed[i, j]))
                    {
                        State = ErrorCode.InvalidInput;
                        return;
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    shortArray[i][j] = parsed[i, j];
                }
            }
        }

    }

    class Program {

        public static void Main(string[] args) {

            Demo();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine($"Matrices left: {MatrixShort.Count}, vectors left: {VectorShort.Count}");

        }

        private static MatrixShort Make(int rows, int columns, string text)
        {
            MatrixShort matrix = new MatrixShort(rows, columns);
            matrix.ReadFromText(text);
            return matrix;
        }

        private static void Show(string title, MatrixShort matrix)
        {
            Console.WriteLine($"{title} (n = {matrix.N}, size = {matrix.Size}, State = {matrix.State}):");
            matrix.Print();
        }

        private static void Demo()
        {
            MatrixShort empty = new MatrixShort();
            MatrixShort identity = new MatrixShort(3);
            MatrixShort zeros = new MatrixShort(2, 3);
            MatrixShort filled = new MatrixShort(2, 3, 7);
            MatrixShort broken = new MatrixShort(-1, 2);

            Show("empty", empty);
            Show("identity", identity);
            Show("zeros", zeros);
            Show("filled", filled);
            Console.WriteLine($"broken: State = {broken.State}");
            Console.WriteLine($"Matrices alive: {MatrixShort.Count}");
            Console.WriteLine();

            MatrixShort first = Make(2, 3, "1 2 3; 4 5 6");
            MatrixShort second = Make(2, 3, "6 5 4; 3 2 1");
            MatrixShort third = Make(3, 2, "1 2; 3 4; 5 6");
            Show("first", first);
            Show("second", second);
            Show("third", third);

            MatrixShort invalid = new MatrixShort(2, 2);
            invalid.ReadFromText("1 2; abc");
            Console.WriteLine($"invalid input: State = {invalid.State}");
            Console.WriteLine();

            MatrixShort counter = new MatrixShort(first);
            MatrixShort previous = counter++;
            Show("counter++ : previous", previous);
            Show("counter++ : counter", counter);
            Show("--counter", --counter);
            Console.WriteLine($"!first = {!first}, !empty = {!empty}");
            Show("~first", ~first);
            Show("-first", -first);
            Console.WriteLine();

            Show("first + second", first + second);
            Show("first + 10", first + 10);
            Show("first - second", first - second);
            Show("first - 1", first - 1);
            Show("first * third", first * third);
            Show("first * 2", first * 2);
            Show("2 * first", (short)2 * first);
            Show("first / 2", first / 2);
            Show("first % 4", first % 4);

            VectorShort vector = new VectorShort(3);
            vector[0] = 1;
            vector[1] = 0;
            vector[2] = 2;
            VectorShort product = first * vector;
            Console.WriteLine($"first * {vector} = {product}");
            VectorShort wrongProduct = first * new VectorShort(2);
            Console.WriteLine($"first * vector of 2 : State = {wrongProduct.State}");
            Console.WriteLine($"first + third : State = {(first + third).State}");
            Console.WriteLine($"first / 0 : State = {(first / 0).State}");
            Console.WriteLine($"first % 0 : State = {(first % 0).State}");
            Console.WriteLine();

            Show("first | second", first | second);
            Show("first | 8", first | 8);
            Show("first ^ second", first ^ second);
            Show("first ^ 1", first ^ 1);
            Show("first & second", first & second);
            Show("first & 3", first & 3);
            Show("first << 2", first << 2);
            Show("first >> 1", first >> 1);
            Console.WriteLine();

            MatrixShort accumulator = new MatrixShort(first);
            accumulator += second;
            Show("accumulator += second", accumulator);
            accumulator -= second;
            Show("accumulator -= second", accumulator);
            accumulator += 5;
            Show("accumulator += 5", accumulator);
            accumulator -= 5;
            Show("accumulator -= 5", accumulator);
            accumulator *= 3;
            Show("accumulator *= 3", accumulator);
            accumulator /= 3;
            Show("accumulator /= 3", accumulator);
            accumulator %= 4;
            Show("accumulator %= 4", accumulator);
            accumulator |= second;
            Show("accumulator |= second", accumulator);
            accumulator ^= second;
            Show("accumulator ^= second", accumulator);
            accumulator &= second;
            Show("accumulator &= second", accumulator);
            accumulator <<= 2;
            Show("accumulator <<= 2", accumulator);
            accumulator >>= 1;
            Show("accumulator >>= 1", accumulator);
            accumulator = new MatrixShort(first);
            accumulator *= third;
            Show("accumulator = first, accumulator *= third", accumulator);
            Console.WriteLine();

            MatrixShort copy = new MatrixShort();
            copy.Assign(first);
            Show("copy.Assign(first)", copy);
            Console.WriteLine($"copy == first : {copy == first}, first == second : {first == second}");
            Console.WriteLine($"first != second : {first != second}");
            Console.WriteLine($"first < second : {first < second}, first > second : {first > second}");
            Console.WriteLine($"first <= copy : {first <= copy}, first >= copy : {first >= copy}");
            Console.WriteLine();

            first[0][1] = 20;
            Console.WriteLine($"first[0][1] = {first[0][1]}, first[1] = {first[1]}");
            VectorShort lastRow = first[10];
            Console.WriteLine($"first[10] = {lastRow}, State = {first.State}");
            Console.WriteLine($"first(1, 2) = {first.Invoke(1, 2)}");
            Console.WriteLine($"first(5, 5) = {first.Invoke(5, 5)}, State = {first.State}");
            Console.WriteLine();
        }

    }

}
