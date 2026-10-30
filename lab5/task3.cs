class SafeVector
{
    public int upperBorder;
    public int lowerBorder;

    private int _x;
    private int _y;
    private int _z;

    public int x
    {
        get => _x;
        set
        {
            if (value > lowerBorder && value < upperBorder)
                _x = value;
        }
    }

    public int y
    {
        get => _y;
        set
        {
            if (value > lowerBorder && value < upperBorder)
                _y = value;
        }
    }

    public int z
    {
        get => _z;
        set
        {
            if (value > lowerBorder && value < upperBorder)
                _z = value;
        }
    }

    public bool isSafe;

    public SafeVector(int UpperBorder, int LowerBorder, int X, int Y, int Z)
    {
        upperBorder = UpperBorder;
        lowerBorder = LowerBorder;

        x = X;
        y = Y;
        z = Z;

        isSafe = x < upperBorder && x > lowerBorder
              && y < upperBorder && y > lowerBorder
              && z < upperBorder && z > lowerBorder;
    }

    public SafeVector(int x, int y, int z)
    {
        upperBorder = 10000;
        lowerBorder = -10000;

        this.x = x;
        this.y = y;
        this.z = z;

        isSafe = this.x == x && this.y == y && this.z == z;
    }

    public SafeVector(SafeVector someSafeVector)
    {
        upperBorder = someSafeVector.upperBorder;
        lowerBorder = someSafeVector.lowerBorder;

        x = someSafeVector.x;
        y = someSafeVector.y;
        z = someSafeVector.z;

        isSafe = someSafeVector.isSafe;
    }

    public virtual void setUpperBorder(int num)
    {
        upperBorder = num;
    }

    public virtual void setLowerBorder(int num)
    {
        lowerBorder = num;
    }
}

class Vector : SafeVector
{
    public Vector(int x, int y, int z) : base(x, y, z)
    {
    }

    public override string ToString()
    {
        return $"x: {x} \ny: {y} \nz: {z}";
    }

    public Vector(Vector someVector) : base(someVector)
    {
    }

    public override void setUpperBorder(int num)
    {
        base.setUpperBorder(num);
    }

    public override void setLowerBorder(int num)
    {
        base.setLowerBorder(num);
    }
}

class Program
{
    public static void Main(string[] args)
    {
        SafeVector testSafeVector = new SafeVector(100, -100, 1, 2, 3);

        Vector testVector = new Vector(1, 2, 3);
    }
}

