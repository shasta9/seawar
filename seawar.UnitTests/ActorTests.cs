using NodaTime;
using NUnit.Framework;
using seawar.Actors;

namespace seawar.UnitTests {
   [TestFixture]
   public class ActorTests {
      [Test]
      public void Creation() {
         var a = new Actor(null, null);
         Assert.AreEqual(string.Empty, a.Name);
         var b = new Actor(null, null) { Name = "B" };
         Assert.AreEqual("B", b.Name);
      }

      [Test]
      public void Update() {
         var spy = new ActionSpy();
         var a = new Actor(null,null);
         a.AddAction(spy);
         Assert.AreEqual(0,spy.PerformedCount);
         Assert.IsFalse(spy.IsComplete);
         a.Update(Duration.Epsilon);
         Assert.AreEqual(1, spy.PerformedCount);
         Assert.IsTrue(spy.IsComplete);
         a.Update(Duration.Epsilon);
         Assert.AreEqual(1, spy.PerformedCount);
      }
   }
}
