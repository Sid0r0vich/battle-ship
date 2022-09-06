using NUnit.Framework;
using static NUnit.Framework.Assert;
using static Task2.Task2;

namespace Task2;

public class Tests
{
    [Test]
    public void Min3Test1()
    {
        That(Min3(2, 0, 3), Is.EqualTo(0));
    }

    [Test]
    public void Min3Test2()
    {
        throw new NotImplementedException("Добавьте тестовый случай");
    }

    [Test]
    public void Min3Test3()
    {
        throw new NotImplementedException("Добавьте тестовый случай");
    }

    [Test]
    public void Max3Test1()
    {
        throw new NotImplementedException("Добавьте тестовый случай");
    }

    [Test]
    public void Max3Test2()
    {
        throw new NotImplementedException("Добавьте тестовый случай");
    }

    [Test]
    public void Max3Test3()
    {
        throw new NotImplementedException("Добавьте тестовый случай");
    }

    [Test]
    public void Deg2RadTest1()
    {
        That(Deg2Rad(180.0), Is.EqualTo(Math.PI).Within(1e-5));
        That(Deg2Rad(2 * 360 + 180.0), Is.EqualTo(5 * Math.PI).Within(1e-5));
    }

    [Test]
    public void Rad2DegTest1()
    {
        That(Rad2Deg(Math.PI), Is.EqualTo(180.0).Within(1e-5));
        That(Rad2Deg(5 * Math.PI), Is.EqualTo(5 * 180.0).Within(1e-5));
    }

    [Test]
    public void MoreRadDegTests()
    {
        throw new NotImplementedException(
            "Напишите для каждой функции `deg2Rad` и `rad2Deg` по 5 тестов, проверяющих разные случаи");
    }
}