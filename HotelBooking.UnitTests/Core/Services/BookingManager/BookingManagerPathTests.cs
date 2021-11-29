using System;
using HotelBooking.Core;
using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace HotelBooking.UnitTests {
	public class BookingManagerPathTests {
		private IBookingManager bookingManager;
		private Mock<IRepository<Booking>> bookingRepository;
		private List<Booking> emptyBookings;
		private Mock<IRepository<Room>> roomRepository;
		private List<Room> oneRoom;
		private List<Room> twoRooms;

		public BookingManagerPathTests() {
			// Booking Repository
			bookingRepository = new Mock<IRepository<Booking>>();
			emptyBookings = new List<Booking> { };

			// Room Repository
			roomRepository = new Mock<IRepository<Room>>();
			oneRoom = new List<Room>
			{
				new Room { Id=1, Description="A" },
			};
			twoRooms = new List<Room>
			{
				new Room { Id=1, Description="A" },
				new Room { Id=2, Description="B" },
			};

			// Booking Manager
			bookingManager = new BookingManager(bookingRepository.Object, roomRepository.Object);
		}

		// MARK: Node coverage | 1 - 2 - 13
		[Fact]
		public void FindAvailableRoom_StartDateInThePast_ThrowsArgumentException() {
			// Arrange
			bookingRepository.Setup(x => x.GetAll()).Returns(emptyBookings);
			roomRepository.Setup(x => x.GetAll()).Returns(twoRooms);

			int startDateDaysFromToday = -1;
			DateTime startDate = DateTime.Today.AddDays(startDateDaysFromToday);
			DateTime endDate = startDate.AddDays(1);

			// Act
			Action act = () => bookingManager.FindAvailableRoom(startDate, endDate);

			// Assert
			Assert.Throws<ArgumentException>(act);
		}

		// MARK: Node coverage | 1 - 3/4 - 5 - 6 - 7 - 8/10 - 11 - 5 - 12 - 13
		[Fact]
		public void FindAvailableRoom_ValidDates_ExistingRoomId() {
			// Arrange
			bookingRepository.Setup(x => x.GetAll()).Returns(emptyBookings);
			roomRepository.Setup(x => x.GetAll()).Returns(oneRoom);

			int startDateDaysFromToday = 1;
			int endDateDaysFromToday = 5;
			DateTime startDate = DateTime.Today.AddDays(startDateDaysFromToday);
			DateTime endDate = DateTime.Today.AddDays(endDateDaysFromToday);

			// Act
			int roomId = bookingManager.FindAvailableRoom(startDate, endDate);

			// Assert
			Assert.Contains(roomId, oneRoom.Select(x => x.Id).ToArray());
		}

		// MARK: Edge coverage | 1 - 3/4 - 5 - 6 - 7 - 11 - 5 - 12 - 13
		[Fact]
		public void FindAvailableRoom_ValidDates_MinusOne() {
			// Arrange
			int startDateDaysFromToday = 1;
			int endDateDaysFromToday = 5;
			DateTime startDate = DateTime.Today.AddDays(startDateDaysFromToday);
			DateTime endDate = DateTime.Today.AddDays(endDateDaysFromToday);

			List<Booking> fullBookings = new List<Booking> { };
			fullBookings.Add(new Booking {
				StartDate = startDate.AddDays(-10),
				EndDate = endDate.AddDays(10),
				Room = oneRoom[0],
				IsActive = true
			});
			// foreach (Room room in oneRoom) {
			// 	fullBookings.Add(new Booking {
			// 		StartDate = startDate.AddDays(-1),
			// 		EndDate = endDate.AddDays(1),
			// 		Room = room,
			// 		IsActive = true
			// 	});
			// }

			bookingRepository.Setup(x => x.GetAll()).Returns(fullBookings);
			roomRepository.Setup(x => x.GetAll()).Returns(oneRoom);

			// Act
			int roomId = bookingManager.FindAvailableRoom(startDate, endDate);

			// Assert
			Assert.Equal(-1, roomId);
		}
	}
}