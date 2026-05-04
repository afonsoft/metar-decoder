using Metar.Decoder.Entity;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Metar.Decoder.Tests.Entity
{
    [TestFixture, Category("Entity")]
    public class PresentWeatherTest
    {
        [Test]
        public void TestPrecipitations()
        {
            var pw = new PresentWeather();
            ClassicAssert.IsNotNull(pw.Precipitations);
            ClassicAssert.AreEqual(0, pw.Precipitations.Count);

            pw.AddPrecipitation(1);
            pw.AddPrecipitation(2);
            pw.AddPrecipitation(3);

            ClassicAssert.AreEqual(3, pw.Precipitations.Count);
            ClassicAssert.AreEqual(1, pw.Precipitations[0]);
            ClassicAssert.AreEqual(2, pw.Precipitations[1]);
            ClassicAssert.AreEqual(3, pw.Precipitations[2]);
        }

        [Test]
        public void TestObscurations()
        {
            var pw = new PresentWeather();
            ClassicAssert.IsNotNull(pw.Obscurations);
            ClassicAssert.AreEqual(0, pw.Obscurations.Count);

            pw.AddObscuration(10);
            pw.AddObscuration(20);

            ClassicAssert.AreEqual(2, pw.Obscurations.Count);
            ClassicAssert.AreEqual(10, pw.Obscurations[0]);
            ClassicAssert.AreEqual(20, pw.Obscurations[1]);
        }

        [Test]
        public void TestVicinities()
        {
            var pw = new PresentWeather();
            ClassicAssert.IsNotNull(pw.Vicinities);
            ClassicAssert.AreEqual(0, pw.Vicinities.Count);

            pw.AddVicinity(100);

            ClassicAssert.AreEqual(1, pw.Vicinities.Count);
            ClassicAssert.AreEqual(100, pw.Vicinities[0]);
        }

        [Test]
        public void TestReadOnlyCollections()
        {
            var pw = new PresentWeather();
            pw.AddPrecipitation(5);
            pw.AddObscuration(15);
            pw.AddVicinity(25);

            var precipitations = pw.Precipitations;
            var obscurations = pw.Obscurations;
            var vicinities = pw.Vicinities;

            ClassicAssert.AreEqual(1, precipitations.Count);
            ClassicAssert.AreEqual(1, obscurations.Count);
            ClassicAssert.AreEqual(1, vicinities.Count);
        }
    }
}
