using System;
using HotelBooking.Core;
using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace HotelBooking.UnitTests {
	public class BookingManagerTests {
		private IBookingManager _bookingManager;
		private Mock<IRepository<Booking>> _bookingRepository;
		private List<Booking> _emptyBookings;
		private Mock<IRepository<Room>> _roomRepository;
		private List<Room> _twoRooms;
		private List<Room> _emptyRooms;

		public BookingManagerTests() {
			// Booking Repository
			_bookingRepository = new Mock<IRepository<Booking>>();
			_emptyBookings = new List<Booking> { };

			// Room Repository
			_roomRepository = new Mock<IRepository<Room>>();
			_twoRooms = new List<Room>
			{
				new Room { Id=1, Description="A" },
				new Room { Id=2, Description="B" },
			};
			_emptyRooms = new List<Room>
			{

			};

			// Booking Manager
			_bookingManager = new BookingManager(_bookingRepository.Object, _roomRepository.Object);
		}

		//FindAvailableRooms

		[Theory]
		[JsonData("Core/Services/BookingManager/TestData/FindAvailableRoom_StartDateInThePast.json")]
		public void FindAvailableRoom_StartDateInThePast_ThrowsArgumentException(int startDateDaysFromToday) {
			// Arrange
			_bookingRepository.Setup(x => x.GetAll()).Returns(_emptyBookings);
			_roomRepository.Setup(x => x.GetAll()).Returns(_twoRooms);

			DateTime startDate = DateTime.Today.AddDays(startDateDaysFromToday);
			DateTime endDate = startDate.AddDays(1);

			// Act
			Action act = () => _bookingManager.FindAvailableRoom(startDate, endDate);

			// Assert
			Assert.Throws<ArgumentException>(act);
		}

		[Theory]
		[JsonData("Core/Services/BookingManager/TestData/FindAvailableRoom_StartDateAfterEndDate.json")]
		public void FindAvailableRoom_StartDateLaterThanEndDate_ThrowsArgumentException(int days)
		{
			// Arrange
			_bookingRepository.Setup(x => x.GetAll()).Returns(_emptyBookings);
			_roomRepository.Setup(x => x.GetAll()).Returns(_twoRooms);

			DateTime startDate = DateTime.Today.AddDays(days + 1);
			DateTime endDate = DateTime.Today.AddDays(days);

			// Act
			Action act = () => _bookingManager.FindAvailableRoom(startDate, endDate);

			// Assert
			Assert.Throws<ArgumentException>(act);
		}

		[Theory]
		[JsonData("Core/Services/BookingManager/TestData/FindAvailableRoom_EndDateInThePast.json")]
		public void FindAvailableRoom_EndDateNotInTheFuture_ThrowsArgumentException(int endDateDaysFromToday)
		{
			// Arrange
			_bookingRepository.Setup(x => x.GetAll()).Returns(_emptyBookings);
			_roomRepository.Setup(x => x.GetAll()).Returns(_twoRooms);

			DateTime endDate = DateTime.Today.AddDays(endDateDaysFromToday);
			DateTime startDate = endDate.AddDays(-1);


			// Act
			Action act = () => _bookingManager.FindAvailableRoom(startDate, endDate);

			// Assert
			Assert.Throws<ArgumentException>(act);
		}

		[Theory]
		[JsonData("Core/Services/BookingManager/TestData/FindAvailableRoom_NoRooms.json")]
		public void FindAvailableRoo_NoRooms_ReturnsMinusOne(int startDateDaysFromToday, int endDateDaysFromToday)
		{
			// Arrange
			_bookingRepository.Setup(x => x.GetAll()).Returns(_emptyBookings);
			_roomRepository.Setup(x => x.GetAll()).Returns(_emptyRooms);

			DateTime startDate = DateTime.Today.AddDays(startDateDaysFromToday);
			DateTime endDate = DateTime.Today.AddDays(endDateDaysFromToday);

			// Act
			int roomId = _bookingManager.FindAvailableRoom(startDate, endDate);

			// Assert
			Assert.Equal(roomId, -1);
		}

		[Theory]
		[JsonData("Core/Services/BookingManager/TestData/FindAvailableRoom_RoomUnavailable.json")]
		public void FindAvailableRoo_UnavailableRoom_ReturnsMinusOne(int startDateDaysFromToday, int endDateDaysFromToday)
		{
			// Arrange
			_bookingRepository.Setup(x => x.GetAll()).Returns(_emptyBookings);
			_roomRepository.Setup(x => x.GetAll()).Returns(_emptyRooms);

			DateTime startDate = DateTime.Today.AddDays(startDateDaysFromToday);
			DateTime endDate = DateTime.Today.AddDays(endDateDaysFromToday);

			// Act
			int roomId = _bookingManager.FindAvailableRoom(startDate, endDate);

			// Assert
			Assert.Equal(roomId, -1);
		}

		[Theory]
		[JsonData("Core/Services/BookingManager/TestData/FindAvailableRoom_RoomAvailable.json")]
		public void FindAvailableRoom_ValidDates_ExistingRoomId(int startDateDaysFromToday, int endDateDaysFromToday)
		{
			// Arrange
			_bookingRepository.Setup(x => x.GetAll()).Returns(_emptyBookings);
			_roomRepository.Setup(x => x.GetAll()).Returns(_twoRooms);

			DateTime startDate = DateTime.Today.AddDays(startDateDaysFromToday);
			DateTime endDate = DateTime.Today.AddDays(endDateDaysFromToday);

			// Act
			int roomId = _bookingManager.FindAvailableRoom(startDate, endDate);

			// Assert
			Assert.Contains(roomId, _twoRooms.Select(x => x.Id).ToArray());
		}

		//GetFullyOccupiedDates


		[Theory]
		[JsonData("Core/Services/BookingManager/TestData/GetFullyOccupiedDates_StartDateBiggerThanEndDate.json")]
		public void GetFullyOccupiedDates_StartDateBiggerThanEndDate_ThrowsArgumentException(int startDateDaysFromToday, int endDateDaysFromToday)
		{
			// Arrange
			_bookingRepository.Setup(x => x.GetAll()).Returns(_emptyBookings);
			_roomRepository.Setup(x => x.GetAll()).Returns(_twoRooms);

			DateTime startDate = DateTime.Today.AddDays(startDateDaysFromToday);
			DateTime endDate = DateTime.Today.AddDays(endDateDaysFromToday);


			// Act
			Action act = () => _bookingManager.GetFullyOccupiedDates(startDate, endDate);

			// Assert
			Assert.Throws<ArgumentException>(act);
		}

		[Theory]
		[JsonData("Core/Services/BookingManager/TestData/GetFullyOccupiedDates_ValidDates.json")]
		public void GetFullyOccupiedDates_ValidDates_EmptyArray(int startDateDaysFromToday, int endDateDaysFromToday) {
			// Arrange
			_bookingRepository.Setup(x => x.GetAll()).Returns(_emptyBookings);
			_roomRepository.Setup(x => x.GetAll()).Returns(_twoRooms);

			DateTime startDate = DateTime.Today.AddDays(startDateDaysFromToday);
			DateTime endDate = DateTime.Today.AddDays(endDateDaysFromToday);

			// Act
			List<DateTime> occupiedDates = _bookingManager.GetFullyOccupiedDates(startDate, endDate);

			// Assert
			Assert.Empty(occupiedDates);
		}

		[Theory]
		[JsonData("Core/Services/BookingManager/TestData/GetFullyOccupiedDates_ValidDates.json")]
		public void GetFullyOccupiedDates_BookingsLessThanRooms_EmptyArray(int startDateDaysFromToday, int endDateDaysFromToday)
		{
			// Arrange
			_bookingRepository.Setup(x => x.GetAll()).Returns(_emptyBookings);
			_roomRepository.Setup(x => x.GetAll()).Returns(_twoRooms);

			DateTime startDate = DateTime.Today.AddDays(startDateDaysFromToday);
			DateTime endDate = DateTime.Today.AddDays(endDateDaysFromToday);

			// Act
			List<DateTime> occupiedDates = _bookingManager.GetFullyOccupiedDates(startDate, endDate);

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
			foreach (Room room in _twoRooms) {
				fullBookings.Add(new Booking {
					StartDate = startDate,
					EndDate = endDate,
					Room = room,
					IsActive = true
				});
			}

			_bookingRepository.Setup(x => x.GetAll()).Returns(fullBookings);
			_roomRepository.Setup(x => x.GetAll()).Returns(_twoRooms);

			List<DateTime> expected = new List<DateTime> { };
			for (int i = startDateDaysFromToday; i <= endDateDaysFromToday; i++) {
				expected.Add(DateTime.Today.AddDays(i));
			}

			// Act
			List<DateTime> occupiedDates = _bookingManager.GetFullyOccupiedDates(startDate, endDate);

			// Assert
			Assert.Equal(expected, occupiedDates);
		}
	}
}
