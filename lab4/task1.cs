using System;

public enum VectorState
{
    Ok = 0,
    NullArray = 1,
    ArrayTooShort = 2,
    DivideByZero = 3,
    IndexOutOfRange = 4,
    InvalidInput = 5
}

public sealed class Vector3F : IDisposable
{
    private static int objectCount;

    public static int Count
    {
        get { return objectCount; }
    }

    private float x;
    private float y;
    private float z;
    private bool isReleased;

    public VectorState State { get; private set; }

    public float X
    {
        get { return x; }
        set { x = value; }
    }

    public float Y
    {
        get { return y; }
        set { y = value; }
    }

    public float Z
    {
        get { return z; }
        set { z = value; }
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
            State = VectorState.NullArray;
        }
        else if (source.Length < 3)
        {
            State = VectorState.ArrayTooShort;
        }
        else
        {
            x = source[0];
            y = source[1];
            z = source[2];
        }
        objectCount++;
    }

    private void Release()
    {
        if (isReleased)
        {
            return;
        }
        isReleased = true;
        objectCount--;
        Console.WriteLine("[~Vector3F] " + ToString() + ", State = " + State);
    }

    public void Dispose()
    {
        Release();
        GC.SuppressFinalize(this);
    }

    ~Vector3F()
    {
        Release();
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
        x = other.x;
        y = other.y;
        z = other.z;
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
        return "(" + x + ", " + y + ", " + z + ")";
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
            x = newX;
            y = newY;
            z = newZ;
        }
        else
        {
            State = VectorState.InvalidInput;
        }
    }
}

internal static class Program
{
    private static void Main()
    {
        int failedCount = Tests.Run();

        GC.Collect();
        GC.WaitForPendingFinalizers();
        Console.WriteLine();
        Console.WriteLine("Живих об'єктів після GC: " + Vector3F.Count);
        if (failedCount == 0)
        {
            Console.WriteLine("Усі тести пройдено.");
        }
        else
        {
            Console.WriteLine("Провалено тестів: " + failedCount);
        }
    }
}

internal static class Tests
{
    private static int failedCount;

    private static void Check(string testName, bool passed)
    {
        string verdict = "FAIL";
        if (passed)
        {
            verdict = "PASS";
        }
        else
        {
            failedCount++;
        }
        Console.WriteLine(verdict + ": " + testName);
    }

    public static int Run()
    {
        int countBefore = Vector3F.Count;
        using (Vector3F temporary = new Vector3F())
        {
            Check("Count збільшується після створення", Vector3F.Count == countBefore + 1);
        }
        Check("Count зменшується після Dispose", Vector3F.Count == countBefore);

        Vector3F first = new Vector3F(1, 2, 3);
        Vector3F second = new Vector3F(4, 5, 6);
        Vector3F zero = new Vector3F();

        Check("Конструктор без параметрів", zero == new Vector3F(0, 0, 0));
        Check("Конструктор з одним числом", new Vector3F(7f) == new Vector3F(7, 7, 7));
        Check("Конструктор з масиву", new Vector3F(new float[] { 7f, 8f, 9f }) == new Vector3F(7, 8, 9));

        float[] missingArray = null;
        Vector3F fromMissingArray = new Vector3F(missingArray);
        Check("Конструктор з null → NullArray", fromMissingArray.State == VectorState.NullArray);

        Vector3F fromShortArray = new Vector3F(new float[] { 1f });
        Check("Конструктор з коротким масивом → ArrayTooShort", fromShortArray.State == VectorState.ArrayTooShort);

        Vector3F counter = new Vector3F(1, 1, 1);
        Vector3F previous = counter++;
        Check("Постфіксний ++ повертає старе значення", previous == new Vector3F(1, 1, 1));
        Check("Постфіксний ++ змінює змінну", counter == new Vector3F(2, 2, 2));
        Check("Префіксний --", --counter == new Vector3F(1, 1, 1));

        bool firstIsNonZero = !first;
        bool zeroIsNonZero = !zero;
        Check("! для ненульового вектора", firstIsNonZero == true);
        Check("! для нульового вектора", zeroIsNonZero == false);
        Check("Унарний мінус", -first == new Vector3F(-1, -2, -3));

        Vector3F perpendicular = ~first;
        float dotProduct = first[0] * perpendicular[0] + first[1] * perpendicular[1] + first[2] * perpendicular[2];
        Check("~ дає перпендикуляр", dotProduct == 0f);

        Vector3F alongZ = new Vector3F(0, 0, 5);
        Vector3F perpendicularToZ = ~alongZ;
        float dotProductAlongZ = alongZ[0] * perpendicularToZ[0] + alongZ[1] * perpendicularToZ[1] + alongZ[2] * perpendicularToZ[2];
        Check("~ для вектора вздовж Z", dotProductAlongZ == 0f && perpendicularToZ != zero);

        Check("+", first + second == new Vector3F(5, 7, 9));
        Check("-", first - second == new Vector3F(-3, -3, -3));
        Check("* (вектор * число)", first * 2f == new Vector3F(2, 4, 6));
        Check("* (число * вектор)", 2f * first == new Vector3F(2, 4, 6));
        Check("/", second / 2f == new Vector3F(2, 2.5f, 3));
        Check("% для цілої частини", first % 2 == new Vector3F(1, 0, 1));
        Check("% (4,5,6) % 4", second % 4 == new Vector3F(0, 1, 2));
        Check("& (векторний добуток)", (first & second) == new Vector3F(-3, 6, -3));

        Vector3F accumulator = new Vector3F(1, 2, 3);
        accumulator += second;
        Check("+=", accumulator == new Vector3F(5, 7, 9));
        accumulator -= second;
        Check("-=", accumulator == first);
        accumulator *= 3f;
        Check("*=", accumulator == new Vector3F(3, 6, 9));
        accumulator /= 3f;
        Check("/=", accumulator == first);
        accumulator &= second;
        Check("&=", accumulator == new Vector3F(-3, 6, -3));
        accumulator = new Vector3F(7, 8, 9);
        accumulator %= 4;
        Check("%=", accumulator == new Vector3F(3, 0, 1));

        Vector3F divisionResult = first / 0f;
        Check("Ділення на 0 → DivideByZero", divisionResult.State == VectorState.DivideByZero);
        Vector3F remainderResult = first % 0;
        Check("% 0 → DivideByZero", remainderResult.State == VectorState.DivideByZero);

        Vector3F copy = new Vector3F();
        copy.Assign(first);
        Check("Assign копіює значення", copy == first);
        Check("Assign не робить аліасу", (object)copy != (object)first);

        Vector3F indexed = new Vector3F(1, 2, 3);
        Check("Індексатор [1]", indexed[1] == 2f && indexed.State == VectorState.Ok);
        indexed[0] = 10f;
        Check("Запис через індексатор", indexed.X == 10f);
        float invalidRead = indexed[7];
        Check("Неправильний індекс → z + IndexOutOfRange",
            invalidRead == 3f && indexed.State == VectorState.IndexOutOfRange);
        indexed[10] = 42f;
        Check("Запис за неправильним індексом змінює z", indexed.Z == 42f);

        Check("==", first == new Vector3F(1, 2, 3));
        Check("!=", first != second);
        Check("<", first < second);
        Check(">", second > first);
        Check("<= для рівних", first <= new Vector3F(1, 2, 3));
        Check(">= для рівних", first >= new Vector3F(1, 2, 3));

        Vector3F incomparable = new Vector3F(1, 5, 1);
        Vector3F reference = new Vector3F(2, 2, 2);
        Check("Непорівнянні вектори: ні < ні >", !(incomparable < reference) && !(incomparable > reference));

        Check("Invoke() — довжина вектора", new Vector3F(3, 4, 0).Invoke() == 5f);
        Check("ToString", first.ToString() == "(1, 2, 3)");

        Vector3F parsed = new Vector3F();
        parsed.ReadFromText("1.5 2,5 -3");
        Check("ReadFromText з дробами і мінусом", parsed == new Vector3F(1.5f, 2.5f, -3f));

        Vector3F invalidInput = new Vector3F();
        invalidInput.ReadFromText("abc");
        Check("ReadFromText з помилкою → InvalidInput", invalidInput.State == VectorState.InvalidInput);

        return failedCount;
    }
}
