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
		private List<Room> emptyRooms;
		private List<Room> oneRoom;
		private List<Room> twoRooms;

		public BookingManagerPathTests() {
			// Booking Repository
			bookingRepository = new Mock<IRepository<Booking>>();
			emptyBookings = new List<Booking> { };

			// Room Repository
			roomRepository = new Mock<IRepository<Room>>();
			emptyRooms = new List<Room> { };
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

		// FindAvailableRoom

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
		public void FindAvailableRoom_ValidDates_ExistingRoomId()
		{
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
		public void FindAvailableRoom_ValidDates_MinusOne()
		{
			// Arrange
			int startDateDaysFromToday = 1;
			int endDateDaysFromToday = 5;
			DateTime startDate = DateTime.Today.AddDays(startDateDaysFromToday);
			DateTime endDate = DateTime.Today.AddDays(endDateDaysFromToday);

			List<Booking> fullBookings = new List<Booking> { };
			foreach (Room room in oneRoom)
			{
				fullBookings.Add(new Booking
				{
					StartDate = startDate,
					EndDate = endDate,
					Room = room,
					RoomId = room.Id,
					IsActive = true
				});
			}

			bookingRepository.Setup(x => x.GetAll()).Returns(fullBookings);
			roomRepository.Setup(x => x.GetAll()).Returns(oneRoom);

			// Act
			int roomId = bookingManager.FindAvailableRoom(startDate, endDate);

			// Assert
			Assert.Equal(-1, roomId);
		}


		// MARK: Loop coverage  | 1 - 3/4 - 5 - 12 - 13
		[Fact]
		public void FindAvailableRoom_NoRooms_MinusOne()
		{
			// Arrange
			bookingRepository.Setup(x => x.GetAll()).Returns(emptyBookings);
			roomRepository.Setup(x => x.GetAll()).Returns(emptyRooms);

			int startDateDaysFromToday = 1;
			int endDateDaysFromToday = 5;
			DateTime startDate = DateTime.Today.AddDays(startDateDaysFromToday);
			DateTime endDate = DateTime.Today.AddDays(endDateDaysFromToday);

			// Act
			int roomId = bookingManager.FindAvailableRoom(startDate, endDate);

			// Assert
			Assert.Equal(-1, roomId);
		}

		// MARK: Multiple condition coverage | 1 - 2 - 13
		[Fact]
		public void FindAvailableRoom_StartDateLaterThanEndDate_ThrowsArgumentException()
		{
			// Arrange
			bookingRepository.Setup(x => x.GetAll()).Returns(emptyBookings);
			roomRepository.Setup(x => x.GetAll()).Returns(twoRooms);

			int endDateDaysFromToday = -1;
			DateTime startDate = DateTime.Today.AddDays(1);
			DateTime endDate = startDate.AddDays(endDateDaysFromToday);

			// Act
			Action act = () => bookingManager.FindAvailableRoom(startDate, endDate);

			// Assert
			Assert.Throws<ArgumentException>(act);
		}

		// GetFullyOccupiedDates

		// MARK: Node coverage | 1 - 2 - 18
		[Fact]
		public void GetFullyOccupiedDates_StartDateBiggerThanEndDate_ThrowsArgumentException() {
			// Arrange
			int startDateDaysFromToday = 5;
			int endDateDaysFromToday = 1;
			bookingRepository.Setup(x => x.GetAll()).Returns(emptyBookings);
			roomRepository.Setup(x => x.GetAll()).Returns(twoRooms);

			DateTime startDate = DateTime.Today.AddDays(startDateDaysFromToday);
			DateTime endDate = DateTime.Today.AddDays(endDateDaysFromToday);


			// Act
			Action act = () => bookingManager.GetFullyOccupiedDates(startDate, endDate);

			// Assert
			Assert.Throws<ArgumentException>(act);
		}

		// MARK: Node coverage | 1 - 3/7 - 8 - 9 - 10/12 - 13 - 14 - 15	- 9 - 16 - 17 - 18
		[Fact]
		public void GetFullyOccupiedDates_ValidDates_ValidDates() {
			// Arrange
			int startDateDaysFromToday = 1;
			int endDateDaysFromToday = 1;
			DateTime startDate = DateTime.Today.AddDays(startDateDaysFromToday);
			DateTime endDate = DateTime.Today.AddDays(endDateDaysFromToday);

			List<Booking> fullBookings = new List<Booking> { };
			foreach (Room room in twoRooms) {
				fullBookings.Add(new Booking {
					StartDate = startDate,
					EndDate = endDate,
					Room = room,
					RoomId = room.Id,
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

		// MARK: Edge coverage | 1 - 3/7 - 8 - 16 - 17 - 18
		[Fact]
		public void GetFullyOccupiedDates_ValidDates_EmptyArray() {
			// Arrange
			int startDateDaysFromToday = 1;
			int endDateDaysFromToday = 1;
			bookingRepository.Setup(x => x.GetAll()).Returns(emptyBookings);
			roomRepository.Setup(x => x.GetAll()).Returns(twoRooms);

			DateTime startDate = DateTime.Today.AddDays(startDateDaysFromToday);
			DateTime endDate = DateTime.Today.AddDays(endDateDaysFromToday);

			// Act
			List<DateTime> occupiedDates = bookingManager.GetFullyOccupiedDates(startDate, endDate);

			// Assert
			Assert.Empty(occupiedDates);
		}

		// MARK: Edge coverage | 1 - 3/7 - 8 - 9 - 10/12 - 13 - 15 - 9 - 16 - 17 - 18
		[Fact]
		public void GetFullyOccupiedDates_ValidDates_NumberBookingsLessThanNumberRooms() {
			// Arrange
			int startDateDaysFromToday = 1;
			int endDateDaysFromToday = 1;
			DateTime startDate = DateTime.Today.AddDays(startDateDaysFromToday);
			DateTime endDate = DateTime.Today.AddDays(endDateDaysFromToday);

			List<Booking> bookings = new List<Booking> { };
			bookings.Add(new Booking {
				StartDate = startDate,
				EndDate = endDate,
				Room = twoRooms[0],
				RoomId = twoRooms[0].Id,
				IsActive = true
			});

			bookingRepository.Setup(x => x.GetAll()).Returns(bookings);
			roomRepository.Setup(x => x.GetAll()).Returns(twoRooms);

			// Act
			List<DateTime> occupiedDates = bookingManager.GetFullyOccupiedDates(startDate, endDate);

			// Assert
			Assert.Empty(occupiedDates);
		}
	}
}
