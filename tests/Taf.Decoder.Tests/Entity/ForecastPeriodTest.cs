using Taf.Decoder.Entity;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Taf.Decoder.Tests.Entity
{
    [TestFixture, Category("Entity")]
    public class ForecastPeriodTest
    {
        [Test]
        public void TestValidForecastPeriod()
        {
            var fp = new ForecastPeriod
            {
                FromDay = 1,
                FromHour = 6,
                ToDay = 1,
                ToHour = 18
            };
            ClassicAssert.IsTrue(fp.IsValid);
        }

        [Test]
        public void TestInvalidNullFromDay()
        {
            var fp = new ForecastPeriod
            {
                FromDay = null,
                FromHour = 6,
                ToDay = 1,
                ToHour = 18
            };
            ClassicAssert.IsFalse(fp.IsValid);
        }

        [Test]
        public void TestInvalidNullFromHour()
        {
            var fp = new ForecastPeriod
            {
                FromDay = 1,
                FromHour = null,
                ToDay = 1,
                ToHour = 18
            };
            ClassicAssert.IsFalse(fp.IsValid);
        }

        [Test]
        public void TestInvalidNullToDay()
        {
            var fp = new ForecastPeriod
            {
                FromDay = 1,
                FromHour = 6,
                ToDay = null,
                ToHour = 18
            };
            ClassicAssert.IsFalse(fp.IsValid);
        }

        [Test]
        public void TestInvalidNullToHour()
        {
            var fp = new ForecastPeriod
            {
                FromDay = 1,
                FromHour = 6,
                ToDay = 1,
                ToHour = null
            };
            ClassicAssert.IsFalse(fp.IsValid);
        }

        [Test]
        public void TestInvalidFromDayZero()
        {
            var fp = new ForecastPeriod
            {
                FromDay = 0,
                FromHour = 6,
                ToDay = 1,
                ToHour = 18
            };
            ClassicAssert.IsFalse(fp.IsValid);
        }

        [Test]
        public void TestInvalidFromDayTooLarge()
        {
            var fp = new ForecastPeriod
            {
                FromDay = 32,
                FromHour = 6,
                ToDay = 1,
                ToHour = 18
            };
            ClassicAssert.IsFalse(fp.IsValid);
        }

        [Test]
        public void TestInvalidToDayZero()
        {
            var fp = new ForecastPeriod
            {
                FromDay = 1,
                FromHour = 6,
                ToDay = 0,
                ToHour = 18
            };
            ClassicAssert.IsFalse(fp.IsValid);
        }

        [Test]
        public void TestInvalidToDayTooLarge()
        {
            var fp = new ForecastPeriod
            {
                FromDay = 1,
                FromHour = 6,
                ToDay = 32,
                ToHour = 18
            };
            ClassicAssert.IsFalse(fp.IsValid);
        }

        [Test]
        public void TestInvalidFromHourTooLarge()
        {
            var fp = new ForecastPeriod
            {
                FromDay = 1,
                FromHour = 25,
                ToDay = 1,
                ToHour = 18
            };
            ClassicAssert.IsFalse(fp.IsValid);
        }

        [Test]
        public void TestInvalidToHourTooLarge()
        {
            var fp = new ForecastPeriod
            {
                FromDay = 1,
                FromHour = 6,
                ToDay = 1,
                ToHour = 25
            };
            ClassicAssert.IsFalse(fp.IsValid);
        }

        [Test]
        public void TestInvalidSameDayFromHourEqualToHour()
        {
            var fp = new ForecastPeriod
            {
                FromDay = 1,
                FromHour = 12,
                ToDay = 1,
                ToHour = 12
            };
            ClassicAssert.IsFalse(fp.IsValid);
        }

        [Test]
        public void TestInvalidSameDayFromHourGreaterThanToHour()
        {
            var fp = new ForecastPeriod
            {
                FromDay = 1,
                FromHour = 18,
                ToDay = 1,
                ToHour = 6
            };
            ClassicAssert.IsFalse(fp.IsValid);
        }

        [Test]
        public void TestValidDifferentDays()
        {
            var fp = new ForecastPeriod
            {
                FromDay = 1,
                FromHour = 18,
                ToDay = 2,
                ToHour = 6
            };
            ClassicAssert.IsTrue(fp.IsValid);
        }

        [Test]
        public void TestValidBoundaryValues()
        {
            var fp = new ForecastPeriod
            {
                FromDay = 1,
                FromHour = 0,
                ToDay = 31,
                ToHour = 24
            };
            ClassicAssert.IsTrue(fp.IsValid);
        }

        [Test]
        public void TestDefaultValues()
        {
            var fp = new ForecastPeriod();
            ClassicAssert.IsNull(fp.FromDay);
            ClassicAssert.IsNull(fp.FromHour);
            ClassicAssert.IsNull(fp.ToDay);
            ClassicAssert.IsNull(fp.ToHour);
            ClassicAssert.IsFalse(fp.IsValid);
        }
    }
}
