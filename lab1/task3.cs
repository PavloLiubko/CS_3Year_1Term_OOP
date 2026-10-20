namespace laba1 {

    class Program {

        public static int Main(string[] args) {

            Console.WriteLine("Введіть кількість елементів масиву:");
            int n = Convert.ToInt32(Console.ReadLine()!);

            if (n <= 0 || n > 200) {

                Console.WriteLine("Значення порушує умови задачі!");
                return 0;

            }

            double[] A = new double[n];

            for (int i = 0; i < n; i++) {

                Console.WriteLine("Введіть значення елементу масиву:");
                A[i]= Convert.ToDouble(Console.ReadLine()!);

            }

            List<double> B = new List<double>();

            for (int i = 1; i < n; i++) {

                if (i != n - 1)
                {

                    B.Add((A[i - 1] + A[i + 1]) / 2);

                }
                else { break; }

            }

            Console.WriteLine("Утворений за умовами масив:");

            for (int i = 0; i < B.Count; i++)
            {

                Console.WriteLine($"{B[i]} \t");

            }

            return 0;

        }

    }

}
