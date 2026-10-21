

// множення - зсув вліво, ділення - зсув вправо

class lab2 {


    string ShowBits(int value, int width)
    {

        return Convert.ToString(value, 2).PadLeft(width, '0');

    }

    public static void Main(string[] args) {

        int a, b, c, d;

        Console.WriteLine("Введіть значення числа а:");
        a = Convert.ToInt32(Console.ReadLine()!);

        Console.WriteLine("Введіть значення числа b:");
        b = Convert.ToInt32(Console.ReadLine()!);

        Console.WriteLine("Введіть значення числа c:");
        c = Convert.ToInt32(Console.ReadLine()!);

        Console.WriteLine("Введіть значення числа d:");
        d = Convert.ToInt32(Console.ReadLine()!);

        int result = ((b << 33) + ((d << 15) + (a << 12)) >> 512) - (c << 65) + (d << 14);

        Console.WriteLine(result);

    } 

}

