
using System;
using System.Collections.Generic;
using System.Linq;
using ScottPlot;

class Program
{
    const int Count = 100000;

    static Random random = new Random();

    static void Main()
    {
        // 1. Створюємо два мішки слів
        List<string> uniformBag = new List<string>();
        List<string> normalBag = new List<string>();

        // 2. Генеруємо 100 000 значень
        for (int i = 0; i < Count; i++)
        {
            // Рівномірний розподіл
            int uniformNumber = random.Next(0, 100);

            // Нормальний розподіл
            int normalNumber = GenerateNormalNumber();

            uniformBag.Add(uniformNumber.ToString("D2"));
            normalBag.Add(normalNumber.ToString("D2"));
        }

        // 3. Будуємо гістограми
        int[] uniformHistogram = BuildHistogram(uniformBag);
        int[] normalHistogram = BuildHistogram(normalBag);

        // 4. Виводимо результати
        Console.WriteLine("Кількість символів: " + Count);

        Console.WriteLine("\nРівномірний розподіл:");
        PrintHistogram(uniformHistogram);

        Console.WriteLine("\nНормальний розподіл:");
        PrintHistogram(normalHistogram);

        // 5. Створюємо графіки
        DrawHistogram(
            uniformHistogram,
            "Uniform Distribution",
            "uniform.png"
        );

        DrawHistogram(
            normalHistogram,
            "Normal Distribution",
            "normal.png"
        );

        Console.WriteLine("\nГістограми збережено:");
        Console.WriteLine("uniform.png");
        Console.WriteLine("normal.png");

        Console.WriteLine("\nНатисніть Enter для завершення...");
        Console.ReadLine();
    }

    // Генератор нормального розподілу (Box-Muller)
    static double GenerateNormal()
    {
        double u1 = 1.0 - random.NextDouble();
        double u2 = 1.0 - random.NextDouble();

        double z = Math.Sqrt(-2.0 * Math.Log(u1)) *
                   Math.Sin(2.0 * Math.PI * u2);

        return z;
    }

    // Перетворення нормального розподілу в діапазон 0-99
    static int GenerateNormalNumber()
    {
        double z = GenerateNormal();

        double value = 50 + z * 16;

        int number = (int)Math.Round(value);

        return Math.Clamp(number, 0, 99);
    }

    // Підрахунок частоти появи чисел
    static int[] BuildHistogram(List<string> bag)
    {
        int[] histogram = new int[100];

        foreach (string item in bag)
        {
            int number = int.Parse(item);

            histogram[number]++;
        }

        return histogram;
    }

    // Виведення гістограми в консоль
    static void PrintHistogram(int[] histogram)
    {
        for (int i = 0; i < histogram.Length; i++)
        {
            Console.WriteLine(
                $"{i:D2}: {histogram[i]}"
            );
        }
    }

    // Побудова та збереження графіка
    static void DrawHistogram(
        int[] data,
        string title,
        string fileName)
    {
        var plt = new Plot();

        double[] values = data
            .Select(x => (double)x)
            .ToArray();

        plt.Add.Bars(values);

        plt.Title(title);
        plt.XLabel("Number (00-99)");
        plt.YLabel("Frequency");

        plt.SavePng(fileName, 1200, 600);
    }
}
