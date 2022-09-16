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
        That(Min3(7, 4, 3), Is.EqualTo(3));
    }

    [Test]
    public void Min3Test3()
    {
        That(Min3(1, 4, 200), Is.EqualTo(1));
    }

    [Test]
    public void Max3Test1()
    {
        That(Max3(2, 0, 3), Is.EqualTo(3));
    }

    [Test]
    public void Max3Test2()
    {
        That(Max3(5, 3, -1), Is.EqualTo(5));
    }

    [Test]
    public void Max3Test3()
    {
        That(Max3(20000000, 0, 3), Is.EqualTo(20000000));
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
    public void Deg2radTest2()
    {
        That(Deg2Rad(540.0), Is.EqualTo(3 * Math.PI).Within(1e-5));
        That(Deg2Rad(360 + 180.0), Is.EqualTo(3 * Math.PI).Within(1e-5));
    }

    [Test]
    public void Deg2radTest3()
    {
        That(Deg2Rad(180.0*8), Is.EqualTo(8 * Math.PI).Within(1e-5));
        That(Deg2Rad(7 * 360 + 180.0), Is.EqualTo(15 * Math.PI).Within(1e-5));
    }

    [Test]
    public void Deg2radTest4()
    {
        That(Deg2Rad(180.0*10), Is.EqualTo(10 * Math.PI).Within(1e-5));
        That(Deg2Rad(2 * 360 + 540.0), Is.EqualTo(7 * Math.PI).Within(1e-5));
    }

    [Test]
    public void Deg2radTest5()
    {
        That(Deg2Rad(180.0 + 540), Is.EqualTo(4 * Math.PI).Within(1e-5));
        That(Deg2Rad(720), Is.EqualTo(4 * Math.PI).Within(1e-5));
    }

    [Test]
    public void Deg2radTest6()
    {
        That(Deg2Rad(180.0 * 90000), Is.EqualTo(90000 * Math.PI).Within(1e-5));
        That(Deg2Rad(0 * 360 + 1800.0), Is.EqualTo(10 * Math.PI).Within(1e-5));
    }

    [Test]
    public void Rad2DegTest2()
    {
        That(Rad2Deg(Math.PI * 10000), Is.EqualTo(180.0 * 10000).Within(1e-5));
        That(Rad2Deg(7 * Math.PI), Is.EqualTo(7 * 180.0).Within(1e-5));
    }

    [Test]
    public void Rad2DegTest3()
    {
        That(Rad2Deg(Math.PI + Math.PI), Is.EqualTo(360.0).Within(1e-5));
        That(Rad2Deg(1000000 * Math.PI), Is.EqualTo(1000000 * 180.0).Within(1e-5));
    }

    [Test]
    public void Rad2DegTest4()
    {
        That(Rad2Deg(4 * Math.PI), Is.EqualTo(4 * 180.0).Within(1e-5));
        That(Rad2Deg(9 * Math.PI), Is.EqualTo(9 * 180.0).Within(1e-5));
    }

    [Test]
    public void Rad2DegTest5()
    {
        That(Rad2Deg(Math.PI * 90000000), Is.EqualTo(90000000 * 180.0).Within(1e-5));
        That(Rad2Deg(6 * Math.PI), Is.EqualTo(6 * 180.0).Within(1e-5));
    }

    [Test]
    public void Rad2DegTest6()
    {
        That(Rad2Deg(Math.PI * 7000000000), Is.EqualTo(180.0 * 7000000000).Within(1e-5));
        That(Rad2Deg(8000 * Math.PI), Is.EqualTo(8000 * 180.0).Within(1e-5));
    }

}
