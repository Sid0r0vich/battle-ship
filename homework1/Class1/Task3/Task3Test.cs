using NUnit.Framework;
using static NUnit.Framework.Assert;
using static Task3.Task3;

namespace Task3;

public class Tests
{
    [Test]
    public void FTest()
    {
        That(F(0.0), Is.EqualTo(1.0).Within(1e-5));
        That(F(1.0), Is.EqualTo(-1.0).Within(1e-5));
        That(F(-4.0), Is.EqualTo(0.0).Within(1e-5));
    }

    [Test]
    public void NumberOfDaysTest()
    {
        That(NumberOfDays(2021), Is.EqualTo(365));
        That(NumberOfDays(1700), Is.EqualTo(365));
        That(NumberOfDays(1004), Is.EqualTo(366));
    }

    [Test]
    public void Rotate2Test()
    {
        That(Rotate2('С', 1, 2), Is.EqualTo('В'));
        That(Rotate2('З', 1, 1), Is.EqualTo('В'));
        That(Rotate2('Ю', 1, -1), Is.EqualTo('Ю'));
    }

    [Test]
    public void AgeDescriptionTest()
    {
        That(AgeDescription(42), Is.EqualTo("сорок два года"));
        That(AgeDescription(68), Is.EqualTo("шестьдесят восемь лет"));
        That(AgeDescription(21), Is.EqualTo("двадцать один год"));
    }

    [Test]
    public void MainTest()
    {
        Main(Array.Empty<string>());
    }
}