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
		private List<Booking> emptyBookings;
		private Mock<IRepository<Room>> roomRepository;
		private List<Room> twoRooms;

		public BookingManagerTests() {
			// Booking Repository
			bookingRepository = new Mock<IRepository<Booking>>();
			emptyBookings = new List<Booking> { };

			// Room Repository
			roomRepository = new Mock<IRepository<Room>>();
			twoRooms = new List<Room>
			{
				new Room { Id=1, Description="A" },
				new Room { Id=2, Description="B" },
			};

			// Booking Manager
			bookingManager = new BookingManager(bookingRepository.Object, roomRepository.Object);
		}

		[Theory]
		[JsonData("Core/Services/BookingManager/TestData/FindAvailableRoom_RoomAvailable.json")]
		public void FindAvailableRoom_ValidDates_ExistingRoomId(int startDateDaysFromToday, int endDateDaysFromToday) {
			// Arrange
			bookingRepository.Setup(x => x.GetAll()).Returns(emptyBookings);
			roomRepository.Setup(x => x.GetAll()).Returns(twoRooms);

			DateTime startDate = DateTime.Today.AddDays(startDateDaysFromToday);
			DateTime endDate = DateTime.Today.AddDays(endDateDaysFromToday);

			// Act
			int roomId = bookingManager.FindAvailableRoom(startDate, endDate);

			// Assert
			Assert.Contains(roomId, twoRooms.Select(x => x.Id).ToArray());
		}

		[Theory]
		[JsonData("Core/Services/BookingManager/TestData/FindAvailableRoom_StartDateNotInTheFuture.json")]
		public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException(int startDateDaysFromToday) {
			// Arrange
			bookingRepository.Setup(x => x.GetAll()).Returns(emptyBookings);
			roomRepository.Setup(x => x.GetAll()).Returns(twoRooms);

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
			bookingRepository.Setup(x => x.GetAll()).Returns(emptyBookings);
			roomRepository.Setup(x => x.GetAll()).Returns(twoRooms);

			DateTime endDate = DateTime.Today.AddDays(endDateDaysFromToday);
			DateTime startDate = endDate.AddDays(-1);


			// Act
			Action act = () => bookingManager.FindAvailableRoom(startDate, endDate);

			// Assert
			Assert.Throws<ArgumentException>(act);
		}

		[Theory]
		[JsonData("Core/Services/BookingManager/TestData/GetFullyOccupiedDates_ValidDates.json")]
		public void GetFullyOccupiedDates_ValidDates_EmptyArray(int startDateDaysFromToday, int endDateDaysFromToday) {
			// Arrange
			bookingRepository.Setup(x => x.GetAll()).Returns(emptyBookings);
			roomRepository.Setup(x => x.GetAll()).Returns(twoRooms);

			DateTime startDate = DateTime.Today.AddDays(startDateDaysFromToday);
			DateTime endDate = DateTime.Today.AddDays(endDateDaysFromToday);

			// Act
			List<DateTime> occupiedDates = bookingManager.GetFullyOccupiedDates(startDate, endDate);

			// Assert
			Assert.Empty(occupiedDates);
		}

		[Theory]
		[JsonData("Core/Services/BookingManager/TestData/GetFullyOccupiedDates_ValidDates.json")]
		public void GetFullyOccupiedDates_ValidDates_ValidDates(int startDateDaysFromToday, int endDateDaysFromToday) {
			// Arrange
			DateTime startDate = DateTime.Today.AddDays(startDateDaysFromToday);
			DateTime endDate = DateTime.Today.AddDays(endDateDaysFromToday);

			List<Booking> fullBookings = new List<Booking> { };
			foreach (Room room in twoRooms) {
				fullBookings.Add(new Booking {
					StartDate = startDate,
					EndDate = endDate,
					Room = room,
					IsActive = true
				});
			}

			bookingRepository.Setup(x => x.GetAll()).Returns(fullBookings);
			roomRepository.Setup(x => x.GetAll()).Returns(twoRooms);

			List<DateTime> expected = new List<DateTime> { };
			for (int i = startDateDaysFromToday; i <= endDateDaysFromToday; i++) {
				expected.Add(DateTime.Today.AddDays(i));
			}

			// Act
			List<DateTime> occupiedDates = bookingManager.GetFullyOccupiedDates(startDate, endDate);

			// Assert
			Assert.Equal(expected, occupiedDates);
		}

		[Theory]
		[JsonData("Core/Services/BookingManager/TestData/GetFullyOccupiedDates_StartDateBiggerThanEndDate.json")]
		public void GetFullyOccupiedDates_StartDateBiggerThanEndDate_ThrowsArgumentException(int startDateDaysFromToday, int endDateDaysFromToday) {
			// Arrange
			bookingRepository.Setup(x => x.GetAll()).Returns(emptyBookings);
			roomRepository.Setup(x => x.GetAll()).Returns(twoRooms);

			DateTime startDate = DateTime.Today.AddDays(startDateDaysFromToday);
			DateTime endDate = DateTime.Today.AddDays(endDateDaysFromToday);


			// Act
			Action act = () => bookingManager.GetFullyOccupiedDates(startDate, endDate);

			// Assert
			Assert.Throws<ArgumentException>(act);
		}
	}
}
