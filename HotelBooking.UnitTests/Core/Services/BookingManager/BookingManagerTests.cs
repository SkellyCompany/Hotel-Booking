using System;
using HotelBooking.Core;
using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace HotelBooking.UnitTests {
	public class BookingManagerTests {
		private IBookingManager bookingManager;
		private Mock<IRepository<Booking>> bookingRepository;
		private List<Booking> bookings;
		private Mock<IRepository<Room>> roomRepository;
		private List<Room> rooms;

		public BookingManagerTests() {
			// Booking Repository
			bookingRepository = new Mock<IRepository<Booking>>();
			bookings = new List<Booking> { };
			bookingRepository.Setup(x => x.GetAll()).Returns(bookings);

			// Room Repository
			roomRepository = new Mock<IRepository<Room>>();
			rooms = new List<Room>
			{
				new Room { Id=1, Description="A" },
				new Room { Id=2, Description="B" },
			};
			roomRepository.Setup(x => x.GetAll()).Returns(rooms);

			// Booking Manager
			bookingManager = new BookingManager(bookingRepository.Object, roomRepository.Object);
		}

		[Theory]
		[JsonData("Core/Services/BookingManager/TestData/FindAvailableRoom_StartDateNotInTheFuture.json")]
		public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException(int startDateDaysFromToday) {
			// Arrange
			DateTime startDate = DateTime.Today.AddDays(startDateDaysFromToday);
			DateTime endDate = startDate.AddDays(1);

			// Act
			Action act = () => bookingManager.FindAvailableRoom(startDate, endDate);

			// Assert
			Assert.Throws<ArgumentException>(act);
		}

		[Theory]
		[JsonData("Core/Services/BookingManager/TestData/FindAvailableRoom_EndDateNotInTheFuture.json")]
		public void FindAvailableRoom_EndDateNotInTheFuture_ThrowsArgumentException(int endDateDaysFromToday) {
			// Arrange
			DateTime endDate = DateTime.Today.AddDays(endDateDaysFromToday);
			DateTime startDate = endDate.AddDays(-1);


			// Act
			Action act = () => bookingManager.FindAvailableRoom(startDate, endDate);

			// Assert
			Assert.Throws<ArgumentException>(act);
		}

		[Theory]
		[JsonData("Core/Services/BookingManager/TestData/FindAvailableRoom_RoomAvailable.json")]
		public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne(int startDateDaysFromToday, int endDateDaysFromToday) {
			// Arrange
			DateTime startDate = DateTime.Today.AddDays(startDateDaysFromToday);
			DateTime endDate = DateTime.Today.AddDays(endDateDaysFromToday);

			// Act
			int roomId = bookingManager.FindAvailableRoom(startDate, endDate);

			// Assert
			Assert.Contains(roomId, rooms.Select(x => x.Id).ToArray());
		}
	}
}
