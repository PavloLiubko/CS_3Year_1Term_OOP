class lab2 {

    public static void Main(String[] args)
    {

        string text = "Kyiv is capital of Ukraine.";

        string strighted = text.PadRight(128, ' ');

        string encrypted = " ";

        for (int i = 0; i < 128; i++) {

            Int16 cryptSymbol = 0;

            if (cryptSymbol % 2 > 0) {

                Int16 mask = 1;

                cryptSymbol = (Int16)(cryptSymbol | mask);
            
            }

            int ascii_num = (int)strighted[i];

            int changed_ascii = (ascii_num << 1);

            cryptSymbol = (Int16)(cryptSymbol | changed_ascii);

            int position_bits = (i << 9);

            cryptSymbol = (Int16)(cryptSymbol | position_bits);

            encrypted += Convert.ToString(cryptSymbol);
            encrypted += " ";
        }

        Console.WriteLine(encrypted);

    }
}
