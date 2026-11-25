using System;

namespace PhoneDirectoryLab {

    enum ErrorCode
    {
        Ok = 0,
        NotFound = 1,
        InvalidPhone = 2,
        InvalidFullName = 3,
        DuplicatePhone = 4,
        InvalidInput = 5
    }

    class PhoneDirectory {

        private string[] phones;
        private string[] fullNames;
        private int count;

        public ErrorCode CodeError { get; private set; }

        public int Count
        {
            get { return count; }
        }

        public PhoneDirectory()
        {
            this.phones = new string[4];
            this.fullNames = new string[4];
            this.count = 0;
        }

        public static PhoneDirectory CreateSampleSet()
        {
            PhoneDirectory directory = new PhoneDirectory();
            directory.Add("+380501112233", "Іваненко Іван Іванович");
            directory.Add("+380672223344", "Петренко Марія Олексіївна");
            directory.Add("+380933334455", "Коваленко Андрій Миколайович");
            directory.Add("+380504445566", "Шевченко Олена Василівна");
            directory.Add("0975556677", "Бондаренко Сергій Петрович");
            return directory;
        }

        private static bool IsPhoneValid(string phoneNumber)
        {
            if (phoneNumber == null)
            {
                return false;
            }

            int startPosition = 0;
            if (phoneNumber.Length > 0 && phoneNumber[0] == '+')
            {
                startPosition = 1;
            }

            int digitCount = 0;
            for (int position = startPosition; position < phoneNumber.Length; position++)
            {
                char symbol = phoneNumber[position];
                if (symbol < '0' || symbol > '9')
                {
                    return false;
                }
                digitCount++;
            }
            return digitCount >= 10 && digitCount <= 12;
        }

        private static bool IsFullNameValid(string fullName)
        {
            if (fullName == null)
            {
                return false;
            }

            int wordCount = 0;
            bool insideWord = false;
            for (int position = 0; position < fullName.Length; position++)
            {
                if (fullName[position] == ' ')
                {
                    insideWord = false;
                }
                else if (!insideWord)
                {
                    insideWord = true;
                    wordCount++;
                }
            }
            return wordCount == 3;
        }

        private int FindPhone(string phoneNumber)
        {
            for (int position = 0; position < count; position++)
            {
                if (phones[position] == phoneNumber)
                {
                    return position;
                }
            }
            return -1;
        }

        private int FindFullName(string fullName)
        {
            for (int position = 0; position < count; position++)
            {
                if (fullNames[position] == fullName)
                {
                    return position;
                }
            }
            return -1;
        }

        private void EnsureCapacity()
        {
            if (count < phones.Length)
            {
                return;
            }

            string[] biggerPhones = new string[phones.Length * 2];
            string[] biggerFullNames = new string[fullNames.Length * 2];
            for (int position = 0; position < count; position++)
            {
                biggerPhones[position] = phones[position];
                biggerFullNames[position] = fullNames[position];
            }
            phones = biggerPhones;
            fullNames = biggerFullNames;
        }

        public bool Add(string phoneNumber, string fullName)
        {
            if (!IsPhoneValid(phoneNumber))
            {
                CodeError = ErrorCode.InvalidPhone;
                return false;
            }
            if (!IsFullNameValid(fullName))
            {
                CodeError = ErrorCode.InvalidFullName;
                return false;
            }
            if (FindPhone(phoneNumber) >= 0)
            {
                CodeError = ErrorCode.DuplicatePhone;
                return false;
            }

            EnsureCapacity();
            phones[count] = phoneNumber;
            fullNames[count] = fullName;
            count++;
            CodeError = ErrorCode.Ok;
            return true;
        }

        public string this[string phoneNumber]
        {
            get
            {
                int position = FindPhone(phoneNumber);
                if (position < 0)
                {
                    CodeError = ErrorCode.NotFound;
                    return "";
                }
                CodeError = ErrorCode.Ok;
                return fullNames[position];
            }
            set
            {
                int position = FindPhone(phoneNumber);
                if (position < 0)
                {
                    Add(phoneNumber, value);
                }
                else if (!IsFullNameValid(value))
                {
                    CodeError = ErrorCode.InvalidFullName;
                }
                else
                {
                    fullNames[position] = value;
                    CodeError = ErrorCode.Ok;
                }
            }
        }

        public string Invoke(string fullName)
        {
            int position = FindFullName(fullName);
            if (position < 0)
            {
                CodeError = ErrorCode.NotFound;
                return "";
            }
            CodeError = ErrorCode.Ok;
            return phones[position];
        }

        public override string ToString()
        {
            string result = "";
            for (int position = 0; position < count; position++)
            {
                result = result + phones[position] + " - " + fullNames[position];
                if (position < count - 1)
                {
                    result = result + Environment.NewLine;
                }
            }
            return result;
        }

        public void Print()
        {
            Console.WriteLine(ToString());
        }

        public bool ReadFromText(string text)
        {
            if (text == null)
            {
                CodeError = ErrorCode.InvalidInput;
                return false;
            }

            string[] parts = text.Split(new char[] { ';' });
            if (parts.Length != 2)
            {
                CodeError = ErrorCode.InvalidInput;
                return false;
            }
            return Add(parts[0].Trim(), parts[1].Trim());
        }

        public bool Read()
        {
            return ReadFromText(Console.ReadLine());
        }

    }

    class Program {

        public static void Main(string[] args) {

            PhoneDirectory directory = PhoneDirectory.CreateSampleSet();

            Console.WriteLine("Phone directory:");
            directory.Print();
            Console.WriteLine();

            string foundName = directory["+380672223344"];
            Console.WriteLine($"+380672223344 -> {foundName}, CodeError = {directory.CodeError}");

            string missingName = directory["+380000000000"];
            Console.WriteLine($"+380000000000 -> '{missingName}', CodeError = {directory.CodeError}");

            string foundPhone = directory.Invoke("Шевченко Олена Василівна");
            Console.WriteLine($"Шевченко Олена Василівна -> {foundPhone}, CodeError = {directory.CodeError}");

            string missingPhone = directory.Invoke("Невідомий Невідомий Невідомович");
            Console.WriteLine($"Невідомий Невідомий Невідомович -> '{missingPhone}', CodeError = {directory.CodeError}");
            Console.WriteLine();

            directory["+380509998877"] = "Коваль Марія Петрівна";
            Console.WriteLine($"Added new record, Count = {directory.Count}");
            directory["+380509998877"] = "Коваль Марія Іванівна";
            Console.WriteLine($"Updated record: {directory["+380509998877"]}, Count = {directory.Count}");
            Console.WriteLine();

            bool addedDuplicate = directory.Add("+380501112233", "Інший Чоловік Іванович");
            Console.WriteLine($"Add duplicate phone: {addedDuplicate}, CodeError = {directory.CodeError}");

            bool addedInvalidPhone = directory.Add("12ab", "Тестов Тест Тестович");
            Console.WriteLine($"Add invalid phone: {addedInvalidPhone}, CodeError = {directory.CodeError}");

            bool addedInvalidName = directory.Add("+380631112233", "Іваненко Іван");
            Console.WriteLine($"Add invalid full name: {addedInvalidName}, CodeError = {directory.CodeError}");
            Console.WriteLine();

            bool readSucceeded = directory.ReadFromText("+380671234567; Мельник Олег Степанович");
            Console.WriteLine($"ReadFromText: {readSucceeded}, CodeError = {directory.CodeError}");

            bool readFailed = directory.ReadFromText("+380671234568 Мельник Олег Степанович");
            Console.WriteLine($"ReadFromText without ';': {readFailed}, CodeError = {directory.CodeError}");
            Console.WriteLine();

            Console.WriteLine("Phone directory after changes:");
            directory.Print();

        }

    }

}
