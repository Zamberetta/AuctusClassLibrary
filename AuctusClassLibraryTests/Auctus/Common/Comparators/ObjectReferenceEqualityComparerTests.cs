using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Auctus.DataMiner.Library.Common.Comparators.Tests
{
    [TestClass]
    public class ObjectReferenceEqualityComparerTests
    {
        [TestMethod]
        public void InstanceProperty_ReturnsSingleton()
        {
            ObjectReferenceEqualityComparer comparer1 = ObjectReferenceEqualityComparer.Instance;
            comparer1.Should().NotBeNull();

            ObjectReferenceEqualityComparer comparer2 = ObjectReferenceEqualityComparer.Instance;
            comparer1.Should().Be(comparer2);
        }

        [TestMethod]
        public void Equals_UsesReferenceEquals()
        {
            MyClass o1 = new MyClass { SomeInt = 10 };
            MyClass o2 = new MyClass { SomeInt = 10 };
            o1.Should().Be(o2);

            ObjectReferenceEqualityComparer comparer1 = ObjectReferenceEqualityComparer.Instance;
            IEqualityComparer comparer2 = ObjectReferenceEqualityComparer.Instance;
            IEqualityComparer<object> comparer3 = ObjectReferenceEqualityComparer.Instance;
            IEqualityComparer<MyClass> comparer4 = ObjectReferenceEqualityComparer.Instance; // Test Contravariance

            comparer1.Equals(null, null).Should().BeTrue();
            comparer1.Equals(null, o2).Should().BeFalse();
            comparer1.Equals(o1, o2).Should().BeFalse();
            comparer1.Equals(o1, null).Should().BeFalse();

            comparer2.Equals(null, null).Should().BeTrue();
            comparer2.Equals(null, o2).Should().BeFalse();
            comparer2.Equals(o1, o2).Should().BeFalse();
            comparer2.Equals(o1, null).Should().BeFalse();

            comparer3.Equals(null, null).Should().BeTrue();
            comparer3.Equals(null, o2).Should().BeFalse();
            comparer3.Equals(o1, o2).Should().BeFalse();
            comparer3.Equals(o1, null).Should().BeFalse();

            comparer4.Equals(null, null).Should().BeTrue();
            comparer4.Equals(null, o2).Should().BeFalse();
            comparer4.Equals(o1, o2).Should().BeFalse();
            comparer4.Equals(o1, null).Should().BeFalse();
        }

        [TestMethod]
        public void GetHashCode_UsesRuntimeHelpers()
        {
            ClassWithBadGetHashCodeImplementation o = new ClassWithBadGetHashCodeImplementation(); // Make sure we don't call object.GetHashCode().

            ObjectReferenceEqualityComparer comparer1 = ObjectReferenceEqualityComparer.Instance;
            IEqualityComparer comparer2 = ObjectReferenceEqualityComparer.Instance;
            IEqualityComparer<object> comparer3 = ObjectReferenceEqualityComparer.Instance;
            IEqualityComparer<ClassWithBadGetHashCodeImplementation> comparer4 = ObjectReferenceEqualityComparer.Instance; // Test Contravariance.

            int runtimeHelpersHashCode = RuntimeHelpers.GetHashCode(o);

            runtimeHelpersHashCode.Should().Be(comparer1.GetHashCode(o));
            runtimeHelpersHashCode.Should().Be(comparer2.GetHashCode(o));
            runtimeHelpersHashCode.Should().Be(comparer3.GetHashCode(o));
            runtimeHelpersHashCode.Should().Be(comparer4.GetHashCode(o));
        }

        private class ClassWithBadGetHashCodeImplementation
        {
            public override int GetHashCode() => throw new NotImplementedException();
        }

        private class MyClass
        {
            public int SomeInt;
            private readonly int SomeHash = 10;

            public override bool Equals(object obj) => obj is MyClass c && this.SomeInt == c.SomeInt;

            public override int GetHashCode()
            {
                return SomeHash;
            }
        }
    }
}