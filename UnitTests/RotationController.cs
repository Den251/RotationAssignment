
using Microsoft.AspNetCore.Mvc;
using RotationAssignment.Controllers;
using RotationAssignment.Models;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;
using Moq;
using Xunit.Sdk;

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
            // Arrange
            List<Cargo> cargolist = new List<Cargo>();
            cargolist.Add(new Cargo() { Id = "1", ATC = DateTime.Now, Name = "cargo", TerminalId = "0175d159-f61f-40b9-aaf7-967dbe461349" });
            List<TimeStamp> prospect = new List<TimeStamp>();
            prospect.Add(new TimeStamp() { Id = "1", Time = DateTime.Now, Description = "timeStamp", Type = TimeTypes.ATC });
            string id = "1";
            // Act
            RotationController rotationController = new RotationController(cargolist, new Rotation(), prospect);
            var result = rotationController.DeleteCargo(id);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(((int)HttpStatusCode.NoContent).ToString(), result.StatusCode.ToString());
        }


       
        [Fact]
        public void RotationReturnsCorrectObject()
        {
            // Arrange
            List<Cargo> cargolist = new List<Cargo>();
            cargolist.AddRange( new List<Cargo>() {
                new Cargo() { Id = "1", ATC = DateTime.Now, Name = "cargo1", TerminalId = "0175d159-f61f-40b9-aaf7-967dbe461349" },
                new Cargo() { Id = "2", ATC = DateTime.Now, Name = "cargo2", TerminalId = "0175d159-f61f-40b9-aaf7-967dbe461349" },
                new Cargo() { Id = "3", ATC = DateTime.Now, Name = "cargo3", TerminalId = "02ec54f7-8a50-454f-9503-62eeb2f3f4d0" } });
            Rotation rotation = new Rotation();
            rotation.Terminals.AddRange(new List<Rotation.Terminal>() {
                new Rotation.Terminal(){ TerminalId ="0175d159-f61f-40b9-aaf7-967dbe461349", Cargoes = new List<Rotation.Cargo> { new Rotation.Cargo() { CargoId = "1"}, new Rotation.Cargo() { CargoId = "2" } } },
                new Rotation.Terminal(){ TerminalId ="02ec54f7-8a50-454f-9503-62eeb2f3f4d0", Cargoes = new List<Rotation.Cargo> { new Rotation.Cargo() { CargoId = "3"} } }
            });
            
            // Act
            RotationController rotationController = new RotationController(cargolist, rotation, new List<TimeStamp>());
            var result = rotationController.GetRotation();
            // Assert
            Assert.NotNull(result);
            Assert.Equal(rotation, result);
        }
        [Fact]
        public void EditRotation()
        {
            // Arrange
            
            Rotation rotation = new Rotation();
            rotation.Terminals.AddRange(new List<Rotation.Terminal>() {
                new Rotation.Terminal(){ TerminalId ="0175d159-f61f-40b9-aaf7-967dbe461349", Cargoes = new List<Rotation.Cargo> { new Rotation.Cargo() { CargoId = "1"}, new Rotation.Cargo() { CargoId = "2" } } },
                new Rotation.Terminal(){ TerminalId ="02ec54f7-8a50-454f-9503-62eeb2f3f4d0", Cargoes = new List<Rotation.Cargo> { new Rotation.Cargo() { CargoId = "3"} } }
            });
            //Rotation rotationResult = new Rotation();
            //rotation.Terminals.AddRange(new List<Rotation.Terminal>() {
            //    new Rotation.Terminal(){ TerminalId ="0175d159-f61f-40b9-aaf7-967dbe461349", Cargoes = new List<Rotation.Cargo> { new Rotation.Cargo() { CargoId = "2"} } },
            //    new Rotation.Terminal(){ TerminalId ="02ec54f7-8a50-454f-9503-62eeb2f3f4d0", Cargoes = new List<Rotation.Cargo> { new Rotation.Cargo() { CargoId = "1"} } }
            //});
            Rotation rotationChanges = new Rotation();
            rotation.Terminals.AddRange(new List<Rotation.Terminal>() {
                new Rotation.Terminal(){ TerminalId ="02ec54f7-8a50-454f-9503-62eeb2f3f4d0", Cargoes = new List<Rotation.Cargo> { new Rotation.Cargo() { CargoId = "1"} } }
                
            });

            // Act
            RotationController rotationController = new RotationController(new List<Cargo>(), rotation, new List<TimeStamp>());

            // Assert
            var result = rotationController.EditRotation(rotationChanges) as StatusCodeResult;
            Assert.NotNull(result);
            Assert.Equal(((int)HttpStatusCode.OK).ToString(), result.StatusCode.ToString());

        }

    }
}
