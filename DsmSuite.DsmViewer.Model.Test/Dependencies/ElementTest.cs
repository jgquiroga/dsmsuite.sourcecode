﻿using DsmSuite.DsmViewer.Model.Dependencies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DsmSuite.DsmViewer.Model.Test.Dependencies
{
    [TestClass]
    public class ElementTest
    {
        [TestMethod]
        public void TestElementConstructor()
        {
            int elementId = 1;
            string elementName = "name1";
            string elementType = "file";
            Element element = new Element(elementId, elementName, elementType);
            Assert.AreEqual(elementId, element.Id);
            Assert.AreEqual(elementName, element.Name);
            Assert.AreEqual(elementType, element.Type);
            Assert.AreEqual(0, element.Order);
            Assert.AreEqual(elementName, element.Fullname);
        }

        [TestMethod]
        public void TestElementHierarchy()
        {
            int parentId = 1;
            string parentName = "parent";
            Element parent = new Element(parentId, parentName, "");
            Assert.AreEqual(null, parent.Parent);
            Assert.AreEqual(0, parent.Children.Count);

            int child1Id = 10;
            string child1Name = "child1";
            Element child1 = new Element(child1Id, child1Name, "");
            Assert.AreEqual(null, child1.Parent);
            Assert.AreEqual(null, child1.FirstChild);
            Assert.AreEqual(null, child1.PreviousSibling);
            Assert.AreEqual(null, child1.NextSibling);

            parent.AddChild(child1);
            Assert.AreEqual("parent.child1", child1.Fullname);

            Assert.AreEqual(1, parent.Children.Count);
            Assert.AreEqual(child1, parent.FirstChild);
            Assert.AreEqual(child1, parent.Children[0]);
            Assert.AreEqual(parent, child1.Parent);
            Assert.AreEqual(null, child1.PreviousSibling);
            Assert.AreEqual(null, child1.NextSibling);

            int child2Id = 100;
            string child2Name = "child2";
            Element child2 = new Element(child2Id, child2Name, "");
            Assert.AreEqual(null, child2.Parent);
            Assert.AreEqual(null, child2.FirstChild);
            Assert.AreEqual(null, child2.PreviousSibling);
            Assert.AreEqual(null, child2.NextSibling);

            parent.AddChild(child2);
            Assert.AreEqual("parent.child2", child2.Fullname);

            Assert.AreEqual(2, parent.Children.Count);
            Assert.AreEqual(child1, parent.FirstChild);
            Assert.AreEqual(child1, parent.Children[0]);
            Assert.AreEqual(child2, parent.Children[1]);
            Assert.AreEqual(parent, child2.Parent);
            Assert.AreEqual(null, child1.PreviousSibling);
            Assert.AreEqual(child2, child1.NextSibling);
            Assert.AreEqual(child1, child2.PreviousSibling);
            Assert.AreEqual(null, child2.NextSibling);

            int child3Id = 1000;
            string child3Name = "child3";
            Element child3 = new Element(child3Id, child3Name, "");
            Assert.AreEqual(null, child3.Parent);
            Assert.AreEqual(null, child3.Parent);
            Assert.AreEqual(null, child3.FirstChild);
            Assert.AreEqual(null, child3.PreviousSibling);
            Assert.AreEqual(null, child3.NextSibling);

            parent.AddChild(child3);
            Assert.AreEqual("parent.child3", child3.Fullname);

            Assert.AreEqual(3, parent.Children.Count);
            Assert.AreEqual(child1, parent.FirstChild);
            Assert.AreEqual(child1, parent.Children[0]);
            Assert.AreEqual(child2, parent.Children[1]);
            Assert.AreEqual(child3, parent.Children[2]);
            Assert.AreEqual(parent, child2.Parent);
            Assert.AreEqual(null, child1.PreviousSibling);
            Assert.AreEqual(child2, child1.NextSibling);
            Assert.AreEqual(child1, child2.PreviousSibling);
            Assert.AreEqual(child3, child2.NextSibling);
            Assert.AreEqual(child2, child3.PreviousSibling);
            Assert.AreEqual(null, child3.NextSibling);

            parent.RemoveChild(child1);
            Assert.AreEqual(null, child1.Parent);
            Assert.AreEqual(2, parent.Children.Count);
            Assert.AreEqual(child2, parent.FirstChild);
            Assert.AreEqual(child2, parent.Children[0]);
            Assert.AreEqual(child3, parent.Children[1]);

            parent.RemoveChild(child2);
            Assert.AreEqual(null, child2.Parent);
            Assert.AreEqual(1, parent.Children.Count);
            Assert.AreEqual(child3, parent.FirstChild);
            Assert.AreEqual(child3, parent.Children[0]);

            parent.RemoveChild(child3);
            Assert.AreEqual(null, child3.Parent);
            Assert.AreEqual(0, parent.Children.Count);
        }

        [TestMethod]
        public void TestElementSwap()
        {
            int parentId = 1;
            string parentName = "parent";
            Element parent = new Element(parentId, parentName, "");

            int child1Id = 10;
            string child1Name = "child1";
            Element child1 = new Element(child1Id, child1Name, "");
            parent.AddChild(child1);

            int child2Id = 100;
            string child2Name = "child2";
            Element child2 = new Element(child2Id, child2Name, "");
            parent.AddChild(child2);

            int child3Id = 1000;
            string child3Name = "child3";
            Element child3 = new Element(child3Id, child3Name, "");
            parent.AddChild(child3);

            Assert.AreEqual(3, parent.Children.Count);
            Assert.AreEqual(child1, parent.Children[0]);
            Assert.AreEqual(child2, parent.Children[1]);
            Assert.AreEqual(child3, parent.Children[2]);

            Assert.IsTrue(parent.Swap(child1, child2));

            Assert.AreEqual(3, parent.Children.Count);
            Assert.AreEqual(child2, parent.Children[0]);
            Assert.AreEqual(child1, parent.Children[1]);
            Assert.AreEqual(child3, parent.Children[2]);
        }
    }
}
