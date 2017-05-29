using ElectoralCalculator.MVVMMessages;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectoralCalculator.Tests
{
    public class TestMessage : MessageBase
    {
    }

    [TestFixture]
    public class MVVMServiceTests
    {
        public MVVMServiceTests()
        {
            MVVMMessagerService.Init();
        }

        [SetUp]
        public void InitializeTest()
        {
            if (MVVMMessagerService.ReceiverExist<TestMessage>())
                MVVMMessagerService.UnregisterReceiver<TestMessage>();
        }

        [Test]
        public void registered_receiver_should_exist()
        {
            MVVMMessagerService.RegisterReceiver<TestMessage>(
                (test) => { });

            Assert.IsTrue(MVVMMessagerService.ReceiverExist<TestMessage>());
        }

        [Test]
        public void registered_method_should_be_executed()
        {
            MVVMMessagerService.RegisterReceiver<TestMessage>(
                (test) => { Assert.Pass(); });
        }

        [Test]
        public void registering_twice_same_type_of_receiever_should_throw_error()
        {
            MVVMMessagerService.RegisterReceiver<TestMessage>(
                (test) => { });

            Assert.Throws<Exception>(new TestDelegate(testMethod));
               
            void testMethod()
            {
                MVVMMessagerService.RegisterReceiver<TestMessage>(
               (test) => { });
            }
        }

        [Test]
        public void unregistering_unexisted_receiver_should_throw_error()
        {
            Assert.Throws<Exception>(unregister_unexist_receiver);

            void unregister_unexist_receiver()
            {
                MVVMMessagerService.UnregisterReceiver<TestMessage>();
            }
        }

        [Test]
        public void unregistering_receivers_should_work()
        {
            MVVMMessagerService.RegisterReceiver<TestMessage>((t) => { });
            MVVMMessagerService.UnregisterReceiver<TestMessage>();

            Assert.IsFalse(MVVMMessagerService.ReceiverExist<TestMessage>());
        }
    }
}
