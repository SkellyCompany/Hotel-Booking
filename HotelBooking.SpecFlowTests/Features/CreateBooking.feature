Feature: CreateBooking
	A hotel room can be booked for a period (start date – end date) in the future
	provided that it is not already booked for one or more days during the desired period

@mytag
Scenario: Start date and end date before occupied range
	Given the start date is 2021-11-04
	And the end date is 2021-11-05
	When the method 'CreateBooking' is called
	Then the result should return true
