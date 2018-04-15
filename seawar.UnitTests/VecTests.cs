using NUnit.Framework;

namespace seawar.UnitTests {
   [TestFixture]
   public class VecTests {
      [Test]
      public void BearingAndDistance() {
         var v = new Vec(0, 0);
         Assert.AreEqual(0.0, v.Bearing);
         Assert.That(v.Length, Is.EqualTo(0.0).Within(0.001));
         v = new Vec(0, 1);
         Assert.AreEqual(0.0, v.Bearing);
         Assert.That(v.Length, Is.EqualTo(1.0).Within(0.001));
         v = new Vec(1, 1);
         Assert.AreEqual(45.0, v.Bearing);
         Assert.That(v.Length, Is.EqualTo(1.4142).Within(0.001));
         v = new Vec(1, 0);
         Assert.AreEqual(90.0, v.Bearing);
         Assert.That(v.Length, Is.EqualTo(1.0).Within(0.001));
         v = new Vec(1, -1);
         Assert.AreEqual(135.0, v.Bearing);
         Assert.That(v.Length, Is.EqualTo(1.4142).Within(0.001));
         v = new Vec(0, -1);
         Assert.AreEqual(180.0, v.Bearing);
         Assert.That(v.Length, Is.EqualTo(1.0).Within(0.001));
         v = new Vec(-1, -1);
         Assert.AreEqual(225.0, v.Bearing);
         Assert.That(v.Length, Is.EqualTo(1.4142).Within(0.001));
         v = new Vec(-1, 0);
         Assert.AreEqual(270.0, v.Bearing);
         Assert.That(v.Length, Is.EqualTo(1.0).Within(0.001));
         v = new Vec(-1, 1);
         Assert.AreEqual(315.0, v.Bearing);
         Assert.That(v.Length, Is.EqualTo(1.4142).Within(0.001));
      }

      [Test]
      public void Equality() {
         var a = new Vec(1, 2);
         Assert.AreEqual(new Vec(1, 2), a);
         Assert.AreNotEqual(new object(), a);
      }

      [Test]
      public void Addition() {
         var a = new Vec(1, 2);
         var b = new Vec(3, 4);
         Assert.AreEqual(new Vec(4, 6), a + b);
      }

      [Test]
      public void Subtraction() {
         var a = new Vec(1, 2);
         var b = new Vec(3, 4);
         Assert.AreEqual(new Vec(-2, -2), a - b);
         Assert.AreEqual(new Vec(2, 2), b - a);
      }
   }
}
