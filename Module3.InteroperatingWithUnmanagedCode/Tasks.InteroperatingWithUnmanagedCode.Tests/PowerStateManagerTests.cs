using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tasks.InteroperatingWithUnmanagedCode.Tests
{
    [TestClass]
    public class PowerStateManagerTests
    {
        [TestMethod]
        public void GetLastSleepTimeTest()
        {
            // Arrange
            var powerManager = new PowerStateManager();

            // Act
            var date = powerManager.GetLastSleepTime();

            // Assert
            Console.WriteLine($"Last Sleep Time in UTC: {date:u}. In Local Time: {date.ToLocalTime()}");
        }

        [TestMethod]
        public void GetLastWakeTimeTest()
        {
            // Arrange
            var powerManager = new PowerStateManager();

            // Act
            var date = powerManager.GetLastWakeTime();

            // Assert
            Console.WriteLine($"Last Wake Time in UTC: {date:u}. In Local Time: {date.ToLocalTime()}");
        }

        [TestMethod]
        public void GetSystemBatteryStateTest()
        {
            // Arrange
            var powerManager = new PowerStateManager();

            // Act
            var state = powerManager.GetSystemBatteryState();

            // Assert
            Console.WriteLine($"System Battery State: {state}");
        }

        [TestMethod]
        public void GetSystemPowerInformationTest()
        {
            // Arrange
            var powerManager = new PowerStateManager();

            // Act
            var information = powerManager.GetSystemPowerInformation();

            // Assert
            Console.WriteLine($"System Power Information: {information}");
        }

        [TestMethod]
        public void ReserveHibernationFileTest()
        {
            // Arrange
            var powerManager = new PowerStateManager();

            // Act
            powerManager.ReserveHibernationFile();

            // Assert
        }

        [TestMethod]
        public void RemoveHibernationFileTest()
        {
            // Arrange
            var powerManager = new PowerStateManager();

            // Act
            powerManager.RemoveHibernationFile();

            // Assert
        }

        [TestMethod]
        public void SleepSystemTest()
        {
            // Arrange
            var powerManager = new PowerStateManager();

            // Act
            powerManager.SleepSystem();

            // Assert
        }

        [TestMethod]
        public void HibernateSystemTest()
        {
            // Arrange
            var powerManager = new PowerStateManager();

            // Act
            powerManager.HibernateSystem();

            // Assert
        }
    }
}
