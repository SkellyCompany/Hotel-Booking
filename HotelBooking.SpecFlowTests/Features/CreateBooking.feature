Feature: CreateBooking


@mytag
Scenario: The start date and the end date are before the occupied range
	Given the start date is today + 1 days
	And the end date is today + 2 days
	When the method 'CreateBooking' is called
	Then the result should return true

@mytag
Scenario: The start date and the end date are after the occupied range
	Given the start date is today + 11 days
	And the end date is today + 12 days
	When the method 'CreateBooking' is called
	Then the result should return true

@mytag
Scenario: The start date is before and the end date is after the occupied range
	Given the start date is today + 1 days
	And the end date is today + 12 days
	When the method 'CreateBooking' is called
	Then the result should return false
