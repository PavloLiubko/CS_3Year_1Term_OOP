namespace Lab5
{

    class Card
    {
        public int number { get; set; }

        public string color { get; set; }

        public Card(int Number, string Color)
        {

            this.number = Number;
            this.color = Color;

        }

    }

    class Deck
    {

        public int cardsAmount { get; set; }

        public List<Card> fullDeck = new List<Card>();

        public Deck()
        { // chaotic

            Random randEntity = new Random();

            int cardsAmount = randEntity.Next(1, 20);

            int[] randNumbers = new int[cardsAmount];
            for (int i = 0; i < cardsAmount; i++)
            {

                randNumbers[i] = randEntity.Next(1, 100);

            }

            for (int i = 0; i < cardsAmount; i++)
            {
                int randColorCode = randEntity.Next(1, 3);

                if (randColorCode == 1)
                {

                    this.fullDeck.Add(new Card(randNumbers[i], "black"));

                }
                else
                {

                    this.fullDeck.Add(new Card(randNumbers[i], "red"));


                }

            }


        }

        public Deck(int CardsAmount) // organized 
        {

            Random randEntity = new Random();

            for (int i = 0; i < CardsAmount; i++)
            {
                int randNumber = randEntity.Next(1, 3);

                if (randNumber == 1)
                {
                    this.fullDeck.Add(new Card(i, "black"));
                }

                else
                {

                    this.fullDeck.Add(new Card(i, "red"));


                }
            }


        }


    }

    class Solitaire
    {

        public List<Card> SolitaireList = new List<Card>();

        public List<Card> getSolitaire(Deck SomeDeck)
        {
            List<Card> Copy = new List<Card>(SomeDeck.fullDeck);
            SomeDeck.fullDeck.AddRange(Copy);
            
            if (SomeDeck.fullDeck.Count <= 3) {

                SolitaireList = SomeDeck.fullDeck;
                return SolitaireList;
            
            }

            for (int i = 0; i < SomeDeck.fullDeck.Count; i += 3)
            {
                if (i + 3 > SomeDeck.fullDeck.Count) { break; }


                if (SomeDeck.fullDeck[i].color == SomeDeck.fullDeck[i + 2].color)
                {

                    SolitaireList.Add(SomeDeck.fullDeck[i + 1]);

                }

                else
                {

                    List<Card> trio = new List<Card>();
                    SolitaireList.Add(SomeDeck.fullDeck[i]);
                    SolitaireList.Add(SomeDeck.fullDeck[i + 1]);
                    SolitaireList.Add(SomeDeck.fullDeck[i + 2]);

                }

            }

            return SolitaireList;
        }

    }
    class Program
    {
        public static void Main(string[] args) { 
        
            Deck testDeck1 = new Deck();

            Console.WriteLine("Перша колода, створена хаотично:");
            for (int i = 0; i < testDeck1.fullDeck.Count; i++)
            {

                Console.WriteLine(testDeck1.fullDeck[i].number);

            }

            Solitaire testSolitaire1 = new Solitaire();
            testSolitaire1.getSolitaire(testDeck1);

            Console.WriteLine("Перший пасьянс:");
            for (int i = 0; i < testSolitaire1.SolitaireList.Count; i++) {

                Console.WriteLine(testSolitaire1.SolitaireList[i].number);
            
            }

            Deck testDeck2 = new Deck(10);

            Console.WriteLine("Друга колода, створена впорядковано:");
            for (int i = 0; i < testDeck2.fullDeck.Count; i++)
            {

                Console.WriteLine(testDeck2.fullDeck[i].number);

            }

            Solitaire testSolitaire2 = new Solitaire();
            testSolitaire2.getSolitaire(testDeck2);

            Console.WriteLine("Другий пасьянс:");
            for (int i = 0; i < testSolitaire2.SolitaireList.Count; i++)
            {

                Console.WriteLine(testSolitaire2.SolitaireList[i].number);

            }

        }
    }
}
