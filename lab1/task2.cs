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

            Console.WriteLine("Введіть введіть номер елемента Т:");
            int T = Convert.ToInt32(Console.ReadLine()!);

            int minusElId = 0;
            int minusEl = A[T-1];

            for (int i = T - 1; i < n; i++) {

                if (minusEl < 0 && minusEl > A[i]) {

                    minusEl = A[i];
                    minusElId = i+1;

                }
            
            }

            Console.WriteLine($"Номер останнього максимального значення серед від'ємних елементів:{minusEl}");

        }

    }

}
