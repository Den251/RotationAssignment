
using Microsoft.AspNetCore.Mvc;
using RotationAssignment.Controllers;
using RotationAssignment.Models;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;


namespace UnitTests
{
    public class RotationControllerTests
    {
        [Fact]
        public void CreateCargoReturnsCreated()
        {
            RotationController rotationController = new RotationController(new List<Cargo> (), new Rotation (), new List <TimeStamp>());
            var result = rotationController.CreateCargo(new Cargo());
            var okResult = result as StatusCodeResult;
            Assert.NotNull(result);
            Assert.Equal(((int)HttpStatusCode.Created).ToString(), okResult.StatusCode.ToString());
        }

        [Fact]
        public void DeletCargoReturnsNoContent()
        {
            //var mock = new Mock<IRepository>();
            //mock.Setup(repo => repo.GetAll()).Returns(GetTestUsers());
            //var controller = new HomeController(mock.Object);

            RotationController rotationController = new RotationController(new List<Cargo>(), new Rotation(), new List<TimeStamp>());
            string id = "1";
            var result = rotationController.DeleteCargo(id);
            Assert.NotNull(result);
            Assert.Equal(((int)HttpStatusCode.NoContent).ToString(), result.StatusCode.ToString());
        }

    }
}
