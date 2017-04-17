using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;
using SportsStore.WebUI.HtmlHelpers;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            //准备
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID=1,Name="P1" },
                new Product {ProductID=2,Name="P2" },
                new Product {ProductID=3,Name="P3" },
                new Product {ProductID=4,Name="P4" },
                new Product {ProductID=5,Name="P5" }
            });
            //创建控制器,并使页面大小(PageSize)容纳3个物品
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //动作
            //IEnumerable<Product> result = (IEnumerable<Product>)controller.List(2).Model;
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null,2).Model;

            //断言
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
            //动作
            
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            //准备
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID=1,Name="P1" },
                new Product {ProductID=2,Name="P2" },
                new Product {ProductID=3,Name="P3" },
                new Product {ProductID=4,Name="P4" },
                new Product {ProductID=5,Name="P5" }
            });
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //动作
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null,2).Model;

            //断言
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //准备 - 定义一个HTML辅助器,这是必须的,目的是运用扩展方法
            HtmlHelper myHelper = null;

            //准备 - 创建PagingInfo数据
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            //准备 - 用lambda表达式创建委托
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            //动作
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            //断言
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
            + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
            + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
            result.ToString());
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            //准备 - 创建模仿储存库
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID=1,Name="P1",Category="Cat1" },
                new Product {ProductID=2,Name="P2",Category="Cat2" },
                new Product {ProductID=3,Name="P3",Category="Cat1" },
                new Product {ProductID=4,Name="P4",Category="Cat2" },
                new Product {ProductID=5,Name="P5",Category="Cat3" }
            });

            //准备 - 创建控制器,并使页面大小为3个物品
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //动作
            Product[] result = ((ProductsListViewModel)controller.List("Cat2", 1).Model).Products.ToArray();

            //断言
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m=>m.Products).Returns(new Product[] {
                new Product {ProductID=1,Name="P1",Category="Apples" },
                new Product {ProductID=2,Name="P2",Category="Apples" },
                new Product {ProductID=3,Name="P3",Category="Plums" },
                new Product {ProductID=4,Name="P4",Category="Oranges" }
            });

            NavController target = new NavController(mock.Object);

            string[] results = ((IEnumerable<string>)target.Menu().Model).ToArray();

            Assert.AreEqual(results.Length, 3);
            Assert.AreEqual(results[0], "Apples");
            Assert.AreEqual(results[1], "Oranges");
            Assert.AreEqual(results[2], "Plums");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m=>m.Products).Returns(new Product[] {
                new Product {ProductID=1,Name="P1",Category="Apples" },
                new Product {ProductID=4,Name="P2",Category="Oranges" }
            });

            NavController target = new NavController(mock.Object);

            string categoryToSelect = "Apples";

            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(m=>m.Products).Returns(new Product[] {
                new Product {ProductID=1,Name="P1",Category="Cat1" },
                new Product {ProductID=2,Name="P2",Category="Cat2" },
                new Product {ProductID=3,Name="P3",Category="Cat1" },
                new Product {ProductID=4,Name="P4",Category="Cat2" },
                new Product {ProductID=5,Name="P5",Category="Cat3" }
            });

            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;

            int res1 = ((ProductsListViewModel)target.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((ProductsListViewModel)target.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((ProductsListViewModel)target.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((ProductsListViewModel)target.List(null).Model).PagingInfo.TotalItems;

            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}
