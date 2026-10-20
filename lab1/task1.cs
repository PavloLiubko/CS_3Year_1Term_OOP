namespace laba1 {

    class Program {

        public static void Main(string[] args) {

            Console.WriteLine("Введіть кількість елементів масиву:");
            int n = Convert.ToInt32(Console.ReadLine()!);

            int[] A = new int[n];

            for (int i = 0; i < n; i++) {

                Console.WriteLine("Введіть значення елементу масиву:");
                A[i]= Convert.ToInt32(Console.ReadLine()!);

            }

            int nullCounter = 0;

            for (int i = 0; i < n; i++)
            {

                if (A[i] == 0) { nullCounter++; }

            }

            Console.WriteLine($"Кількість нульових елементів: {nullCounter}");

        }

    }

}
