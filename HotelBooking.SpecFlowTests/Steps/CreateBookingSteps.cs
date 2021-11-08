using FluentAssertions;
using HotelBooking.Core;
using Moq;
using System;
using TechTalk.SpecFlow;

namespace HotelBooking.SpecFlowTests.Steps
{
	[Binding]
	public sealed class CreateBookingSteps
	{
        private readonly ScenarioContext _scenarioContext;
        private readonly Mock<IRepository<Booking>> _bookingRepository = new();
        private readonly Mock<IRepository<Room>> _roomRepository = new();
        private readonly IBookingManager _bookingManager;
        public static int _startOccupiedDay = 5;
        public static int _endOccupiedDay = 10;

        public CreateBookingSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            DateTime startOccupiedDate = DateTime.Now.AddDays(_startOccupiedDay);
            DateTime endOccupiedDate = DateTime.Now.AddDays(_endOccupiedDay);

            Booking[] activeBookings = new Booking[3]
            {
                new Booking { StartDate = startOccupiedDate, EndDate = endOccupiedDate, IsActive = true, RoomId = 1 },
                new Booking { StartDate = startOccupiedDate, EndDate = endOccupiedDate, IsActive = true, RoomId = 2 },
                new Booking { StartDate = startOccupiedDate, EndDate = endOccupiedDate, IsActive = true, RoomId = 3 }
            };
            Room[] rooms = new Room[3]
            {
                new Room { Id = 1, Description = "Room A" },
                new Room { Id = 2, Description = "Room B" },
                new Room { Id = 3, Description = "Room C" }
            };

            _bookingRepository.Setup(x => x.Add(It.IsAny<Booking>()));
            _bookingRepository.Setup(x => x.GetAll()).Returns(activeBookings);
            _roomRepository.Setup(x => x.GetAll()).Returns(rooms);
            _bookingManager = new BookingManager(_bookingRepository.Object, _roomRepository.Object);
        }

        [When("the method 'CreateBooking' is called")]
        public void CreateBookingCalled()
        {
            DateTime startDate = _scenarioContext.Get<DateTime>("startDate");
            DateTime endDate = _scenarioContext.Get<DateTime>("endDate");
            Booking booking = new()
            {
                StartDate = startDate,
                EndDate = endDate
            };
            bool result = _bookingManager.CreateBooking(booking);
            _scenarioContext.Add("result", result);
        }

        [Given(@"the start date is today \+ (.*) days")]
        public void GivenTheFirstDateTodayPlusDays(int days)
        {
            DateTime startDate = DateTime.Now.AddDays(days);
            _scenarioContext.Add("startDate", startDate);
        }

        [Given(@"the end date is today \+ (.*) days")]
        public void GivenTheEndDateTodayPlusDays(int days)
        {
            DateTime endDate = DateTime.Now.AddDays(days);
            _scenarioContext.Add("endDate", endDate);
        }

        [Then("the result should return (.*)")]
        public void ThenTheResultShouldBe(bool expected)
        {
            bool result = _scenarioContext.Get<bool>("result");
            result.Should().Be(expected);
        }
    }
}
