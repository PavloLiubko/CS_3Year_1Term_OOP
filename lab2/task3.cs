class lab2 {

    public static void Main(string[] args) {

        string text = "Blablabla";

        Console.WriteLine("Введіть кількість зсувів, яку бажаєте оформити:");
        int n = Convert.ToInt32(Console.ReadLine()!);

        string encrypted = " ";

        for (int i = 0; i < text.Length; i++) {

            string cryptSymbol = Convert.ToString( (int)text[i] << 1 );

            encrypted += cryptSymbol;
            encrypted += " ";


        }

        Console.WriteLine($"Encrypted text: {encrypted}");

    }

}
