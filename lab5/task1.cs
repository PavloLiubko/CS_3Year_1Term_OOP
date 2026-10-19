namespace Lab5 {

    class Vehicle {

        public string name;
        private float speed;
        private float weight;
        private float cost;

        public Vehicle (string name, float speed, float weight, float cost)
        {
            this.name = name;
            this.speed = speed;
            this.weight = weight;
            this.cost = cost;
        }

        public virtual void Show() { }

        ~Vehicle()  {

            Console.WriteLine("Vehicle was deleted");
        
        }

    }

    class Car : Vehicle
        {

        private string carMake;
        private int wheelsAmount;

        private string driveSystem;


        public Car(string name, float speed, float weight, float cost, string CarMake, int wheelsAmount, string driveSystem) : base(name, speed, weight, cost) { 
        
            this.carMake = CarMake;
            this.wheelsAmount = wheelsAmount;
            this.driveSystem = driveSystem;

        }

        public override void Show() {
            
            Console.WriteLine($"This {this.name} is driving!");
        
        }

        ~Car()
        {

            Console.WriteLine("Car was deleted");

        }

    }

    class Prom : Vehicle {

        private float capacity;

        public Prom(string name, float speed, float weight, float cost, float capacity) : base( name,  speed,  weight,  cost)
        {
            this.capacity = capacity;
        }

        public override void Show() {


            Console.WriteLine($"This {this.name} is swimming");
        
        }

        ~Prom()
        {

            Console.WriteLine("Prom was deleted");

        }

    }

    class Plane : Vehicle
    {

        private float maxHeight;
        private int turbinesAmount;

        public Plane(string name, float speed, float weight, float cost, int turbinesAmount, float maxHeight) : base(name, speed, weight, cost)
        {
            this.maxHeight = maxHeight;
            this.turbinesAmount = turbinesAmount;
        }

        public override void Show()
        {

            Console.WriteLine($"This {this.name} is swimming");

        }

        ~Plane()
        {

            Console.WriteLine("Plane was deleted");

        }
    }

    class Van : Car { 
    
        private float capacity;
        private bool hasTrailer;

        public Van(string name, float speed, float weight, float cost, float capacity, bool hasTrailer, string carMake, int wheelsAmount, string driveSystem) : base (name, speed, weight, cost, carMake, wheelsAmount, driveSystem)
        {
            this.capacity = capacity;
            this.hasTrailer = hasTrailer;
        }

        public override void Show()
        {
            Console.WriteLine($"This {this.name} is flying");
        }

        ~Van()
        {

            Console.WriteLine("Van was deleted");

        }
    }

    class Program {

        public static void Main(string[] args) {

            Car maibah = new Car("Maibah", 250, 3000, 5000000, "Mercedes", 4, "Rear");
            Van Fura = new Van("NewFura", 250, 3000, 5000000, 500, true, "Man", 6, "Rear");

            Prom goodProm = new Prom("Promis", 50, 40000, 5000000, 30000);

            Plane Dodge = new Plane("Plane dodge", 500, 550000, 45000000, 6, 1500);


            maibah.Show();

            Fura.Show();

            goodProm.Show();

            Dodge.Show();

        }

    }


}
