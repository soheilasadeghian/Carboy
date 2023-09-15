using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarBoyWebservice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarBoyWebservice.Tests
{
    [TestClass()]
    public class EngineTests
    {
        [TestMethod()]
        public void geDistributorStartLocationTest()
        {
            Engine eng = new Engine();
            eng.geDistributorStartLocation(1, 1508239485624);

            Assert.Fail();
        }

        [TestMethod()]
        public void getCarboyPathTest()
        {
            Engine eng = new Engine();
            eng.getCarboyPath(1, 1, 720);

            Assert.Fail();
        }

        [TestMethod()]
        public void getCarboyServiceListTest()
        {
            var eng = new Engine();

            eng.getCarboyServiceList(5, Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds));

            Assert.Fail();
        }

        [TestMethod()]
        public void getCarboyServiceDetailTest()
        {
            var eng = new Engine();

            var result = eng.getCarboyServiceDetail(7, "1CC39B");

            Assert.Fail();
        }

        [TestMethod()]
        public void carboyMoveToCustomerTest()
        {

            var eng = new Engine();

            var result = eng.carboyMoveToCustomer(1, "125552");


            Assert.Fail();
        }

        [TestMethod()]
        public void carboyServiceCompleteTest()
        {
            var eng = new Engine();

            var result = eng.carboyServiceComplete(28,"2","136982");
            Assert.Fail();
        }

        [TestMethod()]
        public void getRequiredDistributorProductTest()
        {
            var eng = new Engine();

            var result = eng.getRequiredDistributorProduct(7, 1512034336572);

            Assert.Fail();
        }

        [TestMethod()]
        public void getUserProductListTest()
        {
            var eng = new Engine();

            var result = eng.getUserProductList(7);

            Assert.Fail();
        }

        [TestMethod()]
        public void editCustomerCarTest()
        {
            var eng = new Engine();

            var result = eng.editCustomerCar(7, "644144", 542);
            Assert.Fail();
        }

        [TestMethod()]
        public void carboyRequestServiceCompleteTest1()
        {
            var eng = new Engine();

            var result = eng.carboyRequestServiceComplete(12, "4AC71C");
            Assert.Fail();
        }

        [TestMethod()]
        public void carboyServiceCompleteTest1()
        {
            var eng = new Engine();

            var result = eng.carboyServiceComplete(12, "loWSJSYLIGuZMY7N4thBD229hp16ceyQ0lxV8dfx/l0=", "192285");
            Assert.Fail();
        }
        

    }
}